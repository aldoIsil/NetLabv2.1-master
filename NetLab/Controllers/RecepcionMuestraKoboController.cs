using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using BusinessLayer.DatoClinico;
using ClosedXML.Excel;
using Model;
using NetLab.Extensions.ActionResults;
using NetLab.Models;
using NetLab.Models.Shared;
using Model.Temp;
using Newtonsoft.Json;

namespace NetLab.Controllers
{
    public class RecepcionMuestraKoboController : ParentController
    {
        // GET: RecepcionMuestraKobo
        public ActionResult Index()
        {
            List<SelectListItem> seguroList = new List<SelectListItem>();
            ListaBl listaBl = new ListaBl();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoSeguroSalud).ListaDetalle)
                seguroList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.seguroList = seguroList;

            List<SelectListItem> establecimientoList = new List<SelectListItem>();
            establecimientoList.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.establecimientoList = establecimientoList;

            var tipoDocumentoList = new List<SelectListItem>();
            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });
            ViewBag.tipoDocumentoList = tipoDocumentoList;
            CargaUbigeoEstablecimiento();

            new TempOrdenBl().DeleTempOrden(Logueado.idUsuario);

            ViewBag.idOrdenPadreExportado = (string)Guid.NewGuid().ToString();
            return View();
        }
        public void CargaUbigeoEstablecimiento()
        {
            Laboratorio LaboratorioUbigeo = new Laboratorio();
            LaboratorioUbigeo.UbigeoLaboratorio = new Model.Ubigeo();
            LaboratorioUbigeo.UbigeoLaboratorio.Id = "      ";
            ViewBag.LabUbigeo = LaboratorioUbigeo;
        }
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        public ActionResult VerSolicitante()
        {
            var listaBl = new ListaBl();
            var profesiones = listaBl.GetListaByOpcion(OpcionLista.Profesion);
            var solicitantevm = new SolicitanteViewModels
            {
                Solicitante = new OrdenSolicitante
                {
                    idUbigeoReniec = 0,
                    UbigeoActual = new Model.Ubigeo() { Id = "      " },
                    UbigeoReniec = new Model.Ubigeo() { Id = "      " }
                },
                Profesion = new ListaDetalleViewModels { Data = profesiones.ListaDetalle }
            };

            return PartialView("_FormPopupSolicitante", solicitantevm);
        }
        public ActionResult AddVariables(string idOrden)
        {
            int idEnfermedad = int.Parse(GetSetting<string>("EnfVirusRespiratorio")); //1005680;
            string idExamen = GetSetting<string>("Covid19"); //Guid.Parse("1B2BEC28-772C-49DF-BCC2-85E0C5CCA667");
            string idExamenAgrupado = GetSetting<string>("idExamenAgrupado");
            string idTipoMuestra = GetSetting<string>("idTipoMuestra");
            //cambioj
            OrdenBl ordenBl = new OrdenBl();
            Orden orden = ordenBl.GetOrdenById(Guid.Parse(idOrden));
            var enfermedad = new Enfermedad();

            if (!(orden.enfermedadList != null && orden.enfermedadList.Any()))
            {
                var datoClinicoBl = new DatoClinicoBl();
                var categoriaDatoList = datoClinicoBl.GetCategoriaByEnfermedad(idEnfermedad, 1, idExamen);

                if (enfermedad != null)
                {
                    enfermedad.categoriaDatoList = categoriaDatoList;
                }
            }
            else
            {
                enfermedad = orden.enfermedadList.FirstOrDefault();
            }

            var Examenbl = new ExamenBl();
            var TipoMuestraList = new List<SelectListItem>();
            foreach (var itemDetalle in Examenbl.GetExamenAgrupado(int.Parse(idExamenAgrupado)))
                TipoMuestraList.Add(new SelectListItem
                {
                    Text = itemDetalle.descripcion,
                    Value = itemDetalle.idTipoMuestra.ToString()
                });
            ViewBag.TipoMuestra = TipoMuestraList;
            int val = 0;
            if (orden.ordenMuestraList != null)
            {
                if (orden.ordenMuestraList.Count() > 0)
                {
                    ViewBag.tipoMuestraSel = orden.ordenMuestraList.First().TipoMuestra.idTipoMuestra.ToString();
                    ViewBag.FechaObtencion = orden.ordenMuestraList.First().fechaColeccion.ToShortDateString();
                    val = 1;
                }
            }

            if (val == 0)
            {
                ViewBag.tipoMuestraSel = idTipoMuestra;
                ViewBag.FechaObtencion = "";
            }

            var paciente = new PacienteBl().GetPacienteByDocumento(orden.Paciente.NroDocumento);
            ViewBag.Paciente = paciente.ApellidoPaterno + " " + paciente.ApellidoMaterno + ", " + paciente.Nombres;
            ViewBag.idPaciente = paciente.IdPaciente.ToString();
            ViewBag.nroCelularPaciente = paciente.Celular1;
            ViewBag.idOrden = idOrden;

            if (orden.establecimiento != null)
            {
                ViewBag.idEstablecimientoOrigenEdit = orden.establecimiento.IdEstablecimiento == 0 ? "" : orden.establecimiento.IdEstablecimiento.ToString();
                ViewBag.nombreEstablecimientoOrigenEdit = string.IsNullOrEmpty(orden.establecimiento.Nombre) ? "" : orden.establecimiento.Nombre;
            }

            return PartialView("_AddVariables", enfermedad);
        }
        public string PreRegistroAntiguo(string TipoDocumento, string nroDocumento, string EstablecimientoOrigen, string EstablecimientoEnvio, string FechaObtencion, string solicitante, string fechaIngresoINS)
        {
            //Cargar objetos
            int idEnfermedad = int.Parse(GetSetting<string>("EnfVirusRespiratorio")); //1005680;
            string idExamen = GetSetting<string>("Covid19"); //Guid.Parse("1B2BEC28-772C-49DF-BCC2-85E0C5CCA667");
            string idExamenAgrupado = GetSetting<string>("idExamenAgrupado");
            TempModel.TempOrden tempOrden = null;
            var pacienteBl = new PacienteBl();
            var paciente = new Paciente();
            var res = Guid.Empty;
            paciente.TipoDocumento = int.Parse(TipoDocumento);
            paciente.NroDocumento = nroDocumento;
            List<Paciente> pacienteList = pacienteBl.getPacientes2(1, 10, paciente.TipoDocumento.Value, paciente.NroDocumento);
            if (pacienteList.Any())
            {
                paciente = pacienteList.FirstOrDefault();
                tempOrden = new TempModel.TempOrden
                {
                    IdProyecto = 1,
                    IdPaciente = paciente.IdPaciente,
                    estatus = 0,
                    estado = 1,
                    observacion = "REGISTRO RAPIDO COVID19",
                    idUsuario = Logueado.idUsuario//cambioj
                };
                //Datos Clinicos
                List<OrdenDatoClinico> ordenDatoClinicoList = new List<OrdenDatoClinico>();
                var datoClinicoBl = new DatoClinicoBl();
                var categoriaDatoList = datoClinicoBl.GetCategoriaByEnfermedad(idEnfermedad, 1, idExamen);
                var oEnfermedad = new Enfermedad();
                oEnfermedad.idEnfermedad = idEnfermedad;
                oEnfermedad.categoriaDatoList = categoriaDatoList;
                List<Enfermedad> enfermedadListAgregados = new List<Enfermedad>();
                enfermedadListAgregados.Add(oEnfermedad);
                {
                    var datosClinicos =
                           enfermedadListAgregados.SelectMany(e => e.categoriaDatoList)
                               .SelectMany(c => c.OrdenDatoClinicoList);
                    foreach (var ordenDatoClinico in datosClinicos)
                    {
                        var id = ordenDatoClinico.Enfermedad.idEnfermedad.ToString() +
                                 ordenDatoClinico.Dato.IdDato.ToString();

                        OrdenDatoClinico ordenDatoClinicoFormulario =
                            ordenDatoClinicoList.FirstOrDefault(x => x.Dato.IdDato == ordenDatoClinico.Dato.IdDato);

                        //Si el dato clinico existe previamente en el formulario entonces se copia los valores del existente
                        if (ordenDatoClinicoFormulario != null)
                        {
                            ordenDatoClinico.noPrecisa = ordenDatoClinicoFormulario.noPrecisa;
                            ordenDatoClinico.ValorString = ordenDatoClinicoFormulario.ValorString;
                        }
                        else
                        {
                            //Si el dato clinico no existe se crea con los valores obtenidos
                            ordenDatoClinicoFormulario = new OrdenDatoClinico();
                            Dato datoClinico = new Dato();
                            datoClinico.IdDato = ordenDatoClinico.Dato.IdDato;
                            ordenDatoClinicoFormulario.Dato = datoClinico;
                            ordenDatoClinicoList.Add(ordenDatoClinicoFormulario);


                            var formValue = Request.Form["valueDato" + id];
                            string checknoprecisa = Request.Form["checkNoPrecisa" + id];
                            var checkNoPrecisaBoolean = !string.IsNullOrEmpty(checknoprecisa) && (checknoprecisa.ToLower() == "on" || checknoprecisa.ToLower() == "true");

                            if ((int)Enums.TipoCampo.CHECKBOX == ordenDatoClinico.Dato.IdTipo
                                || (int)Enums.TipoCampo.COMBO == ordenDatoClinico.Dato.IdTipo)
                            {
                                ordenDatoClinico.noPrecisa = formValue == null || formValue.Equals("0");
                                ordenDatoClinico.ValorString = formValue == null || formValue.Equals("0")
                                    ? ""
                                    : formValue;
                            }
                            else
                            {
                                ordenDatoClinico.noPrecisa = checkNoPrecisaBoolean;
                                ordenDatoClinico.ValorString = !checkNoPrecisaBoolean ? formValue : string.Empty;
                            }
                            ordenDatoClinicoFormulario.noPrecisa = ordenDatoClinico.noPrecisa;
                            ordenDatoClinicoFormulario.ValorString = ordenDatoClinico.ValorString;
                        }
                    }
                }
                tempOrden.enfermedadList = enfermedadListAgregados;
                //Insert orden
                var tempOrdenBl = new TempOrdenBl();
                res = tempOrdenBl.InsertTempOrden(tempOrden);
                //Insert TempDatoClinico
                tempOrdenBl.InsertTempOrdenDatoClinico(tempOrden);
            }
            return res.ToString();
        }
        public string PreSaveOrden()
        {
            var orden = new Orden();
            var ordenBl = new OrdenBl();
            int idEnfermedad = int.Parse(GetSetting<string>("EnfVirusRespiratorio"));

            string fechaObtencion = Request.Form["fechaObtencion"];
            string idExamen = GetSetting<string>("Covid19");
            string idTipoMuestra = Request.Form["TipoMuestraCovid"];
            string nroCelularPaciente = Request.Form["nroCelularPaciente"];
            Guid idPaciente = Guid.Parse(Request.Form["GuidIdPaciente"].ToString());
            orden.idOrden = Guid.Parse(Request.Form["idOrden"].ToString());


            List<OrdenDatoClinico> ordenDatoClinicoList = new List<OrdenDatoClinico>();
            var datoClinicoBl = new DatoClinicoBl();
            var categoriaDatoList = datoClinicoBl.GetCategoriaByEnfermedad(idEnfermedad, 1, idExamen);
            var oEnfermedad = new Enfermedad();
            oEnfermedad.idEnfermedad = idEnfermedad;
            oEnfermedad.categoriaDatoList = categoriaDatoList;
            List<Enfermedad> enfermedadListAgregados = new List<Enfermedad>();
            enfermedadListAgregados.Add(oEnfermedad);
            {
                var datosClinicos =
                       enfermedadListAgregados.SelectMany(e => e.categoriaDatoList)
                           .SelectMany(c => c.OrdenDatoClinicoList);
                foreach (var ordenDatoClinico in datosClinicos)
                {
                    var id = ordenDatoClinico.Enfermedad.idEnfermedad.ToString() +
                             ordenDatoClinico.Dato.IdDato.ToString();

                    OrdenDatoClinico ordenDatoClinicoFormulario =
                        ordenDatoClinicoList.FirstOrDefault(x => x.Dato.IdDato == ordenDatoClinico.Dato.IdDato);

                    //Si el dato clinico existe previamente en el formulario entonces se copia los valores del existente
                    if (ordenDatoClinicoFormulario != null)
                    {
                        ordenDatoClinico.noPrecisa = ordenDatoClinicoFormulario.noPrecisa;
                        ordenDatoClinico.ValorString = ordenDatoClinicoFormulario.ValorString;
                    }
                    else
                    {
                        //Si el dato clinico no existe se crea con los valores obtenidos
                        ordenDatoClinicoFormulario = new OrdenDatoClinico();
                        Dato datoClinico = new Dato();
                        datoClinico.IdDato = ordenDatoClinico.Dato.IdDato;
                        ordenDatoClinicoFormulario.Dato = datoClinico;
                        ordenDatoClinicoList.Add(ordenDatoClinicoFormulario);


                        var formValue = Request.Form["valueDato" + id];
                        string checknoprecisa = Request.Form["checkNoPrecisa" + id];
                        var checkNoPrecisaBoolean = !string.IsNullOrEmpty(checknoprecisa) && (checknoprecisa.ToLower() == "on" || checknoprecisa.ToLower() == "true");

                        if ((int)Enums.TipoCampo.CHECKBOX == ordenDatoClinico.Dato.IdTipo
                            || (int)Enums.TipoCampo.COMBO == ordenDatoClinico.Dato.IdTipo)
                        {
                            ordenDatoClinico.noPrecisa = formValue == null || formValue.Equals("0");
                            ordenDatoClinico.ValorString = formValue == null || formValue.Equals("0")
                                ? ""
                                : formValue;
                        }
                        else
                        {
                            ordenDatoClinico.noPrecisa = checkNoPrecisaBoolean;
                            ordenDatoClinico.ValorString = !checkNoPrecisaBoolean ? formValue : string.Empty;
                        }
                        ordenDatoClinicoFormulario.noPrecisa = ordenDatoClinico.noPrecisa;
                        ordenDatoClinicoFormulario.ValorString = ordenDatoClinico.ValorString;
                    }
                }
            }
            orden.enfermedadList = enfermedadListAgregados;
            orden.IdUsuarioRegistro = Logueado.idUsuario;
            orden.ordenMuestraList = new List<OrdenMuestra>();
            orden.ordenExamenList = new List<OrdenExamen>();
            var oOrdenMuestra = new OrdenMuestra();
            var oOrdenExamen = new OrdenExamen();
            var oMuestraCodificacion = new MuestraCodificacion();
            var codigoOrdenGenerado = string.Empty;
            int EstablecimientoOrigen = string.IsNullOrEmpty(Request.Form["EESSOrigen"].ToString()) ? 0 : int.Parse(Request.Form["EESSOrigen"]);
            int hddDatoEESSOrigenEdit = string.IsNullOrEmpty(Request.Form["hddDatoEESSOrigenEdit"].ToString()) ? 0 : int.Parse(Request.Form["hddDatoEESSOrigenEdit"]);
            //OrdenMuestra   
            oOrdenMuestra.TipoMuestra = new TipoMuestra { idTipoMuestra = int.Parse(idTipoMuestra) };
            oOrdenMuestra.fechaColeccion = DateTime.Parse(fechaObtencion);
            oOrdenMuestra.horaColeccion = DateTime.Now;
            oOrdenMuestra.idProyecto = 1;
            orden.idEstablecimiento = EstablecimientoOrigen;
            orden.editarEstablecimiento = "N";
            if (EstablecimientoOrigen != hddDatoEESSOrigenEdit)
            {
                //Orden
                orden.editarEstablecimiento = "S";
                orden.codigoOrden = ordenBl.GenerarCodigoOrden(hddDatoEESSOrigenEdit);
                orden.idEstablecimiento = hddDatoEESSOrigenEdit;
                //OrdenMuestra       
                oMuestraCodificacion = new MuestraBl().GeneraCodigosMuestraKobo(hddDatoEESSOrigenEdit, Logueado.idUsuario, 1);
                oOrdenMuestra.MuestraCodificacion = oMuestraCodificacion;
            }
            orden.ordenMuestraList.Add(oOrdenMuestra);

            orden.Paciente = new Paciente { IdPaciente = idPaciente, Celular1 = nroCelularPaciente };

            var Res = "Registro Exitoso.";
            try
            {
                ordenBl.UpdateOrdenCovid(orden);
            }
            catch (Exception ex)
            {
                Res = ex.Message;
            }
            return Res;
        }
        public void DeleteDatosPaciente(string idOrden)
        {
            //new TempOrdenBl().DeleteOrden(Guid.Parse(idOrden), Logueado.idUsuario);
        }
        public string PreRegistro(string TipoDocumento, string nroDocumento, string EstablecimientoOrigen, string EstablecimientoEnvio, string FechaObtencion, string solicitante, string fechaIngresoINS, string nroOficio, string idOrdenPadreExportado)
        {
            string Response = "";
            try
            {
                //Cargar objetos
                var orden = GenerarOrden(TipoDocumento, nroDocumento, EstablecimientoOrigen, EstablecimientoEnvio, FechaObtencion, solicitante, fechaIngresoINS, nroOficio, idOrdenPadreExportado);
                if (orden != null)
                {
                    Response = orden.Paciente.IdPaciente + "|" + orden.idOrden + "|" + orden.observacion;
                }
            }
            catch (Exception ex)
            {
                Response = ex.Message;
            }
            return Response;
        }
        public Orden GenerarOrden(string TipoDocumento, string nroDocumento, string EstablecimientoOrigen, string EstablecimientoEnvio, string FechaObtencion, string solicitante, string fechaIngresoINS, string nroOficio, string idOrdenPadreExportado)
        {
            var codigoOrdenGenerado = string.Empty;
            using (var ordenBl = new OrdenBl())
            {
                //Generar Codigo de Orden
                codigoOrdenGenerado = ordenBl.GenerarCodigoOrden(int.Parse(EstablecimientoOrigen));
            }


            var datoPaciente = "";
            var idEstablecimientoDestino = EstablecimientoEnvio;
            var idEstablecimientoOrigen = EstablecimientoOrigen;
            var fechaIngresoRom = fechaIngresoINS;
            var oOrden = new OrdenBl();
            var oTempOrden = new TempOrdenBl();
            var orden = new Orden();
            var pacienteBl = new PacienteBl();
            var examenBl = new ExamenBl();
            var establecimientoBl = new EstablecimientoBl();

            // Variables produccion
            int idEnfermedad = int.Parse(GetSetting<string>("EnfVirusRespiratorio")); //1005680;
            Guid idExamen = Guid.Parse(GetSetting<string>("Covid19"));
            var idTipoMuestra = int.Parse(GetSetting<string>("idTipoMuestra"));
            orden.nroOficio = nroOficio;
            orden.codigoOrden = codigoOrdenGenerado;
            {
                datoPaciente = nroDocumento;
                List<Paciente> pacienteList = pacienteBl.getPacientes2(1, 10, int.Parse(TipoDocumento), datoPaciente);
                if (pacienteList.Any())
                {
                    bool continuar = true;
                    var paciente = pacienteList.First();

                    var enfermedadListAgregados = new List<Enfermedad>();
                    var ordenExamenListAgregados = new List<OrdenExamen>();
                    var ordenMaterialListAgregados = new List<OrdenMaterial>();
                    var ordenMuestraListAgregados = new List<OrdenMuestra>();
                    var Establecimientobl = new EstablecimientoBl();
                    var oEstablecimiento = new Establecimiento();
                    if (string.IsNullOrEmpty(idEstablecimientoOrigen))
                    {
                        idEstablecimientoOrigen = "991";
                    }
                    oEstablecimiento = Establecimientobl.GetEstablecimientosById(int.Parse(idEstablecimientoOrigen));
                    orden.idEstablecimiento = oEstablecimiento.IdEstablecimiento;
                    orden.EstablecimientoCodigoUnico = oEstablecimiento.CodigoUnico;
                    orden.idEstablecimientoEnvio = int.Parse(EstablecimientoEnvio);
                    orden.IdUsuarioRegistro = Logueado.idUsuario;
                    orden.IdLaboratorioDestino = 995;
                    orden.Paciente = new PacienteBl().getPacienteById(paciente.IdPaciente);
                    orden.solicitante = solicitante;
                    orden.fechaIngresoINS = DateTime.Parse(fechaIngresoRom);
                    var laboratorio = new Laboratorio() { IdLaboratorio = 991, CodigoUnico = "10000R16" };
                    orden.estatus = 1;
                    orden.IdUsuarioRegistro = Logueado.idUsuario;
                    orden.fechaSolicitud = DateTime.Parse(DateTime.Now.ToShortDateString());
                    var idOrdenPadreExportado2 = Guid.NewGuid();
                    
                    string nuevoCodigoVerificador = string.Empty;
                    if (string.IsNullOrWhiteSpace(idOrdenPadreExportado) || idOrdenPadreExportado == Guid.Empty.ToString())
                    {
                        nuevoCodigoVerificador= string.Format("{0}---{1}", Logueado.idUsuario, idOrdenPadreExportado2.ToString());
                        ViewBag.idOrdenPadreExportado = nuevoCodigoVerificador;
                        orden.observacion = nuevoCodigoVerificador;
                    }
                    else
                    {
                        var arr = idOrdenPadreExportado.Split(new string[] { "---" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr[0] == Logueado.idUsuario.ToString())
                        {
                            ViewBag.idOrdenPadreExportado = idOrdenPadreExportado;
                            orden.observacion = idOrdenPadreExportado;
                        }
                        else
                        {
                            nuevoCodigoVerificador = string.Format("{0}---{1}", Logueado.idUsuario, Guid.NewGuid().ToString());
                            ViewBag.idOrdenPadreExportado = nuevoCodigoVerificador;
                            orden.observacion = nuevoCodigoVerificador;
                        }
                    }
                    orden.Proyecto = new Proyecto
                    {
                        IdProyecto = 1
                    };

                    if (continuar)
                    {
                        var examenList = new Examen();
                        //var tipoMuestraList = new List<TipoMuestra>();
                        var materialList = new List<Material>();
                        //
                        materialList = new MaterialBl().GetMaterialesByIdTipoMuestra(idTipoMuestra).ToList();
                        var LstoExamen = new List<OrdenExamen>();
                        orden.ordenMuestraList = new List<OrdenMuestra>();
                        orden.ordenMuestraList.Add(new OrdenMuestra
                        {
                            idTipoMuestra = idTipoMuestra,
                            TipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra },
                            fechaColeccion = DateTime.Parse(FechaObtencion),//
                            horaColeccion = DateTime.Now,
                            idProyecto = 1,
                            MuestraCodificacion = new MuestraCodificacion()
                        });

                        var ordexmen = new OrdenExamen
                        {
                            idEnfermedad = idEnfermedad,
                            Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                            idExamen = idExamen,
                            Examen = new Examen { idExamen = idExamen },
                            IdTipoMuestra = idTipoMuestra,
                            ordenMuestraList = orden.ordenMuestraList
                        };
                        LstoExamen.Add(ordexmen);
                        orden.ordenExamenList = LstoExamen;

                        orden.ordenMaterialList = new List<OrdenMaterial>();

                        //ORDENMATERIAL
                        var materiales = new MaterialBl().GetMaterialesByIdTipoMuestra(idTipoMuestra);
                        var material = materiales.FirstOrDefault();
                        if (material != null)
                        {
                            orden.ordenMaterialList.Add(new OrdenMaterial
                            {
                                cantidad = 1,
                                idMaterial = material.idMaterial,
                                Material = new Material { idMaterial = material.idMaterial, TipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra } },
                                fechaEnvio = DateTime.Today,
                                horaEnvio = DateTime.Now,
                                volumenMuestraColectada = material != null ? material.volumen : 1,
                                OrdenExamen = ordexmen,
                                Laboratorio = laboratorio
                            });
                        }
                        List<OrdenDatoClinico> ordenDatoClinicoList = new List<OrdenDatoClinico>();
                        var datoClinicoBl = new DatoClinicoBl();
                        var categoriaDatoList = datoClinicoBl.GetCategoriaByEnfermedad(idEnfermedad, 1, idExamen.ToString());
                        var oEnfermedad = new Enfermedad();
                        oEnfermedad.idEnfermedad = idEnfermedad;
                        oEnfermedad.categoriaDatoList = categoriaDatoList;
                        enfermedadListAgregados.Add(oEnfermedad);
                        {
                            var datosClinicos =
                                   enfermedadListAgregados.SelectMany(e => e.categoriaDatoList)
                                       .SelectMany(c => c.OrdenDatoClinicoList);
                            foreach (var ordenDatoClinico in datosClinicos)
                            {
                                var id = ordenDatoClinico.Enfermedad.idEnfermedad.ToString() +
                                         ordenDatoClinico.Dato.IdDato.ToString();

                                OrdenDatoClinico ordenDatoClinicoFormulario =
                                    ordenDatoClinicoList.FirstOrDefault(x => x.Dato.IdDato == ordenDatoClinico.Dato.IdDato);

                                //Si el dato clinico existe previamente en el formulario entonces se copia los valores del existente
                                if (ordenDatoClinicoFormulario != null)
                                {
                                    ordenDatoClinico.noPrecisa = ordenDatoClinicoFormulario.noPrecisa;
                                    ordenDatoClinico.ValorString = ordenDatoClinicoFormulario.ValorString;
                                }
                                else
                                {
                                    //Si el dato clinico no existe se crea con los valores obtenidos
                                    ordenDatoClinicoFormulario = new OrdenDatoClinico();
                                    Dato datoClinico = new Dato();
                                    datoClinico.IdDato = ordenDatoClinico.Dato.IdDato;
                                    ordenDatoClinicoFormulario.Dato = datoClinico;
                                    ordenDatoClinicoList.Add(ordenDatoClinicoFormulario);


                                    var formValue = Request.Form["valueDato" + id];
                                    string checknoprecisa = Request.Form["checkNoPrecisa" + id];
                                    var checkNoPrecisaBoolean = !string.IsNullOrEmpty(checknoprecisa) && (checknoprecisa.ToLower() == "on" || checknoprecisa.ToLower() == "true");

                                    if ((int)Enums.TipoCampo.CHECKBOX == ordenDatoClinico.Dato.IdTipo
                                        || (int)Enums.TipoCampo.COMBO == ordenDatoClinico.Dato.IdTipo)
                                    {
                                        ordenDatoClinico.noPrecisa = formValue == null || formValue.Equals("0");
                                        ordenDatoClinico.ValorString = formValue == null || formValue.Equals("0")
                                            ? ""
                                            : formValue;
                                    }
                                    else
                                    {
                                        ordenDatoClinico.noPrecisa = checkNoPrecisaBoolean;
                                        ordenDatoClinico.ValorString = !checkNoPrecisaBoolean ? formValue : string.Empty;
                                    }
                                    ordenDatoClinicoFormulario.noPrecisa = ordenDatoClinico.noPrecisa;
                                    ordenDatoClinicoFormulario.ValorString = ordenDatoClinico.ValorString;
                                }
                            }
                        }
                        orden.enfermedadList = enfermedadListAgregados;
                        oOrden.InsertOrdenCovid(orden, Enums.TipoRegistroOrden.ORDEN_RECEPCION);
                    }
                }
            }

            return orden;
        }
        [HttpPost]
        public ActionResult Save(string pacientes, string solicitante, string hddDatoEESSEnvio, string hddDatoEESSOrigen, string fechaIngresoINS, string FechaObtencion, string tipoDocumento, string nroOficio, string idOrdenPadreExportado)
        {

            ViewBag.idOrdenPadreExportado = "";
            string codigoOrdenGenerado = string.Empty;
            var LstxOrdenMuestraLinealkobo = new List<OrdenMuestraLinealkobo>();

            LstxOrdenMuestraLinealkobo = new OrdenBl().ListOrdenMuestraLinealkobo(idOrdenPadreExportado);
            DataTable dtlista = new DataTable("dtlista");
            dtlista = ConvertToDataTable(LstxOrdenMuestraLinealkobo);

            dtlista.TableName = "MUESTRAS";
            dtlista.Columns[0].ColumnName = "CODIGO ORDEN";
            dtlista.Columns[1].ColumnName = "CODIGO MUESTRA";
            dtlista.Columns[2].ColumnName = "CODIGO LINEAL";
            dtlista.Columns[3].ColumnName = "Nro Documento";
            dtlista.Columns[4].ColumnName = "APEPAT";
            dtlista.Columns[5].ColumnName = "APEMAT";
            dtlista.Columns[6].ColumnName = "NOMBRE";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtlista);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                return new ExcelResult(wb, "Resultados");

            }
        }

    }
}
using BusinessLayer;
using BusinessLayer.Compartido;
using BusinessLayer.Interfaces;
using ClosedXML.Excel;
using DataLayer;
using Model;
using Model.Enums;
using Model.ViewData;
using NetLab.App_Code;
using NetLab.Extensions.ActionResults;
using NetLab.Models.Shared;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Utilitario;
using Ubigeo = Model.Ubigeo;

namespace NetLab.Controllers
{
    /// <summary>
    /// Descripción: Controlador para mostrar la pantalla de busqueda y configuracion de paramteros.
    /// Author: Terceros.
    /// Fecha Creacion: 01/01/2017
    /// Fecha Modificación: 02/02/2017.
    /// Modificación: Se agregaron comentarios.
    /// </summary>
    public class ConsultaResultadosController : ParentController
    {
        // GET: Consulta
        public ActionResult Index()
        {
            ListaBl listaBl = new ListaBl();
            List<SelectListItem> tipoDocumentoList = new List<SelectListItem>();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.tipoDocumentoList = tipoDocumentoList;

            ViewBag.esFechaRegistro = "1";

            ViewBag.esTipoUbigueo = "2";

            ViewBag.esTipoEstablecimiento = "1";

            ViewBag.IsSearch = false;

            var dal = new UsuarioDal();

            //Institucion-DISA-RED-MICRORED-EE.SS_LAB
            var laboratorios = dal.getLaboratoriosUsuario(Logueado.idUsuario);

            var local = new Local
            {
                IdUsuario = Logueado.idUsuario,
                TipoLocal = TipoLocal.Institucion
            };

            var localBl = new LocalBl();
            var institucion = localBl.GetInstituciones(local);


            Session["LaboratoriosLogin"] = laboratorios;
            Session["Instituciones"] = institucion;


            var instituciones = (List<Institucion>)Session["Instituciones"];

            instituciones?.Insert(0, new Institucion
            {
                IdInstitucion = "0",
                nombreInstitucion = "Seleccione un Sub Sector",
                codigoInstitucion = 0
            });

            ViewBag.laboratorios = laboratorios;
            ViewBag.instituciones = instituciones;

            ViewBag.Locales = new Local
            {
                IdInstitucion = 0,
                IdEstablecimiento = 0,
                IdRed = "0",
                IdDisa = "0",
                IdMicroRed = "0"
            };

            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nombrePaciente"></param>
        /// <param name="apellidoPaciente"></param>
        /// <param name="apellidoPaciente2"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="enfermedades"></param>
        /// <param name="nroOficio"></param>
        /// <param name="esTipoEstablecimiento"></param>
        /// <param name="establecimientoscadena"></param>
        /// <param name="ActualDepartamento"></param>
        /// <param name="ActualProvincia"></param>
        /// <param name="ActualDistrito"></param>
        /// <param name="examen"></param>
        /// <param name="tipoPaciente"></param>
        /// <param name="esTipoUbigueo"></param>
        /// <param name="estadoResultado"></param>
        /// <param name="ordenarPor"></param>
        /// <param name="tipoOrdenacion"></param>
        /// <param name="hdnTipo"></param>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="hdnEstablecimiento"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult IndexConsulta(int esFechaRegistro, string fechaDesde, string fechaHasta, string nroDocumento, string nombrePaciente, string apellidoPaciente,
                                          string apellidoPaciente2, string codigoMuestra, string codigoOrden, string enfermedades, string nroOficio, int? esTipoEstablecimiento,
                                          string establecimientoscadena, string ActualDepartamento, string ActualProvincia, string ActualDistrito, string examen, int? tipoPaciente,
                                          int? esTipoUbigueo, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion, string hdnTipo, string hdnInstitucion, string hdnDisa,
                                          string hdnRed, string hdnMicroRed, string hdnEstablecimiento)
        {
            ViewBag.IsSearch = true;
            initFormData();

            DateTime fechaDesdeA = DateTime.Parse("2016-01-01 00:00:00");
            DateTime fechaHastaA = DateTime.Now;

            var fechaDesdeCriteria = fechaDesde ?? string.Empty;
            var fechaHastaCriteria = fechaHasta ?? string.Empty;
            if (!fechaDesdeCriteria.Equals(""))
                fechaDesdeA = DateTime.ParseExact(fechaDesdeCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (!fechaHastaCriteria.Equals(""))
                fechaHastaA = DateTime.ParseExact(fechaHastaCriteria + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            ConsultaResultadosBl ordenBl = new ConsultaResultadosBl();
            List<OrdenIngresarResultadoVd> ordenes = new List<OrdenIngresarResultadoVd>();
            //ordenes = ordenBl.GetOrdenMuestraResultadosByUser(Logueado.idUsuario, 
            //    esFechaRegistro, fechaDesdeA, fechaHastaA, nroDocumento,
            //    nombrePaciente, apellidoPaciente, apellidoPaciente2,
            //    codigoMuestra, codigoOrden, enfermedades, nroOficio, 
            //    esTipoEstablecimiento, establecimientoscadena, examen, tipoPaciente, esTipoUbigueo, ActualDepartamento,ActualProvincia,ActualDistrito, estadoResultado,ordenarPor,tipoOrdenacion);

            ViewBag.esFechaRegistro = esFechaRegistro.ToString();

            ViewBag.esTipoUbigueo = esTipoUbigueo.ToString();

            ViewBag.esTipoEstablecimiento = esTipoEstablecimiento.ToString();

            return View();
        }
        /// <summary>
        /// Descripción: Metodo par inicializar los comobos: Ordenacion y Tipo Oreden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        private void initFormData()
        {
            ViewBag.ordenacionList = new List<SelectListItem>{
                new SelectListItem{ Text="Fecha Registro", Value = "0" },
                new SelectListItem{ Text="Fecha Coleccion", Value = "1" },
                new SelectListItem{ Text="Fecha Resultado", Value = "2" },
                new SelectListItem{ Text="Enfermedad", Value = "3" },
                new SelectListItem{ Text="N° Documento", Value = "4" },
                new SelectListItem{ Text="Paciente", Value = "5" },
                new SelectListItem{ Text="Código Muestra", Value = "6" }
            };

            ViewBag.tipoOrdenacionList = new List<SelectListItem>{
                new SelectListItem{ Text="Ascendente", Value = "0" },
                new SelectListItem{ Text="Descendente", Value = "1" }
            };
        }
        /// <summary>
        /// Descripción: Metodo para setear el valor seleccionado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        private void setSelectedItem(List<SelectListItem> list, string value)
        {
            foreach (SelectListItem item in list)
            {
                if (item.Value == value) item.Selected = true;
            }
        }
        /// <summary>
        /// Descripción: Metodo para convertir una lista a un data table y exportar a Excel.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                System.Data.DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        /// <summary>
        /// Descripción: Controlador para la exportacion de los resultados a excel
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportExcel()
        {
            try
            {
                List<OrdenIngresarResultadoVd> ordenes = new List<OrdenIngresarResultadoVd>();

                if (this.Session["CER_Ordenes"] != null)
                {
                    ordenes = (List<OrdenIngresarResultadoVd>)this.Session["CER_Ordenes"];
                }

                int tipoUsuario = (Logueado.tipo == 6 || Logueado.tipo == 7) ? 67 : Logueado.tipo;

                DataTable dtExportData = new DataTable("dtExportData");
                dtExportData = ConvertToDataTable(ordenes);

                dtExportData.Columns.Remove("idOrden");
                dtExportData.Columns.Remove("idPaciente");
                dtExportData.Columns.Remove("idAnimal");
                dtExportData.Columns.Remove("idCBS");
                dtExportData.Columns.Remove("nombreEstablecimiento");
                dtExportData.Columns.Remove("nroOficio");
                dtExportData.Columns.Remove("codigoOrden");
                dtExportData.Columns.Remove("nroDocumento");
                dtExportData.Columns.Remove("tipoOrden");
                dtExportData.Columns.Remove("idEstablecimiento");
                dtExportData.Columns.Remove("estadoOrden");
                dtExportData.Columns.Remove("fechaNacimiento");
                dtExportData.Columns.Remove("fechaRegistro");
                dtExportData.Columns.Remove("fechaSolicitud");
                dtExportData.Columns.Remove("nombrePaciente");
                dtExportData.Columns.Remove("edadPaciente");
                dtExportData.Columns.Remove("edad");
                dtExportData.Columns.Remove("nombreProyecto");
                dtExportData.Columns.Remove("codigoPaciente");
                dtExportData.Columns.Remove("EdadAnios");
                dtExportData.Columns.Remove("Genero");
                dtExportData.Columns.Remove("tipoDocumento");
                dtExportData.Columns.Remove("nombreGenero");
                dtExportData.Columns.Remove("idExamen");
                dtExportData.Columns.Remove("nombreEnfermedad");
                dtExportData.Columns.Remove("nombreExamen");
                dtExportData.Columns.Remove("idLaboratorioProcesamiento");
                dtExportData.Columns.Remove("codigoMuestra");
                dtExportData.Columns.Remove("fechaRecepcion");
                dtExportData.Columns.Remove("fechaColeccion");
                dtExportData.Columns.Remove("metodoExamen");
                dtExportData.Columns.Remove("status");
                dtExportData.Columns.Remove("muestrasValidar");
                dtExportData.Columns.Remove("muestrasPendientesRecepcion");
                dtExportData.Columns.Remove("areas");
                dtExportData.Columns.Remove("step");
                dtExportData.Columns.Remove("ingresado");
                dtExportData.Columns.Remove("validado");
                dtExportData.Columns.Remove("liberado");
                dtExportData.Columns.Remove("statusP");
                dtExportData.Columns.Remove("conformeProceso");
                dtExportData.Columns.Remove("estatusE");
                dtExportData.Columns.Remove("ordenInfoListnew");
                dtExportData.Columns.Remove("resultadosList");
                dtExportData.Columns.Remove("MuestraConforme");
                //dtExportData.Columns.Remove("criteriosRechazo");
                dtExportData.Columns.Remove("flagResultado");
                dtExportData.Columns.Remove("conformeP");
                dtExportData.Columns.Remove("UbigeoActual");
                dtExportData.Columns.Remove("UbigeoReniec");
                //dtExportData.Columns.Remove("conMuestraConforme");
                dtExportData.Columns.Remove("conEstatusE");
                dtExportData.Columns.Remove("ConColor");
                dtExportData.Columns.Remove("ConFechAddExamen");
                dtExportData.Columns.Remove("error");
                dtExportData.Columns.Remove("dias");
                dtExportData.Columns.Remove("catalogo");
                dtExportData.Columns.Remove("idOrdenExamen");
                //dtExportData.Columns.Remove("ConMotivo");
                dtExportData.Columns.Remove("tipoExamen");
                dtExportData.Columns.Remove("FechaSiembraCultivo");
                dtExportData.Columns.Remove("ConInstitucionOrigen");

                dtExportData.TableName = "Registros";
                dtExportData.Columns.Remove("ConFechaHoraRecepcionInsRom");
                dtExportData.Columns.Remove("ConHoraRecepcionROM");
                switch (tipoUsuario)
                {
                    case 1:
                    case 4:

                        dtExportData.Columns.Remove("ConFechaSolicitud");
                        dtExportData.Columns.Remove("ConnroOficio");
                        //dtExportData.Columns.Remove("ConDepartamentoOrigen");
                        //dtExportData.Columns.Remove("ConProvinciaOrigen");
                        //dtExportData.Columns.Remove("ConDistritoOrigen");
                        //dtExportData.Columns.Remove("ConDisaOrigen");
                        //dtExportData.Columns.Remove("ConRedOrigen");
                        //dtExportData.Columns.Remove("ConMicroRedOrigen");
                        dtExportData.Columns.Remove("ConEstablecimientoLatitud");
                        dtExportData.Columns.Remove("ConEstablecimientoLongitud");
                        dtExportData.Columns.Remove("ConDepartamentoDestino");
                        dtExportData.Columns.Remove("ConProvinciaDestino");
                        dtExportData.Columns.Remove("ConDistritoDestino");
                        dtExportData.Columns.Remove("ConDisaDestino");
                        dtExportData.Columns.Remove("ConRedDestino");
                        dtExportData.Columns.Remove("ConMicroRedDestino");
                        dtExportData.Columns.Remove("ConFechaNacimiento");
                        dtExportData.Columns.Remove("ConEnfermedad");
                        dtExportData.Columns.Remove("ConComponente");
                        dtExportData.Columns.Remove("ConMuestraConforme");
                        dtExportData.Columns.Remove("criteriosRechazo");
                        //dtExportData.Columns.Remove("ConFechaHoraRecepcionInsRom");
                        dtExportData.Columns.Remove("ConFechaHoraRecepcionLAB");
                        dtExportData.Columns.Remove("ConHoraRecepcionLAB");
                        //dtExportData.Columns.Remove("ConFechaHoraValidado");
                        dtExportData.Columns.Remove("fechaRegistroOrden");
                        dtExportData.Columns.Remove("horaRegistroOrden");
                        dtExportData.Columns.Remove("fechaRegistroRecepcionMuestra");
                        dtExportData.Columns.Remove("horaRegistroRecepcionMuestra");
                        dtExportData.Columns.Remove("ConSexo");
                        dtExportData.Columns.Remove("ObservacionRechazo");
                        //dtExportData.Columns.Remove("Catalogo");

                        dtExportData.Columns[0].ColumnName = "Codigo De Orden";
                        dtExportData.Columns[1].ColumnName = "Departamento EE.SS Origen";
                        dtExportData.Columns[2].ColumnName = "Provincia EE.SS Origen";
                        dtExportData.Columns[3].ColumnName = "Distrito EE.SS Origen";
                        dtExportData.Columns[4].ColumnName = "Disa EE.SS Origen";
                        dtExportData.Columns[5].ColumnName = "Red EE.SS Origen";
                        dtExportData.Columns[6].ColumnName = "Micro Red EE.SS Origen";
                        dtExportData.Columns[7].ColumnName = "Establecimiento de Origen";
                        dtExportData.Columns[8].ColumnName = "Laboratorio Destino";
                        dtExportData.Columns[9].ColumnName = "Documento de Indentidad";
                        dtExportData.Columns[10].ColumnName = "Nombre Paciente";
                        dtExportData.Columns[11].ColumnName = "Codigo de Muestra";
                        dtExportData.Columns[12].ColumnName = "Tipo de Muestra";
                        dtExportData.Columns[13].ColumnName = "Nombre de Examen";
                        dtExportData.Columns[14].ColumnName = "Resultado";
                        dtExportData.Columns[15].ColumnName = "Fecha Colección";
                        dtExportData.Columns[16].ColumnName = "Hora Colección";
                        dtExportData.Columns[17].ColumnName = "Fecha Recepción ROM";
                        //dtExportData.Columns[18].ColumnName = "Hora Recepción ROM";
                        dtExportData.Columns[18].ColumnName = "Fecha Verificación";
                        dtExportData.Columns[19].ColumnName = "Hora Verificación";
                        dtExportData.Columns[20].ColumnName = "Estatus Resultado";
                        dtExportData.Columns[21].ColumnName = "Motivo";
                        dtExportData.Columns[22].ColumnName = "Edad";
                        dtExportData.Columns[23].ColumnName = "Direccion de Paciente";
                        dtExportData.Columns[24].ColumnName = "Telefono";
                        break;

                    case 5:
                        dtExportData.Columns[0].ColumnName = "Fecha Solicitud";
                        dtExportData.Columns[1].ColumnName = "Codigo Orden";
                        dtExportData.Columns[2].ColumnName = "Documento Referencia";
                        dtExportData.Columns[3].ColumnName = "Departamento EE.SS Origen";
                        dtExportData.Columns[4].ColumnName = "Provincia EE.SS Origen";
                        dtExportData.Columns[5].ColumnName = "Distrito EE.SS Origen";
                        dtExportData.Columns[6].ColumnName = "Disa EE.SS Origen";
                        dtExportData.Columns[7].ColumnName = "Red EE.SS Origen";
                        dtExportData.Columns[8].ColumnName = "Micro Red EE.SS Origen";
                        dtExportData.Columns[9].ColumnName = "EE.SS Origen";
                        dtExportData.Columns[10].ColumnName = "Latitud";
                        dtExportData.Columns[11].ColumnName = "Longitud";
                        dtExportData.Columns[12].ColumnName = "Departamento EE.SS Destino";
                        dtExportData.Columns[13].ColumnName = "Provincia EE.SS Destino";
                        dtExportData.Columns[14].ColumnName = "Distrito EE.SS Destino";
                        dtExportData.Columns[15].ColumnName = "Disa EE.SS Destino";
                        dtExportData.Columns[16].ColumnName = "Red EE.SS Destino";
                        dtExportData.Columns[17].ColumnName = "Micro Red EE.SS Destino";
                        dtExportData.Columns[18].ColumnName = "EE.SS Destino";
                        dtExportData.Columns[19].ColumnName = "Documento Identidad";
                        dtExportData.Columns[20].ColumnName = "Fecha Nacimiento";
                        dtExportData.Columns[21].ColumnName = "Paciente";
                        dtExportData.Columns[22].ColumnName = "Codigo Muestra";
                        dtExportData.Columns[23].ColumnName = "Tipo Muestra";
                        dtExportData.Columns[24].ColumnName = "Enfermedad";
                        dtExportData.Columns[25].ColumnName = "Nombre de Examen";
                        dtExportData.Columns[26].ColumnName = "Componente";
                        dtExportData.Columns[27].ColumnName = "Resultado";
                        dtExportData.Columns[28].ColumnName = "Muestra Conforme";
                        dtExportData.Columns[29].ColumnName = "Criterios de Rechazo";
                        dtExportData.Columns[30].ColumnName = "Observaciones de Rechazo";
                        dtExportData.Columns[31].ColumnName = "Fecha Colección";
                        dtExportData.Columns[32].ColumnName = "Hora Colección";
                        //dtExportData.Columns[33].ColumnName = "Fecha Recepción INS ROM";
                        dtExportData.Columns[33].ColumnName = "Fecha Recepción ROM";
                        //dtExportData.Columns[35].ColumnName = "Hora Recepción ROM";
                        dtExportData.Columns[34].ColumnName = "Fecha Recepción LAB";
                        dtExportData.Columns[35].ColumnName = "Hora Recepción LAB";
                        dtExportData.Columns[36].ColumnName = "Fecha Verificación";
                        dtExportData.Columns[37].ColumnName = "Hora Verificación";

                        dtExportData.Columns[38].ColumnName = "Fecha Registro Orden";
                        dtExportData.Columns[39].ColumnName = "Hora Registro Orden";
                        dtExportData.Columns[40].ColumnName = "Fecha Registro Recepcion Muestra";
                        dtExportData.Columns[41].ColumnName = "Hora Registro Recepcion Muestra";

                        dtExportData.Columns[42].ColumnName = "Estatus Resultados";
                        dtExportData.Columns[43].ColumnName = "Motivo";
                        dtExportData.Columns[44].ColumnName = "Edad";
                        dtExportData.Columns[45].ColumnName = "Genero";
                        dtExportData.Columns[46].ColumnName = "Direccion de Paciente";
                        dtExportData.Columns[47].ColumnName = "Telefono";
                        break;
                    case 67:

                        dtExportData.Columns.Remove("ConFechaSolicitud");
                        dtExportData.Columns.Remove("ConnroOficio");
                        //dtExportData.Columns.Remove("ConDepartamentoOrigen");
                        //dtExportData.Columns.Remove("ConProvinciaOrigen");
                        //dtExportData.Columns.Remove("ConDistritoOrigen");
                        //dtExportData.Columns.Remove("ConDisaOrigen");
                        //dtExportData.Columns.Remove("ConRedOrigen");
                        //dtExportData.Columns.Remove("ConMicroRedOrigen");
                        dtExportData.Columns.Remove("ConEstablecimientoLatitud");
                        dtExportData.Columns.Remove("ConEstablecimientoLongitud");
                        //dtExportData.Columns.Remove("ConEstablecimientoSolicitante"); 
                        dtExportData.Columns.Remove("ConDepartamentoDestino");
                        dtExportData.Columns.Remove("ConProvinciaDestino");
                        dtExportData.Columns.Remove("ConDistritoDestino");
                        dtExportData.Columns.Remove("ConDisaDestino");
                        dtExportData.Columns.Remove("ConRedDestino");
                        dtExportData.Columns.Remove("ConMicroRedDestino");
                        dtExportData.Columns.Remove("ConFechaNacimiento");
                        dtExportData.Columns.Remove("ConEnfermedad");
                        dtExportData.Columns.Remove("ConComponente");
                        dtExportData.Columns.Remove("ConMuestraConforme");
                        dtExportData.Columns.Remove("criteriosRechazo");
                        //dtExportData.Columns.Remove("ConFechaHoraRecepcionInsRom");
                        dtExportData.Columns.Remove("ConFechaHoraRecepcionLAB");
                        dtExportData.Columns.Remove("ConHoraRecepcionLAB");
                        //dtExportData.Columns.Remove("ConFechaHoraValidado");
                        dtExportData.Columns.Remove("fechaRegistroOrden");
                        dtExportData.Columns.Remove("horaRegistroOrden");
                        dtExportData.Columns.Remove("fechaRegistroRecepcionMuestra");
                        dtExportData.Columns.Remove("horaRegistroRecepcionMuestra");
                        dtExportData.Columns.Remove("ObservacionRechazo");
                        dtExportData.Columns.Remove("ConnombrePaciente");
                        dtExportData.Columns.Remove("ConID_Muestra");
                        dtExportData.Columns.Remove("ConcodigoOrden");
                        dtExportData.Columns.Remove("ConTipoMuestra");
                        dtExportData.Columns.Remove("ConDireccionPaciente");

                        dtExportData.Columns[0].ColumnName = "Departamento EE.SS Origen";
                        dtExportData.Columns[1].ColumnName = "Provincia EE.SS Origen";
                        dtExportData.Columns[2].ColumnName = "Distrito EE.SS Origen";
                        dtExportData.Columns[3].ColumnName = "Disa EE.SS Origen";
                        dtExportData.Columns[4].ColumnName = "Red EE.SS Origen";
                        dtExportData.Columns[5].ColumnName = "Micro Red EE.SS Origen";
                        dtExportData.Columns[6].ColumnName = "Establecimiento de Origen";
                        dtExportData.Columns[7].ColumnName = "Laboratorio Destino";
                        dtExportData.Columns[8].ColumnName = "Documento de Indentidad";
                        dtExportData.Columns[9].ColumnName = "Nombre de Examen";
                        dtExportData.Columns[10].ColumnName = "Resultado";
                        dtExportData.Columns[11].ColumnName = "Fecha Colección";
                        dtExportData.Columns[12].ColumnName = "Hora Colección";
                        dtExportData.Columns[13].ColumnName = "Fecha Recepción ROM";
                        //dtExportData.Columns[14].ColumnName = "Hora Recepción ROM";
                        dtExportData.Columns[14].ColumnName = "Fecha Verificación";
                        dtExportData.Columns[15].ColumnName = "Hora Verificación";
                        dtExportData.Columns[16].ColumnName = "Estatus Resultado";
                        dtExportData.Columns[17].ColumnName = "Motivo";
                        dtExportData.Columns[18].ColumnName = "Edad";
                        dtExportData.Columns[19].ColumnName = "Sexo";
                        dtExportData.Columns[20].ColumnName = "Telefono";
                        break;
                    default:
                        dtExportData.Columns[0].ColumnName = "Fecha Solicitud";
                        dtExportData.Columns[1].ColumnName = "Codigo Orden";
                        dtExportData.Columns[2].ColumnName = "Documento Referencia";
                        dtExportData.Columns[3].ColumnName = "Departamento EE.SS Origen";
                        dtExportData.Columns[4].ColumnName = "Provincia EE.SS Origen";
                        dtExportData.Columns[5].ColumnName = "Distrito EE.SS Origen";
                        dtExportData.Columns[6].ColumnName = "Disa EE.SS Origen";
                        dtExportData.Columns[7].ColumnName = "Red EE.SS Origen";
                        dtExportData.Columns[8].ColumnName = "Micro Red EE.SS Origen";
                        dtExportData.Columns[9].ColumnName = "EE.SS Origen";
                        dtExportData.Columns[10].ColumnName = "Latitud";
                        dtExportData.Columns[11].ColumnName = "Longitud";
                        dtExportData.Columns[12].ColumnName = "Departamento EE.SS Destino";
                        dtExportData.Columns[13].ColumnName = "Provincia EE.SS Destino";
                        dtExportData.Columns[14].ColumnName = "Distrito EE.SS Destino";
                        dtExportData.Columns[15].ColumnName = "Disa EE.SS Destino";
                        dtExportData.Columns[16].ColumnName = "Red EE.SS Destino";
                        dtExportData.Columns[17].ColumnName = "Micro Red EE.SS Destino";
                        dtExportData.Columns[18].ColumnName = "EE.SS Destino";
                        dtExportData.Columns[19].ColumnName = "Documento Identidad";
                        dtExportData.Columns[20].ColumnName = "Fecha Nacimiento";
                        dtExportData.Columns[21].ColumnName = "Paciente";
                        dtExportData.Columns[22].ColumnName = "Codigo Muestra";
                        dtExportData.Columns[23].ColumnName = "Tipo Muestra";
                        dtExportData.Columns[24].ColumnName = "Enfermedad";
                        dtExportData.Columns[25].ColumnName = "Nombre de Examen";
                        dtExportData.Columns[26].ColumnName = "Componente";
                        dtExportData.Columns[27].ColumnName = "Resultado";
                        dtExportData.Columns[28].ColumnName = "Muestra Conforme";
                        dtExportData.Columns[29].ColumnName = "Criterios de Rechazo";
                        dtExportData.Columns[30].ColumnName = "Observaciones de Rechazo";
                        dtExportData.Columns[31].ColumnName = "Fecha Colección";
                        dtExportData.Columns[32].ColumnName = "Fecha Recepción ROM";
                        dtExportData.Columns[33].ColumnName = "Fecha Recepción LAB";
                        dtExportData.Columns[34].ColumnName = "Fecha Verificación";
                        dtExportData.Columns[35].ColumnName = "Estatus Resultados";
                        dtExportData.Columns[36].ColumnName = "Motivo";
                        dtExportData.Columns[37].ColumnName = "edad";
                        dtExportData.Columns[38].ColumnName = "Genero";
                        dtExportData.Columns[39].ColumnName = "Direccion de Paciente";
                        dtExportData.Columns[40].ColumnName = "Telefono";
                        break;
                }


                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtExportData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    return new ExcelResult(wb, "Resultados");

                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;filename= ResultadoBusqueda.xlsx");

                    //using (MemoryStream MyMemoryStream = new MemoryStream())
                    //{
                    //    wb.SaveAs(MyMemoryStream);
                    //    MyMemoryStream.WriteTo(Response.OutputStream);
                    //    Response.Flush();
                    //    Response.End();
                    //}
                }
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", " COnsultaResultadosController - ExportExcel ");
                throw ex;
            }
        }
        /// <summary>
        /// Descripción: Exportar los Datos Clinicos a Excel.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        //public ActionResult ExportExcelDatosClinicos()
        //{

        //    DataTable DTexcelDatosClinicos = Session["dtDatosClinicos"] as DataTable;

        //    if (DTexcelDatosClinicos.Rows.Count > 0)
        //    {
        //        DTexcelDatosClinicos.TableName = "Datos Clínicos";

        //        using (XLWorkbook wb = new XLWorkbook())
        //        {

        //            wb.Worksheets.Add(DTexcelDatosClinicos);
        //            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //            wb.Style.Font.Bold = true;

        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.Charset = "";
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("content-disposition", "attachment;filename= Datos Clinicos.xlsx");

        //            using (MemoryStream MyMemoryStream = new MemoryStream())
        //            {
        //                wb.SaveAs(MyMemoryStream);
        //                MyMemoryStream.WriteTo(Response.OutputStream);
        //                Response.Flush();
        //                Response.End();
        //            }
        //        }
        //    }
        //    return View("Index");
        //}
        /// <summary>
        /// Descripción: Metodo para seleccionar los parametros y configurar mas campos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnTipo"></param>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="hdnEstablecimiento"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nombrePaciente"></param>
        /// <param name="apellidoPaciente"></param>
        /// <param name="apellidoPaciente2"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="enfermedades"></param>
        /// <param name="nroOficio"></param>
        /// <param name="esTipoEstablecimiento"></param>
        /// <param name="ActualDepartamento"></param>
        /// <param name="ActualProvincia"></param>
        /// <param name="ActualDistrito"></param>
        /// <param name="examen"></param>
        /// <param name="tipoPaciente"></param>
        /// <param name="esTipoUbigueo"></param>
        /// <param name="estadoResultado"></param>
        /// <param name="ordenarPor"></param>
        /// <param name="tipoOrdenacion"></param>
        /// <param name="EstablecimientoCadena"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectMain(string hdnTipo, string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed,
                                       string hdnEstablecimiento, int esFechaRegistro, string fechaDesde, string fechaHasta, string nroDocumento,
                                       string nombrePaciente, string apellidoPaciente, string apellidoPaciente2, string codigoMuestra,
                                       string codigoOrden, string hdnEnfermedad, string nroOficio, int? esTipoEstablecimiento, string ActualDepartamento,
                                       string ActualProvincia, string ActualDistrito, string hdnExamen, int? tipoPaciente, int? esTipoUbigueo, int? estadoResultado,
                                       int? ordenarPor, int? tipoOrdenacion, string EstablecimientoCadena, string esDatoClinico)
        {

            ViewBag.esFechaRegistro = esFechaRegistro.ToString();

            ViewBag.esTipoUbigueo = esTipoUbigueo.ToString();

            ViewBag.esTipoEstablecimiento = esTipoEstablecimiento.ToString();

            ViewBag.iddepartamento = ActualDepartamento;

            ViewBag.idprovincia = ActualProvincia;

            ViewBag.iddistrito = ActualDistrito;

            switch (hdnTipo)
            {
                case "Instituciones":
                    return SelectInstituciones(hdnInstitucion, Logueado.idUsuario);
                case "Disa":
                    return SelectDisa(hdnInstitucion, hdnDisa, Logueado.idUsuario);
                case "Red":
                    return SelectRed(hdnInstitucion, hdnDisa, hdnRed, Logueado.idUsuario);
                case "MicroRed":
                    return SelectMicroRed(hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, Logueado.idUsuario);
                case "Establecimientos":
                    return SelectFilter(hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, hdnEstablecimiento, esFechaRegistro, fechaDesde,
                                        fechaHasta, nroDocumento, nombrePaciente, apellidoPaciente, apellidoPaciente2, codigoMuestra, codigoOrden, hdnEnfermedad,
                                        nroOficio, esTipoEstablecimiento, ActualDepartamento, ActualProvincia, ActualDistrito, hdnExamen, tipoPaciente, esTipoUbigueo,
                                        estadoResultado, ordenarPor, tipoOrdenacion, EstablecimientoCadena, esDatoClinico);
            }
            ViewBag.IsSearch = false;
            return View("Index");
        }

        /// <summary>
        /// Descripción: Controlador para realizar la consulta con los paratmetros configurados, 
        ///              obtiene informacion de los resultados y de los datos clinicos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="hdnEstablecimiento"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nombrePaciente"></param>
        /// <param name="apellidoPaciente"></param>
        /// <param name="apellidoPaciente2"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="enfermedades"></param>
        /// <param name="nroOficio"></param>
        /// <param name="esTipoEstablecimiento"></param>
        /// <param name="ActualDepartamento"></param>
        /// <param name="ActualProvincia"></param>
        /// <param name="ActualDistrito"></param>
        /// <param name="examen"></param>
        /// <param name="tipoPaciente"></param>
        /// <param name="esTipoUbigueo"></param>
        /// <param name="estadoResultado"></param>
        /// <param name="ordenarPor"></param>
        /// <param name="tipoOrdenacion"></param>
        /// <param name="EstablecimientoCadena"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectFilter(string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed,
                                         string hdnEstablecimiento, int esFechaRegistro, string fechaDesde, string fechaHasta, string nroDocumento, string nombrePaciente,
                                         string apellidoPaciente, string apellidoPaciente2, string codigoMuestra, string codigoOrden, string hdnEnfermedad, string nroOficio,
                                         int? esTipoEstablecimiento, string ActualDepartamento, string ActualProvincia, string ActualDistrito, string hdnExamen, int? tipoPaciente,
                                         int? esTipoUbigueo, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion, string EstablecimientoCadena, string esDatoClinico)
        {
            try
            {
                ViewBag.IsSearch = true;
                initFormData();

                if (EstablecimientoCadena == null)
                {
                    EstablecimientoCadena = " ";
                }

                if (esTipoEstablecimiento == 1 || esTipoEstablecimiento == 3)
                {
                    EstablecimientoCadena = " ";
                }

                if (esTipoEstablecimiento == 2 || esTipoEstablecimiento == 4)
                {
                    hdnInstitucion = "0";
                    hdnDisa = " ";
                    hdnRed = " ";
                    hdnMicroRed = " ";
                    hdnEstablecimiento = " ";
                }


                if (nombrePaciente == "")
                {
                    nombrePaciente = " ";
                }

                if (apellidoPaciente == "")
                {
                    apellidoPaciente = " ";
                }
                if (apellidoPaciente2 == "")
                {
                    apellidoPaciente2 = " ";
                }
                if (codigoMuestra == "")
                {
                    codigoMuestra = " ";
                }
                if (codigoOrden == "")
                {
                    codigoOrden = " ";
                }
                if (hdnEnfermedad == "")
                {
                    hdnEnfermedad = " ";
                }
                if (nroOficio == "")
                {
                    nroOficio = " ";
                }
                if (hdnExamen == "")
                {
                    hdnExamen = " ";
                }
                if (ActualDepartamento == "")
                {
                    ActualDepartamento = " ";
                }
                if (ActualProvincia == "")
                {
                    ActualProvincia = " ";
                }
                if (ActualDistrito == "")
                {
                    ActualDistrito = " ";
                }
                if (nroDocumento == "")
                {
                    nroDocumento = " ";
                }
                if (hdnRed.Length > 0 && hdnRed != "-" && hdnRed != " ")
                {
                    hdnRed = hdnRed.Substring(0, 2);
                }
                if (hdnMicroRed.Length > 0 && hdnMicroRed != "-" && hdnMicroRed != " ")
                {
                    hdnMicroRed = hdnMicroRed.Length < 2 ? hdnMicroRed : hdnMicroRed.Substring(0, 2);
                }
                DateTime fechaDesdeA = DateTime.Parse("2016-01-01 00:00:00");
                DateTime fechaHastaA = DateTime.Now;

                var fechaDesdeCriteria = fechaDesde ?? string.Empty;
                var fechaHastaCriteria = fechaHasta ?? string.Empty;
                if (!fechaDesdeCriteria.Equals(""))
                    fechaDesdeA = DateTime.ParseExact(fechaDesdeCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (!fechaHastaCriteria.Equals(""))
                    fechaHastaA = DateTime.ParseExact(fechaHastaCriteria + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                ConsultaResultadosBl ordenBl = new ConsultaResultadosBl();
                List<OrdenIngresarResultadoVd> ordenes = new List<OrdenIngresarResultadoVd>();

                if (esDatoClinico == "1")
                {
                    DataTable datosClinicos = ordenBl.GetPruebaDatosClinicos(Logueado.idUsuario, esFechaRegistro, fechaDesde, fechaHasta, nroDocumento, nombrePaciente, apellidoPaciente,
                                                                  apellidoPaciente2, codigoMuestra, codigoOrden, hdnEnfermedad, nroOficio, esTipoEstablecimiento, hdnExamen, tipoPaciente,
                                                                  esTipoUbigueo, ActualDepartamento, ActualProvincia, ActualDistrito, estadoResultado, ordenarPor, tipoOrdenacion,
                                                                  EstablecimientoCadena, hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, hdnEstablecimiento);
                    if (datosClinicos != null)
                    {
                        return ExportExcelDatosClinicos(datosClinicos);
                    }

                }
                else
                {
                    //var ordenBln = new OrdenDal();
                    //var ordenesnew = ordenBln.GetDatos(Logueado.idUsuario, esFechaRegistro, fechaDesde, fechaHasta, nroDocumento, nombrePaciente, apellidoPaciente,
                    //                                              apellidoPaciente2, codigoMuestra, codigoOrden, hdnEnfermedad, nroOficio, esTipoEstablecimiento, hdnExamen, tipoPaciente,
                    //                                              esTipoUbigueo, ActualDepartamento, ActualProvincia, ActualDistrito, estadoResultado, ordenarPor, tipoOrdenacion,
                    //                                              EstablecimientoCadena, hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, hdnEstablecimiento, Semaforo);
                    ordenes = ordenBl.GetOrdenMuestraResultadosByUser(Logueado.idUsuario, esFechaRegistro, fechaDesde, fechaHasta, nroDocumento, nombrePaciente, apellidoPaciente,
                                                                  apellidoPaciente2, codigoMuestra, codigoOrden, hdnEnfermedad, nroOficio, esTipoEstablecimiento, hdnExamen, tipoPaciente,
                                                                  esTipoUbigueo, ActualDepartamento, ActualProvincia, ActualDistrito, estadoResultado, ordenarPor, tipoOrdenacion,
                                                                  EstablecimientoCadena, hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, hdnEstablecimiento);
                }


                //ViewBag.OrdenesList = ordenes;
                //if (hdnEnfermedad != " ") //!String.IsNullOrEmpty(hdnEnfermedad)
                //{
                //    DataTable dtDatosClinicos = ordenBl.dtDatosClinicos(Logueado.idUsuario, esFechaRegistro, fechaDesde, fechaHasta, nroDocumento, nombrePaciente, apellidoPaciente, apellidoPaciente2,
                //                                                    codigoMuestra, codigoOrden, hdnEnfermedad, nroOficio, esTipoEstablecimiento, hdnExamen, tipoPaciente, esTipoUbigueo, ActualDepartamento,
                //                                                    ActualProvincia, ActualDistrito, estadoResultado, ordenarPor, tipoOrdenacion, EstablecimientoCadena, hdnInstitucion, hdnDisa, hdnRed,
                //                                                    hdnMicroRed, hdnEstablecimiento);


                //    Session["dtDatosClinicos"] = dtDatosClinicos;
                //}

                var dal = new UsuarioDal();

                //Institucion-DISA-RED-MICRORED-EE.SS_LAB
                var laboratorios = dal.getLaboratoriosUsuario(Logueado.idUsuario);

                var local = new Local
                {
                    IdUsuario = Logueado.idUsuario,
                    TipoLocal = TipoLocal.Institucion
                };

                var localBl = new LocalBl();
                var institucion = localBl.GetInstituciones(local);


                Session["LaboratoriosLogin"] = laboratorios;
                Session["Instituciones"] = institucion;


                var instituciones = (List<Institucion>)Session["Instituciones"];

                instituciones?.Insert(0, new Institucion
                {
                    IdInstitucion = "0",
                    nombreInstitucion = "Seleccione un Sub Sector",
                    codigoInstitucion = 0
                });

                ViewBag.laboratorios = laboratorios;
                ViewBag.instituciones = instituciones;

                ViewBag.Locales = new Local
                {
                    IdInstitucion = 0,
                    IdEstablecimiento = 0,
                    IdRed = "0",
                    IdDisa = "0",
                    IdMicroRed = "0"
                };

                Session["CER_Ordenes"] = ordenes;

                ViewBag.tipoUsuario = Convert.ToString(Logueado.tipo);
                ViewBag.TotalOrdenes = ordenes.Count;
                int maximaCantidadFilas = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ConsultaOnlineVistaMaxRows"]) ? ordenes.Count : int.Parse(ConfigurationManager.AppSettings["ConsultaOnlineVistaMaxRows"]);
                return View("Index", ordenes.Take(maximaCantidadFilas));
            }
            catch (Exception ex)
            {
                string datos = string.Format("Logueado.idUsuario:{0}, esFechaRegistro:{1}, fechaDesde:{2}, fechaHasta:{3}, nroDocumento:{4}, nombrePaciente:{5}, apellidoPaciente:{6},apellidoPaciente2:{7}, codigoMuestra:{8}, codigoOrden:{9}, hdnEnfermedad:{10}, nroOficio:{11}, esTipoEstablecimiento:{12}, hdnExamen:{13}, tipoPaciente:{14},esTipoUbigueo:{15}, ActualDepartamento:{16}, ActualProvincia:{17}, ActualDistrito:{18}, estadoResultado:{19}, ordenarPor:{20}, tipoOrdenacion:{21},EstablecimientoCadena:{22}, hdnInstitucion:{23}, hdnDisa:{24}, hdnRed:{25}, hdnMicroRed:{26}, hdnEstablecimiento:{27}.", Logueado.idUsuario, esFechaRegistro, fechaDesde, fechaHasta, nroDocumento, nombrePaciente, apellidoPaciente, apellidoPaciente2, codigoMuestra, codigoOrden, hdnEnfermedad, nroOficio, esTipoEstablecimiento, hdnExamen, tipoPaciente, esTipoUbigueo, ActualDepartamento, ActualProvincia, ActualDistrito, estadoResultado, ordenarPor, tipoOrdenacion, EstablecimientoCadena, hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, hdnEstablecimiento);
                new bsPage().LogError(ex, "LogNetLab", datos, " COnsultaResultadosController - SelectFilter ");
                throw ex;
            }
        }

        public ActionResult ExportExcelDatosClinicos(DataTable dtExportData)
        {
            DataTable DTexcelDatosClinicos = dtExportData;

            if (DTexcelDatosClinicos.Rows.Count > 0)
            {
                DTexcelDatosClinicos.TableName = "Datos Clínicos";
                //DTexcelDatosClinicos.Columns[0].ColumnName = "Fecha Solicitud";
                //DTexcelDatosClinicos.Columns[1].ColumnName = "Codigo Orden";
                //DTexcelDatosClinicos.Columns[2].ColumnName = "Documento Referencia";
                //DTexcelDatosClinicos.Columns[3].ColumnName = "Departamento EE.SS Origen";
                //DTexcelDatosClinicos.Columns[4].ColumnName = "Provincia EE.SS Origen";
                //DTexcelDatosClinicos.Columns[5].ColumnName = "Distrito EE.SS Origen";
                //DTexcelDatosClinicos.Columns[6].ColumnName = "Disa EE.SS Origen";
                //DTexcelDatosClinicos.Columns[7].ColumnName = "Red EE.SS Origen";
                //DTexcelDatosClinicos.Columns[8].ColumnName = "Micro Red EE.SS Origen";
                //DTexcelDatosClinicos.Columns[9].ColumnName = "EE.SS Origen";
                //DTexcelDatosClinicos.Columns[10].ColumnName = "Latitud";
                //DTexcelDatosClinicos.Columns[11].ColumnName = "Longitud";
                //DTexcelDatosClinicos.Columns[12].ColumnName = "Departamento EE.SS Destino";
                //DTexcelDatosClinicos.Columns[13].ColumnName = "Provincia EE.SS Destino";
                //DTexcelDatosClinicos.Columns[14].ColumnName = "Distrito EE.SS Destino";
                //DTexcelDatosClinicos.Columns[15].ColumnName = "Disa EE.SS Destino";
                //DTexcelDatosClinicos.Columns[16].ColumnName = "Red EE.SS Destino";
                //DTexcelDatosClinicos.Columns[17].ColumnName = "Micro Red EE.SS Destino";
                //DTexcelDatosClinicos.Columns[18].ColumnName = "EE.SS Destino";
                //DTexcelDatosClinicos.Columns[19].ColumnName = "Documento Identidad";
                //DTexcelDatosClinicos.Columns[20].ColumnName = "Fecha Nacimiento";
                //DTexcelDatosClinicos.Columns[21].ColumnName = "Paciente";
                //DTexcelDatosClinicos.Columns[22].ColumnName = "Codigo Muestra";
                //DTexcelDatosClinicos.Columns[23].ColumnName = "Tipo Muestra";
                //DTexcelDatosClinicos.Columns[24].ColumnName = "Enfermedad";
                //DTexcelDatosClinicos.Columns[25].ColumnName = "Examen";
                //DTexcelDatosClinicos.Columns[26].ColumnName = "Componente";
                //DTexcelDatosClinicos.Columns[27].ColumnName = "Resultado";
                //DTexcelDatosClinicos.Columns[28].ColumnName = "Muestra Conforme";
                //DTexcelDatosClinicos.Columns[29].ColumnName = "Criterios de Rechazo";
                //DTexcelDatosClinicos.Columns[30].ColumnName = "Observacion Rechazo";
                //DTexcelDatosClinicos.Columns[31].ColumnName = "Fecha Colección";
                //DTexcelDatosClinicos.Columns[32].ColumnName = "Fecha Recepción ROM";
                //DTexcelDatosClinicos.Columns[33].ColumnName = "Fecha Recepción LAB";
                //DTexcelDatosClinicos.Columns[34].ColumnName = "Fecha Verificación";
                //DTexcelDatosClinicos.Columns[35].ColumnName = "Fecha Agregar Exámen";
                //DTexcelDatosClinicos.Columns[36].ColumnName = "Estatus Resultados";
                //DTexcelDatosClinicos.Columns[37].ColumnName = "Motivo";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(DTexcelDatosClinicos);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    return new ExcelResult(wb, "ResultadoOnLineDatosClinicos");
                }
            }
            return null;
        }


        /// <summary>
        /// Descripción: Metodo para obtener y setear los datos de las disas, redes, microredes y establecimientos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectInstituciones(string hdnInstitucion, int idUsuario)
        {
            ViewBag.Locales = new Local { IdInstitucion = int.Parse(hdnInstitucion) };

            ViewBag.instituciones = Session["Instituciones"] as List<Institucion>;

            var local = new Local
            {
                IdInstitucion = int.Parse(hdnInstitucion),
                IdUsuario = idUsuario
            };

            var bl = new LocalBl();
            var disas = bl.GetDisas(local);

            if (disas.Count == 1)
            {
                var idDisa = disas.First().IdDISA;

                var redes = GetRedByDisa(hdnInstitucion, idDisa, idUsuario);

                if (redes.Count == 1)
                {
                    var idRed = redes.First().IdRed;

                    var microredes = GetMicroRedByRed(hdnInstitucion, idDisa, idRed + idDisa, idUsuario);

                    if (microredes.Count == 1)
                    {
                        var idMicroRed = microredes.First().IdMicroRed;

                        var establecimientos = GetEstablecimientoByMicroRed(hdnInstitucion, idDisa, idRed + idDisa, idMicroRed + idRed + idDisa, idUsuario);
                        SetEstablecimientoView(establecimientos);
                    }

                    SetMicroRedView(microredes);
                }

                SetRedView(redes);
            }

            SetDisaView(disas);

            return View("Index");
        }
        /// <summary>
        /// Descripción: Metodo para obtener y setear los datos de las disas y redes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectDisa(string hdnInstitucion, string hdnDisa, int idUsuario)
        {
            var redes = GetRedByDisa(hdnInstitucion, hdnDisa, idUsuario);

            SetRedView(redes);

            return View("Index");
        }
        /// <summary>
        /// Descripción: Metodo para obtener y setear los datos de las redes y microredes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectRed(string hdnInstitucion, string hdnDisa, string hdnRed, int idUsuario)
        {
            var microRedes = GetMicroRedByRed(hdnInstitucion, hdnDisa, hdnRed, idUsuario);

            SetMicroRedView(microRedes);

            return View("Index");
        }
        /// <summary>
        /// Descripción: Metodo para obtener y setear los datos de las microredes y establecimientos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectMicroRed(string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, int idUsuario)
        {
            var establecimientos = GetEstablecimientoByMicroRed(hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, idUsuario);

            SetEstablecimientoView(establecimientos);

            return View("Index");
        }
        /// <summary>
        /// Descripción: Metodo para obtener y setear los datos de las redes por disa seleccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private List<Red> GetRedByDisa(string idInstitucion, string idDisa, int idUsuario)
        {
            ViewBag.Locales = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdDisa = idDisa
            };

            ViewBag.instituciones = Session["Instituciones"] as List<Institucion>;
            ViewBag.disas = Session["disas"] as List<DISA>;

            var local = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdUsuario = idUsuario,
                IdDisa = idDisa
            };

            var bl = new LocalBl();
            return bl.GetRedes(local);
        }
        /// <summary>
        /// Descripción: Metodo para obtener y setear los datos de las microredes por red seleccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private List<MicroRed> GetMicroRedByRed(string idInstitucion, string idDisa, string idRed, int idUsuario)
        {
            ViewBag.Locales = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdDisa = idDisa,
                IdRed = idRed
            };

            ViewBag.instituciones = Session["Instituciones"];
            ViewBag.disas = Session["disas"];
            ViewBag.redes = Session["redes"];

            var local = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdUsuario = idUsuario,
                IdDisa = idDisa,
                IdRed = idRed.Length < 2 ? idRed : idRed.Substring(0, 2)
            };

            var bl = new LocalBl();
            return bl.GetMicroRedes(local);
        }
        /// <summary>
        /// Descripción: Metodo para obtener y setear los datos de los establecimientos por micro red seleccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicroRed"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private List<Establecimiento> GetEstablecimientoByMicroRed(string idInstitucion, string idDisa, string idRed, string idMicroRed, int idUsuario)
        {
            ViewBag.Locales = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdDisa = idDisa,
                IdRed = idRed,
                IdMicroRed = idMicroRed
            };

            ViewBag.instituciones = Session["Instituciones"];
            ViewBag.disas = Session["disas"];
            ViewBag.redes = Session["redes"];
            ViewBag.microredes = Session["microredes"];

            var local = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdUsuario = idUsuario,
                IdDisa = idDisa,
                IdRed = idRed.Length < 2 ? idRed : idRed.Substring(0, 2),
                IdMicroRed = idMicroRed.Length < 2 ? idMicroRed : idMicroRed.Substring(0, 2)
            };

            var bl = new LocalBl();
            return bl.GetEstablecimientos(local);
        }
        /// <summary>
        /// Descripción: Setear los datos de las disas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="disas"></param>
        private void SetDisaView(IList<DISA> disas)
        {
            Session["disas"] = disas;
            disas.Insert(0, new DISA { IdDISA = "-", NombreDISA = "Seleccione un elemento" });
            ViewBag.disas = disas;
        }
        /// <summary>
        /// Descripción: Setear los datos de las Redes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="redes"></param>
        private void SetRedView(IList<Red> redes)
        {
            Session["redes"] = redes;
            redes.Insert(0, new Red { IdRed = "-", NombreRed = "Seleccione una Red" });
            ViewBag.redes = redes;
        }
        /// <summary>
        /// Descripción: Setear los datos de las MicroRedes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="microRedes"></param>
        private void SetMicroRedView(IList<MicroRed> microRedes)
        {
            Session["microredes"] = microRedes;
            microRedes.Insert(0, new MicroRed { IdMicroRed = "-", NombreMicroRed = "Seleccione una Micro Red" });
            ViewBag.microredes = microRedes;
        }
        /// <summary>
        /// Descripción: Setear los datos de los establecimientos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="establecimientos"></param>
        private void SetEstablecimientoView(IList<Establecimiento> establecimientos)
        {
            //var loginEstablecimientoList = LoginEstablecimientoList(establecimientos);

            //Session["loginEstablecimientoList"] = loginEstablecimientoList;
            Session["EstablecimientosLogin"] = establecimientos;
            establecimientos.Insert(0,
                new Establecimiento { IdEstablecimiento = 0, Nombre = "Seleccione un Establecimiento", CodigoUnico = "00000" });
            ViewBag.establecimientos = establecimientos;
        }
        /// <summary>
        /// Descripción: Obtiene el establecimiento del Usuario Logeado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="establecimientos"></param>
        /// <returns></returns>
        private static List<Establecimiento> LoginEstablecimientoList(IEnumerable<Establecimiento> establecimientos)
        {
            var cacheEstablecimientos = StaticCache.GetEstablecimientoCache();

            if (cacheEstablecimientos == null)
                return new List<Establecimiento>();

            return cacheEstablecimientos.Where(x => establecimientos.Any(y => y.CodigoUnico == x.CodigoUnico)).ToList();
        }
        /// <summary>
        /// Descripción: Metodo para obtener información de consultas de resultados de un paciente.
        /// Autor: Marcos Mejia.
        /// Fecha Creacion: 15/07/2018
        public ActionResult ConsultaReporteResultado(string fechaDesde, string fechaHasta, string codigoOrden, string nroDocumento)
        {
            ConsultaResultadosBl bl = new ConsultaResultadosBl();
            List<OrdenIngresarResultadoVd> result = null;
            if (fechaDesde == null || fechaHasta == null)
            {
                return View("ConsultaReporteResultado");
            }
            result = bl.ConsultaReporteResultado(fechaDesde, fechaHasta, nroDocumento, codigoOrden, Logueado.idUsuario);
            var pageOfSeg = result.ToPagedList(1, GetSetting<int>(PageSize));
            return View("ConsultaReporteResultado", pageOfSeg);
        }

        public ActionResult ConsultaEnfermedadResultados()
        {
            return View();
        }
        public ActionResult ConsultaResultadosCovid()
        {
            OrdenDal dal = new OrdenDal();
            //List<string> resultado = new List<string>();
            string resultado = "";
            resultado = dal.GetOrdenesConsultaResultados();
            //DataTable dt = ConvertStringToDatatable(resultado);
            string[] fila = resultado.Split('$');

            return Export(fila);
        }

        public FileResult Export(string[] fila)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fila.Length; i++)
            {
                string[] customer = fila[i].Split('!');
                for (int j = 0; j < customer.Length; j++)
                {
                    //Append data with separator.
                    sb.Append(customer[j] + ",");
                }

                //Append new line character.
                sb.Append("\r\n");

            }
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Grid.csv");
        }


        public DataTable ConvertStringToDatatable(string datos)
        {
            DataTable dtExportData = new DataTable("dtExportData");
            dtExportData.TableName = "Base Resultados";
            dtExportData.Columns.Add("FechaSolicitud");
            dtExportData.Columns.Add("codigoOrden");
            dtExportData.Columns.Add("codificacion");
            dtExportData.Columns.Add("Oficio");
            dtExportData.Columns.Add("EESSDepOrigen");
            dtExportData.Columns.Add("EESSProvOrigen");
            dtExportData.Columns.Add("EESSDistOrigen");
            dtExportData.Columns.Add("EESSSubSector");
            dtExportData.Columns.Add("EESSDisaOrigen");
            dtExportData.Columns.Add("EESSRedOrigen");
            dtExportData.Columns.Add("EESSMicroRedOrigen");
            dtExportData.Columns.Add("EstablecimientoOrigen");
            dtExportData.Columns.Add("EESSDepDestino");
            dtExportData.Columns.Add("EESSProvDestino");
            dtExportData.Columns.Add("EESSDistDestino");
            dtExportData.Columns.Add("EESSDisaDestino");
            dtExportData.Columns.Add("EESSRedDestino");
            dtExportData.Columns.Add("EESSMicroRedDestino");
            dtExportData.Columns.Add("EESS_LAB_Destino");
            dtExportData.Columns.Add("DocIdentidad");
            dtExportData.Columns.Add("fechaNacimiento");
            dtExportData.Columns.Add("nombrePaciente");
            dtExportData.Columns.Add("edad");
            dtExportData.Columns.Add("SexoPaciente");
            dtExportData.Columns.Add("CodigoMuestra");
            dtExportData.Columns.Add("TipoMuestra");
            dtExportData.Columns.Add("Enfermedad");
            dtExportData.Columns.Add("nombreExamen");
            dtExportData.Columns.Add("Componente");
            dtExportData.Columns.Add("fechaRegistro");
            dtExportData.Columns.Add("FechaHoraColeccion");
            dtExportData.Columns.Add("FechaRecepcionROM");
            dtExportData.Columns.Add("FechaHoraRecepcionLAB");
            dtExportData.Columns.Add("FechaHoraValidado");
            dtExportData.Columns.Add("MuestraConforme");
            dtExportData.Columns.Add("CriteriosRechazo");
            dtExportData.Columns.Add("convResultado");
            dtExportData.Columns.Add("EstatusResultado");

            string[] fila = datos.Split('$');
            string[] columna;

            for (int i = 0; i < fila.Length; i++)
            {
                System.Data.DataRow row = dtExportData.NewRow();
                columna = fila[i].Split('!');
                for (int j = 0; j < columna.Length; j++)
                {
                    row[j] = columna[j].ToString();
                }
                dtExportData.Rows.Add(row);
            }

            //for (int i = 0; i < datos.Count; i++)
            //{
            //    DataRow row = dtExportData.NewRow();
            //    columna = datos[i].Split('!');
            //    for (int j = 0; j < columna.Length; j++)
            //    {
            //        row[j] = columna[j].ToString();
            //    }
            //    dtExportData.Rows.Add(row);
            //}

            return dtExportData;
        }

        public ActionResult ExportarExcelResultados(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Base Resultados";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        return new ExcelResult(wb, "Resultados");
                    }
                }
                else
                {
                    List<EnfermedadResultados> resultado = new List<EnfermedadResultados>();
                    if (this.Session["resultado"] != null)
                    {
                        resultado = (List<EnfermedadResultados>)this.Session["resultado"];
                    }
                    DataTable dtExportData = new DataTable("dtExportData");
                    dtExportData = ConvertToDataTable(resultado);
                    dtExportData.TableName = "Base Resultados";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dtExportData);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        return new ExcelResult(wb, "Resultados");
                    }
                }
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", " COnsultaResultadosController - ExportarExcelResultados ");
                throw ex;
            }
        }

        public ActionResult DatosNominalesCovid()
        {
            ListaBl listaBl = new ListaBl();
            List<SelectListItem> tipoDocumentoList = new List<SelectListItem>();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            ViewBag.tipoDocumentoList = tipoDocumentoList;

            ViewBag.esFechaRegistro = "1";

            ViewBag.esTipoUbigueo = "2";

            ViewBag.esTipoEstablecimiento = "1";

            ViewBag.IsSearch = false;

            var dal = new UsuarioDal();

            //Institucion-DISA-RED-MICRORED-EE.SS_LAB
            var laboratorios = dal.getLaboratoriosUsuario(Logueado.idUsuario);

            var local = new Local
            {
                IdUsuario = Logueado.idUsuario,
                TipoLocal = TipoLocal.Institucion
            };


            return View();
        }

        [AllowAnonymous]
        public ActionResult ObtenerDatosNominalesCovid(int esFechaRegistro, string fechaDesde, string fechaHasta, string codigoMuestra, string codigoOrden, string nroOficio)
        {
            try
            {
                ViewBag.IsSearch = true;
                initFormData();

                if (codigoMuestra == "")
                {
                    codigoMuestra = " ";
                }
                if (codigoOrden == "")
                {
                    codigoOrden = " ";
                }

                if (string.IsNullOrWhiteSpace(nroOficio))
                {
                    nroOficio = " ";
                }

                DateTime fechaDesdeA = DateTime.Parse("2016-01-01 00:00:00");
                DateTime fechaHastaA = DateTime.Now;

                var fechaDesdeCriteria = fechaDesde ?? string.Empty;
                var fechaHastaCriteria = fechaHasta ?? string.Empty;
                if (!fechaDesdeCriteria.Equals(""))
                    fechaDesdeA = DateTime.ParseExact(fechaDesdeCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (!fechaHastaCriteria.Equals(""))
                    fechaHastaA = DateTime.ParseExact(fechaHastaCriteria + " 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                ConsultaResultadosBl ordenBl = new ConsultaResultadosBl();
                List<OrdenIngresarResultadoVd> ordenes = new List<OrdenIngresarResultadoVd>();

                ordenes = ordenBl.ObtenerDatosNominalesCovidPorUsuario(Logueado.idUsuario, esFechaRegistro, fechaDesde, fechaHasta, codigoMuestra, codigoOrden, nroOficio);

                ViewBag.OrdenesList = ordenes;

                Session["CER_OrdenesDatosNominales"] = ordenes;

                ViewBag.tipoUsuario = Convert.ToString(Logueado.tipo);
                ViewBag.TotalOrdenes = ordenes.Count;
                int maximaCantidadFilas = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ConsultaOnlineVistaMaxRows"]) ? ordenes.Count : int.Parse(ConfigurationManager.AppSettings["ConsultaOnlineVistaMaxRows"]);
                return View("DatosNominalesCovid", ordenes.Take(maximaCantidadFilas));
            }
            catch (Exception ex)
            {
                string datos = string.Format("Logueado.idUsuario:{0}, esFechaRegistro:{1}, fechaDesde:{2}, fechaHasta:{3}, codigoMuestra:{4}, codigoOrden:{5}, nroOficio:{6}.", Logueado.idUsuario, esFechaRegistro, fechaDesde, fechaHasta, codigoMuestra, codigoOrden, nroOficio);
                new bsPage().LogError(ex, "LogNetLab", datos, " COnsultaResultadosController - ObtenerDatosNominalesCovid ");
                throw ex;
            }
        }

        public ActionResult ExportExcelNominalesCovid()
        {
            try
            {
                List<OrdenIngresarResultadoVd> ordenes = new List<OrdenIngresarResultadoVd>();

                if (this.Session["CER_OrdenesDatosNominales"] != null)
                {
                    ordenes = (List<OrdenIngresarResultadoVd>)this.Session["CER_OrdenesDatosNominales"];
                }

                DataTable dtExportData = new DataTable("dtExportData");
                dtExportData = ConvertToDataTable(ordenes);

                dtExportData.Columns.Remove("idOrden");
                dtExportData.Columns.Remove("idPaciente");
                dtExportData.Columns.Remove("idAnimal");
                dtExportData.Columns.Remove("idCBS");
                dtExportData.Columns.Remove("nombreEstablecimiento");
                dtExportData.Columns.Remove("nroOficio");
                dtExportData.Columns.Remove("codigoOrden");
                dtExportData.Columns.Remove("nroDocumento");
                dtExportData.Columns.Remove("tipoOrden");
                dtExportData.Columns.Remove("idEstablecimiento");
                dtExportData.Columns.Remove("estadoOrden");
                dtExportData.Columns.Remove("fechaNacimiento");
                dtExportData.Columns.Remove("fechaRegistro");
                dtExportData.Columns.Remove("fechaSolicitud");
                dtExportData.Columns.Remove("nombrePaciente");
                dtExportData.Columns.Remove("edadPaciente");
                dtExportData.Columns.Remove("edad");
                dtExportData.Columns.Remove("nombreProyecto");
                dtExportData.Columns.Remove("codigoPaciente");
                dtExportData.Columns.Remove("EdadAnios");
                dtExportData.Columns.Remove("Genero");
                dtExportData.Columns.Remove("tipoDocumento");
                dtExportData.Columns.Remove("nombreGenero");
                dtExportData.Columns.Remove("idExamen");
                dtExportData.Columns.Remove("nombreEnfermedad");
                dtExportData.Columns.Remove("nombreExamen");
                dtExportData.Columns.Remove("idLaboratorioProcesamiento");
                dtExportData.Columns.Remove("codigoMuestra");
                dtExportData.Columns.Remove("fechaRecepcion");
                dtExportData.Columns.Remove("fechaColeccion");
                dtExportData.Columns.Remove("metodoExamen");
                dtExportData.Columns.Remove("status");
                dtExportData.Columns.Remove("muestrasValidar");
                dtExportData.Columns.Remove("muestrasPendientesRecepcion");
                dtExportData.Columns.Remove("areas");
                dtExportData.Columns.Remove("step");
                dtExportData.Columns.Remove("ingresado");
                dtExportData.Columns.Remove("validado");
                dtExportData.Columns.Remove("liberado");
                dtExportData.Columns.Remove("statusP");
                dtExportData.Columns.Remove("conformeProceso");
                dtExportData.Columns.Remove("estatusE");
                dtExportData.Columns.Remove("ordenInfoListnew");
                dtExportData.Columns.Remove("resultadosList");
                dtExportData.Columns.Remove("MuestraConforme");
                dtExportData.Columns.Remove("flagResultado");
                dtExportData.Columns.Remove("conformeP");
                dtExportData.Columns.Remove("UbigeoActual");
                dtExportData.Columns.Remove("UbigeoReniec");
                dtExportData.Columns.Remove("conEstatusE");
                dtExportData.Columns.Remove("ConColor");
                dtExportData.Columns.Remove("ConFechAddExamen");
                dtExportData.Columns.Remove("error");
                dtExportData.Columns.Remove("dias");
                dtExportData.Columns.Remove("catalogo");
                dtExportData.Columns.Remove("idOrdenExamen");
                dtExportData.Columns.Remove("tipoExamen");
                dtExportData.Columns.Remove("FechaSiembraCultivo");
                dtExportData.Columns.Remove("ConDireccionPaciente");
                dtExportData.Columns.Remove("Telefono");
                dtExportData.Columns.Remove("ConnroOficio");
                dtExportData.Columns.Remove("ConEstablecimientoLatitud");
                dtExportData.Columns.Remove("ConEstablecimientoLongitud");
                dtExportData.Columns.Remove("ConDocIdentidad");
                dtExportData.Columns.Remove("ConnombrePaciente");
                dtExportData.Columns.Remove("ConfechaNacimiento");
                dtExportData.Columns.Remove("ConRedOrigen");
                dtExportData.Columns.Remove("ConMicroRedOrigen");
                dtExportData.Columns.Remove("ConDisaDestino");
                dtExportData.Columns.Remove("ConRedDestino");
                dtExportData.Columns.Remove("ConMicroRedDestino");
                dtExportData.Columns.Remove("ConComponente");
                dtExportData.Columns.Remove("ConHoraColeccion");
                //dtExportData.Columns.Remove("ConFechaHoraRecepcionInsROM");
                dtExportData.Columns.Remove("ConHoraRecepcionROM");
                dtExportData.Columns.Remove("ConHoraRecepcionLab");
                dtExportData.Columns.Remove("ConHoravalidado");
                dtExportData.Columns.Remove("HoraRegistroOrden");
                dtExportData.Columns.Remove("fechaRegistroRecepcionMuestra");
                dtExportData.Columns.Remove("HoraRegistroRecepcionMuestra");

                dtExportData.TableName = "Registros";
                dtExportData.Columns[0].ColumnName = "Fecha Solicitud";
                dtExportData.Columns[1].ColumnName = "Codigo Orden";
                dtExportData.Columns[2].ColumnName = "Departamento EE.SS Origen";
                dtExportData.Columns[3].ColumnName = "Provincia EE.SS Origen";
                dtExportData.Columns[4].ColumnName = "Distrito EE.SS Origen";
                dtExportData.Columns[5].ColumnName = "Disa EE.SS Origen";
                dtExportData.Columns[6].ColumnName = "Institucion Origen";
                dtExportData.Columns[7].ColumnName = "EE.SS Origen";
                dtExportData.Columns[8].ColumnName = "Departamento EE.SS Destino";
                dtExportData.Columns[9].ColumnName = "Provincia EE.SS Destino";
                dtExportData.Columns[10].ColumnName = "Distrito EE.SS Destino";
                dtExportData.Columns[11].ColumnName = "EE.SS Destino";
                dtExportData.Columns[12].ColumnName = "Codigo Muestra";
                dtExportData.Columns[13].ColumnName = "Tipo Muestra";
                dtExportData.Columns[14].ColumnName = "Enfermedad";
                dtExportData.Columns[15].ColumnName = "Nombre de Examen";
                dtExportData.Columns[16].ColumnName = "Resultado";
                dtExportData.Columns[17].ColumnName = "Muestra Conforme";
                dtExportData.Columns[18].ColumnName = "Criterios de Rechazo";
                dtExportData.Columns[19].ColumnName = "Observaciones de Rechazo";
                dtExportData.Columns[20].ColumnName = "Fecha Colección";
                dtExportData.Columns[21].ColumnName = "Fecha de Recepción ROM INS";
                dtExportData.Columns[22].ColumnName = "Fecha de Recepción";
                dtExportData.Columns[23].ColumnName = "Fecha Recepción LAB";
                dtExportData.Columns[24].ColumnName = "Fecha Verificación";
                dtExportData.Columns[25].ColumnName = "Fecha Registro de Orden";
                dtExportData.Columns[26].ColumnName = "Estatus Resultados";
                dtExportData.Columns[27].ColumnName = "Motivo";
                dtExportData.Columns[28].ColumnName = "Edad";
                dtExportData.Columns[29].ColumnName = "Genero";

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtExportData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    return new ExcelResult(wb, "DatosNominalesCovid19");
                }
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", " COnsultaResultadosController - ExportExcelNominalesCovid ");
                throw ex;
            }
        }

        public ActionResult ConsultaResultadosRecepcionINS()
        {
            var enfermerdad = new DataLayer.EnfermedadDal();
            var enfermedades = enfermerdad.GetEnfermedades();
            ViewBag.enfermedad = enfermedades;
            return View();
        }

        public ActionResult GetConsultaResultadosRecepcionINS(string idEnfermedad, string fechaDesde, string fechaHasta)
        {
            //List<ResultadosINS> ordenes = new List<ResultadosINS>();
            var bl = new ConsultaResultadosBl();
            DataTable dt = bl.GetConsultaResultadosRecepcionINS(idEnfermedad, fechaDesde, fechaHasta);
            //DataTable dtExportData = new DataTable("dtExportData");
            //dtExportData = ConvertToDataTable(ordenes);
            dt.TableName = "Datos Nominales Recepcion INS";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                return new ExcelResult(wb, "DatosNominalesINS");
            }
        }
    }
}
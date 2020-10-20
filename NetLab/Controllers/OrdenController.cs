using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using BusinessLayer;
using BusinessLayer.Interfaces;
using Model;
using Model.ViewData;
using PagedList;
using BusinessLayer.DatoClinico;
using BusinessLayer.Compartido;
using BusinessLayer.Plantilla;
using DataLayer;
using Enums;
using NetLab.Helpers;
using BusinessLayer.Compartido.Enums;
using NetLab.Models;
using NetLab.Models.Shared;
using Rotativa;
using Utilitario;
using Newtonsoft.Json;
using System.Web.UI;
using NetLab.App_Code;
using System.Configuration;

namespace NetLab.Controllers
{
    public partial class OrdenController : ParentController
    {
        #region busqueda
        /// <summary>
        /// Descripción: Controlador que realiza el llamado del popup _FormPopupCodigoMuestra
        /// permitiendo ver el codigo de muestra y la impresion el codigo QR.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="codificacion"></param>
        /// <returns></returns>
        public ActionResult ShowPopupCodigoMuestra(string codificacion)
        {
            var muestraCodificacion = new MuestraCodificacion { codificacion = codificacion };
            return PartialView("_FormPopupCodigoMuestra", muestraCodificacion);
        }

        /// <summary>
        /// Descripción: Controlador que realiza el llamado del popup _FormSeleccionarPlantilla
        /// permitiendo seleccionar una de las plantillas creadas para una enfermedad
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nombre"></param>
        /// <param name="tipoRegistro"></param>
        /// <returns></returns>
        public ActionResult ShowSelectorPlantilla(string idPaciente, string nroDocumento, string nombre, int? tipoRegistro)
        {
            ViewBag.idPaciente = idPaciente;
            ViewBag.NroDocumento = nroDocumento;
            ViewBag.Nombre = nombre;


            if (tipoRegistro == 1)
            {
                this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.ORDEN_RECEPCION;
            }
            else
            {
                this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.SOLO_ORDEN_MUESTRA;
            }
            //Session["tipoRegistro"] = tipoRegistro == null ? TipoRegistroOrden.SOLO_ORDEN : TipoRegistroOrden.ORDEN_RECEPCION;
            //this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.SOLO_ORDEN_MUESTRA;

            var plantillaList = new List<Plantilla>();
            var establecimiento = Session["establecimientoSeleccionado"] as Establecimiento;
            Session["establecimientoActual"] = establecimiento;
            IPlantillaBl plantillaBl = new PlantillaBl();

            if (establecimiento == null)
                return PartialView("_FormSeleccionarPlantilla", plantillaList);

            plantillaList = plantillaBl.GetPlantillaByEstablecimiento(establecimiento.IdEstablecimiento);
            return PartialView("_FormSeleccionarPlantilla", plantillaList);
        }

        /// <summary>
        /// Descripción: Controlador que inicializa los campos, cargando la información pre establecida (Memoria).
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nombre"></param>
        /// <param name="idPlantilla"></param>
        /// <param name="tipoRegistro"></param>
        /// <returns></returns>
        public ActionResult NewOrdenPlantilla(string idPaciente, string nroDocumento, string nombre, int idPlantilla, int? tipoRegistro)
        {
            DelSessionOrden();
            Session["idPaciente"] = idPaciente;
            var ordenBl = new OrdenBl();
            var establecimiento = (Establecimiento)Session["establecimientoActual"];

            var orden = ordenBl.GetOrdenByIdPlantilla(idPlantilla, establecimiento.IdEstablecimiento, Guid.Parse(idPaciente));

            ViewBag.OrdenExistente = false;
            //if (Session["ordenExamenListAgregados"] != null)
            //{
            //    ViewBag.OrdenExistente = true;
            //}

            ViewBag.tipoRegistro = (TipoRegistroOrden)Session["tipoRegistro"];

            var listaBl = new ListaBl();

            var seguroList = listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoSeguroSalud).
                ListaDetalle.Select(itemDetalle =>
                new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) }).ToList();

            ViewBag.seguroList = seguroList;

            var establecimientoList = new List<SelectListItem> { new SelectListItem { Text = "", Value = "" } };
            ViewBag.establecimientoList = establecimientoList;

            IProyectoBl proyectoBl = new ProyectoBl();
            ViewBag.proyectoList = proyectoBl.GetProyectos();

            var establecimientoBl = new EstablecimientoBl();
            var establecimientosFrecuentes = establecimientoBl.GetEstablecimientosFrecuentesByIdUsuario(((Usuario)Session["UsuarioLogin"]).idUsuario);
            establecimientosFrecuentes.Add(new Establecimiento { IdEstablecimiento = 0, Nombre = "Seleccione el Establecimiento" });

            ViewBag.establecimientoListFrecuentes = establecimientosFrecuentes.OrderBy(x => x.IdEstablecimiento).ToList();

            Session["ordenExamenListAgregados"] = orden.ordenExamenList;
            Session["ordenMuestraListAgregados"] = orden.ordenMuestraList;

            foreach (var ordenMaterial in orden.ordenMaterialList)
            {
                ordenMaterial.ordenMuestraRecepcionList = new List<OrdenMuestraRecepcion>();

                if ((TipoRegistroOrden)Session["tipoRegistro"] == TipoRegistroOrden.SOLO_ORDEN || (TipoRegistroOrden)Session["tipoRegistro"] == TipoRegistroOrden.SOLO_ORDEN_MUESTRA) continue;

                for (var i = 0; i < ordenMaterial.cantidad; i++)
                {
                    var ordenMuestraRecepcion = new OrdenMuestraRecepcion
                    {
                        conforme = 1,
                        idOrdenMuestraRecepcion = Guid.NewGuid()
                    };

                    var criterioRechazoBl = new CriterioRechazoBl();
                    ordenMuestraRecepcion.criterioRechazoList =
                        criterioRechazoBl.GetCriteriosByTipoMuestra(ordenMaterial.Material.TipoMuestra.idTipoMuestra, 2);

                    foreach (var criterioRechazo in ordenMuestraRecepcion.criterioRechazoList)
                        criterioRechazo.rechazado = false;

                    ordenMaterial.ordenMuestraRecepcionList.Add(ordenMuestraRecepcion);
                }
            }

            Session["ordenMaterialListAgregados"] = orden.ordenMaterialList;
            Session["enfermedadListAgregados"] = orden.enfermedadList;
            Session["enfermedadList"] = orden.enfermedadList;

            CargaUbigeoEstablecimiento();
            getDatosFromSession(orden);

            //ViewBag.idEstablecimiento = orden.establecimiento.IdEstablecimiento;
            //ViewBag.nombreEstablecimiento = orden.establecimiento.Nombre;

            orden.fechaSolicitud = DateTime.Now;
            //LoadSolicitantes();
            return View("New", orden);
        }
        /// <summary>
        /// Descripción: Metodo que devuelve el codigo del establecimiento y la descripcion.
        /// Author: Juan Muga.
        /// Fecha Creacion: 08/08/2018
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        public JsonResult GetEstablecimientosNew(string Prefix, string tipo, string Departamento = null, string Provincia = null, string Distrito = null)
        {
            try
            {
                var establecimientoList = new List<Establecimiento>();
                //var establecimientoBl = new EstablecimientoBl();
                var ubigeo = Departamento + Provincia + Distrito;
                //var listaEstablecimientos = Session["loginEstablecimientoList"] as List<Establecimiento>;
                //var establecimientoList = establecimientoBl.GetEstablecimientosByTextoBusqueda(Prefix, ((Usuario)Session["UsuarioLogin"]).idUsuario, listaEstablecimientos).FindAll(p => p.IdLabIns != 1 && p.Ubigeo.Contains(ubigeo) ).ToList();
                //var establecimientoList = establecimientoBl.GetEstablecimientosByTextoBusqueda(Prefix, Convert.ToInt16(tipo)).FindAll(p => p.IdLabIns != 1 && p.Ubigeo.Contains(ubigeo)).ToList();

                var laboratorioList = StaticCache.ObtenerLaboratorios();
                if (laboratorioList.Any())
                {
                    var laboratorioEnumerable = laboratorioList.Where(x => x.IdLabIns != 1);
                    if (!string.IsNullOrWhiteSpace(ubigeo))
                    {
                        laboratorioEnumerable = laboratorioList.Where(x => x.Ubigeo.Contains(ubigeo));
                    }
                    
                    if (!string.IsNullOrWhiteSpace(Prefix))
                    {
                        laboratorioEnumerable = laboratorioEnumerable.Where(x => x.NombreEstablecimiento.ToLower().Contains(Prefix.Trim().ToLower()) || x.CodigoUnico.ToLower().Contains(Prefix.Trim().ToLower()));
                    }

                    if (!string.IsNullOrWhiteSpace(tipo))
                    {
                        laboratorioEnumerable = tipo.Trim() == "1" ? laboratorioEnumerable.Where(x => x.IdLabIns == 0 || x.IdLabIns == 2) : laboratorioEnumerable.Where(x => x.IdLabIns.HasValue);
                    }

                    laboratorioList = laboratorioEnumerable.ToList();
                    laboratorioList.ForEach(l =>
                    {
                        establecimientoList.Add(new Establecimiento
                        {
                            IdEstablecimiento = l.IdEstablecimiento,
                            Nombre = l.Nombre
                        });
                    });
                }

                return Json(establecimientoList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "Orden GetEstablecimientosNew", "");
                return Json(new List<Establecimiento>(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetEstablecimientosNew2(string Prefix, string tipo)
        {
            var establecimientoBl = new EstablecimientoBl();
            //var listaEstablecimientos = Session["loginEstablecimientoList"] as List<Establecimiento>;
            var establecimientoList = establecimientoBl.GetEstablecimientosByTextoBusqueda(Prefix, Convert.ToInt16(tipo)).FindAll(p => p.IdLabIns != 1).ToList();
            return Json(establecimientoList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Descripción: Metodo que devuelve el codigo del establecimiento y la descripcion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public string GetEstablecimientos(string dato)
        {
            var data = Request.Params["data[q]"];

            var establecimientoBl = new EstablecimientoBl();
            //var listaEstablecimientos = Session["loginEstablecimientoList"] as List<Establecimiento>;
            var establecimientoList = establecimientoBl.GetEstablecimientosByTextoBusqueda(data, ((Usuario)Session["UsuarioLogin"]).idUsuario);

            Session["establecimientoList"] = establecimientoList;

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeEstablecimiento = false;

            if (dato == "1")
            {
                foreach (var establecimiento in establecimientoList)
                {
                    //TODO: YRVING LIMPIAR ESTO
                    //String nombre = establecimiento.nombre + " - Departamento: " + establecimiento.departamento +
                    //    " - Provincia: " + establecimiento.provincia + " - Distrito: " + establecimiento.distrito;

                    var nombre = establecimiento.Nombre.Replace("\"", "");

                    resultado += "{\"id\":\"" + establecimiento.IdEstablecimiento + "\",\"text\":\"" + nombre + "\"},";
                    existeEstablecimiento = true;
                }

            }
            else
            {
                foreach (var establecimiento in establecimientoList.FindAll(p => p.IdLabIns != 1).ToList())
                {
                    //TODO: YRVING LIMPIAR ESTO
                    //String nombre = establecimiento.nombre + " - Departamento: " + establecimiento.departamento +
                    //    " - Provincia: " + establecimiento.provincia + " - Distrito: " + establecimiento.distrito;

                    var nombre = establecimiento.Nombre.Replace("\"", "");

                    resultado += "{\"id\":\"" + establecimiento.IdEstablecimiento + "\",\"text\":\"" + nombre + "\"},";
                    existeEstablecimiento = true;
                }

            }

            if (existeEstablecimiento)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }
        public JsonResult GetLaboratoriosNew(string Prefix, string ExamenVA)
        {
           var listaEstablecimientos = new List<Establecimiento>();

            var laboratorioBl = new LaboratorioBl();
            //var laboratorioList = laboratorioBl.GetLaboratoriosByTextoBusqueda(Prefix, ((Usuario)Session["UsuarioLogin"]).idUsuario);
            var laboratorioList = StaticCache.ObtenerLaboratorios();
            //var test = laboratorioList.Where(x => x.CodigoUnico.Contains("0005617")).ToList();
            laboratorioList = string.IsNullOrWhiteSpace(Prefix)? laboratorioList : laboratorioList.Where(x => x.Nombre.ToLower().Contains(Prefix.Trim().ToLower()) || x.CodigoUnico.ToLower().Contains(Prefix.Trim().ToLower())).ToList();
            //var laboratorioList = laboratorioBl.GetLaboratoriosByTextoBusqueda(Prefix, 1);

            if (ExamenVA.ToUpper() == "VALIDADOR" || ExamenVA.ToUpper() == "RomINS")
            {
                Session["tipoRegistro"] = TipoRegistroOrden.SOLO_ORDEN;
                var ClasificacionEESS = EstablecimientoSeleccionado.clasificacion.ToString();
                if (ClasificacionEESS.TrimEnd().ToString().Contains("LAB INS"))
                {
                    Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                    laboratorioList = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                }
                else
                {
                    Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                    laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                }
                return Json(laboratorioList, JsonRequestBehavior.AllowGet);
            }
            if ((TipoRegistroOrden)Session["tipoRegistro"] == TipoRegistroOrden.ORDEN_RECEPCION && (EstablecimientoSeleccionado.IdEstablecimiento == 991 || EstablecimientoSeleccionado.IdEstablecimiento == 23638))
            {
                Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                laboratorioList = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                return Json(laboratorioList, JsonRequestBehavior.AllowGet);
            }
            if ((TipoRegistroOrden)Session["tipoRegistro"] == TipoRegistroOrden.SOLO_ORDEN_MUESTRA)
            {
                if (ExamenVA.ToUpper() == "VALIDADOR")
                {
                    var ClasificacionEESS = EstablecimientoSeleccionado.clasificacion.ToString();
                    if (ClasificacionEESS.TrimEnd().ToString().Contains("LAB INS"))
                    {
                        Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                        laboratorioList = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                    }
                    else
                    {
                        Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                        laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                    }
                }
                else
                {
                    Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                    laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                }

            }
            else
            {
                Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
            }

            return Json(laboratorioList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Descripción: Metodo que devuelve el codigo del laboratorio y la descripcion.
        /// Enlazado con el metodo GetLaboratoriosForMaterial
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public string GetLaboratorios(string ExamenVA)
        {
            return GetLaboratoriosForMaterial(null, 0, ExamenVA);
        }

        public string GetLaboratoriosForMaterial(string idEnfermedadExamen, int idTipoMuestraMaterial, string ExamenVA)
        {
            var data = Request.Params["data[q]"];
            ILaboratorioBl laboratorioBl = new LaboratorioBl();
            var resultado = "{\"q\":\"" + data + "\",\"results\":[";
            List<LaboratorioVMSelect> laboratorioList = new List<LaboratorioVMSelect>();
            try
            {
                if (idEnfermedadExamen != null)
                {
                    var idEnfermedadExamenArray = idEnfermedadExamen.Split('_');
                    var idExamen = Guid.Parse(idEnfermedadExamenArray[1]);

                    laboratorioList = laboratorioBl.GetLaboratoriosByTextoBusqueda(data, ((Usuario)Session["UsuarioLogin"]).idUsuario, idExamen);
                }
                else
                {
                    //laboratorioList = laboratorioBl.GetLaboratoriosByTextoBusqueda(data, ((Usuario)Session["UsuarioLogin"]).idUsuario);
                    laboratorioList = StaticCache.ObtenerLaboratorios();
                    laboratorioList = string.IsNullOrWhiteSpace(data)? laboratorioList : laboratorioList.Where(x => x.NombreEstablecimiento.ToLower().Contains(data.Trim().ToLower())).ToList();
                }

                if (ExamenVA.ToUpper() == "VALIDADOR")
                {
                    Session["tipoRegistro"] = TipoRegistroOrden.SOLO_ORDEN;
                }
                if ((TipoRegistroOrden)Session["tipoRegistro"] == TipoRegistroOrden.ORDEN_RECEPCION)
                {
                    Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                    laboratorioList = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                }
                if ((TipoRegistroOrden)Session["tipoRegistro"] == TipoRegistroOrden.SOLO_ORDEN_MUESTRA)
                {
                    if (ExamenVA.ToUpper() == "VALIDADOR")
                    {
                        var ClasificacionEESS = EstablecimientoSeleccionado.clasificacion.ToString();
                        if (ClasificacionEESS.TrimEnd().ToString().Contains("LAB INS"))
                        {
                            Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                            laboratorioList = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                        }
                        else
                        {
                            Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                            laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                        }
                    }
                    else
                    {
                        Session["laboratorioList"] = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                        laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                    }

                }



                var existeLaboratorio = false;
                foreach (var laboratorio in laboratorioList)
                {
                    var nombre = laboratorio.Direccion.Replace("\"", "");//laboratorio.Nombre.Replace("\"", "") + laboratorio.Ubigeo.Replace("\"", "") + laboratorio.Direccion.Replace("\"", "");
                    resultado += "{\"id\":\"" + laboratorio.IdEstablecimiento + "\",\"text\":\"" + nombre + "\"},";
                    existeLaboratorio = true;
                }

                if (existeLaboratorio)
                    resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
                else
                    resultado = resultado.Substring(0, resultado.Length) + "]}";

            }
            catch (Exception ex)
            {


            }
            finally
            {

            }
            return resultado;

        }

        /// <summary>
        /// Descripción: Metodo que devuelve el codigo de la enfermedad y la descripcion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public string GetEnfermedades()
        {
            var data = Request.Params["data[q]"];

            IEnfermedadBl enfermedadBl = new EnfermedadBl();
            var enfermedadList = enfermedadBl.GetEnfermedadesByNombre(data);

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeEnfermedad = false;

            foreach (var enfermedad in enfermedadList)
            {
                var text = enfermedad.nombre;
                var snomed = enfermedad.Snomed;

                //  if (snomed != null && snomed.Contains(data))
                text = snomed + " -  " + text;

                resultado += "{\"id\":\"" + enfermedad.idEnfermedad + "\",\"text\":\"" + text + "\"},";
                existeEnfermedad = true;
            }

            if (existeEnfermedad)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            Session["enfermedadList"] = enfermedadList;

            return resultado;
        }
        /// <summary>
        /// Descripción: Analista - Metodo que devuelve el codigo de la enfermedad y la descripcion.
        /// Author: Juan Muga
        /// Fecha Creacion: 06/11/2018
        /// </summary>
        /// <returns></returns>
        public string GetEnfermedadesAnalista(string idOrden)
        {
            var data = Request.Params["data[q]"];

            IEnfermedadBl enfermedadBl = new EnfermedadBl();
            var enfermedadList = enfermedadBl.GetEnfermedadesByNombre(data, idOrden);

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeEnfermedad = false;

            foreach (var enfermedad in enfermedadList)
            {
                var text = enfermedad.nombre;
                var snomed = enfermedad.Snomed;

                //  if (snomed != null && snomed.Contains(data))
                text = snomed + " -  " + text;

                resultado += "{\"id\":\"" + enfermedad.idEnfermedad + "\",\"text\":\"" + text + "\"},";
                existeEnfermedad = true;
            }

            if (existeEnfermedad)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            Session["enfermedadList"] = enfermedadList;

            return resultado;
        }
        /// <summary>
        /// Descripción: Metodo que devuelve enfermedades que estan enlazadas a un laboratorio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public string GetEnfermedadesByLaboratorio(int idLaboratorio)
        {
            var data = Request.Params["data[q]"];

            var enfermedadBl = new EnfermedadBl();
            var enfermedadList = enfermedadBl.GetEnfermedadesByNombre(data, idLaboratorio, Logueado.idUsuario);

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeEnfermedad = false;
            foreach (var enfermedad in enfermedadList)
            {
                resultado += "{\"id\":\"" + enfermedad.idEnfermedad + "\",\"text\":\"" + enfermedad.nombre + "\"},";
                existeEnfermedad = true;
            }

            if (existeEnfermedad)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";
            Session["enfermedadList"] = enfermedadList;
            return resultado;

        }
        #endregion

        #region AgregarExamen
        /// <summary>
        /// Descripción: Controlador que agrega en memoria las enfermedades, muestras y materiales en la pantalla de ingreso de orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLaboratorio"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="idExamen"></param>
        /// <param name="idTipoMuestra"></param>
        /// <param name="fechaColeccion"></param>
        /// <param name="horaColeccion"></param>
        /// <param name="idMaterial"></param>
        /// <param name="noPrecisaVolumen"></param>
        /// <param name="volumen"></param>
        /// <param name="cantidad"></param>
        /// <param name="tipoMaterial"></param>
        /// <param name="fechaEnvio"></param>
        /// <param name="horaEnvio"></param>
        /// <returns></returns>
        public ActionResult AddExamen(int idLaboratorio, int idEnfermedad, string idExamen, int idTipoMuestra, string fechaColeccion,
                                      string horaColeccion, int idMaterial, bool noPrecisaVolumen, decimal volumen, int cantidad, int tipoMaterial,
                                      string fechaEnvio, string horaEnvio, int IdProyecto)
        {

            //ENFERMEDADES AGREGADAS
            var enfermedadListAgregados = new List<Enfermedad>();
            if (Session["enfermedadListAgregados"] != null)
                enfermedadListAgregados = (List<Enfermedad>)Session["enfermedadListAgregados"];

            //EXAMENES AGREGADOS
            var ordenExamenListAgregados = new List<OrdenExamen>();
            if (Session["ordenExamenListAgregados"] != null)
                ordenExamenListAgregados = (List<OrdenExamen>)Session["ordenExamenListAgregados"];

            //MATERIALES AGREGADOS
            var ordenMaterialListAgregados = new List<OrdenMaterial>();
            if (Session["ordenMaterialListAgregados"] != null)
                ordenMaterialListAgregados = (List<OrdenMaterial>)Session["ordenMaterialListAgregados"];

            //ORDENMUESTRA AGREGADOS
            var ordenMuestraListAgregados = new List<OrdenMuestra>();
            if (Session["ordenMuestraListAgregados"] != null)
                ordenMuestraListAgregados = (List<OrdenMuestra>)Session["ordenMuestraListAgregados"];

            var laboratorioBl = new LaboratorioBl();
            var laboratorioList = StaticCache.ObtenerLaboratorios();
            


            //LABORATORIO
            if ((List<LaboratorioVMSelect>)Session["laboratorioList"] == null)
            {
                laboratorioList = laboratorioList.Where(x => x.IdEstablecimiento == idLaboratorio).ToList();
            }
            else
            {
                laboratorioList = (List<LaboratorioVMSelect>)Session["laboratorioList"];
            }
            var laboratoriovm = laboratorioList.FirstOrDefault(x => x.IdEstablecimiento == Convert.ToInt32(idLaboratorio));
            Laboratorio laboratorio = new Laboratorio
            {
                IdLaboratorio = laboratoriovm.IdEstablecimiento,
                IdLabIns = laboratoriovm.IdLabIns.Value,
                clasificacion = laboratoriovm.clasificacion,
                CodigoUnico = laboratoriovm.CodigoUnico,
                Direccion = laboratoriovm.Direccion,
                //Nombre = laboratoriovm.NombreEstablecimiento,
                Nombre = laboratoriovm.Nombre,
                Ubigeo = laboratoriovm.Ubigeo
            };
            //Session["laboratorioSeleccionado"] = laboratorio;

            //ENFERMEDAD
            var enfermedadList = (List<Enfermedad>)Session["enfermedadList"];
            var enfermedad = enfermedadList.FirstOrDefault(x => x.idEnfermedad == idEnfermedad);

            //LISTA DE EXAMENES
            var examenList = (List<Examen>)Session["examenList"];

            //LISTA DE MUESTRAS
            var tipoMuestraList = (List<TipoMuestra>)Session["tipoMuestraList"];

            //LISTA DE MATERIALES
            var materialList = (List<Material>)Session["materialList"];


            //Si la enfermedad NO existe en la lista de agregados se agrega => SE AGREGA
            if (enfermedadListAgregados.FirstOrDefault(x => x.idEnfermedad == idEnfermedad) == null)
            {
                var datoClinicoBl = new DatoClinicoBl();
                var categoriaDatoList = datoClinicoBl.GetCategoriaByEnfermedad(idEnfermedad, IdProyecto, idExamen);

                if (enfermedad != null)
                {
                    enfermedad.categoriaDatoList = categoriaDatoList;
                    enfermedadListAgregados.Add(enfermedad);
                }

                Session["enfermedadListAgregados"] = enfermedadListAgregados;
            }

            var existeEnfermedad = false;
            var existeMuestra = false;
            var duplicado = false;

            //Se busca la enfermedad con el examen, si no existe => SE AGREGA
            var ordenExamen = ordenExamenListAgregados.FirstOrDefault(x => x.Examen.idExamen == Guid.Parse(idExamen) &&
                x.Enfermedad.idEnfermedad == idEnfermedad &&
                x.IdTipoMuestra == idTipoMuestra);

            if (ordenExamen == null)
            {
                ordenExamen = new OrdenExamen
                {
                    Enfermedad = enfermedad,
                    Examen = examenList.FirstOrDefault(x => x.idExamen == Guid.Parse(idExamen)),
                    IdTipoMuestra = idTipoMuestra,
                    ordenMuestraList = new List<OrdenMuestra>()
                };

                var tipoMuestra = tipoMuestraList.FirstOrDefault(x => x.idTipoMuestra == Convert.ToInt32(idTipoMuestra));
                if (tipoMuestra == null || (tipoMuestra != null && tipoMuestra.idTipoMuestra == 0)) tipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra };
                //Se busca la muestra si no existe se agrega
                var ordenMuestra = ordenMuestraListAgregados.FirstOrDefault(x => x.TipoMuestra.idTipoMuestra == Convert.ToInt32(idTipoMuestra));
                //SOTERO BUSTAMANTE: OBTENER NUMERO DE MUESTRA POR PACIENTE  SEGUN ENFERMEDAD Y TIPODE MUESTRA


                var ordenDal = new OrdenDal();
                var nroMuestra = 0;
                string idPaciente = Session["idPaciente"].ToString();
                nroMuestra = ordenDal.GetNumeroExamenByMuestra(Guid.Parse(idPaciente), idEnfermedad, idTipoMuestra, Guid.Parse(idExamen));

                /***********************************************************************************************/
                if (ordenMuestra == null)
                {
                    ordenMuestra = new OrdenMuestra
                    {

                        idOrdenMuestra = Guid.NewGuid(),
                        MuestraCodificacion = new MuestraCodificacion(),
                        TipoMuestra = tipoMuestra,
                        idTipoMuestra = idTipoMuestra,
                        ordenExamenList = new List<OrdenExamen>()
                    };

                    var fecha = fechaColeccion.Split('/');
                    ordenMuestra.fechaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));
                    var hora = horaColeccion.Split(':');
                    ordenMuestra.horaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);

                    ordenMuestraListAgregados.Add(ordenMuestra);
                }
                else
                {
                    existeMuestra = true;
                }

                //La nueva muestra o la muestra existente se asigna al examen en caso no exista en el examen
                if (!ordenExamen.ordenMuestraList.Any(x => x.TipoMuestra.idTipoMuestra == idTipoMuestra))
                {
                    ordenExamen.ordenMuestraList.Add(ordenMuestra);
                }

                //El nuevo examen se agrega a la lista de examenes
                ordenExamenListAgregados.Add(ordenExamen);
                ordenMuestra.ordenExamenList.Add(ordenExamen);

                //Como es una nueva enfermedad examen se agrega el material
                //idMaterial = 0 está mal, debe cargar el valor x default
                //var material = materialList.FirstOrDefault(x => x.idMaterial == Convert.ToInt32(idMaterial));
                var material = materialList.FirstOrDefault();

                ordenMaterialListAgregados = CreateObjectOrdenMaterial(cantidad, noPrecisaVolumen, volumen, tipoMaterial, material,
                    fechaEnvio, horaEnvio, ordenMuestra, laboratorio, ordenExamen, ordenMaterialListAgregados, nroMuestra);
            }
            else //Si la ordenExamen existe entonces se verifica si se puede agregar la ordenMuestra
            {
                existeEnfermedad = true;

                var ordenMuestra = ordenMuestraListAgregados.FirstOrDefault(x => x.TipoMuestra.idTipoMuestra == Convert.ToInt32(idTipoMuestra));

                if (ordenMuestra == null)
                {
                    var tipoMuestra = tipoMuestraList.FirstOrDefault(x => x.idTipoMuestra == Convert.ToInt32(idTipoMuestra));
                    if (tipoMuestra == null || (tipoMuestra!=null && tipoMuestra.idTipoMuestra == 0)) tipoMuestra = new TipoMuestra { idTipoMuestra = idTipoMuestra };
                    ordenMuestra = new OrdenMuestra
                    {
                        idOrdenMuestra = Guid.NewGuid(),
                        TipoMuestra = tipoMuestra,
                        MuestraCodificacion = new MuestraCodificacion(),
                        ordenExamenList = new List<OrdenExamen>(),
                        idTipoMuestra = idTipoMuestra
                    };

                    var fecha = fechaColeccion.Split('/');
                    ordenMuestra.fechaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));
                    var hora = horaColeccion.Split(':');
                    ordenMuestra.horaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);

                    ordenMuestraListAgregados.Add(ordenMuestra);
                }
                else
                {
                    existeMuestra = true;
                }

                //La nueva muestra o la muestra existente se asigna al examen en caso no exista en el examen
                if (ordenExamen.ordenMuestraList.FirstOrDefault(x => x.TipoMuestra.idTipoMuestra == idTipoMuestra) == null)
                {
                    ordenExamen.ordenMuestraList.Add(ordenMuestra);
                }

                ordenMuestra.ordenExamenList.Add(ordenExamen);

                var ordenMaterial = ordenMaterialListAgregados.FirstOrDefault(x => x.Material.idMaterial == idMaterial
                    && x.Material.TipoMuestra.idTipoMuestra == idTipoMuestra
                    //&& x.Material.IdTipoMuestra == idTipoMuestra
                    && x.Laboratorio.IdLaboratorio == idLaboratorio
                    && x.OrdenExamen.Enfermedad.idEnfermedad == idEnfermedad
                    && x.OrdenExamen.Examen.idExamen == Guid.Parse(idExamen));

                //Si OrdenMaterial existe, no se puede agregar porque ya existiria el examen, la enfermedad y el material
                if (ordenMaterial == null)
                {
                    var material = materialList.FirstOrDefault(x => x.idMaterial == Convert.ToInt32(idMaterial));
                    var ordenMuestraTmp = ordenMuestraListAgregados.FirstOrDefault(x => x.idTipoMuestra == Convert.ToInt32(idTipoMuestra));

                    ordenMaterialListAgregados = CreateObjectOrdenMaterial(cantidad, noPrecisaVolumen, volumen, tipoMaterial,
                        material, fechaEnvio, horaEnvio, ordenMuestraTmp, laboratorio, ordenExamen, ordenMaterialListAgregados, 0);
                }
                else
                {
                    duplicado = true;
                }
            }

            Session["ordenMuestraListAgregados"] = ordenMuestraListAgregados;
            Session["ordenExamenListAgregados"] = ordenExamenListAgregados;

            var ordenTmp = new Orden
            {
                ordenExamenList =
                    !existeEnfermedad
                        ? new List<OrdenExamen> { ordenExamenListAgregados.Last() }
                        : new List<OrdenExamen>(),
                ordenMuestraList =
                    !existeMuestra
                        ? new List<OrdenMuestra> { ordenMuestraListAgregados.Last() }
                        : new List<OrdenMuestra>(),
                ordenMaterialList =
                    !duplicado ? new List<OrdenMaterial> { ordenMaterialListAgregados.Last() } : new List<OrdenMaterial>()
            };

            //Si esta duplicado
            if (duplicado) return PartialView("_NoAgregado");

            var tipoRegistro = (TipoRegistroOrden)Session["tipoRegistro"];
            var partialview = "";
            switch (tipoRegistro)
            {
                case TipoRegistroOrden.SOLO_ORDEN:
                    partialview = "_TblMultiple";
                    break;
                case TipoRegistroOrden.ORDEN_RECEPCION:
                    partialview = "_TblMultipleRecepcionar";
                    break;
                case TipoRegistroOrden.SOLO_ORDEN_MUESTRA:
                    partialview = "_TblMultipleManual";
                    break;
                default:
                    break;
            }
            var xDatosOrdenExamenSesison = new DatosOrdenExamenSesison {
                ordenMuestra = ordenMuestraListAgregados.Last(),
                ordenMaterial = ordenMaterialListAgregados.Last(),
                ordenExamen = ordenExamenListAgregados.Last(),
                idUsuarioLogin = Logueado.idUsuario
            };

            bool usarstaticcache = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["usarstaticcache"], out usarstaticcache);
            if(usarstaticcache) StaticCache.CargarDatosOrdenExamen(xDatosOrdenExamenSesison);

            return PartialView(partialview, ordenTmp);
        }
        /// <summary>
        /// Descripción: Controlador que agrega Examen, muestras y materiales en la pantalla del VERIFICADOR.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 11/11/2017
        /// Fecha Modificación: 11/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        /// 

        public ContentResult AddExamenVerificador(string datos)
        {
            new OrdenBl().InsertOrdenVerificador(datos, Logueado.idUsuario, EstablecimientoSeleccionado.IdEstablecimiento);
            return null;
        }
        /// <summary>
        /// Descripción: Controlador que agrega Examen, muestras y materiales en la pantalla del ANALISTA.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 18/11/2017
        /// Fecha Modificación: 11/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        /// 

        public ContentResult AddExamenAnalista(string datos)
        {
            var eesusuario = EstablecimientoSeleccionado.IdEstablecimiento;
            var ordenBl = new OrdenBl();
            ordenBl.InsertOrdenAnalista(datos, Logueado.idUsuario, eesusuario);

            return null;
        }
        public ContentResult AddExamenRomINS(string datos)
        {
            new OrdenBl().InsertOrdenRomINS(datos, Logueado.idUsuario, EstablecimientoSeleccionado.IdEstablecimiento);
            return null;
        }
        /// <summary>
        /// Descripción: Controlador que obtiene los datos clinicos configurados para una enfermedad.
        /// Muestran los datos en la pantalla _TblDatoClinico
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public ActionResult GetDatosClinicos(int idEnfermedad)
        {
            var enfermedadListAgregados = new List<Enfermedad>();

            if (Session["enfermedadListAgregados"] != null)
                enfermedadListAgregados = (List<Enfermedad>)Session["enfermedadListAgregados"];

            var enfermedad = enfermedadListAgregados.FirstOrDefault(x => x.idEnfermedad ==idEnfermedad);

            return PartialView("_TblDatoClinico", enfermedad);
        }
        /// <summary>
        /// Descripción: Controlador que obtiene los materiales seleccionados
        /// Muestran los datos en la pantalla _TblMaterial o _TblMaterialRecepcionar
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <param name="idMaterial"></param>
        /// <param name="idLaboratorio"></param>
        /// <param name="volumen"></param>
        /// <param name="cantidad"></param>
        /// <param name="noPrecisaVolumen"></param>
        /// <param name="tipoMaterial"></param>
        /// <param name="fechaEnvio"></param>
        /// <param name="horaEnvio"></param>
        /// <param name="idEnfermedadExamen"></param>
        /// <returns></returns>
        public ActionResult AddMaterial(int idTipoMuestra, int idMaterial, int idLaboratorio, decimal volumen, int cantidad,
            bool noPrecisaVolumen, int tipoMaterial, string fechaEnvio, string horaEnvio, string idEnfermedadExamen)
        {
            var array = idEnfermedadExamen.Split('_');
            var idEnfermedad = Convert.ToInt32(array[0]);
            var idExamen = array[1];

            var ordenExamenListAgregados = (List<OrdenExamen>)Session["ordenExamenListAgregados"];
            var ordenExamen = ordenExamenListAgregados.FirstOrDefault(x => x.Examen.idExamen == Guid.Parse(idExamen) && x.Enfermedad.idEnfermedad == idEnfermedad);

            var ordenMaterialListAgregados = new List<OrdenMaterial>();
            if (Session["ordenMaterialListAgregados"] != null)
                ordenMaterialListAgregados = (List<OrdenMaterial>)Session["ordenMaterialListAgregados"];

            var ordenMuestraListAgregados = new List<OrdenMuestra>();
            if (Session["ordenMuestraListAgregados"] != null)
                ordenMuestraListAgregados = (List<OrdenMuestra>)Session["ordenMuestraListAgregados"];

            var laboratorioList = (List<LaboratorioVMSelect>)Session["laboratorioList"];
            var laboratoriovm = laboratorioList.FirstOrDefault(x => x.IdEstablecimiento == Convert.ToInt32(idLaboratorio));
            Laboratorio laboratorio = new Laboratorio
            {
                IdLaboratorio = laboratoriovm.IdLabIns.Value,
                clasificacion = laboratoriovm.clasificacion,
                CodigoUnico = laboratoriovm.CodigoUnico,
                Direccion= laboratoriovm.Direccion,
                Nombre = laboratoriovm.NombreEstablecimiento,
                Ubigeo= laboratoriovm.Ubigeo
            };

            Session["laboratorioSeleccionado"] = laboratorio;

            var ordenMuestra = ordenMuestraListAgregados.FirstOrDefault(x => x.TipoMuestra.idTipoMuestra == Convert.ToInt32(idTipoMuestra));
            var materialList = (List<Material>)Session["materialList"];
            var material = materialList.FirstOrDefault(x => x.idMaterial == Convert.ToInt32(idMaterial));

            CreateObjectOrdenMaterial(cantidad, noPrecisaVolumen, volumen, tipoMaterial, material,
                fechaEnvio, horaEnvio, ordenMuestra, laboratorio, ordenExamen, ordenMaterialListAgregados, 0);

            var ordenMaterialList = new List<OrdenMaterial> { ordenMaterialListAgregados.Last() };

            var tipoRegistro = (TipoRegistroOrden)Session["tipoRegistro"];
            var partialview = "";
            switch (tipoRegistro)
            {
                case TipoRegistroOrden.SOLO_ORDEN:
                    partialview = "_TblMultiple";
                    break;
                case TipoRegistroOrden.ORDEN_RECEPCION:
                    partialview = "_TblMultipleRecepcionar";
                    break;
                case TipoRegistroOrden.SOLO_ORDEN_MUESTRA:
                    partialview = "_TblMultipleManual";
                    break;
                default:
                    break;
            }
            return PartialView(partialview, ordenMaterialList);
            //return PartialView(tipoRegistro == (int)TipoRegistroOrden.SOLO_ORDEN ? "_TblMaterial" : "_TblMaterialRecepcionar", ordenMaterialList);
        }
        /// <summary>
        /// Descripción: Metodo que crea y obtiene una lista con los materiales agregados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="cantidad"></param>
        /// <param name="noPrecisaVolumen"></param>
        /// <param name="volumen"></param>
        /// <param name="tipoMaterial"></param>
        /// <param name="material"></param>
        /// <param name="fechaEnvio"></param>
        /// <param name="horaEnvio"></param>
        /// <param name="ordenMuestra"></param>
        /// <param name="laboratorio"></param>
        /// <param name="ordenExamen"></param>
        /// <param name="ordenMaterialListAgregados"></param>
        /// <returns></returns>
        private List<OrdenMaterial> CreateObjectOrdenMaterial(int cantidad, bool noPrecisaVolumen, decimal volumen,
           int tipoMaterial, Material material, string fechaEnvio, string horaEnvio, OrdenMuestra ordenMuestra,
           Laboratorio laboratorio, OrdenExamen ordenExamen, List<OrdenMaterial> ordenMaterialListAgregados, int nroMuestra)
        {
            var ordenMaterial = new OrdenMaterial
            {
                idOrdenMaterial = Guid.NewGuid(),
                cantidad = cantidad,
                OrdenExamen = ordenExamen,
                noPrecisaVolumen = noPrecisaVolumen ? 1 : 0,
                volumenMuestraColectada = noPrecisaVolumen ? 0 : volumen,
                Tipo = tipoMaterial,
                Material = material,
                ordenMuestra = ordenMuestra,
                Laboratorio = laboratorio,
                ordenMuestraRecepcionList = new List<OrdenMuestraRecepcion>(),
                nroMuestra = nroMuestra,
                idTipoMuestra = ordenMuestra.idTipoMuestra
            };

            //var fecha = fechaEnvio.Split('/'); juan muga
            //ordenMaterial.fechaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0])); juan muga
            ordenMaterial.fechaEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //var hora = horaEnvio.Split(':'); juan muga
            //ordenMaterial.horaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0); juan muga
            ordenMaterial.horaEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            //Juan Muga
            if ((TipoRegistroOrden)Session["tipoRegistro"] != TipoRegistroOrden.SOLO_ORDEN && (TipoRegistroOrden)Session["tipoRegistro"] != TipoRegistroOrden.SOLO_ORDEN_MUESTRA)
            //if (Session["tipoRegistroO_R"] != null && (int)Session["tipoRegistroO_R"] == (int)TipoRegistroOrden.ORDEN_RECEPCION)
            {
                for (var i = 0; i < cantidad; i++)
                {
                    var ordenMuestraRecepcion = new OrdenMuestraRecepcion
                    {
                        conforme = 1,
                        idOrdenMuestraRecepcion = Guid.NewGuid()
                    };
                    int x = 0;
                    if (ordenMuestra.TipoMuestra == null)
                    {
                        x= ordenExamen.IdTipoMuestra;
                    }
                    else
                    {
                        x = ordenMuestra.TipoMuestra.idTipoMuestra;
                    }
                    var criterioRechazoBl = new CriterioRechazoBl();
                    ordenMuestraRecepcion.criterioRechazoList = criterioRechazoBl.GetCriteriosByTipoMuestra(x, 2);

                    foreach (var criterioRechazo in ordenMuestraRecepcion.criterioRechazoList)
                    {
                        criterioRechazo.rechazado = false;
                    }

                    ordenMaterial.ordenMuestraRecepcionList.Add(ordenMuestraRecepcion);
                }
            }
            ordenMaterialListAgregados.Add(ordenMaterial);

            Session["ordenMaterialListAgregados"] = ordenMaterialListAgregados;

            return ordenMaterialListAgregados;
        }

        /// <summary>
        /// Descripción: Controlador que quita de la lista los examenes agregados en memoria.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DelExamen(string idExamen, int idEnfermedad, int idTipoMuestra)
        {
            //Se elimina el examen
            List<OrdenExamen> ordenExamenList = (List<OrdenExamen>)this.Session["ordenExamenListAgregados"];
            ordenExamenList = ordenExamenList.Where(x => x.Examen.idExamen != Guid.Parse(idExamen) || x.Enfermedad.idEnfermedad != idEnfermedad).ToList();

            int mx = 0;
            foreach (var item in ordenExamenList)
            {
                if (item.IdTipoMuestra == idTipoMuestra)
                {
                    mx++;
                }
            }
            this.Session["ordenExamenListAgregados"] = ordenExamenList;
                        
            ViewBag.eliminarEnfermedad = ordenExamenList.Where(x => x.Enfermedad.idEnfermedad == idEnfermedad).Count();

            //Si non hay mas examenes asociados a la enfermedad se elimina la enfermedad
            if (ViewBag.eliminarEnfermedad == 0)
            {
                if (this.Session["enfermedadListAgregados"] != null)
                {
                    List<Enfermedad> enfermedadListAgregados = (List<Enfermedad>)this.Session["enfermedadListAgregados"];
                    this.Session["enfermedadListAgregados"] = enfermedadListAgregados.Where(x => x.idEnfermedad != idEnfermedad).ToList();

                    List<OrdenDatoClinico> ordenDatoClinicoListAgregados = (List<OrdenDatoClinico>)this.Session["ordenDatoClinicoListAgregados"];
                    //this.Session["ordenDatoClinicoListAgregados"] = ordenDatoClinicoListAgregados.Where(x => x.Enfermedad.idEnfermedad != idEnfermedad).ToList();
                    if (this.Session["ordenDatoClinicoListAgregados"] != null)
                    {
                        this.Session["ordenDatoClinicoListAgregados"] = ordenDatoClinicoListAgregados.Where(x => x.Enfermedad.idEnfermedad != idEnfermedad).ToList();
                    }
                }
            }

            /*Se eliminan los tipos de Muestra Asociadas al examen siempre y cuando no se encuentre en otro examen*/
            List<OrdenMuestra> ordenMuestraList = (List<OrdenMuestra>)this.Session["ordenMuestraListAgregados"];

            if (mx == 0)
            {
                ordenMuestraList = ordenMuestraList.Where(x => x.idTipoMuestra != idTipoMuestra).ToList();
            }
            List<OrdenMuestra> ordenMuestraListNueva = new List<OrdenMuestra>();

            foreach (OrdenMuestra ordenMuestra in ordenMuestraList)
            {
                //Se retira el examen
                ordenMuestra.ordenExamenList = ordenMuestra.ordenExamenList.Where(x => x.Examen.idExamen != Guid.Parse(idExamen) || x.Enfermedad.idEnfermedad != idEnfermedad ).ToList();

                if (ordenMuestra.ordenExamenList.Count() > 0)
                {
                    ordenMuestraListNueva.Add(ordenMuestra);
                }
                else
                {
                    if (this.Session["ordenMaterialListAgregados"] != null)
                    {
                        List<OrdenMaterial> ordenMaterialList = (List<OrdenMaterial>)this.Session["ordenMaterialListAgregados"];
                        this.Session["ordenMaterialListAgregados"] = ordenMaterialList.Where(x => x.Material.TipoMuestra.idTipoMuestra != ordenMuestra.TipoMuestra.idTipoMuestra).ToList();
                    }
                }
            }

            this.Session["ordenMuestraListAgregados"] = ordenMuestraListNueva;

            Orden ordenTmp = new Orden();
            ordenTmp.ordenExamenList = ordenExamenList;
            ordenTmp.ordenMuestraList = ordenMuestraListNueva;
            var ListMaterialExamen = (List<OrdenMaterial>)this.Session["ordenMaterialListAgregados"];
            ordenTmp.ordenMaterialList = ListMaterialExamen.Where(x => x.OrdenExamen.Examen.idExamen != Guid.Parse(idExamen)).ToList();
            this.Session["ordenMaterialListAgregados"] = ListMaterialExamen.Where(x => x.OrdenExamen.Examen.idExamen != Guid.Parse(idExamen)).ToList();

            var tipoRegistro = (Enums.TipoRegistroOrden)this.Session["tipoRegistro"];
            var partialview = "";
            switch (tipoRegistro)
            {
                case TipoRegistroOrden.SOLO_ORDEN:
                    partialview = "_TblMultiple";
                    break;
                case TipoRegistroOrden.ORDEN_RECEPCION:
                    partialview = "_TblMultipleRecepcionar";
                    break;
                case TipoRegistroOrden.SOLO_ORDEN_MUESTRA:
                    partialview = "_TblMultipleManual";
                    break;
                default:
                    break;
            }
            return PartialView(partialview, ordenTmp);
        }

        /// <summary>
        /// Descripción: Controlador que quita de la lista los materiales agregados en memoria.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenMaterial"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DelMaterial(Guid idOrdenMaterial)
        {
            List<OrdenMaterial> ordenMaterialList = (List<OrdenMaterial>)this.Session["ordenMaterialListAgregados"];
            ordenMaterialList = ordenMaterialList.Where(x => x.idOrdenMaterial != idOrdenMaterial).ToList();
            this.Session["ordenMaterialListAgregados"] = ordenMaterialList;

            var tipoRegistro = (Enums.TipoRegistroOrden)this.Session["tipoRegistro"];
            var partialview = "";
            switch (tipoRegistro)
            {
                case TipoRegistroOrden.SOLO_ORDEN:
                    partialview = "_TblMultiple";
                    break;
                case TipoRegistroOrden.ORDEN_RECEPCION:
                    partialview = "_TblMultipleRecepcionar";
                    break;
                case TipoRegistroOrden.SOLO_ORDEN_MUESTRA:
                    partialview = "_TblMultipleManual";
                    break;
                default:
                    break;
            }
            return PartialView(partialview, ordenMaterialList);
        }

        #endregion
        /// <summary>
        /// Descripción: Metodo que limpia las variables de sesion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        public void DelSessionOrden()
        {
            Session["enfermedadListAgregados"] = null;
            Session["ordenExamenListAgregados"] = null;
            Session["ordenMuestraListAgregados"] = null;
            Session["ordenMaterialListAgregados"] = null;
            Session["ordenDatoClinicoListAgregados"] = null;
            Session["laboratorioList"] = null;
            Session["laboratorioSeleccionado"] = null;
            Session["orden"] = null;
        }

        /// <summary>
        /// Descripción: Controlador que Crea una nueva orden.
        /// Paso 1 INICIO NUEVA ORDEN
        /// Paso 2 REVICION ORDEN
        /// Paso 3 FINALIZAR ORDEN (SOLO ORDEN) REFERENCIAR ORDEN (ORDEN RECEPCION) 
        /// Paso 4 FINALIZAR ORDEN RECEPCION
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <param name="NroDocumento"></param>
        /// <param name="Nombre"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="tipoRegistro"></param>
        /// <returns></returns>
        public ActionResult New(String idPaciente, String NroDocumento, String Nombre, String codigoOrden, int? tipoRegistro)
        {
            DelSessionOrden();
            Session["idPaciente"] = idPaciente;
            ViewBag.OrdenExistente = false;
            if (this.Session["ordenExamenListAgregados"] != null)
            {
                ViewBag.OrdenExistente = true;
            }


            //Juan Muga - inicio
            //if (tipoRegistro == (int)Enums.TipoRegistroOrden.ORDEN_RECEPCION)
            //{
            //    this.Session["tipoRegistroO_R"] = tipoRegistro;
            //}
            //Juan Muga - fin
            //this.Session["tipoRegistro"] = tipoRegistro == null ? Enums.TipoRegistroOrden.SOLO_ORDEN : Enums.TipoRegistroOrden.ORDEN_RECEPCION;
            //this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.SOLO_ORDEN_MUESTRA;

            switch (tipoRegistro)
            {
                case (int)Enums.TipoRegistroOrden.ORDEN_RECEPCION:
                    this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.ORDEN_RECEPCION;
                    //nuevaOrden.TipoRegistro = TipoRegistroOrden.ORDEN_RECEPCION;
                    break;
                case (int)Enums.TipoRegistroOrden.SOLO_ORDEN_MUESTRA:
                    this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.SOLO_ORDEN_MUESTRA;
                    //nuevaOrden.TipoRegistro = TipoRegistroOrden.SOLO_ORDEN_MUESTRA;
                    break;
                case (int)Enums.TipoRegistroOrden.SOLO_ORDEN:
                    this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.SOLO_ORDEN;
                    //nuevaOrden.TipoRegistro = TipoRegistroOrden.SOLO_ORDEN;
                    break;
                default:
                    this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.SOLO_ORDEN_MUESTRA;
                    //nuevaOrden.TipoRegistro = TipoRegistroOrden.SOLO_ORDEN_MUESTRA;
                    break;
            }


            ViewBag.tipoRegistro = (Enums.TipoRegistroOrden)this.Session["tipoRegistro"];
            //ViewBag.tipoRegistro = nuevaOrden.TipoRegistro; // Cambiar esta linea por la anterior que usa session

            //this.Session["pasoRegistro"] = 1; 

            //List<SelectListItem> seguroList = new List<SelectListItem>();
            //ListaBl listaBl = new ListaBl();
            //foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoSeguroSalud).ListaDetalle)
            //    seguroList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });
            //ViewBag.seguroList = seguroList;
            CargarSeguroList();

            List <SelectListItem> establecimientoList = new List<SelectListItem>();
            establecimientoList.Add(new SelectListItem { Text = "", Value = "" });
            ViewBag.establecimientoList = establecimientoList;

            CargarProyectos();

            EstablecimientoBl establecimientoBl = new EstablecimientoBl();
            List<Establecimiento> establecimientosFrecuentes = establecimientoBl.GetEstablecimientosFrecuentesByIdUsuario(((Usuario)this.Session["UsuarioLogin"]).idUsuario);
            establecimientosFrecuentes.Add(new Establecimiento { IdEstablecimiento = 0, Nombre = "Seleccione el Establecimiento" });

            ViewBag.establecimientoListFrecuentes = establecimientosFrecuentes.OrderBy(x => x.IdEstablecimiento).ToList();

            Orden orden = new Orden(); //Se crea la Orden

            orden.codigoOrden = codigoOrden;

            //Se crea Objeto Paciente
            Paciente paciente = new Paciente();
            if (idPaciente != null)
            {
                PacienteBl pacienteBl = new PacienteBl();
                paciente = pacienteBl.getPacienteById(Guid.Parse(idPaciente));

                //paciente.IdPaciente = Guid.Parse(idPaciente);
                //paciente.Nombres = Nombre; //contiene todo los nombres
                //paciente.NroDocumento = NroDocumento;
            }
            
            orden.Paciente = paciente;
            //orden.establecimiento.eUbigeo = new Model.Ubigeo();
            CargaUbigeoEstablecimiento();
            getDatosFromSession(orden);
            ViewBag.SolicitanteNombre = string.Empty;
            if (Session["SolicitanteSeleccionadoId"] != null && Session["SolicitanteSeleccionadoNombre"] != null)
            {
                //ViewBag.SolicitanteId = solicitanteSeleccionado.FirstOrDefault().Value;
                orden.solicitante = (string)Session["SolicitanteSeleccionadoId"];
                ViewBag.SolicitanteNombre = (string)Session["SolicitanteSeleccionadoNombre"];
                var solicitanteListItem = new List<SelectListItem>();
                solicitanteListItem.Add(new SelectListItem { Value = orden.solicitante, Text = ViewBag.SolicitanteNombre });
                ViewBag.Solicitantes = solicitanteListItem;
            }

            return View(orden);
            //return View(nuevaOrden);
        }

        /// <summary>
        /// Descripción: Metodo que obtiene la informacion de una orden ingresada a medias.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        private void getDatosFromSession(Orden orden)
        {

            /*Orden examenes*/
            List<OrdenExamen> ordenExamenListAgregados = new List<OrdenExamen>();
            if (this.Session["ordenExamenListAgregados"] != null)
            {
                ordenExamenListAgregados = (List<OrdenExamen>)this.Session["ordenExamenListAgregados"];
            }
            orden.ordenExamenList = ordenExamenListAgregados;


            /*Orden Muestra*/
            List<OrdenMuestra> ordenMuestraListAgregados = new List<OrdenMuestra>();
            if (this.Session["ordenMuestraListAgregados"] != null)
            {
                ordenMuestraListAgregados = (List<OrdenMuestra>)this.Session["ordenMuestraListAgregados"];
            }
            orden.ordenMuestraList = ordenMuestraListAgregados;


            /*Orden Material*/
            List<OrdenMaterial> ordenMaterialListAgregados = new List<OrdenMaterial>();
            if (this.Session["ordenMaterialListAgregados"] != null)
            {
                ordenMaterialListAgregados = (List<OrdenMaterial>)this.Session["ordenMaterialListAgregados"];
            }
            orden.ordenMaterialList = ordenMaterialListAgregados;


            /*Orden Datos Clinicos*/

            List<OrdenDatoClinico> ordenDatoClinicoListAgregados = new List<OrdenDatoClinico>();
            if (this.Session["ordenDatoClinicoListAgregados"] != null)
            {
                ordenDatoClinicoListAgregados = (List<OrdenDatoClinico>)this.Session["ordenDatoClinicoListAgregados"];
            }

            orden.ordenDatoClinicoList = ordenDatoClinicoListAgregados;



            List<Enfermedad> enfermedadListAgregados = new List<Enfermedad>();
            if (this.Session["enfermedadListAgregados"] != null)
            {
                enfermedadListAgregados = (List<Enfermedad>)this.Session["enfermedadListAgregados"];
            }
            orden.enfermedadList = enfermedadListAgregados;
            if (this.Session["solicitante"] != null)
            {
                LoadNombreSolicitante(this.Session["solicitante"].ToString());
                //LoadSolicitantes();
                orden.solicitante = this.Session["solicitante"].ToString();
            }
            if (this.Session["ordenNroOficio"] != null)
            {
                orden.nroOficio = this.Session["ordenNroOficio"].ToString();
            }
            if (this.Session["ordenidEstablecimiento"] != null && this.Session["ordenNombreEstablecimiento"] != null)
            {
                orden.idEstablecimiento = int.Parse(this.Session["ordenidEstablecimiento"].ToString());
                ViewBag.idEstablecimiento = this.Session["ordenidEstablecimiento"].ToString();
                ViewBag.nombreEstablecimiento = this.Session["ordenNombreEstablecimiento"].ToString();
            }

            if (this.Session["ordenidEstablecimientoEnvio"] != null)
            {
                orden.idEstablecimientoEnvio = int.Parse(this.Session["ordenidEstablecimientoEnvio"].ToString());
                ViewBag.idEstablecimientoEnvio = this.Session["ordenidEstablecimientoEnvio"].ToString();
                ViewBag.nombreEstablecimientoEnvio = this.Session["ordenNombreEstablecimientoEnvio"].ToString();
            }
        }
        /// <summary>
        /// Descripción: Controlador usado para mostrar el popup de Enfermedades.
        /// Formulario enlazado _FormPopupEnfermedadExamen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowPopupEnfermedadExamen()//TipoRegistroOrden tipoRegistro
        {
            var ordenExamenListAgregados = new List<OrdenExamen>();
            var ordenMaterialListAgregados = new List<OrdenMaterial>();
            bool usarstaticcache = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["usarstaticcache"], out usarstaticcache);
            if (usarstaticcache)
            {
                var lstdatos = StaticCache.DevuelveDatosOrdenExamen(Logueado.idUsuario);
                if (lstdatos != null && lstdatos.Any())
                {
                    ordenExamenListAgregados.Add(lstdatos.Last().ordenExamen);
                    ordenMaterialListAgregados.Add(lstdatos.Last().ordenMaterial);
                }
            }

            if (ordenExamenListAgregados.Count > 0)
            {
                var ordenExamen = ordenExamenListAgregados.Last();
                ViewBag.valueEnfermedadPreSeleccionada = ordenExamen.Enfermedad.idEnfermedad;
                ViewBag.textoEnfermedadPreSeleccionada = ordenExamen.Enfermedad.Snomed + " - " + ordenExamen.Enfermedad.nombre;
                ViewBag.valueExamenPreSeleccionada = ordenExamen.Examen.idExamen;
                ViewBag.textoExamenPreSeleccionada = ordenExamen.Examen.nombre;
            }
            if (ordenMaterialListAgregados.Count > 0)
            {
                var ordenMaterial = ordenMaterialListAgregados.Last();
                ViewBag.valueLaboratorioPreSeleccionada = ordenMaterial.Laboratorio.IdLaboratorio.ToString();
                ViewBag.textoLaboratorioPreSeleccionada = ordenMaterial.Laboratorio.Nombre;
                ViewBag.direccionLaboratorio = ordenMaterial.Laboratorio.Direccion;
                ViewBag.idTipoMuestra = ordenMaterial.idTipoMuestra;
            }

            var tipoRegistro = (TipoRegistroOrden)Session["tipoRegistro"];

            ViewBag.tipoRegistro = tipoRegistro;

            ViewBag.examenList = new List<Examen>();

            ViewBag.tipoMuestraList = new List<TipoMuestra>();

            return PartialView("_FormPopupEnfermedadExamen");
        }
        /// <summary>
        /// Descripción: Controlador usado para mostrar el popup de Enfermedades.VERIFICADOR Y ANALISTA
        /// Formulario enlazado _FormPopupEnfermedadExamen
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 10/11/2017
        /// Fecha Modificación: 10/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowPopupNuevoEnfermedadExamen()
        {
            var ordenExamenListAgregados = new List<OrdenExamen>();

            if (Session["ordenExamenListAgregados"] != null)
                ordenExamenListAgregados = (List<OrdenExamen>)Session["ordenExamenListAgregados"];

            if (ordenExamenListAgregados.Count > 0)
            {
                var ordenExamen = ordenExamenListAgregados.Last();
                ViewBag.valueEnfermedadPreSeleccionada = ordenExamen.Enfermedad.idEnfermedad;
                ViewBag.textoEnfermedadPreSeleccionada = ordenExamen.Enfermedad.Snomed + " - " + ordenExamen.Enfermedad.nombre;
            }

            Session["tipoRegistro"] = 1;
            var tipoRegistro = (TipoRegistroOrden)Session["tipoRegistro"];

            ViewBag.tipoRegistro = tipoRegistro;

            ViewBag.examenList = new List<Examen>();
            ViewBag.tipoMuestraList = new List<TipoMuestra>();

            return PartialView("_FormPopupEnfermedadExamen");
        }
        /// <summary>
        /// Descripción: Controlador usado para mostrar el popup de Enfermedades.
        /// Formulario enlazado _FormPopupMaterial
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public ActionResult ShowPopupMaterial(int idTipoMuestra)
        {
            var ordenMuestraListAgregados = (List<OrdenMuestra>)this.Session["ordenMuestraListAgregados"];
            var ordenMuestra = ordenMuestraListAgregados.FirstOrDefault(x => x.TipoMuestra.idTipoMuestra == Convert.ToInt32(idTipoMuestra));

            var ordenExamenListAgregados = new List<OrdenExamen>();
            if (this.Session["ordenExamenListAgregados"] != null)
                ordenExamenListAgregados = (List<OrdenExamen>)this.Session["ordenExamenListAgregados"];

            ViewBag.ordenExamenList = ordenExamenListAgregados.Where(
                                            ordenExamen => ordenExamen.ordenMuestraList.
                                                                Any(x => x.TipoMuestra.idTipoMuestra == idTipoMuestra)).ToList(); ;

            var materialBl = new MaterialBl();
            var materialList = materialBl.GetMaterialesByIdTipoMuestra(ordenMuestra.TipoMuestra.idTipoMuestra);
            this.Session["materialList"] = materialList;
            ViewBag.materialList = materialList;

            var tipoRegistro = (Enums.TipoRegistroOrden)this.Session["tipoRegistro"];
            ViewBag.tipoRegistro = tipoRegistro;

            var ordenMaterial = new OrdenMaterial { ordenMuestra = ordenMuestra };

            return PartialView("_FormPopupMaterial", ordenMaterial);
        }
        /// <summary>
        /// Descripción: Controlador usado para mostrar el popup de Solicitantes.
        /// Formulario enlazado _FormPopupSolicitante
        /// Author: Juan Muga.
        /// Fecha Creacion: 03/04/2017
        /// Fecha Modificación: 
        /// Modificación: 
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowPopupSolicitante()
        {
            //ViewBag.page = page;
            //ViewBag.search = search;

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
        /// <summary>
        /// Descripción: Controlador usado para el registro de un solicitante.
        /// Author: Juan Muga.
        /// Fecha Creacion: 03/04/2017
        /// Fecha Modificación: 
        /// Modificación: 
        /// </summary>
        /// <param name="DNI"></param>
        /// <param name="codigoColegio"></param>
        /// <param name="ApePat"></param>
        /// <param name="ApeMat"></param>
        /// <param name="Nombre"></param>
        /// <param name="Correo"></param>
        /// <param name="profesion"></param>
        /// <param name="laboratorio"></param>
        /// <param name="telefono"></param>
        /// <returns></returns>
        public int AddSolicitante(String DNI, String codigoColegio, String ApePat,
                                     String ApeMat, String Nombre, String Correo,
                                     int profesion, String laboratorio, String telefono)
        {
            int valor = 0;
            var Solicitante = new OrdenSolicitante
            {
                apellidoMaterno = ApeMat,
                apellidoPaterno = ApePat,
                Nombres = Nombre,
                codigoColegio = int.Parse(codigoColegio),
                idProfesion = profesion,
                CodigoUnico = laboratorio,
                telefonoContacto = telefono,
                correo = Correo,
                Dni = DNI,
                idUsuarioRegistro = Logueado.idUsuario
            };
            SolicitanteBl oSolicitanteBl = new SolicitanteBl();
            try
            {
                valor = oSolicitanteBl.InsertSolicitante(Solicitante);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return valor;
        }
        /// <summary>
        /// Descripción: Controlador usado para mostrar obtener informacion de los tipos de muestra seleccionados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public ActionResult GetMaterialByTiposMuestra(int idTipoMuestra)
        {
            MaterialBl materialBl = new MaterialBl();
            List<Material> materialList = materialBl.GetMaterialesByIdTipoMuestra(idTipoMuestra);
            //JRCR-REQ01
            ViewBag.MaterialPorDefecto = materialList.Any() ? materialList.First().idMaterial : 0;
            this.Session["materialList"] = materialList;
            return PartialView("_Material", materialList);
        }

        public List<Examen> ObtenerExamenes(int genero, int idenfermedad)
        {
            List<Examen> resultados = new List<Examen>();
            string nombreSession = string.Format("ListaExamenesEnf{0}", idenfermedad);
            if(Session[nombreSession] != null)
            {
                if (((List<Examen>)Session[nombreSession]).Any())
                {
                    resultados = (List<Examen>)Session[nombreSession];
                }
                else
                {
                    resultados = CargarExamenes(idenfermedad);
                }
            }
            else
            {
                resultados = CargarExamenes(idenfermedad);
            }

            Session[nombreSession] = resultados;
            return resultados;
        }

        public List<Examen> CargarExamenes(int idenfermedad)
        {
            List<Examen> resultados = new List<Examen>();
            IExamenBl examenBl = new ExamenBl();
            resultados = examenBl.GetExamenesPorEnfermedad(idenfermedad);

            return resultados;
        }

        /// <summary>
        /// Descripción: Metodo que retorna examenes concatenado con el codigo y la descripcion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="genero"></param>
        /// <param name="idlab"></param>
        /// <returns></returns>
        public String GetExamenes(int genero, int idenfermedad)
        {
            String data = this.Request.Params["data[q]"];

            //IExamenBl examenBl = new ExamenBl();
            //List<Examen> examenList = examenBl.GetExamenesByIdLaboratorio(idenfermedad, genero, data);
            var examenList = ObtenerExamenes(genero, idenfermedad);
            examenList = string.IsNullOrWhiteSpace(data)? examenList : examenList.Where(x => x.nombre.ToLower().Contains(data.Trim().ToLower())).ToList();

            string resultado = "{\"q\":\"" + data + "\",\"results\":[";

            Boolean existeEnfermedad = false;
            foreach (var examen in examenList)
            {
                resultado += "{\"id\":\"" + examen.idExamen + "\",\"text\":\"" + examen.nombre.TrimEnd() + "\"},";
                existeEnfermedad = true;
            }

            if (existeEnfermedad)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            Session["examenesSeleccionados"] = new String[0];
            Session["tiposMuestraSeleccionados"] = new String[0];
            Session["materialesSeleccionados"] = new String[0];

            /*Se limpian las variables de Session de listas*/
            Session["tipoMuestraList"] = new List<TipoMuestra>();
            Session["materialList"] = new List<MaterialVd>();

            /*Se agrega la nueva lista de examenes de la enfermedad seleccionada*/
            Session["examenList"] = examenList;

            var model = examenList;
            return resultado;
        }
        public ActionResult GetExamenes2(int genero, int idenfermedad)
        {
            String data = this.Request.Params["data[q]"];

            IExamenBl examenBl = new ExamenBl();

            List<Examen> examenList = examenBl.GetExamenesByIdLaboratorio(idenfermedad, genero, data);
            //List<Examen> examenList = examenBl.GetExamenesByGenero(genero, data);
            //List<Examen> examenList = examenBl.GetExamenesByIdEnfermedadOrden(idEnfermedad, data);


            return Json(examenList.Select(m => new
            {
                value = m.idExamen,
                text = m.nombre
            }), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Descripción: Cotrolador que devuelve los tipos de muestras de una solo examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public ActionResult GetTiposMuestraByIdExamen(string[] idExamen)
        {
            List<TipoMuestra> tipoMuestraList = new List<TipoMuestra>();
            List<Guid> idExamenList = new List<Guid>();
            if (idExamen != null)
            {
                ITipoMuestraBl tipoMuestraBl = new TipoMuestraBl();
                for (int i = 0; i < idExamen.Count(); i++)
                {
                    idExamenList.Add(Guid.Parse(idExamen[i]));
                }
                tipoMuestraList = tipoMuestraBl.GetTiposMuestraByIdExamen(idExamenList);
            }


            List<TipoMuestra> tipoMuestraListTmp = new List<TipoMuestra>();
            tipoMuestraListTmp.Add(new TipoMuestra { nombre = "" });
            foreach (TipoMuestra tipoMuestra in tipoMuestraList)
            {
                tipoMuestraListTmp.Add(tipoMuestra);
            }
            var model = tipoMuestraListTmp;


            //Se agrega los examenes seleccionados a la Session
            this.Session["examenesSeleccionados"] = idExamen;

            //Se agrega la lista de tipos de muestra a la Session
            this.Session["tipoMuestraList"] = model;

            return PartialView("_TipoMuestra", model);
        }
        /// <summary>
        /// Descripción: Metodo que retorna examenes concatenado con el codigo y la descripcion.
        /// Author: sotero bustamante.
        /// Fecha Creacion: 19/11/2017
        /// Fecha Modificación: 19/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="genero"></param>
        /// <param name="idlab"></param>
        /// /// <param name="idEnfermedad"></param>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public String GetExamenesByTipoMuestra(string idOrden, int idEnfermedad)
        {
            String data = this.Request.Params["data[q]"];

            var examenBl = new ExamenBl();

            List<Examen> examenList = examenBl.GetExamenesByTipoMuestra(idOrden, data, idEnfermedad);
            //List<Examen> examenList = examenBl.GetExamenesByGenero(genero, data);
            //List<Examen> examenList = examenBl.GetExamenesByIdEnfermedadOrden(idEnfermedad, data);
            //OrdenExamenByOrden

            string resultado = "{\"q\":\"" + data + "\",\"results\":[";


            Boolean existeEnfermedad = false;
            foreach (var examen in examenList)
            {
                resultado += "{\"id\":\"" + examen.idExamen + "\",\"text\":\"" + examen.nombre.TrimEnd() + "\"},";
                existeEnfermedad = true;
            }

            if (existeEnfermedad)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            this.Session["examenesSeleccionados"] = new String[0];
            this.Session["tiposMuestraSeleccionados"] = new String[0];
            this.Session["materialesSeleccionados"] = new String[0];

            /*Se limpian las variables de Session de listas*/
            this.Session["tipoMuestraList"] = new List<TipoMuestra>();
            this.Session["materialList"] = new List<MaterialVd>();

            /*Se agrega la nueva lista de examenes de la enfermedad seleccionada*/
            this.Session["examenList"] = examenList;
            var model = examenList;
            return resultado;
        }
        /// <summary>
        /// Descripción: Cotrolador que permite mostrar la pantalla SHOW para referenciar las muestras a otro laboratorio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult Referenciar()
        {
            Orden ordenSession = (Orden)this.Session["orden"];

            List<OrdenMuestraRecepcion> ordenMuestraRecepcionList = new List<OrdenMuestraRecepcion>();
            /*ORDEN MATERIAL*/
            List<OrdenMaterial> ordenMaterialListAgregados = new List<OrdenMaterial>();
            if (this.Session["ordenMaterialListAgregados"] != null)
            {
                ordenMaterialListAgregados = (List<OrdenMaterial>)this.Session["ordenMaterialListAgregados"];



                for (int i = 0; i < ordenMaterialListAgregados.Count; i++)
                {

                    OrdenMaterial ordenMaterial = ordenMaterialListAgregados[i];


                    foreach (OrdenMuestraRecepcion ordenMuestraRecepcion in ordenMaterial.ordenMuestraRecepcionList)
                    {
                        //Si es conforme (no fue rechazada) es posible que fuera referenciada
                        if (ordenMuestraRecepcion.conforme == 1)
                        {
                            var sufijo = ordenMuestraRecepcion.idOrdenMuestraRecepcion;
                            if (Request.Form["chkNom_" + sufijo] != null)
                            {

                                ordenMuestraRecepcion.idLaboratorioOrigen = ordenMaterialListAgregados[i].Laboratorio.IdLaboratorio;
                                ordenMuestraRecepcion.idLaboratorioDestino = Convert.ToInt32(Request.Form["cmbLab_" + sufijo]);
                                var fechatmp = Request.Params["txtFec_" + sufijo];
                                String[] fecha = fechatmp.Split('/');
                                //Juan Muga - inicio
                                //ordenMaterial.fechaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));
                                //String[] hora = Request.Params["txtHor_" + sufijo].Split(':');
                                //ordenMaterial.horaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);

                                if (fecha.Count() > 1)
                                {
                                    ordenMaterial.fechaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));
                                    String[] hora = Request.Params["txtHor_" + sufijo].Split(':');
                                    ordenMaterial.horaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);
                                }
                                else
                                {
                                    ordenMaterial.fechaEnvio = DateTime.Today;
                                    ordenMaterial.horaEnvio = DateTime.Now;
                                    ordenMuestraRecepcion.fechaEnvio = DateTime.Today;
                                    ordenMuestraRecepcion.horaEnvio = DateTime.Now;
                                }
                                //Juan Muga - Fin

                                ordenMuestraRecepcionList.Add(ordenMuestraRecepcion);
                            }
                        }

                    }
                }
            }
            this.Session["ordenMaterialListAgregados"] = ordenMaterialListAgregados;
            ordenSession.ordenMaterialList = ordenMaterialListAgregados;

            if (ordenMuestraRecepcionList.Count > 0)
            {
                OrdenMuestraBl ordenMuestraBl = new OrdenMuestraBl();
                ordenMuestraBl.ReferenciarMuestras(ordenMuestraRecepcionList, Logueado.idUsuario);
            }

            ViewBag.referenciar = 0;

            DelSessionOrden();

            var usuario = ((Usuario)this.Session["UsuarioLogin"]);
            ViewBag.nombreUsuario = usuario.nombres + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;

            return View("Show", ordenSession);

        }
        public void CargaUbigeoEstablecimiento()
        {
            Laboratorio LaboratorioUbigeo = new Laboratorio();
            LaboratorioUbigeo.UbigeoLaboratorio = new Model.Ubigeo();
            LaboratorioUbigeo.UbigeoLaboratorio.Id = "      ";
            ViewBag.LabUbigeo = LaboratorioUbigeo;
        }

        /// <summary>
        /// Descripción: Cotrolador que permite realizar la transaccion para el registro de una o varias enfermedades en una misma orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        ///               SE AGREGO LA OPCION TipoRegistroOrden.SOLO_ORDEN_MUESTRA - JUAN MUGA - 08/06/2017
        /// </summary>
        /// <param name="orden"></param>
        /// <param name="idEstablecimiento"></param>
        /// <param name="idEstablecimientoFrecuente"></param>
        /// <param name="fechaMuestra"></param>
        /// <param name="horaMuestra"></param>
        /// <param name="idDato"></param>
        /// <param name="codigoOrdenTmp"></param>
        /// <param name="ActualDepartamento"></param>
        /// <param name="ActualProvincia"></param>
        /// <param name="ActualDistrito"></param>
        /// <param name="idOrdenTmp"></param>
        /// <param name="codificacion"></param>
        /// <param name="chkActualizarUbicacion"></param>
        /// <returns></returns>
        public ActionResult Save(Orden orden, String idEstablecimiento, String hddDatoEESSOrigen, String CodigoUnicoOrigen,
            String idEstablecimientoFrecuente, String hddDatoEESSEnvio, String CodigoUnicoEnvio,
            String[] fechaMuestra, String[] horaMuestra, int[] idDato,
            String codigoOrdenTmp, String ActualDepartamento, String ActualProvincia, String ActualDistrito,
            String idOrdenTmp, String[] codificacion, Boolean chkActualizarUbicacion = false)
        {
            string logInfoData = string.Empty;
            //logInfoData = string.Format(" - FechaHora:{2} - IdUsuario: {0}, EstablecimientoOrigen: {1} ", Logueado.idUsuario, hddDatoEESSOrigen, DateTime.Now);
            //new bsPage().LogInfo(Logueado.idUsuario.ToString(), "Inicio metodo Save", logInfoData);
            //variables para logError
            string lineaDeCorte = string.Empty;
            bool ordenCodigoOrdenIsNull = true;
            lineaDeCorte = "antes de ViewBag.tipoRegistro";
            var tipoRegistro = Session["tipoRegistro"] != null ? (TipoRegistroOrden)Session["tipoRegistro"] : TipoRegistroOrden.SOLO_ORDEN_MUESTRA;
            ViewBag.tipoRegistro = tipoRegistro;
            lineaDeCorte = "despues de ViewBag.tipoRegistro";
            string codigoOrdenGenerado = string.Empty;
            try
            {
                #region Datos y Sesiones Simples
                if ((!string.IsNullOrWhiteSpace(idEstablecimientoFrecuente) || !string.IsNullOrWhiteSpace(hddDatoEESSOrigen)) && string.IsNullOrWhiteSpace(codigoOrdenTmp))
                {
                    using (var ordenBl = new OrdenBl())
                    {
                        //Generar Codigo de Orden
                        if (!string.IsNullOrWhiteSpace(hddDatoEESSOrigen))
                        {
                            codigoOrdenGenerado = ordenBl.GenerarCodigoOrden(int.Parse(hddDatoEESSOrigen));
                        }
                        
                        else if (!string.IsNullOrWhiteSpace(idEstablecimientoFrecuente) && int.Parse(idEstablecimientoFrecuente) != 0)
                        {
                            codigoOrdenGenerado = ordenBl.GenerarCodigoOrden(int.Parse(idEstablecimientoFrecuente));
                        }

                        if (string.IsNullOrWhiteSpace(codigoOrdenGenerado))
                        {
                            new bsPage().LogError(new Exception("CodigoOrden Generado regresó vacio"), "LogNetLab", "CodigoOrden Generado regresó vacio", "");
                            ViewBag.textoRegistro = "Error de conexión. Por favor intente nuevamente";
                            ViewBag.existeError = "S";
                            orden.codigoOrden = string.Empty;
                            Session["solicitante"] = orden.solicitante;
                            //orden.solicitante = (string)Session["SolicitanteSeleccionadoId"];
                            ViewBag.SolicitanteNombre = (string)Session["SolicitanteSeleccionadoNombre"];
                            
                            var solicitanteListItem = new List<SelectListItem>();
                            solicitanteListItem.Add(new SelectListItem { Value = orden.solicitante, Text = ViewBag.SolicitanteNombre });
                            ViewBag.Solicitantes = solicitanteListItem;
                            if (!string.IsNullOrEmpty(hddDatoEESSOrigen))
                            {
                                orden.idEstablecimiento = int.Parse(hddDatoEESSOrigen);
                                ViewBag.nombreEstablecimiento = CodigoUnicoOrigen;
                                ViewBag.idEstablecimiento = hddDatoEESSOrigen;
                                var establec = new Establecimiento() { IdEstablecimiento = int.Parse(hddDatoEESSOrigen), Nombre = CodigoUnicoOrigen };
                                orden.establecimiento = establec;
                            }

                            if (!string.IsNullOrEmpty(hddDatoEESSEnvio))
                            {
                                orden.idEstablecimientoEnvio = int.Parse(hddDatoEESSEnvio);
                                ViewBag.nombreEstablecimientoEnvio = CodigoUnicoEnvio;
                                ViewBag.idEstablecimientoEnvio = hddDatoEESSEnvio;
                                var establecEnvio = new Establecimiento() { IdEstablecimiento = int.Parse(hddDatoEESSEnvio), Nombre = CodigoUnicoEnvio };
                                orden.establecimientoEnvio = establecEnvio;
                                this.Session["ordenidEstablecimientoEnvio"] = orden.idEstablecimientoEnvio;
                                this.Session["ordenNombreEstablecimientoEnvio"] = ViewBag.nombreEstablecimientoEnvio;
                                this.Session["ordenNroOficio"] = orden.nroOficio;
                                if (orden.Paciente == null) orden.Paciente = new Paciente();
                                orden.Paciente.UbigeoActual = new Model.Ubigeo
                                {
                                    Id = ActualDepartamento + ActualProvincia + ActualDistrito
                                };
                            }

                            CargarSeguroList();
                            CargaUbigeoEstablecimiento();
                            CargarProyectos();
                            orden.ordenExamenList = new List<OrdenExamen>();
                            orden.ordenMuestraList = new List<OrdenMuestra>();
                            orden.ordenMaterialList = new List<OrdenMaterial>();
                            orden.enfermedadList = new List<Enfermedad>();
                            return View("New", orden);
                        }
                    }
                }

                //// Create new stopwatch.
                //Stopwatch stopwatch = new Stopwatch();
                lineaDeCorte = "Inicio";
                //LoadSolicitantes();
                CargaUbigeoEstablecimiento();
                string codificacionFinalizar = string.Empty;
                var guardarfinalizar = Request.Form["GuardarFinalizar"];
                //orden.file = (Request.Form["Archivo"]!="")? Convert.FromBase64String(Request.Form["Archivo"]):null;

                //var _procedencia = Request.Form["procPaciente"];
                //orden.Procedencia = (_procedencia != "[]")?_procedencia.Replace("\"",""):"";

                //var ejecutor = (Usuario)Session["ejecutor"];
                //if (ejecutor!=null)
                //{
                //    orden.dniEjecutor = Request.Form["dniEjecutorPr"];
                //    orden.ejecutorPr = ejecutor.nombres;
                //    orden.ApePatEjecutor = ejecutor.apellidoPaterno;
                //    orden.ApeMatEjecutor = ejecutor.apellidoMaterno;
                //}
                orden.dniEjecutor = Request.Form["dniEjecutorPr"];
                orden.ejecutorPr = Request.Form["ejecutorPr"];
                lineaDeCorte = "set orden.ejecutorPr";
                if (!string.IsNullOrEmpty(hddDatoEESSOrigen))
                {
                    orden.idEstablecimiento = int.Parse(hddDatoEESSOrigen);
                    ViewBag.nombreEstablecimiento = CodigoUnicoOrigen;
                    ViewBag.idEstablecimiento = hddDatoEESSOrigen;
                    var establec = new Establecimiento() { IdEstablecimiento = int.Parse(hddDatoEESSOrigen), Nombre = CodigoUnicoOrigen };
                    orden.establecimiento = establec;
                }

                if (!string.IsNullOrEmpty(hddDatoEESSEnvio))
                {
                    orden.idEstablecimientoEnvio = int.Parse(hddDatoEESSEnvio);
                    ViewBag.nombreEstablecimientoEnvio = CodigoUnicoEnvio;
                    ViewBag.idEstablecimientoEnvio = hddDatoEESSEnvio;
                    var establecEnvio = new Establecimiento() { IdEstablecimiento = int.Parse(hddDatoEESSEnvio), Nombre = CodigoUnicoEnvio };
                    orden.establecimientoEnvio = establecEnvio;
                    this.Session["ordenidEstablecimientoEnvio"] = orden.idEstablecimientoEnvio;
                    this.Session["ordenNombreEstablecimientoEnvio"] = ViewBag.nombreEstablecimientoEnvio;
                    this.Session["ordenNroOficio"] = orden.nroOficio;
                }

                lineaDeCorte = "despues de if(!string.IsNullOrEmpty(hddDatoEESSEnvio))";
                this.Session["solicitante"] = orden.solicitante;
                AsignarSessionSolicitanteIngresado(orden.solicitante);
                if (orden.idOrden != Guid.Empty)
                {
                    codificacionFinalizar = Request.Form["codifFinalizar"];
                }
                //si parametro orden no tiene paciente, asignarle paciente de Session Orden
                if (!(orden.Paciente != null && orden.Paciente.IdPaciente != Guid.Empty) && Session["orden"] != null && (Orden)Session["orden"] != null)
                {
                    if (orden.Paciente == null) orden.Paciente = new Paciente();
                    orden.Paciente = ((Orden)this.Session["orden"]).Paciente;
                }
                //Si es necesario, descomentar ... se agregó un if para determinar si session["establecimientoList"] no es null
                if (!(orden.establecimiento != null && orden.establecimiento.IdEstablecimiento != 0) && Session["orden"] != null && (Orden)Session["orden"] != null)
                {
                    if (orden.establecimiento == null) orden.establecimiento = new Establecimiento();
                    orden.establecimiento = ((Orden)this.Session["orden"]).establecimiento;
                }
                //Juan Muga - fin

                //IProyectoBl proyectoBl = new ProyectoBl();
                //ViewBag.proyectoList = proyectoBl.GetProyectos();
                CargarProyectos();

                var establecimientoBl = new EstablecimientoBl();
                var establecimientosFrecuentes = establecimientoBl.GetEstablecimientosFrecuentesByIdUsuario(
                        ((Usuario)Session["UsuarioLogin"]).idUsuario);

                lineaDeCorte = "despues de crear establecimientosFrecuentes";
                if (orden.codigoOrden == null && codigoOrdenTmp == null)
                {
                    if (string.IsNullOrWhiteSpace(hddDatoEESSOrigen) || hddDatoEESSOrigen == "0")
                    {
                        orden.idEstablecimiento = Convert.ToInt32(idEstablecimientoFrecuente);
                        Session["establecimientoList"] = establecimientosFrecuentes;
                    }
                }

                lineaDeCorte = "antes de establecimientosFrecuentes.Add(new Establecimiento";
                establecimientosFrecuentes.Add(new Establecimiento
                {
                    IdEstablecimiento = 0,
                    Nombre = "Seleccione el Establecimiento"
                });

                lineaDeCorte = "antes de Viewbag.EstablecimientoListFrecuente";
                ViewBag.establecimientoListFrecuentes =
                    establecimientosFrecuentes.OrderBy(x => x.IdEstablecimiento).ToList();

                lineaDeCorte = "antes de foreach";
                //List<SelectListItem> seguroList = new List<SelectListItem>();
                //ListaBl listaBl = new ListaBl();
                //foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(BusinessLayer.Compartido.Enums.OpcionLista.TipoSeguroSalud).ListaDetalle)
                //{
                //    lineaDeCorte = string.Format(" --- itemDetalle.GLosa: {0} - itemDetalle.IdDetalleLista: {1} ---", itemDetalle.Glosa, itemDetalle.IdDetalleLista);
                //    seguroList.Add(new SelectListItem
                //    {
                //        Text = itemDetalle.Glosa,
                //        Value = Convert.ToString(itemDetalle.IdDetalleLista)
                //    });
                //}

                //ViewBag.seguroList = seguroList;
                CargarSeguroList();

                PacienteBl pacienteBl = new PacienteBl();
                //Si se seleccionó el check entonces se actualiza la direccion del Paciente
                lineaDeCorte = "antes de if(chkActualizarUbicacion)";
                if (chkActualizarUbicacion) 
                {
                    lineaDeCorte = string.Format(" -orden.Paciente is null: {0} -", orden.Paciente == null);
                    //setear nuevo paciente si orden.Paciente es null
                    if (orden.Paciente == null) orden.Paciente = new Paciente { UbigeoActual = new Model.Ubigeo() };
                    orden.Paciente.IdUsuarioEdicion = ((Usuario)this.Session["UsuarioLogin"]).idUsuario;
                    orden.Paciente.UbigeoActual = new Model.Ubigeo
                    {
                        Id = ActualDepartamento + ActualProvincia + ActualDistrito
                    };
                    pacienteBl.UpdateDatosPaciente(orden.Paciente);
                }

                lineaDeCorte = "antes de if(codigoOrdenTmp !=null ...)";
                if (!string.IsNullOrWhiteSpace(codigoOrdenTmp))
                {
                    orden.codigoOrden = codigoOrdenTmp;
                }

                 orden.IdUsuarioRegistro = ((Usuario)this.Session["UsuarioLogin"]).idUsuario;

                lineaDeCorte = "antes de ORDEN EXAMENES";
                /*ORDEN EXAMENES*/
                List<OrdenExamen> ordenExamenListAgregados = new List<OrdenExamen>();
                if (this.Session["ordenExamenListAgregados"] != null)
                {
                    ordenExamenListAgregados = (List<OrdenExamen>)this.Session["ordenExamenListAgregados"];

                }
                orden.ordenExamenList = ordenExamenListAgregados;

                #endregion
                lineaDeCorte = "antes de ORDEN MUESTRA";
                /*ORDEN MUESTRA*/
                #region OrdenMuestra
                var ordenMuestraListAgregados = new List<OrdenMuestra>();
                //Juan Muga
                //if (Session["ordenMuestraListAgregados"] != null)
                if (Session["ordenMuestraListAgregados"] != null && ((List<OrdenMuestra>)Session["ordenMuestraListAgregados"]).Count > 0)
                {
                    ordenMuestraListAgregados = (List<OrdenMuestra>)Session["ordenMuestraListAgregados"];

                    foreach (var ordenMuestra in ordenMuestraListAgregados)
                    {
                        lineaDeCorte = " comienzo de revision si ordenMuestra.TipoMuestra es nulo";
                        if (ordenMuestra.TipoMuestra == null || (ordenMuestra.TipoMuestra != null && ordenMuestra.TipoMuestra.idTipoMuestra == 0))
                        {
                            var covidExamenes = new List<string>();
                            
                            covidExamenes.Add("8B5823DE-6DED-42A0-A0BA-44241DA4E40B");
                            covidExamenes.Add("2499D3AC-49B2-466A-ADC3-482AEF35A069");
                            covidExamenes.Add("1B2BEC28-772C-49DF-BCC2-85E0C5CCA667");
                            covidExamenes.Add("3DF3C0C8-3579-4F63-AB32-D8D24F92BA90");
                            covidExamenes.Add("0F35E2E1-41D6-4423-8458-F20DCE9C62BD");
                            covidExamenes.Add("C5F591F3-C858-42CA-8CC8-32B42972F13B");
                            covidExamenes.Add("9D454F39-ED5F-4C4A-8A1A-BD4E298534FE");
                            covidExamenes.Add("9EF3225D-C6AD-4B3A-BEB1-76430DB4DD77");//pruebas
                            //foreach (var item in covidExamenes)
                            //{
                            //    var g = Guid.Parse(item);

                            //}
                            //var testlist = covidExamenes.Select(s => Guid.Parse(s));
                            lineaDeCorte = " revision si ordenMuestra.TipoMuestra es nulo - llenar list<string>";
                            if (ordenExamenListAgregados.Any(x=> x.idOrdenExamen == ordenMuestra.idOrdenExamen && covidExamenes.Any(c=> c.ToLower() == x.Examen.idExamen.ToString().ToLower())))
                            {
                                lineaDeCorte = " comienzo de revision si ordenMuestra.TipoMuestra es nulo - entra a if(...)";
                                var oe = ordenExamenListAgregados.First(x => x.idOrdenExamen == ordenMuestra.idOrdenExamen && covidExamenes.Any(c => c.ToLower() == x.Examen.idExamen.ToString().ToLower()));
                                ITipoMuestraBl tipoMuestraBl = new TipoMuestraBl();
                                lineaDeCorte = " comienzo de revision si ordenMuestra.TipoMuestra es nulo - llamar a tipoMuestraBl.GetTiposMuestraByIdExamen";
                                var tipomuestras = tipoMuestraBl.GetTiposMuestraByIdExamen(oe.Examen.idExamen);
                                lineaDeCorte = " comienzo de revision si ordenMuestra.TipoMuestra es nulo - asigar tipomuestras.FirstOrDefault() a ordenMuestra.TipoMuestra";
                                ordenMuestra.TipoMuestra = tipomuestras.FirstOrDefault();
                            }
                        }

                        lineaDeCorte = " fin de revision si ordenMuestra.TipoMuestra es nulo";

                        var idMuestra = ordenMuestra.idOrdenMuestra;

                        if (tipoRegistro == TipoRegistroOrden.ORDEN_RECEPCION || tipoRegistro == TipoRegistroOrden.SOLO_ORDEN_MUESTRA)
                        {
                            var codificaciontmp = Request.Params["codificacion" + idMuestra];
                            ordenMuestra.MuestraCodificacion.codificacion = codificaciontmp;
                        }

                        var fechatmp = Request.Params["fechaMuestra" + idMuestra];
                        var fecha = fechatmp.Split('/');
                        ordenMuestra.fechaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]),
                            Convert.ToInt32(fecha[0]));

                        var horatmp = Request.Params["horaMuestra" + idMuestra];
                        var hora = horatmp.Split(':');
                        ordenMuestra.horaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]),
                            Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);
                    }
                }

                Session["ordenMuestraListAgregados"] = ordenMuestraListAgregados;
                orden.ordenMuestraList = ordenMuestraListAgregados;

                if (Session["establecimientoList"] == null && this.Session["orden"] != null)
                {
                    List<Establecimiento> sessionEstablecimientoList = new List<Establecimiento>();
                    sessionEstablecimientoList.Add(((Orden)this.Session["orden"]).establecimiento);
                    Session["establecimientoList"] = sessionEstablecimientoList;
                }

                var establecimientoList = (List<Establecimiento>)Session["establecimientoList"];

                var establecimiento = establecimientoList == null ? null : !establecimientoList.Any() ? null : establecimientoList.FirstOrDefault(x => x.IdEstablecimiento == orden.idEstablecimiento);

                if (establecimiento != null && establecimiento.IdEstablecimiento != 0)
                    orden.EstablecimientoCodigoUnico = establecimiento.CodigoUnico;

                #endregion
                /*FIN ORDEN MUESTRA*/

                lineaDeCorte = "antes de ORDEN MATERIAL";
                /*ORDEN MATERIAL*/
                List<OrdenMaterial> ordenMaterialListAgregados = new List<OrdenMaterial>();

                Boolean mxRechazo = false;
                if (this.Session["ordenMaterialListAgregados"] != null)
                {
                    ordenMaterialListAgregados = (List<OrdenMaterial>)this.Session["ordenMaterialListAgregados"];
                    for (int i = 0; i < ordenMaterialListAgregados.Count; i++)
                    {
                        var idRow = ordenMaterialListAgregados[i].idOrdenMaterial;
                        OrdenMaterial ordenMaterial = ordenMaterialListAgregados[i];
                        ordenMaterial.cantidad = 1;

                        var valor = Request.Params["laboratorio" + idRow];

                        ordenMaterial.Laboratorio = new Laboratorio();
                        ordenMaterial.Laboratorio.IdLaboratorio = Convert.ToInt32(Request.Params["laboratorio" + idRow]);
                        //if (tipoRegistro == TipoRegistroOrden.ORDEN_RECEPCION)
                        //{
                        //    ordenMaterial.Laboratorio.IdLaboratorio = 983;
                        //}
                        //else
                        //{
                        //    ordenMaterial.Laboratorio.IdLaboratorio = Convert.ToInt32(Request.Params["laboratorio" + idRow]);
                        //}


                        /*if (ordenMaterial.Laboratorio.Nombre == null || ordenMaterial.Laboratorio.Nombre.Equals(""))
                    {*/
                        //LaboratorioBl laboratorioBl = new LaboratorioBl();
                        //ordenMaterial.Laboratorio.Nombre = laboratorioBl.GetLaboratorioById(ordenMaterial.Laboratorio.IdLaboratorio).Nombre;
                        ordenMaterial.Laboratorio.Nombre = StaticCache.ObtenerLaboratorios().FirstOrDefault(x => x.IdEstablecimiento == ordenMaterial.Laboratorio.IdLaboratorio).NombreEstablecimiento;
                        //}

                        //ordenMaterial.Laboratorio.Nombre = Request.Params["laboratorioNombre" + idRow];

                        //laboratorio

                        //var fechatmp = Request.Params["fechaEnvio" + idRow]; juan muga
                        var fechatmp = DateTime.Now.ToShortDateString();//juan muga

                        String[] fecha = fechatmp.Split('/');
                        ordenMaterial.fechaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]),
                            Convert.ToInt32(fecha[0]));
                        //String[] hora = Request.Params["horaEnvio" + idRow].Split(':'); juan muga

                        //ordenMaterial.horaEnvio = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]),
                        //    Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);

                        ordenMaterial.horaEnvio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            DateTime.Now.Hour, DateTime.Now.Second, 0);//juan muga

                        String volumenMuestraColectadaForm = Request.Params["volumenMuestraColectada" + idRow];
                        if (volumenMuestraColectadaForm == null)
                        {
                            ordenMaterial.noPrecisaVolumen = 1;
                            ordenMaterial.volumenMuestraColectada = 0;
                        }
                        else
                        {
                            ordenMaterial.noPrecisaVolumen = 0;
                            ordenMaterial.volumenMuestraColectada = Convert.ToDecimal(volumenMuestraColectadaForm);
                        }

                        //Juan Muga - inicio
                        if (tipoRegistro == Enums.TipoRegistroOrden.ORDEN_RECEPCION)
                        {
                            //Juan Muga - fin
                            //   ordenMaterial.ordenMuestraRecepcionList = new List<OrdenMuestraRecepcion>();

                            //Se recorre la lista de ordenMuestraRecepcion
                            // foreac (int countMuestraRecepcion = 0; countMuestraRecepcion < ordenMaterial.cantidad ; countMuestraRecepcion++)
                            if (ordenMaterial.ordenMuestraRecepcionList.Count == 0)
                            {
                            }
                            else
                            {
                                foreach (
                                    OrdenMuestraRecepcion ordenMuestraRecepcion in
                                        ordenMaterial.ordenMuestraRecepcionList)
                                {
                                    ordenMuestraRecepcion.estatus = 0;

                                    ordenMuestraRecepcion.conforme =
                                        Request.Form["conforme" + ordenMuestraRecepcion.idOrdenMuestraRecepcion] == null
                                            ? 0
                                            : 1;
                                    if (ordenMuestraRecepcion.conforme == 0)
                                    {
                                        String motivoRechazo =
                                            this.Request.Params[
                                                "motivosRechazo" + ordenMuestraRecepcion.idOrdenMuestraRecepcion];
                                        string[] listaRechazos = motivoRechazo.Split(',');

                                        foreach (
                                            CriterioRechazo criterioRechazo in ordenMuestraRecepcion.criterioRechazoList
                                            )
                                        {
                                            foreach (String rechazo in listaRechazos)
                                            {
                                                if (Convert.ToInt32(rechazo) == criterioRechazo.IdCriterioRechazo)
                                                {
                                                    criterioRechazo.rechazado = true;
                                                    mxRechazo = criterioRechazo.rechazado;
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                this.Session["ordenMaterialListAgregados"] = ordenMaterialListAgregados;
                orden.ordenMaterialList = ordenMaterialListAgregados;

                /*FIN ORDEN MATERIAL*/


                this.Session["ordenDatoClinicoListAgregados"] = orden.ordenDatoClinicoList;

                lineaDeCorte = "antes de pacienteBl.getPacienteById";
                //PacienteBl pacienteBl = new PacienteBl();
                orden.Paciente = pacienteBl.getPacienteById(orden.Paciente.IdPaciente);

                List<Enfermedad> enfermedadListAgregados = new List<Enfermedad>();
                if (this.Session["enfermedadListAgregados"] != null)
                {
                    enfermedadListAgregados = (List<Enfermedad>)this.Session["enfermedadListAgregados"];
                }
                lineaDeCorte = "antes de ordenDatoClinicoList";
                List<OrdenDatoClinico> ordenDatoClinicoList = new List<OrdenDatoClinico>();
                var ValidacionDatoClinico = "";
                //     if (idDato != null)
                {
                    var datosClinicos =
                        enfermedadListAgregados.SelectMany(e => e.categoriaDatoList)
                            .SelectMany(c => c.OrdenDatoClinicoList);

                    if (datosClinicos.ToList().Count() == 0)
                    {
                        ValidacionDatoClinico = "Error: Es necesario configurar Datos Clínicos Epidemiológicos.";
                    }

                    lineaDeCorte = "antes de foreachDatosClinicos";
                    foreach (var ordenDatoClinico in datosClinicos)
                    {
                        var id = ordenDatoClinico.Enfermedad.idEnfermedad.ToString() +
                                 ordenDatoClinico.Dato.IdDato.ToString();

                        // if (!idDato.Contains(ordenDatoClinico.Dato.IdDato)) continue;


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
                            lineaDeCorte = "antes de ordenDatoClinico.Dato.IdDato";
                            //Si el dato clinico no existe se crea con los valores obtenidos
                            ordenDatoClinicoFormulario = new OrdenDatoClinico();
                            Dato datoClinico = new Dato();
                            datoClinico.IdDato = ordenDatoClinico.Dato.IdDato;
                            ordenDatoClinicoFormulario.Dato = datoClinico;
                            ordenDatoClinicoList.Add(ordenDatoClinicoFormulario);

                            lineaDeCorte = "despues de ordenDatoClinico.Dato.IdDato";
                            var formValue = Request.Form["valueDato" + id];
                            var checkNoPrecisaBoolean = Convert.ToBoolean(Request.Form["checkNoPrecisa" + id]);

                            if ((int)Enums.TipoCampo.CHECKBOX == ordenDatoClinico.Dato.IdTipo
                                || (int)Enums.TipoCampo.COMBO == ordenDatoClinico.Dato.IdTipo)
                            {
                                ordenDatoClinico.noPrecisa = string.IsNullOrWhiteSpace(formValue) || formValue=="0";
                                ordenDatoClinico.ValorString = string.IsNullOrWhiteSpace(formValue) || formValue == "0" ? "" : formValue;
                            }
                            else
                            {
                                ordenDatoClinico.noPrecisa = checkNoPrecisaBoolean;
                                ordenDatoClinico.ValorString = !checkNoPrecisaBoolean ? formValue : string.Empty;
                            }
                            //Juan Muga - inicio - 01/10/2018
                            int Proyecto = Request.Form["Proyecto.IdProyecto"] == null ? 0 : int.Parse(Request.Form["Proyecto.IdProyecto"]);
                            if (ordenDatoClinico.Dato.Obligatorio && String.IsNullOrEmpty(formValue) &&
                                (ordenDatoClinico.Dato.idProyecto == Proyecto || ordenDatoClinico.Dato.idProyecto == 0))
                            {
                                switch (ordenDatoClinico.Dato.idProyecto)
                                {
                                    case 0:
                                        if (Proyecto == 1)
                                        {
                                            ValidacionDatoClinico = string.IsNullOrEmpty(ValidacionDatoClinico) ? "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                            ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio." :
                                            ValidacionDatoClinico + "\n" + "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                            ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio.";
                                        }
                                        if (Proyecto == 2)
                                        {
                                            ValidacionDatoClinico = string.IsNullOrEmpty(ValidacionDatoClinico) ? "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                            ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio." :
                                            ValidacionDatoClinico + "\n" + "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                            ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio.";
                                        }
                                        break;
                                    case 1:
                                        ValidacionDatoClinico = string.IsNullOrEmpty(ValidacionDatoClinico) ? "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                            ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio." :
                                            ValidacionDatoClinico + "\n" + "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                            ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio.";
                                        break;
                                    case 2:
                                        ValidacionDatoClinico = string.IsNullOrEmpty(ValidacionDatoClinico) ? "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                            ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio." :
                                            ValidacionDatoClinico + "\n" + "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                            ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio.";
                                        break;
                                    default:
                                        break;
                                }
                            }
                            //Juan Muga - fin - 01/10/2018
                            ordenDatoClinicoFormulario.noPrecisa = ordenDatoClinico.noPrecisa;
                            ordenDatoClinicoFormulario.ValorString = ordenDatoClinico.ValorString;
                        }
                    }

                    lineaDeCorte = "despues de ordenDatoClinicoList";
                }

                this.Session["enfermedadListAgregados"] = enfermedadListAgregados;

                orden.enfermedadList = enfermedadListAgregados;

                this.Session["ordenidEstablecimiento"] = orden.idEstablecimiento;
                this.Session["ordenNombreEstablecimiento"] = ViewBag.nombreEstablecimiento;


                //var ordenBl = OrdenBlUtils.GetOrdenBlInstancia();
                using (var ordenBl = new OrdenBl())
                {
                    lineaDeCorte = "antes de if (orden.codigoOrden == null)";
                    if (string.IsNullOrWhiteSpace(orden.codigoOrden))
                    {
                        //asignar codigo orden generado en nuevoSP
                        orden.codigoOrden = codigoOrdenGenerado;

                        ordenCodigoOrdenIsNull = true;
                        //Juan Muga - Inicio - 01/10/2018
                        if (!string.IsNullOrEmpty(ValidacionDatoClinico))
                        {
                            ViewBag.textoRegistro = ValidacionDatoClinico;
                            ViewBag.existeError = "S";
                            orden.codigoOrden = string.Empty;
                            codigoOrdenTmp = string.Empty;
                            return View("New", orden);
                        }
                        //Juan Muga - Fin - 01/10/2018
                        if ((tipoRegistro == TipoRegistroOrden.ORDEN_RECEPCION || tipoRegistro == TipoRegistroOrden.SOLO_ORDEN_MUESTRA) && ExisteCodigoMuestraVacio(ordenMuestraListAgregados))
                        {
                            ViewBag.textoRegistro = "Ingrese el código de muestra en todos los tipos de muestra";
                            ViewBag.existeError = "S";
                            orden.codigoOrden = string.Empty;
                            codigoOrdenTmp = string.Empty;
                            return View("New", orden);
                        }
                        //if (tipoRegistro == TipoRegistroOrden.SOLO_ORDEN_MUESTRA && ExisteCodigoMuestraVacio(ordenMuestraListAgregados))
                        //{
                        //    ViewBag.textoRegistro = "Ingrese el código de muestra en todos los tipos de muestra";
                        //    return View("New", orden);
                        //}
                        //JRCR-REQ02
                        var existeSolicitante = new SolicitanteBl().GetCodigoSolicitante(orden.solicitante);
                        if (!existeSolicitante)
                        {
                            ViewBag.textoRegistro = "Solicitante inexistente, por favor registrar al nuevo solicitante.";
                            ViewBag.existeError = "S";
                            orden.codigoOrden = string.Empty;
                            codigoOrdenTmp = string.Empty;
                            return View("New", orden);
                        }

                        var existeInvalido = ordenMuestraListAgregados.Any(x => ValidaCodificacion(x.MuestraCodificacion.codificacion, orden.idEstablecimiento, x.ordenExamenList));
                        if (existeInvalido)
                        {
                            ViewBag.textoRegistro = "La orden contiene código(s) de muestra inválido(s).";
                            ViewBag.existeError = "S";
                            orden.codigoOrden = string.Empty;
                            codigoOrdenTmp = string.Empty;
                            ViewBag.SolicitanteNombre = string.Empty;
                            if (Session["SolicitanteSeleccionadoId"] != null && Session["SolicitanteSeleccionadoNombre"] != null)
                            {
                                //ViewBag.SolicitanteId = solicitanteSeleccionado.FirstOrDefault().Value;
                                orden.solicitante = (string)Session["SolicitanteSeleccionadoId"];
                                ViewBag.SolicitanteNombre = (string)Session["SolicitanteSeleccionadoNombre"];
                                var solicitanteListItem = new List<SelectListItem>();
                                solicitanteListItem.Add(new SelectListItem { Value = orden.solicitante, Text = ViewBag.SolicitanteNombre });
                                ViewBag.Solicitantes = solicitanteListItem;
                            }
                            return View("New", orden);
                        }

                        orden.estatus = 0;
                        string vista = "New";
                        int pr = 0;
                        foreach (var item in ordenExamenListAgregados)
                        {
                            if (item.Examen.PruebaRapida == 1)
                            {
                                pr = 1;
                            }
                        }
                        lineaDeCorte = "antes de if (guardarfinalizar == 1)";
                        if (guardarfinalizar == "1" && pr == 0)
                        {
                            orden.estatus = 1;
                            vista = (tipoRegistro == TipoRegistroOrden.ORDEN_RECEPCION) ? "SearchRecepcion" : "Index";
                        }
                        else if (guardarfinalizar == "1" && pr == 1)
                        {
                            orden.estatus = 1;
                            vista = "Show";
                        }

                        lineaDeCorte = "antes de ordenBl.InsertOrden(orden, tipoRegistro)";
                        
                        Session["orden"] = orden;
                        ordenBl.InsertOrden(orden, tipoRegistro);
                        //ordenBl.ProcesarOrden(orden, tipoRegistro);
                        Session["orden"] = orden;

                        if (chkActualizarUbicacion)
                        {
                            ViewBag.textoRegistro = "Se actualizó los datos del paciente con código: n° " +
                                                    orden.Paciente.NroDocumento + " y se generó el número de orden " +
                                                    orden.codigoOrden;
                        }
                        else
                        {
                            ViewBag.textoRegistro = "Se generó el número de orden " + orden.codigoOrden + " al paciente " + orden.Paciente.nombreCompleto;
                        }

                        lineaDeCorte = string.Format("Pasó InsertOrden(orden) - IdOrden creado: {0} - antes de LoadNombreSolicitante(orden.solicitante) - orden.solicitante: {1}", orden.idOrden, orden.solicitante);
                        LoadNombreSolicitante(orden.solicitante);

                        if (guardarfinalizar == "1" && pr == 0)
                        {
                            TempData["shortMessage"] = ViewBag.textoRegistro;
                            lineaDeCorte = "Orden creada, se llama a DelSessionOrden para limpiar session";
                            DelSessionOrden();
                            return RedirectToAction(vista, "Paciente");
                        }
                        else
                        {
                            //en que casos entraria aqui???
                            return View(vista, orden);
                        }
                    }
                    else // finalizar orden if (orden.codigoOrden != null && (int)this.Session["pasoRegistro"] == 2)
                    {
                        ordenCodigoOrdenIsNull = false;
                        var ordenSession = (Orden)Session["orden"];
                        lineaDeCorte = string.Format("ELSE - obtener Session[orden]  - ordenSession.IdOrden: ", ordenSession.idOrden);
                        //ordenSession.estatus = tipoRegistro == TipoRegistroOrden.SOLO_ORDEN ? 1 : 2;
                        switch (tipoRegistro)
                        {
                            case TipoRegistroOrden.SOLO_ORDEN:
                            case TipoRegistroOrden.SOLO_ORDEN_MUESTRA:
                                ordenSession.estatus = 1;
                                break;
                            case TipoRegistroOrden.ORDEN_RECEPCION:
                                ordenSession.estatus = 2;
                                break;
                            case TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO:
                                ordenSession.estatus = 3;
                                break;
                            default:
                                break;
                        }

                        ordenSession.observacion = orden.observacion;
                        ordenSession.nroOficio = orden.nroOficio;
                        ordenSession.IdUsuarioRegistro = orden.IdUsuarioRegistro;
                        
                        //Juan Muga - Inicio - 01/10/2018
                        if (!string.IsNullOrEmpty(ValidacionDatoClinico))
                        {
                            ViewBag.textoRegistro = ValidacionDatoClinico;
                            return View("New", orden);
                        }
                        //Juan Muga - Fin - 01/10/2018
                        if ((tipoRegistro == TipoRegistroOrden.ORDEN_RECEPCION || tipoRegistro == TipoRegistroOrden.SOLO_ORDEN_MUESTRA) && ExisteCodigoMuestraVacio(ordenMuestraListAgregados))
                        {
                            ViewBag.textoRegistro = "Ingrese el código de muestra en todos los tipos de muestra";
                            ViewBag.existeError = "S";
                            return View("New", ordenSession);
                        }

                        var existeInvalido = ordenMuestraListAgregados.Any(x => ValidaCodificacion(x.MuestraCodificacion.codificacion, ordenSession.idEstablecimiento, x.ordenExamenList));
                        if (existeInvalido)
                        {
                            ViewBag.textoRegistro = "La orden contiene código(s) de muestra inválido(s).";
                            ViewBag.existeError = "S";
                            return View("New", ordenSession);
                        }
                        else
                        {
                            //Juan Muga - inicio
                            //if(!ordenSession.ordenMuestraList.Any())
                            //{
                            //    ordenSession.ordenMuestraList = ordenMuestraListAgregados;
                            //}
                            if (!ordenSession.ordenMuestraList.Any() || ordenSession.ordenMuestraList.Any(a => string.IsNullOrEmpty(a.MuestraCodificacion.codificacion)))
                            {
                                foreach (var item in ordenSession.ordenMuestraList)
                                {
                                    item.MuestraCodificacion.codificacion = Request.Form["codificacion" + item.idOrdenMuestra];
                                }
                                if ((tipoRegistro == TipoRegistroOrden.ORDEN_RECEPCION || tipoRegistro == TipoRegistroOrden.SOLO_ORDEN_MUESTRA) && ExisteCodigoMuestraVacio(ordenSession.ordenMuestraList))
                                {
                                    ViewBag.textoRegistro = "Ingrese el código de muestra en todos los tipos de muestra";
                                    ViewBag.existeError = "S";
                                    return View("New", ordenSession);
                                }
                                if (ordenSession.ordenMuestraList.Any(x => ValidaCodificacion(x.MuestraCodificacion.codificacion, ordenSession.idEstablecimiento, x.ordenExamenList)))
                                {
                                    ViewBag.textoRegistro = "La orden contiene código(s) de muestra inválido(s).";
                                    ViewBag.existeError = "S";
                                    return View("New", ordenSession);
                                }
                                //ordenSession.ordenMuestraList.First().MuestraCodificacion.codificacion = codificacionFinalizar;
                                Session["ordenMuestraListAgregados"] = ordenSession.ordenMuestraList;
                            }

                            //Juan Muga = Fin
                        }

                        //new bsPage().LogInfo(ordenSession, "LogNetLab");
                        lineaDeCorte = string.Format("ELSE - antes de ordenBl.UpdateOrden(ordenSession, tipoRegistro) - ordenSession.idOrden: {0} .", ordenSession.idOrden);
                        var result = ordenBl.UpdateOrden(ordenSession, tipoRegistro);
                        lineaDeCorte = string.Format("ELSE - despues de ordenBl.UpdateOrden(ordenSession, tipoRegistro) - ordenSession.idOrden: {0} .", ordenSession.idOrden);
                        //Autor : Marcos Mejia 
                        //Descricpion : Envio de correo a UTM informando rechazo.
                        //Fecha: 25/09/2018
                        //if (mxRechazo)
                        //{
                        //    var usuario = ordenMuestraBl.ConsultaDatosUsuario(ordenSession.codigoOrden);
                        //    orden.usuario = new Usuario();
                        //    orden.usuario = usuario;
                        //    var correo = new EnvioCorreo();
                        //    string destinatario = orden.usuario.correo;
                        //    string asunto = "Rechazo de muestra Neltab v2";
                        //    string contenido = "Estimado(a) usuario: " + orden.usuario.nombres + "\n" + "Se rechazó un examen con Código de Orden N° " + usuario.cargo + ", para mayor detalle revisar la bandeja de Muestras Rechazadas en Netlabv2";
                        //    correo.EnviarCorreo(destinatario, asunto, contenido);
                        //}

                        if (result)
                        {
                            if (tipoRegistro == TipoRegistroOrden.SOLO_ORDEN || tipoRegistro == TipoRegistroOrden.SOLO_ORDEN_MUESTRA)
                            {
                                DelSessionOrden();
                            }
                            else
                            {
                                ViewBag.referenciar = 1;
                                Session["orden"] = ordenSession;
                            }
                        }
                        else
                        {
                            ViewBag.textoRegistro = ordenBl.ErrorMessage;
                            return View("New", ordenSession);
                        }

                        var usuario = ((Usuario)Session["UsuarioLogin"]);
                        ViewBag.nombreUsuario = usuario.nombres + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                        lineaDeCorte = string.Format("Pasó ordenBl.UpdateOrden(ordenSession, tipoRegistro) - IdOrden : {0} - antes de LoadNombreSolicitante(orden.solicitante) ordenSession.solicitante: {1}", ordenSession.idOrden, ordenSession.solicitante);
                        LoadNombreSolicitante(ordenSession.solicitante);

                        return View("Show", ordenSession);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.textoRegistro = ex.Message + "->" + ex.StackTrace;
                //Juan Muga
                orden = (Orden)Session["Orden"];
                orden.codigoOrden = string.Empty;
                string idpaciente = string.Empty;
                if(orden != null && orden.Paciente != null)
                {
                    idpaciente = orden.Paciente.IdPaciente.ToString();
                }

                string stringObj = string.Format(" - idpaciente: {0} - idUsuario: {1} - idEstablecimiento: {2}", idpaciente, ((Usuario)Session["UsuarioLogin"]).idUsuario, idEstablecimiento);
                new bsPage().LogError(ex, "LogNetLab", stringObj, lineaDeCorte);
                return View("New", orden);
            }
        }
        public void CargarOrdenExamen(DatosOrdenExamenSesison datos)
        {
            StaticCache.CargarDatosOrdenExamen(datos);
        }
        public void CargarSeguroList()
        {
            var seguroList = StaticCache.ObtenerTipoSeguroSalud();
            if (seguroList == null || !seguroList.Any())
            {
                if (Session["OrdenTipoSeguro"] != null)
                {
                    if (((List<SelectListItem>)Session["OrdenTipoSeguro"]).Any())
                    {
                        seguroList = (List<SelectListItem>)Session["OrdenTipoSeguro"];
                    }
                    else
                    {
                        seguroList = ObtenerTipoSeguroSalud();
                    }
                }
            }

            ViewBag.seguroList = seguroList;
        }

        public void CargarProyectos()
        {
            IProyectoBl proyectoBl = new ProyectoBl();
            List<Proyecto> proyectoList = new List<Proyecto>();
            proyectoList = StaticCache.ObtenerListaProyectos();
            if (proyectoList == null || !proyectoList.Any())
            {
                if (Session["OrdenProyectos"] != null)
                {
                    if (((List<SelectListItem>)Session["OrdenProyectos"]).Any())
                    {
                        proyectoList = (List<Proyecto>)Session["OrdenProyectos"];
                    }
                    else
                    {
                        proyectoList = ObtenerListaProyectos();
                    }
                }
            }

            ViewBag.proyectoList = proyectoList;
        }

        #region Solicitante
        public string GetSolicitantes()
        {
            var data = Request.Params["data[q]"];

            List<OrdenSolicitante> solicitantes = new List<OrdenSolicitante>();
            SolicitanteBl solicitanteBL = new SolicitanteBl();
            solicitantes = solicitanteBL.GetListaSolicitante(data);

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeDatos = false;

            foreach (var s in solicitantes)
            {
                var text = string.Format("{0} - {1} {2} {3}", s.codigoColegio, s.apellidoPaterno, s.apellidoMaterno, s.Nombres);

                resultado += "{\"id\":\"" + s.idSolicitante + "\",\"text\":\"" + text + "\"},";
                existeDatos = true;
            }

            if (existeDatos)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }

        public PartialViewResult LoadSolicitante()
        {
            return PartialView("_Solicitante");
        }

        public bool ValidarCodigoColegio(string codigoColegio)
        {
            return new SolicitanteBl().GetCodigoSolicitantePorCodigo(codigoColegio);
        }

        //private void LoadSolicitantes()
        //{
        //    List<OrdenSolicitante> solicitantes = new List<OrdenSolicitante>();
        //    if (Session["solicitantes"] != null)
        //    {
        //        solicitantes = (List<OrdenSolicitante>)Session["solicitantes"];
        //    }
        //    else
        //    {
        //        SolicitanteBl solicitanteBL = new SolicitanteBl();
        //        solicitantes = solicitanteBL.GetListaSolicitante();
        //        Session["solicitantes"] = solicitantes;
        //    }

        //    ViewBag.Solicitantes = solicitantes.Select(s => new SelectListItem
        //    {
        //        Text = string.Format("{0} - {1} {2} {3}", s.codigoColegio, s.apellidoPaterno, s.apellidoMaterno, s.Nombres),
        //        Value = s.idSolicitante.ToString()
        //    });
        //}

        private void LoadNombreSolicitante(string solicitante)
        {
            SolicitanteBl solicitanteBL = new SolicitanteBl();
            ViewBag.SolicitanteNombre = solicitanteBL.GetSolicitanteById(solicitante);
        }

        #endregion

        /// <summary>
        /// Descripción: Metodo que valída el código de muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenMuestraListAgregados"></param>
        /// <returns></returns>
        private static bool ExisteCodigoMuestraVacio(IEnumerable<OrdenMuestra> ordenMuestraListAgregados)
        {
            var i = ordenMuestraListAgregados.Any(ExisteCodigoMuestraVacio);
            return i;
        }
        /// <summary>
        /// Descripción: Metodo que valída el codigo de muestra
        /// Enlazado con el Metodo ExisteCodigoMuestraVacio
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenMuestra"></param>
        /// <returns></returns>
        private static bool ExisteCodigoMuestraVacio(OrdenMuestra ordenMuestra)
        {
            return ordenMuestra.MuestraCodificacion?.codificacion == null ||
                ordenMuestra.MuestraCodificacion.codificacion.Trim().IsEmpty();
        }

        /// <summary>
        /// Descripción: Metodo que valida si el codigo de muestra es inválido.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="codificacion"></param>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        private static bool ValidaCodificacion(string codificacion, int idEstablecimiento,List<OrdenExamen> ordenExamenList)
        {
            if (string.IsNullOrEmpty(codificacion))
                return false;

            using (var muestraDal = new MuestraDal())
            {

                var muestraCodificacion = muestraDal.CodificacionByCodigo(codificacion, idEstablecimiento);
                if (muestraCodificacion == null) //Codificacion que no existe
                    return true;

                if (ordenExamenList.Any(p => p.Examen.idExamen.ToString().ToUpper() == GetSetting<string>("CargaViral") || p.Examen.idExamen.ToString().ToUpper() == GetSetting<string>("Cd4") || p.Examen.idExamen.ToString().ToUpper() == GetSetting<string>("Covid19")))
                {
                    if (string.IsNullOrEmpty(muestraCodificacion.codificacionLineal))
                    {
                        return true;
                    }
                }


                return muestraCodificacion.Estado == 1;//Codificacion que ya pertenece a otra orden
            }
        }

        /// <summary>
        /// Descripción: Metodo que verifica si existen codificaciones disponibles
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        /// <returns></returns>
        private static bool CodificacionesDisponibles(Orden orden)
        {
            using (var muestraDal = new MuestraDal())
            {
                var list = muestraDal.MuestraCodificacionDisponiblesByEstablecimiento(orden.idEstablecimiento);

                if (list == null || !list.Any())
                    return false;

                if (orden.ordenMuestraList != null && list.Count < orden.ordenMuestraList.Count)
                    return false;

                return true;
            }
        }

        #region Search
        /// <summary>
        /// Descripción: Controlador que realiza una busqueda de ordenes con un rango de fechas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult Search(string codigoOrden = null)
        {
            ViewBag.fechaDesde = DateTime.Now.ToDefaultDateFormatString();
            ViewBag.fechaHasta = DateTime.Now.ToDefaultDateFormatString();

            //return View();
            return Search(null, ViewBag.fechaDesde, ViewBag.fechaHasta, string.IsNullOrEmpty(codigoOrden) ? "" : codigoOrden, "", 0, "1");
        }

        /// <summary>
        ///  Descripción: Controlador que realiza la busqueda de muestras y retorna una lista paginada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="docIdentidad"></param>
        /// <param name="nroOficio"></param>
        /// <param name="estadoOrden"></param>
        /// <param name="esFechaSolicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(int? page, string fechaDesde, string fechaHasta, string docIdentidad, string nroOficio, int estadoOrden, string esFechaSolicitud)
        {

            var dateFrom = new DateTime();
            var dateTo = new DateTime();
            var fechaSolicitud = 0;

            var pageNumber = page ?? 1;

            if (!string.IsNullOrEmpty(fechaDesde))
                dateFrom = DateTime.ParseExact(fechaDesde, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(fechaHasta))
                dateTo = DateTime.ParseExact(fechaHasta, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (!string.IsNullOrEmpty(esFechaSolicitud))
                fechaSolicitud = int.Parse(esFechaSolicitud);

            OrdenBl ordenBl = new OrdenBl();
            int idEstablecimientoSeleccionado = EstablecimientoSeleccionado == null ? 0 : EstablecimientoSeleccionado.IdEstablecimiento;

            List<Orden> ordenList = ordenBl.GetOrdenes(dateFrom, dateTo, docIdentidad.Trim(), nroOficio.Trim(), estadoOrden, fechaSolicitud, idEstablecimientoSeleccionado, Logueado.idUsuario);

            ViewBag.fechaDesde = dateFrom.ToDefaultDateFormatString();
            ViewBag.fechaHasta = dateTo.ToDefaultDateFormatString();
            ViewBag.docIdentidad = docIdentidad.Trim();
            if (ordenList != null)
            {
                //var pageOfOrdenMuestra = ordenList.ToPagedList(pageNumber, GetSetting<int>(PageSize));

                var pageOfOrden = ordenList.ToPagedList(pageNumber, 50);

                // ViewBag.idEstablecimiento = idEstablecimientoF;
                //ViewBag.recepcionado = estadoOrdenF;

                return View(pageOfOrden);
            }

            return View();
        }

        /// <summary>
        /// Descripción: Controlador que realiza la busqueda de ordenes REGISTRADAS
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public ActionResult Show(Guid idOrden)
        {
            //orden.IdUsuarioRegistro = ((Usuario)this.Session["UsuarioLogin"]).idUsuario;

            //ORDEN
            OrdenBl ordenBl = new OrdenBl();
            Orden orden = ordenBl.GetOrdenById(idOrden);
            if (orden.estatus == 0)
            {
                this.Session["orden"] = orden;
            }

            LoadNombreSolicitante(orden.solicitante);
            return View("Show", orden);

        }
        [HttpPost]
        public ActionResult ShowEditRom(string idOrden, string Origen, string Controlador)
        {
            //ORDEN
            OrdenBl ordenBl = new OrdenBl();
            Orden orden = ordenBl.GetOrdenById(Guid.Parse(idOrden));
            this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO;
            ViewBag.tipoRegistro = Enums.TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO;
            ViewBag.viewOrigen = Origen;
            ViewBag.idEstablecimiento = EstablecimientoSeleccionado.IdEstablecimiento;
            this.Session["viewOrigen"] = Origen;
            this.Session["viewControlador"] = Controlador;
            LoadNombreSolicitante(orden.solicitante);
            this.Session["orden"] = orden;
            var listaBl = new ListaBl();
            var tipoDocumentoList = new List<SelectListItem>();

            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });

            ViewBag.tipoDocumentoList = tipoDocumentoList;
            var solicitanteListItem = new List<SelectListItem>();
            solicitanteListItem.Add(new SelectListItem { Value = orden.solicitante, Text = ViewBag.SolicitanteNombre });
            ViewBag.Solicitantes = solicitanteListItem;
            return PartialView("_ShowOrden", orden);

        }
        /// <summary>
        /// Descripción: Constrolador que realiza las ediciones de los registros de las ordenes
        /// Luego de editar la informacion regresa a la pantalla de busqueda.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Guid idOrden)
        {
            Orden orden = (Orden)this.Session["orden"];
            if (orden.idOrden == idOrden)
            {
                this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.SOLO_ORDEN_MUESTRA;
                ViewBag.tipoRegistro = Enums.TipoRegistroOrden.SOLO_ORDEN_MUESTRA;
                LoadNombreSolicitante(orden.solicitante);
                return View("New", orden);
            }
            else
            {
                return RedirectToAction("Search");
            }
        }
        [HttpPost]
        public ActionResult EditOrden(Orden orden, string tipoDocumento, string nroDocumento)
        {
            //return idOrden;
            var ordenSession = (Orden)Session["orden"];
            var controladorOrigen = (string)(this.Session["viewControlador"]);
            var vistaOrigen = (string)(this.Session["viewOrigen"]);
            this.Session["tipoRegistro"] = Enums.TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO;
            ViewBag.tipoRegistro = Enums.TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO;
            var tipoRegistro = Enums.TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO;
            LoadNombreSolicitante(ordenSession.solicitante);
            //Orden Muestra
            if (vistaOrigen == "BusquedaMuestrasIngresadas")
            {
                var ordenMuestraListAgregados = new List<OrdenMuestra>();
                if (ordenSession.ordenMuestraList != null)
                {
                    ordenMuestraListAgregados = ordenSession.ordenMuestraList;

                    foreach (var ordenMuestra in ordenMuestraListAgregados)
                    {
                        var xordenMuestra = new OrdenMuestra();
                        var idMuestra = ordenMuestra.idOrdenMuestra;
                        if (string.IsNullOrEmpty(Request.Params["fechaMuestra" + idMuestra]) || string.IsNullOrEmpty(Request.Params["horaMuestra" + idMuestra]))
                        {
                            break;
                        }
                        var fechatmp = Request.Params["fechaMuestra" + idMuestra];
                        var fecha = fechatmp.Split('/');
                        ordenMuestra.fechaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]),
                            Convert.ToInt32(fecha[0]));

                        var horatmp = Request.Params["horaMuestra" + idMuestra];
                        var hora = horatmp.Split(':');
                        ordenMuestra.horaColeccion = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]),
                            Convert.ToInt32(fecha[0]), Convert.ToInt32(hora[0]), Convert.ToInt32(hora[1]), 0);
                    }
                }
                ordenSession.ordenMuestraList = ordenMuestraListAgregados;

                //Orden Material
                List<OrdenMaterial> ordenMaterialListAgregados = new List<OrdenMaterial>();
                if (ordenSession.ordenMaterialList != null)
                {
                    ordenMaterialListAgregados = ordenSession.ordenMaterialList;
                    for (int i = 0; i < ordenMaterialListAgregados.Count; i++)
                    {
                        var idRow = ordenMaterialListAgregados[i].idOrdenMaterial;
                        OrdenMaterial ordenMaterial = ordenMaterialListAgregados[i];
                        var valor = Request.Params["laboratorio" + idRow];

                        if (!string.IsNullOrEmpty(valor))
                        {
                            ordenMaterial.Laboratorio = new Laboratorio();
                            ordenMaterial.Laboratorio.IdLaboratorio = Convert.ToInt32(Request.Params["laboratorio" + idRow]);
                        }
                    }
                }
                ordenSession.ordenMaterialList = ordenMaterialListAgregados;
                //Edicion Paciente
                if (!string.IsNullOrEmpty(tipoDocumento) && !string.IsNullOrEmpty(nroDocumento))
                {
                    var existePaciente = new PacienteBl().GetPacientesByTipoNroDocumento(int.Parse(tipoDocumento), nroDocumento);
                    if (existePaciente.Count() > 0)
                    {
                        ordenSession.idPaciente = existePaciente.FirstOrDefault().IdPaciente;
                    }
                }
            }
            else
            {
                ordenSession.ordenMuestraList = null;
                ordenSession.ordenMaterialList = null;
            }


            //Edicion Orden Datos Clinicos, Observaciones
            this.Session["ordenDatoClinicoListAgregados"] = ordenSession.ordenDatoClinicoList;
            var ordenBl = new OrdenBl();
            List<Enfermedad> enfermedadListAgregados = new List<Enfermedad>();
            enfermedadListAgregados = ordenSession.enfermedadList;
            List<OrdenDatoClinico> ordenDatoClinicoList = new List<OrdenDatoClinico>();
            var ValidacionDatoClinico = "";
            //     if (idDato != null)
            {
                var datosClinicos = enfermedadListAgregados.SelectMany(e => e.categoriaDatoList).SelectMany(c => c.OrdenDatoClinicoList);

                foreach (var ordenDatoClinico in datosClinicos)
                {
                    var id = ordenDatoClinico.Enfermedad.idEnfermedad.ToString() + ordenDatoClinico.Dato.IdDato.ToString();

                    // if (!idDato.Contains(ordenDatoClinico.Dato.IdDato)) continue;


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
                        var checkNoPrecisaBoolean = Convert.ToBoolean(Request.Form["checkNoPrecisa" + id]);

                        if ((int)Enums.TipoCampo.CHECKBOX == ordenDatoClinico.Dato.IdTipo
                            || (int)Enums.TipoCampo.COMBO == ordenDatoClinico.Dato.IdTipo)
                        {
                            ordenDatoClinico.noPrecisa = string.IsNullOrWhiteSpace(formValue) || formValue == "0";
                            ordenDatoClinico.ValorString = string.IsNullOrWhiteSpace(formValue) || formValue == "0" ? "" : formValue;
                        }
                        else
                        {
                            ordenDatoClinico.noPrecisa = checkNoPrecisaBoolean;
                            ordenDatoClinico.ValorString = !checkNoPrecisaBoolean ? formValue : string.Empty;
                        }

                        int Proyecto = Request.Form["Proyecto.IdProyecto"] == null ? 0 : int.Parse(Request.Form["Proyecto.IdProyecto"]);
                        if (ordenDatoClinico.Dato.Obligatorio && String.IsNullOrEmpty(formValue) &&
                            (ordenDatoClinico.Dato.idProyecto == Proyecto || ordenDatoClinico.Dato.idProyecto == 0))
                        {
                            switch (ordenDatoClinico.Dato.idProyecto)
                            {
                                case 0:
                                    if (Proyecto == 1)
                                    {
                                        ValidacionDatoClinico = string.IsNullOrEmpty(ValidacionDatoClinico) ? "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                        ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio." :
                                        ValidacionDatoClinico + "\n" + "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                        ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio.";
                                    }
                                    if (Proyecto == 2)
                                    {
                                        ValidacionDatoClinico = string.IsNullOrEmpty(ValidacionDatoClinico) ? "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                        ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio." :
                                        ValidacionDatoClinico + "\n" + "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                        ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio.";
                                    }
                                    break;
                                case 1:
                                    ValidacionDatoClinico = string.IsNullOrEmpty(ValidacionDatoClinico) ? "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                        ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio." :
                                        ValidacionDatoClinico + "\n" + "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                        ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio.";
                                    break;
                                case 2:
                                    ValidacionDatoClinico = string.IsNullOrEmpty(ValidacionDatoClinico) ? "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                        ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio." :
                                        ValidacionDatoClinico + "\n" + "Para el motivo: " + ordenDatoClinico.Dato.Proyecto +
                                        ", el campo: " + ordenDatoClinico.Dato.Prefijo + " es obligatorio.";
                                    break;
                                default:
                                    break;
                            }
                        }
                        ordenDatoClinicoFormulario.noPrecisa = ordenDatoClinico.noPrecisa;
                        ordenDatoClinicoFormulario.ValorString = ordenDatoClinico.ValorString;
                    }
                }
            }

            this.Session["enfermedadListAgregados"] = enfermedadListAgregados;

            ordenSession.enfermedadList = enfermedadListAgregados;
            
            ordenSession.estatus = 3; // Edicion de orden
            ordenSession.observacion = string.IsNullOrEmpty(orden.observacion) ? "" : orden.observacion.TrimEnd();
            ordenSession.nroOficio = orden.nroOficio;
            ordenSession.IdUsuarioEdicion = Logueado.idUsuario;
            ordenSession.solicitante = orden.solicitante;
            ordenSession.fechaIngresoINS = orden.fechaIngresoINS;
            ordenSession.fechaReevaluacionFicha = orden.fechaReevaluacionFicha;
            ordenSession.fechaSolicitud = orden.fechaSolicitud;

            if (!string.IsNullOrEmpty(ValidacionDatoClinico))
            {
                ViewBag.textoRegistro = ValidacionDatoClinico;
            }

            try
            {
                ordenBl.UpdateOrden(ordenSession, tipoRegistro);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            ViewBag.codigoOrden = ordenSession.codigoOrden;
            
            return RedirectToAction(vistaOrigen, controladorOrigen, new { codigoOrden = ordenSession.codigoOrden });
        }
        [HttpPost]
        public ActionResult RechazarOrden(string idRechazo, string idOrden, string Controlador, string codigoOrden)
        {
            try
            {
                new OrdenBl().RechazarOrden(idRechazo, idOrden, Controlador, Logueado.idUsuario, codigoOrden);
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        #endregion
        #region PruebaRapida
        /// <summary>
        /// Descripción: Actualización de la orden para las Pruebas Rápidas
        /// Author: Juan Muga.
        /// Fecha Creacion:  20/11/2017
        /// Modificación: 
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public string RecepcionEESSandRecepcionLab(string idOrden)
        {
            Guid IdOrdenGuid = new Guid(idOrden);
            var ordenBl = new OrdenBl();
            return ordenBl.RecepcionEESSandRecepcionLab(IdOrdenGuid,Logueado.idUsuario);            
        }

        #endregion

        public ActionResult ImprimirOrden(string idorden)
        {
            Orden orden = new Orden();
            OrdenBl _orden = new OrdenBl();
            Guid _idOrden = new Guid(idorden);
            orden = _orden.GetOrdenById(_idOrden);
            return new ViewAsPdf("ImprimirOrden", orden);
        }

        public ActionResult ObtenerFicha(string idOrden)
        {
            string ContentJpg = "application/pdf";
            var bl = new OrdenBl();
            var ficha = bl.ObtenerFicha(idOrden);
            return File(ficha.file, ContentJpg);
        }

        public JsonResult GetCiudadProcedencia(string Prefix)
        {
            var bl = new PacienteBl();
            var ciudadList = new List<Ciudad>();
            ciudadList = bl.GetCiudadProcedencia(Prefix);
            return Json(ciudadList, JsonRequestBehavior.AllowGet);
        }

        public string ValidarPersona(string nroDocumento)
        {
            //Session["ejecutor"] = null;
            Usuario usuario = new Usuario();
            UsuarioBl usuarioBl = new UsuarioBl();
            usuario = usuarioBl.ValidarDatosUsuario(usuario, nroDocumento);
            string ejecutor = usuario.nombres + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
            ViewBag.MensajeServicio = usuarioBl.ErrorMessage;
            //Session["ejecutor"] = usuario;
            return ejecutor;
        }

        public void AsignarSessionSolicitanteIngresado(string idSolicitante)
        {
            SolicitanteBl solicitanteBL = new SolicitanteBl();
            string solicitantenombres = solicitanteBL.GetSolicitanteById(idSolicitante);
            //List<SelectListItem> solicitanteLista = new List<SelectListItem>();
            //solicitanteLista.Add(new SelectListItem
            //{
            //    Text = solicitantenombres,
            //    Value = idSolicitante
            //});

            Session["SolicitanteSeleccionadoId"] = idSolicitante;
            Session["SolicitanteSeleccionadoNombre"] = solicitantenombres;
        }

        private List<SelectListItem> ObtenerTipoSeguroSalud()
        {
            List<SelectListItem> seguroList = new List<SelectListItem>();
            ListaBl listaBl = new ListaBl();
            foreach (ListaDetalle itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoSeguroSalud).ListaDetalle)
                seguroList.Add(new SelectListItem { Text = itemDetalle.Glosa, Value = Convert.ToString(itemDetalle.IdDetalleLista) });

            Session["OrdenTipoSeguro"] = seguroList;
            return seguroList;
        }

        private List<Proyecto> ObtenerListaProyectos()
        {
            IProyectoBl proyectoBl = new ProyectoBl();
            var proyectos = proyectoBl.GetProyectos();
            Session["OrdenProyectos"] = proyectos;

            return proyectos;
        }
    }
}
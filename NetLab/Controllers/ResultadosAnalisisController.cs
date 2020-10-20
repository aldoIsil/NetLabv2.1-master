using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Plantilla;
using DataLayer;
using Model;
using Model.ViewData;
using NetLab.Helpers;
using NetLab.Models;
using Newtonsoft.Json;
using Rotativa;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.ComponentModel;
using ClosedXML.Excel;
using NetLab.Extensions.ActionResults;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Configuration;

namespace NetLab.Controllers
{
    [Serializable()]
    public class ResultadosAnalisisController : ParentController
    {
        protected int itemsPorPag = 20;
        protected int idRechazoFechasRom = 89;

        string userName = ConfigurationManager.AppSettings["usuario"];
        string password = ConfigurationManager.AppSettings["password"];
        string dominio = ConfigurationManager.AppSettings["dominio"];
        // GET: ResultadosAnalisis
        /// <summary>
        /// Descripción: Action del controlador para ejecutar la busqueda de los ordenes listas para:
        /// Recepcionar.
        /// Ingresar Resultados.
        /// Validar Resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string codigoOrden = null, string estadoResultado = null)
        {
            //ViewBag.IsSearch = false;
            //return View();

            ViewBag.fechaDesde = DateTime.Now.ToDefaultDateFormatString();
            ViewBag.fechaHasta = DateTime.Now.ToDefaultDateFormatString();
            //return View();

            return Index(1, "1","0", ViewBag.fechaDesde, ViewBag.fechaHasta, "", "", "", codigoOrden, string.IsNullOrEmpty(codigoOrden) ? estadoResultado : "0", "", "", "");


        }
        /// <summary>
        /// Descripción: Action del controlador para ejecutar la busqueda de los ordenes listas para:
        /// Recepcionar.
        /// Ingresar Resultados.
        /// Validar Resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nroOficio"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="estadoResultado"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(int? page, string esFechaRegistro, string esFechaRegistroRom,string fechaDesde, string fechaHasta, string nroDocumento, string nroOficio, string codigoOrden, string codigoMuestra, string estadoResultado, string idEnfermedadFiltro, string hdnExamen, string ExamenFiltro)
        {
            Session["ordenes"] = null;
            ViewBag.IsSearch = true;

            var fechaDesdeA = new DateTime();
            var fechaHastaA = new DateTime();
            var estadoResultadoF = 0;

            if (estadoResultado != null)
                estadoResultadoF = int.Parse(estadoResultado);


            //obtener idEstablecimiento            
            var idEstablecimientoLogin = EstablecimientoSeleccionado.IdEstablecimiento;

            var fechaDesdeCriteria = fechaDesde ?? string.Empty;
            var fechaHastaCriteria = fechaHasta ?? string.Empty;

            if (!fechaDesdeCriteria.Equals(""))
                fechaDesdeA = DateTime.Parse(fechaDesdeCriteria, CultureInfo.CreateSpecificCulture("es-PE"));
            if (!fechaHastaCriteria.Equals(""))
                fechaHastaA = DateTime.Parse(fechaHastaCriteria, CultureInfo.CreateSpecificCulture("es-PE"));

            var sEnfermedad = "0";
            var sExamen = String.IsNullOrEmpty(hdnExamen) ? Guid.Empty : Guid.Parse(hdnExamen);
            esFechaRegistro = string.IsNullOrEmpty(esFechaRegistro) ? "0" : esFechaRegistro;
            var tipo = int.Parse(esFechaRegistro);

            if (ExamenesByUsuario == null) return View();

            var ordenBl = new IngresoResultadosBl();

            var ordenes = ordenBl.GetOrdenMuestraResultadosByUser(
                tipo, idEstablecimientoLogin, Logueado.idUsuario, nroDocumento, fechaDesdeA, fechaHastaA,
                nroOficio, codigoOrden, codigoMuestra, estadoResultadoF, int.Parse(sEnfermedad), sExamen);

            Session["ordenes"] = ordenes;

            if (ordenes == null) return View();

            ViewBag.TotalRegistros = ordenes.Count;
            int maximaCantidadFilas = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BandejasMaxRows"]) ? ordenes.Count : int.Parse(ConfigurationManager.AppSettings["BandejasMaxRows"]);

            ViewBag.MostrarMensaje = ordenes.Count > maximaCantidadFilas;

            ViewBag.page = page;
            ViewBag.esFechaRegistro = esFechaRegistro;
            ViewBag.fechaDesde = fechaDesde;
            ViewBag.fechaHasta = fechaHasta;
            ViewBag.nroDocumento = nroDocumento;
            ViewBag.nroOficio = nroOficio;
            ViewBag.codigoOrden = codigoOrden;
            ViewBag.codigoMuestra = codigoMuestra;
            ViewBag.ExamenFiltro = ExamenFiltro;
            ViewBag.idExamenFiltro = hdnExamen;
            if (!string.IsNullOrEmpty(codigoOrden))
            {
                ViewBag.estadoResultado = string.IsNullOrEmpty(codigoOrden) ? estadoResultadoF.ToString() : estadoResultado.ToString();
            }

            return View(ordenes.Take(maximaCantidadFilas).ToList());
        }
        /// <summary>
        /// Descripción: Action para obtener informacion de las muestras de la orden seleccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ValidarMuestras2(Guid id, Guid idExamen)
        {
            var ordenBl = new IngresoResultadosBl();
            var idEstablecimientoLogin = EstablecimientoSeleccionado.IdEstablecimiento;

            var ordenMuestraBl = new OrdenMuestraBl();
            var ordenExamen = ordenMuestraBl.GetInfoOrden(id, EstablecimientoSeleccionado.IdEstablecimiento).ordenInfoList.Where(o => o.ConformeR == 1 && o.idExamen == idExamen).ToList().Count() >= 1 ? ordenMuestraBl.GetInfoOrden(id, EstablecimientoSeleccionado.IdEstablecimiento) : null;


            var orden = ordenBl.GetMuestrasValidarResultados(id, Logueado.idUsuario, idEstablecimientoLogin);
            if (orden.muestrasValidar.Count() == 0)
            {
                orden = ordenBl.GetRechazarExamen(id, Logueado.idUsuario, idEstablecimientoLogin);
            }

            orden.ordenInfoListnew = ordenExamen.ordenInfoList.Where(x => x.ConformeR == 1 && x.idExamen == idExamen).ToList();
            orden.muestrasValidar = orden.muestrasValidar.Where(r => r.idExamen == idExamen).ToList();


            var listaCriterios = new Dictionary<int, List<SelectListItem>>();
            var criteriosBl = new CriterioRechazoBl();

            var fechaColeccion = DateTime.MinValue;

            foreach (var item in orden.muestrasValidar)
            {

                if (fechaColeccion < item.fechaColeccion)
                    fechaColeccion = item.fechaColeccion;

                if (listaCriterios.ContainsKey(item.idTipoMuestra)) continue;

                listaCriterios[item.idTipoMuestra] = new List<SelectListItem>();

                foreach (var crItem in criteriosBl.GetCriteriosByTipoMuestra(item.idTipoMuestra, 3))
                {
                    listaCriterios[item.idTipoMuestra].Add(new SelectListItem
                    {
                        Text = crItem.Glosa,
                        Value = crItem.IdCriterioRechazo.ToString()
                    });
                }
            }

            ViewBag.listaCriterios = listaCriterios;
            return PartialView("ValidarMuestras", orden);
        }
        public string MPrueba()
        {
            return "ok";
        }
        /// <summary>
        /// Descripción: Action para ejecutar y registrar la validacion de las muestras.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codigoMuestra"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidarMuestras(Guid id, Guid[] codigoMuestra)
        {
            //ese parametro string observacionrechazo ya no es necesario porque lo chapas del Request.Params
            var ordenBl = new IngresoResultadosBl();
            var orden = new OrdenIngresarResultadoVd
            {
                muestrasValidar = new List<MuestraResultadoVd>(),
                fechaRecepcion = DateTime.Now
            };

            var ordenMuestraBl = new OrdenMuestraBl();
            var ordenExamen = ordenMuestraBl.GetInfoOrden(id, EstablecimientoSeleccionado.IdEstablecimiento);
            //.ConformeR == 1 ? ordenMuestraBl.GetInfoOrden(id, EstablecimientoSeleccionado.IdEstablecimiento) : null;
            if (ordenExamen.ordenInfoList.Where(x => x.ConformeR == 1).Count() > 0)
            {
                orden.ordenInfoListnew = ordenExamen.ordenInfoList.Where(x => x.ConformeR == 1).ToList();
            }

            foreach (var t in codigoMuestra)
            {
                int conforme;
                //bool bRechazoFechaRom = false;

                if (Request.Params["conforme_" + t] != null)
                {
                    conforme = 1;
                }
                else
                {
                    conforme = 0;
                    var criterios = Request.Params["criterioRechazo_" + t];

                    var rechazos = criterios.Split(',');
                    foreach (var rechazo in rechazos)
                    {
                        var criterioRechazo = int.Parse(rechazo);
                        var item = new CriterioRechazo
                        {
                            IdOrdenMuestraRecepcion = t,
                            IdUsuarioRegistro = Logueado.idUsuario,
                            IdCriterioRechazo = criterioRechazo,
                            observacionrechazo = Request.Params["observacionrechazo_" + t]
                        };
                        //if (rechazo == idRechazoFechasRom.ToString()) bRechazoFechaRom = true;
                        ordenBl.RegistroCriterioRechazoResultado(item);
                    }
                }
                ordenBl.ActualizaRecepcionConformeResultado(t, conforme, Logueado.idUsuario);
                //Rechazo de muestras del oficio.
                //if (bRechazoFechaRom) ordenBl.RechazoFechaRom(orden.nroOficio, Logueado.idUsuario, idRechazoFechasRom);
            }

            return PartialView("ValidarMuestras", orden);
        }

        /// <summary>
        /// Descripción: Action para obtener información de la orden para la recepcion correspondiente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        ///               Se agrega el idxamen como parámetro para solo levantar el examen seleccionado - jmuga - 20/11/2018
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        //[HttpPost]
        public ActionResult RecepcionPendiente(string id, string idExamen)
        {
            var ordenBl = new IngresoResultadosBl();
            var idEstablecimientoLogin = EstablecimientoSeleccionado.IdEstablecimiento;

            var orden = ordenBl.GetMuestrasPendientesRecepcionLAB(Guid.Parse(id), Logueado.idUsuario, idEstablecimientoLogin);
            var listaCriterios = new Dictionary<int, List<SelectListItem>>();
            var criteriosBl = new CriterioRechazoBl();

            var fechaColeccion = DateTime.MinValue;

            orden.muestrasPendientesRecepcion = orden.muestrasPendientesRecepcion.Where(x => x.idExamen == Guid.Parse(idExamen)).ToList();
            foreach (var item in orden.muestrasPendientesRecepcion)
            {

                if (fechaColeccion < item.fechaColeccion)
                    fechaColeccion = item.fechaColeccion;

                if (listaCriterios.ContainsKey(item.idTipoMuestra)) continue;

                listaCriterios[item.idTipoMuestra] = new List<SelectListItem>();

                foreach (var crItem in criteriosBl.GetCriteriosByTipoMuestra(item.idTipoMuestra, 3))
                {
                    listaCriterios[item.idTipoMuestra].Add(new SelectListItem
                    {
                        Text = crItem.Glosa,
                        Value = crItem.IdCriterioRechazo.ToString()
                    });
                }
            }
            ViewBag.listaCriterios = listaCriterios;
            return PartialView("RecepcionPendiente", orden);


        }
        public ActionResult RecepcionPendiente2(string id, string idExamen)
        {
            var ordenBl = new IngresoResultadosBl();
            var idEstablecimientoLogin = EstablecimientoSeleccionado.IdEstablecimiento;

            var orden = ordenBl.GetMuestrasPendientesRecepcionLAB(Guid.Parse(id), Logueado.idUsuario, idEstablecimientoLogin);
            var listaCriterios = new Dictionary<int, List<SelectListItem>>();
            var criteriosBl = new CriterioRechazoBl();

            var fechaColeccion = DateTime.MinValue;

            orden.muestrasPendientesRecepcion = orden.muestrasPendientesRecepcion.Where(x => x.idExamen == Guid.Parse(idExamen)).ToList();
            foreach (var item in orden.muestrasPendientesRecepcion)
            {

                if (fechaColeccion < item.fechaColeccion)
                    fechaColeccion = item.fechaColeccion;

                if (listaCriterios.ContainsKey(item.idTipoMuestra)) continue;

                listaCriterios[item.idTipoMuestra] = new List<SelectListItem>();

                foreach (var crItem in criteriosBl.GetCriteriosByTipoMuestra(item.idTipoMuestra, 3))
                {
                    listaCriterios[item.idTipoMuestra].Add(new SelectListItem
                    {
                        Text = crItem.Glosa,
                        Value = crItem.IdCriterioRechazo.ToString()
                    });
                }
            }
            ViewBag.listaCriterios = listaCriterios;
            return PartialView("RecepcionPendiente", orden);


        }
        /// <summary>
        /// Descripción: Action para recepcionar las ordenes en el laboratorio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codigoMuestra"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RecepcionPendiente(Guid[] codigoMuestra, int[] secuenciaMuestra)
        {
            var ordenBl = new IngresoResultadosBl();
            var ListOrdenRecepcion = new List<MuestraResultadoVd>();

            int i = 0;
            foreach (var t in codigoMuestra)
            {
                var OrdenRecepcion = new MuestraResultadoVd
                {
                    idOrdenMuestraRecepcion = t,
                    idUsuario = Logueado.idUsuario,
                    secuenObtencion = secuenciaMuestra[i]
                };
                ListOrdenRecepcion.Add(OrdenRecepcion);
                i++;
            }

            var resultado = ordenBl.ActualizaOrdenMuestraRecepcionLAB(ListOrdenRecepcion);
            var orden = new OrdenIngresarResultadoVd
            {
                muestrasPendientesRecepcion = new List<MuestraResultadoVd>(),
                fechaRecepcion = DateTime.Now,
                error = resultado
            };

            return PartialView("RecepcionPendiente", orden);
        }


        /// <summary>
        /// Descripción: Action para obtener informacion de la muestra validada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MuestrasValidadas(Guid id, Guid idExamen)
        {

            var ordenBl = new IngresoResultadosBl();
            var idEstablecimientoLogin = EstablecimientoSeleccionado.IdEstablecimiento;

            var ordenMuestraBl = new OrdenMuestraBl();
            var ordenExamen = ordenMuestraBl.GetInfoOrden(id, EstablecimientoSeleccionado.IdEstablecimiento).ConformeR == 1 ? ordenMuestraBl.GetInfoOrden(id, EstablecimientoSeleccionado.IdEstablecimiento) : null;


            var orden = ordenBl.GetMuestrasValidadas(id, Logueado.idUsuario, idEstablecimientoLogin);

            orden.ordenInfoListnew = ordenExamen.ordenInfoList.Where(x => x.ConformeR == 1 && x.idExamen.ToString().ToUpper() == idExamen.ToString().ToUpper()).ToList();

            orden.muestrasValidar = orden.muestrasValidar.Where(q => q.idExamen == idExamen).ToList();

            var listaCriterios = new Dictionary<int, List<SelectListItem>>();
            var criteriosBl = new CriterioRechazoBl();

            var fechaColeccion = DateTime.MinValue;

            foreach (var item in orden.muestrasValidar)
            {

                if (fechaColeccion < item.fechaColeccion)
                    fechaColeccion = item.fechaColeccion;

                if (listaCriterios.ContainsKey(item.idTipoMuestra)) continue;

                listaCriterios[item.idTipoMuestra] = new List<SelectListItem>();

                foreach (var crItem in criteriosBl.GetCriteriosByTipoMuestra(item.idTipoMuestra, 3))
                {
                    listaCriterios[item.idTipoMuestra].Add(new SelectListItem
                    {
                        Text = crItem.Glosa,
                        Value = crItem.IdCriterioRechazo.ToString()
                    });
                }
            }

            ViewBag.listaCriterios = listaCriterios;
            return PartialView("MuestrasValidadas", orden);
        }

        /// <summary>
        /// Descripción: Action que identifica una orden lista para el ingreso de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SeleccionarAP(Guid idOrdenExamen, string pr)
        {
            IngresoResultadosBl ordenBl = new IngresoResultadosBl();
            //var xorden = new List<OrdenIngresarResultadoVd>();
            int idUsuario = (pr != "" || pr != null) ? 1 : Logueado.idUsuario;
            OrdenIngresarResultadoVd orden = ordenBl.GetAreaProcesamientoOrdenUsuario(idOrdenExamen, idUsuario, EstablecimientoSeleccionado.IdEstablecimiento);
            var view = "";
            if (orden != null)// && orden.areas.Count() > 1)
            {
                view = "SeleccionarAP";
            }
            return PartialView(view, orden);
            //else
            //{
            //return PartialView("O");
            //var ordenBl = new IngresoResultadosBl();
            //var xOrden = ordenBl.DatosOrden(orden.idOrden);
            //var idEstablecimiento = EstablecimientoSeleccionado.IdEstablecimiento;
            //var examenes = ordenBl.ExamenesResultadosAnalito(xOrden.idOrden, idOrdenExamen, orden.areas.FirstOrDefault().IdAreaProcesamiento, Logueado.idUsuario, xOrden.EdadAnios, xOrden.Genero, idEstablecimiento);
            //var fechaColeccion = DateTime.MinValue;
            //Session["idGenero"] = xOrden.Genero.ToString();

            //foreach (var item in examenes)
            //{
            //    if (fechaColeccion < item.Muestra.fechaColeccion)
            //    {
            //        fechaColeccion = item.Muestra.fechaColeccion;
            //    }
            //}

            //ViewBag.examenes = examenes;

            //return PartialView("IngresarResultados", xOrden);
            //}

        }
        /// <summary>
        /// Descripción: Action que identifica el area de procesamiento de una orden lista para el ingreso de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>
        public ActionResult IngresarResultados(Guid id, int idArea, Guid idOrdenExamen)
        {
            var ordenBl = new IngresoResultadosBl();
            var orden = ordenBl.DatosOrden(id);
            var idEstablecimiento = EstablecimientoSeleccionado.IdEstablecimiento;
            var examenes = ordenBl.ExamenesResultadosAnalito(id, idOrdenExamen, idArea, Logueado.idUsuario, orden.EdadAnios, orden.Genero, idEstablecimiento);
            var fechaColeccion = DateTime.MinValue;
            Session["idGenero"] = orden.Genero.ToString();

            foreach (var item in examenes)
            {
                if (fechaColeccion < item.Muestra.fechaColeccion)
                {
                    fechaColeccion = item.Muestra.fechaColeccion;
                }
            }
            ViewBag.examenes = examenes;

            return PartialView("IngresarResultados", orden);
        }

        public string InterpretacionResultado(Guid[] idOrdenExamen, List<AnalitoOpcionResultado> resul)
        {
            BusinessLayer.Reportes.ReporteResultadosBl resultado = new BusinessLayer.Reportes.ReporteResultadosBl();
            var datosInterpretacion = resultado.GetInterpretacionExamen(idOrdenExamen);

            string interpretacion = "";
            string intp = "";
            List<string> lista = new List<string>();
            foreach (var opcion in resul.OrderBy(a => a.IdAnalitoOpcionParent).ToList())

            {
                foreach (var i in datosInterpretacion)
                {
                    if (opcion.GlosaParent == i.GlosaParent && opcion.Glosa == i.Glosa)
                    {
                        intp = i.Interpretacion;
                        lista.Add(intp);
                    }
                }
            }
            if (lista.Count > 0)
            {
                var repetido = lista.GroupBy(x => x).OrderByDescending(x => x.Count()).First().ToList();

                if (repetido.Count != 0)
                {
                    interpretacion = repetido[0].ToString();
                }
                else
                {
                    interpretacion = lista[0].ToString();
                }
            }
            return interpretacion;
        }

        /// <summary>
        /// Descripción: Action para el registro de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Se realizaron las modificaciones para el registro de los datos configurados por componente. Juan Muga
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="resultados"></param>
        /// <param name="idOrdenResultadoAnalito"></param>
        /// <param name="idAnalito"></param>
        /// <param name="resultado"></param>
        /// <param name="observacion"></param>
        /// <param name="metodo"></param>
        /// <param name="ordenExamen"></param>
        /// <param name="estatusOrdenExamen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegistroResultados(Guid idOrden, Guid[] resultados, Guid[] idOrdenResultadoAnalito, Guid[] idAnalito,
            string[] resultado, string[] observacion, int[] metodo, Guid[] ordenExamen, int[] estatusOrdenExamen, int ListaPlataforma)
        {
            // List<Models.AnalitoValues> deserializedObject = JsonConvert.DeserializeObject<List<Models.AnalitoValues>>(Request.Form["jsonvalues"]);
            List<Model.AnalitoValues> deserializedObject = JsonConvert.DeserializeObject<List<Model.AnalitoValues>>(Request.Form["jsonvalues"]);
            List<ObservacionesValues> deserializedObservaciones = JsonConvert.DeserializeObject<List<ObservacionesValues>>(Request.Form["observacionesjsonvalues"]);

            List<AnalitoOpcionResultado> resul = null;
            string convResultado = "";
            string interpretacion = "";

            if (deserializedObject == null || !deserializedObject.Any())
            {
                return PartialView("Blank");
            }
            else
            {
                resul = new DetalleAnalitoDal().GetAnalitosbyIdAnalitoResultado(deserializedObject);
                convResultado = new OrdenDal().RetornaResultado(resul);
                interpretacion = InterpretacionResultado(ordenExamen, resul);
            }

            var ordenBl = new IngresoResultadosBl();
            var detalleAnalitoBl = new DetalleAnalitoBl();

            var disabledCount = 0;

            for (var i = 0; i < ordenExamen.Length; i++)
            {
                if (!(estatusOrdenExamen[i] == 10 || estatusOrdenExamen[i] == 11))
                {
                    disabledCount = disabledCount + 1;

                    if (!resultados.Contains(ordenExamen[i])) continue;

                    var idExamenMetodo = 0;

                    if (metodo != null && metodo[disabledCount - 1] > -1)
                        idExamenMetodo = metodo[disabledCount - 1];

                    var ordenResultadoAnalito = ordenBl.GetOrdenResultadoAnalito(ordenExamen[i]);

                    var lista = new List<OrdenResultadoAnalito>();
                    foreach (var item in deserializedObject.GroupBy(x => x.IdOrdenResultadoAnalito))
                    {
                        string serializedObject = JsonConvert.SerializeObject(item.ToList());

                        var analito = new OrdenResultadoAnalito
                        {
                            idOrdenResultadoAnalisis = item.Key,
                            resultado = serializedObject,
                            idExamenMetodo = idExamenMetodo,
                            observacion = deserializedObservaciones.Any(x => x.IdOrdenResultadoAnalito == item.Key) ? deserializedObservaciones.First(x => x.IdOrdenResultadoAnalito == item.Key).ObservacionContent : string.Empty,
                            convResultado = convResultado,
                            interpretacion = interpretacion,
                            Plataforma = ListaPlataforma
                        };

                        if (deserializedObservaciones.Any(x => x.IdOrdenResultadoAnalito == item.Key))
                        {
                            analito.observacion = deserializedObservaciones.First(x => x.IdOrdenResultadoAnalito == item.Key).ObservacionContent;
                        }

                        lista.Add(analito);
                    }
                    ordenBl.RegistrarResultadosOrdenAnalito(lista, Logueado.idUsuario);

                    var detalle = new List<OrdenResultadoAnalito>();
                    OrdenResultadoAnalito analitoDetalle = null;

                    foreach (var item in resul)
                    {
                        if (item.idAnalitoCabeceraVariable != 0)
                        {
                            analitoDetalle = new OrdenResultadoAnalito
                            {
                                idOrdenResultadoAnalisis = ordenResultadoAnalito[0],
                                idAnalisis = item.idAnalitoCabeceraVariable,
                                idSecuencia = 1,
                                convResultado = (item.idAnalitoCabeceraVariable == 1 || item.idAnalitoCabeceraVariable == 2) ? convResultado : item.Glosa
                            };
                            detalle.Add(analitoDetalle);
                        }
                    }
                    ordenBl.RegistoResultadoAnalitoDetalle(detalle);
                }
            }
            return PartialView("Blank");
        }
        /// <summary>
        /// Descripción: Action para mostrar el detalle de la prueba seleccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarDetalle(string[] idOrdenExamen, int idPlataforma)
        {
            var genero = int.Parse(Session["idGenero"].ToString());

            var detalleanalitoBl = new DetalleAnalitoBl();
            var detalle = detalleanalitoBl.GetAnalitoByOrdenExamenAndGenero(idOrdenExamen, genero,idPlataforma);
            Session["ListaDetalles"] = detalle;
            //return PartialView("DetalleResultadoAnalisis", detalle);
            return Json(JsonConvert.SerializeObject(detalle));
        }

        public ActionResult DetalleOpcion(string idOpcion)
        {

            var detalle = (List<ResultAnalito>)Session["ListaDetalles"];
            //var detalle = Session["ListaDetalles"];
            List<AnalitoOpcionResultado> LstOpcionDetalle = new List<AnalitoOpcionResultado>();
            List<AnalitoOpcionResultado> LstOpcionDetalles = new List<AnalitoOpcionResultado>();
            var listaDetalles = new List<ResultAnalito>();
            foreach (var item in detalle)
            {
                LstOpcionDetalle = item.Metodos;
            }

            foreach (var item in LstOpcionDetalle)
            {
                if (item.IdAnalitoOpcionParent.ToString().Trim().ToUpper() == idOpcion)
                {
                    LstOpcionDetalles.Add(item);
                }
            }
            foreach (var item in detalle)
            {
                var detalles = new ResultAnalito
                {
                    IdAnalito = item.IdAnalito,
                    IdOrdenResultadoAnalito = item.IdOrdenResultadoAnalito,
                    Resultado = item.Resultado,
                    Unidad = item.Unidad,
                    tipo = item.tipo,
                    estatusE = item.estatusE,
                    Metodos = LstOpcionDetalles
                };
                listaDetalles.Add(detalles);
            }


            return PartialView("DetalleOpcionesResultado", listaDetalles);
        }
        /// <summary>
        /// Descripción: Action para finalizar el registro del resultado.
        /// Estado = 10
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Finalizar(string[] idOrdenExamen)
        {
            var ordenExamenBl = new OrdenExamenBl();

            var ordenExamen = new OrdenExamen
            {
                estatusE = 10, //Resultado por verificar
                IdUsuarioEdicion = Logueado.idUsuario
            };

            foreach (var element in idOrdenExamen)
            {
                ordenExamen.idOrdenExamen = Guid.Parse(element);
                ordenExamenBl.UpdateOrdenExamenStatus(ordenExamen);
            }

            return PartialView("Blank");
        }
        /// <summary>
        /// Descripción: Metodo que retorna examenes concatenado con el codigo y la descripcion.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 15/11/2017
        /// Fecha Modificación: 15/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public String GetOrdenExamenByOrden(Guid idOrden)
        {
            String data = this.Request.Params["data[q]"];
            OrdenMuestraDal examenDal = new OrdenMuestraDal();

            List<OrdenExamen> examenList = examenDal.OrdenExamenByOrden(idOrden);
            int IdEstablecimiento = EstablecimientoSeleccionado.IdEstablecimiento;

            Session["ordenExamenListAgregados"] = examenList;

            string resultado = "{\"q\":\"" + data + "\",\"results\":[";
            Boolean existeEnfermedad = false;
            foreach (var examen in examenList)
            {
                resultado += "{\"idEnfermedad\":\"" + examen.idEnfermedad + "\",\"idEstablecimiento\":\"" + IdEstablecimiento + "\",\"idExamen\":\"" + examen.idExamen + "\",\"idTipoMuestra\":\"" + examen.IdTipoMuestra + "\"},";
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
        /// Descripción: Controlador usado para mostrar el popup de Enfermedades. ANALISTA
        /// Formulario enlazado _FormPopupEnfermedadExamen
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 15/11/2017
        /// Fecha Modificación: 10/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowPopupNuevoEnfermedadExamen(Guid idOrden)
        {
            var ordenExamenListAgregados = new List<OrdenExamen>();

            //OrdenMuestraDal examenDal = new OrdenMuestraDal();
            ViewBag.idOrdenAnalista = idOrden;
            //ordenExamenListAgregados = examenDal.OrdenExamenByOrden(idOrden);
            //var ordenExamen = ordenExamenListAgregados.Last();

            ViewBag.valueLaboratorioPreSeleccionada = EstablecimientoSeleccionado.IdEstablecimiento;
            ViewBag.textoLaboratorioPreSeleccionada = EstablecimientoSeleccionado.Nombre;
            //if (ordenExamenListAgregados.Count > 0)
            //if (ordenExamenListAgregados.Count() == 1)
            //{
            //    ViewBag.valueEnfermedadPreSeleccionada = ordenExamen.Enfermedad.idEnfermedad;
            //    ViewBag.textoEnfermedadPreSeleccionada = ordenExamen.Enfermedad.Snomed + " - " + ordenExamen.Enfermedad.nombre;
            //    ViewBag.valueLaboratorioPreSeleccionada = EstablecimientoSeleccionado.IdEstablecimiento;
            //    ViewBag.textoLaboratorioPreSeleccionada = EstablecimientoSeleccionado.Nombre;
            //    ViewBag.valueTipoMuestraPreSeleccionada = ordenExamen.IdTipoMuestra;
            //    ViewBag.textoTipoMuestraPreSeleccionada = ordenExamen.nombreTipoMuestra;
            //}
            //else
            //{
            //    ViewBag.valueEnfermedadPreSeleccionada = ordenExamen.Enfermedad.idEnfermedad;
            //    ViewBag.textoEnfermedadPreSeleccionada = ordenExamen.Enfermedad.Snomed + " - " + ordenExamen.Enfermedad.nombre;
            //    ViewBag.valueLaboratorioPreSeleccionada = EstablecimientoSeleccionado.IdEstablecimiento;
            //    ViewBag.textoLaboratorioPreSeleccionada = EstablecimientoSeleccionado.Nombre;
            //}


            ViewBag.tipoRegistro = 3;

            ViewBag.examenList = new List<Examen>();
            ViewBag.tipoMuestraList = new List<TipoMuestra>();

            return PartialView("_FormPopupEnfermedadExamen");
        }

        //SOTERO     
        public ActionResult DetalleOpciones(string NewOpcionesList, string idOpcionParent)
        {
            return PartialView("_DetallesOpciones", JsonConvert.DeserializeObject<List<AnalitoOpcionResultado>>(NewOpcionesList));
        }
        public ActionResult CargaResultadoMasivo()
        {
            return PartialView("_FormCargaResultadoMasivo");
        }
        /// <summary>
        /// Descripción: Metodo para la creacion del JSON a patir del archivo excel
        /// Author: Juan Muga
        /// Fecha Creacion: 25/07/2018
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ExcelValuesJson"></param>
        /// <returns></returns>
        public string ProcesarResultadosExcelJson(List<ExcelResultadoTbGenotype> ExcelValuesJson)
        {
            if (!ExcelValuesJson.Any() || ExcelValuesJson.FirstOrDefault().codigoMuestra == null)
            {
                return "Error: Por favor verificar el contenido del excel.";// PartialView("Blank");
            }
            var ordenBl = new IngresoResultadosBl();
            var ResultadoBl = new ResultadosBl();
            var lista = new List<OrdenResultadoAnalito>();
            var JsonRes = new Models.AnalitoValues();
            var LstJsonRes = new List<Models.AnalitoValues>();
            foreach (var item in ExcelValuesJson)
            {
                //armar Lista Json: por fila sale un json 
                var d = ResultadoBl.OrdenAnalitoResultadobyCodigoOrden(item.codigoOrden, item.codigoMuestra, item.tipoMuestra, EstablecimientoSeleccionado.IdEstablecimiento, "Genotype");
                if (d == null)
                {
                    return "Error: Por favor verificar que la muestra se procese en su establecimiento.";
                }
                switch (item.resultado)
                {
                    case "COMPLEJO MYCOBACTERIUM TUBERCULOSIS":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1097).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1097",
                            Tipo = "combo"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    case "NO COMPLEJO MYCOBACTERIUM TUBERCULOSIS":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1097).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1098",
                            Tipo = "combo"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    case "INDETERMINADO":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1097).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1099",
                            Tipo = "combo"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    default:
                        break;
                }
                switch (item.Rifampicina)
                {
                    case "RESISTENTE":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1103).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "on",
                            Tipo = "checkbox"
                        };
                        LstJsonRes.Add(JsonRes);
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1103).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1103",
                            Tipo = "checkbox"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    case "SENSIBLE":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1102).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "on",
                            Tipo = "checkbox"
                        };
                        LstJsonRes.Add(JsonRes);
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1102).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1102",
                            Tipo = "checkbox"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    default:
                        break;
                }
                switch (item.Isoniacida)
                {
                    case "RESISTENTE":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1106).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "on",
                            Tipo = "checkbox"
                        };
                        LstJsonRes.Add(JsonRes);
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1106).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1106",
                            Tipo = "checkbox"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    case "SENSIBLE":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1105).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "on",
                            Tipo = "checkbox"
                        };
                        LstJsonRes.Add(JsonRes);
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1105).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1105",
                            Tipo = "checkbox"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    default:
                        break;
                }
                switch (item.GenRPOAltaResistente)
                {
                    case "SI":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1104).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1104",
                            Tipo = "combo"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    default:
                        break;
                }
                switch (item.GenINHaAltaResistencia)
                {
                    case "SI":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1107).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1107",
                            Tipo = "combo"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    default:
                        break;
                }
                switch (item.GenKatGBajaResistencia)
                {
                    case "SI":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1108).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1108",
                            Tipo = "combo"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    default:
                        break;
                }
                switch (item.GenINHaAltaResistencia_GenKatGBajaResistencia)
                {
                    case "SI":
                        JsonRes = new Models.AnalitoValues
                        {
                            AnalitoId = d.IdAnalito,
                            IdOrdenExamen = d.IdOrdenExamen,
                            IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                            Id = d.Metodos.Where(x => x.IdAnalitoOpcionResultado == 1109).FirstOrDefault().IdAnalitoOpcionParent.ToString().TrimEnd(),
                            Value = "1109",
                            Tipo = "combo"
                        };
                        LstJsonRes.Add(JsonRes);
                        break;
                    default:
                        break;
                }
            }

            foreach (var item in LstJsonRes.GroupBy(x => x.IdOrdenResultadoAnalito))
            {
                string serializedObject = JsonConvert.SerializeObject(item.ToList());

                var analito = new OrdenResultadoAnalito
                {
                    idOrdenResultadoAnalisis = item.Key,
                    resultado = serializedObject,
                    idExamenMetodo = 392,
                    observacion = ""
                };
                lista.Add(analito);
            }
            string resultado = null;
            try
            {
                ordenBl.RegistrarResultadosOrdenAnalito(lista, Logueado.idUsuario);
                resultado = "Se ha finalizado correctamente el proceso, por favor verificar el resultado.";
            }
            catch (Exception ex)
            {
                resultado = "Error: " + ex.Message + ". Contactarse con soporte de usuario NetLab v2.0.";
            }

            return resultado;
        }
        public JsonResult ShowHistoriaClinicaPaciente(int? page, string NroDocumento)
        {
            PacienteBl pacienteBL = new PacienteBl();
            List<Paciente> pacienteList = new List<Paciente>();
            pacienteList = pacienteBL.GetDatosPacienteByNroDocumento(NroDocumento,Logueado.idUsuario);
            var pageNumber = page ?? 1;
            const int numRegPorPagima = 10;
            var pageOfPacientes = pacienteList.ToPagedList(pageNumber, numRegPorPagima);
            ViewBag.notFound = "No se encontraron resultados";
            ViewBag.NroDocumento = NroDocumento;
            return Json(pageOfPacientes, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ShowSelectorPlantilla(string idOrden)
        {
            var plantillaList = new List<Plantilla>();
            var establecimiento = Session["establecimientoSeleccionado"] as Establecimiento;
            Session["establecimientoActual"] = establecimiento;
            Session["idOrdenPlantilla"] = idOrden;
            IPlantillaBl plantillaBl = new PlantillaBl();

            if (establecimiento == null)
                return PartialView("_FormAddPlantillaExamen");

            plantillaList = plantillaBl.GetPlantillaByEstablecimiento(establecimiento.IdEstablecimiento).Where(x => x.IdTipo == 5).ToList();

            return PartialView("_FormAddPlantillaExamen", plantillaList);
        }
        public String SelectPlantilla(int idplantilla)
        {
            var establecimiento = Session["establecimientoSeleccionado"] as Establecimiento;
            Session["establecimientoActual"] = establecimiento;
            var idOrden = (string)Session["idOrdenPlantilla"].ToString();

            var ordenBl = new IngresoResultadosBl();
            return ordenBl.AddExamenAnalistaPlantilla(Guid.Parse(idOrden), establecimiento.IdEstablecimiento, Logueado.idUsuario, idplantilla);
        }

        public ActionResult RecepcionMasiva(int TipoProcesoMasivo)
        {
            Session["TipoProcesoMasivo"] = TipoProcesoMasivo;
            ViewBag.UsuarioLogin = Logueado.nombres + ' ' + Logueado.apellidoPaterno + ' ' + Logueado.apellidoMaterno;
            ViewBag.idEstablecimiento = EstablecimientoSeleccionado.IdEstablecimiento.ToString();
            return PartialView("_RecepcionarMuestra");
        }
        public string ProcesoMasivoLabCodigoMuestra(string tipoProcesoMasivo, string codigoMuestra, string UsuarioRom, string metodoKit, string nroSolicitud)
        {
            try
            {
                return new ResultadosBl().ProcesoMasivoLabCodigoMuestra(int.Parse(tipoProcesoMasivo), codigoMuestra, EstablecimientoSeleccionado.IdEstablecimiento, Logueado.idUsuario, UsuarioRom, metodoKit, nroSolicitud);
            }
            catch (Exception ex)
            {
                return EstadoRecepcionMuestra.ERROR + ":" + ex.Message;
            }
        }
        public ActionResult ImprimirRecepcionLab()
        {
            return new ActionAsPdf("_RecepcionarMuestra", new { pdf = true });
        }
        public ActionResult Upload()
        {
            return View("UploadResults");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            DataTable csvTable = new DataTable();
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {

                    if (upload.FileName.EndsWith(".csv"))
                    {
                        Stream stream = upload.InputStream;
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            string[] headers = sr.ReadLine().Split(',');
                            foreach (string titulo in headers)
                            {
                                csvTable.Columns.Add(titulo);
                            }
                            while (!sr.EndOfStream)
                            {
                                string[] rows = sr.ReadLine().Split(',');
                                DataRow dr = csvTable.NewRow();
                                for (int i = 0; i < headers.Length; i++)
                                {
                                    dr[i] = rows[i];
                                }
                                csvTable.Rows.Add(dr);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return View("UploadResults", ProcesaDatosCitometro(convertDataTableString(csvTable)));
        }
        #region CargaMasiva
        public List<CitometriaResultadoMasivoProcesado> ProcesaDatosCitometro(List<CitometriaResultadoMasivo> ExcelValuesJson)
        {
            var idExamenCitometria = "952FAB63-2A4D-4561-AE80-0A9491E8C4C2";
            if (!ExcelValuesJson.Any() || ExcelValuesJson.FirstOrDefault().codigoMuestra == null)
            {
                return null;// PartialView("Blank");
            }

            var ordenBl = new IngresoResultadosBl();
            var ResultadoBl = new ResultadosBl();
            var lista = new List<OrdenResultadoAnalito>();
            var JsonRes = new Model.AnalitoValues();
            var LstJsonRes = new List<Model.AnalitoValues>();
            var ListaReturn = new List<CitometriaResultadoMasivoProcesado>();
            foreach (var item in ExcelValuesJson)
            {
                //armar Lista Json: por fila sale un json 
                var d = ResultadoBl.getOrdenAnalitoResultadobyCodigoExamen(item.codigoMuestra, idExamenCitometria);

                if (d == null)
                {
                    ListaReturn.Add(new CitometriaResultadoMasivoProcesado()
                    {
                        CodigoMuestra = item.codigoMuestra,
                        Observacion = "Muestra no Registrada en el Sistema",
                        Estado = "No Procesado"
                    });
                    continue;
                }
                if (d.estatusE >= 10)
                {
                    ListaReturn.Add(new CitometriaResultadoMasivoProcesado()
                    {
                        CodigoMuestra = item.codigoMuestra,
                        Observacion = "Muestra con Resultados Ingresados y/o Verificados",
                        Estado = "No Procesado"
                    });
                    continue;
                }
                if (d.estatusE == 8 || !String.IsNullOrEmpty(d.Resultado))
                {
                    ListaReturn.Add(new CitometriaResultadoMasivoProcesado()
                    {
                        CodigoMuestra = item.codigoMuestra,
                        Observacion = "Muestra con Resultados Ingresados",
                        Estado = "No Procesado"
                    });
                    continue;
                }

                JsonRes = new Model.AnalitoValues
                {
                    AnalitoId = d.IdAnalito,
                    IdOrdenExamen = d.IdOrdenExamen,
                    IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                    Id = d.IdAnalito.ToString(),
                    Value = "2464",// "2438",
                    Tipo = "combo"
                };
                LstJsonRes.Add(JsonRes);

                //CD4
                JsonRes = new Model.AnalitoValues
                {
                    AnalitoId = d.IdAnalito,
                    IdOrdenExamen = d.IdOrdenExamen,
                    IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                    Id = "1616",//"2439",
                    Value = item.CD4,
                    Tipo = "inputtext"
                };
                LstJsonRes.Add(JsonRes);
                //CD8
                JsonRes = new Model.AnalitoValues
                {
                    AnalitoId = d.IdAnalito,
                    IdOrdenExamen = d.IdOrdenExamen,
                    IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                    Id = "1617",//"2440",
                    Value = item.CD8,
                    Tipo = "inputtext"
                };
                LstJsonRes.Add(JsonRes);
                //CD3
                JsonRes = new Model.AnalitoValues
                {
                    AnalitoId = d.IdAnalito,
                    IdOrdenExamen = d.IdOrdenExamen,
                    IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                    Id = "1618",//"2441",
                    Value = item.CD3,
                    Tipo = "inputtext"
                };
                LstJsonRes.Add(JsonRes);
                //CD4/CD8
                JsonRes = new Model.AnalitoValues
                {
                    AnalitoId = d.IdAnalito,
                    IdOrdenExamen = d.IdOrdenExamen,
                    IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                    Id = "1619",//"2442",
                    Value = item.CD4_CD8,
                    Tipo = "inputtext"
                };
                LstJsonRes.Add(JsonRes);
                //CD4/CD3
                JsonRes = new Model.AnalitoValues
                {
                    AnalitoId = d.IdAnalito,
                    IdOrdenExamen = d.IdOrdenExamen,
                    IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                    Id = "1620",//"2443",
                    Value = item.CD4_CD3,
                    Tipo = "inputtext"
                };
                LstJsonRes.Add(JsonRes);
                //CD8/CD3
                JsonRes = new Model.AnalitoValues
                {
                    AnalitoId = d.IdAnalito,
                    IdOrdenExamen = d.IdOrdenExamen,
                    IdOrdenResultadoAnalito = d.IdOrdenResultadoAnalito,
                    Id = "1621",//"2444",
                    Value = item.CD8_CD3,
                    Tipo = "inputtext"
                };
                LstJsonRes.Add(JsonRes);
                try
                {
                    //var res = new DetalleAnalitoDal().GetAnalitosbyIdAnalitoResultado(LstJsonRes);
                    foreach (var item2 in LstJsonRes.GroupBy(x => x.IdOrdenResultadoAnalito))
                    {
                        string serializedObject = JsonConvert.SerializeObject(item2.ToList());
                        //List<Model.AnalitoValues> deserializedObject = JsonConvert.DeserializeObject<List<Model.AnalitoValues>>(item);

                        var analito = new OrdenResultadoAnalito
                        {
                            idOrdenResultadoAnalisis = item2.Key,
                            resultado = serializedObject,
                            idExamenMetodo = 550,//,549
                            observacion = "",
                            convResultado = new OrdenDal().RetornaResultado(new DetalleAnalitoDal().GetAnalitosbyIdAnalitoResultado(LstJsonRes)),
                            RegistroyFinalizacion = 1
                        };

                        lista.Add(analito);
                        //Registrar y Finalizar resultado
                        ordenBl.RegistrarResultadosOrdenAnalito(lista, Logueado.idUsuario);
                    }
                    LstJsonRes.Clear();
                }
                catch (Exception ex)
                {
                    ListaReturn.Add(new CitometriaResultadoMasivoProcesado()
                    {
                        CodigoMuestra = item.codigoMuestra,
                        Observacion = ex.StackTrace,
                        Estado = "Error No Controlado, por favor comunicarse con Soporte.",

                    });
                    LstJsonRes.Clear();
                }

                ListaReturn.Add(new CitometriaResultadoMasivoProcesado()
                {
                    CodigoMuestra = item.codigoMuestra,
                    Observacion = "Muestra Registrada en el Sistema",
                    Estado = "Procesado",

                });
                LstJsonRes.Clear();
            }
            

            return ListaReturn;
        }
                
        public List<CitometriaResultadoMasivo> convertDataTableString(DataTable dataTable)
        {
            int codUsuario = Logueado.idUsuario;

            string data = string.Empty;
            int rowsCount = dataTable.Rows.Count;
            List<CitometriaResultadoMasivo> ListaCitometria = new List<CitometriaResultadoMasivo>();
            try
            {
                for (int i = 0; i < rowsCount; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    
                    string codMuestra = row.ItemArray[7].ToString();
                    int cont = codMuestra.Length;
                    string cd4 = row.ItemArray[85].ToString();
                    string cd8 = row.ItemArray[83].ToString();
                    string cd3 = row.ItemArray[81].ToString();
                    var fechaAnalisis = devuelveDateTime(row.ItemArray[11].ToString());
                    if (row.ItemArray[17].ToString().Trim() == "OK" || row.ItemArray[17].ToString().Trim() == "Reviewed")
                    {
                        if (codMuestra != "CONTROL")
                        {
                            if (cont >= 10)
                            {
                                if (cd4.ToString() != "NaN" && !string.IsNullOrEmpty(cd4.ToString()))
                                {
                                    if (cd8.ToString() != "NaN" && !string.IsNullOrEmpty(cd8.ToString()))
                                    {
                                        if (cd3.ToString() != "NaN" && !string.IsNullOrEmpty(cd3.ToString()))
                                        {
                                            var sCd4 = decimal.Parse(cd4);//cd4.ToString().Split('.');
                                            var sCd3 = decimal.Parse(cd3);//cd3.ToString().Split('.');
                                            var sCd8 = decimal.Parse(cd8);//cd8.ToString().Split('.');

                                            decimal x = sCd4 / sCd8;
                                            decimal y = sCd4 / sCd3;
                                            decimal z = sCd8 / sCd3;

                                            string Res1 = Math.Round(x, 2).ToString("0.00");
                                            string Res2 = Math.Round(y, 2).ToString("0.00");
                                            string Res3 = Math.Round(z, 2).ToString("0.00");

                                            var DatoCitometria = new CitometriaResultadoMasivo()
                                            {
                                                codigoMuestra = row.ItemArray[7].ToString(),
                                                CD4 = sCd4.ToString("#"),//Entero
                                                CD3 = sCd3.ToString("#"),//Entero
                                                CD8 = sCd8.ToString("#"),//Entero
                                                CD4_CD8 = Res1.Replace(',', '.'),
                                                CD4_CD3 = Res2.Replace(',', '.'),
                                                CD8_CD3 = Res3.Replace(',', '.'),
                                                fechaAnalisis = fechaAnalisis
                                            };

                                            //Crear Lista con Json de resultado
                                            var lMuestra = ListaCitometria.Where(p => p.codigoMuestra == row.ItemArray[7].ToString()).ToList();
                                            if (lMuestra.Count() > 0)
                                            {
                                                if (fechaAnalisis > lMuestra.FirstOrDefault().fechaAnalisis)
                                                {
                                                    ListaCitometria.Remove(lMuestra.FirstOrDefault());
                                                    ListaCitometria.Add(DatoCitometria);
                                                }
                                            }
                                            else
                                            {
                                                ListaCitometria.Add(DatoCitometria);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //log.Error("ERROR : " + ex.Message);
                //Log.Write(brLog.LogLevel.Error, ex.Message, ruta, Page.ToString());
                //lblMensaje.Text = "Error: No se registraron los Resultados, revise el archivo";
            }
            //data = data.substring(0, data.length - 1);
            return ListaCitometria;
        }
        
        #endregion
        private string GetUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }
        private DateTime devuelveDateTime(string sdatetime)
        {
            var val = sdatetime.Remove(0, 1).ToString().Remove(sdatetime.Remove(0, 1).Length - 1, 1).Split('/');
            return DateTime.Parse(val[1] + "/" + val[0] + "/" + val[2]);
        }
        /*
          Descripción: Registra la fecha de siembra del cultivo
          Autor: Marcos Mejia
          Fecha: 24/10/2018
          */
        public int RegistrarFechaSiembra(string idOrdenExamen)
        {
            int valor = 0;
            if (idOrdenExamen != null)
            {
                try
                {
                    var orden = new IngresoResultadosBl();
                    orden.RegistrarFechaSiembra(idOrdenExamen, Logueado.idUsuario);
                    valor = 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return valor;
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
        public ActionResult ExportarExcel()
        {
            List<OrdenIngresarResultadoVd> lista = new List<OrdenIngresarResultadoVd>();
            if (this.Session["ordenes"] != null)
            {
                lista = (List<OrdenIngresarResultadoVd>)this.Session["ordenes"];
            }

            DataTable dtlista = new DataTable("dtlista");
            dtlista = ConvertToDataTable(lista);

            foreach (DataRow row in dtlista.Rows)
            {
                if (Convert.ToInt32(row["estatusE"].ToString()) >= 10)
                {
                   row["status"] = "Resultado Ingresado";
                }
                else if (Convert.ToInt32(row["estatusE"].ToString()) < 10 && row["conformeP"].ToString() == "1")
                {
                    row["status"] = "Pendiente de Resultado";
                }
                else if (Convert.ToInt32(row["statusP"].ToString()) == 5)
                {
                    row["status"] = "Pendiente de Validación";
                }
                else
                {
                    row["status"] = "Pendiente de Recepción";
                }
            }

            dtlista.Columns.Remove("idOrden");
            dtlista.Columns.Remove("idPaciente");
            dtlista.Columns.Remove("idAnimal");
            dtlista.Columns.Remove("idCBS");
            dtlista.Columns.Remove("tipoOrden");
            dtlista.Columns.Remove("IdEstablecimiento");
            dtlista.Columns.Remove("estadoOrden");
            dtlista.Columns.Remove("fechaNacimiento");
            dtlista.Columns.Remove("fechaRegistro");
            dtlista.Columns.Remove("edadPaciente");
            dtlista.Columns.Remove("Edad");
            dtlista.Columns.Remove("nombreProyecto");
            dtlista.Columns.Remove("codigoPaciente");
            dtlista.Columns.Remove("EdadAnios");
            dtlista.Columns.Remove("Genero");
            dtlista.Columns.Remove("nombreGenero");
            dtlista.Columns.Remove("idExamen");
            dtlista.Columns.Remove("nombreEnfermedad");
            dtlista.Columns.Remove("idLaboratorioProcesamiento");
            dtlista.Columns.Remove("codigoMuestra");
            dtlista.Columns.Remove("fechaRecepcion");
            //dtlista.Columns.Remove("fechaColeccion");
            dtlista.Columns.Remove("metodoExamen");
            //dtlista.Columns.Remove("status");
            dtlista.Columns.Remove("muestrasValidar");
            dtlista.Columns.Remove("muestrasPendientesRecepcion");
            dtlista.Columns.Remove("tipoExamen");
            dtlista.Columns.Remove("areas");
            dtlista.Columns.Remove("step");
            dtlista.Columns.Remove("ingresado");
            dtlista.Columns.Remove("validado");
            dtlista.Columns.Remove("statusP");
            dtlista.Columns.Remove("conformeProceso");
            dtlista.Columns.Remove("estatusE");
            dtlista.Columns.Remove("MuestraConforme");
            dtlista.Columns.Remove("flagResultado");
            dtlista.Columns.Remove("conformeP");
            dtlista.Columns.Remove("UbigeoActual");
            dtlista.Columns.Remove("UbigeoReniec");
            dtlista.Columns.Remove("ConFechaSolicitud");
            dtlista.Columns.Remove("ConcodigoOrden");
            dtlista.Columns.Remove("ConnroOficio");
            dtlista.Columns.Remove("ConDepartamentoOrigen");
            dtlista.Columns.Remove("ConProvinciaOrigen");
            dtlista.Columns.Remove("ConDistritoOrigen");
            dtlista.Columns.Remove("ConRedOrigen");
            dtlista.Columns.Remove("ConMicroRedOrigen");
            dtlista.Columns.Remove("ConEstablecimientoSolicitante");
            dtlista.Columns.Remove("ConEstablecimientoLatitud");
            dtlista.Columns.Remove("ConEstablecimientoLongitud");
            dtlista.Columns.Remove("ConDepartamentoDestino");
            dtlista.Columns.Remove("ConProvinciaDestino");
            dtlista.Columns.Remove("ConDistritoDestino");
            dtlista.Columns.Remove("ConDisaDestino");
            dtlista.Columns.Remove("ConRedDestino");
            dtlista.Columns.Remove("ConMicroRedDestino");
            dtlista.Columns.Remove("ConEESS_LAB_Destino");
            dtlista.Columns.Remove("ConDocIdentidad");
            dtlista.Columns.Remove("ConfechaNacimiento");
            dtlista.Columns.Remove("ConnombrePaciente");
            dtlista.Columns.Remove("ConID_Muestra");
            dtlista.Columns.Remove("ConTipoMuestra");
            dtlista.Columns.Remove("ConEnfermedad");
            dtlista.Columns.Remove("ConnombreExamen");
            //dtlista.Columns.Remove("ConComponente");
            dtlista.Columns.Remove("ConnResultado");
            dtlista.Columns.Remove("ConMuestraConforme");
            dtlista.Columns.Remove("criteriosRechazo");
            dtlista.Columns.Remove("ObservacionRechazo");
            //dtlista.Columns.Remove("ConFechaHoraColeccion");
            dtlista.Columns.Remove("ConFechaHoraRecepcionROM");
            dtlista.Columns.Remove("ConFechaHoraRecepcionLAB");
            dtlista.Columns.Remove("ConFechaHoravalidado");
            dtlista.Columns.Remove("ConEstatusResultado");
            dtlista.Columns.Remove("ConEstatusE");
            dtlista.Columns.Remove("ConFechAddExamen");
            dtlista.Columns.Remove("ConMotivo");
            dtlista.Columns.Remove("ConEdad");
            dtlista.Columns.Remove("ConSexo");
            dtlista.Columns.Remove("ConDireccionPaciente");
            dtlista.Columns.Remove("ConColor");
            dtlista.Columns.Remove("error");
            dtlista.Columns.Remove("dias");
            dtlista.Columns.Remove("catalogo");
            dtlista.Columns.Remove("idOrdenExamen");
            dtlista.Columns.Remove("ConDisaOrigen");
            dtlista.Columns.Remove("liberado");
            dtlista.Columns.Remove("ordenInfoListnew");
            dtlista.Columns.Remove("resultadosList");
            dtlista.Columns.Remove("fechaSolicitud");
            dtlista.Columns.Remove("ConFechaHoraColeccion");
            dtlista.Columns.Remove("ConHoraColeccion");
            dtlista.Columns.Remove("ConFechaHoraRecepcionInsROM");
            dtlista.Columns.Remove("ConHoraRecepcionROM");
            dtlista.Columns.Remove("ConHoraRecepcionLAB");
            dtlista.Columns.Remove("ConHoravalidado");
            dtlista.Columns.Remove("fechaRegistroOrden");
            dtlista.Columns.Remove("HoraRegistroOrden");
            dtlista.Columns.Remove("fechaRegistroRecepcionMuestra");
            dtlista.Columns.Remove("HoraRegistroRecepcionMuestra");
            dtlista.Columns.Remove("Telefono");
            dtlista.Columns.Remove("ConInstitucionOrigen");
            dtlista.TableName = "Procesamiento de Resultados";
            dtlista.Columns[0].ColumnName = "Establecimiento Origen";
            dtlista.Columns[1].ColumnName = "Documento Referencia";
            dtlista.Columns[2].ColumnName = "Codigo Orden";
            dtlista.Columns[3].ColumnName = "Nro.Documento";
            dtlista.Columns[4].ColumnName = "Paciente";
            dtlista.Columns[5].ColumnName = "Tipo Documento";
            dtlista.Columns[6].ColumnName = "Examen";
            dtlista.Columns[7].ColumnName = "Fecha Siembra";
            dtlista.Columns[8].ColumnName = "Fecha Obtención de Muestra";
            dtlista.Columns[9].ColumnName = "Estado Resultado";
            dtlista.Columns[10].ColumnName = "Analista";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtlista);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                return new ExcelResult(wb, "Resultados");
            }
        }
        public JsonResult ProcesarResultadosExcelJson2(string ExcelValuesJson)
        {
            var Lstprotocolo = new List<Protocolo>();
            if (!ExcelValuesJson.Any())
            {
                return null;//"Error: Por favor verificar el contenido del excel.";// PartialView("Blank");
            }
            else
            {
                Lstprotocolo = new ResultadosBl().ValidaCodigoLinealProtocolo(ExcelValuesJson);
            }
            return Json(Lstprotocolo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AsignarMetodoProtocolo()
        {
            return PartialView("_FormAsignarMetodoProtocolo");
        }
        public string ProcesarProtocolo(List<Protocolo> lstProtocolo)
        {
            string result = "";
            try
            {
                new ResultadosBl().RegistraCodigoLinealProtocoloMASED(lstProtocolo, Logueado.idUsuario);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public ActionResult VerArchivo(string file)
        {
            string ruta = "";
            if (impersonateValidUser(userName, dominio, password))
            {
                ruta = System.Configuration.ConfigurationManager.AppSettings["rutaFichas"] + file.Trim() + ".pdf";
            }
            //string ruta = @"E:\APP\" + file + ".pdf";
            //string[] files = Directory.GetFiles(@"E:\APP", "*pdf*");
            //var IsExistsQS = (from num in files
            //                  select num).Contains(ruta);
            byte[] fileBytes = System.IO.File.ReadAllBytes(ruta);
            string fileName = file.Trim() + ".pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public bool ValidaFicha(string ficha)
        {
            bool x = false;
            if (impersonateValidUser(userName, dominio, password))
            {
                string ruta = System.Configuration.ConfigurationManager.AppSettings["rutaFichas"] + ficha.Trim() + ".pdf";
                x = System.IO.File.Exists(ruta);
            }
            return x;
        }

        #region Impersonate
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;
        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);
        public bool impersonateValidUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }
        #endregion
    }

}

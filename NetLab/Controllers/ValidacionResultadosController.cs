using BusinessLayer;
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using NetLab.Helpers;
///Agregado Sotero Bustamante 21/10/2017
///Nuevas Funciones para Ingreso de Nuevo Resultado
using Model.ViewData;
using Newtonsoft.Json;
using NetLab.Models;
using Model;
using System.Collections.Generic;
using NetLab.Extensions.Request;
using DataLayer;
using System.ComponentModel;
using System.Data;
using ClosedXML.Excel;
using NetLab.Extensions.ActionResults;
using System.Net.Http;
using Utilitario;
using System.Diagnostics;
using System.Configuration;

namespace NetLab.Controllers
{
    public class ValidacionResultadosController : ParentController
    {
        protected int ItemsPorPag = 20;

        public ActionResult Index()
        {
            var FechaDesde = DateTime.Parse(DateTime.Now.ToDefaultDateFormatString()).AddDays(-1).ToShortDateString().ToString();
            var FechaHasta = DateTime.Parse(DateTime.Now.ToDefaultDateFormatString()).ToShortDateString().ToString();
            return Index(1, "1", FechaDesde, FechaHasta, "", "", "", "0", "10", "","", "");
        }
       
        /// <summary>
        /// Descripción: Controlador para la busqueda de resultados ingresados para la confirmacion del mismo.
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
        /// <param name="tipoOrden"></param>
        /// <param name="estadoResultado"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(int? page, string esFechaRegistro, string fechaDesde, string fechaHasta, string nroDocumento, string nroOficio, string codigoOrden, string tipoOrden, string estadoResultado, string idEnfermedadFiltro, string hdnExamen, string ExamenFiltro)
        {
            Session["validaciones"] = null;
            ViewBag.IsSearch = true;

            var fechaDesdeA = new DateTime();
            var fechaHastaA = new DateTime();
            var fechaRegistro = 0;
            ViewBag.enfermedades = idEnfermedadFiltro;
            ViewBag.examen = ExamenFiltro;
            var fechaDesdeCriteria = fechaDesde ?? DateTime.Now.ToDefaultDateFormatString();
            var fechaHastaCriteria = fechaHasta ?? DateTime.Now.ToDefaultDateFormatString();
            if (!fechaDesdeCriteria.Equals(""))
                fechaDesdeA = DateTime.ParseExact(fechaDesdeCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (!fechaHastaCriteria.Equals(""))
                fechaHastaA = DateTime.ParseExact(fechaHastaCriteria, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var fechaRegistroTmp = esFechaRegistro ?? string.Empty;
            if (!fechaRegistroTmp.Equals(""))
                fechaRegistro = int.Parse(fechaRegistroTmp);
            else if (fechaDesdeCriteria.Equals("") || fechaHastaCriteria.Equals(""))
                fechaRegistro = 0;
            else if (!fechaDesdeCriteria.Equals("") && !fechaHastaCriteria.Equals(""))
            {
                if (fechaDesdeA == fechaHastaA)
                {
                    fechaHastaA.AddDays(1);
                }
            }

            var codigoOrdenF = codigoOrden ?? string.Empty;
            var nroOficioA = nroOficio ?? string.Empty;
            var nroDocumentoA = nroDocumento ?? string.Empty;

            var sEnfermedad = String.IsNullOrEmpty(idEnfermedadFiltro) || idEnfermedadFiltro == "" ? "0" : idEnfermedadFiltro;
            var sExamen = String.IsNullOrEmpty(hdnExamen) || hdnExamen == "" ? Guid.Empty : Guid.Parse(hdnExamen);

            var validaResultBl = new ValidaResultBl();
            var validaciones = validaResultBl.GetValidaciones(Logueado.idUsuario, fechaRegistro, codigoOrdenF,
                fechaDesdeA, fechaHastaA, nroOficioA, nroDocumentoA, EstablecimientoSeleccionado.IdEstablecimiento, int.Parse(estadoResultado), int.Parse(sEnfermedad), sExamen);

            if(validaciones == null) {
                validaciones = new List<ValidaResult>();
            }
            
            ViewBag.TotalRegistros = validaciones == null ? 0 : validaciones.Count;
            int maximaCantidadFilas = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["BandejasMaxRows"]) ? validaciones.Count : int.Parse(ConfigurationManager.AppSettings["BandejasMaxRows"]);

            ViewBag.MostrarMensaje = validaciones.Count > maximaCantidadFilas;

            ViewBag.ExamenFiltro = ExamenFiltro;
            ViewBag.idExamenFiltro = hdnExamen;
            Session["validaciones"] = validaciones;
            return View(validaciones.Take(maximaCantidadFilas).ToList());
        }

        /// <summary>
        /// Descripción: Controlador para identificar si el registro ya tiene resultado verificado o esta listo para la impresion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipoValidacion"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidarResultados(Guid id,string idOrdenExamen, string tipoValidacion)
        {
            ViewBag.IsSearch = true;

            var resultadotBl = new ResultadosBl();
            var idEstablecimiento = EstablecimientoSeleccionado.IdEstablecimiento;

            var orden = resultadotBl.GetOrdenById(id, idOrdenExamen,idEstablecimiento, Logueado.idUsuario);

            orden.IdLaboratorioDestino = idEstablecimiento;

            if (tipoValidacion == "0")//TipoValidacionResultado.PorValidar)
            {
                orden.resultadosList = orden.resultadosList.Where(r => r.conforme == 0).ToList();

                return PartialView("ValidarResultadosPendientes", orden);
            }

            orden.resultadosList = orden.resultadosList.Where(r => r.conforme == 1).ToList();

            return PartialView("ValidarResultados", orden);
        }
        [HttpPost]
        public ActionResult ValidacionResultados(int? page, Guid id)
        {
            return null;

        }
        /// <summary>
        /// Descripción: Controlador para la mostrar el detalle de cada resultado
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarDetalle(string[] idOrdenExamen)
        {
            ViewBag.IsSearch = true;

            var detalleanalitoBl = new DetalleAnalitoBl();
            var detalle = detalleanalitoBl.GetAnalitoByOrdenExamen(idOrdenExamen);

            return PartialView("DetalleAnalito", detalle);
        }
        /// <summary>
        /// Descripción: Controlador para la realizar la validacion del resultado y actualizar la tabla en bd.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <param name="comentario"></param>
        /// <param name="esConforme"></param>
        /// <returns></returns>
        public ActionResult Update(Guid idOrdenExamen, string comentario, int esConforme, string idOrden)
        {
            string corte = string.Empty;
            var resultadoBl = new ResultadosBl();
            resultadoBl.UpdateDatosResultados(idOrdenExamen, comentario, esConforme, Logueado.idUsuario);
            corte = "despues de resultadoBl.UpdateDatosResultados";
            string infoData = string.Format("idOrdenExamen: {0} - comentario: {1} - esConforme: {2} - idOrden: {3} - idUsuario: {4} - ", idOrdenExamen, comentario, esConforme, idOrden, Logueado.idUsuario);
            new bsPage().LogInfoValidacionResultados("ValidacionResultados.Update", "despues de resultadoBl.UpdateDatosResultados", infoData);
            if (esConforme == 1)
            {
                try
                {
                    var datos = resultadoBl.GetDatosCorreo(idOrdenExamen.ToString());
                    corte = "despues de GetDatosCorreo";
                    if (datos.CorreoSolicitante != "")
                    {
                        EnviarCorreo(datos);
                        corte = "despues de enviarCorreo()";
                    }
                    
                    if (datos.Envio == 1 && datos.CelularPaciente != "")
                    {
                        var sendSms = new EnvioCorreo();
                        string message = datos.Resultado;
                        string send = sendSms.NewSendAlertaPaciente(datos.CelularPaciente,message);
                        corte = "despues de SendAlertaPaciente - resultado: " + send;
                    }

                    //Enviar Resultados MINSA EQHALI
                    corte = "antes de ObtenerResultadosCovidPorOrden";
                    //new NetLab.Extensions.Request.SendRquest().EnviarResultados(idOrden.ToString());
                    var data = resultadoBl.ObtenerResultadoCovidPorOrden(Guid.Parse(idOrden));
                    if (!string.IsNullOrWhiteSpace(data.tip_prueba))
                    {
                        corte = "despues de ObtenerResultadoCovidPorOrden";
                        var enviocorrecto = new SendRquest().EnviarResultadosCovid(data);
                        corte = "resultado de envio webservice: " + enviocorrecto;
                        if (!string.IsNullOrWhiteSpace(enviocorrecto))
                        {
                            corte = "antes de InsertarResultadoCovidFallido";
                            resultadoBl.InsertarResultadoCovidFallido(data, Logueado.idUsuario, enviocorrecto);
                            corte = "despues de InsertarResultadoCovidFallido";
                        }
                        else
                        {
                            infoData = string.Format("{0} - EnvioCorrecto a WebService MINSA", infoData);
                            new bsPage().LogInfoValidacionResultados("ValidacionResultados.Update", "envio correcto SendRquest().EnviarResultadosCovid", infoData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new bsPage().LogErrorValidacionResultados(ex, "LogNetLab", "ValidacionResultados.Update", corte);
                }
                //
            }

            return View();
        }
        //public string SendAlertaPaciente(Mensaje m)
        //{
        //        string resul = string.Empty;            
        //        using (var client = new HttpClient())
        //        {
        //            //client.BaseAddress = new Uri("https://netlabv2pru.ins.gob.pe/WSnetlabv2prusms/api/send/result");
        //            //HTTP POST
        //            var postTask = client.PostAsJsonAsync<Mensaje>("https://netlabv2pru.ins.gob.pe/WSnetlabv2prusms/api/send/result", m);
        //            postTask.Wait();

        //            var result = postTask.Result;
        //            if (result.IsSuccessStatusCode)
        //            {
        //                resul="ok";
        //            }
        //        }
        //        //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
        //    return resul;
        //}
        //public static async Task<string> SendSMS(string phone, string msg)
        //{
        //    string user = "INS-SMS";
        //    string pass = "f6k934y9r";

        //    var values = new Dictionary<string, string>
        //        {
        //        { "username", user },
        //        { "password", pass },
        //        { "phone", phone },
        //        { "message", msg },
        //        };

        //    var content = new FormUrlEncodedContent(values);
        //    var client = new HttpClient();

        //    var response = await client.PostAsync("https://loginsmsbulk.com:9912/rest/ws/json-sms", content);

        //    var responseString = await response.Content.ReadAsStringAsync();

        //    return responseString;

        //}

        /// Descripción: Metodo encargado del envío de correo informativo al solicitante de la Orden.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 30/04/2018
        //void EnviarCorreo(Guid idOrdenExamen)
        //{
        //    var resultadoBl = new ResultadosBl();
        //    var mail = resultadoBl.GetDatosCorreo(idOrdenExamen);
        //    if (mail != null)
        //    {
        //        var correo = new EnvioCorreo();
        //        string correoSol = mail.Solicitante.correo;
        //        if (correoSol != "")
        //        {
        //            string asunto = "Resultado informado de Paciente";
        //            string contenido = "Estimado(a) Dr(a): " + mail.Solicitante.Nombres + "\n" + "El resultado del paciente con Código de Orden N° " + mail.Orden.codigoOrden + " ya se encuentra ingresado en el Sistema Netlab v2.0";
        //            correo.EnviarCorreo(correoSol, asunto, contenido);
        //        }
        //    }
        //}
        void EnviarCorreo(EnvioAlerta datos)
        {
            var correo = new EnvioCorreo();
            string asunto = "Resultado informado de Paciente";
            string contenido = "Estimado(a) Dr(a): " + datos.Solicitante + "\n" + "El resultado del paciente con Código de Orden N° " + datos.CodigoOrden + " ya se encuentra ingresado en el Sistema Netlab v2.0";
            correo.EnviarCorreo(datos.CorreoSolicitante, asunto, contenido);
        }

        /// Descripción: Registro log de sms enviados.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 11/06/2018
        public void EnvioSMS(String phone, String message, int tipo)
        {
            var resultadoBL = new ResultadosBl();
            resultadoBL.RegistroEnvioSms(phone, message, tipo);
        }

        /// <summary>
        /// Descripción: Action que identifica una orden lista para el ingreso de resultados.
        /// Author: Sotero Bustamante.
        /// Fecha Creacion: 21/10/2017
        /// Fecha Modificación: 21/10/2017.
        /// Modificación: Se creo funcion para agregar Resultados bandeja de Verificador.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SeleccionarAPVerificador(Guid id)
        {
            IngresoResultadosBl ordenBl = new IngresoResultadosBl();
            OrdenIngresarResultadoVd orden = ordenBl.GetAreaProcesamientoOrdenUsuario(id, Logueado.idUsuario, EstablecimientoSeleccionado.IdEstablecimiento);

            return PartialView("SeleccionarAPVerificador", orden);
        }
        /// <summary>
        /// Descripción: Action que identifica una orden lista para el ingreso de resultados.
        /// Author: Sotero Bustamante.
        /// Fecha Creacion: 21/10/2017
        /// Fecha Modificación: 21/10/2017.
        /// Modificación: Se creo funcion para agregar Resultados bandeja de Verificador.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idArea"></param>
        /// <returns></returns>
        public ActionResult IngresarResultadosVerificador(Guid id, Guid idOrdenExamen, int idArea)
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

            return PartialView("IngresarResultadosVerificador", orden);
        }

        /// <summary>
        /// Descripción: Action que identifica una orden lista para el ingreso de resultados.
        /// Author: Sotero Bustamante.
        /// Fecha Creacion: 21/10/2017
        /// Fecha Modificación: 21/10/2017.
        /// Modificación: Se creo funcion para agregar Resultados bandeja de Verificador.
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
        public ActionResult RegistroResultadosVerificador(Guid idOrden, Guid[] resultados, Guid[] idOrdenResultadoAnalito, Guid[] idAnalito,
            string[] resultado, string[] observacion, int[] metodo, Guid[] ordenExamen, int[] estatusOrdenExamen)
        {
            //List<Models.AnalitoValues> deserializedObject = JsonConvert.DeserializeObject<List<Models.AnalitoValues>>(Request.Form["jsonvalues"]);
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
                        interpretacion = interpretacion
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
                            idSecuencia = 2,
                            convResultado = (item.idAnalitoCabeceraVariable == 1 || item.idAnalitoCabeceraVariable == 2) ? convResultado : item.Glosa
                        };
                        detalle.Add(analitoDetalle);
                    }
                }

                ordenBl.RegistoResultadoAnalitoDetalle(detalle);
            }

            return PartialView("Blank");
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
        /// Descripción: Action para mostrar el detalle de la prueba seleccionada.
        /// Author: Sotero Bustamante.
        /// Fecha Creacion: 21/10/2017
        /// Fecha Modificación: 21/10/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarDetalleVerificador(string[] idOrdenExamen)
        {
            var genero = int.Parse(Session["idGenero"].ToString());

            var detalleanalitoBl = new DetalleAnalitoBl();
            var detalle = detalleanalitoBl.GetAnalitoByOrdenExamenAndGenero(idOrdenExamen, genero);

            return PartialView("DetalleResultadoAnalisis", detalle);
        }

        /// <summary>
        /// Descripción: Action que solicita orden para el ingreso de resultados.
        /// Author: Sotero Bustamante.
        /// Fecha Creacion: 21/10/2017
        /// Fecha Modificación: 21/10/2017.
        /// Modificación: Se creo funcion para agregar Resultados bandeja de Verificador.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SolicitaAUVerificador(Guid id, int estatusSol)
        {            
            try
            {
                IngresoResultadosBl ordenBl = new IngresoResultadosBl();
                ordenBl.SolicitaIngresoNvoResultados(id, Logueado.idUsuario, estatusSol);
                var liberador = ObtenerDatosUsuarioLiberador(id.ToString());
                if (liberador.Celular != "")
                {
                    var sendSms = new EnvioCorreo();
                    string send = sendSms.NewSendAlertaPaciente(liberador.Celular, liberador.mensaje);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Descripción: Recepcion masiva de muestras.
        /// Author: Terceros.
        /// Fecha Creacion: 26/11/2017
        /// Fecha Modificación: 12-08-2019
        /// Modificación: se cambia para que recepcione una entidad tipada - jmuga.
        /// </summary>
        /// <param name="comentarioList"></param>
        /// <returns></returns>
        public string ValidarMuestrasMasivo(List<ValidaResultadoMasivo> comentarioList)
        {
            string rpta = "";
            var ordenMuestraBl = new OrdenMuestraBl();
            int idUsuario = Logueado.idUsuario;
            string corte = string.Empty;
            string infoData = "ValidarMuestrasMasivo - ";
            new bsPage().LogInfoValidacionResultados("ValidacionResultados.ValidarMuestrasMasivo", $" - Masivo - comienzo validacion - {comentarioList.Count} registros", infoData);
            var watch = Stopwatch.StartNew();
            try
            {
                if (comentarioList != null)
                {
                    if (ordenMuestraBl.VerificarMuestrasMasivo(comentarioList, idUsuario))
                    {
                        watch.Stop();
                        new bsPage().LogInfoValidacionResultados("ValidacionResultados.ValidarMuestrasMasivo", $" - Masivo - fin VerificarMuestrasMasivo - {watch.ElapsedMilliseconds} ms", infoData);
                        for (int i = 0; i < comentarioList.Count(); i++)
                        {
                            var watch2 = Stopwatch.StartNew();
                            new bsPage().LogInfoValidacionResultados("ValidacionResultados.ValidarMuestrasMasivo", $" - Masivo - inicio envio alerta - indice registro: {i} ", infoData);
                            var resultadoBl = new ResultadosBl();
                            var datos = resultadoBl.GetDatosCorreo(comentarioList[i].idOrdenExamen);
                            if (datos.CorreoSolicitante != "")
                            {
                                EnviarCorreo(datos);
                            }

                            if (datos.Envio == 1 && datos.CelularPaciente != "")
                            {
                                var sendSms = new EnvioCorreo();
                                string message = datos.Resultado;
                                new bsPage().LogInfoValidacionResultados("ValidacionResultados.ValidarMuestrasMasivo", $" - Masivo - inicio sms - indice registro: {i} ", infoData);
                                string send = sendSms.NewSendAlertaPaciente(datos.CelularPaciente, message);
                                corte = " Envio sms " + send;
                                watch2.Stop();
                                var ms = watch2.ElapsedMilliseconds;
                                watch2.Restart();
                                new bsPage().LogInfoValidacionResultados("ValidacionResultados.ValidarMuestrasMasivo", $" - Masivo - fin sms - indice registro: {i} -tiempo transcurrido: {ms} ms ", infoData);
                            }

                            var data = resultadoBl.ObtenerResultadoCovidPorOrdenExamen(Guid.Parse(comentarioList[i].idOrdenExamen));
                            if (!string.IsNullOrWhiteSpace(data.tip_prueba))
                            {
                                corte = " Masivo - despues de ObtenerResultadoCovidPorOrden";
                                var enviocorrecto = new SendRquest().EnviarResultadosCovid(data);
                                corte = " Masivo - resultado de envio webservice: " + enviocorrecto;
                                if (!string.IsNullOrWhiteSpace(enviocorrecto))
                                {
                                    corte = " Masivo - antes de InsertarResultadoCovidFallido";
                                    resultadoBl.InsertarResultadoCovidFallido(data, Logueado.idUsuario, enviocorrecto);
                                    corte = " Masivo - despues de InsertarResultadoCovidFallido";
                                }
                                else
                                {
                                    infoData = string.Format("{0} -  Masivo - EnvioCorrecto a WebService MINSA", infoData);
                                    new bsPage().LogInfoValidacionResultados("ValidacionResultados.ValidarMuestrasMasivo", " - Masivo - envio correcto SendRquest().EnviarResultadosCovid", infoData);
                                }
                            }
                        }

                        rpta = "ok";
                    }
                }
            }
            catch (Exception ex)
            {
                new bsPage().LogErrorValidacionResultados(ex, "LogNetLab", "ValidacionResultados.ValidarMuestrasMasivo", corte);
            }

            watch.Stop();
            new bsPage().LogInfoValidacionResultados("ValidacionResultados.ValidarMuestrasMasivo", $" - Masivo - fin validacion - {comentarioList.Count} - tiempo transcurrido: {watch.ElapsedMilliseconds} ms ", infoData);
            return rpta;
        }

        /// <summary>
        /// Descripción: Action que actualiza el resultado para verificarlo automáticamente.
        /// Author: Juan Muga.
        /// Fecha Creacion: 06/11/2017
        /// Fecha Modificación:  
        /// Modificación: 
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public int UpdatePruebaRapida(string[] idOrdenExamen)
        {
            var resultadoBl = new ResultadosBl();
            string sIdOrden = string.Empty;
            foreach (var element in idOrdenExamen)
            {
                sIdOrden = resultadoBl.UpdateResultadoPruebaRapida(Guid.Parse(element), "Registro prueba rápida.", 1, Logueado.idUsuario);
            }
            var datos = resultadoBl.GetDatosCorreo(idOrdenExamen[0].ToString());
            if (datos.CorreoSolicitante != "")
            {
                EnviarCorreo(datos);
            }

            if (datos.Envio == 1 && datos.CelularPaciente != "")
            {
                var sendSms = new EnvioCorreo();
                string message = datos.Resultado;
                string send = sendSms.NewSendAlertaPaciente(datos.CelularPaciente, message);
            }

            int idLaboratorio = Convert.ToInt32(sIdOrden);
            return idLaboratorio;
            /////////
        }

        public UsuarioEnvioSms ObtenerDatosUsuarioLiberador(string id)
        {
            var liberadorDal = new OrdenMuestraDal();
            UsuarioEnvioSms liberador = new UsuarioEnvioSms();
            liberador = liberadorDal.ObtenerDatosUsuarioLiberador(id);
            return liberador;
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
            List<ValidaResult> lista = new List<ValidaResult>();
            if (this.Session["validaciones"] != null)
            {
                lista = (List<ValidaResult>)this.Session["validaciones"];
            }

            DataTable dtlista = new DataTable("dtlista");
            dtlista = ConvertToDataTable(lista);
            dtlista.Columns.Remove("idOrden");
            dtlista.Columns.Remove("idUsuarioIngreso");
            dtlista.Columns.Remove("idLaboratorio");
            dtlista.Columns.Remove("NomLab");
            dtlista.Columns.Remove("fechaSolicitud");
            dtlista.Columns.Remove("genero");
            dtlista.Columns.Remove("validado");
            dtlista.Columns.Remove("EXISTE_PENDIENTE");
            dtlista.Columns.Remove("EXISTE_VALIDADO");
            dtlista.Columns.Remove("SOLICITA_INGRESO");
            dtlista.Columns.Remove("SOLICITA_OK");
            dtlista.Columns.Remove("SOLICITA_NO");
            dtlista.Columns.Remove("idProyecto");
            dtlista.Columns.Remove("idExamen");
            dtlista.Columns.Remove("idOrdenExamen");
            dtlista.Columns.Remove("celular");
            dtlista.Columns.Remove("ExamenComun");
            dtlista.TableName = "Verificacion de Resultados";
            dtlista.Columns[0].ColumnName = "Establecimiento Origen";
            dtlista.Columns[1].ColumnName = "Codigo Orden";
            dtlista.Columns[2].ColumnName = "Numero Documento";
            dtlista.Columns[3].ColumnName = "Fecha Registro";
            dtlista.Columns[4].ColumnName = "Oficio";
            dtlista.Columns[5].ColumnName = "Paciente";
            dtlista.Columns[6].ColumnName = "Fecha Nacimiento";
            dtlista.Columns[7].ColumnName = "Tipo Documento";
            dtlista.Columns[8].ColumnName = "Examen";
            dtlista.Columns[9].ColumnName = "Resultado";
            dtlista.Columns[10].ColumnName = "Estado";
            dtlista.Columns[11].ColumnName = "Comentario";
            dtlista.Columns[12].ColumnName = "Codigo Muestra";
            dtlista.Columns[13].ColumnName = "Fecha Obtención de Muestra";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtlista);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                return new ExcelResult(wb, "Resultados Verificados");
            }
        }


        //public string ValidarMuestrasMasivo(List<ValidaResultadoMasivo> comentarioList)
        //{
        //    string rpta = "";
        //    int idUsuario = Logueado.idUsuario;
        //    string corte = string.Empty;
        //    string infoData = "ValidarMuestrasMasivo - ";
        //    try
        //    {
        //        if (comentarioList != null)
        //        {
        //            var bl = new OrdenMuestraBl();
        //            rpta = bl.VerificarMuestrasMasivo(comentarioList, idUsuario);
        //            if (rpta.Length > 0)
        //            {
        //                var sendSms = new EnvioCorreo();
        //                var filas = rpta.Split('$');
        //                for (int i = 0; i < filas.Length; i++)
        //                {
        //                    var col = filas[i].Split('|');
        //                    var x = new UsuarioEnvioSms();
        //                    x.Celular = col[0].ToString();
        //                    x.mensaje = col[1].ToString();
        //                    string send = sendSms.NewSendAlertaPaciente(x.Celular, x.mensaje);
        //                }
        //            }

        //            for (int i = 0; i < comentarioList.Count(); i++)
        //            {
        //                var resultadoBl = new ResultadosBl();
        //                var data = resultadoBl.ObtenerResultadoCovidPorOrdenExamen(Guid.Parse(comentarioList[i].idOrdenExamen));
        //                if (!string.IsNullOrWhiteSpace(data.tip_prueba))
        //                {
        //                    corte = " Masivo - despues de ObtenerResultadoCovidPorOrden";
        //                    var enviocorrecto = new SendRquest().EnviarResultadosCovid(data);
        //                    corte = " Masivo - resultado de envio webservice: " + enviocorrecto;
        //                    if (!string.IsNullOrWhiteSpace(enviocorrecto))
        //                    {
        //                        corte = " Masivo - antes de InsertarResultadoCovidFallido";
        //                        resultadoBl.InsertarResultadoCovidFallido(data, Logueado.idUsuario, enviocorrecto);
        //                        corte = " Masivo - despues de InsertarResultadoCovidFallido";
        //                    }
        //                    else
        //                    {
        //                        infoData = string.Format("{0} -  Masivo - EnvioCorrecto a WebService MINSA", infoData);
        //                        new bsPage().LogInfoValidacionResultados("ValidacionResultados.ValidarMuestrasMasivo", " - Masivo - envio correcto SendRquest().EnviarResultadosCovid", infoData);
        //                    }
        //                }    
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new bsPage().LogErrorValidacionResultados(ex, "LogNetLab", "ValidacionResultados.ValidarMuestrasMasivo", corte);
        //    }

        //    return rpta;
        //}
    }

}
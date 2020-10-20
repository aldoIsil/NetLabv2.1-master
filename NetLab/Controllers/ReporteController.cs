using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using Model.ReportesDTO;
using Newtonsoft.Json;
using System.Text;
using PagedList;
using ClosedXML.Excel;
using Model;
using Rotativa.Options;
using System.Data;
using NetLab.Extensions.ActionResults;
using Utilitario;

namespace NetLab.Controllers
{
    public class ReporteController : ParentController
    {
        public ActionResult Index()
        {
            return View();
        }

        [Route("/Reporte/GetMuestras/{tipoReporte}")]
        public ActionResult GetMuestras(string tipoReporte)
        {
            switch ((Enums.TipoReporte)int.Parse(tipoReporte))
            {
                case Enums.TipoReporte.OportunidadObtencionMuestras:
                    ViewBag.TipoReporte = Enums.TipoReporte.OportunidadObtencionMuestras;
                    ViewBag.PageTitle = "Oportunidad de Obtención de Muestras";
                    break;
                case Enums.TipoReporte.OportunidadEnvioMuestras:
                    ViewBag.TipoReporte = Enums.TipoReporte.OportunidadEnvioMuestras;
                    ViewBag.PageTitle = "Oportunidad de Envío de Muestras";
                    break;
                case Enums.TipoReporte.OportunidadAnalisisMuestras:
                    ViewBag.TipoReporte = Enums.TipoReporte.OportunidadAnalisisMuestras;
                    ViewBag.PageTitle = "Oportunidad de Análisis de Muestras";
                    break;
                case Enums.TipoReporte.PorcentajeResultadosEmitidos:
                    ViewBag.TipoReporte = Enums.TipoReporte.PorcentajeResultadosEmitidos;
                    ViewBag.PageTitle = "Porcentaje de Resultados Emitidos";
                    break;
                case Enums.TipoReporte.MotivoRechazoROM:
                    ViewBag.TipoReporte = Enums.TipoReporte.MotivoRechazoROM;
                    ViewBag.PageTitle = "Motivos de Rechazo de la Muestra en ROM";
                    break;
                case Enums.TipoReporte.MotivoRechazoLaboratorio:
                    ViewBag.TipoReporte = Enums.TipoReporte.MotivoRechazoLaboratorio;
                    ViewBag.PageTitle = "Motivos de Rechazo de la Muestra en Laboratorio";
                    break;
                case Enums.TipoReporte.NroPorcentajeMuestrasRechazadasEnROM:
                    ViewBag.TipoReporte = Enums.TipoReporte.NroPorcentajeMuestrasRechazadasEnROM;
                    ViewBag.PageTitle = "Número y Porcentaje de Muestras Rechazadas";
                    break;
                case Enums.TipoReporte.NroPorcentajeResultadosRechazadosPorVerificador:
                    ViewBag.TipoReporte = Enums.TipoReporte.NroPorcentajeResultadosRechazadosPorVerificador;
                    ViewBag.PageTitle = "Número y Porcentaje de Resultados Rechazados";
                    break;
                case Enums.TipoReporte.NroResultadosCorregidosDespuesPublicacion:
                    ViewBag.TipoReporte = Enums.TipoReporte.NroResultadosCorregidosDespuesPublicacion;
                    ViewBag.PageTitle = "Número de Resultados Corregidos Después de su Publicación";
                    break;
                //case Enums.TipoReporte.NroMuestrasProcesadasBaciloscopia:
                //    ViewBag.TipoReporte = Enums.TipoReporte.NroMuestrasProcesadasBaciloscopia;
                //    ViewBag.PageTitle = "Número de Muestras Procesadas de Baciloscopia y Cultivos";
                //    //return View("ReporteCantidadBaciloscopia");
                //    break;
                case Enums.TipoReporte.CantidadReporteResultadoConsultado:
                    ViewBag.TipoReporte = Enums.TipoReporte.CantidadReporteResultadoConsultado;
                    ViewBag.PageTitle = "Cantidad de Reporte de Resultados consultados";
                    break;
                case Enums.TipoReporte.CantidadReporteResultadoConsultadoPorSolicitante:
                    ViewBag.TipoReporte = Enums.TipoReporte.CantidadReporteResultadoConsultadoPorSolicitante;
                    ViewBag.PageTitle = "Cantidad de Reporte de Resultados consultados por el Solicitante";
                    break;
                case Enums.TipoReporte.CantidadReporteResultadoInformados:
                    ViewBag.TipoReporte = Enums.TipoReporte.CantidadReporteResultadoInformados;
                    ViewBag.PageTitle = "Cantidad de Reporte de Resultados Informados";
                    break;
            }

            LocalBl bl = new LocalBl();
            EnfermedadBl enfermedadBL = new EnfermedadBl();
            if (Session["reporteInstituciones"] == null || !((List<Model.Institucion>)Session["reporteInstituciones"]).Any())
            {
                Session["reporteInstituciones"] = bl.GetInstituciones();
            }

            if (Session["reporteDISAs"] == null || !((List<Model.DISA>)Session["reporteDISAs"]).Any())
            {
                Session["reporteDISAs"] = bl.GetDisas();
            }

            if (Session["reporteRedes"] == null || !((List<Model.Red>)Session["reporteRedes"]).Any())
            {
                Session["reporteRedes"] = bl.GetRedes();
            }

            if (Session["reporteMicroRedes"] == null || !((List<Model.MicroRed>)Session["reporteMicroRedes"]).Any())
            {
                Session["reporteMicroRedes"] = bl.GetMicroRedes();
            }

            if (Session["reporteEstablecimientos"] == null || !((List<Model.Establecimiento>)Session["reporteEstablecimientos"]).Any())
            {
                Session["reporteEstablecimientos"] = bl.GetEstablecimientos();
            }

            ViewBag.Instituciones = Session["reporteInstituciones"];
            //ViewBag.DISAs = bl.GetDisas();
            //ViewBag.GetRedes = bl.GetRedes();
            //ViewBag.Redes = bl.GetMicroRedes();
            ViewBag.Establecimientos = bl.GetEstablecimientos();
            ViewBag.Enfermedades = enfermedadBL.GetEnfermedades();

            return View("GetReporte");
        }

        public ActionResult GetIndicadorBCS(string TipoReporte)
        {
            ViewBag.TipoReporte = Enums.TipoReporte.NroMuestrasProcesadasBaciloscopia;
            ViewBag.PageTitle = "Número de Muestras Procesadas de Baciloscopia y Cultivos";
            LocalBl bl = new LocalBl();
            EnfermedadBl enfermedadBL = new EnfermedadBl();
            if (Session["reporteInstituciones"] == null || !((List<Model.Institucion>)Session["reporteInstituciones"]).Any())
            {
                Session["reporteInstituciones"] = bl.GetInstituciones();
            }

            if (Session["reporteDISAs"] == null || !((List<Model.DISA>)Session["reporteDISAs"]).Any())
            {
                Session["reporteDISAs"] = bl.GetDisas();
            }

            if (Session["reporteRedes"] == null || !((List<Model.Red>)Session["reporteRedes"]).Any())
            {
                Session["reporteRedes"] = bl.GetRedes();
            }

            if (Session["reporteMicroRedes"] == null || !((List<Model.MicroRed>)Session["reporteMicroRedes"]).Any())
            {
                Session["reporteMicroRedes"] = bl.GetMicroRedes();
            }

            if (Session["reporteEstablecimientos"] == null || !((List<Model.Establecimiento>)Session["reporteEstablecimientos"]).Any())
            {
                Session["reporteEstablecimientos"] = bl.GetEstablecimientos();
            }

            ViewBag.Instituciones = Session["reporteInstituciones"];
            //ViewBag.DISAs = bl.GetDisas();
            //ViewBag.GetRedes = bl.GetRedes();
            //ViewBag.Redes = bl.GetMicroRedes();
            ViewBag.Establecimientos = bl.GetEstablecimientos();
            //ViewBag.Enfermedades = enfermedadBL.GetEnfermedades();
            return View("ReporteCantidadBaciloscopia");
        }

        public ActionResult EnvioMuestras()
        {
            LocalBl bl = new LocalBl();
            EnfermedadBl enfermedadBL = new EnfermedadBl();
            if (Session["reporteInstituciones"] == null || !((List<Model.Institucion>)Session["reporteInstituciones"]).Any())
            {
                Session["reporteInstituciones"] = bl.GetInstituciones();
            }

            if (Session["reporteDISAs"] == null || !((List<Model.DISA>)Session["reporteDISAs"]).Any())
            {
                Session["reporteDISAs"] = bl.GetDisas();
            }

            if (Session["reporteRedes"] == null || !((List<Model.Red>)Session["reporteRedes"]).Any())
            {
                Session["reporteRedes"] = bl.GetRedes();
            }

            if (Session["reporteMicroRedes"] == null || !((List<Model.MicroRed>)Session["reporteMicroRedes"]).Any())
            {
                Session["reporteMicroRedes"] = bl.GetMicroRedes();
            }

            if (Session["reporteEstablecimientos"] == null || !((List<Model.Establecimiento>)Session["reporteEstablecimientos"]).Any())
            {
                Session["reporteEstablecimientos"] = bl.GetEstablecimientos();
            }

            ViewBag.Instituciones = Session["reporteInstituciones"];
            //ViewBag.DISAs = bl.GetDisas();
            //ViewBag.GetRedes = bl.GetRedes();
            //ViewBag.Redes = bl.GetMicroRedes();
            ViewBag.Establecimientos = bl.GetEstablecimientos();
            ViewBag.Enfermedades = enfermedadBL.GetEnfermedades();
            return View();
        }

        public ActionResult AnalisisMuestras()
        {
            LocalBl bl = new LocalBl();
            EnfermedadBl enfermedadBL = new EnfermedadBl();
            if (Session["reporteInstituciones"] == null || !((List<Model.Institucion>)Session["reporteInstituciones"]).Any())
            {
                Session["reporteInstituciones"] = bl.GetInstituciones();
            }

            if (Session["reporteDISAs"] == null || !((List<Model.DISA>)Session["reporteDISAs"]).Any())
            {
                Session["reporteDISAs"] = bl.GetDisas();
            }

            if (Session["reporteRedes"] == null || !((List<Model.Red>)Session["reporteRedes"]).Any())
            {
                Session["reporteRedes"] = bl.GetRedes();
            }

            if (Session["reporteMicroRedes"] == null || !((List<Model.MicroRed>)Session["reporteMicroRedes"]).Any())
            {
                Session["reporteMicroRedes"] = bl.GetMicroRedes();
            }

            if (Session["reporteEstablecimientos"] == null || !((List<Model.Establecimiento>)Session["reporteEstablecimientos"]).Any())
            {
                Session["reporteEstablecimientos"] = bl.GetEstablecimientos();
            }

            ViewBag.Instituciones = Session["reporteInstituciones"];
            //ViewBag.DISAs = bl.GetDisas();
            //ViewBag.GetRedes = bl.GetRedes();
            //ViewBag.Redes = bl.GetMicroRedes();
            ViewBag.Establecimientos = bl.GetEstablecimientos();
            ViewBag.Enfermedades = enfermedadBL.GetEnfermedades();
            return View();
        }

        [HttpPost]
        public ActionResult GetReporteData()
        {
            Enums.TipoReporte tipoReporte = (Enums.TipoReporte)Enum.Parse(typeof(Enums.TipoReporte), Request.Form["tipoReporte"]);
            //OBTENER LISTA DE REDES Y ESTABLECIMIENTOS, GUARDA
            //JRCR - 2doProducto
            ReporteRequest request = new ReporteRequest();
            DateTime fechafinaux = Request.Form["fechaHasta"] == null ? DateTime.Today : Convert.ToDateTime(Request.Form["fechaHasta"]);
            DateTime fechainicioaux = Request.Form["fechaDesde"] == null ? fechafinaux.AddMonths(-1) : Convert.ToDateTime(Request.Form["fechaDesde"]);

            //request.TipoEnfermedad = 1;//tbc
            request.IdEnfermedad = Convert.ToInt32(Request.Form["selEnfermedad"]);
            request.FechaFin = fechafinaux;
            request.FechaInicio = fechainicioaux;
            string filtro = Request.Form["nombrefiltro"];
            request.NombreFiltro = filtro;
            switch (filtro)
            {
                case "institucion":
                    request.IdJurisdiccion = Request.Form["hdnInstitucion"];
                    break;
                case "disa":
                    request.IdJurisdiccion = Request.Form["hdnDisa"];
                    break;
                case "red":
                    request.IdJurisdiccion = Request.Form["hdnRed"];
                    break;
                case "microred":
                    request.IdJurisdiccion = Request.Form["hdnMicroRed"];
                    break;
                case "establecimiento":
                    request.IdJurisdiccion = Request.Form["hdnEstablecimiento"];
                    break;
            }

            ReporteBl bl = new ReporteBl();
            List<ReporteResult> result = bl.GenerarReporte(request, tipoReporte);
            ViewBag.ReporteResult = result;
            List<ReportResponse> objResult = result.GroupBy(x => x.EstablecimientoCodigoUnico)
                .Select(b => new ReportResponse
                {
                    EstablecimientoCodigoUnico = b.Key,
                    //EESSDias = b.Select(s => s.NumeroDias.ToString()).ToList()
                    EESSDias = b.Select(s => s.NumeroDias).ToList()
                }).ToList();



            ViewBag.Enfermedad = request.IdEnfermedad;

            foreach (var item in objResult)
            {
                item.EESSDias.Insert(0, item.EstablecimientoCodigoUnico);
            }

            ViewBag.DatosTabla = GetPlotValues(objResult);
            //retornar objResult a un partialview para armar la grafica
            return PartialView("_ChartResult", objResult);
        }

        [HttpPost]
        public ActionResult GetReportePorcentajeData()
        {
            Enums.TipoReporte tipoReporte = (Enums.TipoReporte)Enum.Parse(typeof(Enums.TipoReporte), Request.Form["tipoReporte"]);
            //OBTENER LISTA DE REDES Y ESTABLECIMIENTOS, GUARDA
            //JRCR - 2doProducto
            ReporteRequest request = new ReporteRequest();
            DateTime fechafinaux = Request.Form["fechaHasta"] == null ? DateTime.Today : Convert.ToDateTime(Request.Form["fechaHasta"]);
            DateTime fechainicioaux = Request.Form["fechaDesde"] == null ? fechafinaux.AddMonths(-1) : Convert.ToDateTime(Request.Form["fechaDesde"]);

            //request.TipoEnfermedad = 1;//tbc
            request.IdEnfermedad = Convert.ToInt32(Request.Form["selEnfermedad"]);
            request.FechaFin = fechafinaux;
            request.FechaInicio = fechainicioaux;
            string filtro = Request.Form["nombrefiltro"];
            request.NombreFiltro = filtro;
            switch (filtro)
            {
                case "institucion":
                    request.IdJurisdiccion = Request.Form["hdnInstitucion"];
                    break;
                case "disa":
                    request.IdJurisdiccion = Request.Form["hdnDisa"];
                    break;
                case "red":
                    request.IdJurisdiccion = Request.Form["hdnRed"];
                    break;
                case "microred":
                    request.IdJurisdiccion = Request.Form["hdnMicroRed"];
                    break;
                case "establecimiento":
                    request.IdJurisdiccion = Request.Form["hdnEstablecimiento"];
                    break;
            }

            ReporteBl bl = new ReporteBl();
            List<ReportePorcentajeResponse> result = bl.GenerarReportePorcentaje(request, tipoReporte);

            if (!result.Any(a => !string.IsNullOrEmpty(a.Establecimiento)))
            {
                ViewBag.NoDataMessage = "No hay data disponible";
            }

            ViewBag.ValuesX = JsonConvert.SerializeObject(result.Select(s => s.Establecimiento));
            ViewBag.ValuesY = JsonConvert.SerializeObject(result.Select(s => s.Porcentaje));
            ViewBag.ReporteResult = result;

            ViewBag.Enfermedad = request.IdEnfermedad;
            return PartialView("_BarChartResult", result);
        }

        [HttpPost]
        public ActionResult GetDataEnvioMuestras()
        {
            //OBTENER LISTA DE REDES Y ESTABLECIMIENTOS, GUARDA
            //JRCR - 2doProducto
            ReporteRequest request = new ReporteRequest();
            DateTime fechafinaux = Request.Form["fechaHasta"] == null ? DateTime.Today : Convert.ToDateTime(Request.Form["fechaHasta"]);
            DateTime fechainicioaux = Request.Form["fechaDesde"] == null ? fechafinaux.AddMonths(-1) : Convert.ToDateTime(Request.Form["fechaDesde"]);

            //request.TipoEnfermedad = 1;//tbc
            request.IdEnfermedad = Convert.ToInt32(Request.Form["selEnfermedad"]);
            request.FechaFin = fechafinaux;
            request.FechaInicio = fechainicioaux;
            string filtro = Request.Form["nombrefiltro"];
            request.NombreFiltro = filtro;
            switch (filtro)
            {
                case "institucion":
                    request.IdJurisdiccion = Request.Form["hdnInstitucion"];
                    break;
                case "disa":
                    request.IdJurisdiccion = Request.Form["hdnDisa"];
                    break;
                case "red":
                    request.IdJurisdiccion = Request.Form["hdnRed"];
                    break;
                case "microred":
                    request.IdJurisdiccion = Request.Form["hdnMicroRed"];
                    break;
                case "establecimiento":
                    request.IdJurisdiccion = Request.Form["hdnEstablecimiento"];
                    break;
            }

            ReporteBl bl = new ReporteBl();
            List<ReporteResult> result = bl.GenerarReporte(request, Enums.TipoReporte.OportunidadEnvioMuestras);
            ViewBag.ReporteResult = result;

            List<ReportResponse> objResult = result.GroupBy(x => x.EstablecimientoCodigoUnico)
                .Select(b => new ReportResponse
                {
                    EstablecimientoCodigoUnico = b.Key,
                    EESSDias = b.Select(s => s.NumeroDias).ToList()
                }).ToList();

            foreach (var item in objResult)
            {
                item.EESSDias.Insert(0, item.EstablecimientoCodigoUnico);
            }

            ViewBag.Enfermedad = request.IdEnfermedad;
            //retornar objResult a un partialview para armar la grafica
            return PartialView("_ChartResult", objResult);
        }

        public List<ReporteTableValues> GetPlotValues(List<ReportResponse> obj)
        {
            List<ReporteTableValues> result = new List<ReporteTableValues>();
            var muestras = obj.Select(s => s.EESSDias);
            var array = muestras.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                array[i].RemoveAt(0);
                var arr = array[i].OrderBy(o => o).ToArray();
                int max = arr[arr.Length - 1];
                int min = arr[0];
                decimal median = GetMedian(arr);

                decimal firstQuartile = GetMedian(arr.Take(4).ToArray());
                decimal thirdQuartile = GetMedian(arr.Skip(3).ToArray());

                result.Add(new ReporteTableValues
                {
                    Min = min,
                    Max = max,
                    Median = median,
                    //Total = array.Length
                    FirstQuartile = firstQuartile,
                    ThirdQuartile = thirdQuartile
                });
            }

            return result;
        }

        public decimal GetMedian(int[] array)
        {
            if (array.Length == 0) return 0;

            int length = array.Length;
            if (length % 2 == 0)
            {
                var midUpper = length / 2;
                var midLower = midUpper - 1;

                //return (decimal)((array[midUpper] + array[midLower]) / 2);
                return Decimal.Divide(array[midUpper] + array[midLower], 2);
            }
            else
            {
                return array[length / 2];
            }
        }

        //JRCR - 2doProducto
        [HttpPost]
        public ActionResult GetReportesRadar()
        {
            Enums.TipoReporte tipoReporte = (Enums.TipoReporte)Enum.Parse(typeof(Enums.TipoReporte), Request.Form["tipoReporte"]);
            //JRCR - 2doProducto
            ReporteRequest request = new ReporteRequest();
            DateTime fechafinaux = Request.Form["fechaHasta"] == null ? DateTime.Today : Convert.ToDateTime(Request.Form["fechaHasta"]);
            DateTime fechainicioaux = Request.Form["fechaDesde"] == null ? fechafinaux.AddMonths(-1) : Convert.ToDateTime(Request.Form["fechaDesde"]);

            //request.TipoEnfermedad = 1;//tbc
            request.IdEnfermedad = Convert.ToInt32(Request.Form["selEnfermedad"]);
            request.FechaFin = fechafinaux;
            request.FechaInicio = fechainicioaux;
            string filtro = Request.Form["nombrefiltro"];
            request.NombreFiltro = filtro;
            switch (filtro)
            {
                case "institucion":
                    request.IdJurisdiccion = Request.Form["hdnInstitucion"];
                    break;
                case "disa":
                    request.IdJurisdiccion = Request.Form["hdnDisa"];
                    break;
                case "red":
                    request.IdJurisdiccion = Request.Form["hdnRed"];
                    break;
                case "microred":
                    request.IdJurisdiccion = Request.Form["hdnMicroRed"];
                    break;
                case "establecimiento":
                    request.IdJurisdiccion = Request.Form["hdnEstablecimiento"];
                    break;
            }

            ReporteBl bl = new ReporteBl();
            //List<ReporteRadarDatos> datosReporte = bl.GenerarReporteRadar(request, tipoReporte);
            List<MotivoRechazoDatos> datosReporte = bl.GenerarReporteRadar(request, tipoReporte);

            List<string> motivosRechazoList = datosReporte.Select(x => x.CriterioRechazo).Distinct().ToList();
            List<string> establecimeintoList = datosReporte.Select(x => x.Establecimiento).Distinct().ToList();
            //List<List<int>> valoresDataSet = new List<List<int>>();
            //Dictionary<string, string> valoresDataSet = new Dictionary<string, string>();
            List<RadarReporteObject> objList = new List<RadarReporteObject>();
            int index = 0;
            foreach (var eess in establecimeintoList)
            {
                index++;
                RadarReporteObject newObj = new RadarReporteObject();
                newObj.label = eess;
                List<int> intValues = new List<int>();
                foreach (var motivoRechazo in motivosRechazoList)
                {
                    //intValues.Add(datosReporte.FirstOrDefault(x => x.Establecimiento == eess && x.CriterioRechazo == motivoRechazo).CantidadMuestras);
                    if (datosReporte.Any(x => x.Establecimiento == eess && x.CriterioRechazo == motivoRechazo))
                    {
                        intValues.Add(datosReporte.First(x => x.Establecimiento == eess && x.CriterioRechazo == motivoRechazo).CantidadMuestras);
                    }
                    else
                    {
                        intValues.Add(0);
                    }
                }

                //valoresDataSet.Add(eess, JsonConvert.SerializeObject(intValues));
                if (index > Enum.GetNames(typeof(Enums.Colores)).Length)
                {
                    index = 1;
                }

                string colorname = ((Enums.Colores)index).ToString().ToLower();
                //System.Drawing.Color color = System.Drawing.Color.FromName(colorname);
                //string datasetColor = color.IsEmpty ? "lightblue" : color.Name;
                newObj.backgroundColor = string.Format("color({0}).alpha(0.2).rgbString()", colorname);
                newObj.pointBackgroundColor = colorname;
                newObj.borderColor = colorname;
                newObj.data = JsonConvert.SerializeObject(intValues);
                objList.Add(newObj);
            }

            ViewBag.Labels = JsonConvert.SerializeObject(motivosRechazoList);
            ViewBag.DataSet = JsonConvert.SerializeObject(objList);

            ViewBag.Enfermedad = request.IdEnfermedad;
            return PartialView("_RadarChartResult", datosReporte);
        }

        [HttpPost]
        public ActionResult GetReportePorcentajeMuestrasRechazadas()//Pie chart
        {
            Enums.TipoReporte tipoReporte = (Enums.TipoReporte)Enum.Parse(typeof(Enums.TipoReporte), Request.Form["tipoReporte"]);
            ReporteRequest request = new ReporteRequest();
            DateTime fechafinaux = Request.Form["fechaHasta"] == null ? DateTime.Today : Convert.ToDateTime(Request.Form["fechaHasta"]);
            DateTime fechainicioaux = Request.Form["fechaDesde"] == null ? fechafinaux.AddMonths(-1) : Convert.ToDateTime(Request.Form["fechaDesde"]);

            //request.TipoEnfermedad = 1;//tbc
            request.IdEnfermedad = Convert.ToInt32(Request.Form["selEnfermedad"]);
            request.FechaFin = fechafinaux;
            request.FechaInicio = fechainicioaux;
            string filtro = Request.Form["nombrefiltro"];
            request.NombreFiltro = filtro;
            switch (filtro)
            {
                case "institucion":
                    request.IdJurisdiccion = Request.Form["hdnInstitucion"];
                    break;
                case "disa":
                    request.IdJurisdiccion = Request.Form["hdnDisa"];
                    break;
                case "red":
                    request.IdJurisdiccion = Request.Form["hdnRed"];
                    break;
                case "microred":
                    request.IdJurisdiccion = Request.Form["hdnMicroRed"];
                    break;
                case "establecimiento":
                    request.IdJurisdiccion = Request.Form["hdnEstablecimiento"];
                    break;
            }

            ReporteBl bl = new ReporteBl();
            List<ReportePieResponse> result = bl.GenerarReportePorcentajeMuestrasRechazadas(request, tipoReporte);

            if (!result.Any())
            {
                ViewBag.NoDataMessage = "No hay data disponible";
            }

            ViewBag.Labels = JsonConvert.SerializeObject(result.Select(s => s.Indicador));
            ViewBag.DataNumero = JsonConvert.SerializeObject(result.Select(s => s.ValorIndicadorNumero));
            ViewBag.DataPorcentajes = JsonConvert.SerializeObject(result.Select(s => s.ValorIndicadorPorcentaje));
            ViewBag.ReporteResult = result;

            ViewBag.Enfermedad = request.IdEnfermedad;
            return PartialView("_PieChartResult", result);
        }

        [HttpPost]
        public ActionResult GetReporteMuestrasCorregidas()//Bar chart
        {
            Enums.TipoReporte tipoReporte = (Enums.TipoReporte)Enum.Parse(typeof(Enums.TipoReporte), Request.Form["tipoReporte"]);
            ReporteRequest request = new ReporteRequest();
            DateTime fechafinaux = Request.Form["fechaHasta"] == null ? DateTime.Today : Convert.ToDateTime(Request.Form["fechaHasta"]);
            DateTime fechainicioaux = Request.Form["fechaDesde"] == null ? fechafinaux.AddMonths(-1) : Convert.ToDateTime(Request.Form["fechaDesde"]);

            //request.TipoEnfermedad = 1;//tbc
            request.IdEnfermedad = Convert.ToInt32(Request.Form["selEnfermedad"]);
            request.FechaFin = fechafinaux;
            request.FechaInicio = fechainicioaux;
            request.IdUsuario = string.IsNullOrEmpty(Request.Form["idUsuario"]) ? 0 : Convert.ToInt32(Request.Form["idUsuario"]);
            string filtro = Request.Form["nombrefiltro"];
            request.NombreFiltro = filtro;
            switch (filtro)
            {
                case "institucion":
                    request.IdJurisdiccion = Request.Form["hdnInstitucion"];
                    break;
                case "disa":
                    request.IdJurisdiccion = Request.Form["hdnDisa"];
                    break;
                case "red":
                    request.IdJurisdiccion = Request.Form["hdnRed"];
                    break;
                case "microred":
                    request.IdJurisdiccion = Request.Form["hdnMicroRed"];
                    break;
                case "establecimiento":
                    request.IdJurisdiccion = Request.Form["hdnEstablecimiento"];
                    break;
            }

            ReporteBl bl = new ReporteBl();
            List<ReportePieResponse> result = bl.GenerarReporteMuestrasCorregidas(request, tipoReporte);

            if (!result.Any())
            {
                ViewBag.NoDataMessage = "No hay data disponible";
            }

            ViewBag.Labels = JsonConvert.SerializeObject(result.Select(s => s.Indicador));
            ViewBag.DataNumero = JsonConvert.SerializeObject(result.Select(s => s.ValorIndicadorNumero));
            ViewBag.ReporteResult = result;

            ViewBag.Enfermedad = request.IdEnfermedad;
            return PartialView("_BarChartResult", result);
        }
        /* Descripción: Método que retorna la cantidad de Baciloscopias procesadas
         * Autor: Marcos Mejia
           Fecha: 10/07/2018 */
        public ActionResult ReporteCantidadBaciloscopia(int examen, int trimestre, string anio)
        {
            ReporteBl bl = new ReporteBl();
            string nombrefiltro = Request.Form["nombrefiltro"];
            string IdJurisdiccion = "";
            string Jurisdiccion = Request.Form["nombreJurisdiccion"];
            switch (nombrefiltro)
            {
                case "institucion":
                    IdJurisdiccion = Request.Form["hdnInstitucion"];
                    break;
                case "disa":
                    IdJurisdiccion = Request.Form["hdnDisa"];
                    break;
                case "red":
                    IdJurisdiccion = Request.Form["hdnRed"];
                    break;
                case "microred":
                    IdJurisdiccion = Request.Form["hdnMicroRed"];
                    break;
                case "establecimiento":
                    IdJurisdiccion = Request.Form["hdnEstablecimiento"];
                    break;
            }

            List<ReporteCantidadResultado> result = bl.CantidadBaciloscopia(examen, trimestre, anio, nombrefiltro, IdJurisdiccion);
            var pageOfSeg = result.ToPagedList(1, GetSetting<int>(PageSize));
            Session["result"] = result;

            LocalBl lbl = new LocalBl();
            ViewBag.Instituciones = lbl.GetInstituciones();
            ViewBag.Establecimientos = lbl.GetEstablecimientos();
            ViewBag.nombrefiltro = nombrefiltro;
            ViewBag.Jurisdiccion = Jurisdiccion;
            ViewBag.examen = examen;
            return View("ReporteCantidadBaciloscopia", pageOfSeg);
        }
        /* Descripción: Método que convierte una lista en un dataTable
           Fecha: 10/07/2018 */
        public System.Data.DataTable ConvertToDataTable<T>(IList<T> data)
        {
            System.ComponentModel.PropertyDescriptorCollection properties = System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
            System.Data.DataTable table = new System.Data.DataTable();
            foreach (System.ComponentModel.PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                System.Data.DataRow row = table.NewRow();
                foreach (System.ComponentModel.PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        /* Descripción: Método que exporta a formato Excel la consulta de Baciloscopias y el indicador de Usuarios
         * Autor: Marcos Mejia
           Fecha: 30/07/2018 */
        public ActionResult ExportExcel(string examen)
        {
            try
            {
                List<ReporteCantidadResultado> muestra = new List<ReporteCantidadResultado>();
                System.Data.DataTable dtExportData = new System.Data.DataTable("dtExportData");
                int tipo = Convert.ToInt16(examen);
                switch (tipo)
                {
                    case 0:
                        List<ConsultaReporteResultado> result = new List<ConsultaReporteResultado>();
                        result = (List<ConsultaReporteResultado>)this.Session["result"];
                        dtExportData = ConvertToDataTable(result);
                        dtExportData.TableName = "Reporte";
                        dtExportData.Columns.Remove("CantidadConsulta");
                        dtExportData.Columns.Remove("totalResultado");
                        dtExportData.Columns[0].ColumnName = "USUARIO";
                        dtExportData.Columns[1].ColumnName = "EESS ORIGEN";
                        dtExportData.Columns[2].ColumnName = "ENFERMEDAD";
                        dtExportData.Columns[3].ColumnName = "EXAMEN";
                        dtExportData.Columns[4].ColumnName = "INFORME N°";
                        dtExportData.Columns[5].ColumnName = "FECHA VERIFICACION";
                        dtExportData.Columns[6].ColumnName = "FECHA CONSULTA";
                        dtExportData.Columns[7].ColumnName = "N° HORAS RETRASO";
                        break;
                    case 1:
                    case 2:
                        muestra = (List<ReporteCantidadResultado>)this.Session["result"];
                        dtExportData = ConvertToDataTable(muestra);
                        dtExportData.TableName = "Reporte";
                        dtExportData.Columns.Remove("canTriRi");
                        dtExportData.Columns.Remove("canAcumRi");
                        dtExportData.Columns.Remove("canTriRri");
                        dtExportData.Columns.Remove("canAcumRri");
                        dtExportData.Columns[0].ColumnName = "ACTIVIDADES";
                        dtExportData.Columns[1].ColumnName = "CANTIDAD TRIMESTRAL";
                        dtExportData.Columns[2].ColumnName = "CANTIDAD ACUMULADA";
                        dtExportData.Columns[3].ColumnName = "CANTIDAD TRIMESTRAL POSITIVAS";
                        dtExportData.Columns[4].ColumnName = "CANTIDAD ACUMULADA POSITIVAS";
                        break;
                    case 3:
                        muestra = (List<ReporteCantidadResultado>)this.Session["result"];
                        dtExportData = ConvertToDataTable(muestra);
                        dtExportData.TableName = "Reporte";
                        dtExportData.Columns[0].ColumnName = "PRUEBAS DE SENSIBILIDAD";
                        dtExportData.Columns[1].ColumnName = "TRIMESTRAL REALIZADAS";
                        dtExportData.Columns[2].ColumnName = "ACUMULADA REALIZADAS";
                        dtExportData.Columns[3].ColumnName = "TRIMESTRAL SENSIBLE H y R";
                        dtExportData.Columns[4].ColumnName = "ACUMULADA SENSIBLE H y R";
                        dtExportData.Columns[5].ColumnName = "TRIMESTRAL RESISTENTE H";
                        dtExportData.Columns[6].ColumnName = "ACUMULADA RESISTENTE H";
                        dtExportData.Columns[7].ColumnName = "TRIMESTRAL RESISTENTE H y R (MDR)";
                        dtExportData.Columns[8].ColumnName = "ACUMULADA RESISTENTE H y R (MDR)";
                        break;
                    case 4:
                        List<reporteProductividadRom> reporte = new List<reporteProductividadRom>();
                        reporte = (List<reporteProductividadRom>)this.Session["reporteROM"];
                        dtExportData = ConvertToDataTable(reporte);
                        dtExportData.TableName = "Reporte Productividad ROM";
                        dtExportData.Columns[0].ColumnName = "CODIGO ORDEN";
                        dtExportData.Columns[1].ColumnName = "CODIGO MUESTRA";
                        dtExportData.Columns[2].ColumnName = "TIPO DE MUESTRA";
                        dtExportData.Columns[3].ColumnName = "PACIENTE";
                        dtExportData.Columns[4].ColumnName = "ESTABLECIMIENTO ORIGEN";
                        dtExportData.Columns[5].ColumnName = "FECHA SOLICITUD";
                        dtExportData.Columns[6].ColumnName = "FECHA REGISTRO";
                        dtExportData.Columns[7].ColumnName = "FECHA RECEPCION";
                        dtExportData.Columns[8].ColumnName = "ENFERMEDAD";
                        dtExportData.Columns[9].ColumnName = "EXAMEN";
                        break;

                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtExportData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    return new ExcelResult(wb, "ReporteConsulta");
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;filename= ReporteConsulta.xlsx");
                    //using (System.IO.MemoryStream MyMemoryStream = new System.IO.MemoryStream())
                    //{
                    //    wb.SaveAs(MyMemoryStream);
                    //    MyMemoryStream.WriteTo(Response.OutputStream);
                    //    Response.Flush();
                    //    Response.End();
                    //}
                }
                //return View();
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", " ReporteController - ExportExcel ");
                throw ex;
            }
        }
        /* Descripción: Método que retorna la cantidad de Reportes de resultados que fueron consultados
         * Autor: Marcos Mejia
           Fecha: 30/07/2018 */
        public ActionResult GetCantidadResultadosConsultados(ReporteRequest request)
        {
            ReporteBl bl = new ReporteBl();
            List<ConsultaReporteResultado> result = bl.GetCantidadResultadosConsultados(request);

            if (!result.Any())
            {
                ViewBag.NoDataMessage = "No hay data disponible";
            }
            Session["request"] = request;
            return PartialView("_CantResulConsultados", result);
        }
        /* Descripción: Método que retorna el detalle de la cantidad de Reportes de resultados que fueron consultados
         * Autor: Marcos Mejia
           Fecha: 30/07/2018 */
        public ActionResult GeDetalleResultadosConsultados(ReporteRequest request)
        {
            ReporteBl bl = new ReporteBl();
            List<ConsultaReporteResultado> result = bl.GetCantidadResultadosConsultados(request);

            if (!result.Any())
            {
                ViewBag.NoDataMessage = "No hay data disponible";
            }
            Session["result"] = result;
            return PartialView("_DetalleResulConsultados", result);
        }
        /* Descripción: Método que retorna la cantidad de Reportes de resultados que fueron consultados por el Solicitante dentro de los 3 primeros dias.
         * Autor: Marcos Mejia
           Fecha: 07/09/2018 */
        public ActionResult GetCantidadResultadosConsultadosPorSolicitante(ReporteRequest request)
        {
            ReporteBl bl = new ReporteBl();
            List<ConsultaReporteResultado> result = bl.GetCantidadResultadosConsultadosPorSolicitante(request);

            if (!result.Any())
            {
                ViewBag.NoDataMessage = "No hay data disponible";
            }
            return PartialView("_CantResulConsultadosSolicitante", result);
        }
        /* Descripción: Método que retorna el detalle de la cantidad de Reportes de resultados que fueron consultados por el Solicitante dentro de los 3 primeros dias.
         * Autor: Marcos Mejia
           Fecha: 07/09/2018 */
        public ActionResult GetDetalleResultadosConsultadosPorSolicitante(ReporteRequest request)
        {
            ReporteBl bl = new ReporteBl();
            List<ConsultaReporteResultado> result = bl.GetCantidadResultadosConsultadosPorSolicitante(request);

            if (!result.Any())
            {
                ViewBag.NoDataMessage = "No hay data disponible";
            }
            Session["result"] = result;
            return PartialView("_DetalleResulConsultadosSolicitante", result);
        }

        /* Descripción: Método que retorna la cantidad de examenes procesados.
           Autor: Marcos Mejia
           Fecha: 18/11/2019 */
        public ActionResult GetCantidadResultadosInformados()
        {
            ReporteRequest request = new ReporteRequest();
            DateTime fechafinaux = Request.Form["fechaHasta"] == null ? DateTime.Today : Convert.ToDateTime(Request.Form["fechaHasta"]);
            DateTime fechainicioaux = Request.Form["fechaDesde"] == null ? fechafinaux.AddMonths(-1) : Convert.ToDateTime(Request.Form["fechaDesde"]);
            request.TipoReporte = Convert.ToInt16(Request.Form["tipoReportePorEESS"]);
            request.IdEnfermedad = Convert.ToInt32(Request.Form["selEnfermedad"]);
            request.IdExamen = Request.Form["idExamen"];
            request.FechaFin = fechafinaux;
            request.FechaInicio = fechainicioaux;
            string filtro = Request.Form["nombrefiltro"];
            request.NombreFiltro = filtro;
            switch (filtro)
            {
                case "institucion":
                    request.IdJurisdiccion = Request.Form["hdnInstitucion"];
                    break;
                case "disa":
                    request.IdJurisdiccion = Request.Form["hdnDisa"];
                    break;
                case "red":
                    request.IdJurisdiccion = Request.Form["hdnRed"];
                    break;
                case "microred":
                    request.IdJurisdiccion = Request.Form["hdnMicroRed"];
                    break;
                case "establecimiento":
                    request.IdJurisdiccion = Request.Form["hdnEstablecimiento"];
                    break;
            }

            ReporteBl bl = new ReporteBl();
            List<ReporteResultadoInformado> result = bl.GetCantidadResultadosInformados(request);

            if (!result.Any())
            {
                ViewBag.NoDataMessage = "No hay data disponible";
            }

            return PartialView("_CantidadResultadosInformados", result);
        }

        /*yrca*/


        public ActionResult GetReporteProductividad()
        {
            Session["reporteROM"] = null;
            var fecha = Request.Form["fechaDesde"];
            DateTime fechafinaux = Request.Form["fechaHasta"] == null ? DateTime.Today : Convert.ToDateTime(Request.Form["fechaHasta"]);
            DateTime fechainicioaux = Request.Form["fechaDesde"] == null ? fechafinaux.AddDays(-1) : Convert.ToDateTime(Request.Form["fechaDesde"]);
            ReporteBl bl = new ReporteBl();
            ReporteRequest request = new ReporteRequest();
            request.FechaFin = fechafinaux;
            request.FechaInicio = fechainicioaux;
            request.IdUsuario = Logueado.idUsuario;
            List<reporteProductividadRom> reporte = bl.GenerarReporteROM(request);
            Session["reporteROM"] = reporte;
            return View(reporte);
        }

        public ActionResult ExportarReportePDF()
        {
            string usuario = Logueado.nombres + " " + Logueado.apellidoPaterno + " " + Logueado.apellidoMaterno;
            string establecimiento = EstablecimientoSeleccionado.Nombre;
            ViewBag.usuario = usuario;
            ViewBag.establecimiento = establecimiento;
            List<reporteProductividadRom> reporte = new List<reporteProductividadRom>();
            reporte = (List<reporteProductividadRom>)this.Session["reporteROM"];
            string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");
            string header = Server.MapPath("~/Views/ReporteResultados/PrintHeader.html");

            string customSwitches = string.Format("--header-html  \"{0}\" " +
                                                  "--header-spacing \"0\" " +
                                                  "--footer-html \"{1}\" " +
                                                  "--footer-spacing \"10\" " +
                                                  "--footer-line  " +
                                                  "--footer-font-size \"10\" ", header, footer);
            return new Rotativa.ViewAsPdf("ProductividadRomPdf", reporte)
            {
                FileName = "ReporteProductividad-" + usuario + ".pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Landscape,
                PageMargins = { Top = 15, Bottom = 25 },
                CustomSwitches = customSwitches
            };
        }

        public ActionResult ReporteOportunidadRom(string idLaboratorio, string fechaDesde, string fechaHasta)
        {
            List<ReporteOportunidadRom> reporte = new List<ReporteOportunidadRom>();
            DateTime _fechaHasta = Convert.ToDateTime(fechaHasta);
            DateTime _fechaDesde = Convert.ToDateTime(fechaDesde);
            ViewBag.Establecimiento = GetLaboratorioINS();
            ReporteBl bl = new ReporteBl();
            if (idLaboratorio != null)
            {
                reporte = bl.ReporteOportunidadRom(_fechaDesde, _fechaHasta, Convert.ToInt32(idLaboratorio));
            }
            ViewBag.idLaboratorio = Convert.ToInt32(idLaboratorio);
            return View(reporte);
        }

        public ActionResult ReporteOportunidadLaboratorio(string idLaboratorio, string fechaDesde, string fechaHasta)
        {
            List<ReporteOportunidadRom> reporte = new List<ReporteOportunidadRom>();
            DateTime _fechaHasta = Convert.ToDateTime(fechaHasta);
            DateTime _fechaDesde = Convert.ToDateTime(fechaDesde);
            ViewBag.Establecimiento = GetLaboratorioINS();
            ReporteBl bl = new ReporteBl();
            if (idLaboratorio != null)
            {
                reporte = bl.ReporteOportunidadLaboratorio(_fechaDesde, _fechaHasta, Convert.ToInt32(idLaboratorio));
            }
            ViewBag.idLaboratorio = Convert.ToInt32(idLaboratorio);
            return View(reporte);
        }

        public List<Establecimiento> GetLaboratorioINS()
        {
            List<Establecimiento> labIns = new List<Establecimiento>();
            EstablecimientoBl dal = new EstablecimientoBl();
            labIns = dal.GetLaboratorioINS();
            return labIns;
        }

        public ActionResult PacienteCoronavirus(string fechaDesde, string fechaHasta)
        {
            ViewBag.TipoUsuarioEstablecimiento = EstablecimientoSeleccionado.CodigoUnico ;
            var ReporteCoronavirus = new DTOReporteCoronavirus();
            var LrptPacienteCoronavirus = new List<ReportePacienteCoronavirus>();
            DateTime _fechaHasta = Convert.ToDateTime(fechaHasta);
            DateTime _fechaDesde = Convert.ToDateTime(fechaDesde);
            ReporteBl bl = new ReporteBl();
            LrptPacienteCoronavirus = bl.ReportePacienteCoronavirus(_fechaDesde, _fechaHasta);
            ReporteCoronavirus.LstReportePacienteCoronavirus = LrptPacienteCoronavirus;
            var auxpaciente = "";
            List<PacienteCoronavirus> xPacienteCoronavirus = new List<PacienteCoronavirus>();
            foreach (var item in LrptPacienteCoronavirus.OrderBy(x => x.nombrePaciente).ToList())
            {
                int Positivo = 0;
                int Negativo = 0;
                
                if (item.EstatusResultado == "Resultado Verificado")
                {
                    if (auxpaciente != item.nombrePaciente)
                    {
                        var codigoMuestra = "";
                        var antecedentes = "";
                        DateTime fecval = new DateTime();
                        if (LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Verificado" && x.nombrePaciente == item.nombrePaciente && x.convResultado == "POSITIVO").Count() >= 1)
                        {
                            Positivo++;
                            codigoMuestra = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Verificado" && x.nombrePaciente == item.nombrePaciente && x.convResultado == "POSITIVO").FirstOrDefault().codigoMuestra;
                            antecedentes = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Verificado" && x.nombrePaciente == item.nombrePaciente && x.convResultado == "POSITIVO").FirstOrDefault().AntecedenteViaje;
                        }
                        if (LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Verificado" && x.nombrePaciente == item.nombrePaciente && x.convResultado == "NEGATIVO").Count() >= 1)
                        {
                            Negativo++;
                            if (string.IsNullOrEmpty(codigoMuestra))
                            {
                                codigoMuestra = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Verificado" && x.nombrePaciente == item.nombrePaciente && x.convResultado == "NEGATIVO").FirstOrDefault().codigoMuestra;
                                antecedentes = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Verificado" && x.nombrePaciente == item.nombrePaciente && x.convResultado == "NEGATIVO").FirstOrDefault().AntecedenteViaje;
                            }                            
                        }
                        if (Positivo > 0 || Negativo > 0)
                        {
                            var resultado = "";
                            var fechHoraVal = "";
                            if (Positivo > 0)
                            {
                                resultado = "POSITIVO";
                                fecval = item.FechaValidado.Value.Date;
                                fechHoraVal = item.FechHoraValidado2;
                            }
                            else if (Negativo > 0)
                            {
                                resultado = "NEGATIVO";
                                fecval = item.FechaValidado.Value.Date;
                                fechHoraVal = item.FechHoraValidado2;
                            }
                            
                            PacienteCoronavirus ePaciente = new PacienteCoronavirus()
                            {
                                codigoMuestra = codigoMuestra,
                                codigoOrden = item.codigoOrden,
                                EESSDepOrigen = item.EESSDepOrigen,
                                EESSProvOrigen = item.EESSProvOrigen,
                                EESSDistOrigen = item.EESSDistOrigen,
                                EESSDisaOrigen = item.EESSDisaOrigen,
                                EESSRedOrigen = item.EESSRedOrigen,
                                EESSMicroRedOrigen = item.EESSMicroRedOrigen,
                                EstablecimientoOrigen = item.EstablecimientoOrigen,
                                DocIdentidad = item.DocIdentidad,
                                fechaNacimiento = item.fechaNacimiento,
                                nombrePaciente = item.nombrePaciente,
                                edad = item.edad,
                                SexoPaciente = item.SexoPaciente,
                                TipoMuestra = item.TipoMuestra,
                                FechaColeccion = item.FechaColeccion,
                                FechaRecepcionROM = item.FechaRecepcionROM,
                                FechaValidado = fecval,
                                MuestraConforme = item.MuestraConforme,
                                Positivo = Positivo,
                                Negativo = Negativo,
                                Resultado = resultado,
                                direccion = item.DireccionPaciente,
                                AntecedenteViaje = antecedentes,
                                FechHoraValidado2 = fechHoraVal,
                                celular = item.celular

                            };
                            xPacienteCoronavirus.Add(ePaciente);
                        }
                    }
                    auxpaciente = item.nombrePaciente;
                }
            }
            ReporteCoronavirus.LstReporteMuestrasCoronavirus = LrptPacienteCoronavirus;
            ReporteCoronavirus.LstPacienteCoronavirus = xPacienteCoronavirus;
            //ReporteCoronavirus.TotaPacientesVerificados = xPacienteCoronavirus.Distinct().Count();
            //ReporteCoronavirus.TotalMuestrasNegativos = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Verificado" && x.convResultado == "NEGATIVO").Count();
            //ReporteCoronavirus.TotalMuestrasPositivo = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Verificado" && x.convResultado == "POSITIVO").Count();
            ReporteCoronavirus.TotalPacientesNegativos = xPacienteCoronavirus.Where(x => x.Negativo == 1 && x.Positivo == 0).Count();
            ReporteCoronavirus.TotalPacientesPositivos = xPacienteCoronavirus.Where(x => x.Positivo == 1).Count();
            //ReporteCoronavirus.TotalMuestrasPendientes = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Pendiente").Count();
            //ReporteCoronavirus.TotalMuestrasIngresadas = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Ingresado por Rom").Count();
            //ReporteCoronavirus.TotalMuestrasRechzosRom = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Rechazo Rom").Count();
            //ReporteCoronavirus.TotalMuestrasRechzosLab = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Rechazo Laboratorio").Count();
            //ReporteCoronavirus.TotalMuestrasPendienteVerificacion = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Pendiente Verificación de Resultados").Count();
            //ReporteCoronavirus.TotalMuestrasVerificacion = LrptPacienteCoronavirus.Where(x => x.EstatusResultado == "Resultado Verificado").Count();
            //ReporteCoronavirus.TotalPacientesOrdenadoxDepxFechaVal = from s in xPacienteCoronavirus
            //                         orderby s.EESSDepOrigen, s.FechaValidado
            //                         select new { s };
            //var LstPacienteCoronavirusxDep = from line in xPacienteCoronavirus
            //                                 group line by line.EESSDepOrigen into g
            //                                 select new PacienteCoronavirusxDep
            //                                 {
            //                                     Region = g.First().EESSDepOrigen,
            //                                     PositivoRegion = g.Sum(pc => pc.Positivo),
            //                                     NegativoRegion = g.Sum(pc => pc.Negativo)
            //                                 };
            //ReporteCoronavirus.LstPacienteCoronavirusxDep = LstPacienteCoronavirusxDep.ToList();
            //ReporteCoronavirus.LstMuestrasCoronavirusxDep = from line in LrptPacienteCoronavirus
            //                                                group line by line.EESSDepOrigen into g
            //                                                select new PacienteCoronavirus
            //                                                {
            //                                                    EESSDepOrigen = g.First().EESSDepOrigen,
            //                                                    TotalDepPos = g.Sum(pc => pc.Positivo),
            //                                                    TotalDepNeg = g.Sum(pc => pc.Negativo)
            //                                                };
            Session["reporteCoronavirus"] = ReporteCoronavirus;
            return View("ReportePacienteExamen", ReporteCoronavirus);
        }
        [HttpPost]
        public string DatochkDatosPaciente(string check)
        {
            Session["DatochkDatosPaciente"] = check;
            return "";
        }
        public ActionResult ExportExcelCoronavirus()
        {
            DTOReporteCoronavirus ReporteCoronavirus2 = new DTOReporteCoronavirus();
            ReporteCoronavirus2 = (DTOReporteCoronavirus)Session["reporteCoronavirus"];
            var Paciente = (string)Session["DatochkDatosPaciente"];
            //ReporteCoronavirus.LstPacienteCoronavirus = xPacienteCoronavirus;
            List<ReporteCantidadResultado> muestra = new List<ReporteCantidadResultado>();
            System.Data.DataTable dtExportData = new System.Data.DataTable("dtExportData");
            List<PacienteCoronavirus> xPacienteCoronavirus = new List<PacienteCoronavirus>();
            //xPacienteCoronavirus = ReporteCoronavirus2.LstPacienteCoronavirus.OrderBy(x => x.FechaValidado && x.FechHoraValidado2).ToList();
            int i = 1;
            foreach (var item in ReporteCoronavirus2.LstPacienteCoronavirus.OrderBy(x => x.FechHoraValidado2).ToList())
            {
                item.NroPaciente = i++;
                xPacienteCoronavirus.Add(item);
            }
            dtExportData = ConvertToDataTable(xPacienteCoronavirus);
            if (Paciente == "1")
            {
                dtExportData.TableName = "Reporte";
                dtExportData.Columns.Remove("codigoOrden");
                dtExportData.Columns.Remove("nroOficio");
                dtExportData.Columns.Remove("EESSProvOrigen");
                dtExportData.Columns.Remove("EESSDistOrigen");
                dtExportData.Columns.Remove("EESSDisaOrigen");
                dtExportData.Columns.Remove("EESSRedOrigen");
                dtExportData.Columns.Remove("EESSMicroRedOrigen");
                dtExportData.Columns.Remove("FechaColeccion");
                dtExportData.Columns.Remove("FechaRecepcionROM");
                dtExportData.Columns.Remove("FechaHoraColeccion");
                dtExportData.Columns.Remove("FechaHoraRecepcionROM");
                dtExportData.Columns.Remove("FechaHoraRecepcionLAB");
                dtExportData.Columns.Remove("FechaHoraValidado");
                dtExportData.Columns.Remove("MuestraConforme");
                dtExportData.Columns.Remove("CriteriosRechazo");
                dtExportData.Columns.Remove("Positivo");
                dtExportData.Columns.Remove("Negativo");
                dtExportData.Columns.Remove("TotalDepNeg");
                dtExportData.Columns.Remove("fechaNacimiento");

                dtExportData.Columns[0].ColumnName = "Nro Pacientes";
                dtExportData.Columns[1].ColumnName = "Región";
                dtExportData.Columns[2].ColumnName = "Establecimiento de Origen";
                dtExportData.Columns[3].ColumnName = "Nombre de Paciente";
                dtExportData.Columns[4].ColumnName = "Nro Documento";
                dtExportData.Columns[5].ColumnName = "Dirección del Paciente";
                dtExportData.Columns[6].ColumnName = "Código de Muestra";
                dtExportData.Columns[7].ColumnName = "Tipo de Muestra";
                dtExportData.Columns[8].ColumnName = "Edad";
                dtExportData.Columns[9].ColumnName = "Sexo";
                dtExportData.Columns[10].ColumnName = "Fecha de Resultado";
                dtExportData.Columns[11].ColumnName = "Resultado";
                dtExportData.Columns[12].ColumnName = "Antecedente de Viaje";
                dtExportData.Columns[13].ColumnName = "Georefenciacion EESS Origen";

            }
            else
            {
                dtExportData.TableName = "Reporte";
                dtExportData.Columns.Remove("codigoOrden");
                dtExportData.Columns.Remove("nroOficio");
                dtExportData.Columns.Remove("nombrePaciente");
                dtExportData.Columns.Remove("EESSProvOrigen");
                dtExportData.Columns.Remove("EESSDistOrigen");
                dtExportData.Columns.Remove("EESSDisaOrigen");
                dtExportData.Columns.Remove("EESSRedOrigen");
                dtExportData.Columns.Remove("EESSMicroRedOrigen");
                dtExportData.Columns.Remove("FechaColeccion");
                dtExportData.Columns.Remove("FechaRecepcionROM");
                dtExportData.Columns.Remove("FechaHoraColeccion");
                dtExportData.Columns.Remove("FechaHoraRecepcionROM");
                dtExportData.Columns.Remove("FechaHoraRecepcionLAB");
                dtExportData.Columns.Remove("FechaHoraValidado");
                dtExportData.Columns.Remove("MuestraConforme");
                dtExportData.Columns.Remove("CriteriosRechazo");
                dtExportData.Columns.Remove("Positivo");
                dtExportData.Columns.Remove("Negativo");
                dtExportData.Columns.Remove("TotalDepNeg");
                dtExportData.Columns.Remove("fechaNacimiento");
                dtExportData.Columns.Remove("Direccion");
                dtExportData.Columns.Remove("AntecedenteViaje");
                dtExportData.Columns.Remove("DocIdentidad");

                dtExportData.Columns[0].ColumnName = "Nro Pacientes";
                dtExportData.Columns[1].ColumnName = "Región";
                dtExportData.Columns[2].ColumnName = "Establecimiento de Origen";
                dtExportData.Columns[3].ColumnName = "Codigo de Muestra";
                dtExportData.Columns[4].ColumnName = "Tipo de Muestra";
                dtExportData.Columns[5].ColumnName = "Edad";
                dtExportData.Columns[6].ColumnName = "Sexo";
                dtExportData.Columns[7].ColumnName = "Fecha de Resultado";
                //dtExportData.Columns[8].ColumnName = "Resultado";
            }
           

            using (XLWorkbook wb = new XLWorkbook())
            {
                var aCode = 65;
                var ws = wb.Worksheets.Add("Coronavirus");
                var wsReportNameHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 1, Char.ConvertFromUtf32(aCode + dtExportData.Columns.Count)));
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNameHeaderRange.Value = "Muestras Procesadas el " + DateTime.Now.ToString("dd/MM/yyyy") + " : " + xPacienteCoronavirus.Where(x => x.FechaValidado == DateTime.Parse(DateTime.Now.ToShortDateString())).ToList().Count();

                var wsReportDateHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 2, Char.ConvertFromUtf32(aCode + dtExportData.Columns.Count)));
                wsReportDateHeaderRange.Merge();
                wsReportDateHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportDateHeaderRange.Value = string.Format("Total de Muestras :{0}", xPacienteCoronavirus.Count());

                var wsReportCreatedByHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 3, Char.ConvertFromUtf32(aCode + dtExportData.Columns.Count)));
                wsReportCreatedByHeaderRange.Merge();
                wsReportCreatedByHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportCreatedByHeaderRange.Value = string.Format("Positivos: {0}", xPacienteCoronavirus.Where(x => x.Positivo == 1).Count());

                var wsReportNegativoRange = ws.Range(string.Format("A{0}:{1}{0}", 4, Char.ConvertFromUtf32(aCode + dtExportData.Columns.Count)));
                wsReportNegativoRange.Merge();
                wsReportNegativoRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNegativoRange.Value = string.Format("Negativos: {0}", xPacienteCoronavirus.Where(x => x.Negativo == 1 && x.Positivo == 0).Count());

                ws.Row(4).InsertRowsBelow(1);
                ws.Row(5).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                ws.Row(5).Style.Border.RightBorder = XLBorderStyleValues.None;
                ws.Row(5).Style.Border.LeftBorder = XLBorderStyleValues.None;
                int rowIndex = 6;
                int columnIndex = 0;
                foreach (DataColumn column in dtExportData.Columns)
                {
                    ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Value = column.ColumnName;

                    ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Style.Font.Bold = true;
                    columnIndex++;
                }
                rowIndex++;
                foreach (DataRow row in dtExportData.Rows)
                {
                    int valueCount = 0;
                    foreach (object rowValue in row.ItemArray)
                    {                       
                        ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + valueCount), rowIndex)).Value = rowValue;
                        ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + valueCount), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        valueCount++;
                    }
                    rowIndex++;
                }
                ws.Columns().AdjustToContents();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= ReporteConsulta.xlsx");
                using (System.IO.MemoryStream MyMemoryStream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }
    }
}

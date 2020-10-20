using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Data;
using System.ComponentModel;
using ClosedXML.Excel;
using NetLab.Extensions.ActionResults;
using BusinessLayer;
using NetLab.Helpers;
using Rotativa.Options;

namespace NetLab.Controllers
{
    public class IndicadorController : ParentController
    {
        // GET: Indicador
        public ActionResult index(int param1)
        {
            ViewBag.fechaDesde = DateTime.Now.ToDefaultDateFormatString();
            ViewBag.fechaHasta = DateTime.Now.ToDefaultDateFormatString();
            Session["idExamenAgrupado"] = param1;
            switch (param1)
            {
                case 5:
                    ViewBag.Examen = "COMPLEJO MYCOBACTERIUM TUBERCULOSIS [IDENTIFICACIÓN] Y RESISTENCIA A FLUOROQUINOLONAS, AMINOGLUCÓSIDOS / PÉPTIDOS CÍCLICOS [SUSCEPTIBILIDAD] ASOCIADA A GENES [PRESENCIA] POR ENSAYO DE SONDA LINEAL";
                    break;
                case 6:
                    ViewBag.Examen = "ENSAYO DE SONDA EN LÍNEA PARA LA IDENTIFICACIÓN Y DETECCIÓN DE RESISTENCIA A RIFAMPICINA E ISONIACIDA DEL COMPLEJO MYCOBACTERIUM TUBERCULOSIS";
                    break;
                case 13:
                    ViewBag.Examen = "CUANTIFICACIÓN DE CARGA VIRAL VIH-1";
                    break;
                case 14:
                    ViewBag.Examen = "RECUENTO DE LINFOCITOS T CD4,CD8,CD3";
                    break;
                case 17:
                    ViewBag.Examen = "RT-PCR";
                    break;
                case 19:
                    ViewBag.Examen = "SARS COV19";
                    break;
                default:
                    break;
            }

            Session["nombreExamen"] = ViewBag.Examen;
            return index("1", ViewBag.fechaDesde, ViewBag.fechaHasta, ViewBag.Examen);
        }
        [HttpPost]
        public ActionResult index(string tipoIndicador, string fechaDesde, string fechaHasta, string examen)
        {
            var fechaDesdeA = new DateTime();
            var fechaHastaA = new DateTime();
            var idExamenAgrupado = (int)(Session["idExamenAgrupado"]);

            var fechaDesdeCriteria = fechaDesde ?? string.Empty;
            var fechaHastaCriteria = fechaHasta ?? string.Empty;

            if (!fechaDesdeCriteria.Equals(""))
                fechaDesdeA = DateTime.Parse(fechaDesdeCriteria, CultureInfo.CreateSpecificCulture("es-PE"));
            if (!fechaHastaCriteria.Equals(""))
                fechaHastaA = DateTime.Parse(fechaHastaCriteria, CultureInfo.CreateSpecificCulture("es-PE"));

            Session["fechaInicio"] = fechaDesde;
            Session["fechaFin"] = fechaHasta;
            Session["tipoIndicador"] = tipoIndicador;

            var indicadorBl = new IndicadorBl();
            var resultado = new List<Model.ViewData.OportunidadResultado>();
            if (idExamenAgrupado == 19)
            {
                resultado = indicadorBl.GetOportunidadResultadoCovid("1", fechaDesdeA, fechaHastaA, idExamenAgrupado, EstablecimientoSeleccionado.IdEstablecimiento);
            }
            else
            {
                resultado = indicadorBl.GetOportunidadResultado("1", fechaDesdeA, fechaHastaA, idExamenAgrupado, EstablecimientoSeleccionado.IdEstablecimiento);
            }
            
            Session["resultado"] = resultado;
            ViewBag.Examen = (string)Session["nombreExamen"];
            return View(resultado);
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

        public ActionResult ExportarReportePDF(string FechaInicio, string FechaFin)
        {           
            ViewBag.Examen = (string)Session["nombreExamen"];
            var x = ViewBag.fechaDesde;
            List<Model.ViewData.OportunidadResultado> reporte = new List<Model.ViewData.OportunidadResultado>();
            if (this.Session["resultado"] != null)
            {
                reporte = (List<Model.ViewData.OportunidadResultado>)this.Session["resultado"];
                string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");

                string customSwitches = string.Format("--debug-javascript --javascript-delay 500 --enable-javascript --no-stop-slow-scripts  " +
                                                        "--footer-html \"{0}\" " +
                                                       "--footer-spacing \"10\" " +
                                                       "--footer-line  " +
                                                       "--footer-font-size \"10\" ", footer);
                return new ViewAsPdf("OportunidadPDF", reporte)
                {
                    FileName = "Oportunidad-" + ViewBag.Examen + ".pdf",
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 10, 30, 10),
                    CustomSwitches = customSwitches
                };
            }
            else
            {
                return null;
            }
        }

        public ActionResult ExportarExcel()
        {
            List<Model.ViewData.OportunidadResultado> lista = new List<Model.ViewData.OportunidadResultado>();
            if (this.Session["resultado"] != null)
            {
                lista = (List<Model.ViewData.OportunidadResultado>)this.Session["resultado"];
            }

            DataTable dtlista = new DataTable("dtlista");
            dtlista = ConvertToDataTable(lista);
            dtlista.Columns.Remove("idIndicador");
            dtlista.TableName = "Oportunidad de Resultados";
            dtlista.Columns[0].ColumnName = "Total Muestras";
            dtlista.Columns[1].ColumnName = "Año";
            dtlista.Columns[2].ColumnName = "Mes";
            dtlista.Columns[3].ColumnName = "Muestras dentro del Tiempo en Catalogo";
            dtlista.Columns[4].ColumnName = "Oportunidad (%)";
            dtlista.Columns[5].ColumnName = "Tiempo Catalogo";

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

using BusinessLayer;
using ClosedXML.Excel;
using Model.ViewModels;
using NetLab.App_Code;
using NetLab.Extensions.ActionResults;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public partial class OrdenMuestraController : ParentController
    {
        // GET: Recepcion
        public ActionResult RegistroRecepcionMasivaROM()
        {
            int labVirusResp = 995;
            ViewBag.LabVirusRespId = labVirusResp;
            ViewBag.LabVirusRespNombre = StaticCache.ObtenerLaboratorios().FirstOrDefault(x => x.IdEstablecimiento == 995).Nombre;
            return View();
        }

        public JsonResult VerificarMuestraPorRecepcionar(string codigoMuestra)
        {
            var bl = new RecepcionBl();
            return Json(bl.VerificarMuestraPorRecepcionar(codigoMuestra), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RecepcionarMuestraROM(OrdenMuestraVM model)
        {
            var bl = new RecepcionBl();
            model.IdUsuario = Logueado.idUsuario;
            var recepcionado = bl.RecepcionarMuestra(model);
            if (recepcionado.Any())
            {
                return PartialView("_MuestrasRecepcionadas_NuevaFila", recepcionado);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult ObtenerMuestrasRecepcionadas()
        {
            //List<OrdenMuestraRecepcionados> listado = new List<OrdenMuestraRecepcionados>();
            List<SelectListItem> motivoRechazoItems = new List<SelectListItem>();

            ViewBag.MotivosRechazo = motivoRechazoItems;

            return PartialView("_MuestrasRecepcionadasROM");//, listado);
        }

        public ActionResult FinalizarProceso(List<OrdenMuestraRecepcionados> model)
        {
            model.ForEach(m => { m.IdUsuario = Logueado.idUsuario; });

            //rechazar
            var bl = new RecepcionBl();
            foreach (var item in model.Where(x=> x.MotivoRechazo.Any()))
            {
                bl.RechazarMuestra(item);
            }

            var listadoExcel = new List<OrdenMuestraRecepcionadosExcel>();
            //procesar
            foreach(var item in model.Where(x=> !x.MotivoRechazo.Any()))
            {
                listadoExcel.Add(new OrdenMuestraRecepcionadosExcel
                {
                    CodigoMuestra = item.CodigoMuestra,
                    CodigoLineal = item.CodigoLineal, //string.IsNullOrWhiteSpace(item.CodigoLineal) ? "No generado": item.CodigoLineal,
                    Paciente = item.Paciente
                });
            }

            DataTable dt = new DataTable("muestrasrecepcionadas");
            dt = ConvertToDataTable(listadoExcel);
            dt.TableName = "listado";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                return new ExcelResult(wb, "MuestrasRecepcionadas");
            }
        }
    }
}
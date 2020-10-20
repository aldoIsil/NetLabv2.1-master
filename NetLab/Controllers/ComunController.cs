using BusinessLayer;
using BusinessLayer.Interfaces;
using DataLayer;
using Enums;
using Model.Entidades;
using Model.ViewModels;
using NetLab.App_Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilitario;

namespace NetLab.Controllers
{
    public class ComunController : ParentController
    {
        // GET: Comun
        public JsonResult ObtenerEnfermedadesPorNombre(string nombre)
        {
            IEnfermedadBl enfermedadBl = new EnfermedadBl();
            var enfermedadList = enfermedadBl.GetEnfermedadesByNombre(nombre);

            var resultado = enfermedadList.Select(enf => new { id = enf.idEnfermedad, name = string.Format("{0} - {1}", enf.Snomed, enf.nombre) }).ToList();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerEstablecimientosPorNombre(string nombre, string ExamenVA, TipoRegistroOrden tipoRegistro)
        {
            var laboratorioBl = new LaboratorioBl();
            var laboratorioList = StaticCache.ObtenerLaboratorios();
            laboratorioList = string.IsNullOrWhiteSpace(nombre) ? laboratorioList : laboratorioList.Where(x => x.Nombre.ToLower().Contains(nombre.Trim().ToLower()) || x.CodigoUnico.ToLower().Contains(nombre.Trim().ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(ExamenVA) && (ExamenVA.ToUpper() == "VALIDADOR" || ExamenVA.ToUpper() == "ROMINS"))
            {
                var ClasificacionEESS = EstablecimientoSeleccionado.clasificacion.ToString();
                if (ClasificacionEESS.TrimEnd().ToString().Contains("LAB INS"))
                {
                    laboratorioList = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                }
                else
                {
                    laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                }
                return Json(laboratorioList, JsonRequestBehavior.AllowGet);
            }

            if (tipoRegistro == TipoRegistroOrden.ORDEN_RECEPCION && (EstablecimientoSeleccionado.IdEstablecimiento == 991 || EstablecimientoSeleccionado.IdEstablecimiento == 23638))
            {
                laboratorioList = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                return Json(laboratorioList, JsonRequestBehavior.AllowGet);
            }
            if (tipoRegistro == TipoRegistroOrden.SOLO_ORDEN_MUESTRA)
            {
                if (ExamenVA.ToUpper() == "VALIDADOR")
                {
                    var ClasificacionEESS = EstablecimientoSeleccionado.clasificacion.ToString();
                    if (ClasificacionEESS.TrimEnd().ToString().Contains("LAB INS"))
                    {
                        laboratorioList = laboratorioList.FindAll(p => p.IdLabIns == 1).ToList();
                    }
                    else
                    {
                        laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                    }
                }
                else
                {
                    laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
                }

            }
            else
            {
                laboratorioList = laboratorioList.FindAll(p => p.IdLabIns != 1).ToList();
            }

            return Json(laboratorioList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerExamenesPorNombre(int genero, int idenfermedad, string nombre)
        {
            try
            {
                var examenList = ObtenerExamenes(genero, idenfermedad);
                examenList = string.IsNullOrWhiteSpace(nombre) ? examenList : examenList.Where(x => x.Nombre.ToLower().Contains(nombre.Trim().ToLower())).ToList();
                //List<ExamenesVM> examenes = new List<ExamenesVM>();
                //examenList.ForEach(x => examenes.Add(new ExamenesVM { IdExamen = x.IdExamen, NombreExamen = x.Nombre }));
                var resultado = examenList.Select(x => new { id = x.IdExamen.ToString(), name = x.Nombre }).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", " ObtenerExamenesPorNombre ");
                throw ex;
            }
        }

        public List<Examen> ObtenerExamenes(int genero, int idenfermedad)
        {
            List<Examen> resultados = new List<Examen>();
            string nombreSession = string.Format("ListaExamenesEnf{0}", idenfermedad);
            if (Session[nombreSession] != null)
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
            var bl = new CoreBl();
            resultados = bl.GetExamenesPorEnfermedad(idenfermedad);

            return resultados;
        }

        public ActionResult ObtenerTipoMuestraPorExamen(Guid idExamen)
        {
            var bl = new CoreBl();
            var tipoMuestras = bl.ObtenerTipoMuestraPorExamen(idExamen);
            return PartialView("_Crear_TipoMuestra", tipoMuestras);
        }

        public ActionResult ObtenerSolicitantes(string nombre)
        {
            SolicitanteBl solicitanteBL = new SolicitanteBl();
            var solicitantes = solicitanteBL.GetListaSolicitante(nombre);
            var resultado = solicitantes.Select(x => new { id = x.idSolicitante, name = string.Format("{0} - {1} {2} {3}", x.codigoColegio, x.apellidoPaterno, x.apellidoMaterno, x.Nombres) }).ToList();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //[AllowAnonymous]
        public JsonResult ObtenerEstadoReniec()
        {
            var pacienteDal = new PacienteDal();
            return Json(pacienteDal.EstadoReniec(), JsonRequestBehavior.AllowGet);
        }

        //[AllowAnonymous]
        public ActionResult DesactivarReniec()
        {
            var bl = new CoreBl();
            bl.DesactivarReniec();
            return new EmptyResult();
        }

        //[AllowAnonymous]
        public ActionResult ActivarReniec()
        {
            var bl = new CoreBl();
            bl.ActivarReniec();
            return new EmptyResult();
        }

        public ActionResult DescargaResultados(Guid id)
        {
            return new EmptyResult();
        }

        public ActionResult TableTypePrueba(int inicio)
        {
            for (int i = inicio; i < inicio + 100; i++)
            {
                var bl = new CoreBl();
                bl.CrearOrdenDatosClinicosTest(i);
            }

            return new EmptyResult();
        }

        public ActionResult EliminarDatosExamenPorUsuario(int idUsuario)
        {
            StaticCache.EliminarDatosExamenPorUsuario(idUsuario);
            return new EmptyResult();
        }

        public ActionResult LimpiarTodosDatosExamenes()
        {
            StaticCache.LimpiarTodosDatosExamenes();
            return new EmptyResult();
        }
    }
}
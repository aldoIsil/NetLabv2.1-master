using BusinessLayer;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class HomeController : ParentController
    {
        /// <summary>
        /// Descripción: Controlador para la presentacion del sistema.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 10/11/2017.
        /// Modificación: Se agregaron comentarios.
        ///               JRCR - Metodos para la obtención de información de los EESS:
        ///               GetEstablecimientoAllList
        ///               GetInstitucionList
        ///               GetDisaList
        ///               GetRedList
        ///               GetMicroRedList
        ///               GetEstablecimientoList
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {            
            ViewBag.Message = "Pagina Principal";

            return View();
        }
        /// <summary>
        /// Descripción: Controlador para las opciones que aún no estan completas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult EnConstruccion()
        {
            ViewBag.Message = "Esta página se encuentra en construcción.";

            return View();
        }
        /// <summary>
        /// Descripción: Metodos para la obtención de información de los EESS:
        /// GetEstablecimientoAllList
        /// GetInstitucionList
        /// GetDisaList
        /// GetRedList
        /// GetMicroRedList
        /// GetEstablecimientoList
        /// Author: José chávez R.
        /// Fecha Creacion: 10/11/2017
        /// </summary>
        /// <returns></returns>
        #region ConfiguracionListaReportes
        public ActionResult GetEstablecimientoAllList()
        {
            var result = ((List<Establecimiento>)Session["reporteEstablecimientos"]);
            return PartialView("_ddlEstablecimiento", result);
        }

        public ActionResult GetInstitucionList()
        {
            var result = ((List<Institucion>)Session["reporteInstituciones"]);
            return PartialView("_ddlInstitucion", result);
        }

        public ActionResult GetDisaList(int idInstitucion)
        {
            var result = ((List<DISA>)Session["reporteDISAs"]).Where(x => x.IdInstitucion == idInstitucion).ToList();
            return PartialView("_ddlDisa", result);
        }

        public ActionResult GetRedList(string idDisa, int idInstitucion)
        {
            var result = ((List<Red>)Session["reporteRedes"]).Where(x => x.IdDISA == idDisa && x.IdInstitucion == idInstitucion).ToList();
            return PartialView("_ddlRed", result);
        }

        public ActionResult GetMicroRedList(string idRed, string idDisa, int idInstitucion)
        {
            var result = ((List<MicroRed>)Session["reporteMicroRedes"]).Where(x => x.IdRed == idRed && x.IdDISA == idDisa && x.IdInstitucion == idInstitucion).ToList();
            return PartialView("_ddlMicroRed", result);
        }

        public ActionResult GetEstablecimientoList(string idMicroRed, string idRed, string idDisa, string idInstitucion)
        {
            var result = ((List<Establecimiento>)Session["reporteEstablecimientos"]).Where(x => x.CodigoMicroRed == idMicroRed && x.CodigoRed == idRed && x.CodigoDisa == idDisa && x.CodigoInstitucion == idInstitucion).ToList();

            result.Insert(0,
                new Establecimiento { IdEstablecimiento = -1, Nombre = "Seleccione un Establecimiento", CodigoUnico = "00000" });
            return PartialView("_ddlEstablecimiento", result);
        }
        #endregion

        public JsonResult GetExamenUsuario(string Prefix)
        {           
           return Json(new ExamenBl().GetExamenUsuario(Prefix, Logueado.idUsuario), JsonRequestBehavior.AllowGet);
        }
    }
}
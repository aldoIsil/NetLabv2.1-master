using System.Web.Mvc;
using BusinessLayer;
using Model;
using PagedList;
using BusinessLayer.Interfaces;
using NetLab.Models;
using Framework.WebCommon;

namespace NetLab.Controllers
{
    public class CatalogoOfflineController : Controller
    {
        /// <summary>
        /// Descripción: Controlador para mostrar informacion del catalogo de servicio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="busqueda"></param>
        /// <returns></returns>
        public ActionResult Index(int? page, string search, int? busqueda)
        {
            if ((page == null) && (search == null) && (busqueda == null))
                return View();
            else
            {
                var pageNumber = page ?? 1;
                var searchCriteria = search ?? string.Empty;

                var catalogoBl = new CatalogoBl();
                var catalogos = catalogoBl.GetEnfermedades(searchCriteria);
                var pageOfCatalogo = catalogos.ToPagedList(pageNumber, GetSetting<int>("pageSize"));

                ViewBag.search = searchCriteria;

                return View(pageOfCatalogo);
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar informacion del detalle del catalogo de servicio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017.
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult Details(int id, int? page, string search)
        {
            var pageNumber = page ?? 1;
            var searchCriteria = search ?? string.Empty;

            ViewBag.IsSearch = true;

            var detallecatalogoBl = new DetalleCatalogoBl();
            var detalle = detallecatalogoBl.GetCatalogoByIdEnfermedad(id, null);
            return PartialView("DetalleCatalogoOffline", detalle);
        }

        protected static T GetSetting<T>(string settingName)
        {
            return WebConfig.GetSetting<T>(settingName);
        }
    }
}

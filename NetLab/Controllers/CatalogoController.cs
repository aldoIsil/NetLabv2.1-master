using System.Web.Mvc;
using BusinessLayer;
using Model;
using PagedList;
using BusinessLayer.Interfaces;
using NetLab.Models;
using System.Collections.Generic;
using System.Linq;

namespace NetLab.Controllers
{
    public class CatalogoController : ParentController
    {       
        /// <summary>
        /// Descripción: Controlador para la presentacion de la busqueda del Catalogo de Servicio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="busqueda"></param>
        /// <returns></returns>
        [HttpGet]
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
                var pageOfCatalogo = catalogos.ToPagedList(pageNumber, GetSetting<int>(PageSize));

                ViewBag.search = searchCriteria;

                return View(pageOfCatalogo);
            }
        }

        /// <summary>
        /// Descripción: Controlador para mostrar el detalle en un poppup el resultado de la busqueda realizada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult ListarDetalle(int idCatalogo,  int? page, string search)
        //{
        //    var pageNumber = page ?? 1;
        //    ViewBag.IsSearch = true;
        //    var detallecatalogoBl = new DetalleCatalogoBl();
        //    var detalle = detallecatalogoBl.GetCatalogoByIdEnfermedad(idCatalogo);
            
        //    var pageOfDetCatalogo = detalle.ToPagedList(pageNumber, GetSetting<int>(PageSize));

        //    return PartialView("DetalleCatalogo", pageOfDetCatalogo);
        //}
        [HttpGet]
        public ActionResult ListarDetalle(int idCatalogo, string CodigoUnico, string Texto, int? page, string search)
        {
            var detallecatalogoBl = new DetalleCatalogoBl();
            var detalle = detallecatalogoBl.GetCatalogoByIdEnfermedad(idCatalogo, string.IsNullOrEmpty(Request.Params["hddDato"]) ? null : Request.Params["hddDato"].ToString().Substring(0, 8));
            if (!string.IsNullOrEmpty(Texto))
            {
                detalle = detalle.Where(e => e.Prueba.Contains(Texto)).ToList();
            }
            var pageNumber = page ?? 1;
                
            var pageOfDetCatalogo = detalle.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = search;
            ViewBag.idCatalogo = idCatalogo;
            ViewBag.CodigoUnico = CodigoUnico;
            ViewBag.Texto = Texto;
            
            return PartialView("DetalleCatalogo", pageOfDetCatalogo);
        }

        #region CargaEESS-Autocomplete
        [HttpPost]
        public JsonResult GetEESS(string Prefix)
        {
            var establecimientoBl = new EstablecimientoBl();
            var establecimientoList = establecimientoBl.GetEstablecimientosByTextoBusqueda(Prefix, ((Usuario)Session["UsuarioLogin"]).idUsuario);

            return Json(establecimientoList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

}

using System.Web.Mvc;
using BusinessLayer;
using Model;
using PagedList;
using BusinessLayer.Interfaces;
using NetLab.Models;

namespace NetLab.Controllers
{
    public class DetalleCatalogoController : ParentController
    {
        // GET: DetalleCatalogo
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Descripción: Controlador para mostrar el detalle del Catalogo del servicio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult ListarDetalle(int id, int? page, string search)
        {

            var pageNumber = page ?? 1;
            var searchCriteria = search ?? string.Empty;

            ViewBag.IsSearch = true;


            var detallecatalogoBl = new DetalleCatalogoBl();
            var detalle = detallecatalogoBl.GetCatalogoByIdEnfermedad(id, null);
            return PartialView("DetalleCatalogo", detalle);

            /*

            var orden = new ResultadoBl().GetOrdenById(id);
            /* 
            if(ordenes!=null)
            {
                var pageOfValidaciones = ordenes.ToPagedList(pageNumber, 50);
                return View(pageOfValidaciones);
            }

            var filtered = ordenes.Where(pr => pr.idOrden.ToString().Contains(searchCriteria.ToUpper()));
            var pageOfPresentacion = filtered.ToPagedList(pageNumber, GetSetting<int>(PageSize));
            ViewBag.search = searchCriteria;
            */


        }

    }
}
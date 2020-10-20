using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Plantilla;
using Model;
using NetLab.Models.Plantilla;
using PagedList;

namespace NetLab.Controllers
{
    public class EstablecimientoController : ParentController
    {
        private readonly IEstablecimientoPlantillaBl _establecimientoPlantillaBl;
        private readonly IPlantillaBl _plantillaBl;
        private readonly IEstablecimientoBl _establecimientoBl;

        public EstablecimientoController(IEstablecimientoPlantillaBl establecimientoPlantillaBl, IPlantillaBl plantillaBl, IEstablecimientoBl establecimientoBl)
        {
            _establecimientoPlantillaBl = establecimientoPlantillaBl;
            _plantillaBl = plantillaBl;
            _establecimientoBl = establecimientoBl;
        }

        public EstablecimientoController() : this(new EstablecimientoPlantillaBl(), new PlantillaBl(), new EstablecimientoBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de mantenimiento de los Establecimientos por plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EstablecimientoPlantilla(int idPlantilla)
        {
            try
            {
                var plantilla = _plantillaBl.GetPlantillaById(idPlantilla);
                var establecimientos = _establecimientoPlantillaBl.GetEstablecimientosByPlantillaId(idPlantilla);

                var model = new EstablecimientoPlantillaViewModels
                {
                    Plantilla = plantilla,
                    Establecimientos = establecimientos
                };

                return View("EstablecimientoPlantilla", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para la presentacion del sistema.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarEstablecimiento(int? page, string search, int idPlantilla)
        {
            var pageNumber = page ?? 1;

            var establecimientos = _establecimientoBl.GetEstablecimientosByNombre(search);
            //Alexander Buchelli - inicio - fecha 7/12/18 -SE CAMBIO EL VALOR PAGE SIZE1 
            var pageOfEstablecimientos = establecimientos.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = search;
            ViewBag.idPlantilla = idPlantilla;

            return PartialView("_AgregarEstablecimiento", pageOfEstablecimientos);
        }
        /// <summary>
        /// Descripción: Controlador para el registro de datos del establecimiento seleccionado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <param name="establecimientos"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GuardarEstablecimiento(int idPlantilla, int[] establecimientos)
        {
            var plantilla = _plantillaBl.GetPlantillaById(idPlantilla);
            var idUsuario = Logueado.idUsuario;

            _establecimientoPlantillaBl.AgregarEstablecimientosPorPlantilla(plantilla, establecimientos, idUsuario);

            return RedirectToAction("EstablecimientoPlantilla", new { idPlantilla });
        }
        /// <summary>
        /// Descripción: Controlador para el soft delete de datos del establecimiento seleccionado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="plantillaEstablecimiento"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarEstablecimiento(PlantillaEstablecimiento plantillaEstablecimiento)
        {
            plantillaEstablecimiento.IdUsuarioEdicion = Logueado.idUsuario;
            plantillaEstablecimiento.Estado = 0;

            try
            {
                _establecimientoPlantillaBl.UpdateEstablecimientoByPlantilla(plantillaEstablecimiento);

                return RedirectToAction("EstablecimientoPlantilla", new { plantillaEstablecimiento.IdPlantilla });
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
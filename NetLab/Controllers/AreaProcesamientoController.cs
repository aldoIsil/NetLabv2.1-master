using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.AreaProcesamiento;
using BusinessLayer.Interfaces;
using Model;
using PagedList;
using NetLab.Models;
using System.Collections;
using Newtonsoft.Json;

namespace NetLab.Controllers
{
    public class AreaProcesamientoController : ParentController
    {
        private readonly IAreaProcesamientoBl _areaProcesamientoBl;
        private readonly IAreaExamenBl _areaExamenBl;
        private readonly IExamenBl _examenBl;

        public AreaProcesamientoController(IAreaProcesamientoBl areaProcesamientoBl, IAreaExamenBl areaExamenBl, IExamenBl examenBl)
        {
            _areaProcesamientoBl = areaProcesamientoBl;
            _areaExamenBl = areaExamenBl;
            _examenBl = examenBl;
        }

        public AreaProcesamientoController() : this(new AreaProcesamientoBl(), new AreaExamenBl(), new ExamenBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para la busqueda de areas de procesamiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int? page, string search)
        {
            var pageNumber = page ?? 1;
            var searchCriteria = search ?? string.Empty;

            var area = _areaProcesamientoBl.GetAreasProcesamiento(searchCriteria);
            var areas = _areaProcesamientoBl.GetAreasProcesamiento("");
            var pageOfAreas = area.ToPagedList(pageNumber, GetSetting<int>(PageSize));
            
            ArrayList nombres = new ArrayList();
            foreach (var item in areas)
            {
                nombres.Add(item.Nombre.ToString().ToUpper());
            }
            
            ViewBag.nombresLista = JsonConvert.SerializeObject(nombres);
            
            ViewBag.search = searchCriteria;

            return View(pageOfAreas);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de ingreso de una nueva area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevaArea(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_NuevaArea");
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de edicion de un area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarArea(int id, int? page, string search)
        {
            var areaProcesamiento = _areaProcesamientoBl.GetAreaProcesamientoById(id);

            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_EditarArea", areaProcesamiento);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de eliminacion de un area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarArea(int id, int? page, string search)
        {
            try
            {
                var areaProcesamiento = _areaProcesamientoBl.GetAreaProcesamientoById(id);

                areaProcesamiento.IdUsuarioEdicion = Logueado.idUsuario;
                areaProcesamiento.Estado = 0;

                _areaProcesamientoBl.UpdateAreaProcesamiento(areaProcesamiento);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de agregar un nuevo examen al area de procesamiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarExamenes(int id)
        {
            try
            {
                var areaProcesamiento = _areaProcesamientoBl.GetAreaProcesamientoById(id);

                var examenes = _areaExamenBl.GetExamenesByArea(areaProcesamiento.IdAreaProcesamiento);

                var model = new AreaExamenViewModels
                {
                    AreaProcesamiento = areaProcesamiento,
                    Examenes = examenes
                };

                return View("AreaExamen", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para ejecutar el registro de una nueva area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="areaProcesamiento"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevaArea(int? page, string search, AreaProcesamiento areaProcesamiento)
        {
            try
            {
                areaProcesamiento.IdUsuarioRegistro = Logueado.idUsuario;

                _areaProcesamientoBl.InsertAreaProcesamiento(areaProcesamiento);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para ejecutar la edicion de una nueva area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="areaProcesamiento"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarArea(int? page, string search, AreaProcesamiento areaProcesamiento)
        {
            try
            {
                areaProcesamiento.IdUsuarioEdicion = Logueado.idUsuario;

                _areaProcesamientoBl.UpdateAreaProcesamiento(areaProcesamiento);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para ejecutar la adicion de un examen a un area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idAreaProcesamiento"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarExamen(int? page, string search, int idAreaProcesamiento)
        {
            var pageNumber = page ?? 1;

            var examenes = _examenBl.GetExamenesByNombre(search);
            //Alexander Buchelli - inicio - fecha 7/12/18 -SE CAMBIO EL VALOR PAGE SIZE1 
            var pageOfExamenes = examenes.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = search;
            ViewBag.idAreaProcesamiento = idAreaProcesamiento;

            return PartialView("_AgregarExamen", pageOfExamenes);
        }
        /// <summary>
        /// Descripción: Controlador para ejecutar la adicion de un examen a un area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="idAreaProcesamiento"></param>
        /// <param name="examenes"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AgregarExamen(int? page, int idAreaProcesamiento, string[] examenes)
        {
            var area = _areaProcesamientoBl.GetAreaProcesamientoById(idAreaProcesamiento);
            var idUsuario = Logueado.idUsuario;

            _areaExamenBl.InsertExamenesByArea(area, examenes, idUsuario);

            var pageNumber = page ?? 1;

            var examen = _examenBl.GetExamenesByNombre("");
            //Alexander Buchelli - inicio - fecha 7/12/18 -SE CAMBIO EL VALOR PAGE SIZE1 
            var pageOfExamenes = examen.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = "";
            ViewBag.idAreaProcesamiento = idAreaProcesamiento;

            return PartialView("_AgregarExamen", pageOfExamenes);
            // return RedirectToAction("AgregarExamenes", new { id = idAreaProcesamiento });
        }
        /// <summary>
        /// Descripción: Controlador para ejecutar la eliminacion de un examen a un area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <param name="idAreaProcesamiento"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarExamen(Guid idExamen, int idAreaProcesamiento)
        {
            var areaExamen = new AreaProcesamientoExamen
            {
                IdAreaProcesamiento = idAreaProcesamiento,
                IdExamen = idExamen,
                IdUsuarioEdicion = Logueado.idUsuario,
                Estado = 0
            };

            try
            {
                _areaExamenBl.UpdateExamenByArea(areaExamen);

                return RedirectToAction("AgregarExamenes", new { id = idAreaProcesamiento });
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
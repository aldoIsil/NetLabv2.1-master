using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using Model;
using PagedList;
using BusinessLayer.Interfaces;
using NetLab.Models;
using System.Collections;
using Newtonsoft.Json;

namespace NetLab.Controllers
{
    public class TipoMuestraController : ParentController
    {
        private readonly ITipoMuestraBl _tipoMuestraBl;
        private readonly ITipoMuestraCriterioRechazoBl _muestraCriterioRechazoBl;
        private readonly ICriterioRechazoBl _criterioRechazoBl;
        

        public TipoMuestraController(ITipoMuestraBl tipoMuestraBl, ITipoMuestraCriterioRechazoBl muestraCriterioRechazoBl, ICriterioRechazoBl criterioRechazoBl)
        {
            _tipoMuestraBl = tipoMuestraBl;
            _muestraCriterioRechazoBl = muestraCriterioRechazoBl;
            _criterioRechazoBl = criterioRechazoBl;
        }

        public TipoMuestraController() : this(new TipoMuestraBl(), new TipoMuestraCriterioRechazoBl(), new CriterioRechazoBl())
        {
        }

        /// <summary>
        /// Descripción: Controlador para la busqueda de Tipos de Muestra
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
            var tipoMuestralist = _tipoMuestraBl.GetTipoMuestras("");
            ArrayList nombres = new ArrayList();
            foreach (var item in tipoMuestralist)
            {
                nombres.Add(item.nombre.ToString().ToUpper());
            }


            ViewBag.nombresLista = JsonConvert.SerializeObject(nombres);




            if ((page == null) && (search == null) && (busqueda == null))

          
            return View();
            else
            {
                var pageNumber = page ?? 1;
                var searchCriteria = search ?? string.Empty;

                var tipoMuestras = _tipoMuestraBl.GetTipoMuestras(searchCriteria);
                var pageOfTipoMuestra = tipoMuestras.ToPagedList(pageNumber, GetSetting<int>(PageSize));

                ViewBag.search = searchCriteria;

                return View(pageOfTipoMuestra);
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de ingreso de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoTipoMuestra(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_NuevoTipoMuestra");
        }
        /// <summary>
        /// Descripción: Controlador para el registro de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="chkActivoRol"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoTipoMuestra(int? page, string search, string chkActivoRol, FormCollection collection)
        {
            try
            {
                var tipomuestra = new TipoMuestra
                {
                    nombre = collection["nombre"],
                    descripcion = collection["descripcion"],
                    nemo = collection["nemo"],
                    Estado = chkActivoRol == "false" ? 0 : 1,
                    IdUsuarioRegistro = Logueado.idUsuario
                };
                
                _tipoMuestraBl.InsertTipoMuestra(tipomuestra);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de edicion de datos.
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
        public ActionResult EditarTipoMuestra(int id, int? page, string search)
        {
            var tipoMuestra = _tipoMuestraBl.GetTipoMuestraById(id);

            ViewBag.page = page;
            ViewBag.search = search;

            var tipoMuestralist = _tipoMuestraBl.GetTipoMuestras("");
            ArrayList nombres = new ArrayList();
            foreach (var item in tipoMuestralist)
            {
                nombres.Add(item.nombre.ToString().ToUpper());
            }

            ArrayList nombre = new ArrayList();
            nombre.Add(tipoMuestra.nombre.ToString().ToUpper());

            ViewBag.nombresLista = JsonConvert.SerializeObject(nombres);

            ViewBag.nombreTipoMuestra1 = tipoMuestra.nombre.ToString().ToUpper();

            ViewBag.nombreTipoMuestra = JsonConvert.SerializeObject(nombre);

            return PartialView("_EditarTipoMuestra", tipoMuestra);
        }
        /// <summary>
        /// Descripción: Controlador para el registro de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="collection"></param>
        /// <param name="chkActivoRol"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarTipoMuestra(int id, int? page, string search, FormCollection collection,bool chkActivoRol)
        {
            int est = 0;
            if (chkActivoRol)
            {
                est = 1;
            }

            try
            {
                var tipomuestra = new TipoMuestra
                {
                    idTipoMuestra = id,
                    nombre = collection["nombre"],
                    descripcion = collection["descripcion"],
                    nemo = collection["nemo"],
                    Estado = est,
                    IdUsuarioEdicion = Logueado.idUsuario
                };

                _tipoMuestraBl.UpdateTipoMuestra(tipomuestra);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para el soft delete del tipo de muestra.
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
        public ActionResult EliminarTipoMuestra(int id, int? page, string search)
        {
            try
            {
                var tipoMuestra = _tipoMuestraBl.GetTipoMuestraById(id);

                tipoMuestra.Estado = 0;

                _tipoMuestraBl.UpdateTipoMuestra(tipoMuestra);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para obtner los criterios de rechazo por tipo de muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CriterioRechazo(int id)
        {
            try
            {
                var tipoMuestra = _tipoMuestraBl.GetTipoMuestraById(id);

                var criterios = _muestraCriterioRechazoBl.GetCriteriosByTipoMuestraId(id);

                var model = new MuestraCriterioRechazoViewModels
                {
                    TipoMuestra = tipoMuestra,
                    Criterios = criterios
                };

                return View("TipoMuestraCriterioRechazo", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para agregar nuevos criterios de rechazo por tipo de muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarCriterio(int? page, string search, int idTipoMuestra)
        {
            var pageNumber = page ?? 1;

            var criterios = _criterioRechazoBl.GetCriteriosActivos(search?.Trim()).Where(c => c.EstadoCheck);
            //Alexander Buchelli - inicio - fecha 7/12/18 -SE CAMBIO EL VALOR PAGE SIZE1 
            var pageOfCriterios = criterios.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = search;
            ViewBag.idTipoMuestra = idTipoMuestra;

            return PartialView("_AgregarCriterio", pageOfCriterios);
        }
        /// <summary>
        /// Descripción: Controlador para agregar nuevos criterios de rechazo por tipo de muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="idTipoMuestra"></param>
        /// <param name="criterios"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AgregarCriterio(int? page,int idTipoMuestra, int[] criterios)
        {
            var tipoMuestra = _tipoMuestraBl.GetTipoMuestraById(idTipoMuestra);
            var idUsuario = Logueado.idUsuario;

            _muestraCriterioRechazoBl.AgregarCriteriosPorMuestra(tipoMuestra, criterios, idUsuario);

            var pageNumber = page ?? 1;
          

            var criterio = _criterioRechazoBl.GetCriteriosActivos(""?.Trim()).Where(c => c.EstadoCheck);
            //Alexander Buchelli - inicio - fecha 7/12/18 -SE CAMBIO EL VALOR PAGE SIZE1 
            var pageOfCriterios = criterio.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = "";
            ViewBag.idTipoMuestra = idTipoMuestra;

            return PartialView("_AgregarCriterio", pageOfCriterios);
          
        }
        /// <summary>
        /// Descripción: Controlador para quitar/deshabilitar criterios de rechazo por tipo de muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="muestraCriterioRechazo"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarCriterio(TipoMuestraCriterioRechazo muestraCriterioRechazo)
        {
            muestraCriterioRechazo.IdUsuarioEdicion = Logueado.idUsuario;
            muestraCriterioRechazo.Estado = 0;

            try
            {
                _muestraCriterioRechazoBl.UpdateCriterioByTipoMuestra(muestraCriterioRechazo);

                return RedirectToAction("CriterioRechazo", new { id = muestraCriterioRechazo.IdTipoMuestra });
            }
            catch
            {
                return View("Error");
            }
        }

    }
}


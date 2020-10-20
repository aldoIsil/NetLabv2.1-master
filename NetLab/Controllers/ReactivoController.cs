using System.Web.Mvc;
using BusinessLayer;
using Model;
using PagedList;
using BusinessLayer.Interfaces;
using NetLab.Models;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;

namespace NetLab.Controllers
{
    public class ReactivoController : ParentController
    {

        private readonly IReactivoBl _reactivoBl;
        private readonly IReactivoPresentacionBl _reactivoPresentacionBl;
        private readonly IPresentacionBl _presentacionBl;

        public ReactivoController(IReactivoBl reactivoBl, IReactivoPresentacionBl reactivoPresentacionBl, IPresentacionBl presentacionBl)
        {
            _reactivoBl = reactivoBl;
            _reactivoPresentacionBl = reactivoPresentacionBl;
            _presentacionBl = presentacionBl;
        }

        public ReactivoController() : this(new ReactivoBl(), new ReactivoPresentacionBl(), new PresentacionBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para la mostrar la pantalla inicial y de busqueda.
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

            var reactivosList = _reactivoBl.GetReactivos("");
            ArrayList nombres = new ArrayList();
            foreach (var item in reactivosList)
            {
                nombres.Add(item.glosa.ToString().ToUpper());
            }


            ViewBag.nombresLista = JsonConvert.SerializeObject(nombres);
            
            var mensaje = 0;

            if ((page == null) && (search == null) && (busqueda == null))
                return View();
            else
            {
                var pageNumber = page ?? 1;
                var searchCriteria = search ?? string.Empty;

                var reactivos = _reactivoBl.GetReactivos(searchCriteria);
                var pageOfReactivo = reactivos.ToPagedList(pageNumber, GetSetting<int>(PageSize));

                if (reactivos.Count == 0)
                {
                    mensaje = 1;
                    ViewBag.mensaje = mensaje;
                }
                else
                {
                    mensaje = 2;
                    ViewBag.mensaje = mensaje;

                }

                ViewBag.search = searchCriteria;

                return View(pageOfReactivo);
            }
        }
        ///Descripción: Controlador para mostrar la pantalla de ingreso de un nuevo reactivo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.       
        [HttpGet]
        public ActionResult NuevoReactivo(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("NuevoReactivo");
        }
        /// <summary>
        /// Descripción: Controlador para el registro de un reactivo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoReactivo(int? page, string search, FormCollection collection)
        {

            var reactivo = new Reactivo();

            reactivo.glosa = collection["glosa"];
            reactivo.Estado = 1;
            reactivo.IdUsuarioRegistro = Logueado.idUsuario;

            var reactivoBl = new ReactivoBl();
            reactivoBl.InsertReactivo(reactivo); 

            return RedirectToAction("Index", new { page = page, search = search });

        }
        /// <summary>
        /// Descripción: Controlador para mostrar y editar un reactivo
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
        public ActionResult EditarReactivo(int id, int? page, string search)
        {
            //var presentacionBl = new PresentacionBl();
            //var presenta = presentacionBl.GetPresentacionById(id);
            var reactivo = _reactivoBl.GetReactivoById(id);
            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("EditarReactivo", reactivo);

        }
        /// <summary>
        /// Descripción: Controlador para editar un reactivo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="chkActivoRol"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarReactivo(int id, int? page, string search, string chkActivoRol, FormCollection collection)
        {            
            var reactivo = new Reactivo 
            {
                idReactivo = id,
                glosa = collection["glosa"],
                Estado = chkActivoRol == "false" ? 0 : 1,
                IdUsuarioEdicion = Logueado.idUsuario
            };

            var reactivoBl = new ReactivoBl();
            reactivoBl.UpdateReactivo(reactivo); 

            return RedirectToAction("Index", new { page, search});
            
        }
        /// <summary>
        /// Descripción: Controlador para eliminar un reactivo
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
        public ActionResult EliminarReactivo(int id, int? page, string search)
        {
            try
            {
                var reactivoBl = new ReactivoBl();
                var reactivo = reactivoBl.GetReactivoById(id);

                reactivo.IdUsuarioEdicion = Logueado.idUsuario;
                reactivo.Estado = 0;

                reactivoBl.UpdateReactivo(reactivo); 

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la presentacion de un reactivo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Presentacion(int id)
        {
            try
            {
                var reactivo = _reactivoBl.GetReactivoById(id);

                var presentaciones = _reactivoPresentacionBl.GetPresentacionesByReactivoId(id);

                var model = new ReactivoPresentacionViewModels
                {
                    Reactivo = reactivo,
                    Presentaciones = presentaciones
                };

                return View("ReactivoPresentacion", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar y agregar una presentacion a un reactivo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idReactivo"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarPresentacion(int? page,string search ,int idReactivo)
        {
            var pageNumber = page ?? 1;
            
            var presentaciones = _reactivoPresentacionBl.GetPresentacionesActivas(search?.Trim()).Where(c => c.EstadoCheck);
            //var presentaciones = _reactivoPresentacionBl.GetPresentacionesByReactivoId(idReactivo);
            //Alexander Buchelli - inicio - fecha 7/12/18 -SE CAMBIO EL VALOR PAGE SIZE1 
            var pageOfpresentaciones = presentaciones.ToPagedList(pageNumber, GetSetting<int>(PageSize1));
            ViewBag.search = search;
            ViewBag.idReactivo = idReactivo;

            return PartialView("AgregarPresentacion", pageOfpresentaciones);
            
        }
        /// <summary>
        /// Descripción: Controlador para agregar una presentacion a un reactivo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="idReactivo"></param>
        /// <param name="presentaciones"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AgregarPresentacion(int? page, int idReactivo, int[] presentaciones )
        {
            var reactivo = _reactivoBl.GetReactivoById(idReactivo);
            var idUsuario = Logueado.idUsuario;

            _reactivoPresentacionBl.AgregarPresentacionesPorReactivo(reactivo, presentaciones, idUsuario);

            var pageNumber = page ?? 1;

            var presentacion = _reactivoPresentacionBl.GetPresentacionesActivas(""?.Trim()).Where(c => c.EstadoCheck);
            var pageOfpresentaciones = presentacion.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = "";
            ViewBag.idReactivo = idReactivo;
            return PartialView("AgregarPresentacion", pageOfpresentaciones);

            //return RedirectToAction("Presentacion", new { id = idReactivo });
        }
        /// <summary>
        /// Descripción: Controlador para eliminar una presentacion a un reactivo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentacionReactivo"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarPresentacion(PresentacionReactivo presentacionReactivo)
        {
            presentacionReactivo.IdUsuarioEdicion = Logueado.idUsuario;
            presentacionReactivo.Estado = 0;

            try
            {
                _reactivoPresentacionBl.UpdatePresentacionByReactivo(presentacionReactivo); 

                return RedirectToAction("Presentacion", new { id = presentacionReactivo.idReactivo });
            }
            catch
            {
                return View("Error");
            }
        }


    }
}
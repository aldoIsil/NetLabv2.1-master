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
    public class PresentacionController : ParentController
    {

        private readonly IPresentacionBl _presentacionBl;
        private readonly IPresentacionTipoMuestraBl _presentaTipoMuestraBl;
        private readonly ITipoMuestraBl _tipomuestraBl;
                               
        public PresentacionController(IPresentacionBl presentacionBl, IPresentacionTipoMuestraBl presentaTipoMuestraBl, ITipoMuestraBl tipomuestraBl)
        {
            _presentacionBl = presentacionBl;
            _presentaTipoMuestraBl = presentaTipoMuestraBl;
            _tipomuestraBl = tipomuestraBl;
        }

        public PresentacionController() : this(new PresentacionBl(), new PresentacionTipoMuestraBl(), new TipoMuestraBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para la presentacion y busqueda de las presentaciones.
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

            var presentacionesList = _presentacionBl.GetPresentaciones("");
            ArrayList nombres = new ArrayList();
            foreach (var item in presentacionesList)
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

                var presentaciones = _presentacionBl.GetPresentaciones(searchCriteria);
                var pageOfPresentacion = presentaciones.ToPagedList(pageNumber, GetSetting<int>(PageSize));
                
                if (presentaciones.Count==0)
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

                return View(pageOfPresentacion);                             
            }
        }
        /// <summary>
        /// Descripción: Controlador para la abrir la pantalla de ingreso de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoPresentacion(int? page, string search)
        {
            var mensajeNull = 1;
            ViewBag.page = page;
            ViewBag.search = search;
            ViewBag.mensajeNull = mensajeNull;

            return PartialView("_NuevoPresentacion");
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
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoPresentacion(int? page, string search, FormCollection collection)
        {
            try
            {
                var presenta = new Presentacion();

                presenta.glosa = collection["glosa"];
                presenta.Estado = 1;
                presenta.IdUsuarioRegistro = Logueado.idUsuario;

                var presentaBl = new PresentacionBl();

                presentaBl.InsertPresentacion(presenta);

                return RedirectToAction("Index", new { page = page, search = search });
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
        public ActionResult EditarPresentacion(int id, int? page, string search)
        {
            var presentacionBl = new PresentacionBl();
            var presenta = presentacionBl.GetPresentacionById(id);

            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_EditarPresentacion", presenta);
        }
        /// <summary>
        /// Descripción: Controlador para la actualizacion de datos de la presentación.
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
        public ActionResult EditarPresentacion(int id, int? page, string search, string chkActivoRol, FormCollection collection)
        {
           /* try
            {*/
                var presenta = new Presentacion
                {
                    idPresentacion = id,
                    glosa = collection["glosa"],
                    Estado = chkActivoRol == "false" ? 0 : 1,
                    IdUsuarioEdicion = Logueado.idUsuario
                };

                var presentacionBl = new PresentacionBl();
                presentacionBl.UpdatePresentacion(presenta);

                return RedirectToAction("Index", new { page = page, search = search });
            /*}
            catch
            {
                return View("Error");
            }*/
        }
        /// <summary>
        /// Descripción: Controlador para la eliminación de datos de la presentación.
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
        public ActionResult EliminarPresentacion(int id, int? page, string search)
        {
            try
            {
                var presentacionBl = new PresentacionBl();
                var presenta = presentacionBl.GetPresentacionById(id);

                presenta.IdUsuarioEdicion = Logueado.idUsuario;
                presenta.Estado = 0;

                presentacionBl.UpdatePresentacion(presenta);

                return RedirectToAction("Index", new { page = page, search = search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla que agrega un tipo de muestra a la presetación
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TipoMuestra (int id)
        {
            try
            {
                var presentacion = _presentacionBl.GetPresentacionById(id);

                var tipomuestras = _presentaTipoMuestraBl.GetTipoMuestrasByPresentacionId(id);

                var model = new PresentacionTipoMuestraViewModels
                {
                    Presentacion = presentacion,
                    Tipomuestras = tipomuestras
                };

                return View("PresentacionTipoMuestra", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para agregar un tipo de muestra a la presetación
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idPresentacion"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarTipoMuestra (int? page, string search, int idPresentacion)
        {
            var pageNumber = page ?? 1;

            var tipoMuestras = _tipomuestraBl.GetTipoMuestrasActivas(search?.Trim()).Where(c => c.EstadoCheck);
            //Alexander Buchelli - inicio - fecha 7/12/18 -SE CAMBIO EL VALOR PAGE SIZE1 
            var pageOftipoMuestras = tipoMuestras.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = search;
            ViewBag.idPresentacion = idPresentacion;

            return PartialView("AgregarTipoMuestra", pageOftipoMuestras);
        }
        /// <summary>
        /// Descripción: Controlador para agregar un tipo de muestra a la presetación
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="idPresentacion"></param>
        /// <param name="tipomuestras"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AgregarTipoMuestra(int?page,int idPresentacion, int[] tipomuestras)
        {
            var presentacion = _presentacionBl.GetPresentacionById(idPresentacion);
            var idUsuario = Logueado.idUsuario;

            _presentaTipoMuestraBl.AgregarTipoMuestrasPorPresentacion(presentacion,tipomuestras,idUsuario);


            var pageNumber = page ?? 1;

            var tipoMuestras = _tipomuestraBl.GetTipoMuestrasActivas(""?.Trim()).Where(c => c.EstadoCheck);
            var pageOftipoMuestras = tipoMuestras.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = "";
            ViewBag.idPresentacion = idPresentacion;


            return PartialView("AgregarTipoMuestra", pageOftipoMuestras);

            //return RedirectToAction("TipoMuestra", new { id = idPresentacion });
        }
        /// <summary>
        /// Descripción: Controlador para eliminar un tipo de muestra a la presetación
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentaTipoMuestra"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarTipoMuestra (PresentacionTipoMuestra presentaTipoMuestra)
        {
            presentaTipoMuestra.IdUsuarioEdicion = Logueado.idUsuario;
            presentaTipoMuestra.Estado = 0;

            try
            {
                _presentaTipoMuestraBl.UpdateTipoMuestraByPresentacion(presentaTipoMuestra);

                return RedirectToAction("TipoMuestra", new { id = presentaTipoMuestra.IdPresentacion});
            }
            catch
            {
                return View("Error");
            }
        }
    }
}

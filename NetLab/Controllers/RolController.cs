using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Interfaces;
using Model;
using NetLab.Models.Rol;
using PagedList;
using NetLab.Models;
using System;
using System.Collections;
using Newtonsoft.Json;

namespace NetLab.Controllers
{
    public class RolController : ParentController
    {
        private readonly IRolBl _rolBl;
        private readonly IRolMenuBl _rolMenuBl;
        private readonly IMenuBl _menuBl;

        public RolController(IRolBl rolBl, IRolMenuBl rolMenuBl, IMenuBl menuBl)
        {
            _rolBl = rolBl;
            _rolMenuBl = rolMenuBl;
            _menuBl = menuBl;
        }

        public RolController() : this(new RolBl(), new RolMenuBl(), new MenuBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador que carga y confgirua los campos para el mantenimiento de los Roles.
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

            var roles = _rolBl.GetRoles(search?.Trim());


            ArrayList nombreRoles = new ArrayList();

            foreach (var item in roles)
            {
                nombreRoles.Add(item.nombre.ToUpper().Trim());
            }

            ViewBag.nombreRoles= JsonConvert.SerializeObject(nombreRoles);

            if (roles == null) return View();

            var pageOfRoles = roles.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            return View(pageOfRoles);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de Registro de Roles.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoRol(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;

            var model = new RolViewModels
            {
                Rol = new Rol()
            };

            return PartialView("_NuevoRol", model);
        }
        /// <summary>
        /// Descripción: Controlador que realiza el registro de un nuevo rol.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoRol(int? page, string search, RolViewModels model)
        {
            try
            {
                var rol = new Rol
                {
                    nombre = model.Rol.nombre,
                    descripcion = model.Rol.descripcion,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _rolBl.InsertRol(rol);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador que muestra la pantalla para la edicion de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarRol(int id, int? page, string search)
        {
            var rol = _rolBl.GetRolById(id);

            var model = new RolViewModels
            {
                Rol = rol
            };

            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_EditarRol", model);
        }
        /// <summary>
        /// Descripción: Controlador que ejecuta la actualizacion de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="chkActivoRol"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarRol(int id, int? page, string search, string chkActivoRol, RolViewModels model)
        {
            try
            {
                var rol = new Rol
                {
                    idRol = id,
                    nombre = model.Rol.nombre,
                    descripcion = model.Rol.descripcion,
                    Estado = chkActivoRol == "false" ? 0 : 1,
                    IdUsuarioEdicion = Logueado.idUsuario
                };

                _rolBl.UpdateRol(rol);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador que realiza un Soft Delete de un Rol.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarRol(int id, int? page, string search)
        {
            try
            {
                var rol = _rolBl.GetRolById(id);

                rol.IdUsuarioEdicion = Logueado.idUsuario;
                rol.Estado = 0;

                _rolBl.UpdateRol(rol);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }

        #region MenuOpciones
        /// <summary>
        /// Descripción: Controlador que obtiene informacion de los menus por rol.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Menues(int id)
        {
            try
            {
                var rol = _rolBl.GetRolById(id);

                var menues = _rolMenuBl.GetMenuByRolId(id);

                var model = new RolMenuViewModels
                {
                    Rol = rol,
                    Menues = menues
                };

                return View("RolMenu", model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador que muestra la lista de menus para la seleccion correspondiente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idRol"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarMenu(int? page, string search, int idRol)
        {
            var pageNumber = page ?? 1;

            var menues = _menuBl.GetMenues(search?.Trim());
            //Alexander Buchelli - inicio - fecha 7/12/18 -SE CAMBIO EL VALOR PAGE SIZE1 
            var pageOfMenues = menues.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = search;
            ViewBag.idRol = idRol;

            return PartialView("_AgregarMenu", pageOfMenues);

        }
        /// <summary>
        /// Descripción: Registra información de los menues seleccionados para un rol.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idRol"></param>
        /// <param name="menues"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GuardarMenu(int idRol, int[] menues)
        {
            var rol = _rolBl.GetRolById(idRol);
            var idUsuario = Logueado.idUsuario;

            _rolMenuBl.AgregarMenuPorRol(rol, menues, idUsuario);

            return RedirectToAction("Menues", new { id = idRol });
        }
        /// <summary>
        /// Descripción: Controlador que realiza un sof delete de los menus de un rol.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="rolMenu"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarMenu(RolMenu rolMenu)
        {
            rolMenu.IdUsuarioEdicion = Logueado.idUsuario;
            rolMenu.Estado = 0;

            try
            {
                _rolMenuBl.UpdateMenuByRol(rolMenu);

                return RedirectToAction("Menues", new { id = rolMenu.IdRol });
            }
            catch
            {
                return View("Error");
            }
        }

        #endregion

    }
}
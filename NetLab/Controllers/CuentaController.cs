using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class CuentaController : ParentController
    {
        // GET: Cuenta
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Descripción: Controlador para realizar el cambio de password.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public ActionResult CambiarClave()
        {
            ViewBag.notFound = false;
            ViewBag.isPost = false;

            if (Request["txtActual"] != null)
            {
                ViewBag.isPost = true;

                string clave = Request["txtActual"];
                string userClave = (string) Session["LoginUserPassword"];
                if (clave == userClave) {
                    string nuevaClave = Request["txtNuevo"];
                    string confirmarClave = Request["txtConfirmar"];
                    if (nuevaClave == confirmarClave)
                    {
                        using (var dal = new UsuarioDal())
                        {
                            dal.ChangePassword(Logueado.idUsuario, clave, nuevaClave);
                        }
                    } else { ViewBag.notFound = true; }
                } else
                {
                    ViewBag.notFound = true;
                }
                
                
            }
            return View();
        }
    }
}
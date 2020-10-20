using System;
using DataLayer;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Mvc;
using Framework.WebCommon;
using Model;

namespace NetLab.Controllers
{
    [Serializable()]
    public class ParentController : Controller
    {
        public Usuario Logueado;
        protected Establecimiento EstablecimientoSeleccionado;
        protected Laboratorio LaboratorioSeleccionado;

        protected Establecimiento EstablecimientoUsuario;
        protected List<Establecimiento> EstablecimientosUsuario;
        protected const string PageSize = "pageSize"; //TODO: Move it out of Parent controller
        //Alexander Buchelli - inicio - fecha 7/12/18 - SE AGREGO UN NUEVO VALOR PAGESIZE PARA LAS VENTANAS MODALES CON TABLAS SIN CABECERA
        protected const string PageSize1 = "pageSize1"; //TODO: Move it out of Parent controller
        protected List<Examen> ExamenesByUsuario;
        /// <summary>
        /// Descripción: Controlador base para validar el usuario y el establecimiento del login
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (
                (
                requestContext.RouteData.Values["controller"].ToString().ToLower() != "comun"
                &&
                (requestContext.RouteData.Values["action"].ToString().ToLower() != "obtenerestadoreniec"
                || requestContext.RouteData.Values["action"].ToString().ToLower() != "desactivarreniec"
                || requestContext.RouteData.Values["action"].ToString().ToLower() != "activarreniec"
                || requestContext.RouteData.Values["action"].ToString().ToLower() != "tabletypeprueba"
                || requestContext.RouteData.Values["action"].ToString().ToLower() != "eliminardatosexamenporusuario"
                || requestContext.RouteData.Values["action"].ToString().ToLower() != "limpiartodosdatosexamenes"
                )
                )
                ||
                (
                requestContext.RouteData.Values["controller"].ToString().ToLower() == "comun"
                && requestContext.RouteData.Values["action"].ToString().ToLower() != "obtenerestadoreniec"
                && requestContext.RouteData.Values["action"].ToString().ToLower() != "desactivarreniec"
                && requestContext.RouteData.Values["action"].ToString().ToLower() != "activarreniec"
                && requestContext.RouteData.Values["action"].ToString().ToLower() != "eliminardatosexamenporusuario"
                && requestContext.RouteData.Values["action"].ToString().ToLower() != "limpiartodosdatosexamenes"
                || requestContext.RouteData.Values["action"].ToString().ToLower() != "tabletypeprueba"
                )
                //&&
                //(
                //    requestContext.RouteData.Values["controller"].ToString().ToLower() != "reporteresultados"
                //    &&
                //    requestContext.RouteData.Values["action"].ToString().ToLower() != "descargaresultados"
                //)
                )
            {

                var isLogin = false;
                Logueado = Session["UsuarioLogin"] as Usuario;

                if (Logueado != null)
                {
                    isLogin = true;
                    EstablecimientosUsuario = Session["EstablecimientosLogin"] as List<Establecimiento>;
                    ViewBag.nombreUsuario = Logueado.nombres + " " + Logueado.apellidoPaterno + " " + Logueado.apellidoMaterno;
                    ViewBag.establecimientosUsuario = EstablecimientosUsuario;
                }
                else
                {
                    RedirectToAction("Index", "Login");
                }

                var permisos = new List<Menu>();
                var nombreLocal = "";

                if (isLogin)
                {
                    if (Session["menuData"] == null)
                    {
                        var dal = new UsuarioDal();
                        permisos = dal.getMenuUsuario(Logueado.idUsuario);

                        Session["menuData"] = permisos;
                    }

                    if (Session["establecimientoSeleccionado"] != null)
                    {
                        EstablecimientoSeleccionado = Session["establecimientoSeleccionado"] as Establecimiento;

                        if (EstablecimientoSeleccionado != null)
                            nombreLocal = EstablecimientoSeleccionado.Nombre;
                    }

                    if (Session["selLaboratorio"] != null)
                    {
                        LaboratorioSeleccionado = Session["selLaboratorio"] as Laboratorio;

                        if (LaboratorioSeleccionado != null)
                            nombreLocal = LaboratorioSeleccionado.Nombre;
                    }
                    //Examenes Usuario
                    if (Session["ExamenesUsuario"] != null)
                    {
                        ExamenesByUsuario = (List<Model.Examen>)Session["ExamenesUsuario"];
                    }
                    //
                    permisos = Session["menuData"] as List<Menu>;
                }

                ViewBag.establecimientoSeleccionado = EstablecimientoSeleccionado;
                ViewBag.laboratorioSeleccionado = LaboratorioSeleccionado;
                ViewBag.nombreLocal = nombreLocal;


                ViewBag.isLogin = isLogin;
                ViewBag.permisos = permisos;

                //Examenes Usuario
                ViewBag.ExamenesByUsuario = ExamenesByUsuario;
                //
            }
        }
        /// <summary>
        /// Descripción: Metodo que redireciona a la pagina de Login, antes indica que el tiempo de login venció.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (request.Url == null)
                throw new InvalidOperationException();

            var url = new UrlHelper(request.RequestContext);

            var absolutePath = request.Url.AbsolutePath;
            var path = absolutePath.EndsWith("/") ? absolutePath : absolutePath + "/";

            if ((filterContext.Controller.ValueProvider.GetValue("controller").AttemptedValue.ToLower() != "comun"
                &&
                (filterContext.Controller.ValueProvider.GetValue("action").AttemptedValue.ToLower() != "obtenerestadoreniec"
                || filterContext.Controller.ValueProvider.GetValue("action").AttemptedValue.ToLower() != "desactivarreniec"
                || filterContext.Controller.ValueProvider.GetValue("action").AttemptedValue.ToLower() != "activarreniec"
                || filterContext.Controller.ValueProvider.GetValue("action").AttemptedValue.ToLower() != "eliminardatosexamenporusuario"
                || filterContext.Controller.ValueProvider.GetValue("action").AttemptedValue.ToLower() != "limpiartodosdatosexamenes"
                || filterContext.Controller.ValueProvider.GetValue("action").AttemptedValue.ToLower() != "tabletypeprueba")
                ))
                //&&
                //(filterContext.Controller.ValueProvider.GetValue("controller").AttemptedValue.ToLower() != "reporteresultados")
                //&& (filterContext.Controller.ValueProvider.GetValue("action").AttemptedValue.ToLower() != "descargaresultados")
                //)
            {
                if (Session["UsuarioLogin"] == null)
                {
                    if (path == url.Action("Index", "Home"))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Login" }));
                    }
                    else
                    {
                        if (request.IsAjaxRequest())
                        {
                            filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;

                            filterContext.Result = new JsonResult
                            {
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                                Data = new
                                {
                                    Url = url.Action("Timeout", "Login")
                                }
                            };
                        }
                        else
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Timeout", controller = "Login" }));
                        }
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }

        //TODO: Move this out too?
        protected static T GetSetting<T>(string settingName)
        {
            return WebConfig.GetSetting<T>(settingName);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            
        }
    }
}
using DataLayer;
using Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using Model.Enums;
using NetLab.App_Code;
using System;
using NetLab.Extensions.Request;
using Utilitario;

namespace NetLab.Controllers
{
    public class LoginController : Controller
    {
        /// <summary>
        /// Descripción: Controlador para inicializar el login.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Modificacion: : 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index()
        {
            //throw new NullReferenceException("Null reference en objeto X");
            ////Enviar Resultados MINSA EQHALI
            //try
            //{
            //    var resultadoBl = new ResultadosBl();
            //    //new NetLab.Extensions.Request.SendRquest().EnviarResultados(idOrden.ToString());
            //    string idOrden = "83EA7471-60D8-4909-A1EC-67FC872349CA";
            //    var data = resultadoBl.ObtenerResultadoCovidPorOrden(Guid.Parse(idOrden));
            //    var enviocorrecto = new SendRquest().EnviarResultadosCovid(data);
            //    if (!string.IsNullOrWhiteSpace(enviocorrecto))
            //    {
            //        resultadoBl.InsertarResultadoCovidFallido(data, 72, enviocorrecto);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //throw;
            //}

            var loginFault = 0;

            if (Session["loginFault"] != null)
            {
                loginFault = (int)Session["loginFault"];
                Session["loginFault"] = 0;
            }

            if (Session["UsuarioLogin"] != null)
            {
                return RedirectToAction("Index", "Home");

            }

            ViewBag.loginFault = loginFault;
            ViewBag.isLogin = false;

            return View();
        }
        /// <summary>
        /// Descripcion: Metodo ejecutar el cambio de constraseña para el usuairo.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult OlvidoContrasena()
        {
            var mensajePassword = 0;

            if (Session["mensajePassword"] != null)
            {
                mensajePassword = (int)Session["mensajePassword"];
                Session["mensajePassword"] = 0;
            }

            ViewBag.mensajePassword = mensajePassword;

            if (Session["UsuarioLogin"] != null)
            {
                return RedirectToAction("Index", "Home");

            }

            #region
            string usuario = Request["txtlogin"];
            string correo = Request["txtcorreo"];

            //using (var dal = new UsuarioDal())

            //{

            //    if (usuario == null && correo == null)
            //    {
            //        Session["mensajePassword"] = 1;
            //    }

            //    if (usuario != null)
            //    {
            //        string rpta = dal.ActualizarPasword(1, usuario);

            //        if (rpta == "1")
            //        {
            //            Session["mensajePassword"] = 2;
            //        }
            //        else
            //        {

            //            Session["mensajePassword"] = 3;
            //        }

            //    }
            //    if (correo != null)
            //    {
            //        string rpta = dal.ActualizarPasword(2, correo);

            //        if (rpta == "1")
            //        {
            //            Session["mensajePassword"] = 4;
            //        }
            //        else
            //        {

            //            Session["mensajePassword"] = 5;
            //        }
            //    }
            //}

            #endregion

            return View();
        }
        /// <summary>
        /// Descripcion: Metodo para direccionar a la ventan de creacion de una nueva contraseña
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OlvidoContrasena(string login, string email)
        {
            var mensajePassword = 0;

            if (Session["mensajePassword"] != null)
            {


                mensajePassword = (int)Session["mensajePassword"];
                Session["mensajePassword"] = 0;
            }


            ViewBag.mensajePassword = mensajePassword;


            if (Session["UsuarioLogin"] != null)
            {
                return RedirectToAction("Index", "Home");

            }

            string usuario = Request["txtlogin"];
            string correo = Request["txtcorreo"];


            
            using (var dal = new UsuarioDal())

            {
                string asunto = "Datos de acceso - Netlab 2.0";
                EnvioCorreo ec = new EnvioCorreo();

                if (usuario == null && correo == null)
                {
                    Session["mensajePassword"] = 1;
                }

                if (usuario != null)
                {
                    var rpta = dal.ActualizarPasword(1, usuario);

                    if (rpta.condicionLaboral == "1")
                    {
                        Session["mensajePassword"] = 2;
                    }
                    else
                    {
                        ec.EnviarCorreo(rpta.correo, asunto, rpta.condicionLaboral);
                        Session["mensajePassword"] = 3;
                    }

                }
                if (correo != null)
                {
                    var rpta = dal.ActualizarPasword(2, correo);

                    if (rpta.condicionLaboral == "1")
                    {
                        Session["mensajePassword"] = 4;
                    }
                    else
                    {
                        ec.EnviarCorreo(correo, asunto, rpta.condicionLaboral);
                        Session["mensajePassword"] = 5;
                    }
                }


            }


            return RedirectToAction("OlvidoContrasena", "Login");
        }
        /// <summary>
        /// Descripcion: Metodo para direccionar a la ventan de creacion de una nueva contraseña
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult NuevaContrasena()
        {

            var mensajePasswordnew = 0;

            if (Session["mensajePasswordnew"] != null)
            {


                mensajePasswordnew = (int)Session["mensajePasswordnew"];
                Session["mensajePasswordnew"] = 0;
            }


            ViewBag.mensajePasswordnew = mensajePasswordnew;


            if (Session["UsuarioLogin"] != null)
            {
                return RedirectToAction("Index", "Home");

            }

            return View();
        }
        /// <summary>
        /// Descripcion: Metodo para la creacion de una nueva contraseña
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevaContrasena(string id, string password)
        {


            var mensajePasswordnew = 0;

            if (Session["mensajePasswordnew"] != null)
            {


                mensajePasswordnew = (int)Session["mensajePasswordnew"];
                Session["mensajePasswordnew"] = 0;
            }


            ViewBag.mensajePasswordnew = mensajePasswordnew;


            if (Session["UsuarioLogin"] != null)
            {
                return RedirectToAction("Index", "Home");

            }


            string clave = Request["txtPassword"];
            string confirmpassword = Request["txtConfirmPassword"];
            int usuario = int.Parse(Session["idusuario"].ToString());

            using (var dal = new UsuarioDal())

            {
                if (clave != confirmpassword)
                {
                    Session["mensajePasswordnew"] = 1;
                    //return RedirectToAction("CambiarPassword", "Login");
                }
                else
                {
                    int valor = dal.ActualizarPaswordUsuarioNuevo(usuario, clave);

                    if (valor == 1)
                    {
                        Session["mensajePasswordnew"] = 2;
                        // return RedirectToAction("CambiarPassword", "Login");
                    }
                    else
                    {
                        Session["mensajePasswordnew"] = 3;
                        //return RedirectToAction("CambiarPassword", "Login");
                    }


                }

            }

            return RedirectToAction("NuevaContrasena", "Login");





        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Index", "Login");
        }
        /// <summary>
        /// Descripcion: Contenedor de la lógica para que el login tenga la validación correcta.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            var usuario = Request["txtUser"];
            var clave = Request["txtClave"];

            using (var dal = new UsuarioDal())
            {
                var remoteAddr = Request.ServerVariables["REMOTE_ADDR"];
                var userHostAddress = Request.UserHostAddress;
                var forwardedfor = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                new bsPage().LogInfoLogin(usuario, remoteAddr, userHostAddress, forwardedfor);
                //uno de estos 3 valores va a pasar como nuevo parametro para el metodo LoginUsuario(usuario, clave, [nuevo])
                var user = dal.LoginUsuario(usuario, clave, remoteAddr);
                if (user != null)
                {

                    var respuesta = int.Parse(Session["respuesta"].ToString());

                    if (respuesta == 1)
                    {
                        Session["loginFault"] = 1;
                    }
                    else if (respuesta == 2)
                    {
                        Session["loginFault"] = 2;
                        Session["idusuario"] = user.idUsuario;
                        return RedirectToAction("NuevaContrasena", "Login");
                    }
                    else if (respuesta == 3)
                    {
                        Session["loginFault"] = 3;

                    }
                    else if (respuesta == 4)
                    {
                        Session["loginFault"] = 4;
                    }
                    else if (respuesta == 5)
                    {
                        Session["loginFault"] = 5;
                    }
                    else if (respuesta == 6)
                    {
                        //int dias = (int)(user.fechaCaducidad.Date - System.DateTime.Now.Date).TotalDays;
                        //if (dias == 10 || dias == 5)
                        //{
                        //    EnvioCorreo correo = new EnvioCorreo();
                        //    string asunto = "Caducidad cuenta de usuario Netlab v2";
                        //    string contenido = "Estimado usuario, en " + dias + " su cuenta caducará, envíe el formulario de renovación. \nSi ya envió su solicitud no tome en cuenta este mensaje";
                        //    correo.EnviarCorreo(user.correo, asunto, contenido);
                        //}

                        var laboratorios = dal.getLaboratoriosUsuario(user.idUsuario);

                        var local = new Local
                        {
                            IdUsuario = user.idUsuario,
                            TipoLocal = TipoLocal.Institucion
                        };

                        var localBl = new LocalBl();
                        var institucion = localBl.GetInstituciones(local);


                        Session["LaboratoriosLogin"] = laboratorios;
                        Session["UsuarioLogin"] = user;
                        Session["LoginUserPassword"] = clave;
                        Session["Instituciones"] = institucion;

                        if(institucion.Count == 1)
                        {
                            string defaultInstitucion = institucion.First().IdInstitucion;
                            local = new Local
                            {
                                IdInstitucion = int.Parse(defaultInstitucion),
                                IdUsuario = user.idUsuario
                            };

                            // Examenes Usuario
                            try
                            {
                                var ExamenesUsuario = new UsuarioEnfermedadExamenBl().GetExamenByUsuarioId(user.idUsuario).ToList();
                                Session["ExamenesUsuario"] = ExamenesUsuario;
                            }
                            catch (System.Exception ex)
                            {
                                Session["ExamenesUsuario"] = null;
                            }

                            var bl = new LocalBl();
                            var disas = bl.GetDisas(local);

                            if (disas.Count == 1)
                            {
                                var idDisa = disas.First().IdDISA;

                                var redes = GetRedByDisa(defaultInstitucion, idDisa, user.idUsuario);

                                if (redes.Count == 1)
                                {
                                    var idRed = redes.First().IdRed;

                                    var microredes = GetMicroRedByRed(defaultInstitucion, idDisa, idRed + idDisa, user.idUsuario);

                                    if (microredes.Count == 1)
                                    {
                                        var idMicroRed = microredes.First().IdMicroRed;

                                        var establecimientos = GetEstablecimientoByMicroRed(defaultInstitucion, idDisa, idRed + idDisa, idMicroRed + idRed + idDisa, user.idUsuario);
                                        if(establecimientos.Any() && establecimientos.Count == 1)
                                        {
                                            Session["EstablecimientosLogin"] = establecimientos;
                                            return SetSelectLocal(defaultInstitucion, idDisa, idRed + idDisa, idMicroRed + idRed + idDisa, establecimientos.First().IdEstablecimiento.ToString(), true, true);
                                        }
                                        else
                                        {
                                            //return SetSelectLocal(defaultInstitucion, idDisa, idRed + idDisa, idMicroRed + idRed + idDisa, establecimientos.First().IdEstablecimiento.ToString(), true);
                                            return SelectMicroRed(defaultInstitucion, idDisa, idRed + idDisa, idMicroRed + idRed + idDisa, user.idUsuario);
                                        }
                                    }

                                    SetMicroRedView(microredes);
                                }

                                SetRedView(redes);
                            }

                        }

                        return RedirectToAction("SelectLocal", "Login");
                    }
                }

                if (string.IsNullOrWhiteSpace(usuario)) Session["loginFault"] = 7;

                if (!string.IsNullOrWhiteSpace(usuario) && string.IsNullOrWhiteSpace(clave)) Session["loginFault"] = 8;

                return RedirectToAction("Index", "Login");
            }
        }

        [AllowAnonymous]
        public ActionResult Timeout()
        {
            ViewBag.isLogin = false;

            return View("SessionTimeout");
        }
        /// <summary>
        /// Descripcion: Genera la accion para retornar a la vista SetLocal con toda la informacion obtenida de los establecimientos,red,microred y la instituciones.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectLocal()
        {
            var user = Session["UsuarioLogin"] as Usuario;

            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var laboratorios = Session["LaboratoriosLogin"] as List<Laboratorio>;

            var local = new Local
            {
                IdUsuario = user.idUsuario,
                TipoLocal = TipoLocal.Institucion
            };

            var localBl = new LocalBl();
            var institucion = localBl.GetInstituciones(local);

            Session["Instituciones"] = institucion;
            var instituciones = (List<Institucion>)Session["Instituciones"];

            instituciones?.Insert(0, new Institucion
            {
                IdInstitucion = "0",
                nombreInstitucion = "Seleccione un Sub Sector",
                codigoInstitucion = 0
            });

            ViewBag.laboratorios = laboratorios;
            ViewBag.instituciones = instituciones;

            ViewBag.isLogin = false;

            ViewBag.Locales = new Local
            {
                IdInstitucion = 0,
                IdEstablecimiento = 0,
                IdRed = "0",
                IdDisa = "0",
                IdMicroRed = "0"
            };

            return View();
        }
        /// <summary>
        /// Descripcion: Obtiene y setea la información de la institucion, disa, red, microred y los establecimientos
        /// salvando la informacion en  variables de session.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="hdnEstablecimiento"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SetSelectLocal(string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, string hdnEstablecimiento, bool setfromlogin = false, bool solounestb = false)
        {
            if (string.IsNullOrEmpty(hdnEstablecimiento) || hdnEstablecimiento == "0")
            {
                ViewBag.instituciones = Session["Instituciones"];
                ViewBag.disas = Session["disas"];
                ViewBag.redes = Session["redes"];
                ViewBag.microredes = Session["microredes"];
                ViewBag.establecimientos = Session["EstablecimientosLogin"];
                ViewBag.seleccionFault = 1;

                ViewBag.Locales = new Local
                {
                    IdInstitucion = int.Parse(hdnInstitucion),
                    IdDisa = hdnDisa,
                    IdRed = hdnRed,
                    IdMicroRed = hdnMicroRed
                };

                return View("SelectLocal");
            }

            var establecimientos = Session["EstablecimientosLogin"] as List<Establecimiento>;
            var laboratorios = Session["LaboratoriosLogin"] as List<Laboratorio>;
            var instituciones = Session["Instituciones"] as List<Institucion>;

            if (Request["selInstituciones"] != null && instituciones != null)
            {
                var idInst = int.Parse(Request["selInstituciones"]);
                foreach (var es in instituciones)
                {
                    if (idInst == es.codigoInstitucion)
                    {
                        Session["institucionSeleccionada"] = es;
                    }
                }
            }

            if (setfromlogin)
            {
                if (solounestb)
                {
                    var estb = establecimientos.First(x => x.IdEstablecimiento == int.Parse(hdnEstablecimiento));
                    Session["establecimientoSeleccionado"] = estb;
                }
                else
                {
                    //
                }
            }
            else
            {
                if (Request["selEstablecimiento"] != null && establecimientos != null)
                {
                    var idEst = int.Parse(Request["selEstablecimiento"]);
                    foreach (var es in establecimientos)
                    {
                        if (idEst == es.IdEstablecimiento)
                        {
                            Session["establecimientoSeleccionado"] = es;
                        }
                    }
                }
            }

            if (Request["selLaboratorio"] != null && laboratorios != null)
            {
                var idLab = int.Parse(Request["selLaboratorio"]);
                foreach (var lab in laboratorios)
                {
                    if (idLab == lab.IdLaboratorio)
                    {
                        Session["selLaboratorio"] = lab;
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Descripcion: Retorna la informacion de las insttituciones,disa,red,microres y los Estaboecimientos dependiendo del dato seleccionado.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="hdnTipo"></param>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="hdnEstablecimiento"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectMain(string hdnTipo, string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed,
            string hdnEstablecimiento)
        {
            var user = Session["UsuarioLogin"] as Usuario;

            if (user == null)
                return View("SelectLocal");

            switch (hdnTipo)
            {
                case "Instituciones":
                    return SelectInstituciones(hdnInstitucion, user.idUsuario);
                case "Disa":
                    return SelectDisa(hdnInstitucion, hdnDisa, user.idUsuario);
                case "Red":
                    return SelectRed(hdnInstitucion, hdnDisa, hdnRed, user.idUsuario);
                case "MicroRed":
                    return SelectMicroRed(hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, user.idUsuario);
                case "Establecimientos":
                    return SetSelectLocal(hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, hdnEstablecimiento);
            }

            return View("SelectLocal");
        }
        /// <summary>
        /// Descripción: Devuelve el dato de las instituciones y si los demas datos
        /// como disa,red,microred y establecimientos retornan un solo valor se selecciona automaticamente.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectInstituciones(string hdnInstitucion, int idUsuario)
        {
            ViewBag.Locales = new Local { IdInstitucion = int.Parse(hdnInstitucion) };

            ViewBag.instituciones = Session["Instituciones"] as List<Institucion>;

            var local = new Local
            {
                IdInstitucion = int.Parse(hdnInstitucion),
                IdUsuario = idUsuario
            };

            var bl = new LocalBl();
            var disas = bl.GetDisas(local);

            if (disas.Count == 1)
            {
                var idDisa = disas.First().IdDISA;

                var redes = GetRedByDisa(hdnInstitucion, idDisa, idUsuario);

                if (redes.Count == 1)
                {
                    var idRed = redes.First().IdRed;

                    var microredes = GetMicroRedByRed(hdnInstitucion, idDisa, idRed + idDisa, idUsuario);

                    if (microredes.Count == 1)
                    {
                        var idMicroRed = microredes.First().IdMicroRed;

                        var establecimientos = GetEstablecimientoByMicroRed(hdnInstitucion, idDisa, idRed + idDisa, idMicroRed + idRed + idDisa, idUsuario);
                        SetEstablecimientoView(establecimientos);
                    }

                    SetMicroRedView(microredes);
                }

                SetRedView(redes);
            }

            SetDisaView(disas);
            // Examenes Usuario
            try
            {
                var ExamenesUsuario = new UsuarioEnfermedadExamenBl().GetExamenByUsuarioId(idUsuario).ToList();
                Session["ExamenesUsuario"] = ExamenesUsuario;
            }
            catch (System.Exception ex)
            {
                Session["ExamenesUsuario"] = null;
            }            
            //
            return View("SelectLocal");
        }
        /// <summary>
        /// Descripción: Devuelve el dato de las Red de acuerod a la institucion y a la Disa seleccionada.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectDisa(string hdnInstitucion, string hdnDisa, int idUsuario)
        {
            var redes = GetRedByDisa(hdnInstitucion, hdnDisa, idUsuario);

            SetRedView(redes);

            return View("SelectLocal");
        }
        /// <summary>
        /// Descripción: Devuelve el dato de la MicroRed de acuerdo a la institucion, a la Disa y Red seleccionada.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectRed(string hdnInstitucion, string hdnDisa, string hdnRed, int idUsuario)
        {
            var microRedes = GetMicroRedByRed(hdnInstitucion, hdnDisa, hdnRed, idUsuario);

            SetMicroRedView(microRedes);

            return View("SelectLocal");
        }
        /// <summary>
        /// Descripción: Devuelve el dato del Establecimiento de acuerdo a la institucion, a la Disa y Red seleccionada.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectMicroRed(string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, int idUsuario)
        {
            var establecimientos = GetEstablecimientoByMicroRed(hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, idUsuario);

            SetEstablecimientoView(establecimientos);

            return View("SelectLocal");
        }
        /// <summary>
        /// Descripción: Devuelve el dato del Establecimiento de acuerdo a la institucion, a la Disa y Red seleccionada.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private List<Red> GetRedByDisa(string idInstitucion, string idDisa, int idUsuario)
        {
            ViewBag.Locales = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdDisa = idDisa
            };

            ViewBag.instituciones = Session["Instituciones"] as List<Institucion>;
            ViewBag.disas = Session["disas"] as List<DISA>;

            var local = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdUsuario = idUsuario,
                IdDisa = idDisa
            };

            var bl = new LocalBl();
            return bl.GetRedes(local);
        }
        /// <summary>
        /// Descripción: Devuelve el dato de la MicroRed de acuerdo a la institucion, a la Disa y Red seleccionada.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private List<MicroRed> GetMicroRedByRed(string idInstitucion, string idDisa, string idRed, int idUsuario)
        {
            ViewBag.Locales = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdDisa = idDisa,
                IdRed = idRed
            };

            ViewBag.instituciones = Session["Instituciones"];
            ViewBag.disas = Session["disas"];
            ViewBag.redes = Session["redes"];

            var local = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdUsuario = idUsuario,
                IdDisa = idDisa,
                IdRed = idRed.Length < 2 ? idRed : idRed.Substring(0, 2)
            };

            var bl = new LocalBl();
            return bl.GetMicroRedes(local);
        }
        /// <summary>
        /// Descripción: Devuelve el dato del Establecimiento de acuerdo a la institucion, a la Disa , Red  y microred seleccionada.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicroRed"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private List<Establecimiento> GetEstablecimientoByMicroRed(string idInstitucion, string idDisa, string idRed, string idMicroRed, int idUsuario)
        {
            ViewBag.Locales = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdDisa = idDisa,
                IdRed = idRed,
                IdMicroRed = idMicroRed
            };

            ViewBag.instituciones = Session["Instituciones"];
            ViewBag.disas = Session["disas"];
            ViewBag.redes = Session["redes"];
            ViewBag.microredes = Session["microredes"];

            var local = new Local
            {
                IdInstitucion = int.Parse(idInstitucion),
                IdUsuario = idUsuario,
                IdDisa = idDisa,
                IdRed = idRed.Length < 2 ? idRed : idRed.Substring(0, 2),
                IdMicroRed = idMicroRed.Length < 2 ? idMicroRed : idMicroRed.Substring(0, 2)
            };

            var bl = new LocalBl();
            return bl.GetEstablecimientos(local);
        }
        /// <summary>
        /// Descripción: Devuelve la disa concatenado con la descripcion.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="disas"></param>
        private void SetDisaView(IList<DISA> disas)
        {
            Session["disas"] = disas;
            disas.Insert(0, new DISA { IdDISA = "-", NombreDISA = "Seleccione un elemento" });
            ViewBag.disas = disas;
        }
        /// <summary>
        /// Descripción: Devuelve la red concatenado con la descripcion.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="redes"></param>
        private void SetRedView(IList<Red> redes)
        {
            Session["redes"] = redes;
            redes.Insert(0, new Red { IdRed = "-", NombreRed = "Seleccione una Red" });
            ViewBag.redes = redes;
        }
        /// <summary>
        /// Descripción: Devuelve la microred concatenado con la descripcion.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="microRedes"></param>
        private void SetMicroRedView(IList<MicroRed> microRedes)
        {
            Session["microredes"] = microRedes;
            microRedes.Insert(0, new MicroRed { IdMicroRed = "-", NombreMicroRed = "Seleccione una Micro Red" });
            ViewBag.microredes = microRedes;
        }
        /// <summary>
        /// Descripción: Devuelve los establecimientos concatenado con la descripcion.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios.
        /// </summary>
        /// <param name="establecimientos"></param>
        private void SetEstablecimientoView(IList<Establecimiento> establecimientos)
        {
            //NO SE ESTA USANDO
            //var loginEstablecimientoList = LoginEstablecimientoList(establecimientos);
            //Session["loginEstablecimientoList"] = loginEstablecimientoList;
            Session["EstablecimientosLogin"] = establecimientos;
            establecimientos.Insert(0,
                new Establecimiento { IdEstablecimiento = 0, Nombre = "Seleccione un Establecimiento", CodigoUnico = "00000" });
            ViewBag.establecimientos = establecimientos;
        }
        /// <summary>
        /// Descripción: Devuelve los establecimientos concatenado con la descripcion.
        /// Author: Terceros.
        /// Fecha Creación : 01/01/2017
        /// Modificacion: : Se agregaron comentarios. 
        /// </summary>
        /// <param name="establecimientos"></param>
        /// <returns></returns>
        private static List<Establecimiento> LoginEstablecimientoList(IEnumerable<Establecimiento> establecimientos)
        {
            var cacheEstablecimientos = StaticCache.GetEstablecimientoCache();

            if (cacheEstablecimientos == null)
                return new List<Establecimiento>();

            return cacheEstablecimientos.Where(x => establecimientos.Any(y => y.CodigoUnico == x.CodigoUnico)).ToList();
        }
        [AllowAnonymous]
        public ActionResult Contactar()
        {
            return View("Contactar");
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los establecimientos para listarlos.
        /// Author: Jose Chavez
        /// Fecha Creación : 15/06/2019
        /// </summary>
        /// <param name="Prefix"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult GetEstablecimientosNew(string Prefix)
        {
            return Json(new EstablecimientoBl().GetEstablecimientosByTextoBusqueda(Prefix, 0).FindAll(p => p.IdLabIns != 1).ToList(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Descripción: Registra datos para la sugerencia y notifica a traves de un email.
        /// Author: Jose Chavez
        /// Fecha Creación : 15/06/2019
        /// </summary>
        /// <param name="JsContactar"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public string SendMessage(Contactar JsContactar)
        {
            try
            {
                new ContactarBl().InsertQueja(JsContactar);
                //Send Email
                new EnvioCorreo().EnviarCorreo(JsContactar.email, "Sugerencias NetLabv2", JsContactar.mensaje,"soportenetlabv2@gmail.com");
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
            return "Mensaje Enviado";
        }
    }
}
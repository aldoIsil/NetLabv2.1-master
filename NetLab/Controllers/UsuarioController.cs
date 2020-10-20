using System;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using Model;
using PagedList;
using BusinessLayer.Interfaces;
using NetLab.Models;
using System.Web;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using NetLab.Models.Shared;
using BusinessLayer.AreaProcesamiento;
using NetLab.Controllers.FormConverter;
using NetLab.Controllers.FormConverter.Entities;
using System.Collections.Generic;
using Rotativa;
using Rotativa.Options;

namespace NetLab.Controllers
{
    [Serializable()]
    public class UsuarioController : ParentController
    {
        private const string ContentJpg = "image/jpg";
        private readonly IUsuarioBl _usuarioBl;
        private readonly IRolBl _rolBl;
        private readonly IUsuarioRolBl _usuarioRolBl;
        private readonly IAreaProcesamientoBl _areaProcesamientoBl;
        private readonly IUsuarioAreaProcesamientoBl _usuarioAreaProcesamientoBl;
        private readonly IEstablecimientoBl _establecimientoBl;
        private readonly IInstitucionBl _institucionBl;

        public UsuarioController(IRolBl rolBl, IUsuarioRolBl usuarioRolBl, IUsuarioBl usuarioBl, IAreaProcesamientoBl areaProcesamientoBl, IUsuarioAreaProcesamientoBl usuarioAreaProcesamientoBl, IEstablecimientoBl establecimientoBl, IInstitucionBl institucionBl)
        {
            _rolBl = rolBl;
            _usuarioRolBl = usuarioRolBl;
            _usuarioBl = usuarioBl;
            _areaProcesamientoBl = areaProcesamientoBl;
            _usuarioAreaProcesamientoBl = usuarioAreaProcesamientoBl;
            _establecimientoBl = establecimientoBl;
            _institucionBl = institucionBl;
        }

        public UsuarioController() : this(new RolBl(), new UsuarioRolBl(), new UsuarioBl(), new AreaProcesamientoBl(),
            new UsuarioAreaProcesamientoBl(), new EstablecimientoBl(), new InstitucionBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para dar mantenimiento a los usuarios.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="login"></param>
        /// <param name="nombres"></param>
        /// <param name="apellidoMaterno"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="dni"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int? page, string login, string nombres, string apellidoMaterno,
            string apellidoPaterno, string dni)
        {

            if ((page == null) && (login == null) && (nombres == null) && (apellidoMaterno == null) && (apellidoPaterno == null) && (dni == null))
                return View();

            var pageNumber = page ?? 1;
            var loginCriteria = string.IsNullOrEmpty(login) ? string.Empty : login.Trim();
            var nombresCriteria = string.IsNullOrEmpty(nombres) ? string.Empty : nombres.Trim();
            var apellidoMaternoCriteria = string.IsNullOrEmpty(apellidoMaterno)
                ? string.Empty
                : apellidoMaterno.Trim();
            var apellidoPaternoCriteria = string.IsNullOrEmpty(apellidoPaterno)
                ? string.Empty
                : apellidoPaterno.Trim();
            var dniCriteria = dni ?? string.Empty;

            var usuarioBl = new UsuarioBl();

            var usuarios = usuarioBl.GetUsuarios(loginCriteria, apellidoMaternoCriteria, apellidoPaternoCriteria, nombresCriteria, dniCriteria, Logueado.tipo == 3 ? 1 : 0);

            if (usuarios == null) return View();

            var filtered =
                usuarios.Where(
                    ex =>
                        ex.login.ToUpper().Contains(loginCriteria.ToUpper()) &
                        ex.nombres.ToUpper().Contains(nombresCriteria.ToUpper()) &
                        ex.apellidoMaterno.ToUpper().Contains(apellidoMaternoCriteria.ToUpper()) &
                        ex.apellidoPaterno.ToUpper().Contains(apellidoPaternoCriteria.ToUpper()) &
                        ex.documentoIdentidad.ToUpper().Contains(dniCriteria.ToUpper()));
            var pageOfUsuarios = filtered.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.login = loginCriteria;
            ViewBag.nombres = nombresCriteria;
            ViewBag.apellidoMaterno = apellidoMaternoCriteria;
            ViewBag.apellidoPaterno = apellidoPaternoCriteria;
            ViewBag.dni = dniCriteria;
            return View(pageOfUsuarios);
        }
        /// <summary>
        /// Descripción: Controlador para ingresar datos de un nuevo usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MostrarAgregarUsuario(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;

            var listaBl = new ListaBl();
            var tiposUsuarios = listaBl.GetListaByOpcion(OpcionLista.TipoUsuario);
            var profesiones = listaBl.GetListaByOpcion(OpcionLista.Profesion);
            var componente = listaBl.GetListaByOpcion(OpcionLista.Componente);
            var acceso = listaBl.GetListaByOpcion(OpcionLista.Acceso);
            var nAprobacion = listaBl.GetListaByOpcion(OpcionLista.nAprobacion);

            var usuariovm = new UsuarioViewModels
            {
                Usuario = new Usuario
                {
                    fechaIngreso = DateTime.Now,
                    fechaCaducidad = DateTime.Now,
                    estatus = 0
                },
                tipoUsuarioLogeado = Logueado.tipo,
                TiposUsuario = new ListaDetalleViewModels { Data = tiposUsuarios.ListaDetalle },
                Profesion = new ListaDetalleViewModels { Data = profesiones.ListaDetalle },
                Componente = new ListaDetalleViewModels { Data = componente.ListaDetalle },
                Acceso = new ListaDetalleViewModels { Data = acceso.ListaDetalle },
                NivelAprobacion = new ListaDetalleViewModels { Data = nAprobacion.ListaDetalle }
            };

            return PartialView("_PartialAgregarUsuario", usuariovm);
        }

        private static byte[] GetBytes(HttpPostedFileBase file)
        {
            if (!(file?.ContentLength > 0)) return null;

            var imgBinaryData = new byte[file.ContentLength];
            file.InputStream.Read(imgBinaryData, 0, file.ContentLength);

            return imgBinaryData;
        }
        /// <summary>
        /// Descripción: Controlador para ejecutar el registro de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <param name="nombres"></param>
        /// <param name="login"></param>
        /// <param name="codigoColegio"></param>
        /// <param name="rne"></param>
        /// <param name="correo"></param>
        /// <param name="cargo"></param>
        /// <param name="tiempoCaducidad"></param>
        /// <param name="telefono"></param>
        /// <param name="tipo"></param>
        /// <param name="profesion"></param>
        /// <param name="estatusUsuario"></param>
        /// <param name="firmaU"></param>
        /// <returns></returns>
        [HttpPost]
        public string NuevoUsuario(int? page, string search, string apellidoPaterno, string apellidoMaterno,
            string nombres, string login, string codigoColegio, string rne, string correo, string cargo, string tiempoCaducidad,
            string telefono, string tipo, string profesion, string estatusUsuario, string imgfirma, string hddDato)
        //, HttpPostedFileBase firmaU)
        {
            var usuarioBl = new UsuarioBl();

            var existeDni = usuarioBl.ExisteDni(Request.Params["txtDni"]);

            if (existeDni) return "-1";

            var idNuevoUsuario = 0;
            var usuario = new Usuario();
            var existeLogin = usuarioBl.ExisteLogin(login, 0);
            var Rpta = "";

            //int Establecimiento = (hddDato=="")?0:(Convert.ToInt16(hddDato));
            
            if (existeLogin == 0)
            {
                //var firmab = GetBytes(firmaU2);
                usuario.apellidoMaterno = apellidoMaterno;
                usuario.apellidoPaterno = apellidoPaterno;
                usuario.nombres = nombres;
                usuario.login = login;
                usuario.codigoColegio = codigoColegio;
                usuario.RNE = rne;
                usuario.correo = correo;
                usuario.cargo = cargo;
                usuario.tiempoCaducidad = int.Parse(tiempoCaducidad);
                usuario.documentoIdentidad = Request.Params["txtDni"];
                usuario.telefono = telefono;
                usuario.tipo = int.Parse(tipo);
                usuario.profesion = int.Parse(profesion);
                usuario.estatus = int.Parse(estatusUsuario);
                usuario.firmaDigital = null;
                usuario.IdUsuarioRegistro = Logueado.idUsuario;
                usuario.estado = 1;
                //usuario.Establecimiento = Establecimiento;

                Rpta = usuarioBl.InsertUsuario(usuario);
                if (Rpta.Length == 1)
                {
                    idNuevoUsuario = int.Parse(Rpta);
                }

                //ViewBag.MsjError = Rpta;
            }

            var result = existeLogin.ToString();
            if (idNuevoUsuario == 0)
                result = "-1";

            return result;
        }

        public ActionResult NuevoUsuario2(int? page, string search, string apellidoPaterno, string apellidoMaterno,
            string nombres, string login, string codigoColegio, string profesion, string rne, string correo, string cargo,
            string tiempoCaducidad, string telefono, string tipo, string componente, string acceso, string nAprobacion,
            string estatusUsuario, UsuarioViewModels vmusuario, string documentoIdentidad)
        {            
            var usuarioBl = new UsuarioBl();

            var existeDni = usuarioBl.ExisteDni(Request.Params["txtDni"]);

            if (existeDni) return View("Index");

            var idNuevoUsuario = 0;
            var usuario = new Usuario();
            var existeLogin = usuarioBl.ExisteLogin(login, 0);

            //if (!(firma?.ContentLength > 0)) return null;

            //var imgBinaryData = new byte[firma.ContentLength];
            //firma.InputStream.Read(imgBinaryData, 0, firma.ContentLength);

            if (existeLogin == 0)
            {
                //var firma = GetBytes("");

                usuario.apellidoMaterno = apellidoMaterno;
                usuario.apellidoPaterno = apellidoPaterno;
                usuario.nombres = nombres;
                usuario.login = login;
                usuario.codigoColegio = codigoColegio;
                usuario.RNE = rne;
                usuario.correo = correo;
                usuario.cargo = cargo;
                usuario.tiempoCaducidad = int.Parse(tiempoCaducidad);
                usuario.documentoIdentidad = Request.Params["txtDni"] ==null ? documentoIdentidad : Request.Params["txtDni"];
                usuario.telefono = telefono;
                usuario.tipo = int.Parse(tipo);
                usuario.profesion = int.Parse(profesion);
                usuario.estatus = int.Parse(estatusUsuario);
                usuario.firmaDigital = null;
                usuario.IdUsuarioRegistro = Logueado.idUsuario;
                usuario.estado = 1;
                usuario.componente = int.Parse(componente);
                usuario.idTipoUsuario = int.Parse(acceso);
                usuario.nAprobacion = int.Parse(nAprobacion);

                var contenido = usuarioBl.InsertUsuario(usuario);
                if (contenido.Length > 1)
                {
                    idNuevoUsuario = 1;

                    //UsuarioBl user = new UsuarioBl();
                    EnvioCorreo send = new EnvioCorreo();
                    string asunto = "Datos de acceso - Netlab 2.0";
                    //string contenido = Rpta;//user.GetBodyCorreo(usuario);
                    send.EnviarCorreo(usuario.correo, asunto, contenido);
                }
                
                //ViewBag.MsjError = Rpta;
            }

            var result = existeLogin.ToString();

            if (idNuevoUsuario == 0)
                result = "-1";

            return PartialView("Index");
        }


        /// <summary>
        /// Descripción: Controlador para mostrar los datos del usuario y editarlos.
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
        public ActionResult EditarUsuario(int id, int? page, string search)
        {
            var usuarioBl = new UsuarioBl();
            var listaBl = new ListaBl();
            var usuario = usuarioBl.GetUsuarioById(id);

            var tiposUsuariosEdit = listaBl.GetListaByOpcion(OpcionLista.TipoUsuario);
            var profesionesEdit = listaBl.GetListaByOpcion(OpcionLista.Profesion);
            var componente = listaBl.GetListaByOpcion(OpcionLista.Componente);
            var acceso = listaBl.GetListaByOpcion(OpcionLista.Acceso);
            var nAprobacion = listaBl.GetListaByOpcion(OpcionLista.nAprobacion);

            var usuariovm = new UsuarioViewModels
            {
                Usuario = usuario,
                tipoUsuarioLogeado = Logueado.tipo,
                TiposUsuario = new ListaDetalleViewModels { Data = tiposUsuariosEdit.ListaDetalle },
                Profesion = new ListaDetalleViewModels { Data = profesionesEdit.ListaDetalle },
                Componente = new ListaDetalleViewModels { Data = componente.ListaDetalle },
                Acceso = new ListaDetalleViewModels { Data = acceso.ListaDetalle },
                NivelAprobacion = new ListaDetalleViewModels { Data = nAprobacion.ListaDetalle }
            };

            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_PartialEditarUsuario", usuariovm);
        }
        /// <summary>
        /// Descripción: Controlador para ejecutar la edicion de datos de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        ///               Se realizo la referencia faltante del  IdUsuarioEdicion = Logueado.idUsuario
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <param name="nombres"></param>
        /// <param name="login"></param>
        /// <param name="codigoColegio"></param>
        /// <param name="rne"></param>
        /// <param name="correo"></param>
        /// <param name="cargo"></param>
        /// <param name="tiempoCaducidad"></param>
        /// <param name="telefono"></param>
        /// <param name="tipo"></param>
        /// <param name="profesion"></param>
        /// <param name="firmaEdit"></param>
        /// <param name="chkActivo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarUsuario(int id, int? page, string apellidoPaterno, string apellidoMaterno,
            string nombres, string login, string codigoColegio, string profesion, string rne, string correo, string cargo,
            string tiempoCaducidad, string telefono, string tipo, string componente, string acceso, string nAprobacion,
            HttpPostedFileBase firmaEdit, string chkActivo, string chkActivoR)
        {
            var usuarioEditBl = new UsuarioBl();
            var existeLogin = usuarioEditBl.ExisteLogin(login, 1);

            if (existeLogin != 0) return RedirectToAction("Index", new { page, search = "" });

            var firma = GetBytes(firmaEdit);

            var usuario = new Usuario
            {
                idUsuario = Request.Params["idUsuario"] == null ? id : int.Parse(Request.Params["idUsuario"]),
                apellidoMaterno = apellidoMaterno,
                apellidoPaterno = apellidoPaterno,
                nombres = nombres,
                login = login,
                codigoColegio = codigoColegio,
                RNE = rne,
                correo = correo,
                cargo = cargo,
                tiempoCaducidad = int.Parse(tiempoCaducidad),
                //documentoIdentidad = Request.Params["txtDni"],
                telefono = telefono,
                tipo = int.Parse(tipo),
                profesion = int.Parse(profesion),
                firmaDigital = firma,
                estado = (chkActivo == "false" || chkActivo == null) ? 0 : 1,
                Renovacion = (chkActivoR == "false" || chkActivoR == null) ? 0 : 1,
                IdUsuarioEdicion = Logueado.idUsuario,
                componente = int.Parse(componente),
                idTipoUsuario = int.Parse(acceso),
                nAprobacion = int.Parse(nAprobacion)
            };

            var usuarioBl = new UsuarioBl();
            usuarioBl.UpdateUsuario(usuario);

            if (usuario.Renovacion == 1)
            {
                string asunto = "Datos de acceso - Netlab 2.0";
                string contenido = "";
                contenido += "Estimado(a) usuario:\n";
                contenido += "Se renovó su cuenta de usuario, favor de comprobarlo ingresando al sistema.\n";
                contenido += "Saludos cordiales";
                EnvioCorreo ec = new EnvioCorreo();
                ec.EnviarCorreo(usuario.correo, asunto, contenido);
            }
            return RedirectToAction("Index", new { page, search = "" });
        }


        #region "Combos Establecimientos"
        /// <summary>
        /// Descripción: Obtiene las Disas que pertenecen a una institucion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <returns></returns>
        public string GetDisa(int idInstitucion)
        {
            var data = Request.Params["data[q]"];

            var local = new Local
            {
                IdInstitucion = idInstitucion,
                IdUsuario = Logueado.idUsuario
            };

            var bl = new LocalBl();
            var disaList = bl.GetDisas(local);
            disaList.Insert(0, new DISA { IdDISA = "0", NombreDISA = "Seleccione la Disa" });

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";
            var existeDisa = false;

            foreach (var establecimiento in disaList)
            {
                resultado += "{\"id\":\"" + establecimiento.IdDISA + "\",\"text\":\"" + establecimiento.NombreDISA + "\"},";
                existeDisa = true;
            }

            if (existeDisa)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }
        /// <summary>
        /// Descripción: Obtiene las Redes que pertenecen a una institucion y una disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public string GetRed(int idInstitucion, int idDisa)
        {
            var data = Request.Params["data[q]"];

            var local = new Local
            {
                IdInstitucion = idInstitucion,
                IdUsuario = Logueado.idUsuario,
                IdDisa = idDisa.ToString()
            };

            var bl = new LocalBl();
            var redList = bl.GetRedes(local);
            redList.Insert(0, new Red { IdRed = "0", NombreRed = "Seleccione la Red" });

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";
            var existeDisa = false;

            foreach (var establecimiento in redList)
            {
                resultado += "{\"id\":\"" + establecimiento.IdRed + "\",\"text\":\"" + establecimiento.NombreRed + "\"},";
                existeDisa = true;
            }

            if (existeDisa)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }
        /// <summary>
        /// Descripción: Obtiene las Micro Redes que pertenecen a una institucion, disa y red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public string GetMicrored(int idInstitucion, int idDisa, int idRed)
        {
            var data = Request.Params["data[q]"];

            var local = new Local
            {
                IdInstitucion = idInstitucion,
                IdUsuario = Logueado.idUsuario,
                IdDisa = idDisa.ToString(),
                IdRed = idRed.ToString().PadLeft(2, '0')
            };

            var bl = new LocalBl();
            var microredes = bl.GetMicroRedes(local);
            microredes.Insert(0, new MicroRed { IdMicroRed = "0", NombreMicroRed = "Seleccione la MicroRed" });

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";
            var existeDisa = false;

            foreach (var establecimiento in microredes)
            {
                resultado += "{\"id\":\"" + establecimiento.IdMicroRed + "\",\"text\":\"" + establecimiento.NombreMicroRed + "\"},";
                existeDisa = true;
            }

            if (existeDisa)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos que pertenecen a una institucion, disa, red y una micro red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicrored"></param>
        /// <returns></returns>
        public string GetEstablecimientoByMicrored(int idInstitucion, int idDisa, int idRed, int idMicrored)
        {
            var data = Request.Params["data[q]"];

            var local = new Local
            {
                IdInstitucion = idInstitucion,
                IdUsuario = Logueado.idUsuario,
                IdDisa = idDisa.ToString(),
                IdRed = idRed.ToString().PadLeft(2, '0'),
                IdMicroRed = idMicrored.ToString().PadLeft(2, '0')
            };

            var bl = new LocalBl();
            var loginEstablecimientoList = bl.GetEstablecimientos(local);
            loginEstablecimientoList.Insert(0, new Establecimiento { IdEstablecimiento = 0, Nombre = "Seleccione un Establecimiento", CodigoUnico = "00000" });

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";
            var existeDisa = false;

            foreach (var establecimiento in loginEstablecimientoList)
            {
                resultado += "{\"id\":\"" + establecimiento.IdEstablecimiento + "\",\"text\":\"" + establecimiento.Nombre + "\"},";
                existeDisa = true;
            }

            if (existeDisa)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }

        #endregion
        /// <summary>
        /// Descripción: Obtiene establecimientos que pertenecen a una institucion, disa, red y una micro red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public ActionResult ValidarPersona(string nroDocumento)
        {
            Usuario usuario = new Usuario();
            UsuarioBl usuarioBl = new UsuarioBl();
            usuario = usuarioBl.ValidarDatosUsuario(usuario, nroDocumento);

            ViewBag.MensajeServicio = usuarioBl.ErrorMessage;

            return PartialView("_PartialDatosUsuario", usuario);
        }
        /// <summary>
        /// Descripción: Muestra firma digital.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowFirmaDigital(int id)
        {
            UsuarioBl usuarioBl = new UsuarioBl();
            var usuarioTmp = usuarioBl.GetUsuarioById(id);

            return File(usuarioTmp.firmaDigital, ContentJpg);
        }

        #region "AGREGAR ROLES"
        /// <summary>
        /// Descripción: Controlador, muestra la pantalla para el registro de un rol para el usuario registrado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Roles(int id)
        {
            try
            {
                var usuario = _usuarioBl.GetUsuarioById(id);

                var roles = _usuarioRolBl.GetRolByUsuarioId(id);

                var model = new UsuarioRolViewModels
                {
                    Usuario = usuario,
                    Roles = roles
                };

                return View("UsuarioRol", model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Descripción: Controlador, realiza la busqueda de los roles para agregar al usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarRol(int? page, string search, int idUsuario)
        {
            var pageNumber = page ?? 1;

            var roles = _rolBl.GetRoles(search?.Trim());

            ViewBag.search = search;
            ViewBag.idUsuario = idUsuario;

            if (roles == null) return PartialView("_AgregarRolUsuario");

            var pageOfRoles = roles.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            return PartialView("_AgregarRolUsuario", pageOfRoles);
        }
        /// <summary>
        /// Descripción: Ejecuta el registro del rol para el usuario registrado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GuardarRol(int idUsuario, int[] roles)
        {

            var usuario = _usuarioBl.GetUsuarioById(idUsuario);
            var idUsuarioLogueado = Logueado.idUsuario;

            _usuarioRolBl.AgregarRolPorUsuario(usuario, roles, idUsuarioLogueado);

            return RedirectToAction("Roles", new { id = idUsuario });

        }
        /// <summary>
        /// Descripción: Controlador para la ejecucion de la edicion de los roles del usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuarioRol"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarRol(UsuarioRol usuarioRol)
        {
            usuarioRol.IdUsuarioEdicion = Logueado.idUsuario;
            usuarioRol.Estado = 0;

            try
            {
                _usuarioRolBl.UpdateRolByUsuario(usuarioRol);
                return RedirectToAction("Roles", new { id = usuarioRol.idUsuario });
            }
            catch
            {
                return View("Error");
            }
        }

        #endregion


        #region "AGREGAR AREAS PROCESAMIENTO"
        /// <summary>
        /// Descripción: Muestra ventana con las areas de procesamiento de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AreasProcesamiento(int id)
        {
            try
            {
                var usuario = _usuarioBl.GetUsuarioById(id);
                var areas = _usuarioAreaProcesamientoBl.GetAreaByUsuarioId(id);

                var model = new UsuarioAreaProcesamientoViewModels
                {
                    Usuario = usuario,
                    Areas = areas
                };

                return View("UsuarioAreaProcesamiento", model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador que muestra la ventana para seleccionar las areas de procesamiento para el usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarAreaProcesamiento(int? page, string search, int idUsuario)
        {
            var pageNumber = page ?? 1;
            var areas = _areaProcesamientoBl.GetAreasProcesamiento(search?.Trim()).Where(a => a.EstadoCheck);

            var pageOfAreas = areas.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = search;
            ViewBag.idUsuario = idUsuario;

            return PartialView("_AgregarAreaProcesamiento", pageOfAreas);
        }
        /// <summary>
        /// Descripción: Realiza el registro de las areas seleccionadas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="areas"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GuardarAreaProcesamiento(int idUsuario, int[] areas)
        {
            var usuario = _usuarioBl.GetUsuarioById(idUsuario);
            var idUsuarioLogueado = Logueado.idUsuario;

            _usuarioAreaProcesamientoBl.AgregarAreaPorUsuario(usuario, areas, idUsuarioLogueado);

            return RedirectToAction("AreasProcesamiento", new { id = idUsuario });
        }
        /// <summary>
        /// Descripción: Controlador para realizar Soft Delete de un Area de Procesamiento por usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuarioAreaProcesamiento"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarAreaProcesamiento(UsuarioAreaProcesamiento usuarioAreaProcesamiento)
        {
            usuarioAreaProcesamiento.IdUsuarioEdicion = Logueado.idUsuario;
            usuarioAreaProcesamiento.Estado = 0;

            try
            {
                _usuarioAreaProcesamientoBl.UpdateAreaByUsuario(usuarioAreaProcesamiento);

                return RedirectToAction("AreasProcesamiento", new { id = usuarioAreaProcesamiento.idUsuario });
            }
            catch
            {
                return View("Error");
            }
        }

        #endregion

        /// <summary>
        /// Descripción: Controlador para mostrar la ventana con los establecimientos agregados a un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Establecimientos(int idUsuario, int? page = null)
        {
            try
            {
                ViewBag.UsuarioEstablecimiento = "1";
                var pageNumber = page ?? 1;
                var pageSize = GetSetting<int>(PageSize);
                var startIndex = (pageNumber - 1) * pageSize + 1;
                var usuario = _usuarioBl.GetUsuarioById(idUsuario);
                var totalCount = _usuarioBl.GetTotalCountEstablecimientoByUsuario(idUsuario);
                var establecimientos = _usuarioBl.GetEstablecimientoByUsuario(idUsuario, startIndex, pageSize).ToList();
                var instituciones = _institucionBl.GetInstituciones();

                var model = new UsuarioEstablecimientoViewModels
                {
                    Page = pageNumber,
                    Usuario = usuario,
                    Establecimientos = establecimientos,
                    Instituciones = instituciones,
                    PagingMetaData = new StaticPagedList<UsuarioEstablecimiento>(
                        establecimientos, pageNumber, pageSize, totalCount).GetMetaData()
                };

                return View("UsuarioEstablecimiento", model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para asignar el establecimiento seleccionado para un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AsignarEstablecimiento(AsignarEstablecimientoViewModels model)
        {
            try
            {
                switch (model.Submit)
                {
                    case "Establecimiento":
                        ProcesarEstablecimiento(model);
                        break;
                    case "MicroRed":
                        ProcesarMicroRed(model);
                        break;
                    case "Red":
                        ProcesarRed(model);
                        break;
                    case "Disa":
                        ProcesarDisa(model);
                        break;
                    case "Institucion":
                        ProcesarInstitucion(model);
                        break;
                    case "Delete":
                        EliminarEstablecimientosSeleccionados(model);
                        break;
                    case "DeleteAll":
                        EliminarTodosLosEstablecimientos(model);
                        break;
                    default:
                        throw new ArgumentException();
                }

                return Establecimientos(model.IdUsuario);
            }
            catch (Exception)
            {
                return View("Error");
            }

        }
        /// <summary>
        /// Descripción: Controlador para realizar una eliminación de todos los laboratorios de un usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        private void EliminarTodosLosEstablecimientos(AsignarEstablecimientoViewModels model)
        {
            _establecimientoBl.EliminarUsuarioEstablecimiento(Logueado.idUsuario, model.IdUsuario);
        }
        /// <summary>
        /// Descripción: Controlador para realizar una eliminación de los laboratorios seleccionados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        private void EliminarEstablecimientosSeleccionados(AsignarEstablecimientoViewModels model)
        {
            _establecimientoBl.EliminarEstablecimientos(Logueado.idUsuario, model.IdUsuario, model.ChkEliminar);
        }
        /// <summary>
        /// Descripción: Controlador para realizar el registro de las instituciones seleccionadas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        private void ProcesarInstitucion(AsignarEstablecimientoViewModels model)
        {
            _establecimientoBl.InsertarUsuarioEstablecimientoByInstitucion(
                Logueado.idUsuario,
                model.IdUsuario,
                model.CodigoInstitucion
            );
        }
        /// <summary>
        /// Descripción: Controlador para registrar informacion del establcimiento filtrado por institucion y disa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        private void ProcesarDisa(AsignarEstablecimientoViewModels model)
        {
            _establecimientoBl.InsertarUsuarioEstablecimientoByDisa(
                Logueado.idUsuario,
                model.IdUsuario,
                model.CodigoInstitucion,
                model.CodigoDisa.ToString()
            );
        }
        /// <summary>
        /// Descripción: Controlador para registrar informacion del establcimiento filtrado por institucion, disa y red
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        private void ProcesarRed(AsignarEstablecimientoViewModels model)
        {
            _establecimientoBl.InsertarUsuarioEstablecimientoByRed(
                Logueado.idUsuario,
                model.IdUsuario,
                model.CodigoInstitucion,
                model.CodigoDisa.ToString(),
                model.CodigoRed.ToString().PadLeft(2, '0')
            );
        }
        /// <summary>
        /// Descripción: Controlador para registrar informacion del establcimiento filtrado por institucion, disa, red y mircored
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        private void ProcesarMicroRed(AsignarEstablecimientoViewModels model)
        {
            _establecimientoBl.InsertarUsuarioEstablecimientoByMicroRed(
                Logueado.idUsuario,
                model.IdUsuario,
                model.CodigoInstitucion,
                model.CodigoDisa.ToString(),
                model.CodigoRed.ToString().PadLeft(2, '0'),
                model.CodigoMicroRed.ToString().PadLeft(2, '0')
            );
        }

        /// <summary>
        /// Descripción: Controlador para el registro de los establecimientos de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        private void ProcesarEstablecimiento(AsignarEstablecimientoViewModels model)
        {
            var usuarioEstablecimiento = new UsuarioEstablecimiento
            {
                IdUsuarioRegistro = Logueado.idUsuario,
                idUsuario = model.IdUsuario,
                idInstitucion = model.CodigoInstitucion,
                idDISA = model.CodigoDisa.ToString(),
                idRed = model.CodigoRed.ToString().PadLeft(2, '0'),
                idMicrored = model.CodigoMicroRed.ToString().PadLeft(2, '0'),
                idEstablecimiento = model.CodigoEstablecimiento
            };

            _usuarioBl.InsertUsuarioEstablecimiento(usuarioEstablecimiento);
        }
        /// <summary>
        ///  Descripción: Controlador para el reseteo del password de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="login"></param>
        /// <param name="nombres"></param>
        /// <param name="apellidoMaterno"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="dni"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetearUsuario(int id, int page, string login, string nombres, string apellidoMaterno,
            string apellidoPaterno, string documentoIdentidad, string correo)
        {
            try
            {
                if (documentoIdentidad != null)
                {
                    string contenido = _usuarioBl.ResetearClave(id);
                    var usuario = new Usuario();
                    string asunto = "Datos de acceso - Netlab 2.0";
                    EnvioCorreo ec = new EnvioCorreo();
                    ec.EnviarCorreo(correo, asunto, contenido);
                }
                

                return RedirectToAction("Index", new { page, login, nombres, apellidoMaterno, apellidoPaterno, documentoIdentidad });
            }
            catch (Exception)
            {
                return View("Index");
            }
        }

        
        #region Examenes
        /// <summary>
        /// Descripción: Muestra ventana con las areas de procesamiento de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Examenes(int id)
        {
            try
            {
                var usuario = _usuarioBl.GetUsuarioById(id);
                var examenes = new UsuarioEnfermedadExamenBl().GetExamenByUsuarioId(id);

                var model = new UsuarioEnfermedadExamenViewModels
                {
                    Usuario = usuario,
                    Examenes = examenes
                };

                return View("UsuarioEnfermedadExamen", model);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador que muestra la ventana para seleccionar las areas de procesamiento para el usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarExamen(int? page, string search, int idUsuario, string ddlTipoUsuario)
        {
            var pageNumber = page ?? 1;
            var examenes = new ExamenBl().GetExamenesByNombre(search);
            var pageOfExamenes = examenes.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = search;
            ViewBag.idUsuario = idUsuario;

            return PartialView("_AgregarExamen", pageOfExamenes);
        }
        /// <summary>
        /// Descripción: Realiza el registro de las areas seleccionadas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="areas"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AgregarExamen(int? page, int idUsuario, string[] examenes,string ddlTipoUsuario, string ddlEnfermedad)
        {

            var usuario = _usuarioBl.GetUsuarioById(idUsuario);
            var idUsuarioLogueado = Logueado.idUsuario;
            new UsuarioEnfermedadExamenBl().InsertExamenByUsuario(usuario, examenes, idUsuarioLogueado, int.Parse(ddlTipoUsuario), int.Parse(ddlEnfermedad));

            var pageNumber = page ?? 1;
            var examen = new ExamenBl().GetExamenesByNombre("");

            var pageOfExamenes = examen.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = "";
            ViewBag.idUsuario = idUsuario;
            usuario.idTipoUsuario = int.Parse(ddlTipoUsuario);
            var model = new UsuarioEnfermedadExamenViewModels
            {
                Usuario = usuario,
                Examenes = new UsuarioEnfermedadExamenBl().GetExamenByUsuarioId(idUsuario)
            };
            return View("UsuarioEnfermedadExamen", model);
        }
        /// <summary>
        /// Descripción: Controlador para realizar Soft Delete de un Area de Procesamiento por usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuarioAreaProcesamiento"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarExamen(string idExamen, int idUsuario)
        {
            var ExamenUsuario = new ExamenUsuario()
            {
                idUsuario = idUsuario,
                IdExamen = Guid.Parse(idExamen),
                IdUsuarioEdicion = Logueado.idUsuario
            };
            ViewBag.search = "";
            ViewBag.idUsuario = idUsuario;
            try
            {
                new UsuarioEnfermedadExamenBl().UpdateExamenByUsuario(ExamenUsuario);

                var model = new UsuarioEnfermedadExamenViewModels
                {
                    Usuario = _usuarioBl.GetUsuarioById(idUsuario),
                    Examenes = new UsuarioEnfermedadExamenBl().GetExamenByUsuarioId(idUsuario)
                };


                return View("UsuarioEnfermedadExamen", model);
            }
            catch
            {
                return View("Error");
            }
        }
        #endregion

        #region Gestion Usuarios
        public ActionResult ValidarSolicitudUsuario(string fechaDesde, string fechaHasta, string nroDocumento, string nombre, string apePaterno, string apeMaterno, string estado)
        {
            fechaDesde = (fechaDesde == null) ? Convert.ToString(DateTime.Now.AddDays(-1))  : fechaDesde;
            fechaHasta = (fechaHasta == null) ? Convert.ToString(DateTime.Now) : fechaHasta;
            estado = (estado == "") ? "0" : estado;
            int _estado = Convert.ToInt32(estado);
            SolicitudUsuario usuario = new SolicitudUsuario()
            {
                usuario = new Usuario()
                {
                    documentoIdentidad = nroDocumento,
                    nombres = nombre,
                    apellidoPaterno = apePaterno,
                    apellidoMaterno = apeMaterno,
                    idUsuario = Logueado.idUsuario
                }
            };

            UsuarioBl _usuario = new UsuarioBl();
            List<SolicitudUsuario> user = new List<SolicitudUsuario>();
            user = _usuario.ListaSolicitudUsuario(usuario, fechaDesde, fechaHasta, EstablecimientoSeleccionado.IdEstablecimiento,_estado);

            return View(user);
        }

        [HttpGet]
        public ActionResult GetSolicitudUsuario(int id, int tipo)
        {
            string vista = "";
            vista = (tipo == 1) ? "_SolicitudUsuario" : "_SolicitudUsuarioPrint";
            SolicitudUsuario usuario = new SolicitudUsuario();
            UsuarioBl user = new UsuarioBl();
            usuario = user.GetSolicitudUsuario(id);
            usuario.firmante = (usuario.idUsuarioVal2 != 0) ? true : false;
            usuario.firmante = (usuario.idUsuarioVal1 == Logueado.idUsuario || usuario.idUsuarioVal2 == Logueado.idUsuario) ? true : false;
            Session["usuario"] = usuario;
            return PartialView(vista, usuario);
        }

        public void ValidarSolicitud(int idSolicitudUsuario, string comentario, int estado)
        {
            int valor = 0;
            var usuario = new SolicitudUsuario();
            usuario.usuario = new Usuario();
            usuario.idSolicitudUsuario = idSolicitudUsuario;
            usuario.usuario.idUsuario = Logueado.idUsuario;
            usuario.comentario1 = comentario;
            usuario.estado = estado;
            UsuarioBl user = new UsuarioBl();
            valor = user.ValidarSolicitudUsuario(usuario);
            if (valor == 4) 
            {
                SolicitudUsuario solUs = new SolicitudUsuario();
                solUs = (SolicitudUsuario)Session["usuario"];
                var _user = new Usuario();
                _user = user.ValidarDatosUsuario(_user, solUs.usuario.documentoIdentidad);

                var _usuario = new Usuario();
                _usuario = user.GetUsuarioByDocumento(solUs.usuario.documentoIdentidad);

                if (solUs.tipoSolicitud == 1)
                {
                    solUs.usuario.login = _user.nombres.Substring(0, 1) + _user.apellidoPaterno;

                    NuevoUsuario2(null, null, _user.apellidoPaterno, _user.apellidoMaterno, _user.nombres, solUs.usuario.login, solUs.usuario.codigoColegio, Convert.ToString(solUs.usuario.profesion),
                                solUs.usuario.RNE, solUs.usuario.correo, solUs.usuario.cargo, "6", solUs.usuario.telefono, Convert.ToString(solUs.usuario.idTipoUsuario), Convert.ToString(solUs.usuario.componente),
                                "1", "1", "0", null,solUs.usuario.documentoIdentidad);
                }else
                {
                    //EditarUsuario(_usuario.idUsuario, 1, _usuario.apellidoPaterno, _usuario.apellidoMaterno, _usuario.nombres, _usuario.login, _usuario.codigoColegio, Convert.ToString(_usuario.profesion), _usuario.RNE, _usuario.correo,
                    //            _usuario.cargo, "6", _usuario.telefono, Convert.ToString(_usuario.idTipoUsuario), Convert.ToString(_usuario.componente), Convert.ToString(_usuario.tipo), Convert.ToString(_usuario.nAprobacion), null, "true", "true");
                }
            }
        }

        public ActionResult VerSolicitudUsuario()
        {

            return PartialView();
        }

        public ActionResult ImprimirSolicitud()
        {
            SolicitudUsuario usuario = new SolicitudUsuario();
            usuario = (SolicitudUsuario)Session["usuario"];
            string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");
            string customSwitches = string.Format("--footer-html \"{0}\" " +
                                                  "--footer-spacing \"10\" " +
                                                  "--footer-line  " +
                                                  "--footer-font-size \"10\" ", footer);
            return new ViewAsPdf("ImprimirSolicitud", usuario)
            {
                FileName = "SolicitudUsuario.pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(10, 10, 30, 10),
                CustomSwitches = customSwitches
            };
        }
        #endregion
        
    }
}
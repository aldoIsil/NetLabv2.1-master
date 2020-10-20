using BusinessLayer;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using DataLayer;
using DataLayer.Area.AreaProcesamiento;
using Model;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class CuentaUsuarioController : Controller
    {
        // GET: CuentaUsuario
        [AllowAnonymous]
        public ActionResult Index()
        {
            var listaBl = new ListaBl();
            var tipoDocumentoList = new List<SelectListItem>();
            var profesionList = new List<SelectListItem>();

            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });

            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.Profesion).ListaDetalle)
                profesionList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });

            var rolDal = new DataLayer.RolDal();
            var rol = rolDal.GetRoles("");
            var rolList = rol.FindAll(p => p.tipo == "9").OrderBy(r => r.descripcion).ToList();
            var enfermerdad = new DataLayer.EnfermedadDal();
            var enfermedades = enfermerdad.GetEnfermedades();

            SolicitudUsuario usuario = new SolicitudUsuario();
            usuario.tipoDocumento = tipoDocumentoList;
            usuario.profesion = profesionList;
            usuario.rol = rolList;
            usuario.enfermedad = enfermedades;

            return View(usuario);
        }
        [AllowAnonymous]
        public string ValidarPersona(string nroDocumento)
        {
            Usuario usuario = new Usuario();
            UsuarioBl usuarioBl = new UsuarioBl();
            string userSol = "";
            var usuarios = usuarioBl.GetUsuarios("", "", "", "", nroDocumento, 0);
            if (usuarios == null)
            {
                usuario = usuarioBl.ValidarDatosUsuario(usuario, nroDocumento);
                userSol = "1" + "|" + usuario.nombres + "|" + usuario.apellidoPaterno + "|" + usuario.apellidoMaterno + "|" + "1";
            }
            else
            {
                userSol = "0" + "|" + usuarios[0].nombres + "|" + usuarios[0].apellidoPaterno + "|" + usuarios[0].apellidoMaterno + "|" +
                    usuarios[0].codigoColegio + "|"+ usuarios[0].correo + "|"+ usuarios[0].cargo + "|"+ usuarios[0].profesion+ "|"+
                    usuarios[0].login + "|" + "2";
            }

            

            return userSol;
        }

        public JsonResult GetEstablecimientos(string Prefix)
        {
            var establecimientoBl = new EstablecimientoDal();
            var establecimientoList = new List<Establecimiento>();
            establecimientoList = establecimientoBl.GetEstablecimientosByTextoBusqueda(Prefix, 0);
            return Json(establecimientoList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExamenesByIdEnfermedadAgrupado(int idEnfermedad)
        {
            var _examenBl = new ExamenBl();
            var examen = _examenBl.GetExamenesByIdEnfermedadAgrupado(idEnfermedad);
            return Json(examen, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void RegistrarSolicitud(int tipoSolicitud, string login, int tipoDocumento, string documentoIdentidad, string nombre, string apellidoPaterno, string apellidoMaterno, string cargo,
                                       string condicionLaboral, int profesion, string codigoColegio, string iniciales, string correo, string telefono, int Establecimiento, string Renipress,
                                       int tipoEst, int[] rol, int componente, int idTipoUsuario,int[] examen, string jefeEESS, string cargoJF, string[] enfermedad, string archivo, string nombreArchivo)
        {
            try
            {
                SolicitudUsuario datos = new SolicitudUsuario()
                {
                    usuario = new Usuario()
                    {
                        login = login,
                        tipoDocumento = tipoDocumento,
                        documentoIdentidad = documentoIdentidad,
                        nombres = nombre,
                        apellidoPaterno = apellidoPaterno,
                        apellidoMaterno = apellidoMaterno,
                        cargo = cargo,
                        condicionLaboral = condicionLaboral,
                        profesion = profesion,
                        codigoColegio = codigoColegio,
                        iniciales = iniciales,
                        correo = correo,
                        telefono = telefono,
                        Establecimiento = Establecimiento,
                        tipo = tipoEst,
                        componente = componente,
                        idTipoUsuario = idTipoUsuario
                    },
                };

                datos.JefeEESS = jefeEESS;
                datos.cargoJf = cargoJF;

                datos.rol = new List<Rol>();
                foreach (var item in rol)
                {
                    datos.rol.Add(new Rol() { idRol = item });
                }
                if (examen != null && examen.Count() > 0)
                {
                    datos.examen = new List<Examen>();
                    foreach (var item in examen)
                    {
                        datos.examen.Add(new Examen() { idExamenAgrupado = item });
                    }
                }

                datos.tipoSolicitud = tipoSolicitud;
                string nombreCompleto = datos.usuario.nombres + ' ' + datos.usuario.apellidoPaterno + ' ' + datos.usuario.apellidoMaterno;
                UsuarioBl user = new UsuarioBl();
                user.InsertarSolicitudUsuario(datos);
                
                UsuarioBl usNet1 = new UsuarioBl();
                SolicitudUsuario net1 = new SolicitudUsuario();
                net1.usuario = new Usuario();
                net1.usuario.idUsuario = usNet1.DevuelveCorrelativo("UsuarioSol");
                net1.usuario.nombres = datos.usuario.nombres;
                net1.usuario.apellidoPaterno = datos.usuario.apellidoPaterno + " " + datos.usuario.apellidoMaterno;
                net1.usuario.correo = datos.usuario.correo;
                net1.usuario.telefono = datos.usuario.telefono;
                net1.fechaEnvio = Convert.ToString(DateTime.Now).Substring(0,10);
                net1.usuario.cargo = datos.usuario.cargo;
                net1.usuario.documentoIdentidad = datos.usuario.documentoIdentidad;
                net1.tipoSolicitud = (datos.tipoSolicitud == 1 ? 0 : 1);
                net1.usuario.codigoColegio = datos.usuario.codigoColegio;
                net1.usuario.login = datos.usuario.login;
                net1.Establecimiento = Renipress;
                net1.file = new Archivo();
                net1.file.cCodArchivo = usNet1.DevuelveCorrelativo("RArchivo");
                net1.file.Codigo = net1.usuario.idUsuario;
                net1.file.dsNombre = nombreArchivo;
                net1.file.dsData = Convert.FromBase64String(archivo);

                if (Renipress.Substring(0,1)=="1" || Renipress.Substring(0, 3) == "INS" || Renipress.Substring(3, 4) == "CNSP")
                {
                    net1.usuario.TipoUsuario = "01";
                }
                else
                {
                    net1.usuario.TipoUsuario = "03";
                }

                usNet1.InsertarSolicitud(net1);

                EnvioCorreAlerta(datos.usuario.correo, nombreCompleto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Archivo GeneraArchivo(HttpPostedFile archivo)
        {
            Archivo file = new Archivo();
            HttpPostedFile obj = archivo;
            byte[] objStream = new byte[obj.ContentLength + 1];
            obj.InputStream.Read(objStream, 0, obj.ContentLength);
            obj.InputStream.Close();

            file.dsData = objStream;
            file.dsNombre = Path.GetFileName(obj.FileName);
            file.FeRegistro = DateTime.Now;
            return file;
        }

        public ActionResult SeguimientoEstadoSolicitud(string documentoIdentidad)
        {
            var listaBl = new ListaBl();
            var tipoDocumentoList = new List<SelectListItem>();
           
            foreach (var itemDetalle in listaBl.GetListaByOpcion(OpcionLista.TipoDocumento).ListaDetalle)
                tipoDocumentoList.Add(new SelectListItem
                {
                    Text = itemDetalle.Glosa,
                    Value = Convert.ToString(itemDetalle.IdDetalleLista)
                });

            ViewBag.tipoDocumentoList = tipoDocumentoList;
            ViewBag.nroDocumento = documentoIdentidad;

            List<SolicitudUsuario> userSol = new List<SolicitudUsuario>();
            if (documentoIdentidad != null)
            {
                UsuarioBl solicitud = new UsuarioBl();
                userSol = solicitud.GetEstadoSolicitud(documentoIdentidad);
            }
            return View(userSol);
        }

        [AllowAnonymous]
        public void EnvioCorreAlerta(string _correo, string nombre)
        {
            var resultadoBl = new ResultadosBl();
            if (_correo != null)
            {
                var correo = new EnvioCorreo();
                string asunto = "Solicitud de cuenta de usuario";
                string contenido = "Estimado(a) Solicitante: " + nombre.ToUpper() + "\n" + "Su solicitud virtual de acceso al sistema fue realizada con éxito, para poder hacerle seguimiento ingresar a la opción " + "Seguimiento de solicitud virtual de cuenta de usuario";
                correo.EnviarCorreo(_correo, asunto, contenido);
            }
        }

        [AllowAnonymous]
        public ActionResult ImprimirSolicitud(int idSolicitudUsuario)
        {
            UsuarioBl solicitud = new UsuarioBl();
            var solUsuario = new SolicitudUsuario();
            solUsuario = solicitud.GetSolicitudUsuario(idSolicitudUsuario);
            string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");
            string customSwitches = string.Format("--footer-html \"{0}\" " +
                                                  "--footer-spacing \"10\" " +
                                                  "--footer-line  " +
                                                  "--footer-font-size \"10\" ", footer);
            return new ViewAsPdf("ImprimirSolicitud", solUsuario)
            {
                FileName = "SolicitudUsuario.pdf",
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait,
                PageMargins = new Margins(10, 10, 30, 10),
                CustomSwitches = customSwitches
            };
        }
    }
}
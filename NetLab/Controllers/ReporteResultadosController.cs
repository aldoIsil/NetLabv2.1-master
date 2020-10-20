using System;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Reportes;
using NetLab.Models.Reportes;
using Rotativa;
using Newtonsoft.Json;
using Rotativa.Options;
using System.IO;
using Model;
using Utilitario;
using System.Configuration;
using System.Linq;
/*****************************/
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using System.Diagnostics;
//using System.IO;

namespace NetLab.Controllers
{
    public class ReporteResultadosController : ParentController
    {
        private const string ContentJpg = "image/jpg";

        private readonly IReporteResultadosBl _reporteResultadosBl;
        private readonly ILaboratorioBl _laboratorioBl;

        public ReporteResultadosController(ILaboratorioBl laboratorioBl, IReporteResultadosBl reporteResultadosBl)
        {
            _laboratorioBl = laboratorioBl;
            _reporteResultadosBl = reporteResultadosBl;
        }

        public ReporteResultadosController() : this(new LaboratorioBl(), new ReporteResultadosBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para realizar la busqueda de resultados procesdos y listos para imprimir.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorio"></param>
        /// <param name="idexamen"></param>
        /// <returns></returns>
        public ActionResult ImprimirResultadosBusqueda(Guid idOrden, int idLaboratorio, string idexamen)
        {
            string lineacorte = string.Empty;
            try
            {
                var resultados = new[] { idexamen };

                var laboratorioUsuario = _laboratorioBl.GetLaboratorioById(idLaboratorio);
                lineacorte = string.Format(" - laboratorioUsuario is null: {0} - ", laboratorioUsuario == null);
                if (laboratorioUsuario == null) return View("Error");

                var ordenResultado = _reporteResultadosBl.GetOrdenResultado(idOrden, idLaboratorio, resultados, Logueado.idUsuario);
                lineacorte = string.Format(" - ordenResultado is null: {0} - ", ordenResultado == null);
                if (ordenResultado == null)
                {
                    ordenResultado = new OrdenResultado();
                }

                var muestras = _reporteResultadosBl.GetMuestrasbyIdOrden(idOrden, idLaboratorio, resultados);
                lineacorte = string.Format(" - muestras is null: {0} - ", muestras == null);
                var examenes = _reporteResultadosBl.GetDetalleExamenes(idOrden, idLaboratorio, resultados);
                lineacorte = string.Format(" - examenes is null: {0} - ", examenes == null);
                //var interpretacion = _reporteResultadosBl.GetInterpretacionExamen(resultados);
                var cargoUsuarioEstablecimiento = _reporteResultadosBl.CargoUsuarioEstablecimiento(idLaboratorio, examenes.First().FechaHoraEmision);
                if (laboratorioUsuario.LogoRegional != null)
                {
                    lineacorte = " - antes de GetClientLogoReg - ";
                    GetClientLogoReg(laboratorioUsuario.IdLaboratorio);
                }
                if (laboratorioUsuario.Logo != null)
                {
                    lineacorte = " - antes de GetClientLogo - ";
                    GetClientLogo(laboratorioUsuario.IdLaboratorio);
                }
                if (laboratorioUsuario.Sello != null)
                {
                    lineacorte = " - antes de GetClientSello - ";
                    GetClientSello(laboratorioUsuario.IdLaboratorio);
                }

                string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");
                lineacorte = string.Format(" - footer: {0} - ", footer);
                string customSwitches = string.Format("--print-media-type " +
                                                      "--header-spacing \"0\" " +
                                                      "--footer-html \"{0}\" " +
                                                      "--footer-spacing \"10\" " +
                                                      "--footer-line  " +
                                                      "--footer-font-size \"10\" ", footer);
                var model = new ReporteResultadoViewModels
                {
                    Orden = ordenResultado,
                    Laboratorio = laboratorioUsuario,
                    Muestras = muestras,
                    Examenes = examenes,
                    CargoUsuarioEstablecimiento = cargoUsuarioEstablecimiento
                };

                //ViewBag.MensajeObservacion = ConfigurationManager.AppSettings["MensajeObservacionReporteKobos"];

                return new ViewAsPdf("Index", model)
                {
                    FileName = "Reporte-" + model.Orden.codigoOrden + ".pdf",
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 10, 30, 10),
                    CustomSwitches = customSwitches
                };

                ///return View("Index", model);
                //return new ViewAsPdf("Index", model);}
            }
            catch (Exception ex)
            {
                lineacorte = string.Format("idorden: {0} - idLaboratorio: {1} - idexamen: {2} - idUsuario: {3} - corte: {4}", idOrden, idLaboratorio, idexamen, Logueado.idUsuario, lineacorte);
                new bsPage().LogError(ex, "LogNetLab", lineacorte, " ImprimirResultadosBusqueda - Post ");
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Print(Guid idOrden, int idLaboratorio, string[] resultados)
        {
            var laboratorioUsuario = _laboratorioBl.GetLaboratorioById(idLaboratorio);

            if (laboratorioUsuario == null) return null;

            var ordenResultado = _reporteResultadosBl.GetOrdenResultado(idOrden, idLaboratorio, resultados, Logueado.idUsuario);

            var muestras = _reporteResultadosBl.GetMuestrasbyIdOrden(idOrden, idLaboratorio, resultados);

            var examenes = _reporteResultadosBl.GetDetalleExamenes(idOrden, idLaboratorio, resultados);
            var cargoUsuarioEstablecimiento = _reporteResultadosBl.CargoUsuarioEstablecimiento(idLaboratorio, examenes.First().FechaHoraEmision);

            var model = new ReporteResultadoViewModels
            {
                Orden = ordenResultado,
                Laboratorio = laboratorioUsuario,
                Muestras = muestras,
                Examenes = examenes,
                CargoUsuarioEstablecimiento = cargoUsuarioEstablecimiento
            };

            return Json(JsonConvert.SerializeObject(model));
        }
        /// <summary>
        /// Descripción: Controlador para obtener informacion y mandar a imprimir el reporte.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Se realizaron las modificaciones para el registro de los datos configurados por componente. Juan Muga
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorio"></param>
        /// <param name="resultados"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImprimirResultado(Guid idOrden, int idLaboratorio, string[] resultados)
        {
            try
            {
                var laboratorioUsuario = _laboratorioBl.GetLaboratorioById(idLaboratorio);

                if (laboratorioUsuario == null) return null;

                var ordenResultado = _reporteResultadosBl.GetOrdenResultado(idOrden, idLaboratorio, resultados, Logueado.idUsuario);

                var muestras = _reporteResultadosBl.GetMuestrasbyIdOrden(idOrden, idLaboratorio, resultados);

                var examenes = _reporteResultadosBl.GetDetalleExamenes(idOrden, idLaboratorio, resultados);

                //var interpretacion = _reporteResultadosBl.GetInterpretacionExamen(resultados);

                var cargoUsuarioEstablecimiento = _reporteResultadosBl.CargoUsuarioEstablecimiento(idLaboratorio, examenes.First().FechaHoraEmision);
                string imageBase64Data = string.Empty;
                string imageDataURL = string.Empty;

                if (!String.IsNullOrEmpty(Convert.ToString(laboratorioUsuario.Logo)))
                {
                    imageBase64Data = Convert.ToBase64String(laboratorioUsuario.Logo);
                    imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageData = imageDataURL;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(laboratorioUsuario.Sello)))
                {
                    imageBase64Data = Convert.ToBase64String(laboratorioUsuario.Sello);
                    imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageSello = imageDataURL;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(ordenResultado.Validador.FirmaDigital)))
                {
                    imageBase64Data = Convert.ToBase64String(ordenResultado.Validador.FirmaDigital);
                    imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageFirma = imageDataURL;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(laboratorioUsuario.LogoRegional)))
                {
                    imageBase64Data = Convert.ToBase64String(laboratorioUsuario.LogoRegional);
                    imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageLogReg = imageDataURL;
                }

                var model = new ReporteResultadoViewModels
                {
                    Orden = ordenResultado,
                    Laboratorio = laboratorioUsuario,
                    Muestras = muestras,
                    Examenes = examenes,
                    CargoUsuarioEstablecimiento = cargoUsuarioEstablecimiento
                };
                var PrintCabeceraEESS = "";
                var PrintCabeceraPaciente = "";
                #region CabeceraEESS            
                if (model.Laboratorio.LogoRegional != null)
                {
                    PrintCabeceraEESS = PrintCabeceraEESS + "logoregional=" + model.Laboratorio.LogoRegional;
                }
                else
                {
                    PrintCabeceraEESS = PrintCabeceraEESS + "logoregional=" + "~/img/logo regional.png";
                }
                if (model.Laboratorio.CodigoInstitucion == 12)
                {
                    PrintCabeceraEESS = PrintCabeceraEESS + "&Institucion=GOBIERNO REGIONAL DE " + model.Laboratorio.UbigeoLaboratorio.Departamento;
                }
                else if (model.Laboratorio.CodigoInstitucion == 20)
                {
                    PrintCabeceraEESS = PrintCabeceraEESS + "&Institucion1=GOBIERNO REGIONAL DE " + model.Laboratorio.UbigeoLaboratorio.Departamento;
                    PrintCabeceraEESS = PrintCabeceraEESS + "&Institucion2=ORGANISMO PÚBLICO EJECUTOR DEL SECTOR SALUD";
                    PrintCabeceraEESS = PrintCabeceraEESS + "&Institucion3=Investigar para proteger la salud";
                    PrintCabeceraEESS = PrintCabeceraEESS + "&Institucion4=CENTRO NACIONAL DE SALUD PUBLICA";
                }
                else
                {
                    PrintCabeceraEESS = PrintCabeceraEESS + "&Institucion5=" + model.Laboratorio.Nombre;
                }
                if (model.Laboratorio.IdDisa != "0 ")
                {
                    PrintCabeceraEESS = PrintCabeceraEESS + "&nombreDisa=" + model.Laboratorio.nombreDisa;
                }
                if (model.Laboratorio.Logo != null)
                {
                    PrintCabeceraEESS = PrintCabeceraEESS + "&LogoInstitucional=" + ViewBag.ImageData;
                }
                else
                {
                    PrintCabeceraEESS = PrintCabeceraEESS + "&LogoInstitucional=~/img/logo.png";
                }
                PrintCabeceraEESS = PrintCabeceraEESS + "&NroInforme=" + model.Orden.NumeroInforme;
                #endregion
                #region CabeceraPaciente
                PrintCabeceraPaciente = PrintCabeceraPaciente + "&nombrePaciente=" + model.Orden.nombrePaciente;
                PrintCabeceraPaciente = PrintCabeceraPaciente + "&DocIdentidad=" + model.Orden.DocIdentidad;
                PrintCabeceraPaciente = PrintCabeceraPaciente + "&Edad=" + model.Orden.Edad;
                PrintCabeceraPaciente = PrintCabeceraPaciente + "&Sexo=" + model.Orden.Sexo;
                PrintCabeceraPaciente = PrintCabeceraPaciente + "&Direccion=" + model.Orden.Direccion;
                PrintCabeceraPaciente = PrintCabeceraPaciente + "&Telefono=" + model.Orden.Telefono;
                PrintCabeceraPaciente = PrintCabeceraPaciente + "&codigoOrden=" + model.Orden.codigoOrden;

                Session["PrintCabeceraPaciente"] = "Juan Muga";
                #endregion
                //string header = Server.MapPath("~/Views/ReporteResultados/PrintHeader.html");//Path of PrintHeader.html FileUrl.Action("PrintHeader","ReporteResultados",model)
                string header = Server.MapPath("~/Views/ReporteResultados/PrintHeader.html");
                string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");//Path of PrintFooter.html File"/ReporteResultados/PrintHeader.html?" + PrintCabeceraEESS + PrintCabeceraPaciente
                String absoluteDir = Server.MapPath("~");
                String myRelativePath = "Views/ReporteResultados/PrintHeader.html".Replace("/", "\\");

                string absolutePath = Path.Combine(absoluteDir, myRelativePath);
                //Path.Combine(absoluteDir, myRelativePath);
                //"--load-error-handling \"ignore\" " +
                //string customSwitches = string.Format("--header-html  \"{0}\" " +
                //                                       "--header-spacing \"0\" " +
                //                                       "--footer-html \"{1}\" " +
                //                                       "--footer-spacing \"10\" " +
                //                                       "--footer-font-size \"10\" " +
                //                                       "--custom-header Cookie \"Web_SessionId=PruebaCookies\" " +
                //                                       "--enable-javascript " +
                //                                       "--header-font-size \"10\" " , absolutePath, footer);
                //Url.Action("PrintHeader", "Home", model, this.Request.Url.Scheme)
                //Url.Action("PrintHeaderReport", "ReporteResultados", model)1
                string customSwitches = string.Format("--footer-html \"{0}\" " +
                                                       "--footer-spacing \"10\" " +
                                                       "--footer-line  " +
                                                       "--footer-font-size \"10\" ", footer);

                //ViewBag.MensajeObservacion = ConfigurationManager.AppSettings["MensajeObservacionReporteKobos"];
                return new ViewAsPdf("Index", model)
                {
                    FileName = "Reporte-" + model.Orden.codigoOrden + ".pdf",
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 10, 30, 10),
                    CustomSwitches = customSwitches
                };
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", " ImprimirResultado - Post ");
                throw ex;
            }
        }
        [AllowAnonymous]
        public ActionResult PrintHeader(ReporteResultadoViewModels model)
        {
            return View("PrintHeaderReport", model);
        }
        /// <summary>
        /// Descripción: Controlador para obtener informacion del resumen por cada metodo con los resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResumenPorExamen(Guid idOrden, Guid idExamen)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioByUserId(Logueado.idUsuario);

            if (laboratorio == null) return View("Error");

            var ordenResultado = _reporteResultadosBl.GetOrdenResultado(idOrden);
            var muestras = _reporteResultadosBl.GetMuestras(idOrden, idExamen, laboratorio.IdLaboratorio);
            var examenes = _reporteResultadosBl.GetDetalleExamenes(idOrden, idExamen);
            var cargoUsuarioEstablecimiento = _reporteResultadosBl.CargoUsuarioEstablecimiento(laboratorio.IdLaboratorio, examenes.First().FechaHoraEmision);

            var model = new ReporteResultadoViewModels
            {
                Orden = ordenResultado,
                Laboratorio = laboratorio,
                Muestras = muestras,
                Examenes = examenes,
                CargoUsuarioEstablecimiento = cargoUsuarioEstablecimiento
            };

            return View(model);
        }
        /// <summary>
        /// Descripción: Controlador para obtener la firma digital e incluirlo en el reporte.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="firmaDigital"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowFirma(byte[] firmaDigital)
        {
            return File(firmaDigital, ContentJpg);
        }

        public virtual FileContentResult GetClientLogo(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);
            byte[] byteArray = laboratorio.Logo;

            string imageBase64Data = Convert.ToBase64String(laboratorio.Logo);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            ViewBag.ImageData = imageDataURL;

            if (byteArray != null)
                return new FileContentResult(byteArray, "image/jpeg");
            return null;
        }

        /// Descripción: Controlador para convertir el sello a formato de imagen.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 19/01/2017
        public virtual FileContentResult GetClientSello(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);
            byte[] byteArray = laboratorio.Sello;

            string imageBase64Data = Convert.ToBase64String(laboratorio.Sello);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            ViewBag.ImageSello = imageDataURL;

            if (byteArray != null)
                return new FileContentResult(byteArray, "image/jpeg");

            return null;
        }

        /// Descripción: Controlador para convertir el logo Regional a formato de imagen.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 19/01/2017
        public virtual FileContentResult GetClientLogoReg(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);
            byte[] byteArray = laboratorio.LogoRegional;

            string imageBase64Data = Convert.ToBase64String(laboratorio.LogoRegional);
            string imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
            ViewBag.ImageLogReg = imageDataURL;

            if (byteArray != null)
                return new FileContentResult(byteArray, "image/jpeg");

            return null;
        }
        /// <summary>
        /// Autor: Juan Muga
        /// Descripcion: Impresión de resultados para las Pruebas Rápidas
        /// Fecha: 20/11/2017
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorio"></param>
        /// <param name="idexamen"></param>
        /// <returns></returns>
        public ActionResult ImprimirResultadosPruebaRapida(Guid idOrden, int idLaboratorio, string[] idexamen)
        {
            try
            {
                var laboratorioUsuario = _laboratorioBl.GetLaboratorioById(idLaboratorio);

                if (laboratorioUsuario == null) return View("Error");

                var ordenResultado = _reporteResultadosBl.GetOrdenResultado(idOrden, idLaboratorio, idexamen, Logueado.idUsuario);

                var muestras = _reporteResultadosBl.GetMuestrasbyIdOrden(idOrden, idLaboratorio, idexamen);

                var examenes = _reporteResultadosBl.GetDetalleExamenes(idOrden, idLaboratorio, idexamen);

                //var interpretacion = _reporteResultadosBl.GetInterpretacionExamen(idexamen);
                var cargoUsuarioEstablecimiento = _reporteResultadosBl.CargoUsuarioEstablecimiento(idLaboratorio, examenes.First().FechaHoraEmision);

                string imageBase64Data = string.Empty;
                string imageDataURL = string.Empty;

                if (!String.IsNullOrEmpty(Convert.ToString(laboratorioUsuario.Logo)))
                {
                    imageBase64Data = Convert.ToBase64String(laboratorioUsuario.Logo);
                    imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageData = imageDataURL;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(laboratorioUsuario.Sello)))
                {
                    imageBase64Data = Convert.ToBase64String(laboratorioUsuario.Sello);
                    imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageSello = imageDataURL;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(ordenResultado.Validador.FirmaDigital)))
                {
                    imageBase64Data = Convert.ToBase64String(ordenResultado.Validador.FirmaDigital);
                    imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageFirma = imageDataURL;
                }
                if (!String.IsNullOrEmpty(Convert.ToString(laboratorioUsuario.LogoRegional)))
                {
                    imageBase64Data = Convert.ToBase64String(laboratorioUsuario.LogoRegional);
                    imageDataURL = string.Format("data:image/png;base64,{0}", imageBase64Data);
                    ViewBag.ImageLogReg = imageDataURL;
                }
                var model = new ReporteResultadoViewModels
                {
                    Orden = ordenResultado,
                    Laboratorio = laboratorioUsuario,
                    Muestras = muestras,
                    Examenes = examenes,
                    CargoUsuarioEstablecimiento = cargoUsuarioEstablecimiento
                };
                string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");
                string customSwitches = string.Format("--footer-html \"{0}\" " +
                                                       "--footer-spacing \"10\" " +
                                                       "--footer-line  " +
                                                       "--footer-font-size \"10\" ", footer);

                ViewBag.MensajeObservacion = ConfigurationManager.AppSettings["MensajeObservacionReporteKobos"];
                return new ViewAsPdf("Index", model)
                {
                    FileName = "Reporte-" + model.Orden.codigoOrden + ".pdf",
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 10, 30, 10),
                    CustomSwitches = customSwitches
                };
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", "ImprimirResultadosPruebaRapida");
                throw ex;
            }
        }


        public ActionResult ImprimirResultadosKobos(int id)
        {
            try
            {
                ResultadoKobos kobos = new ResultadoKobos();
                ReporteResultadosBl bl = new ReporteResultadosBl();
                kobos = bl.GetResultadoKobosId(id, Logueado.idUsuario);
                string paciente = kobos.apellidoPaterno + " " + kobos.apellidoMaterno + " " + kobos.Nombres;
                string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");
                string customSwitches = string.Format("--print-media-type " +
                                                      "--header-spacing \"0\" " +
                                                      "--footer-html \"{0}\" " +
                                                      "--footer-spacing \"10\" " +
                                                      "--footer-line  " +
                                                      "--footer-font-size \"10\" ", footer);

                ViewBag.MensajeObservacion = ConfigurationManager.AppSettings["MensajeObservacionReporteKobos"];
                return new ViewAsPdf("ReporteKobos", kobos)
                {
                    FileName = "ReporteKobos-" + paciente + ".pdf",
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 10, 30, 10),
                    CustomSwitches = customSwitches
                };

            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", "ImprimirResultadosKobos");
                throw ex;
            }
        }
        public byte[] ImprimirResultadosKobo(int id)
        {
            try
            {
                ResultadoKobos kobos = new ResultadoKobos();
                ReporteResultadosBl bl = new ReporteResultadosBl();
                kobos = bl.GetResultadoKobosId(id, Logueado.idUsuario);
                string paciente = kobos.apellidoPaterno + " " + kobos.apellidoMaterno + " " + kobos.Nombres;
                string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");
                string customSwitches = string.Format("--print-media-type " +
                                                      "--header-spacing \"0\" " +
                                                      "--footer-html \"{0}\" " +
                                                      "--footer-spacing \"10\" " +
                                                      "--footer-line  " +
                                                      "--footer-font-size \"10\" ", footer);

                ViewBag.MensajeObservacion = ConfigurationManager.AppSettings["MensajeObservacionReporteKobos"];
                var pdf = new ViewAsPdf("ReporteKobos", kobos)
                {
                    FileName = "ReporteKobos-" + paciente + ".pdf",
                    PageSize = Size.A4,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(10, 10, 30, 10),
                    CustomSwitches = customSwitches
                };
                return pdf.BuildPdf(ControllerContext);
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", "ImprimirResultadosKobos");
                throw ex;
            }
        }
        public byte[] DescargaResultados(string idOrdenexamen, string codigoPruebaRapida)
        {
            try
            {
                if (string.IsNullOrEmpty(codigoPruebaRapida))
                {
                    return ImprimirResultadosKobo(int.Parse(codigoPruebaRapida));
                }
                else
                {
                    var resultados = new[] { idOrdenexamen };
                    var ordenResultado = _reporteResultadosBl.GetOrdenResultadoWS(Guid.Parse(idOrdenexamen));
                    var laboratorioUsuario = _laboratorioBl.GetLaboratorioById(ordenResultado.idLaboratorioDestino);
                    var muestras = _reporteResultadosBl.GetMuestrasbyIdOrden(ordenResultado.idOrden, ordenResultado.idLaboratorioDestino, resultados);
                    var examenes = _reporteResultadosBl.GetDetalleExamenes(ordenResultado.idOrden, ordenResultado.idLaboratorioDestino, resultados);
                    var cargoUsuarioEstablecimiento = _reporteResultadosBl.CargoUsuarioEstablecimiento(ordenResultado.idLaboratorioDestino, examenes.First().FechaHoraEmision);
                    if (laboratorioUsuario.LogoRegional != null)
                    {
                        GetClientLogoReg(laboratorioUsuario.IdLaboratorio);
                    }
                    if (laboratorioUsuario.Logo != null)
                    {
                        GetClientLogo(laboratorioUsuario.IdLaboratorio);
                    }
                    if (laboratorioUsuario.Sello != null)
                    {
                        GetClientSello(laboratorioUsuario.IdLaboratorio);
                    }

                    string footer = Server.MapPath("~/Views/ReporteResultados/PrintFooter.html");
                    string customSwitches = string.Format("--print-media-type " +
                                                          "--header-spacing \"0\" " +
                                                          "--footer-html \"{0}\" " +
                                                          "--footer-spacing \"10\" " +
                                                          "--footer-line  " +
                                                          "--footer-font-size \"10\" ", footer);
                    var model = new ReporteResultadoViewModels
                    {
                        Orden = ordenResultado,
                        Laboratorio = laboratorioUsuario,
                        Muestras = muestras,
                        Examenes = examenes,
                        CargoUsuarioEstablecimiento = cargoUsuarioEstablecimiento
                    };

                    var pdf = new ViewAsPdf("Index", model)
                    {
                        FileName = "Reporte-" + model.Orden.codigoOrden + ".pdf",
                        PageSize = Size.A4,
                        PageOrientation = Orientation.Portrait,
                        PageMargins = new Margins(10, 10, 30, 10),
                        CustomSwitches = customSwitches
                    };
                    //byte[] applicationPDFData = pdf.BuildPdf(ControllerContext);
                    //string fullPath = @"\\server\network\path\pdfs\" + pdf.FileName;
                    //using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    //{
                    //    fileStream.Write(pdf, 0, pdfData.Length);
                    //}
                    return pdf.BuildPdf(ControllerContext);
                }

            }
            catch (Exception ex)
            {
                //lineacorte = string.Format("idorden: {0} - idLaboratorio: {1} - idexamen: {2} - idUsuario: {3} - corte: {4}", idOrden, idLaboratorio, idexamen, Logueado.idUsuario, lineacorte);
                //new bsPage().LogError(ex, "LogNetLab", lineacorte, " ImprimirResultadosBusqueda - Post ");
                throw ex;
            }
        }
    }
}
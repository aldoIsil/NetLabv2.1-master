using System;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Interfaces;
using Model;
using NetLab.Controllers.FormConverter;
using NetLab.Controllers.FormConverter.Entities;
using NetLab.Controllers.FormConverter.Interfaces;
using NetLab.Models;
using PagedList;
using System.Collections.Generic;
using NetLab.App_Code;
using System.Linq;
using DataLayer;
using Model.Enums;
using System.Web;
using System.Collections;
using Newtonsoft.Json;
using NetLab.Helpers;

namespace NetLab.Controllers
{
    public class EESSController : ParentController
    {
        private const string ContentJpg = "image/png";
        private readonly ILaboratorioBl _laboratorioBl;
        private readonly IExamenBl _examenBl;
        private readonly ILaboratorioExamenBl _laboratorioExamenBl;
        private readonly ILaboratorioConverter _laboratorioConverter;

        public EESSController(ILaboratorioBl laboratorioBl, ILaboratorioExamenBl laboratorioExamenBl, ILaboratorioConverter laboratorioConverter, IExamenBl examenBl)
        {
            _laboratorioBl = laboratorioBl;
            _laboratorioExamenBl = laboratorioExamenBl;
            _laboratorioConverter = laboratorioConverter;
            _examenBl = examenBl;
        }

        public EESSController() : this(new LaboratorioBl(), new LaboratorioExamenBl(), new LaboratorioConverter(), new ExamenBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de busqueda y mantenimiento de un Establecimiento.
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

            var laboratorios = _laboratorioBl.GetLaboratoriosByNombre(search);
            var pageOfLaboratorios = laboratorios.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = search;

            return View(pageOfLaboratorios);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de busqueda y los agregar Establecimientos por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarExamenes(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);
            var examenes = _laboratorioExamenBl.GetExamenesByLaboratorio(id);

            var model = new LaboratorioExamenViewModels
            {
                Laboratorio = laboratorio,
                Examenes = examenes
            };

            return View("LaboratorioExamen", model);
        }
        /// <summary>
        /// Descripción: Controlador para agregar Establecimiento por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarExamen(int idLaboratorio)
        {
            var model = new ExamenLaboratorio
            {
                IdLaboratorio = idLaboratorio
            };

            return PartialView("_AgregarExamen", model);
        }
        /// <summary>
        /// Descripción: Controlador para guardar Establecimiento por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarExamen(ExamenLaboratorio model)
        {
            model.IdUsuarioRegistro = Logueado.idUsuario;

            _laboratorioExamenBl.InsertExamenByLaboratorio(model);

            return PartialView("_AgregarExamen", model);

            //return RedirectToAction("AgregarExamenes", new { id = model.IdLaboratorio });
        }
        /// <summary>
        /// Descripción: Controlador para quitar un examen de un Establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarExamen(Guid idExamen, int idLaboratorio)
        {
            var examenLaboratorio = new ExamenLaboratorio
            {
                IdLaboratorio = idLaboratorio,
                IdExamen = idExamen,
                IdUsuarioEdicion = Logueado.idUsuario,
                Estado = 0
            };

            try
            {
                _laboratorioExamenBl.UpdateExamenByLaboratorio(examenLaboratorio);

                return RedirectToAction("AgregarExamenes", new { id = idLaboratorio });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador muestra la ventana para editar Establecimientos
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
        public ActionResult EditarLaboratorio(int id, int? page, string search)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);

            var model = new LaboratorioViewModels
            {
                Laboratorio = laboratorio
            };

            ViewBag.page = page;
            ViewBag.search = search;
            ViewBag.codigoUnico = model.Laboratorio.CodigoUnico;
            
            @ViewBag.institucion = model.Laboratorio.nombreInstitucion;
            @ViewBag.disa = model.Laboratorio.nombreDisa;
            @ViewBag.red = model.Laboratorio.nombreRed;
            @ViewBag.microred = model.Laboratorio.nombreMicroRED;

            Session["nombreInstitucion"] = model.Laboratorio.nombreInstitucion;
            Session["nombreDisa"] = model.Laboratorio.nombreDisa;
            Session["nombreRed"] = model.Laboratorio.nombreRed;
            Session["nombreMicroRED"] = model.Laboratorio.nombreMicroRED;
            Session["nombreDepartamento"] = model.Laboratorio.UbigeoLaboratorio.Departamento;

            ViewBag.tipo = model.Laboratorio.tipo;

            ViewBag.categoria = model.Laboratorio.IdCategoria;

            // return PartialView("_EditarLaboratorio", model);
            return View("_EditarLaboratorio", model);
        }
        /// <summary>
        /// Descripción: Controlador para realizar la edicion de los Establecimientos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="model"></param>
        /// <param name="tipo"></param>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarLaboratorio(int id, int? page, string search, LaboratorioViewModels model, int tipo, int categoria)
        {
            try
            {
                var lab = model.Laboratorio;
                lab.IdCategoria = categoria;
                lab.tipo = tipo;
                lab.nombreInstitucion = Session["nombreInstitucion"].ToString();
                lab.nombreDisa = Session["nombreDisa"].ToString();
                lab.nombreRed = Session["nombreRed"].ToString();
                lab.nombreMicroRED = Session["nombreMicroRED"].ToString();
                

                var laboratorioConverterRequest = new LaboratorioConverterRequest
                {
                    Laboratorio = lab,
                    IdUsuarioLogueado = Logueado.idUsuario,
                    LaboratorioModel = model
                };

                var laboratorio = _laboratorioConverter.ConvertFrom(laboratorioConverterRequest);

                if (Request.Files["Logo"] != null && Request.Files["Logo"].ContentLength>0)
                {
                    laboratorio.Logo = FileHelpers.GetBytes(Request.Files["Logo"]);
                }

                if (Request.Files["LogoRegional"] != null && Request.Files["LogoRegional"].ContentLength > 0)
                {
                    laboratorio.LogoRegional = FileHelpers.GetBytes(Request.Files["LogoRegional"]);
                }

                if (Request.Files["Sello"] != null && Request.Files["Sello"].ContentLength > 0)
                {
                    laboratorio.Sello = FileHelpers.GetBytes(Request.Files["Sello"]);
                }

                _laboratorioBl.UpdateLaboratorio(laboratorio);

                ViewBag.page = page;
                ViewBag.search = search;
                ViewBag.ocultar = "si";

                //model.Sello = laboratorio.Sello;
                //model.Logo = laboratorio.Logo;
                //model.LogoRegional = laboratorio.LogoRegional;
                //return View("_EditarLaboratorio", model);

                //Actualizar lista estatica de establecimientos
                StaticCache.RecargarLaboratorios();
                return RedirectToAction("EditarLaboratorio", new { id = id, page = page, search = search });
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        /// Descripción: Controlador para obtener el logo Regional de un establecimiento.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 19/01/2018
        [HttpGet]
        public ActionResult ShowLogoRegional(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);

            return File(laboratorio.LogoRegional, ContentJpg);
        }

        /// <summary>
        /// Descripción: Controlador para obtener el logo de un establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowLogo(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);

            return File(laboratorio.Logo, ContentJpg);
        }
        /// <summary>
        /// Descripción: Controlador para obtener el sello de un establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowSello(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);

            return File(laboratorio.Sello, ContentJpg);
        }
        /// <summary>
        /// Descripción: Obtiene examenes filtrado por el nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetExamenes()
        {
            var data = Request.Params["data[q]"];

            var examenList = _examenBl.GetExamenesByNombre(data);

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeExamen = false;

            foreach (var examen in examenList)
            {
                resultado += "{\"id\":\"" + examen.idExamen + "\",\"text\":\"" + examen.nombre + "\"},";
                existeExamen = true;
            }

            if (existeExamen)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }
        /// <summary>
        /// Descripción: Controlador muestra ventana para mostrar y editar un establecimiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarExamen(Guid id, int idLaboratorio)
        {
            var examenLaboratorio = _laboratorioExamenBl.GetExamenLaboratorioById(id, idLaboratorio);

            return PartialView("_EditarExamen", examenLaboratorio);
        }

        /// <summary>
        /// Descripción: Controlador para mostrar la ventana de registro de un Establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoEESS(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;

            var dal = new UsuarioDal();
            //Institucion-DISA-RED-MICRORED-EE.SS_LAB
            var laboratorios = dal.getLaboratoriosUsuario(Logueado.idUsuario);


            ArrayList codigosList = new ArrayList();

            var laboratoriosAll = _laboratorioBl.GetLaboratoriosByNombre("");


            foreach (var item in laboratoriosAll)
            {
                codigosList.Add(item.CodigoUnico);
            }


            ViewBag.nombreList = JsonConvert.SerializeObject(codigosList);


            var local = new Local
            {
                IdUsuario = Logueado.idUsuario,
                TipoLocal = TipoLocal.Institucion
            };

            var localBl = new LocalBl();
            var institucion = localBl.GetInstituciones(local);


            Session["LaboratoriosLogin"] = laboratorios;
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
            
            ViewBag.Locales = new Local
            {
                IdInstitucion = 0,
                IdEstablecimiento = 0,
                IdRed = "0",
                IdDisa = "0",
                IdMicroRed = "0"
            };

            Laboratorio LaboratorioUbigeo = new Laboratorio();
            LaboratorioUbigeo.UbigeoLaboratorio = new Ubigeo();
            LaboratorioUbigeo.UbigeoLaboratorio.Id = "      ";
            ViewBag.LabUbigeo = LaboratorioUbigeo;

            return View("NuevoEESS");
        }

        /// Descripción: Método recibe el código ubigeo generado en Ubigeo.js.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 30/01/2018
        public string CodigoUbigeoLaboratorio(String ubigeo)
        {
            Session["ubigeo"] = ubigeo;
            return ubigeo;
        }

        /// <summary>
        /// Descripción: Controlador para registrar un Establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Modificado por: Marcos Mejia
        /// Fecha Modificación: 30/01/2018.
        /// Modificación: Llama a la variable ubigeo antes de insertar el Establecimiento.
        /// </summary>
        /// <param name="codigoUnico"></param>
        /// <param name="nombreEstablecimiento"></param>
        /// <param name="clasificacion"></param>
        /// <param name="ubigeo"></param>
        /// <param name="direccion"></param>
        /// <param name="hdnTipo"></param>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="categoria"></param>
        /// <param name="latitud"></param>
        /// <param name="longitud"></param>
        /// <param name="tipo"></param>
        /// <param name="correoelectronico"></param>
        /// <param name="website"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SelectMain(string codigoUnico, string nombreEstablecimiento, string clasificacion, string ubigeo, string direccion,
            string hdnTipo, string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, int categoria, string latitud, string longitud, int tipo, string correoelectronico, string website, LaboratorioViewModels model)
        {
            
            ArrayList codigosList = new ArrayList();
            var laboratoriosAll = _laboratorioBl.GetLaboratoriosByNombre("");

            foreach (var item in laboratoriosAll)
            {
                codigosList.Add(item.CodigoUnico);
            }


            ViewBag.nombreList = JsonConvert.SerializeObject(codigosList);

            if (hdnInstitucion != "0")
            {
                Session["idInstitucion"] = hdnInstitucion;
            }

            if (hdnDisa != "0")
            {
                Session["idDisa"] = hdnDisa;
            }
            else
            {
                Session["idDisa"] = "0";
            }

            if (hdnMicroRed != "0")
            {
                Session["idRed"] = hdnMicroRed;

            }
            else
            {
                Session["idRed"] = "00";
            }

            if (hdnRed != "0")
            {
                Session["idMicroRed"] = hdnRed;
            }
            else
            {
                Session["idMicroRed"] = "00";
            }



            string idInstitucionInsert = Session["idInstitucion"].ToString();
            string idDisaInsert = Session["idDisa"].ToString();
            string idRedInsert = Session["idRed"].ToString();
            string isMicroRed = Session["idMicroRed"].ToString();



            switch (hdnTipo)
            {
                case "Instituciones":
                    return SelectInstituciones(hdnInstitucion, Logueado.idUsuario);
                case "Disa":
                    return SelectDisa(hdnInstitucion, hdnDisa, Logueado.idUsuario);
                case "Red":
                    return SelectRed(hdnInstitucion, hdnDisa, hdnRed, Logueado.idUsuario);
                case "MicroRed":
                    return SelectMicroRed(hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, Logueado.idUsuario);
                case "0":
                    ubigeo = Convert.ToString(Session["ubigeo"]);
                    if (Request.Files["Logo"] != null && Request.Files["Logo"].ContentLength > 0)
                    {
                        model.InsertarLogo = FileHelpers.GetBytes(Request.Files["Logo"]);
                    }

                    if (Request.Files["LogoRegional"] != null && Request.Files["LogoRegional"].ContentLength > 0)
                    {
                        model.InsertarLogoRegional = FileHelpers.GetBytes(Request.Files["LogoRegional"]);
                    }

                    if (Request.Files["Sello"] != null && Request.Files["Sello"].ContentLength > 0)
                    {
                        model.InsertarSello = FileHelpers.GetBytes(Request.Files["Sello"]);
                    }

                    return InsertarLaboratorio(codigoUnico, nombreEstablecimiento, clasificacion, ubigeo, direccion,
               idInstitucionInsert, idDisaInsert, idRedInsert, isMicroRed, categoria, latitud, longitud, tipo, correoelectronico, website, model);
            }
            Session["ubigeo"] = "";
            ViewBag.IsSearch = false;
            return View("NuevoEESS");
        }

        /// <summary>
        /// Descripción: Metodo para realizar la ejecucion del registro en BD de un nuevo establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="codigoUnico"></param>
        /// <param name="nombreEstablecimiento"></param>
        /// <param name="clasificacion"></param>
        /// <param name="ubigeo"></param>
        /// <param name="direccion"></param>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="categoria"></param>
        /// <param name="latitud"></param>
        /// <param name="longitud"></param>
        /// <param name="tipo"></param>
        /// <param name="correoelectronico"></param>
        /// <param name="website"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult InsertarLaboratorio(string codigoUnico, string nombreEstablecimiento, string clasificacion, string ubigeo, string direccion,
            string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, int categoria, string latitud, string longitud, int tipo, string correoelectronico, string website, LaboratorioViewModels model)
        {
            try
            {

                var Lab = new Laboratorio
                {
                    CodigoInstitucion = int.Parse(hdnInstitucion),
                    CodigoUnico = codigoUnico,
                    Nombre = nombreEstablecimiento,
                    clasificacion = clasificacion,
                    Ubigeo = ubigeo,
                    Direccion = direccion,
                    IdDisa = hdnDisa.Trim(),
                    IdRed = hdnMicroRed.Trim(),
                    IdMicroRed = hdnRed.Trim(),
                    IdCategoria = categoria,
                    Latitud = latitud,
                    Longitud = longitud,
                    tipo = tipo,
                    correoElectronico = correoelectronico,
                    Website = website
                };


                var laboratorioConverterRequest = new LaboratorioConverterRequest
                {
                    Laboratorio = Lab,
                    IdUsuarioLogueado = Logueado.idUsuario,
                    LaboratorioModel = model
                };

                var laboratorio = _laboratorioConverter.ConvertFromInsert(laboratorioConverterRequest);

                _laboratorioBl.InsertLaboratorio(laboratorio);

                int page = 1;
                string search = "";
                ViewBag.verboton = 1;
                //Actualizar lista estatica de establecimientos
                StaticCache.RecargarLaboratorios();
                return RedirectToAction("NuevoEESS", new { page, search });
            }
            catch (Exception ex)
            {
                var res = ex.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Descripción: Controlador para mostrar la ventana de modificacion y editar los examenes de un EESS
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenLaboratorio"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarExamen(ExamenLaboratorio examenLaboratorio)
        {
            try
            {
                examenLaboratorio.IdUsuarioEdicion = Logueado.idUsuario;

                _laboratorioExamenBl.UpdateExamenByLaboratorio(examenLaboratorio);

                return RedirectToAction("AgregarExamenes", new { id = examenLaboratorio.IdLaboratorio });
            }
            catch
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Descripción: Metodo que devuelve las instituciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Modificado por: Marcos Mejia
        /// Fecha Modificación: 30/01/2018.
        /// Modificación: Se asigna el valor del ubigeo al UbigeoLaboratorio.Id para la vista.
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

            String ubigeo = Convert.ToString(Session["ubigeo"]);
            Laboratorio LaboratorioUbigeo = new Laboratorio();
            LaboratorioUbigeo.UbigeoLaboratorio = new Ubigeo();
            LaboratorioUbigeo.UbigeoLaboratorio.Id = ubigeo;
            ViewBag.LabUbigeo = LaboratorioUbigeo;

            return View("NuevoEESS");
        }
        /// <summary>
        /// Descripción: Metodo que devuelve las Disas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Modificado por: Marcos Mejia
        /// Fecha Modificación: 30/01/2018.
        /// Modificación: Se asigna el valor del ubigeo al UbigeoLaboratorio.Id para la vista.
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

            String ubigeo = Convert.ToString(Session["ubigeo"]);
            Laboratorio LaboratorioUbigeo = new Laboratorio();
            LaboratorioUbigeo.UbigeoLaboratorio = new Ubigeo();
            LaboratorioUbigeo.UbigeoLaboratorio.Id = ubigeo;
            ViewBag.LabUbigeo = LaboratorioUbigeo;

            return View("NuevoEESS");
        }
        /// <summary>
        /// Descripción: Metodo que devuelve las Redes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Modificado por: Marcos Mejia
        /// Fecha Modificación: 30/01/2018.
        /// Modificación: Se asigna el valor del ubigeo al UbigeoLaboratorio.Id para la vista.
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

            String ubigeo = Convert.ToString(Session["ubigeo"]);
            Laboratorio LaboratorioUbigeo = new Laboratorio();
            LaboratorioUbigeo.UbigeoLaboratorio = new Ubigeo();
            LaboratorioUbigeo.UbigeoLaboratorio.Id = ubigeo;
            ViewBag.LabUbigeo = LaboratorioUbigeo;

            return View("NuevoEESS");
        }
        /// <summary>
        /// Descripción: Metodo que devuelve las MicroRedes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Modificado por: Marcos Mejia
        /// Fecha Modificación: 30/01/2018.
        /// Modificación: Se asigna el valor del ubigeo al UbigeoLaboratorio.Id para la vista.
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

            String ubigeo = Convert.ToString(Session["ubigeo"]);
            Laboratorio LaboratorioUbigeo = new Laboratorio();
            LaboratorioUbigeo.UbigeoLaboratorio = new Ubigeo();
            LaboratorioUbigeo.UbigeoLaboratorio.Id = ubigeo;
            ViewBag.LabUbigeo = LaboratorioUbigeo;

            return View("NuevoEESS");
        }
        /// <summary>
        /// Descripción: Metodo que devuelve las Redes filtrados por la institucion y a disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
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
        /// Descripción: Metodo que devuelve las MicroRedes filtrados por la institucion, la disa y la red
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
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
        /// Descripción: Metodo que devuelve los Establecimientos filtrados por la institucion, la disa, la red y la microred
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
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
        /// Descripción: Metodo que inicializa las Disas
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="disas"></param>
        private void SetDisaView(IList<DISA> disas)
        {
            Session["disas"] = disas;
            disas.Insert(0, new DISA { IdDISA = "-", NombreDISA = "Seleccione un elemento" });
            ViewBag.disas = disas;
        }
        /// <summary>
        /// Descripción: Metodo que inicializa las Redes
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="redes"></param>
        private void SetRedView(IList<Red> redes)
        {
            Session["redes"] = redes;
            redes.Insert(0, new Red { IdRed = "-", NombreRed = "Seleccione una Red" });
            ViewBag.redes = redes;
        }
        /// <summary>
        /// Descripción: Metodo que inicializa las MicroRedes
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="microRedes"></param>
        private void SetMicroRedView(IList<MicroRed> microRedes)
        {
            Session["microredes"] = microRedes;
            microRedes.Insert(0, new MicroRed { IdMicroRed = "-", NombreMicroRed = "Seleccione una Micro Red" });
            ViewBag.microredes = microRedes;
        }
        /// <summary>
        /// Descripción: Metodo que inicializa los Establecimientos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="establecimientos"></param>
        private void SetEstablecimientoView(IList<Establecimiento> establecimientos)
        {
            //var loginEstablecimientoList = LoginEstablecimientoList(establecimientos);

            //Session["loginEstablecimientoList"] = loginEstablecimientoList;
            Session["EstablecimientosLogin"] = establecimientos;
            establecimientos.Insert(0,
                new Establecimiento { IdEstablecimiento = 0, Nombre = "Seleccione un Establecimiento", CodigoUnico = "00000" });
            ViewBag.establecimientos = establecimientos;
        }
        /// <summary>
        /// Descripción: Metodo que obtiene el establecimiento del usuario logeado
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
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


        //[Route("NetlabAdmin")]
        public ActionResult RecargarEstablecimientos()
        {
            StaticCache.RecargarLaboratorios();

            return new EmptyResult();
        }
    }
}
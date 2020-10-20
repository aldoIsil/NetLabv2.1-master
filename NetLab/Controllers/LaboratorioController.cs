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

namespace NetLab.Controllers
{
    public class LaboratorioController : ParentController
    {
        private const string ContentJpg = "image/png";
        private readonly ILaboratorioBl _laboratorioBl;
        private readonly IExamenBl _examenBl;
        private readonly ILaboratorioExamenBl _laboratorioExamenBl;
        private readonly ILaboratorioConverter _laboratorioConverter;

        public LaboratorioController(ILaboratorioBl laboratorioBl, ILaboratorioExamenBl laboratorioExamenBl, ILaboratorioConverter laboratorioConverter, IExamenBl examenBl)
        {
            _laboratorioBl = laboratorioBl;
            _laboratorioExamenBl = laboratorioExamenBl;
            _laboratorioConverter = laboratorioConverter;
            _examenBl = examenBl;
        }

        public LaboratorioController() : this(new LaboratorioBl(), new LaboratorioExamenBl(), new LaboratorioConverter(), new ExamenBl())
        {
        }

        [HttpGet]
        public ActionResult Index(int? page, string search)
        {
            var pageNumber = page ?? 1;

            var laboratorios = _laboratorioBl.GetLaboratoriosByNombre(search);
            var pageOfLaboratorios = laboratorios.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = search;

            return View(pageOfLaboratorios);
        }

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

        [HttpGet]
        public ActionResult AgregarExamen(int idLaboratorio)
        {
            var model = new ExamenLaboratorio
            {
                IdLaboratorio = idLaboratorio
            };

            return PartialView("_AgregarExamen", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarExamen(ExamenLaboratorio model)
        {
            model.IdUsuarioRegistro = Logueado.idUsuario;

            _laboratorioExamenBl.InsertExamenByLaboratorio(model);

            return PartialView("_AgregarExamen", model);

            //return RedirectToAction("AgregarExamenes", new { id = model.IdLaboratorio });
        }

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

            return PartialView("_EditarLaboratorio", model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarLaboratorio(int id, int? page, string search, LaboratorioViewModels model)
        {
            try
            {
                var laboratorioConverterRequest = new LaboratorioConverterRequest
                {
                    Laboratorio = model.Laboratorio,
                    IdUsuarioLogueado = Logueado.idUsuario,
                    LaboratorioModel = model
                };



                var laboratorio = _laboratorioConverter.ConvertFrom(laboratorioConverterRequest);

                _laboratorioBl.UpdateLaboratorio(laboratorio);

                ViewBag.page = page;
                ViewBag.search = search;

                return RedirectToAction("Index", new { page, search });
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult ShowLogoRegional(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);

            return File(laboratorio.LogoRegional, ContentJpg);
        }
        [HttpGet]
        public ActionResult ShowLogo(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);

            return File(laboratorio.Logo, ContentJpg);
        }

        [HttpGet]
        public ActionResult ShowSello(int id)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(id);

            return File(laboratorio.Sello, ContentJpg);
        }

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

        [HttpGet]
        public ActionResult EditarExamen(Guid id, int idLaboratorio)
        {
            var examenLaboratorio = _laboratorioExamenBl.GetExamenLaboratorioById(id, idLaboratorio);

            return PartialView("_EditarExamen", examenLaboratorio);
        }

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
    }
}
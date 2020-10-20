using BusinessLayer;
using NetLab.Models;
using System;
using System.Web.Mvc;
using PagedList;
using BusinessLayer.Interfaces;
using Model;

namespace NetLab.Controllers
{
    public class EnfermedadController : ParentController
    {
        private readonly IExamenBl _examenBl;
        private readonly IEnfermedadBl _enfermedadBl;
        private readonly IEnfermedadExamenBl _enfermedadExamenBl;

        public EnfermedadController(IExamenBl examenBl, IEnfermedadBl enfermedadBl, IEnfermedadExamenBl enfermedadExamenBl)
        {
            _examenBl = examenBl;
            _enfermedadBl = enfermedadBl;
            _enfermedadExamenBl = enfermedadExamenBl;
        }

        public EnfermedadController() : this(new ExamenBl(), new EnfermedadBl(), new EnfermedadExamenBl())
        {
        }
        /// <summary>
        /// Descripción: Obteniene todos las enfermedades  y examenes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EnfermedadExamen(Guid idExamen)
        {
            try
            {
                var examen = _examenBl.GetExamenById(idExamen);

                var enfermedades = _enfermedadExamenBl.GetEnfermedadByExamenId(idExamen);

                var model = new EnfermedadExamenViewModels
                {
                    Examen = examen,
                    Enfermedades = enfermedades
                };

                return View("EnfermedadExamen", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Muestra la pantalla para el registro de una enfermedad
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarEnfermedad(int? page, string search, Guid idExamen)
        {
            var pageNumber = page ?? 1;
            var searchCriteria = search ?? string.Empty;

            var enfermedades = _enfermedadBl.GetEnfermedadesByNombre(searchCriteria);
            var pageOfEnfermedades = enfermedades.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = searchCriteria;
            ViewBag.idExamen = idExamen;

            return PartialView("_AgregarEnfermedad", pageOfEnfermedades);
        }
        /// <summary>
        /// Descripción: Registra informacion de la enfermedad.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <param name="enfermedades"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GuardarEnfermedad(Guid idExamen, int[] enfermedades)
        {
            var examen = _examenBl.GetExamenById(idExamen);
            var idUsuario = Logueado.idUsuario;

            _enfermedadExamenBl.AgregarEnfermedadesPorExamen(examen, enfermedades, idUsuario);

            return RedirectToAction("EnfermedadExamen", new {idExamen });
        }
        /// <summary>
        /// Descripción: Realiza el soft delete de una enfermedad.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="enfermedadExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarEnfermedad(EnfermedadExamen enfermedadExamen)
        {
            enfermedadExamen.IdUsuarioEdicion = Logueado.idUsuario;
            enfermedadExamen.Estado = 0;

            try
            {
                _enfermedadExamenBl.UpdateEnfermedadByExamen(enfermedadExamen);

                return RedirectToAction("EnfermedadExamen", new { enfermedadExamen.IdExamen });
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
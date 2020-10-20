using BusinessLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class InstitucionController : ParentController
    {
        // GET: Institucion
        public ActionResult Index(string search)
        {
            InstitucionBl institucion = new InstitucionBl();
            List<Institucion> lista = new List<Institucion>();
            lista = institucion.ObtenerInstitucionPorTexto(search);
            ViewBag.search = search;
            return View("Index",lista);
        }

        public ActionResult Crear()
        {
            return PartialView("_NuevaInstitucion");
        }

        public ActionResult CrearInstitucion(string nombreInstitucion, string descripcion)
        {
            InstitucionBl institucion = new InstitucionBl();
            try
            {
                institucion.IngresarInstitucion(nombreInstitucion, Logueado.idUsuario, descripcion);
                return RedirectToAction("Index", new { search = "" });
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult GetInstitucion(int id)
        {
            InstitucionBl institucion = new InstitucionBl();
            var _institucion = institucion.ObtenerInstitucionPorId(id);
            return PartialView("_EditarInstitucion", _institucion);
        }

        public ActionResult EditarInstitucion(int id, string nombreInstitucion,string descripcion, int estado)
        {
            InstitucionBl institucion = new InstitucionBl();
            try
            {
                institucion.ActualizarInstitucion(id, nombreInstitucion, Logueado.idUsuario, descripcion, estado);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
            
        }
    }
}
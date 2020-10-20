using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using BusinessLayer.Compartido.Interfaces;
using BusinessLayer.Interfaces;
using BusinessLayer.Plantilla;
using Model;
using NetLab.Models.Plantilla;
using PagedList;

namespace NetLab.Controllers
{
    public class PlantillaController : ParentController
    {
        private const int Mixto = 3;
        private readonly IPlantillaBl _plantillaBl;
        private readonly IListaBl _listaBl;
        private readonly IExamenBl _examenBl;
        private readonly ITipoMuestraBl _tipoMuestraBl;
        private readonly IMaterialBl _materialBl;

        public PlantillaController(IPlantillaBl plantillaBl, IListaBl listaBl, IExamenBl examenBl, ITipoMuestraBl tipoMuestraBl, IMaterialBl materialBl)
        {
            _plantillaBl = plantillaBl;
            _listaBl = listaBl;
            _examenBl = examenBl;
            _tipoMuestraBl = tipoMuestraBl;
            _materialBl = materialBl;
        }

        public PlantillaController() : this(new PlantillaBl(), new ListaBl(), new ExamenBl(), new TipoMuestraBl(), new MaterialBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para la busqueda y mantenimiento de las plantillas.
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

            var plantillas = _plantillaBl.ObtenerPlantillas(search);
            var pageOfPlantillas = plantillas.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = search;

            return View(pageOfPlantillas);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de ingreso de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevaPlantilla(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;

            var model = new PlantillaViewModels
            {
                Plantilla = new Plantilla(),
                TipoMuestra = GetTipoMuestraViewModels()
            };

            return PartialView("_NuevaPlantilla", model);
        }
        /// <summary>
        /// Descripción: Controlador para realizar el registro de datos de una plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevaPlantilla(int? page, string search, PlantillaViewModels model)
        {
            try
            {
                var plantilla = new Plantilla
                {
                    Nombre = model.Plantilla.Nombre,
                    Descripcion = model.Plantilla.Descripcion,
                    IdTipo = model.TipoMuestra.IdTipoMuestra,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _plantillaBl.InsertPlantilla(plantilla);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de edición de datos.
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
        public ActionResult EditarPlantilla(int id, int? page, string search)
        {
            var plantilla = _plantillaBl.GetPlantillaById(id);

            var model = new PlantillaViewModels
            {
                Plantilla = plantilla,
                TipoMuestra = GetTipoMuestraViewModels(plantilla)
            };

            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_EditarPlantilla", model);
        }
        /// <summary>
        /// Descripción: Controlador para realizar la edicion de una plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarPlantilla(int id, int? page, string search, PlantillaViewModels model)
        {
            try
            {
                var plantilla = new Plantilla
                {
                    IdPlantilla = id,
                    Nombre = model.Plantilla.Nombre,
                    Descripcion = model.Plantilla.Descripcion,
                    IdTipo = model.TipoMuestra.IdTipoMuestra,
                    Estado = 1,
                    IdUsuarioEdicion = Logueado.idUsuario
                };

                _plantillaBl.UpdatePlantilla(plantilla);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para realizar un soft delete a la plantilla seleccionada.
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
        public ActionResult EliminarPlantilla(int id, int? page, string search)
        {
            try
            {
                var plantilla = _plantillaBl.GetPlantillaById(id);

                plantilla.IdUsuarioEdicion = Logueado.idUsuario;
                plantilla.Estado = 0;

                _plantillaBl.UpdatePlantilla(plantilla);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de Establecimientos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarEstablecimientos(int id)
        {
            return RedirectToAction("EstablecimientoPlantilla", "Establecimiento", new { idPlantilla = id });
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de mantenimiento de Muestras con la lista de muestras activas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarMuestras(int id)
        {
            var plantilla = _plantillaBl.GetPlantillaById(id);
            var enfermedadExamenList = _plantillaBl.ObtenerMuestras(id);

            var muestras = enfermedadExamenList.Select(e => new ExamenMuestraViewModels
            {
                Enfermedad = new Enfermedad { idEnfermedad = e.IdEnfermedad ?? 0, nombre = e.Enfermedad },
                Examen = new Examen { idExamen = e.IdExamen ?? new Guid(), nombre = e.Examen },
                Muestra = new TipoMuestra { idTipoMuestra = e.IdTipoMuestra ?? 0, nombre = e.Muestra },
                Material = new Material
                {
                    idMaterial = e.IdMaterial ?? 0,
                    Presentacion = new Presentacion { glosa = e.Presentacion } ,
                    Reactivo = new Reactivo { glosa = e.Reactivo },
                    volumen = e.Volumen ?? 0
                },
                Cantidad = e.Cantidad
            }).ToList();

            var model = new PlantillaMuestraViewModels
            {
                Plantilla = plantilla,
                Muestras = muestras
            };

            return View("MuestraPlantilla", model);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de Muestras con la lista de muestras activas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarMuestra(int idPlantilla)
        {
            var model = new PlantillaEnfermedadExamen
            {
                IdPlantilla = idPlantilla
            };

            return PartialView("_NuevaMuestra", model);
        }
        /// <summary>
        /// Descripción: Controlador para realizar el registro de la muestra seleccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AgregarMuestra(PlantillaEnfermedadExamen model)
        {
            try
            {
                model.IdUsuarioRegistro = Logueado.idUsuario;

                _plantillaBl.InsertMuestra(model);

                TempData["UserMessage"] = "La muestra se registró correctamente.";

                return RedirectToAction("AgregarMuestras", new { id = model.IdPlantilla });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para realizar un soft delete a la muestra seleccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarMuestra(PlantillaEnfermedadExamen model)
        {
            try
            {
                model.IdUsuarioEdicion = Logueado.idUsuario;
                model.Estado = 0;

                _plantillaBl.UpdateMuestra(model);

                return RedirectToAction("AgregarMuestras", new { id = model.IdPlantilla });
            }
            catch
            {
                return View("Error");
            }
            
        }
        /// <summary>
        /// Descripción: Controlador para obtener las enfermedades y seleccionarlas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExamenes(int idEnfermedad)
        {
            var model = _examenBl.GetExamenesByIdEnfermedad(idEnfermedad, Mixto,"");

            return PartialView("_Examen", model);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar los tipos de muestra que pertencen a la enfermedad selccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTiposMuestra(string idExamen)
        {
            if (string.IsNullOrEmpty(idExamen))
                return PartialView("_Muestra", new List<TipoMuestra>());

            var model = _tipoMuestraBl.GetTiposMuestraByIdExamen(new Guid(idExamen));

            return PartialView("_Muestra", model);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar los materiales de los tipos de muestra que pertencen a la enfermedad selccionada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMaterialByTiposMuestra(int? idTipoMuestra)
        {
            if (!idTipoMuestra.HasValue)
                return PartialView("_Material", new List<Material>());

            var model = _materialBl.GetMaterialesByIdTipoMuestra(idTipoMuestra.Value);

            return PartialView("_Material", model);
        }

        [HttpPost]
        public ActionResult GetTiposMuestra()
        {
            var model = new List<TipoMuestra>();

            return PartialView("_Muestra", model);
        }
        /// <summary>
        /// Descripción: Metodo que carga las listas de la pantalla de registro/edicion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="plantilla"></param>
        /// <returns></returns>
        private TipoMuestraViewModels GetTipoMuestraViewModels(Plantilla plantilla = null)
        {
            var tipoMuestras = _listaBl.GetListaByOpcion(OpcionLista.TipoMuestra);

            return new TipoMuestraViewModels
            {
                Data = tipoMuestras.ListaDetalle,
                IdTipoMuestra = plantilla?.IdTipo ?? 0
            };
        }
    }
}
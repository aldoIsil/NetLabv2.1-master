using System;
using System.Web.Mvc;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using BusinessLayer.Compartido.Interfaces;
using BusinessLayer.DatoClinico;
using BusinessLayer.DatoClinico.Interfaces;
using Model;
using NetLab.Controllers.FormConverter;
using NetLab.Controllers.FormConverter.Entities;
using NetLab.Controllers.FormConverter.Interfaces;
using NetLab.Models;
using NetLab.Models.DatoClinico;
using NetLab.Models.Shared;
using PagedList;
using BusinessLayer.Interfaces;
using BusinessLayer;
using System.Collections.Generic;
using System.Linq;

namespace NetLab.Controllers
{
    public class DatosController : ParentController
    {
        private const int Mixto = 3;
        private readonly IDatoBl _datoBl;
        private readonly IDatoConverter _datoConverter;
        private readonly IListaBl _listaBl;

        public DatosController(IDatoBl datoBl, IDatoConverter datoConverter, IListaBl listaBl)
        {
            _datoBl = datoBl;
            _datoConverter = datoConverter;
            _listaBl = listaBl;
        }

        public DatosController() : this(new DatoBl(), new DatoConverter(), new ListaBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de busqueda y mantenimiento de los Datos de una catgeoria.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(DatoListaViewModels model)
        {
            var datos = _datoBl.GetDatoByCategoria(model.IdCategoria);

            model.Datos = datos;

            return View(model);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de ingreso de Datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idCategoria"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="nombreEnfermedad"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoDato(int idCategoria, string idEnfermedad, string nombreEnfermedad)
        {
            var listaDatoViewModel = GetListaDatoViewModels();
            var tipoDatoViewModel = GetTipoDatoViewModels();

            IProyectoBl proyectoBl = new ProyectoBl();
            var Proyectos = proyectoBl.GetProyectos();
            Proyectos.Add(new Proyecto { IdProyecto = 0, Nombre = "Mixto" });
            ViewBag.proyectoList = Proyectos;

            List<SelectListItem> examenlist = new List<SelectListItem>();// { new SelectListItem { Text = "Opcion Mixta", Value = "opcionmixta" } };
            examenlist.AddRange(this.ObtenerExamenesListItem(idEnfermedad));
            ViewBag.ExamenList = examenlist;// this.ObtenerExamenesListItem(idEnfermedad);
            var model = new DatoViewModels
            {
                IdEnfermedad = idEnfermedad,
                NombreEnfermedad = nombreEnfermedad,
                IdCategoria = idCategoria,
                Lista = listaDatoViewModel,
                Tipo = tipoDatoViewModel,
                Clase = GetClaseGeneroViewModels(Mixto),
            };

            return PartialView("_NuevoDato", model);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de Edición de Datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idCategoria"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="nombreEnfermedad"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarDato(int id, int idCategoria, string idEnfermedad, string nombreEnfermedad)
        {
            var dato = _datoBl.GetDatoById(id);

            if (dato == null) return View("Error");

            dato.IdGenero = dato.IdGenero ?? Mixto;

            IProyectoBl proyectoBl = new ProyectoBl();
            var Proyectos = proyectoBl.GetProyectos();
            Proyectos.Add(new Proyecto { IdProyecto = 0, Nombre = "Mixto" });
            ViewBag.proyectoList = Proyectos;

            List<SelectListItem> examenlist = new List<SelectListItem>();// { new SelectListItem { Text = "Opcion Mixta", Value = "opcionmixta" } };
            //if (dato.IdsExamen != "mostrartodos")
            //{
                examenlist.AddRange(this.ObtenerExamenesListItem(idEnfermedad, dato.IdsExamen));
                ViewBag.ExamenList = examenlist;// this.ObtenerExamenesListItem(idEnfermedad, dato.IdsExamen);
            //}

            var model = new DatoViewModels
            {
                Dato = dato,
                IdCategoria = idCategoria,
                IdEnfermedad = idEnfermedad,
                NombreEnfermedad = nombreEnfermedad,
                Tipo = GetTipoDatoViewModels(dato.IdTipo),
                Lista = GetListaDatoViewModels(dato.IdListaDato),
                Clase = GetClaseGeneroViewModels(dato.IdGenero),
                TipoSeleccionExamen = dato.IdsExamen == "mostrartodos"
            };

            return PartialView("_EditarDato", model);
        }
        /// <summary>
        /// Descripción: Controlador para realizar la actualización de Datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarDato(DatoViewModels model)
        {
            try
            {
                var idsExamen = model.TipoSeleccionExamen ? "mostrartodos" : model.IdsExamen.Any() ? string.Join(",", model.IdsExamen) : string.Empty;
                var datoConverterRequest = new DatoConverterRequest
                {
                    DatoViewModels = model,
                    IdUsuarioLogueado = Logueado.idUsuario
                };

                var dato = _datoConverter.ConvertFrom(datoConverterRequest);

                var datoCategoria = _datoBl.GetDatoCategoriaById(model.Dato.IdDato, model.IdCategoria);
                datoCategoria.IdUsuarioEdicion = Logueado.idUsuario;
                dato.IdsExamen = idsExamen;
                _datoBl.UpdateDato(dato, datoCategoria);

                TempData["UserMessage"] = "El dato se actualizó correctamente.";

                return RedirectToAction("Index", new { model.IdCategoria, model.IdEnfermedad, model.NombreEnfermedad });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para eliminar un Dato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idCategoria"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="nombreEnfermedad"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarDato(int id, int idCategoria, string idEnfermedad, string nombreEnfermedad)
        {
            try
            {
                var dato = _datoBl.GetDatoById(id);

                if (dato == null) return View("Error");

                dato.IdUsuarioEdicion = Logueado.idUsuario;
                dato.Estado = 0;

                var datoCategoria = _datoBl.GetDatoCategoriaById(id, idCategoria);
                datoCategoria.IdUsuarioEdicion = Logueado.idUsuario;
                datoCategoria.Estado = 0;

                _datoBl.UpdateDato(dato, datoCategoria);


                return RedirectToAction("Index", new { idCategoria, idEnfermedad, nombreEnfermedad });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador realizar el registro de un nuevo Dato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoDato(DatoViewModels model)
        {
            try
            {
                var idsExamen = model.IdsExamen.Any() ? string.Join(",", model.IdsExamen) : string.Empty;
                var datoConverterRequest = new DatoConverterRequest
                {
                    DatoViewModels = model,
                    IdUsuarioLogueado = Logueado.idUsuario
                };

                var dato = _datoConverter.ConvertFrom(datoConverterRequest);
                dato.IdsExamen = idsExamen;
                _datoBl.InsertDato(dato, model.IdCategoria);

                TempData["UserMessage"] = "El dato se registró correctamente.";

                return RedirectToAction("Index", new { model.IdCategoria, model.IdEnfermedad, model.NombreEnfermedad });
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las Lista de datos activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLista"></param>
        /// <returns></returns>
        private ListaDatoViewModels GetListaDatoViewModels(int? idLista = null)
        {
            var listas = _datoBl.GetListaDato();

            return new ListaDatoViewModels
            {
                Data = listas,
                IdListaDato = idLista ?? 0
            };
        }
        /// <summary>
        /// Descripción: Obtiene los tipo de datos activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        private TipoDatoViewModels GetTipoDatoViewModels(int? idTipo = null)
        {
            var tipos = _datoBl.GetTipos();

            return new TipoDatoViewModels
            {
                Data = tipos,
                IdTipoDato = idTipo ?? 0
            };
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las listas a mostrar .
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idGenero"></param>
        /// <returns></returns>
        private ClaseGeneroViewModels GetClaseGeneroViewModels(int? idGenero = null)
        {
            var clases = _listaBl.GetListaByOpcion(OpcionLista.ClaseGenero);

            return new ClaseGeneroViewModels
            {
                Data = clases.ListaDetalle,
                IdClase = idGenero ?? 0
            };
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la lista de las opciones existentes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ListaDatos(int? page)
        {
            var pageNumber = page ?? 1;

            var listaDatos = _datoBl.GetListaDato();
            var pageOfListaDatos = listaDatos.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            return View("ListaDatos", pageOfListaDatos);
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de ingreso de datos de Opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevaLista(int? page)
        {
            ViewBag.page = page;

            var model = new ListaDato();

            return PartialView("_NuevaLista", model);
        }
        /// <summary>
        /// Descripción: Controlador para el registro de datos de Opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevaLista(int? page, ListaDato model)
        {
            try
            {
                var listaDato = new ListaDato
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _datoBl.InsertListaDato(listaDato);

                return RedirectToAction("ListaDatos", new { page });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla la edicion de Listas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarLista(int id, int? page)
        {
            var model = _datoBl.GetListaDatoById(id);

            ViewBag.page = page;

            return PartialView("_EditarLista", model);
        }
        /// <summary>
        /// Descripción: Controlador para actualizar informacion de las listas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarLista(int id, int? page, ListaDato model)
        {
            try
            {
                model.IdListaDato = id;
                model.IdUsuarioEdicion = Logueado.idUsuario;
                model.Estado = 1;

                _datoBl.UpdateListaDato(model);

                return RedirectToAction("ListaDatos", new { page });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para la eliminación de datos de Opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarLista(int id, int? page)
        {
            try
            {
                var model = _datoBl.GetListaDatoById(id);

                model.IdUsuarioEdicion = Logueado.idUsuario;
                model.Estado = 0;

                _datoBl.UpdateListaDato(model);

                return RedirectToAction("ListaDatos", new { page });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar en pantalla con la busqueda de las opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AgregarOpciones(int id)
        {
            try
            {
                var listaDato = _datoBl.GetListaDatoById(id);

                var opciones = _datoBl.GetOpcionDatoByLista(id);

                var model = new OpcionDatoViewModels
                {
                    ListaDato = listaDato,
                    Opciones = opciones
                };

                return View("OpcionDato", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de ingreso de datos para una nueva Opción.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idListaDato"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevaOpcion(int idListaDato)
        {
            var model = new OpcionDato
            {
                IdListaDato = idListaDato
            };

            return PartialView("_NuevaOpcion", model);
        }
        /// <summary>
        /// Descripción: Controlador para realizar el registro de datos para una nueva Opción de la lista.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcionDato"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevaOpcion(OpcionDato opcionDato)
        {
            var opcion = _datoBl.GetOpcionDatoById(opcionDato.IdOpcionDato);

            if (opcion != null)
            {
                TempData["UserMessage"] = "Ya existe el id de la opción ingresada";

                return RedirectToAction("AgregarOpciones", new { id = opcionDato.IdListaDato });
            }

            opcionDato.IdUsuarioRegistro = Logueado.idUsuario;

            try
            {
                _datoBl.InsertOpcionDato(opcionDato);

                return RedirectToAction("AgregarOpciones", new { id = opcionDato.IdListaDato });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para eliminar una opción.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcionDato"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarOpcion(OpcionDato opcionDato)
        {
            opcionDato.IdUsuarioEdicion = Logueado.idUsuario;
            opcionDato.Estado = 0;

            try
            {
                _datoBl.UpdateOpcionDato(opcionDato);

                return RedirectToAction("AgregarOpciones", new { id = opcionDato.IdListaDato });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla la edicion de Opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOpcionDato"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarOpcion(int idOpcionDato)
        {
            var opcionDato =_datoBl.GetOpcionDatoById(idOpcionDato);

            return PartialView("_EditarOpcion", opcionDato);
        }
        /// <summary>
        /// Descripción: Controlador para realizar el actualziacion de de Opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcionDato"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarOpcion(OpcionDato opcionDato)
        {
            opcionDato.IdUsuarioEdicion = Logueado.idUsuario;
            opcionDato.Estado = 1;

            try
            {
                _datoBl.UpdateOpcionDato(opcionDato);

                return RedirectToAction("AgregarOpciones", new { id = opcionDato.IdListaDato });
            }
            catch
            {
                return View("Error");
            }
        }

        private List<SelectListItem> ObtenerExamenesListItem(string idEnfermedad, string seleccionados = "")
        {
            var examenBl = new ExamenBl();
            List<Examen> examenList = new List<Examen>();
            if (!string.IsNullOrWhiteSpace(idEnfermedad))
            {
                examenList = examenBl.GetExamenesByIdEnfermedad(int.Parse(idEnfermedad), 0, string.Empty);
            }

            List<Guid> examenIds = new List<Guid>();
            if (!string.IsNullOrWhiteSpace(seleccionados) && seleccionados != "mostrartodos")
            {
                seleccionados.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => examenIds.Add(Guid.Parse(x)));
            }

            var examenlist = examenList.Select(x => new SelectListItem { Value = x.idExamen.ToString(), Text = x.nombre, Selected=examenIds.Any(s=> s == x.idExamen) }).ToList();

            return examenlist;
        }
    }
}
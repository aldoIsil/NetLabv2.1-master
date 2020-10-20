using System;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Interfaces;
using Model;
using NetLab.Models;
using PagedList;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using NetLab.Models.Shared;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections;
using DataLayer;

namespace NetLab.Controllers
{
    public class AnalitoController : ParentController
    {
        private readonly IExamenBl _examenBl;
        private readonly IAnalitoBl _analitoBl;
        private readonly IAnalitoExamenBl _analitoExamenBl;

        public AnalitoController(IAnalitoExamenBl analitoExamenBl, IExamenBl examenBl, IAnalitoBl analitoBl)
        {
            _analitoExamenBl = analitoExamenBl;
            _examenBl = examenBl;
            _analitoBl = analitoBl;
        }

        public AnalitoController() : this(new AnalitoExamenBl(), new ExamenBl(), new AnalitoBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para obtener información de analitos de un examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AnalitoExamen(Guid idExamen)
        {
            try
            {
                var examen = _examenBl.GetExamenById(idExamen);

                var analitos = _analitoExamenBl.GetAnalitoByExamenId(idExamen);

                var model = new AnalitoExamenViewModels
                {
                    Examen = examen,
                    Analitos = analitos
                };

                return View("AnalitoExamen", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para realizar la busqueda de los analitos que tiene un examen.
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
        public ActionResult AgregarAnalitos(int? page, string search, Guid idExamen)
        {
            var pageNumber = page ?? 1;
            var searchCriteria = search ?? string.Empty;
            var analitos = _analitoBl.GetAnalitosByNombre(searchCriteria);
            var pageOfAnalitos = analitos.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = searchCriteria;
            ViewBag.idExamen = idExamen;

            return PartialView("_AgregarAnalito", pageOfAnalitos);
        }
        /// <summary>
        /// Descripción: Controlador para realizar la eliminacion de los analitos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarAnalito(ExamenAnalito model)
        {
            model.IdUsuarioEdicion = Logueado.idUsuario;

            try
            {
                _analitoExamenBl.UpdateAnalitoByExamen(model);

                return RedirectToAction("AnalitoExamen", new { idExamen = model.IdExamen });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para registrar los analitos que tiene un examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <param name="analitos"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GuardarAnalito(Guid idExamen, string[] analitos)
        {
            var examen = _examenBl.GetExamenById(idExamen);
            var idUsuario = Logueado.idUsuario;

            _analitoExamenBl.AgregarAnalitosPorExamen(examen, analitos, idUsuario);

            return RedirectToAction("AnalitoExamen", new { idExamen });
        }
        /// <summary>
        /// Descripción: Controlador para realizar la busqueda de analitos.
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
            var analitosList = _analitoBl.SearchAnalitos("");
            ArrayList nombres = new ArrayList();
            foreach (var item in analitosList)
            {
                nombres.Add(item.Nombre.ToString().ToUpper());
            }
            ViewBag.nombresLista = JsonConvert.SerializeObject(nombres);

            var pageNumber = page ?? 1;
            var searchCriteria = search ?? string.Empty;

            var analitos = _analitoBl.SearchAnalitos(searchCriteria);

            var pageOfAnalitos = analitos.ToPagedList(pageNumber, GetSetting<int>(PageSize));

            ViewBag.search = searchCriteria;

            return View(pageOfAnalitos);
        }
        /// <summary>
        /// Descripción: Controlador para realizar el registro de los analitos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoAnalito(int? page, string search)
        {
            ListaBl listaBL = new ListaBl();
            var tipos = listaBL.GetListaByOpcion(OpcionLista.OpcionesDeRespuesta);

            var unidades = listaBL.GetListaByOpcion(OpcionLista.Unidades);



            ViewBag.page = page;
            ViewBag.search = search;

            var @default = new Analito
            {
            };

            var model = new AnalitoViewModels
            {
                Analito = @default,
                TiposRespuesta = new ListaDetalleViewModels
                {
                    Data = tipos.ListaDetalle

                },
                Unidades = new ListaDetalleViewModels
                {
                    Data = unidades.ListaDetalle
                }
            };
            Session["tipo"] = tipos.ListaDetalle;
            return PartialView("_NuevoAnalito", model);
        }
        /// <summary>
        /// Descripción: Controlador para registrar un nuevo analito.
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
        public ActionResult NuevoAnalito(int? page, string search, AnalitoViewModels model)
        {
            try
            {
                var analito = new Analito
                {
                    Nombre = model.Analito.Nombre,
                    Descripcion = model.Analito.Descripcion,
                    Tipo = model.Analito.Tipo,
                    IdListaUnidad = model.Analito.IdListaUnidad,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _analitoBl.RegistrarAnalito(analito);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para editar un analito.
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
        public ActionResult EditarAnalito(Guid id, int? page, string search)
        {
            var analito = _analitoBl.GetAnalitoById(id);
            ListaBl listaBL = new ListaBl();
            var tipos = listaBL.GetListaByOpcion(OpcionLista.OpcionesDeRespuesta);
            var unidades = listaBL.GetListaByOpcion(OpcionLista.Unidades);

            var model = new AnalitoViewModels
            {
                Analito = analito,
                TiposRespuesta = new ListaDetalleViewModels
                {
                    Data = tipos.ListaDetalle
                },
                Unidades = new ListaDetalleViewModels
                {
                    Data = unidades.ListaDetalle
                }
            };


            ViewBag.estados = new List<SelectListItem>{
                new SelectListItem{ Text="Inactivo", Value = "0" },
                new SelectListItem{ Text="Activo", Value = "1" }
            };

            ViewBag.page = page;
            ViewBag.search = search;
            ViewBag.id = id;

            return PartialView("_EditarAnalito", model);
        }
        /// <summary>
        /// Descripción: Controlador para editar un analito.
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
        public ActionResult EditarAnalito(Guid id, int? page, string search, AnalitoViewModels model)
        {
            /*try
            {*/
            var analito = new Analito
            {
                IdAnalito = id,
                Nombre = model.Analito.Nombre,
                Descripcion = model.Analito.Descripcion,
                Tipo = model.Analito.Tipo,
                IdListaUnidad = model.Analito.IdListaUnidad,
                Estado = model.Analito.Estado,
                IdUsuarioEdicion = Logueado.idUsuario
            };

            _analitoBl.ActualizarAnalito(analito);

            return RedirectToAction("Index", new { page, search });
            /*}
            catch
            {
                return View("Error");
            }*/
        }
        /// <summary>
        /// Descripción: Controlador para eliminar un analito.
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
        public ActionResult EliminarAnalitoItem(Guid id, int? page, string search)
        {
            try
            {
                var analito = _analitoBl.GetAnalitoById(id);

                analito.IdUsuarioEdicion = Logueado.idUsuario;
                analito.Estado = 0;

                _analitoBl.ActualizarAnalito(analito);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla correspondiente
        /// a la verificacion de los analitos de un metodo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AnalitoMetodo(Guid id)
        {
            try
            {
                var analito = _analitoBl.GetAnalitoById(id);

                var metodos = _analitoBl.GetMetodosByAnalito(id);

                var model = new MetodosAnalitoViewModels
                {
                    Analito = analito,
                    Metodos = metodos
                };

                return View("AnalitoMetodo", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para registrar un nuevo metodo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoMetodo(Guid idAnalito)
        {
            var @default = new ExamenMetodo
            {
                IdAnalito = idAnalito
            };

            var model = new AnalitoMetodoViewModels
            {
                Metodo = @default,
            };

            return PartialView("_NuevoMetodo", model);
        }
        /// <summary>
        /// Descripción: Controlador para registrar un nuevo metodo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoMetodo(Guid idAnalito, AnalitoMetodoViewModels model)
        {
            try
            {
                var metodo = new ExamenMetodo
                {
                    Glosa1 = model.Metodo.Glosa1,
                    Glosa2 = model.Metodo.Glosa2,
                    Orden = model.Metodo.Orden,
                    IdAnalito = idAnalito,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _analitoBl.RegistrarMetodo(metodo);

                Guid id = idAnalito;
                return RedirectToAction("AnalitoMetodo", new { id });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para editar nuevo metodo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoMetodo"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarMetodo(Guid idAnalito, int idAnalitoMetodo, AnalitoMetodoViewModels model)
        {
            try
            {
                var metodo = new ExamenMetodo
                {
                    IdAnalito = idAnalito,
                    IdAnalitoMetodo = idAnalitoMetodo,
                    Glosa1 = model.Metodo.Glosa1,
                    Glosa2 = model.Metodo.Glosa2,
                    Orden = model.Metodo.Orden,
                    Estado = 1,
                    IdUsuarioEdicion = Logueado.idUsuario
                };

                _analitoBl.ActualizarMetodo(metodo);

                Guid id = idAnalito;
                return RedirectToAction("AnalitoMetodo", new { id });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para obtener información de las opciones de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        //public ActionResult AnalitoOpcion(Guid id)
        //{
        //    try
        //    {
        //        var analito = _analitoBl.GetAnalitoById(id);

        //        var opciones = _analitoBl.GetOpcionesByAnalito(id);

        //        var model = new OpcionesAnalitoViewModels
        //        {
        //            Analito = analito,
        //            Opciones = opciones
        //        };

        //        return View("AnalitoOpcion", model);
        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }
        //}
        public ActionResult AnalitoOpcion(Guid id)
        {
            try
            {
                var analito = _analitoBl.GetAnalitoById(id);

                var opciones = _analitoBl.GetOpcionesByAnalito(id);

                var model = new OpcionesAnalitoViewModels
                {
                    Analito = analito,
                    Opciones = opciones
                };

                return View("AnalitoOpcion", model);
            }
            catch
            {
                return View("Error");
            }
        }


        /// <summary>
        /// Descripción: Controlador para obtener información de las opciones de un analito.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public OpcionesAnalitoViewModels AnalitoOpciones(Guid id)
        {
            try
            {
                var analito = _analitoBl.GetAnalitoById(id);

                var opciones = _analitoBl.GetOpcionesByAnalito(id);

                var model = new OpcionesAnalitoViewModels
                {
                    Analito = analito,
                    Opciones = opciones
                };

                return model;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Descripción: Controlador para registrar una opcion mas para un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 12/12/2017.
        /// Modificación: sotero bustamante agrego IdOpcionParen para configuracion de resultados.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevaOpcion(Guid idAnalito, string IdOpcionParent)
        {
            ListaBl listaBL = new ListaBl();
            ExamenDal dal = new ExamenDal();
            List<SelectListItem> ListexP = new List<SelectListItem>();
            string listPf = dal.VerListaPlataforma();
            if (listPf.Length > 0)
            {
                string[] lista = listPf.Split('|');
                string[] campo;
                for (int i = 0; i < lista.Length; i++)
                {
                    campo = lista[i].Split(',');
                    ListexP.Add(new SelectListItem() { Text = campo[1].ToString(), Value =campo[0].ToString() });
                }
            }
            var @default = new AnalitoOpcionResultado
            {
                IdAnalito = idAnalito,
                IdAnalitoOpcionParent = IdOpcionParent,
                listPlataformas = ListexP
            };
            var tipos = listaBL.GetListaByOpcion(OpcionLista.OpcionesDeRespuesta);
            var model = new AnalitoOpcionViewModels
            {
                TiposRespuesta = new ListaDetalleViewModels
                {
                    Data = tipos.ListaDetalle
                },
                Opcion = @default,
            };

            return PartialView("_NuevaOpcion", model);
        }

        /// <summary>
        /// Descripción: Controlador para registrar una opcion de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 12/12/2017.
        /// Modificación: sotero bustamante agrego IdOpcionParen para configuracion de resultados.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevaOpcion(Guid idAnalito, AnalitoOpcionViewModels model, string IdOpcionParent)
        {
            try
            {
                var Opcion = new AnalitoOpcionResultado
                {
                    Glosa = model.Opcion.Glosa,
                    Orden = model.Opcion.Orden,
                    IdAnalito = idAnalito,
                    IdUsuarioRegistro = Logueado.idUsuario,
                    IdAnalitoOpcionParent = IdOpcionParent,
                    Tipo = model.Opcion.Tipo,
                    Plataforma = (model.Opcion.idPlataforma == null)? "0": string.Join(",", model.Opcion.idPlataforma)
                };

                _analitoBl.RegistrarOpcion(Opcion);

                Guid id = idAnalito;
                return RedirectToAction("AnalitoOpcion", new { id });
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para editar información de las opciones de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoOpcion"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarOpcion(Guid idAnalito, int idAnalitoOpcion)
        {
            ListaBl listaBL = new ListaBl();

            ExamenDal dal = new ExamenDal();
            List<SelectListItem> ListexP = new List<SelectListItem>();
            string listPf = dal.VerListaPlataforma();
            if (listPf.Length > 0)
            {
                string[] lista = listPf.Split('|');
                string[] campo;
                for (int i = 0; i < lista.Length; i++)
                {
                    campo = lista[i].Split(',');
                    ListexP.Add(new SelectListItem() { Text = campo[1].ToString(), Value = campo[0].ToString() });
                }
            }

            var Opcion = _analitoBl.GetOpcionById(idAnalito, idAnalitoOpcion);
            Opcion.listPlataformas = ListexP;

            var tipos = listaBL.GetListaByOpcion(OpcionLista.OpcionesDeRespuesta);
            var model = new AnalitoOpcionViewModels
            {
                Opcion = Opcion,
                TiposRespuesta = new ListaDetalleViewModels
                {
                    Data = tipos.ListaDetalle
                },
            };

            ViewBag.estados = new List<SelectListItem>{
                new SelectListItem{ Text="Inactivo", Value = "0" },
                new SelectListItem{ Text="Activo", Value = "1" }
            };

            return PartialView("_EditarOpcion", model);
        }
        /// <summary>
        /// Descripción: Controlador para editar información de las opciones de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoOpcion"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarOpcion(Guid idAnalito, int idAnalitoOpcion, AnalitoOpcionViewModels model, string IdOpcionParent)
        {
            try
            {
                var Opcion = new AnalitoOpcionResultado
                {
                    IdAnalito = idAnalito,
                    IdAnalitoOpcionResultado = idAnalitoOpcion,
                    Glosa = model.Opcion.Glosa,
                    Orden = model.Opcion.Orden,
                    Estado = model.Opcion.Estado,
                    IdUsuarioEdicion = Logueado.idUsuario,
                    IdAnalitoOpcionParent = IdOpcionParent,
                    Tipo = model.Opcion.Tipo,
                    Plataforma = (model.Opcion.idPlataforma == null) ? "0" : string.Join(",", model.Opcion.idPlataforma)
                };

                _analitoBl.ActualizarOpcion(Opcion);

                Guid id = idAnalito;
                return RedirectToAction("AnalitoOpcion", new { id });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para eliminar información de las opciones de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoOpcion"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarOpcion(Guid idAnalito, int idAnalitoOpcion)
        {
            try
            {
                var Opcion = _analitoBl.GetOpcionById(idAnalito, idAnalitoOpcion);

                Opcion.IdUsuarioEdicion = Logueado.idUsuario;
                Opcion.Estado = 0;

                _analitoBl.ActualizarOpcion(Opcion);

                Guid id = idAnalito;
                return RedirectToAction("AnalitoOpcion", new { id });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para obtener información de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AnalitoValor(Guid id)
        {
            try
            {
                var analito = _analitoBl.GetAnalitoById(id);

                var valores = _analitoBl.GetValoresByAnalito(id);

                var model = new ValoresAnalitoViewModels
                {
                    Analito = analito,
                    Valores = valores
                };

                return View("AnalitoValor", model);
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para registrar información  de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoValor(Guid idAnalito)
        {
            ListaBl listaBL = new ListaBl();
            var generos = listaBL.GetListaByOpcion(OpcionLista.GeneroPersona);
            var analito = _analitoBl.GetAnalitoById(idAnalito);
            var opciones = _analitoBl.GetOpcionesByAnalito(idAnalito);

            var @default = new AnalitoValorNormal
            {
                idAnalito = idAnalito
            };

            var model = new AnalitoValorViewModels
            {
                Valor = @default,
                Analito = analito,
                Opciones = opciones,
                Generos = new ListaDetalleViewModels
                {
                    Data = generos.ListaDetalle
                }
            };

            return PartialView("_NuevoValor", model);
        }
        /// <summary>
        /// Descripción: Controlador para registrar información  de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoValor(Guid idAnalito, AnalitoValorViewModels model)
        {
            Analito analito = _analitoBl.GetAnalitoById(idAnalito);
            try
            {
                var Valor = new AnalitoValorNormal
                {
                    glosa = model.Valor.glosa,
                    orden = model.Valor.orden,
                    idLista = model.Valor.idLista,
                    grupoGenero = model.Valor.grupoGenero,
                    edadInferior = model.Valor.edadInferior,
                    edadSuperior = model.Valor.edadSuperior,
                    valorInferior = model.Valor.valorInferior,
                    valorSuperior = model.Valor.valorSuperior,
                    valorAlarmaInferior = model.Valor.valorAlarmaInferior,
                    valorAlarmaSuperior = model.Valor.valorAlarmaSuperior,
                    idAnalito = idAnalito,

                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _analitoBl.RegistrarValor(Valor);

                Guid id = idAnalito;
                return RedirectToAction("AnalitoValor", new { id });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para editar información  de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoValor"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarValor(Guid idAnalito, int idAnalitoValor)
        {
            ListaBl listaBL = new ListaBl();
            var generos = listaBL.GetListaByOpcion(OpcionLista.GeneroPersona);
            var analito = _analitoBl.GetAnalitoById(idAnalito);
            var Valor = _analitoBl.GetValorById(idAnalito, idAnalitoValor);
            var opciones = _analitoBl.GetOpcionesByAnalito(idAnalito);

            var model = new AnalitoValorViewModels
            {
                Valor = Valor,
                Analito = analito,
                Opciones = opciones,
                Generos = new ListaDetalleViewModels
                {
                    Data = generos.ListaDetalle
                }
            };

            ViewBag.estados = new List<SelectListItem>{
                new SelectListItem{ Text="Inactivo", Value = "0" },
                new SelectListItem{ Text="Activo", Value = "1" }
            };

            return PartialView("_EditarValor", model);
        }
        /// <summary>
        /// Descripción: Controlador para editar información  de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoValor"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarValor(Guid idAnalito, int idAnalitoValor, AnalitoValorViewModels model)
        {
            try
            {
                var Valor = new AnalitoValorNormal
                {
                    idAnalito = idAnalito,
                    idAnalitoValorNormal = idAnalitoValor,
                    glosa = model.Valor.glosa,
                    orden = model.Valor.orden,
                    edadInferior = model.Valor.edadInferior,
                    edadSuperior = model.Valor.edadSuperior,
                    valorInferior = model.Valor.valorInferior,
                    valorSuperior = model.Valor.valorSuperior,
                    valorAlarmaInferior = model.Valor.valorAlarmaInferior,
                    valorAlarmaSuperior = model.Valor.valorAlarmaSuperior,
                    idLista = model.Valor.idLista,
                    grupoGenero = model.Valor.grupoGenero,
                    Estado = model.Valor.Estado,
                    IdUsuarioEdicion = Logueado.idUsuario
                };

                _analitoBl.ActualizarValor(Valor);

                Guid id = idAnalito;
                return RedirectToAction("AnalitoValor", new { id });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para eliminar información  de un analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoValor"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarValor(Guid idAnalito, int idAnalitoValor)
        {
            try
            {
                var valor = _analitoBl.GetValorById(idAnalito, idAnalitoValor);

                valor.IdUsuarioEdicion = Logueado.idUsuario;
                valor.Estado = 0;

                _analitoBl.ActualizarValor(valor);

                Guid id = idAnalito;
                return RedirectToAction("AnalitoValor", new { id });
            }
            catch
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Sotero Bustamante
        /// </summary>
        /// <param name="IdOrden"></param>
        /// <param name="CodigoOrden"></param>
        /// <returns></returns>
        public ActionResult DetalleOpciones(List<AnalitoOpcionResultado> NewOpcionesList, string idOpcionParent)
        {

            List<AnalitoOpcionResultado> LstOpcionDetalle = new List<AnalitoOpcionResultado>();
            List<AnalitoOpcionResultado> LstOpcionDetalles = new List<AnalitoOpcionResultado>();

            //LstOpcionDetalle = NewOpcionesList;

            foreach (var item in NewOpcionesList)
            {
                if (item.IdAnalitoOpcionParent.Trim() == idOpcionParent)
                {
                    LstOpcionDetalle.Add(item);
                }
                else
                {
                    LstOpcionDetalles.Add(item);
                }

                //nombres.Add(item.Nombre.ToString().ToUpper());
            }
            ViewBag.IdParent = idOpcionParent;
            ViewBag.ListaDetalles = LstOpcionDetalles;
            return PartialView("_DetalleOpciones", LstOpcionDetalle);

            //return PartialView("_DetalleOpciones", NewOpcionesList);

        }

        public ActionResult AnalitoInterpretacion(Guid idAnalito)
        {
            List<ExamenResultadoInterpretacion> interpretacion = new List<ExamenResultadoInterpretacion>();
            AnalitoBl _interpretacion = new AnalitoBl();
            var analito = _interpretacion.GetAnalitoById(idAnalito);
            interpretacion = _interpretacion.ObtenerAnalitoInterpretacion(idAnalito);
            ViewBag.idAnalito = idAnalito;
            ViewBag.Analito = analito.Nombre.ToString();
            return View("AnalitoInterpretacion", interpretacion);
        }

        public ActionResult NuevaInterpretacion(string idAnalito)
        {
            ExamenResultadoInterpretacion interpretacion = new ExamenResultadoInterpretacion();
            interpretacion.idAnalito = idAnalito;
            return PartialView("_NuevaInterpretacion",interpretacion);
        }

        public ActionResult CrearNuevaInterpretacion(string idAnalito, string GlosaParent, string Glosa,string Interpretacion)
        {
            try
            {
                AnalitoBl interpretacion = new AnalitoBl();
                interpretacion.RegistrarNuevaInterpretacion(idAnalito, GlosaParent, Glosa, Interpretacion, Logueado.idUsuario);
                return RedirectToAction("AnalitoInterpretacion", new { idAnalito = idAnalito });
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult ObtenerAnalitoInterpretacionPorId(int idInterpretacion)
        {
            ExamenResultadoInterpretacion interpretacion = new ExamenResultadoInterpretacion();
            AnalitoBl _interpretacion = new AnalitoBl();
            interpretacion = _interpretacion.ObtenerAnalitoInterpretacionPorId(idInterpretacion);
            return PartialView("_EditarAnalitoInterpretacion", interpretacion);
        }

        public ActionResult EditarAnalitoInterpretacion(int idInterpretacion, string idAnalito, string GlosaParent, string Glosa, string Interpretacion, int estado)
        {
            try
            {
                AnalitoBl _interpretacion = new AnalitoBl();
                _interpretacion.EditarAnalitoInterpretacion(idInterpretacion, GlosaParent, Glosa, Interpretacion, estado, Logueado.idUsuario);
                return RedirectToAction("AnalitoInterpretacion", new { idAnalito = idAnalito });
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
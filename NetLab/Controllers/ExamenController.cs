using System;
using System.Web.Mvc;
using BusinessLayer;
using PagedList;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using BusinessLayer.Compartido.Interfaces;
using BusinessLayer.Interfaces;
using Model;
using NetLab.Models;
using NetLab.Models.Shared;
using System.Collections;
using Newtonsoft.Json;
using DataLayer;
using System.Collections.Generic;

namespace NetLab.Controllers
{
    public class ExamenController : ParentController
    {
        private readonly IExamenBl _examenBl;
        private readonly IListaBl _listaBl;
        private readonly ITipoMuestraBl _tipoMuestraBl;
        private readonly IExamenTipoMuestraBl _examenTipoMuestraBl;

        public ExamenController(IListaBl listaBl, IExamenBl examenBl, IExamenTipoMuestraBl examenTipoMuestraBl, ITipoMuestraBl tipoMuestraBl)
        {
            _listaBl = listaBl;
            _examenBl = examenBl;
            _examenTipoMuestraBl = examenTipoMuestraBl;
            _tipoMuestraBl = tipoMuestraBl;
        }

        public ExamenController() : this(new ListaBl(), new ExamenBl(), new ExamenTipoMuestraBl(), new TipoMuestraBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para la busqueda de examenes.
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
            var searchCriteria = search ?? string.Empty;
            var examenesList = _examenBl.GetExamenesByNombre("");
            var examenes = _examenBl.GetExamenesByNombre(searchCriteria);
            var pageOfExamenes = examenes.ToPagedList(pageNumber, GetSetting<int>(PageSize));



            ArrayList nombres = new ArrayList();
            ArrayList CPT = new ArrayList();
            ArrayList LOINC = new ArrayList();
            foreach (var item in examenesList)
            {
                string loincvar = "";

                string nom = item.nombre.ToString().ToUpper();
                string cptvar = item.Cpt.ToString().ToUpper();

                try
                {
                    loincvar = item.Loinc.ToString().ToUpper();
                }
                catch (Exception)
                {

                    loincvar="";
                }
               

                nombres.Add(nom);
                CPT.Add(cptvar);
                LOINC.Add(loincvar);
            }


            ViewBag.nombresLista = JsonConvert.SerializeObject(nombres);
            ViewBag.CPTLista = JsonConvert.SerializeObject(CPT);
            ViewBag.LOINCLista = JsonConvert.SerializeObject(LOINC);




            ViewBag.search = searchCriteria;

            return View(pageOfExamenes);
        }

        /// <summary>
        /// Descripción: Controlador para msotrar la ventana de ingreso de datos de examenes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoExamen(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;
            
            var @default = new Examen
            {
                IdGenero = 3
            };

            var model = new ExamenViewModels
            {
                Examen = @default,
                Clase = GetClaseGeneroViewModels(@default)
            };

            return PartialView("_NuevoExamen", model);
        }
        /// <summary>
        /// Descripción: Controlador para el registro de examenes.
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
        public ActionResult NuevoExamen(int? page, string search, ExamenViewModels model)
        {
            try
            {
                var examen = new Examen
                {
                    nombre = model.Examen.nombre,
                    descripcion = model.Examen.descripcion,
                    Cpt = model.Examen.Cpt,
                    Loinc = model.Examen.Loinc,
                    IdGenero = model.Clase.IdClase,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _examenBl.InsertExamen(examen);

                return RedirectToAction("Index", new {page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de edición de examenes.
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
        public ActionResult EditarExamen(Guid id, int? page, string search)
        {
            var examen = _examenBl.GetExamenById(id);
            examen.IdGenero = examen.IdGenero ?? 3;

            var model = new ExamenViewModels
            {
                Examen = examen,
                Clase = GetClaseGeneroViewModels(examen)
            };

            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_EditarExamen", model);
        }
        /// <summary>
        /// Descripción: Controlador para la edición de examenes.
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
        public ActionResult EditarExamen(Guid id, int? page, string search, ExamenViewModels model)
        {
            try
            {
                var examen = new Examen
                {
                    idExamen = id,
                    nombre = model.Examen.nombre,
                    descripcion = model.Examen.descripcion,
                    Cpt = model.Examen.Cpt,
                    Loinc = model.Examen.Loinc,
                    IdGenero = model.Clase.IdClase,
                    Estado = model.Examen.Estado,
                    IdUsuarioEdicion = Logueado.idUsuario
                };

                _examenBl.UpdateExamen(examen);

                return RedirectToAction("Index", new {page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para la eliminación(edición) de examenes.
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
        public ActionResult EliminarExamen(Guid id, int? page, string search)
        {
            try
            {
                var examen = _examenBl.GetExamenById(id);

                examen.IdUsuarioEdicion = Logueado.idUsuario;
                examen.Estado = 0;

                _examenBl.UpdateExamen(examen);

                return RedirectToAction("Index", new {page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrará la pantalla de registro de enfermedades.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarEnfermedades(Guid id)
        {
            return RedirectToAction("EnfermedadExamen", "Enfermedad", new { idExamen = id });
        }
        /// <summary>
        /// Descripción: Controlador para mostrará la pantalla de registro de analitos o componentes..
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarAnalitos(Guid id)
        {
            return RedirectToAction("AnalitoExamen", "Analito", new { idExamen = id });
        }
        /// <summary>
        /// Descripción: Controlador para mostrará la pantalla de busqueda y seleccion de un tipos de muestra por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarTiposDeMuestra(Guid id)
        {
            var examen = _examenBl.GetExamenById(id);
            var tiposDeMuestra = _examenTipoMuestraBl.GetTipoMuestraByExamen(id);

            ArrayList codigoMuestra = new ArrayList();

            foreach (var item in tiposDeMuestra)
            {
                codigoMuestra.Add(item.TipoMuestra.nombre.ToString().ToUpper());
            }
            ViewBag.nombresLista = JsonConvert.SerializeObject(codigoMuestra);

            var model = new ExamenTipoMuestraViewModels
            {
                Examen = examen,
                TiposDeMuestra = tiposDeMuestra
            };

            return View("ExamenTipoMuestra", model);
        }
        /// <summary>
        /// Descripción: Controlador para mostrará la pantalla para seleccionar y agregar tipos de muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarTipoMuestra(Guid idExamen)
        {
            var model = new ExamenTipoMuestra
            {
                IdExamen = idExamen
            };

            return PartialView("_AgregarTipoMuestra", model);
        }
        /// <summary>
        /// Descripción: Controlador para registrar tipos de muestra por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarTipoMuestra(ExamenTipoMuestra model)
        {
            model.IdUsuarioRegistro = Logueado.idUsuario;

            _examenTipoMuestraBl.InsertTipoMuestraByExamen(model);

            return PartialView("_AgregarTipoMuestra", model);

            //  return RedirectToAction("AgregarTiposDeMuestra", new { id = model.IdExamen });
        }
        /// <summary>
        /// Descripción: Controlador para actualziar tipos de muestra por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarTipoMuestra(int idTipoMuestra, Guid idExamen)
        {
            var examenTipoMuestra = new ExamenTipoMuestra
            {
                IdTipoMuestra = idTipoMuestra,
                IdExamen = idExamen,
                IdUsuarioEdicion = Logueado.idUsuario,
                Estado = 0
            };

            try
            {
                _examenTipoMuestraBl.UpdateTipoMuestraByExamen(examenTipoMuestra);

                return RedirectToAction("AgregarTiposDeMuestra", new { id = idExamen });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para obtener tipos de muestra por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetTiposDeMuestra()
        {
            var data = Request.Params["data[q]"];

            var muestras = _tipoMuestraBl.GetTipoMuestras(data);

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeMuestra = false;

            foreach (var muestra in muestras)
            {
                resultado += "{\"id\":\"" + muestra.idTipoMuestra + "\",\"text\":\"" + muestra.nombre + "\"},";
                existeMuestra = true;
            }

            if (existeMuestra)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            return resultado;
        }
        /// <summary>
        /// Descripción: Controlador para obtener metodos seleccionados y registrarlos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarMetodos(Guid id)
        {
            var examen = _examenBl.GetExamenById(id);
            var metodos = _examenBl.GetMetodoByExamen(id);

            ArrayList nombres = new ArrayList();

            foreach (var item in metodos)
            {
                nombres.Add(item.Glosa.ToString().ToUpper());
            }

            ViewBag.nombresLista = JsonConvert.SerializeObject(nombres);


            var model = new ExamenMetodoViewModels
            {
                Examen = examen,
                Metodos = metodos
            };

            return View("ExamenMetodo", model);
        }
        /// <summary>
        /// Descripción: Controlador para registrar metodos por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idexamen"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarMetodo(Guid idexamen)
        {
            var model = new ExamenMetodo
            {
                IdExamen = idexamen
            };

            return PartialView("_AgregarMetodo", model);
        }
        /// <summary>
        /// Descripción: Controlador para eliminar(actualizar) metodos por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamenMetodo"></param>
        /// <param name="idExamen"></param>
        /// <param name="glosa"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarMetodo(int idExamenMetodo, Guid idExamen, string glosa, int orden)
        {
            var examenMetodo = new ExamenMetodo
            {
                IdExamenMetodo = idExamenMetodo,
                IdExamen = idExamen,
                Glosa = glosa,
                Orden = orden,
                IdUsuarioEdicion = Logueado.idUsuario,
                Estado = 0
            };

            try
            {
                _examenBl.UpdateMetodoByExamen(examenMetodo);

                return RedirectToAction("AgregarMetodos", new { id = idExamen });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para obtener el detalle del Genero.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examen"></param>
        /// <returns></returns>
        private ClaseGeneroViewModels GetClaseGeneroViewModels(Examen examen = null)
        {
            var clases = _listaBl.GetListaByOpcion(OpcionLista.ClaseGenero);

            return new ClaseGeneroViewModels
            {
                Data = clases.ListaDetalle,
                IdClase = examen?.IdGenero ?? 0
            };
        }
        /// <summary>
        /// Descripción: Controlador para registrar metodo por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarMetodo(ExamenMetodo model)
        {
            model.IdUsuarioRegistro = Logueado.idUsuario;

            _examenBl.InsertMetodoByExamen(model);

            return RedirectToAction("AgregarMetodos", new { id = model.IdExamen });
        }

        #region ExamenPlataforma
        public ActionResult ExamenPlataforma(Guid id, string examen)
        {
            List<ExamenPlataforma> ListexP = new List<ExamenPlataforma>();
            ExamenDal dal = new ExamenDal();
            string plataformaExamen = dal.VerListaPlataformaExamen(id);
            if (plataformaExamen.Length > 0)
            {
                string[] lista = plataformaExamen.Split('|');
                string[] campo;
                for (int i = 0; i < lista.Length; i++)
                {
                    ExamenPlataforma exP = new ExamenPlataforma();
                    campo = lista[i].Split(',');
                    exP.idExamen = id;
                    exP.Examen = examen;
                    exP.idPlataforma = Convert.ToInt32(campo[0]);
                    exP.Plataforma = campo[1].ToString();
                    ListexP.Add(exP);
                }
            }
            else
            {
                ExamenPlataforma exP = new ExamenPlataforma();
                exP.idExamen = id;
                exP.Examen = examen;
                ListexP.Add(exP);
            }
            return View(ListexP);
        }
        public string VerListaPlataforma()
        {
            ExamenDal dal = new ExamenDal();
            string plataforma = dal.VerListaPlataforma();
            return plataforma;
        }
        public string RegistrarExamenPlataforma(string[] idPlataforma, Guid idExamen)
        {
            string plataforma = string.Join(",", idPlataforma);
            ExamenDal dal = new ExamenDal();
            string estado = dal.RegistrarExamenPlataforma(plataforma, idExamen, Logueado.idUsuario);
            return estado;
        }

        public string EliminarExamenPlataforma(int idPlataforma, Guid idExamen)
        {
            ExamenDal dal = new ExamenDal();
            string estado = dal.EliminarExamenPlataforma(idPlataforma, idExamen, Logueado.idUsuario);
            return estado;
        }
        #endregion
    }
}

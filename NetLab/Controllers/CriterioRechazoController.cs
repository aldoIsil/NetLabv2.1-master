using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Compartido;
using BusinessLayer.Compartido.Enums;
using BusinessLayer.Compartido.Interfaces;
using BusinessLayer.Interfaces;
using Model;
using NetLab.Models.CriterioRechazo;
using PagedList;
using System.Collections;
using Newtonsoft.Json;

namespace NetLab.Controllers
{
    public class CriterioRechazoController : ParentController
    {
        private readonly IListaBl _listaBl;
        private readonly ICriterioRechazoBl _criterioRechazoBl;

        public CriterioRechazoController(IListaBl listaBl, ICriterioRechazoBl criterioRechazoBl)
        {
            _listaBl = listaBl;
            _criterioRechazoBl = criterioRechazoBl;
        }

        public CriterioRechazoController() : this(new ListaBl(), new CriterioRechazoBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para la busqueda de Criterios.
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

            var criteriosList = _criterioRechazoBl.GetCriterios("");
            ArrayList nombres = new ArrayList();
            foreach (var item in criteriosList)
            {
                nombres.Add(item.Glosa.ToString().ToUpper());
            }


            ViewBag.nombresLista = JsonConvert.SerializeObject(nombres);


            var pageNumber = page ?? 1;

            var criterios = _criterioRechazoBl.GetCriterios(search != null ? search.Trim() : null);
            //Alexander Buchelli - inicio - fecha 7/12/18 -SE CAMBIO EL VALOR PAGE SIZE1 
            var pageOfCriterios = criterios.ToPagedList(pageNumber, GetSetting<int>(PageSize1));

            ViewBag.search = search;

            return View(pageOfCriterios);
        }
        /// <summary>
        /// Descripción: Controlador para el manejo de registro de un nuevo criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoCriterio(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;

            var model = new CriterioViewModels
            {
                Criterio = new CriterioRechazo(),
                TipoCriterio = GetTipoCriterioViewModels()
            };

            return PartialView("_NuevoCriterio", model);
        }
        /// <summary>
        /// Descripción: Controlador para el manejo de registro de un nuevo criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoCriterio(int? page, string search, CriterioViewModels model)
        {
            try
            {
                var criterioRechazo = new CriterioRechazo
                {
                    Glosa = model.Criterio.Glosa,
                    Tipo = model.TipoCriterio.IdTipoCriterio,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _criterioRechazoBl.InsertCriterio(criterioRechazo);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para el manejo de modificacion de un  criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarCriterio(int id, int? page, string search)
        {
            var criterio = _criterioRechazoBl.GetCriterioById(id);

            var model = new CriterioViewModels
            {
                Criterio = criterio,
                TipoCriterio = GetTipoCriterioViewModels(criterio)
            };

            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_EditarCriterio", model);
        }
        /// <summary>
        /// Descripción: Controlador para el manejo de modificacion de un  criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarCriterio(int id, int? page, string search, CriterioViewModels model)
        {
            try
            {
                var criterio = new CriterioRechazo
                {
                    IdCriterioRechazo = id,
                    Glosa = model.Criterio.Glosa,
                    Tipo = model.TipoCriterio.IdTipoCriterio,
                    Estado = model.Criterio.Estado,
                    IdUsuarioEdicion = Logueado.idUsuario
                };

                _criterioRechazoBl.UpdateCriterio(criterio);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }

        /// <summary>
        /// Descripción: Controlador para el manejo de eliminación de un  criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EliminarCriterio(int id, int? page, string search)
        {
            try
            {
                var criterio = _criterioRechazoBl.GetCriterioById(id);

                criterio.IdUsuarioEdicion = Logueado.idUsuario;
                criterio.Estado = 0;

                _criterioRechazoBl.UpdateCriterio(criterio);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para obtener informacion de un  criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        private TipoCriterioViewModels GetTipoCriterioViewModels(CriterioRechazo criterio = null)
        {
            var tipoCriterio = _listaBl.GetListaByOpcion(OpcionLista.TipoCriterioRechazo);

            return new TipoCriterioViewModels
            {
                Data = tipoCriterio.ListaDetalle,
                IdTipoCriterio = criterio?.Tipo ?? 0
            };
        }
    }
}
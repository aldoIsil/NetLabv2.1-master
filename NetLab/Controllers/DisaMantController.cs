using BusinessLayer;
using BusinessLayer.Interfaces;
using Model;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class DisaMantController : ParentController
    {
        private readonly IDisaBI  _idDisabl;




        public DisaMantController(IDisaBI idDisabl)
        {
            _idDisabl = idDisabl;
            
        }

        public DisaMantController() : this(new DisaMantBl())
        {
        }

        // GET: DisaMant
        /// <summary>
        /// /// <summary>
        /// Descripción: Muestra pantalla para la busqueda y mantenimiento de una disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="disa"></param>
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="busqueda"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int? page, string search, int? busqueda)
        {
            var disalist = _idDisabl.GetDisas("");
            ArrayList nombres = new ArrayList();
            ArrayList ids = new ArrayList();

            foreach (var item in disalist)
            {
                nombres.Add(item.NombreDISA.ToString().ToUpper());
            }

            foreach (var item in disalist)
            {
                ids.Add(item.IdDISA.ToString().Trim().ToUpper());
            }


            ViewBag.nombresLista = JsonConvert.SerializeObject(nombres);
            ViewBag.idsLista = JsonConvert.SerializeObject(ids);

            if ((page == null) && (search == null) && (busqueda == null))
                return View();
            else
            {
                var pageNumber = page ?? 1;
                var searchCriteria = search ?? string.Empty;

                var disas = _idDisabl.GetDisas(searchCriteria);
                var pageOfDisas = disas.ToPagedList(pageNumber, GetSetting<int>(PageSize));

                ViewBag.search = searchCriteria;

                return View(pageOfDisas);
            }
        }
        /// <summary>
        /// Descripción: Muestra la pantalla para ingresar unaa nueva disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.        
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoDisaMant(int? page, string search) {
            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_NuevoDisaMant");

        }
        /// <summary>
        /// Descripción: Ingresar una nueva disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="chkActivoRol"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoDisaMant(int? page, string search, string chkActivoRol, FormCollection collection) {
            try
            {
                var disaMant = new DisaMant
                {
                    IdDISA = collection["IdDISA"],
                    NombreDISA = collection["NombreDISA"],
                    Estado = chkActivoRol == "false" ? 0 : 1,
                    IdUsuarioRegistro = Logueado.idUsuario
                };
                _idDisabl.InsertDisa(disaMant);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }

        }

        /// <summary>
        /// Descripción: Muestra la pantalla para editar una disa.
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
        public ActionResult EditarDisaMant(string id, int? page, string search)
        {
          var disaMant = _idDisabl.GetDisaByID(id);

            ViewBag.page = page;
            ViewBag.search = search;

            return PartialView("_EditarDisaMant",disaMant);
        }
        /// <summary>
        /// Descripción: Actualizar una nueva disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="chkActivoRol"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarDisaMant(string id, int? page, string search, bool chkActivoRol, FormCollection collection) {

            int est = 9;

            if (chkActivoRol)
            {
                est = 1;
            }
            else
            {
                est = 0;
            }

            try
            {
                var disaMant = new DisaMant
                {
                    IdDISA = collection["IdDISA"],
                    NombreDISA = collection["NombreDISA"],
                    Estado = est,
                    IdUsuarioRegistro = Logueado.idUsuario
                };
                
                _idDisabl.UpdateDisa(disaMant);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
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
    public class RedController : ParentController
    {
        private readonly IRedBl _idRedbl;

        private readonly IDisaBI _idDisabl;

        public RedController(IRedBl idRedbl, IDisaBI idDisabl)
        {
            _idRedbl = idRedbl;
            _idDisabl = idDisabl;
        }

        public RedController() : this(new RedMantBl(),new DisaMantBl())
        {
        }
        /// <summary>
        /// Descripción: Metodo para la carga de datos de las DISAS.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public string GetGetDisas()
        {
            var data = Request.Params["data[q]"];

            //IDisaBI idDisabl = new IDisaBI();

            var disasList = _idDisabl.GetDisas(data);

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeDisa = false;

            foreach (var dis in disasList)
            {
                var text = dis.IdDISA;
                var snomed = dis.NombreDISA;

                //  if (snomed != null && snomed.Contains(data))
                text = snomed + " -  " + text;

                resultado += "{\"id\":\"" + dis.IdDISA + "\",\"text\":\"" + text + "\"},";
                existeDisa = true;
            }

            if (existeDisa)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            //Session["enfermedadList"] = existeDisa;

            return resultado;
        }

        // GET: Red
        /// <summary>
        /// Descripción: Controlador para la mostrar la busqueda y el mantenimiento de Redes
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="busqueda"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index(int? page, string search, int? busqueda)
        {
            var redList = _idRedbl.GetRedes("");

            ArrayList listidDisaIdRed = new ArrayList();

            foreach (var item in redList)
            {
                listidDisaIdRed.Add(item.idDisa.ToString().ToUpper().Trim() + item.idred.ToString().ToUpper().Trim());
            }


            ArrayList listnombres = new ArrayList();

            foreach (var item in redList)
            {
                listnombres.Add(item.nombrered.ToString().ToUpper().Trim());
            }
        

            ViewBag.nombresLista = JsonConvert.SerializeObject(listidDisaIdRed);
            ViewBag.nombresredLista = JsonConvert.SerializeObject(listnombres);

            if ((page == null) && (search == null) && (busqueda == null)) {
                return View();
            }              
            else
            {
                var pageNumber = page ?? 1;
                var searchCriteria = search ?? string.Empty;

                var redes = _idRedbl.GetRedes(searchCriteria);
                var pageOfRedes = redes.ToPagedList(pageNumber, GetSetting<int>(PageSize));

                ViewBag.search = searchCriteria;

                return View(pageOfRedes);
            }

        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de ingreso de una nueva Red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoRedMant(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;
            return PartialView("_NuevoRedMant");

        }

        /// <summary>
        /// Descripción: Controlador para ejecutar el registro de una Red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="idred"></param>
        /// <param name="nombrered"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoRedMant(int? page, string search,String idEnfermedad,string idred,string nombrered)
        {
            try
            {
                var redMant = new RedMant
                {
                    idDisa =idEnfermedad.Trim(),
                    idred = idred.Trim(),
                    nombrered = nombrered.Trim(),
                    Estado=1,
                    IdUsuarioRegistro = Logueado.idUsuario
                };


                _idRedbl.InsertRedes(redMant);

                return RedirectToAction("Index", new { page, search });
            }
            catch (Exception e)
            {
                string msj = e.Message;
                return View("Error");

                
            }
        }

        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla con la informacion a editar de las redes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="iddisa"></param>
        /// <param name="idred"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarRedMant(string iddisa,string idred, int? page, string search)
        {
            var redMant = _idRedbl.GetRedByID(iddisa,idred);

            ViewBag.page = page;
            ViewBag.search = search;

            ViewBag.valueEnfermedadPreSeleccionada = redMant.idDisa;
            ViewBag.textoEnfermedadPreSeleccionada = redMant.idDisa + " - " + redMant.nombredisa;

            Session["iddisa"]= redMant.idDisa;
            Session["idred"] = redMant.idred;
            ViewBag.textoIdRed = redMant.idred.ToString();

            //eviar model despues del view
            return PartialView("_EditarRedMant", redMant);
        }
        /// <summary>
        /// Descripción: Controlador para ejecutar la edicion de datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="chkActivoRol"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="nombrered"></param>
        /// <returns></returns>
        [HttpPost]
       public ActionResult EditarRedMant( int? page, string search,bool chkActivoRol, String idEnfermedad,string nombrered)
        {
            int est = 9;
       
            string iddisaIn = Session["iddisa"].ToString();
            string idredIN   =Session["idred"].ToString();

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
                var redMant = new RedMant
                {
                    idDisa = iddisaIn.Trim(),
                    idred = idredIN.Trim(),
                    nombrered = nombrered.Trim(),
                    Estado = est,
                    IdUsuarioRegistro = Logueado.idUsuario
                };
                
                _idRedbl.UpdateRedes(redMant);

                return RedirectToAction("Index", new { page, search });
            }
            catch (Exception e)
            {
                string msj = e.Message;
                return View("Error");
            }
        }
    }
}
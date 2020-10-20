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
    public class MicroRedController : ParentController
    {
        private readonly IDisaBI _idDisabl;

        private readonly IRedBl _idRedbl;

        private readonly IMicroRedBI _idMicroRedbl;



        public MicroRedController(IDisaBI idDisabl,IRedBl idRedbl, IMicroRedBI idMicroRedbl)
        {
            _idRedbl = idRedbl;
            _idDisabl = idDisabl;
            _idMicroRedbl = idMicroRedbl;
        }

        public MicroRedController() : this( new DisaMantBl(), new RedMantBl(),new MicroRedBl())
        {
        }

        /// <summary>
        /// Descripción: Metodo para obtener lista de las disas.
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

        /// <summary>
        /// Descripción: Obtiene informacion de las redes filtradoo por el de la Disa,
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public ActionResult GetredesById(string[] idDisa)
        {
            List<RedMant> RedesList = new List<RedMant>();
            List<String> idExamenList = new List<String>();
            if (idDisa != null)
            {
                IRedBl redBl = new RedMantBl();
                for (int i = 0; i < idDisa.Count(); i++)
                {
                    idExamenList.Add(idDisa[i].ToString());
                }

                string idDisaParamn = idExamenList[0].ToString();


                RedesList = redBl.GetRedByIDDisa(idDisaParamn);
            }
            
            var model = RedesList;
            return PartialView("_Redes", model);
        }

        // GET: MICRORED
        /// <summary>
        /// Descripción: Muestra la pantalla con  la sbusqueda de las microredes y su mantenimiento.
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
            var microredList = _idMicroRedbl.GetMicroRedes("");

            ArrayList microredListName = new ArrayList();

            ArrayList microredListNameMicrored = new ArrayList();


            foreach (var item in microredList)
            {
                microredListName.Add(item.iddisa.Trim() + item.idred.Trim() + item.idmicrored.Trim());
            }

            foreach (var item in microredList)
            {
                microredListNameMicrored.Add(item.nombremicrored.ToUpper().Trim() );
            }


            ViewBag.nombresLista = JsonConvert.SerializeObject(microredListName);
            ViewBag.nombreListaMicrored= JsonConvert.SerializeObject(microredListNameMicrored);

            if ((page == null) && (search == null) && (busqueda == null))
            {
                return View();
            }
            else
            {
                var pageNumber = page ?? 1;
                var searchCriteria = search ?? string.Empty;

                var microredes = _idMicroRedbl.GetMicroRedes(searchCriteria);
                var pageOfMicroRedes = microredes.ToPagedList(pageNumber, GetSetting<int>(PageSize));

                ViewBag.search = searchCriteria;

                return View(pageOfMicroRedes);
            }
        }

        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de ingreso de datos de la microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoMicroRedMant(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;
            return PartialView("_NuevoMicroRedMant");
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla y agregar los datos de la microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="idRed"></param>
        /// <param name="idmicrored"></param>
        /// <param name="nombremicrored"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoMicroRedMant(int? page, string search,string idEnfermedad, string idRed,string idmicrored, string nombremicrored)
        {
            try
            {
                var microredMant = new MicroRedMant
                {
                    iddisa = idEnfermedad.Trim(),
                    idred = idRed.Trim(),
                    idmicrored = idmicrored.Trim(),
                    nombremicrored = nombremicrored.Trim(),
                    Estado = 1,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _idMicroRedbl.InsertMicroRedes(microredMant);
                //_idRedbl.InsertRedes(MicroredMant);

                return RedirectToAction("Index", new { page, search });
            }
            catch (Exception e)
            {
                string msj = e.Message;
                return View("Error");
            }
        }

        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla y editar los datos de la microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="iddisa"></param>
        /// <param name="idred"></param>
        /// <param name="idmicrored"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarMicroRedMant(string iddisa, string idred, string idmicrored, int? page, string search)
        {
            var MicroredMant = _idMicroRedbl.GetMicroRedByID(iddisa, idred, idmicrored);

            ViewBag.page = page;
            ViewBag.search = search;


            Session["iddisa"] = MicroredMant.iddisa;
            Session["idred"] = MicroredMant.idred;
            Session["idmicrored"] = MicroredMant.idmicrored;


            ViewBag.textoIdDisa = MicroredMant.nombredisa.ToString();
            ViewBag.textoIdRed = MicroredMant.nombrered.ToString();
            ViewBag.textoIdMicroRed = MicroredMant.idmicrored.ToString();

            //eviar model despues del view
            return PartialView("_EditarMicroRedMant", MicroredMant);
        }

        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla y editar los datos de la microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="nombremicrored"></param>
        /// <param name="chkActivoRol"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarMicroRedMant( int? page, string search,string nombremicrored, bool chkActivoRol)
        {
            int est = 9;

            ViewBag.page = page;
            ViewBag.search = search;
            
            if (chkActivoRol)
            {
                est = 1;
            }
            else
            {
                est = 0;

            }
            
            string iddisaIn =  Session["iddisa"].ToString();
            string idredIN = Session["idred"].ToString();
            string microidredIN =  Session["idmicrored"].ToString();

            try
            {
                var microredMant = new MicroRedMant
                {
                    iddisa = iddisaIn.Trim(),
                    idred = idredIN.Trim(),
                    idmicrored = microidredIN.Trim(),
                    nombremicrored = nombremicrored.Trim(),
                    Estado = est,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _idMicroRedbl.UpdateMicroRedes(microredMant);
                //_idRedbl.InsertRedes(MicroredMant);

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
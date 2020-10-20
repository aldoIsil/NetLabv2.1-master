using System.Web.Mvc;
using BusinessLayer;
using NetLab.Models;


namespace NetLab.Controllers
{
    public class DisaController : ParentController
    {
        private readonly DISABl _disaBl = new DISABl();
        /// <summary>
        /// Descripción: Obtiene información de las redes filtrado por la disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idDISA"></param>
        /// <returns></returns>
        public ActionResult GetRedesOption(string idDISA)
        {
            var model = new RedesViewModels
            {
                Data = _disaBl.GetRedes(idDISA)
            };

            return PartialView("_RedesOption", model);
        }
        /// <summary>
        /// Descripción: COntrolador que muestra la pantalla de inicio y busqueda.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index() {

            return View();
        }
        /// <summary>
        /// Descripción: Obtiene información de las microredes filtrado por la disa y la red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idDISA"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public ActionResult GetMicroRedesOption(string idDISA, string idRed)
        {
            var model = new MicroRedesViewModels
            {
                Data = _disaBl.GetMicroRedes(idDISA, idRed)
            };

            return PartialView("_MicroRedesOption", model);
        }
    }
}
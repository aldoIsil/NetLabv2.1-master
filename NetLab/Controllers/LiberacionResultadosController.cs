using BusinessLayer;
using NetLab.Models;
using System;
using System.Web.Mvc;
using PagedList;
using System.Linq;
using BusinessLayer.Interfaces;
using Model;

namespace NetLab.Controllers
{
    public class LiberacionResultadosController : ParentController
    {
        protected int itemsPorPag = 20;
        /// <summary>
        /// Descripción: Controlador para msotrar la pantalla con la busqueda correspondiente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.IsSearch = false;
            //return Index(1, "", "1", null, null, "", "", "","0",Logueado.idUsuario);
            return Index(1, "1", null, null, "", "", "", "0",1);
            //return View();
        }

        /// <summary>
        /// /// Descripción: Controlador para msotrar la pantalla con la busqueda correspondiente.LIBERAR RESULTADOS
        /// Author: SOTERO BUSTAMANTE
        /// Fecha: 28/10/2017
        /// </summary>
        /// <param name="page"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nroOficio"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="tipoOrden"></param>
        /// <returns></returns>
        [HttpPost]
        //public ActionResult Index(int? page, string search, string esFechaRegistro, string fechaDesde, string fechaHasta, string nroDocumento, string nroOficio, string codigoOrden, string tipoOrden, int idUsuarioIngreso)
        public ActionResult Index(int? page, string esFechaRegistro, string fechaDesde, string fechaHasta, string nroDocumento, string nroOficio, string codigoOrden, string tipoOrden, int EstadoLiberacion)
        {
            //var pageNumber = page ?? 1;
            //var searchCriteria = search ?? string.Empty;

            ViewBag.IsSearch = true;

            DateTime fechaDesdeA = new DateTime();
            DateTime fechaHastaA = new DateTime();
            int fechaRegistro = 0;

            var fechaDesdeCriteria = fechaDesde ?? string.Empty;
            var fechaHastaCriteria = fechaHasta ?? string.Empty;
            if (!fechaDesdeCriteria.Equals(""))
                fechaDesdeA = DateTime.Parse(fechaDesdeCriteria);
            if (!fechaHastaCriteria.Equals(""))
                fechaHastaA = DateTime.Parse(fechaHastaCriteria);

            var fechaRegistroTmp = esFechaRegistro ?? string.Empty;
            if (!fechaRegistroTmp.Equals(""))
                fechaRegistro = int.Parse(fechaRegistroTmp);
            else if (fechaDesdeCriteria.Equals("") || fechaHastaCriteria.Equals(""))
                fechaRegistro = 0;
            else if (!fechaDesdeCriteria.Equals("") && !fechaHastaCriteria.Equals(""))
                if (fechaDesdeA == fechaHastaA)
                {
                    fechaHastaA.AddDays(1);
                }
            var codigoOrdenF = codigoOrden ?? string.Empty;
            var nroOficioA = nroOficio ?? string.Empty;
            var nroDocumentoA = nroDocumento ?? string.Empty;

            var validaResultBl = new ValidaResultBl();
            //var validaciones = validaResultBl.GetValidaciones(idUsuarioIngreso,fechaRegistro,codigoOrden,
            //fechaDesdeA,fechaHastaA,nroOficio,nroDocumento, EstablecimientoSeleccionado.IdEstablecimiento);
            var validaciones = validaResultBl.GetValidacionesSolicitudes(Logueado.idUsuario, fechaRegistro, codigoOrdenF,
                fechaDesdeA, fechaHastaA, nroOficioA, nroDocumentoA, EstablecimientoSeleccionado.IdEstablecimiento, EstadoLiberacion);
            //var filtered = validaciones.Where(pr => pr.Establecimiento.ToUpper().Contains(searchCriteria.ToUpper()));
            //var pageOfPresentacion = filtered.ToPagedList(pageNumber, GetSetting<int>(PageSize));
            //ViewBag.search = searchCriteria;

            return View(validaciones);

        }


        /// <summary>
        /// Descripción: Controlador para realizar la validacion de los resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]        
        public ActionResult ValidarResultados(Guid id, int? page, string search)
        {
            var pageNumber = page ?? 1;
            var searchCriteria = search ?? string.Empty;

            ViewBag.IsSearch = true;

            var liberadoBl = new LiberadosBl();
            var orden = liberadoBl.GetOrdenById(id);
            return PartialView("LiberarResultados", orden);

        }

        [HttpPost]
        public ActionResult ValidacionResultados(int? page, Guid id)
        {
            return null;

        }
        /// <summary>
        /// Descripción: Controlador para realizar la LIBERACION de los resultados.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 28/10/2017
        /// Fecha Modificación: 28/10/2017.
        /// Modificación: Se agregaron LIBERACION DE RESULTADOS
        /// </summary>
        /// <param name="idValResul"></param>
        /// <returns></returns>
        public ActionResult Update(String[] idValResul)
        {

            int tamanioFor = idValResul.Count();

            LiberadosBl liberadoBl = new LiberadosBl();

            for (int i = 0; i < tamanioFor; i++)
            {
                String valida = this.Request.Params["result" + idValResul[i]];

                if (valida != null)
                {
                    Liberados liber = new Liberados();
                    liber.idOrdenExamen = Guid.Parse(idValResul[i]);
                    liber.idUsuarioLiberado = ((Usuario)this.Session["UsuarioLogin"]).idUsuario;
                    liber.liberado = 2;
                    liber.observacion = "";
                    liberadoBl.UpdateDatosLiberados(liber);

                }
            }
            return View();
        }
    }
}
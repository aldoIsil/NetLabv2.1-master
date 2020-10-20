using BusinessLayer;
using NetLab.Models;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class UbigeoPacienteController : Controller
    {
        private readonly UbigeoPacienteBl _ubigeoBl = new UbigeoPacienteBl();
        /// <summary>
        /// Descripción: Controlador para la presentacion de los departamentos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="departamentoSelectId"></param>
        /// <param name="provinciaSelectId"></param>
        /// <param name="distritoSelectId"></param>
        /// <param name="provinciaDivId"></param>
        /// <param name="distritoDivId"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public ActionResult GetDepartamentos(string departamentoSelectId, string provinciaSelectId, string distritoSelectId, string provinciaDivId, string distritoDivId, string selectedValue = null)
        {
            var model = new DepartamentoViewModels
            {
                Data = _ubigeoBl.GetDepartamentos(),
                DepartamentoSelectId = departamentoSelectId,
                ProvinciaSelectId = provinciaSelectId,
                DistritoSelectId = distritoSelectId,
                ProvinciaDivId = provinciaDivId,
                DistritoDivId = distritoDivId,
                UrlProvinciasPorDepartamento = Url.Action("GetProvinciasPorDepartamento"),
                UrlDistritosPorProvincia = Url.Action("GetDistritosPorProvincia"),
                SelectedValue = selectedValue
            };

            return PartialView("_Departamento", model);
        }
        /// <summary>
        /// Descripción: Controlador para la presentacion de las provincias por departamento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="departamentoSelectId"></param>
        /// <param name="provinciaSelectId"></param>
        /// <param name="distritoSelectId"></param>
        /// <param name="distritoDivId"></param>
        /// <param name="codigoDepartamento"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public ActionResult GetProvinciasPorDepartamento(string departamentoSelectId, string provinciaSelectId, string distritoSelectId, string distritoDivId, string codigoDepartamento = "01", string selectedValue = null)
        {
            var model = new ProvinciaViewModels
            {
                Data = _ubigeoBl.GetProvincias(codigoDepartamento),
                DepartamentoSelectId = departamentoSelectId,
                ProvinciaSelectId = provinciaSelectId,
                DistritoSelectId = distritoSelectId,
                DistritoDivId = distritoDivId,
                UrlDistritosPorProvincia = Url.Action("GetDistritosPorProvincia"),
                SelectedValue = selectedValue
            };

            return PartialView("_Provincia", model);
        }
        /// <summary>
        /// Descripción: Controlador para la presentacion de los Distritos por Provincia.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="distritoSelectId"></param>
        /// <param name="codigoDepartamento"></param>
        /// <param name="codigoProvincia"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public ActionResult GetDistritosPorProvincia(string distritoSelectId, string codigoDepartamento = "01", string codigoProvincia = "01", string selectedValue = null)
        {
            var model = new DistritoViewModels
            {
                Data = _ubigeoBl.GetDistritos(codigoDepartamento, codigoProvincia),
                DistritoSelectId = distritoSelectId,
                SelectedValue = selectedValue
            };

            return PartialView("_Distrito", model);
        }
    }
}
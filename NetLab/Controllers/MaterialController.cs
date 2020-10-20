using System;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Interfaces;
using Model;
using NetLab.Models;
using NetLab.Models.Shared;
using PagedList;
using System.Collections.Generic;

namespace NetLab.Controllers
{
    public class MaterialController : ParentController
    {
        private readonly IMaterialBl _materialBl;
        private readonly ITipoMuestraBl _tipoMuestraBl;
        private readonly IPresentacionBl _presentacionBl;
        private readonly IReactivoBl _reactivoBl;

        public MaterialController(IMaterialBl materialBl, ITipoMuestraBl tipoMuestraBl, IPresentacionBl presentacionBl, IReactivoBl reactivoBl)
        {
            _materialBl = materialBl;
            _tipoMuestraBl = tipoMuestraBl;
            _presentacionBl = presentacionBl;
            _reactivoBl = reactivoBl;
        }

        public MaterialController() : this(new MaterialBl(), new TipoMuestraBl(), new PresentacionBl(), new ReactivoBl())
        {
        }
        /// <summary>
        /// Descripción: Controlador para mostrar y seleccionar un material.
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
            var mensaje = 0;

            if ((page == null) && (search == null) && (busqueda == null))
                return View();
            else
            {
                var pageNumber = page ?? 1;
                var searchCriteria = search ?? string.Empty;

                var materiales = _materialBl.GetMateriales(search);
                var pageOfMateriales = materiales.ToPagedList(pageNumber, GetSetting<int>(PageSize));

                ViewBag.search = searchCriteria;

                if (materiales.Count == 0)
                {
                    mensaje = 1;
                    ViewBag.mensaje = mensaje;
                }
                else
                {
                    mensaje = 2;
                    ViewBag.mensaje = mensaje;

                }


                return View(pageOfMateriales);
            }

        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de registro de un material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NuevoMaterial(int? page, string search)
        {
            ViewBag.page = page;
            ViewBag.search = search;

            var tipoMuestraViewModels = GetTipoMuestraViewModels();
            var presentacionViewModels = GetPresentacionViewModelsByMuestra();
            var reactivoViewModels = GetReactivoViewModelsByPresentacion();

            var model = new MaterialViewModels
            {
                TipoMuestra = tipoMuestraViewModels,
                Presentacion = presentacionViewModels,
                Reactivo = reactivoViewModels
            };

            return PartialView("_NuevoMaterial");
        }
        /// <summary>
        /// Descripción: Controlador para el registro de un material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="idExamen"></param>
        /// <param name="idPresentacion"></param>
        /// <param name="idReactivo"></param>
        /// <param name="volumen"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult NuevoMaterial(int? page, string search, string  idExamen, string idPresentacion, string idReactivo , string volumen)
        {
            try
            {
                var material = new Material
                {
                    IdTipoMuestra = int.Parse(idExamen),
                    IdPresentacion =int.Parse(idPresentacion),
                    IdReactivo = int.Parse(idReactivo),
                    volumen = Convert.ToDecimal(volumen),
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _materialBl.InsertMaterial(material);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de edicion de un material
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
        public ActionResult EditarMaterial(int id, int? page, string search)
        {
           var Materialcar=  _materialBl.GetMaterialById(id);

            ViewBag.nombretipomuestra = Materialcar.TipoMuestra.nombre.ToString();
            ViewBag.nombrepresentacion = Materialcar.Presentacion.glosa.ToString();
            ViewBag.nombrereactivo = Materialcar.Reactivo.glosa.ToString();


            Session["idtipomuestra"] = Materialcar.IdTipoMuestra.ToString();
            Session["idpresentacion"] = Materialcar.IdPresentacion.ToString();
            Session["idreactivo"] = Materialcar.IdReactivo.ToString();


            Session["idMaterial"] = Materialcar.idMaterial.ToString();
            return PartialView("_EditarMaterial", Materialcar);

        }
        /// <summary>
        /// Descripción: Controlador para editar el registro de un material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="search"></param>
        /// <param name="volumen"></param>
        /// <param name="chkActivoRol"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditarMaterial(int? page, string search, string volumen, bool chkActivoRol) {
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

            int idmaterial = int.Parse(Session["idMaterial"].ToString());

            int idtipomuestra = int.Parse(Session["idtipomuestra"].ToString());
            int idpresentacion = int.Parse(Session["idpresentacion"].ToString());
            int idreactivo = int.Parse(Session["idreactivo"].ToString());


            try
            {
                var material = new Material
                {
                    IdTipoMuestra = idtipomuestra,
                    IdPresentacion = idpresentacion,
                    IdReactivo = idreactivo,
                    idMaterial= idmaterial,
                    volumen = Convert.ToDecimal(volumen),
                    estado=est,
                    IdUsuarioRegistro = Logueado.idUsuario
                };

                _materialBl.UpdateMaterial(material);

                return RedirectToAction("Index", new { page, search });
            }
            catch (Exception)
            {

                return View("Error");
            }
        }

        /// <summary>
        /// Descripción: Controlador para eliminar(actualizar) de registro de un material.
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
        public ActionResult EliminarMaterial(int id, int? page, string search)
        {
            try
            {
                var material = _materialBl.GetMaterialById(id);

                material.IdUsuarioEdicion = Logueado.idUsuario;
                material.Estado = 0;

                _materialBl.UpdateMaterial(material);

                return RedirectToAction("Index", new { page, search });
            }
            catch
            {
                return View("Error");
            }
        }
        /// <summary>
        /// Descripción: Controlador para mostrar la pantalla de registro de la presentacion de un material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPresentacionPorMuestra(string idTipoMuestra)
        {
            var id = !string.IsNullOrEmpty(idTipoMuestra) ? Convert.ToInt32(idTipoMuestra) : (int?)null;

            var presentacionViewModels = GetPresentacionViewModelsByMuestra(id);

            return PartialView("_Presentacion", presentacionViewModels);
        }
        /// <summary>
        /// Descripción: Obtiene registros de la presentacion de un material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPresentacion"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReactivoPorPresentacion(string idPresentacion)
        {
            var id = !string.IsNullOrEmpty(idPresentacion) ? Convert.ToInt32(idPresentacion) : (int?)null;

            var reactivoViewModels = GetReactivoViewModelsByPresentacion(id);

            return PartialView("_Reactivo", reactivoViewModels);
        }

        public string getTiposMuestras() {

            var data = Request.Params["data[q]"];

            //IDisaBI idDisabl = new IDisaBI();

            var muestrasList = _tipoMuestraBl.GetTipoMuestras(data);

            var resultado = "{\"q\":\"" + data + "\",\"results\":[";

            var existeMuestra = false;

            foreach (var muestras in muestrasList)
            {
                var text = muestras.nombre;
                var snomed = muestras.nombre;

                //  if (snomed != null && snomed.Contains(data))
                //text = snomed + " -  " + text;

                resultado += "{\"id\":\"" + muestras.idTipoMuestra + "\",\"text\":\"" + text + "\"},";
                existeMuestra = true;
            }

            if (existeMuestra)
                resultado = resultado.Substring(0, resultado.Length - 1) + "]}";
            else
                resultado = resultado.Substring(0, resultado.Length) + "]}";

            //Session["enfermedadList"] = existeDisa;

            return resultado;

        }
        /// <summary>
        /// Descripción: Obtiene registros de la presentacion de un material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public ActionResult GetPresentacionesByMuestra(string[] idExamen)
        {
            List<Presentacion> PresentacionList = new List<Presentacion>();
            List<String> idtipomuestra = new List<String>();

            if (idExamen != null)
            {
                string idDisaParamn = idExamen[0].ToString();
                PresentacionList = _presentacionBl.GetPresentacionesByIdTipoMuestra(int.Parse(idDisaParamn)); 
            }
                        
            List<Presentacion> tipoMuestraListTmp = new List<Presentacion>();
            tipoMuestraListTmp.Add(new Presentacion { glosa = "" });
            foreach (Presentacion tipoMuestra in PresentacionList)
            {
                tipoMuestraListTmp.Add(tipoMuestra);
            }
            var model = tipoMuestraListTmp;
            return PartialView("_Presentacion", tipoMuestraListTmp);
        }
        /// <summary>
        /// Descripción: Obtiene registros de los reactivos de una presentacion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public ActionResult GetReactivosbyPresentacion(int idTipoMuestra)
        {
            List<Reactivo> RedesList = new List<Reactivo>();
            List<String> idreactivoList = new List<String>();
            if (idTipoMuestra != 0)      
                RedesList = _reactivoBl.GetReactivosByIdPresentacion(idTipoMuestra);

            var model = RedesList;
            return PartialView("_Reactivo", RedesList);
        }
        /// <summary>
        /// Descripción: Devuelve la entidad TipoMuestraViewModels cargada con los tipos de muestras
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        private TipoMuestraViewModels GetTipoMuestraViewModels(int? idTipoMuestra = null)
        {
            var muestras = _tipoMuestraBl.GetTipoMuestras();

            return new TipoMuestraViewModels
            {
                Data = muestras,
                IdTipoMuestra = idTipoMuestra ?? 0
            };
        }
        /// <summary>
        /// Descripción: Devuelve la entidad PresentacionViewModels cargada con las presentaciones por tipos de muestras
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <param name="idPresentacion"></param>
        /// <returns></returns>
        private PresentacionViewModels GetPresentacionViewModelsByMuestra(int? idTipoMuestra = null, int? idPresentacion = null)
        {
            var presentaciones = _presentacionBl.GetPresentacionesByIdTipoMuestra(idTipoMuestra);

            return new PresentacionViewModels
            {
                Data = presentaciones,
                IdPresentacion = idPresentacion ?? 0
            };
        }
        /// <summary>
        /// Descripción: Devuelve la entidad ReactivoViewModels cargada con las presentaciones por tipos de muestras
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPresentacion"></param>
        /// <param name="idReactivo"></param>
        /// <returns></returns>
        private ReactivoViewModels GetReactivoViewModelsByPresentacion(int? idPresentacion = null, int? idReactivo = null)
        {
            var reactivos = _reactivoBl.GetReactivosByIdPresentacion(idPresentacion);

            return new ReactivoViewModels
            {
                Data = reactivos,
                IdReactivo = idReactivo ?? 0
            };
        }
    }
}
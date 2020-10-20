using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using NetLab.Models.PEED;
using BusinessLayer.PEED;
using PagedList;

namespace NetLab.Controllers
{
    public class ConfiguracionEvalControlCalidadController : ParentController
    {
        // GET: ConfiguracionEvalControlCalidad
        /// <summary>
        /// Descripción: Metodo para realizar la busqueda de las evaluaciones registradas
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018                
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var listConfiguracionEvalControlCalidad = new List<ConfiguracionEvalControlCalidad>();
            //if ((page == null) && (fechaDesde == null) && (fechaHasta == null) && (idEnfermedad == null) && (idExamen == null) && (hddDato == null))
            //    return View();
            //else
            //{
                //var pageNumber = page ?? 1;
                var blconfigeval = new ConfiguracionEvalControlCalidadBl();
            var filtroConfigEvalCC = new ConfiguracionEvalControlCalidad();
            
                //var sDato = hddDato == "" ? "0" : hddDato;
                //var sEnfermedad = idEnfermedad == "" ? "0" : idEnfermedad;
                //var sExamen = idExamen == "" ? Guid.Empty : Guid.Parse(idExamen);
            listConfiguracionEvalControlCalidad = blconfigeval.GetConfiguracionEvalControlCalidad(filtroConfigEvalCC);
                var pageOfSeg = listConfiguracionEvalControlCalidad.ToPagedList(1, GetSetting<int>(PageSize));
                return View(pageOfSeg);
            //}
        }
        /// <summary>
        /// Descripción: Metodo para invocar y mostrar la ventana de registro de evaluaciones.
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018                
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View("Create");
        }
        /// <summary>
        /// Descripción: Metodo para el registro de las evaluaciones
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018                
        /// </summary>
        /// <param name="Nombre"></param>
        /// <param name="Descripcion"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="idEESSEvaluador"></param>
        /// <param name="idEESSEvaluado"></param>
        /// <returns></returns>
        public int AddEvaluacion(string Nombre, string Descripcion, int idEnfermedad, int idEESSEvaluador, int idEESSEvaluado)
        {
            try
            {
                return new ConfiguracionEvalControlCalidadBl().InsertConfigEval(new ConfiguracionEvalControlCalidad()
                {
                    idConfigEvaluacion = Guid.NewGuid(),
                    Nombre = Nombre,
                    Descripcion = Descripcion,
                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                    EstablecimientoEvaluador = new Establecimiento { IdEstablecimiento = idEESSEvaluador },
                    EstablecimientoEvaluado = new Establecimiento { IdEstablecimiento = idEESSEvaluado },
                    idusuarioregistro = Logueado.idUsuario
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Descripción: Metodo para mostrar la ventana de edicion de evaluaciones
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            var model = new ConfiguracionEvalControlCalidadBl().GetConfiguracionEvalControlCalidadById(new ConfiguracionEvalControlCalidad() { idConfigEvaluacion = Guid.Parse(id)});

            return View("Edit",model);
        }
        /// <summary>
        /// Descripción: Metodo para ejecutar la transaccion de modificación de las evaluaciones
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="idConfigEval"></param>
        /// <param name="Nombre"></param>
        /// <param name="Descripcion"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="idEESSEvaluador"></param>
        /// <param name="idEESSEvaluado"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public int EditEvaluacion(string idConfigEval,string Nombre, string Descripcion, int idEnfermedad, int idEESSEvaluador, int idEESSEvaluado, int estado)
        {
            try
            {
                return new ConfiguracionEvalControlCalidadBl().EditConfigEval(new ConfiguracionEvalControlCalidad()
                {
                    idConfigEvaluacion = Guid.Parse(idConfigEval),
                    Nombre = Nombre,
                    Descripcion = Descripcion,
                    Enfermedad = new Enfermedad { idEnfermedad = idEnfermedad },
                    EstablecimientoEvaluador = new Establecimiento { IdEstablecimiento = idEESSEvaluador },
                    EstablecimientoEvaluado = new Establecimiento { IdEstablecimiento = idEESSEvaluado },
                    estado = estado,
                    idusuarioedicion = Logueado.idUsuario
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Descripción: Metodo para visualizar la bandeja de paneles
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddPanel(string id, string nombre)
        {
            //Obtener informacion del Panel amarrado a la evaluacion
            var _panel = new ConfiguracionPanelControlCalidadBl().GetConfiguracionPanelControlCalidad(new ConfiguracionPanelControlCalidad() { idConfigEvaluacion = Guid.Parse(id) });
            
            var model = new EvaluacionPanelesViewModels() {
                Evaluacion = new ConfiguracionEvalControlCalidad()
                {
                    idConfigEvaluacion = Guid.Parse(id),
                    Nombre = nombre
                },
                Paneles = _panel
            };
            return View("ConfigEvalPanel", model);
        }
        /// <summary>
        /// Descripción: Metodo para el visualizar la ventan de registro de un nuevo panel
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="idConfigEvaluacion"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarPanel(Guid idConfigEvaluacion)
        {
            TipoMetodo(0);
            var model = new ConfiguracionPanelControlCalidad
            {
                idConfigEvaluacion = idConfigEvaluacion
            };

            return PartialView("_FormNuevoPanel", model);
        }
        /// <summary>
        /// Descripción: Metodo para la transaccion de registro de un panel
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="objConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public ActionResult AddEvaluacionPanel(ConfiguracionPanelControlCalidad objConfiguracionPanelControlCalidad)
        {
            try
            {
                objConfiguracionPanelControlCalidad.idusuarioregistro = Logueado.idUsuario;
                int res = new ConfiguracionPanelControlCalidadBl().InsertConfigEvalPanel(objConfiguracionPanelControlCalidad);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("AddPanel", new { id = objConfiguracionPanelControlCalidad.idConfigEvaluacion,nombre= objConfiguracionPanelControlCalidad.nombre });
        }
        /// <summary>
        /// Descripción: Metodo para mostrar la ventana de edicion de un panel
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="idConfiguracionPanel"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarPanel(Guid idConfiguracionPanel)
        {
            TipoMetodo(0);
             var model = new ConfiguracionPanelControlCalidadBl().GetConfiguracionPanelControlCalidad(new ConfiguracionPanelControlCalidad() { idConfiguracionPanel = idConfiguracionPanel }).FirstOrDefault();

            return PartialView("_FormEditarPanel", model);
        }
        /// <summary>
        /// Descripción: Metodo para la transaccion de la edicion de un panel
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="objConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public ActionResult EditEvaluacionPanel(ConfiguracionPanelControlCalidad objConfiguracionPanelControlCalidad)
        {
            try
            {
                objConfiguracionPanelControlCalidad.idusuarioedicion = Logueado.idUsuario;
                var Result = new ConfiguracionPanelControlCalidadBl().EditConfigEvalPanel(objConfiguracionPanelControlCalidad);                
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("AddPanel", new { id = objConfiguracionPanelControlCalidad.idConfigEvaluacion, nombre = objConfiguracionPanelControlCalidad.nombre });
        }
        /// <summary>
        /// Descripción: Metodo para mostrar la bandeja de materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="idConfiguracionPanel"></param>
        /// <param name="idConfigEvaluacion"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddMaterial(Guid idConfiguracionPanel,Guid idConfigEvaluacion)
        {
            //Obtener informacion del Panel amarrado a la evaluacion
            var _panel = new ConfiguracionPanelControlCalidadBl().GetConfiguracionPanelControlCalidad(new ConfiguracionPanelControlCalidad() { idConfigEvaluacion = idConfigEvaluacion, idConfiguracionPanel = idConfiguracionPanel }).FirstOrDefault();
            //Obtener informacion del Material amarrado al Panel
            var _material = new ConfiguracionMaterialControlCalidadBl().GetConfiguracionMaterialControlCalidad(new ConfiguracionMaterialControlCalidad() { idConfiguracionPanel = idConfiguracionPanel });

            ViewBag.idTipoPanel = _panel.idTipo;
            ViewBag.idTipoEvaluacionPanel = _panel.idTipoEvaluacion;

            var model = new EvaluacionPanelesViewModels()
            {
                Panel = _panel,
                Materiales = _material
            };
            return View("ConfigEvalPanelMaterial", model);
        }
        /// <summary>
        /// Descripción: Metodo para mostrar la ventana de registro de un material
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="idConfiguracionPanel"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AgregarMaterial(Guid idConfiguracionPanel)
        {
            var _panel = new ConfiguracionPanelControlCalidadBl().GetConfiguracionPanelControlCalidad(new ConfiguracionPanelControlCalidad() { idConfiguracionPanel = idConfiguracionPanel }).FirstOrDefault();
            var model = new ConfiguracionMaterialControlCalidad
            {
                idConfiguracionPanel = idConfiguracionPanel
            };
            TipoMetodo(_panel.idTipo);
            return PartialView("_FormNuevoMaterial", model);
        }
        /// <summary>
        /// Descripción: Metodo para mostrar la ventana de edicion de los materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="idConfigEvaluacionMaterial"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditarMaterial(Guid idConfigEvaluacionMaterial)
        {
            
            var model = new ConfiguracionMaterialControlCalidadBl().GetConfiguracionMaterialControlCalidad(new ConfiguracionMaterialControlCalidad() { idConfigEvaluacionMaterial = idConfigEvaluacionMaterial }).FirstOrDefault();
            TipoMetodo(model.idTipoMetodo);
            return PartialView("_FormEditarMaterial", model);
        }
        /// <summary>
        /// Descripción: Metodo para la transaccion de registro de los materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="objConfiguracionMaterialControlCalidad"></param>
        /// <returns></returns>
        public ActionResult AddEvaluacionMaterial(ConfiguracionMaterialControlCalidad objConfiguracionMaterialControlCalidad)
        {
            var newguidempty = Guid.Empty;
            try
            {
                objConfiguracionMaterialControlCalidad.idusuarioregistro = Logueado.idUsuario;
                int res = new ConfiguracionMaterialControlCalidadBl().InsertConfigEvalMaterial(objConfiguracionMaterialControlCalidad);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("AddMaterial", new { idConfiguracionPanel = objConfiguracionMaterialControlCalidad.idConfiguracionPanel, idConfigEvaluacion = newguidempty });
        }
        /// <summary>
        /// Descripción: Metodo para la transaccion de la edicion de los materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="objConfiguracionMaterialControlCalidad"></param>
        /// <returns></returns>
        public ActionResult EditarEvaluacionMaterial(ConfiguracionMaterialControlCalidad objConfiguracionMaterialControlCalidad)
        {
            var newguidempty = Guid.Empty;
            try
            {
                objConfiguracionMaterialControlCalidad.idusuarioedicion = Logueado.idUsuario;
                int res = new ConfiguracionMaterialControlCalidadBl().EditConfigEvalMaterial(objConfiguracionMaterialControlCalidad);
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("AddMaterial", new { idConfiguracionPanel = objConfiguracionMaterialControlCalidad.idConfiguracionPanel, idConfigEvaluacion = newguidempty });
        }
        /// <summary>
        /// Descripción: Metodo que retorna la lista del tipo de metodo, tipo evaluacion y resultados
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="idTipoPanel"></param>
        void TipoMetodo(int idTipoPanel)
        {
            var TipoEvaluacion = new List<SelectListItem>();
            var TipoMetodo = new List<SelectListItem>();
            var TipoMetodoMaterial = new List<SelectListItem>();

            TipoEvaluacion.Add(new SelectListItem(){Text = "Seleccionar",Value = "0"});
            TipoEvaluacion.Add(new SelectListItem(){Text = "Directo",Value = "1"});
            TipoEvaluacion.Add(new SelectListItem(){Text = "Indirecto",Value = "2"});
            ViewBag.ListTipoEvaluacion = TipoEvaluacion;            

            TipoMetodo.Add(new SelectListItem(){Text = "Seleccionar",Value = "0"});
            TipoMetodo.Add(new SelectListItem(){Text = "Baciloscopia",Value = "1"});
            TipoMetodo.Add(new SelectListItem(){Text = "Suceptibilidad-Genotype",Value = "2"});
            TipoMetodo.Add(new SelectListItem(){Text = "Medio de Cultivo",Value = "3"});
            ViewBag.ListTipoMetodo = TipoMetodo;
            //MATERIAL
            
            TipoMetodoMaterial.Add(new SelectListItem(){Text = "Seleccionar",Value = "0"});
            
            var resultado = new List<SelectListItem>();
            switch (idTipoPanel)
            {
                case 1:  //BK
                    TipoMetodoMaterial.Add(new SelectListItem()
                    {
                        Text = "RELECTURA DE DOBLE CIEGO DE LAMINAS DE BACILOSCOPIAS",
                        Value = "1"
                    });
                    //Resultado
                    resultado.Add(new SelectListItem() { Text = "NEGATIVO", Value = "1" });
                    resultado.Add(new SelectListItem() { Text = "1-9 BAR", Value = "2" });
                    resultado.Add(new SelectListItem() { Text = "1+", Value = "3" });
                    resultado.Add(new SelectListItem() { Text = "2+", Value = "4" });
                    resultado.Add(new SelectListItem() { Text = "3+", Value = "5" });
                    break;
                case 2://SUCEP-GENOTYPE
                    TipoMetodoMaterial.Add(new SelectListItem(){Text = "RIF",Value = "2"});
                    TipoMetodoMaterial.Add(new SelectListItem(){Text = "INH",Value = "3"});
                    TipoMetodoMaterial.Add(new SelectListItem(){Text = "KAT G",Value = "4"});
                    TipoMetodoMaterial.Add(new SelectListItem(){Text = "INH A",Value = "5"});
                    TipoMetodoMaterial.Add(new SelectListItem(){Text = "KATG - INH A",Value = "6"});
                    TipoMetodoMaterial.Add(new SelectListItem(){Text = "Identificación molecular", Value = "7"});
                    //Resultados
                    resultado.Add(new SelectListItem() { Text = "S", Value = "6" });
                    resultado.Add(new SelectListItem() { Text = "R", Value = "7" });
                    resultado.Add(new SelectListItem() { Text = "EXCL", Value = "8" });
                    resultado.Add(new SelectListItem() { Text = "NO TB", Value = "9" });
                    resultado.Add(new SelectListItem() { Text = "C", Value = "10" });
                    resultado.Add(new SelectListItem() { Text = "MTB", Value = "11" });
                    break;
                case 3:// mEDIO cULTIVO
                    TipoMetodoMaterial.Add(new SelectListItem(){Text = "RESULTADOS DEL CRECIMIENTO BACTERIANO DE MTB",Value = "8"});
                    //Resultados
                    resultado.Add(new SelectListItem() { Text = "Otros", Value = "12" });
                    break;
                default:
                    break;
            }
           
            ViewBag.ListTipoMetodoMaterial = TipoMetodoMaterial;
            ViewBag.ListResultado = resultado;
        }
         
    }
}
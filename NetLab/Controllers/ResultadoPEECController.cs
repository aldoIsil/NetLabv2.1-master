using BusinessLayer.PEED;
using Model;
using Model.ViewData;
using NetLab.Models.PEED;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetLab.Controllers
{
    public class ResultadoPEECController : ParentController
    {
        // GET: ResultadoPEEC
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BusquedaEvaluacion()
        {
            var model = new IngresoResultadosControlCalidadBl().GetIngresoResultadoControlCalidad(new IngresoResultadosPEEC() { idEESSEvaluado = EstablecimientoSeleccionado.IdEstablecimiento });
            //var listConfiguracionEvalControlCalidad = new List<ConfiguracionEvalControlCalidad>();
            //if ((page == null) && (fechaDesde == null) && (fechaHasta == null) && (idEnfermedad == null) && (idExamen == null) && (hddDato == null))
            //    return View();
            //else
            //{
            //var pageNumber = page ?? 1;
            //var blconfigeval = new ConfiguracionEvalControlCalidadBl();
            //var filtroConfigEvalCC = new ConfiguracionEvalControlCalidad();

            //var sDato = hddDato == "" ? "0" : hddDato;
            //var sEnfermedad = idEnfermedad == "" ? "0" : idEnfermedad;
            //var sExamen = idExamen == "" ? Guid.Empty : Guid.Parse(idExamen);
            //model = blconfigeval.GetConfiguracionEvalControlCalidad(filtroConfigEvalCC);
            var pageOfSeg = model.ToPagedList(1, GetSetting<int>(PageSize));
            return View(pageOfSeg);
        }
        public ActionResult ResultadoPEED(Guid idConfiguracionPanel, int idLabEvaludaor, string LabEvaluador)
        {
            //Cargar Configuracion
            var _panel = new ConfiguracionPanelControlCalidadBl().GetConfiguracionPanelControlCalidad(new ConfiguracionPanelControlCalidad() { idConfiguracionPanel = idConfiguracionPanel }).FirstOrDefault();
            var _materiales = new ConfiguracionMaterialControlCalidadBl().GetConfiguracionMaterialControlCalidad(new ConfiguracionMaterialControlCalidad() { idConfiguracionPanel = idConfiguracionPanel });
            //
            //Cargar Resultados Configurados
            var _resultados = new IngresoResultadosControlCalidadBl().GetResultadoControlCalidadVd(new ResultadoControlCalidadVd() { idConfigEvaluacion = _panel.idConfigEvaluacion, idConfiguracionPanel = _panel.idConfiguracionPanel });
            //                      
            TipoMetodo(_panel.idTipo);

            //Carga las preguntas configuradas
            var NroPregunta = new List<SelectListItem> { new SelectListItem() { Text = "Seleccionar", Value = "" } };

            foreach (var item in _materiales.GroupBy(test => test.nroPregunta).Select(grp => grp.First()).ToList())
            {
                NroPregunta.Add(new SelectListItem() { Text = item.nroPregunta, Value = item.nroPregunta });
            }
            ViewBag.ListaNroPregunta = NroPregunta;
            //
            var model = new ResultadoPEEDViewModels()
            {
                ResultadosControlCalidad = _resultados,
                Panel = _panel,
                Materiales = _materiales,
                ResultadoControlCalidad = new ResultadoControlCalidadVd() { idEstablecimientoEvaluador = idLabEvaludaor, EESSEvaluador = LabEvaluador }
            };
            Session["ResultadoControlCalidadVd"] = model;
            return View("ResultadoControlCalidadVd", model);
        }
        public ActionResult Refresh(Guid idConfigEvaluacion, Guid idConfiguracionPanel, int idEstablecimientoEvaluador)
        {
            var model = new ResultadoPEEDViewModels();
            model = (ResultadoPEEDViewModels)Session["ResultadoControlCalidadVd"];
            

            var oResultadoPEEDViewModels = new ResultadoPEEDViewModels();
            oResultadoPEEDViewModels.ResultadosControlCalidad = new IngresoResultadosControlCalidadBl().GetResultadoEvaluacionMaterial(new ResultadoControlCalidadVd() { idConfigEvaluacion = idConfigEvaluacion, idConfiguracionPanel = idConfiguracionPanel, idEstablecimientoEvaluado = EstablecimientoSeleccionado.IdEstablecimiento, idEstablecimientoEvaluador = idEstablecimientoEvaluador });

            var modelresult = new SuceptibilidadGenotype();
            var modelresultbk = new Baciloscopia();
            var modelresultcv = new MedioCultivo();

            switch (model.Panel.idTipo)
            {
                case 1:
                    modelresultbk.LstBaciloscopia = ModelBaciloscopia(oResultadoPEEDViewModels).Distinct().ToList();
                    var pageOfSegBK = modelresultbk.LstBaciloscopia.ToPagedList(1, GetSetting<int>(PageSize));
                    return PartialView("_TablaResultadoBaciloscopia", pageOfSegBK);
                case 2:
                    modelresult.LstSuceptibilidadGenotype = ModelSucepGenotypeCargarDatos(oResultadoPEEDViewModels).Distinct().ToList();
                    var pageOfSegGN = modelresult.LstSuceptibilidadGenotype.ToPagedList(1, GetSetting<int>(PageSize));
                    return PartialView("_TablaResultadoSuceptibilidad", pageOfSegGN);
                case 3:
                    modelresultcv.LstMedioCultivo = ModelMedioCultivo(oResultadoPEEDViewModels).Distinct().ToList();
                    var pageOfSegMC = modelresultcv.LstMedioCultivo.ToPagedList(1, GetSetting<int>(PageSize));
                    return PartialView("_TablaResultadoMedioCultivo", pageOfSegMC);
            }

            return View();
        }
        

        public ActionResult Save(ResultadoPEEDViewModels oResultadoPEEDViewModels)
        {
            var model = new ResultadoPEEDViewModels();
            var _partilaView = "";
            model = (ResultadoPEEDViewModels)Session["ResultadoControlCalidadVd"];
            var _resultados = new IngresoResultadosControlCalidadBl().InsertResultadoControlCalidadVd(new ResultadoControlCalidadVd() { idConfigEvaluacion = oResultadoPEEDViewModels.Panel.idConfigEvaluacion, idConfiguracionPanel = oResultadoPEEDViewModels.Panel.idConfiguracionPanel, idEstablecimientoEvaluado = EstablecimientoSeleccionado.IdEstablecimiento, idEstablecimientoEvaluador = oResultadoPEEDViewModels.ResultadoControlCalidad.idEstablecimientoEvaluador, NroPregunta = oResultadoPEEDViewModels.ResultadoControlCalidad.NroPregunta, Respuesta = oResultadoPEEDViewModels.ResultadoControlCalidad.Respuesta, ValorRespuesta = oResultadoPEEDViewModels.ResultadoControlCalidad.ValorRespuesta, idTipoMetodo = oResultadoPEEDViewModels.ResultadoControlCalidad.idTipoMetodo, idTipoPanel = oResultadoPEEDViewModels.Panel.idTipo });           

            oResultadoPEEDViewModels.ResultadosControlCalidad = new IngresoResultadosControlCalidadBl().GetResultadoEvaluacionMaterial(new ResultadoControlCalidadVd() { idConfigEvaluacion = oResultadoPEEDViewModels.Panel.idConfigEvaluacion, idConfiguracionPanel = oResultadoPEEDViewModels.Panel.idConfiguracionPanel, idEstablecimientoEvaluado = EstablecimientoSeleccionado.IdEstablecimiento, idEstablecimientoEvaluador = oResultadoPEEDViewModels.ResultadoControlCalidad.idEstablecimientoEvaluador });

            //var modelresult = new SuceptibilidadGenotype();
            //var modelresultbk = new Baciloscopia();
            //var modelresultcv = new MedioCultivo();
            //var pageOfSegBK = new List<Baciloscopia>();
            //var pageOfSegMC = new List<MedioCultivo>();
            //var pageOfSegGN = new List<SuceptibilidadGenotype>();
            switch (model.Panel.idTipo)                                                                           
            {
                case 1:
                    _partilaView = "_TablaResultadoBaciloscopia";
                    //modelresultbk.LstBaciloscopia = ModelBaciloscopia(oResultadoPEEDViewModels).Distinct().ToList();
                    //pageOfSegBK = modelresultbk.LstBaciloscopia.ToPagedList(1, GetSetting<int>(PageSize));
                    return PartialView(_partilaView, ModelBaciloscopia(oResultadoPEEDViewModels).Distinct().ToList());
                case 2:
                    _partilaView = "_TablaResultadoSuceptibilidad";
                    /*modelresult.LstSuceptibilidadGenotype = ModelSucepGenotypeCargarDatos(oResultadoPEEDViewModels).Distinct().ToList();*/
                    //pageOfSegGN = modelresult.LstSuceptibilidadGenotype.ToPagedList(1, GetSetting<int>(PageSize));
                    return PartialView(_partilaView, ModelSucepGenotypeCargarDatos(oResultadoPEEDViewModels).Distinct().ToList());
                case 3:
                    _partilaView = "_TablaResultadoMedioCultivo";
                    //modelresultcv.LstMedioCultivo = ModelMedioCultivo(oResultadoPEEDViewModels).Distinct().ToList();
                    //pageOfSegMC = modelresultcv.LstMedioCultivo.ToPagedList(1, GetSetting<int>(PageSize));
                    return PartialView(_partilaView, ModelMedioCultivo(oResultadoPEEDViewModels).Distinct().ToList());
            }
            //int x = 0; int y = 0;
            //var t = x < y ? -1 : x > y ? 1 : 0;
            //var s = _partilaView == "_TablaResultadoBaciloscopia"? modelresultbk.LstBaciloscopia : _partilaView == "_TablaResultadoSuceptibilidad" ? modelresult.LstSuceptibilidadGenotype : modelresultcv.LstMedioCultivo);
            return PartialView(_partilaView);
        }

        public List<Baciloscopia> ModelBaciloscopia(ResultadoPEEDViewModels model)
        {
            var res = new List<Baciloscopia>();
            Baciloscopia obj = new Baciloscopia();
            foreach (var item in model.ResultadosControlCalidad.GroupBy(test => test.NroPregunta).SelectMany(grp => grp).ToList())
            {
                obj = new Baciloscopia();
                obj.NroPregunta = item.NroPregunta;
                obj.Resultados = ObtenerDescricpionRespuesta(item.Respuesta);
                res.Add(obj);
            }

            return res;
        }
        public List<MedioCultivo> ModelMedioCultivo(ResultadoPEEDViewModels model)
        {
            var res = new List<MedioCultivo>();
            MedioCultivo obj = new MedioCultivo();
            foreach (var item in model.ResultadosControlCalidad.GroupBy(test => test.NroPregunta).SelectMany(grp => grp).ToList())
            {
                obj = new MedioCultivo();
                obj.NroPregunta = item.NroPregunta;
                obj.Resultados = item.ValorRespuesta;
                res.Add(obj);
            }

            return res;
        }
        public string ObtenerDescricpionRespuesta (string dato)
        {
            string res= "";
            switch (dato)
            {
                case "1":
                    res = "NEGATIVO";
                    break;
                case "2":
                    res = "1-9 BAR";
                    break;
                case "3":
                    res = "1+";
                    break;
                case "4":
                    res = "2+";
                    break;
                case "5":
                    res = "3+";
                    break;
                case "12":
                    res = "Otros";
                    break;
                default:
                    break;
            }
            return res;
        }
        void TipoMetodo(int idTipoPanel)
        {
            var TipoEvaluacion = new List<SelectListItem>();
            var TipoMetodo = new List<SelectListItem>();
            var TipoMetodoMaterial = new List<SelectListItem>();

            TipoEvaluacion.Add(new SelectListItem() { Text = "Seleccionar", Value = "0" });
            TipoEvaluacion.Add(new SelectListItem() { Text = "Directo", Value = "1" });
            TipoEvaluacion.Add(new SelectListItem() { Text = "Indirecto", Value = "2" });
            ViewBag.ListTipoEvaluacion = TipoEvaluacion;

            TipoMetodo.Add(new SelectListItem() { Text = "Seleccionar", Value = "0" });
            TipoMetodo.Add(new SelectListItem() { Text = "Baciloscopia", Value = "1" });
            TipoMetodo.Add(new SelectListItem() { Text = "Suceptibilidad-Genotype", Value = "2" });
            TipoMetodo.Add(new SelectListItem() { Text = "Medio de Cultivo", Value = "3" });
            ViewBag.ListTipoMetodo = TipoMetodo;
            //MATERIAL

            TipoMetodoMaterial.Add(new SelectListItem() { Text = "Seleccionar", Value = "0" });

            var resultado = new List<SelectListItem>();
            switch (idTipoPanel)//TipoMetodo
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
                    TipoMetodoMaterial.Add(new SelectListItem() { Text = "RIF", Value = "2" });
                    TipoMetodoMaterial.Add(new SelectListItem() { Text = "INH", Value = "3" });
                    TipoMetodoMaterial.Add(new SelectListItem() { Text = "KAT G", Value = "4" });
                    TipoMetodoMaterial.Add(new SelectListItem() { Text = "INH A", Value = "5" });
                    TipoMetodoMaterial.Add(new SelectListItem() { Text = "KATG - INH A", Value = "6" });
                    TipoMetodoMaterial.Add(new SelectListItem() { Text = "Identificación Molecular", Value = "13" });
                    //Resultados
                    resultado.Add(new SelectListItem() { Text = "S", Value = "6" });
                    resultado.Add(new SelectListItem() { Text = "R", Value = "7" });
                    resultado.Add(new SelectListItem() { Text = "EXCL", Value = "8" });
                    resultado.Add(new SelectListItem() { Text = "NO TB", Value = "9" });
                    resultado.Add(new SelectListItem() { Text = "C", Value = "10" });
                    resultado.Add(new SelectListItem() { Text = "MTB", Value = "11" });
                    resultado.Add(new SelectListItem() { Text = "Ninguno", Value = "14" });
                    break;
                case 3:// MEDIO CULTIVO
                    TipoMetodoMaterial.Add(new SelectListItem() { Text = "RESULTADOS DEL CRECIMIENTO BACTERIANO DE MTB", Value = "8" });
                    //Resultados
                    resultado.Add(new SelectListItem() { Text = "Otros", Value = "12" });
                    break;
                default:
                    break;
            }

            ViewBag.ListTipoMetodoMaterial = TipoMetodoMaterial;
            ViewBag.ListResultado = resultado;


        }
        public List<SuceptibilidadGenotype> ModelSucepGenotypeCargarDatos(ResultadoPEEDViewModels model)
        {
            var res = new List<SuceptibilidadGenotype>();
            SuceptibilidadGenotype obj = new SuceptibilidadGenotype();

            foreach (var item in model.ResultadosControlCalidad.GroupBy(test => test.NroPregunta).SelectMany(grp => grp).ToList())
            {
                if (res.Where(r => r.NroPregunta == item.NroPregunta).Count() == 0)
                {
                    obj = new SuceptibilidadGenotype();
                }
                obj.NroPregunta = item.NroPregunta;
                obj.idOpcion = String.IsNullOrEmpty(item.Respuesta) ? 0 : int.Parse(item.Respuesta);
                if (item.idTipoMetodo == 13)
                {
                    switch (item.Respuesta)
                    {
                        case "6":
                            obj.IdentificacionMolecular = "SENSIBLE";
                            break;
                        case "7":
                            obj.IdentificacionMolecular = "RESISTENTE";
                            break;
                        case "8":
                            obj.IdentificacionMolecular = "EXCLUIDO";
                            break;
                        case "9":
                            obj.IdentificacionMolecular = "NO TB";
                            break;
                        case "10":
                            obj.IdentificacionMolecular = "CONTAMINADO";
                            break;
                        case "11":
                            obj.IdentificacionMolecular = "COMPLEJO MTB";
                            break;
                        case "14":
                            obj.IdentificacionMolecular = item.ValorRespuesta;
                            break;
                        default:
                            obj.IdentificacionMolecular = item.ValorRespuesta;
                            break;
                    };
                }
                if (item.idTipoMetodo == 2)
                {
                    switch (item.Respuesta)
                    {
                        case "6":
                            obj.Rifampicina = "SENSIBLE";
                            break;
                        case "7":
                            obj.Rifampicina = "RESISTENTE";
                            break;
                        case "8":
                            obj.Rifampicina = "EXCLUIDO";
                            break;
                        case "9":
                            obj.Rifampicina = "NO TB";
                            break;
                        case "10":
                            obj.Rifampicina = "CONTAMINADO";
                            break;
                        case "11":
                            obj.Rifampicina = "COMPLEJO MTB";
                            break;
                        case "14":
                            obj.Rifampicina = item.ValorRespuesta;
                            break;
                        default:
                            obj.Rifampicina = item.ValorRespuesta;
                            break;
                    };
                }
                if (item.idTipoMetodo == 3)
                {
                    switch (item.Respuesta)
                    {
                        case "6":
                            obj.Isoniacida = "SENSIBLE";
                            break;
                        case "7":
                            obj.Isoniacida = "RESISTENTE";
                            break;
                        case "8":
                            obj.Isoniacida = "EXCLUIDO";
                            break;
                        case "9":
                            obj.Isoniacida = "NO TB";
                            break;
                        case "10":
                            obj.Isoniacida = "CONTAMINADO";
                            break;
                        case "11":
                            obj.Isoniacida = "COMPLEJO MTB";
                            break;
                        case "14":
                            obj.Isoniacida = item.ValorRespuesta;
                            break;
                        default:
                            obj.Isoniacida = item.ValorRespuesta;
                            break;
                    }
                }
                if (item.idTipoMetodo == 4)
                {
                    switch (item.Respuesta)
                    {
                        case "6":
                            obj.KatG = "SENSIBLE";
                            break;
                        case "7":
                            obj.KatG = "RESISTENTE";
                            break;
                        case "8":
                            obj.KatG = "EXCLUIDO";
                            break;
                        case "9":
                            obj.KatG = "NO TB";
                            break;
                        case "10":
                            obj.KatG = "CONTAMINADO";
                            break;
                        case "11":
                            obj.KatG = "COMPLEJO MTB";
                            break;
                        case "14":
                            obj.KatG = item.ValorRespuesta;
                            break;
                        default:
                            obj.KatG = item.ValorRespuesta;
                            break;
                    }
                }
                if (item.idTipoMetodo == 5)
                {
                    switch (item.Respuesta)
                    {
                        case "6":
                            obj.InhA = "SENSIBLE";
                            break;
                        case "7":
                            obj.InhA = "RESISTENTE";
                            break;
                        case "8":
                            obj.InhA = "EXCLUIDO";
                            break;
                        case "9":
                            obj.InhA = "NO TB";
                            break;
                        case "10":
                            obj.InhA = "CONTAMINADO";
                            break;
                        case "11":
                            obj.InhA = "COMPLEJO MTB";
                            break;
                        case "14":
                            obj.InhA = item.ValorRespuesta;
                            break;
                        default:
                            obj.InhA = item.ValorRespuesta;
                            break;
                    }
                }
                if (item.idTipoMetodo == 6)
                {
                    switch (item.Respuesta)
                    {
                        case "6":
                            obj.KatGInhA = "SENSIBLE";
                            break;
                        case "7":
                            obj.KatGInhA = "RESISTENTE";
                            break;
                        case "8":
                            obj.KatGInhA = "EXCLUIDO";
                            break;
                        case "9":
                            obj.KatGInhA = "NO TB";
                            break;
                        case "10":
                            obj.KatGInhA = "CONTAMINADO";
                            break;
                        case "11":
                            obj.KatGInhA = "COMPLEJO MTB";
                            break;
                        case "14":
                            obj.KatGInhA = item.ValorRespuesta;
                            break;
                        default:
                            obj.KatGInhA = item.ValorRespuesta;
                            break;
                    }
                }
                res.Add(obj);
            }

            return res;
        }
    }
}

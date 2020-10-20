using BusinessLayer;
using Enums;
using Model.Entidades;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Utilitario;

namespace NetLab.Controllers
{
    public partial class OrdenController : ParentController
    {
        public ActionResult Test()
        {
            var model = new CrearOrdenVM
            {
                Orden = new Orden
                {
                    IdPaciente = Guid.Parse("7A2F8913-154D-4F0F-9125-E64783FED729"),
                    CodigoOrden = string.Empty
                },
                TipoRegistro = TipoRegistroOrden.ORDEN_RECEPCION
            };

            var coreBl = new CoreBl();
            model.Paciente = coreBl.ObtenerPacientePorId(model.Orden.IdPaciente);

            CargarProyectos();
            CargarSeguroList();
            CargaUbigeoEstablecimiento();
            return View("Crear", model);
        }

        public ActionResult Crear(Guid idPaciente, string codigoOrden, TipoRegistroOrden tipoRegistro)
        {
            Guid ordenId = Guid.Empty;
            var paciente = new Paciente();
            using (var coreBl = new CoreBl())
            {
                ordenId = coreBl.CrearOrdenInicial(idPaciente, Logueado.idUsuario, tipoRegistro);
            }

            return RedirectToAction("Procesar", new { ordenId, idPaciente });
        }

        public ActionResult Procesar(Guid ordenId, Guid idPaciente)
        {
            //Guid ordenId = Guid.Empty;
            var paciente = new Paciente();
            var orden = new Orden();
            using (var coreBl = new CoreBl())
            {
                paciente = coreBl.ObtenerPacientePorId(idPaciente);
                orden = coreBl.ObtenerOrdenPorId(ordenId);
            }

            var ordenvm = new CrearOrdenVM
            {
                Orden = orden,
                Paciente = paciente,
                TipoRegistro = orden.TipoRegistro
            };

            CargarProyectos();
            CargarSeguroList();
            CargaUbigeoEstablecimiento();
            return View("Crear", ordenvm);
        }

        [HttpPost]
        public ActionResult CrearOrdenExamen(CrearOrdenExamenVM model)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                using (var bl = new CoreBl())
                {
                    model.UsuarioId = Logueado.idUsuario;
                    bl.CrearOrdenExamenCore(model);
                }

                //return PartialView();
                //return new EmptyResult();
                watch.Stop();
                new bsPage().LogCrearOrdenInfo("LogNetLab", "OrdenNuevo - CrearOrdenExamen - Info ", $" Info OrdenNuevo - CrearOrdenExamen - Tiempo transcurrido: {watch.ElapsedMilliseconds} ms");
                return RedirectToAction("ObtenerOrdenExamenList", new { idOrden = model.OrdenId, tipoRegistro = model.TipoRegistro });
            }
            catch (Exception ex)
            {
                watch.Stop();
                new bsPage().LogError(ex, "LogNetLab", "", $" OrdenNuevo - CrearOrdenExamen - Tiempo transcurrido: {watch.ElapsedMilliseconds} ms");
                throw ex;
            }
        }

        public ActionResult CargarNuevoExamen(Guid ordenId)
        {
            var model = new CrearOrdenExamenVM
            {
                OrdenId = ordenId
            };

            return PartialView("_Crear_AgregarExamen", model);
        }

        public ActionResult ObtenerOrdenExamenList(Guid idOrden, TipoRegistroOrden tipoRegistro)
        {
            var model = new List<CrearOrdenExamenTabla>();
            using (var bl = new CoreBl())
            {
                model = bl.ObtenerOrdenExamenesActivos(idOrden);
            }

            ViewBag.TipoRegistro = tipoRegistro;
            ViewBag.TienePR = model.Any(x => x.TienePR) ? 1 : 0;
            return PartialView("_Crear_OrdenExamenes", model);
        }

        public ActionResult CargarDatosClinicos(Guid idOrden)
        {
            var datos = new List<EnfermedadDatos>();
            using (var bl = new CoreBl())
            {
                //enfermedadIds.Distinct().ToList().ForEach(enfermedadId =>
                //{
                //    datos.AddRange(bl.ObtenerDatosClinicosPorEnfermedad(enfermedadId, idExamen));
                //});
                datos.AddRange(bl.ObtenerDatosClinicosPorEnfermedad(idOrden));
            }

            return PartialView("_Crear_DatosClinicos", datos);
        }

        [HttpPost]
        public ActionResult EliminarOrdenExamen(Guid ordenId, Guid ordenExamenId, TipoRegistroOrden tipoRegistro)
        {
            try
            {
                using (var bl = new CoreBl())
                {
                    bl.EliminarOrdenExamen(ordenExamenId);
                }

                return RedirectToAction("ObtenerOrdenExamenList", new { idOrden = ordenId, tipoRegistro = tipoRegistro });
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", "", " OrdenNuevoController - EliminarOrdenExamen ");
                throw ex;
            }
        }

        public ActionResult Guardar(CrearOrdenVM model)
        {
            var watch = Stopwatch.StartNew();
            string mensajeResultado = string.Empty;
            try
            {
                var datosclinicos = model.OrdenDatosClinicos.Where(x => !string.IsNullOrWhiteSpace(x.Valor)).ToList();
                datosclinicos.ForEach(dc => { dc.IdUsuarioRegistro = Logueado.idUsuario; });

                model.OrdenExamenes.ForEach(oe => { oe.ProyectoId = model.ProyectoId; });
                foreach(var items in model.OrdenExamenes.GroupBy(x=> x.IdTipoMuestra))
                {
                   var item= items.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x.CodigoMuestra));
                    items.ToList().ForEach(i =>
                    {
                        i.CodigoMuestra = item.CodigoMuestra;
                    });
                }

                using (var bl = new CoreBl())
                {
                    var watch2 = Stopwatch.StartNew();
                    bl.CrearOrdenDatosClinicos(datosclinicos, model.OrdenExamenes);
                    watch2.Stop();
                    new bsPage().LogCrearOrdenInfo("LogNetLab", "Guardar - CrearOrdenDatosClinicos ", $" Info Guardar {datosclinicos.Count} datos clinicos - Tiempo transcurrido: {watch2.ElapsedMilliseconds} ms");
                    mensajeResultado = bl.FinalizarCreacionOrden(model);
                }

                var recepcionbl = new RecepcionBl();
                //rechazar
                foreach (var item in model.OrdenExamenes.Where(x => x.MotivoRechazo.Any()))
                {
                    item.IdUsuario = Logueado.idUsuario;
                    recepcionbl.RechazarMuestra(new OrdenMuestraRecepcionados
                    {
                        IdOrdenMuestraRecepcion = item.IdOrdenMuestraRecepcion,
                        IdUsuario = item.IdUsuario,
                        MotivoRechazo = item.MotivoRechazo
                    });
                }

                watch.Stop();
                new bsPage().LogCrearOrdenInfo("LogNetLab", "OrdenNuevo - Guardar - Info ", $" Info OrdenNuevo - Guardar - Tiempo transcurrido: {watch.ElapsedMilliseconds} ms");

                if(!string.IsNullOrWhiteSpace(mensajeResultado)) ViewBag.textoRegistro = mensajeResultado;

                return RedirectToAction("SearchRecepcion", "Paciente", new { mensajeregistro = mensajeResultado });
            }
            catch (Exception ex)
            {
                watch.Stop();
                new bsPage().LogError(ex, "LogNetLab", "", $" OrdenNuevo - Guardar - Tiempo transcurrido: {watch.ElapsedMilliseconds} ms");
                throw ex;
            }
        }

        public JsonResult ValidarExamen(Guid idPaciente, int idLaboratorio, Guid idExamen, string fechaColeccion)
        {
            //bool existeExamen = false;
            string existeExamen = "";
            var ordenBL = new OrdenBl();
            var fecha = fechaColeccion.Split('/');
            var dt = new DateTime(Convert.ToInt32(fecha[2]), Convert.ToInt32(fecha[1]), Convert.ToInt32(fecha[0]));
            //existeExamen = ordenBL.VerificarExisteExamenPorPaciente(idPaciente, idLaboratorio, idExamen, dt);
            existeExamen = ordenBL.ValidaRegistroExamenPaciente(idPaciente, idLaboratorio, idExamen, dt);
            return Json(existeExamen, JsonRequestBehavior.AllowGet);
        }
    }
}
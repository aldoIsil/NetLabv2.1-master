using BusinessLayer.Interfaces;
using DataLayer;
using System;
using System.Collections.Generic;
using Model;
using System.Linq;
using Enums;
using static Model.ResultadoWebService;
using Utilitario;
using System.Configuration;
//using Newtonsoft.Json;

namespace BusinessLayer
{
    public class OrdenBl : IOrdenBl
    {
        private static OrdenBl instancia;
        public string ErrorMessage = string.Empty;
        /// <summary>
        /// Descripción: Metodo para registrar la información de la orden.
        /// enlazado con el metodo InsertarOrden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public Orden InsertOrden(Orden orden, TipoRegistroOrden tipo)
        {
            //Guid newIdOrden = Guid.Empty;
            //using (var ordenDal = new OrdenDal())
            //{
            //    try
            //    {
            //        if (orden.estatus == 1)
            //        {
            //            OrdenDal.ESTADO_BORRADOR = 1;
            //        }
            //        if (orden.estatus == 1 && tipo == TipoRegistroOrden.ORDEN_RECEPCION)
            //        {
            //            OrdenDal.ESTADO_BORRADOR = 2;
            //        }
            //        orden.estatus = OrdenDal.ESTADO_BORRADOR;

            //        orden = ordenDal.InsertOrden(orden);
            //        newIdOrden = orden.idOrden;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}

            using (var ordenDal = new OrdenDal())
            {

                ordenDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    if (orden.estatus == 1)
                    {
                        OrdenDal.ESTADO_BORRADOR = 1;
                    }
                    if (orden.estatus == 1 && tipo == TipoRegistroOrden.ORDEN_RECEPCION)
                    {
                        OrdenDal.ESTADO_BORRADOR = 2;
                    }
                    orden.estatus = OrdenDal.ESTADO_BORRADOR;

                    orden = ordenDal.InsertOrden(orden);
                    //throw new Exception();
                    InsertarOrden(orden, ordenDal, tipo);

                    ordenDal.Commit();

                }
                catch (Exception ex)
                {
                    ordenDal.Rollback();
                    //EliminarRegistroOrden(orden.idOrden);
                    //string stringObj = JsonConvert.SerializeObject(orden.ordenMuestraList, Formatting.Indented, new JsonSerializerSettings()
                    //{
                    //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    //});

                    //string datosExc = string.Format(" - ordenMuestraList: {0}", stringObj);
                    new bsPage().LogError(ex, "LogNetLab", string.Empty, "OrdenBl.InsertOrden");

                    ValidateOrderStateOnException(ex);
                    throw ex;
                }
                finally
                {
                    ordenDal.Dispose();
                }

            }
            //var newCodigoOrden = "";
            //using (var ordenDal = new OrdenDal())
            //{
            //    newCodigoOrden = CorreccionCodigoOrden(orden.codigoOrden, orden.idEstablecimiento);
            //}
            //if (!string.IsNullOrEmpty(newCodigoOrden))
            //{
            //    orden.codigoOrden = newCodigoOrden;
            //}
            return orden;
        }
        public Orden InsertOrdenCovid(Orden orden, TipoRegistroOrden tipo)
        {
            using (var ordenDal = new OrdenDal())
            {

                ordenDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    OrdenDal.ESTADO_BORRADOR = 2;
                    orden.estatus = OrdenDal.ESTADO_BORRADOR;

                    orden = ordenDal.InsertOrden(orden);
                    //throw new Exception();
                    InsertarOrdenCovid(orden, ordenDal, tipo);

                    ordenDal.Commit();

                }
                catch (Exception ex)
                {
                    ordenDal.Rollback();
                    //string stringObj = JsonConvert.SerializeObject(orden.ordenMuestraList, Formatting.Indented, new JsonSerializerSettings()
                    //{
                    //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    //});

                    //string datosExc = string.Format(" - ordenMuestraList: {0}", stringObj);
                    new bsPage().LogError(ex, "LogNetLab", string.Empty, "ordenBl.InsertOrdenCovid - ");
                    ValidateOrderStateOnException(ex);
                    throw ex;
                }
                finally
                {
                    ordenDal.Dispose();
                }
            }

            return orden;
        }
        /// <summary>
        /// Descripción: Metodo para registrar Nuevo examene VERIFICADOR.
        /// enlazado con el metodo InsertarOrden
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 11/11/2017
        /// Fecha Modificación: 11/11/2017.
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="idUsuario"></param>
        /// /// <param name="idEstablecimiento"></param>
        /// <returns></returns>

        public string InsertOrdenVerificador(string datos, int idUsuario, int idEstablecimiento)
        {
            var ordenDal = new OrdenDal();
            ordenDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            try
            {

                ordenDal.InsertOrdenVerificador(datos, idUsuario, idEstablecimiento);
                ordenDal.Commit();
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                ordenDal.Rollback();
                ValidateOrderStateOnException(ex);
            }


            return null;
        }
        /// <summary>
        /// Descripción: Metodo para registrar Nuevo examene ANALISTA.
        /// enlazado con el metodo InsertarOrden
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 11/11/2017
        /// Fecha Modificación: 11/11/2017.
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="idUsuario"></param>
        /// /// <param name="idEstablecimiento"></param>
        /// <returns></returns>

        public string InsertOrdenAnalista(string datos, int idUsuario, int idEstablecimiento)
        {
            var ordenDal = new OrdenDal();
            ordenDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            try
            {

                ordenDal.InsertOrdenAnalista(datos, idUsuario, idEstablecimiento);
                ordenDal.Commit();
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                ordenDal.Rollback();
                ValidateOrderStateOnException(ex);
            }


            return null;
        }
        public void InsertOrdenRomINS(string datos, int idUsuario, int idEstablecimiento)
        {
            try
            {
                var ordenDal = new OrdenDal();
                ordenDal.InsertOrdenRomINS(datos, idUsuario, idEstablecimiento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Descripción: Metodo transaccional para actualizar la orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public bool UpdateOrden(Orden orden, TipoRegistroOrden tipo)
        {
            using (var ordenDal = new OrdenDal())
            {

                ordenDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    //Se cambia el estado a eliminado de lo registrado previamente
                    ordenDal.UpdateOrden(orden);

                    InsertarOrden(orden, ordenDal, tipo);

                    ordenDal.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    ordenDal.Rollback();
                    ValidateOrderStateOnException(ex);
                    return false;
                }
            }
        }

        public void UpdateOrdenCovid(Orden orden)
        {
            try
            {
                var ordenDal = new OrdenDal();
                ordenDal.UpdateOrdenCovid(orden);
                //Update Dato Clinico
                if (orden.enfermedadList != null) //orden.enfermedadList puede venir null? en que casos?
                {
                    foreach (var enfermedad in orden.enfermedadList)
                    {
                        foreach (var ordenDatoClinico in enfermedad.categoriaDatoList.Where(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList != null &&
                            categoriaDato.OrdenDatoClinicoList.Count != 0).SelectMany(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList))
                        {
                            ordenDatoClinico.idOrden = orden.idOrden;
                            ordenDatoClinico.IdUsuarioEdicion = orden.IdUsuarioEdicion;
                            ordenDatoClinico.estatus = orden.estatus;
                            ordenDatoClinico.Enfermedad = new Enfermedad { idEnfermedad = enfermedad.idEnfermedad };
                            ordenDal.InsertOrdenDatoClinicoEdicion(ordenDatoClinico);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Descripción: Metodo transaccional para registrar la orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        /// <param name="ordenDal"></param>
        /// <param name="tipo"></param>
        private static void InsertarOrden(Orden orden, OrdenDal ordenDal, TipoRegistroOrden tipo, bool covidxls = false, int idAnalitoOpcionResultado = 0)
        {
            string strIdExamen = ConfigurationManager.AppSettings["SarsIdExamen"];
            Guid idExamenDefault = Guid.Parse(strIdExamen);
            string strIdTipoMuestra = ConfigurationManager.AppSettings["HisopadoIdTipoMuestra"];
            int idTipoMuestraDefault = int.Parse(strIdTipoMuestra);
            var idOrdenExamenOld = Guid.Empty;
            //Se inserta Orden Examen
            if (tipo == TipoRegistroOrden.EDITAR_ORDEN_DATOCLINICO)
            {
                //Update OrdenMuestra - Fecha Obtención
                if (orden.ordenMuestraList != null) //orden.ordenMuestraList puede venir null? en que casos?
                {
                    foreach (var ordenmuestra in orden.ordenMuestraList)
                    {
                        ordenmuestra.IdUsuarioEdicion = orden.IdUsuarioEdicion;
                        ordenDal.UpdateOrdenMuestra(ordenmuestra);
                    }
                }
                //Update OrdenMuestra - Establecimiento Destino
                if (orden.ordenMaterialList != null) //orden.ordenMaterialList puede venir null? en que casos?
                {
                    foreach (var ordenMaterial in orden.ordenMaterialList)
                    {
                        ordenMaterial.IdUsuarioEdicion = orden.IdUsuarioEdicion;
                        ordenDal.UpdateOrdenMaterial(ordenMaterial);
                    }
                }
                //Update Dato Clinico
                if (orden.enfermedadList != null) //orden.enfermedadList puede venir null? en que casos?
                {
                    foreach (var enfermedad in orden.enfermedadList)
                    {
                        foreach (var ordenDatoClinico in enfermedad.categoriaDatoList.Where(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList != null &&
                            categoriaDato.OrdenDatoClinicoList.Count != 0).SelectMany(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList))
                        {
                            ordenDatoClinico.idOrden = orden.idOrden;
                            ordenDatoClinico.IdUsuarioEdicion = orden.IdUsuarioEdicion;
                            ordenDatoClinico.estatus = orden.estatus;
                            ordenDatoClinico.Enfermedad = new Enfermedad { idEnfermedad = enfermedad.idEnfermedad };
                            ordenDal.InsertOrdenDatoClinicoEdicion(ordenDatoClinico);
                        }

                    }
                }
            }
            else
            {

                foreach (var ordenExamen in orden.ordenExamenList)
                {
                    ordenExamen.idOrden = orden.idOrden;
                    ordenExamen.estatus = orden.estatus;
                    ordenExamen.idOrdenExamen = covidxls ? ordenDal.InsertOrdenExamenODK(ordenExamen) : ordenDal.InsertOrdenExamen(ordenExamen);
                }

                var muestrabl = new MuestraBl();

                //Se inserta Orden Muestra
                foreach (var ordenMuestra in orden.ordenMuestraList)
                {
                    ordenMuestra.idOrden = orden.idOrden;
                    ordenMuestra.idProyecto = orden.Proyecto.IdProyecto;
                    ordenMuestra.IdUsuarioRegistro = orden.IdUsuarioRegistro;
                    ordenMuestra.estatus = orden.estatus;

                    if (tipo == TipoRegistroOrden.SOLO_ORDEN)
                    {
                        if (ordenMuestra.MuestraCodificacion == null) ordenMuestra.MuestraCodificacion = new MuestraCodificacion();
                        ordenMuestra.MuestraCodificacion.codificacion =
                            (orden.estatus == OrdenDal.ESTADO_PERMANENTE)
                                ? muestrabl.GeneraCodigosMuestra(orden.idEstablecimiento, orden.IdUsuarioRegistro, 1)
                                : null;
                    }

                    if (covidxls)
                    {
                        ordenDal.InsertOrdenMuestraODK(ordenMuestra, orden.idEstablecimiento);
                    }
                    else
                    {
                        if (ordenMuestra.TipoMuestra == null)
                        {
                            string infoData = string.Format(" - InsertarOrden - idOrden: {0} - idEstablecimiento: {1} - idExamen: {2} - ", ordenMuestra.idOrden, orden.idEstablecimiento, orden.ordenExamenList.First().idExamen);
                            new bsPage().LogInfo("ordnmuestraList", "ordenMuestra.TipoMuestra == null", infoData);
                            if (orden.ordenExamenList.Any(x => x.Examen.idExamen == idExamenDefault))
                                ordenMuestra.TipoMuestra = new TipoMuestra
                                {
                                    idTipoMuestra = idTipoMuestraDefault
                                };
                        }

                        ordenDal.InsertOrdenMuestra(ordenMuestra, orden.idEstablecimiento);
                    }
                }

                //Grabar OrdenExamenOrdenMuestra
                foreach (var ordenExamen in orden.ordenExamenList)
                {
                    var ordenExamenOrdenMuestra = new OrdenExamenOrdenMuestra { idOrdenExamen = ordenExamen.idOrdenExamen };

                    foreach (var ordenMuestra in ordenExamen.ordenMuestraList)
                    {
                        ordenExamenOrdenMuestra.idOrdenMuestra = orden.ordenMuestraList.First(
                            x => x.TipoMuestra.idTipoMuestra == ordenMuestra.TipoMuestra.idTipoMuestra).idOrdenMuestra;
                        ordenExamenOrdenMuestra.estatus = orden.estatus;
                        ordenExamenOrdenMuestra.IdUsuarioRegistro = orden.IdUsuarioRegistro;
                        ordenDal.InsertOrdenExamenOrdenMuestra(ordenExamenOrdenMuestra);

                        ordenDal.InsertOrdenExamenLineal(ordenExamenOrdenMuestra);
                    }
                }


                foreach (var ordenMaterial in orden.ordenMaterialList)
                {
                    var ordenMuestra = orden.ordenMuestraList.FirstOrDefault(x => x.TipoMuestra.idTipoMuestra == ordenMaterial.Material.TipoMuestra.idTipoMuestra);
                    ordenMaterial.idOrden = orden.idOrden;
                    ordenMaterial.IdUsuarioRegistro = orden.IdUsuarioRegistro;
                    ordenMaterial.ordenMuestra = ordenMuestra;
                    ordenMaterial.estatus = orden.estatus;
                    if (orden.ordenMaterialList.Any(x => x.idOrden == ordenMuestra.idOrden && x.OrdenExamen.idOrdenExamen == Guid.Empty))
                    {
                        if (orden.ordenExamenList.Count() == 1)
                        {
                            ordenMaterial.OrdenExamen.idOrdenExamen = orden.ordenExamenList.First(x => x.IdTipoMuestra == ordenMuestra.TipoMuestra.idTipoMuestra && x.worked == 0).idOrdenExamen;
                        }
                        else
                        {
                            ordenMaterial.OrdenExamen.idOrdenExamen = orden.ordenExamenList.Find(x => x.IdTipoMuestra == ordenMuestra.TipoMuestra.idTipoMuestra && x.worked == 0 && x.Examen.idExamen == ordenMaterial.OrdenExamen.Examen.idExamen).idOrdenExamen;
                            
                        }

                         ordenMaterial.OrdenExamen.idOrdenExamen = idOrdenExamenOld == ordenMaterial.OrdenExamen.idOrdenExamen ? orden.ordenExamenList.Last(x => x.IdTipoMuestra == ordenMuestra.TipoMuestra.idTipoMuestra && x.worked == 0 && ordenMaterial.OrdenExamen.idOrdenExamen != null).idOrdenExamen : ordenMaterial.OrdenExamen.idOrdenExamen;
                    }
                    idOrdenExamenOld = ordenMaterial.OrdenExamen.idOrdenExamen;
                    //Se insertan OrdenMuestraRecepcion
                    //int estatusOrdenMuestraRecepcion = 0;
                    var estatusOrdenMuestraRecepcion = orden.estatus;
                    if (ordenMaterial.ordenMuestraRecepcionList == null || !ordenMaterial.ordenMuestraRecepcionList.Any()) //revisar idordenexamen no llega
                    {
                        const bool insertaOrdenMuestraRecepcion = true;
                        if (covidxls)
                        {
                            ordenDal.InsertOrdenMaterialODK(ordenMaterial, estatusOrdenMuestraRecepcion, insertaOrdenMuestraRecepcion);
                        }
                        else
                        {
                            ordenDal.InsertOrdenMaterial(ordenMaterial, estatusOrdenMuestraRecepcion, insertaOrdenMuestraRecepcion);
                            //ordenDal.InsertOrdenMaterialLaboratoriosDistintos(ordenMaterial, estatusOrdenMuestraRecepcion, insertaOrdenMuestraRecepcion, orden.idEstablecimiento);
                        }

                        ordenMaterial.OrdenExamen.worked = 1;
                        orden.ordenExamenList.First(x => x.idOrdenExamen == ordenMaterial.OrdenExamen.idOrdenExamen).worked = 1;
                    }
                    else
                    {
                        const bool insertaOrdenMuestraRecepcion = false;
                        //Juan Muga - inicio - para cuando se finaliza orden desde busqueda de orden
                        switch (tipo)
                        {
                            case TipoRegistroOrden.SOLO_ORDEN:
                            case TipoRegistroOrden.SOLO_ORDEN_MUESTRA:
                                estatusOrdenMuestraRecepcion = 1;
                                break;
                            case TipoRegistroOrden.ORDEN_RECEPCION:
                                estatusOrdenMuestraRecepcion = 3;
                                break;
                            default:
                                break;
                        }

                        //estatusOrdenMuestraRecepcion = orden.estatus == 0 ? 0 : 3; //recepcionado
                        //Juan Muga - fin
                        ordenDal.InsertOrdenMaterial(ordenMaterial, estatusOrdenMuestraRecepcion, insertaOrdenMuestraRecepcion);
                        //ordenDal.InsertOrdenMaterialLaboratoriosDistintos(ordenMaterial, estatusOrdenMuestraRecepcion, insertaOrdenMuestraRecepcion, orden.idEstablecimiento);
                        ordenMaterial.OrdenExamen.worked = 1;
                        orden.ordenExamenList.First(x => x.idOrdenExamen == ordenMaterial.OrdenExamen.idOrdenExamen).worked = 1;
                        //Juan Muga - Cambio de Muga 25Julio
                        if (estatusOrdenMuestraRecepcion == 3)
                        {
                            foreach (var ordenMuestraRecepcion in ordenMaterial.ordenMuestraRecepcionList)
                            {
                                ordenMuestraRecepcion.idOrden = ordenMaterial.idOrden;
                                ordenMuestraRecepcion.OrdenMaterial = ordenMaterial;
                                ordenDal.InsertOrdenMuestraRecepcion(ordenMuestraRecepcion);

                                if (ordenMuestraRecepcion.conforme != 0) continue;

                                foreach (var criterioRechazo in ordenMuestraRecepcion.criterioRechazoList.Where(
                                    criterioRechazo => criterioRechazo.rechazado))
                                {
                                    criterioRechazo.IdOrdenMuestraRecepcion = ordenMuestraRecepcion.idOrdenMuestraRecepcion;
                                    criterioRechazo.IdUsuarioRegistro = ordenMuestraRecepcion.OrdenMaterial.IdUsuarioRegistro;
                                    ordenDal.InsertOrdenMuestraRecepcionRechazo(criterioRechazo);
                                }
                            }
                        }
                    }
                }

                //Si ya se esta finalizando la creacion de la orden se registran los resultados analitos
                if (orden.estatus > 0)
                {
                    if (covidxls)
                    {
                        var idOrdenExamen_ = orden.ordenExamenList.First().idOrdenExamen;
                        ordenDal.InsertOrdenResultadoAnalitoODK(orden, idOrdenExamen_, idAnalitoOpcionResultado);
                    }
                    else
                    {
                        ordenDal.InsertOrdenResultadoAnalito(orden);
                    }
                }

                if (orden.enfermedadList != null)
                {
                    foreach (var enfermedad in orden.enfermedadList)
                    {
                        foreach (var ordenDatoClinico in enfermedad.categoriaDatoList.Where(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList != null &&
                            categoriaDato.OrdenDatoClinicoList.Count != 0).SelectMany(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList))
                        {
                            ordenDatoClinico.idOrden = orden.idOrden;
                            ordenDatoClinico.IdUsuarioRegistro = orden.IdUsuarioRegistro;
                            ordenDatoClinico.estatus = orden.estatus;
                            ordenDatoClinico.Enfermedad = new Enfermedad { idEnfermedad = enfermedad.idEnfermedad };
                            ordenDal.InsertOrdenDatoClinico(ordenDatoClinico);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(orden.Procedencia))
                {
                    ordenDal.InsertOrdenProcedenciaPaciente(orden);
                }

                if (!string.IsNullOrWhiteSpace(orden.ejecutorPr))
                {
                    foreach (var item in orden.ordenExamenList)
                    {
                        if (item.Examen.PruebaRapida == 1)
                        {
                            ordenDal.InsertOrdenEjecutorPruebaRapida(orden);
                        }
                    }
                }
            }
        }
        private static void InsertarOrdenCovid(Orden orden, OrdenDal ordenDal, TipoRegistroOrden tipo, bool covidxls = false, int idAnalitoOpcionResultado = 0)
        {
            try
            {
                string strIdExamen = ConfigurationManager.AppSettings["SarsIdExamen"];
                Guid idExamenDefault = Guid.Parse(strIdExamen);
                string strIdTipoMuestra = ConfigurationManager.AppSettings["HisopadoIdTipoMuestra"];
                int idTipoMuestraDefault = int.Parse(strIdTipoMuestra);
                var idOrdenExamenOld = Guid.Empty;
                //Se inserta Orden Examen
                foreach (var ordenExamen in orden.ordenExamenList)
                {
                    ordenExamen.idOrden = orden.idOrden;
                    ordenExamen.estatus = orden.estatus;
                    ordenExamen.idOrdenExamen = ordenDal.InsertOrdenExamen(ordenExamen);
                }

                var muestrabl = new MuestraBl();

                //Se inserta Orden Muestra
                foreach (var ordenMuestra in orden.ordenMuestraList)
                {
                    ordenMuestra.idOrden = orden.idOrden;
                    ordenMuestra.idProyecto = orden.Proyecto.IdProyecto;
                    ordenMuestra.IdUsuarioRegistro = orden.IdUsuarioRegistro;
                    ordenMuestra.estatus = orden.estatus;

                    ordenMuestra.MuestraCodificacion = muestrabl.GeneraCodigosMuestraKobo(orden.idEstablecimiento, orden.IdUsuarioRegistro, 1);
                    //ordenMuestra.MuestraCodificacion.codificacion = x.MuestraCodificacion.codificacion;
                    //ordenMuestra.MuestraCodificacion.codificacionLineal = x.MuestraCodificacion.codificacionLineal;

                    if (ordenMuestra.TipoMuestra == null)
                    {
                        string infoData = string.Format(" - InsertarOrdenCovid - idOrden: {0} - idEstablecimiento: {1} - idExamen: {2} - ", ordenMuestra.idOrden, orden.idEstablecimiento, orden.ordenExamenList.First().idExamen);
                        new bsPage().LogInfo("ordnmuestraList", "ordenMuestra.TipoMuestra == null", infoData);
                        if (orden.ordenExamenList.Any(x => x.Examen.idExamen == idExamenDefault))
                            ordenMuestra.TipoMuestra = new TipoMuestra
                            {
                                idTipoMuestra = idTipoMuestraDefault
                            };
                    }

                    ordenDal.InsertOrdenMuestraKobo(ordenMuestra, orden.idEstablecimiento);
                }

                //Grabar OrdenExamenOrdenMuestra
                foreach (var ordenExamen in orden.ordenExamenList)
                {
                    var ordenExamenOrdenMuestra = new OrdenExamenOrdenMuestra { idOrdenExamen = ordenExamen.idOrdenExamen };

                    foreach (var ordenMuestra in ordenExamen.ordenMuestraList)
                    {
                        ordenExamenOrdenMuestra.idOrdenMuestra = orden.ordenMuestraList.First(
                            x => x.TipoMuestra.idTipoMuestra == ordenMuestra.TipoMuestra.idTipoMuestra).idOrdenMuestra;
                        ordenExamenOrdenMuestra.estatus = orden.estatus;
                        ordenExamenOrdenMuestra.IdUsuarioRegistro = orden.IdUsuarioRegistro;
                        ordenDal.InsertOrdenExamenOrdenMuestra(ordenExamenOrdenMuestra);

                        ordenDal.InsertOrdenExamenLineal(ordenExamenOrdenMuestra);
                    }
                }

                foreach (var ordenMaterial in orden.ordenMaterialList)
                {
                    var ordenMuestra = orden.ordenMuestraList.FirstOrDefault(x => x.TipoMuestra.idTipoMuestra == ordenMaterial.Material.TipoMuestra.idTipoMuestra);
                    ordenMaterial.idOrden = orden.idOrden;
                    ordenMaterial.IdUsuarioRegistro = orden.IdUsuarioRegistro;
                    ordenMaterial.ordenMuestra = ordenMuestra;
                    ordenMaterial.estatus = orden.estatus;
                    if (orden.ordenMaterialList.Any(x => x.idOrden == ordenMuestra.idOrden && x.OrdenExamen.idOrdenExamen == Guid.Empty))
                    {
                        if (orden.ordenExamenList.Count() == 1)
                        {
                            ordenMaterial.OrdenExamen.idOrdenExamen = orden.ordenExamenList.First(x => x.IdTipoMuestra == ordenMuestra.TipoMuestra.idTipoMuestra && x.worked == 0).idOrdenExamen;
                        }
                        else
                        {
                            ordenMaterial.OrdenExamen.idOrdenExamen = orden.ordenExamenList.First(x => x.IdTipoMuestra == ordenMuestra.TipoMuestra.idTipoMuestra && x.worked == 0 && x.Examen.idExamen != ordenMaterial.OrdenExamen.Examen.idExamen).idOrdenExamen;
                        }

                        ordenMaterial.OrdenExamen.idOrdenExamen = idOrdenExamenOld == ordenMaterial.OrdenExamen.idOrdenExamen ? orden.ordenExamenList.Last(x => x.IdTipoMuestra == ordenMuestra.TipoMuestra.idTipoMuestra && x.worked == 0 && ordenMaterial.OrdenExamen.idOrdenExamen != null).idOrdenExamen : ordenMaterial.OrdenExamen.idOrdenExamen;
                    }
                    idOrdenExamenOld = ordenMaterial.OrdenExamen.idOrdenExamen;
                    //Se insertan OrdenMuestraRecepcion
                    //int estatusOrdenMuestraRecepcion = 0;
                    var estatusOrdenMuestraRecepcion = orden.estatus;
                    if (ordenMaterial.ordenMuestraRecepcionList == null || !ordenMaterial.ordenMuestraRecepcionList.Any()) //revisar idordenexamen no llega
                    {
                        const bool insertaOrdenMuestraRecepcion = true;
                        ordenDal.InsertOrdenMaterialKOBO(ordenMaterial, estatusOrdenMuestraRecepcion, insertaOrdenMuestraRecepcion);

                        ordenMaterial.OrdenExamen.worked = 1;
                        orden.ordenExamenList.First(x => x.idOrdenExamen == ordenMaterial.OrdenExamen.idOrdenExamen).worked = 1;
                    }
                    else
                    {
                        const bool insertaOrdenMuestraRecepcion = false;
                        //Juan Muga - inicio - para cuando se finaliza orden desde busqueda de orden
                        estatusOrdenMuestraRecepcion = 3;

                        //estatusOrdenMuestraRecepcion = orden.estatus == 0 ? 0 : 3; //recepcionado
                        //Juan Muga - fin
                        ordenDal.InsertOrdenMaterialKOBO(ordenMaterial, estatusOrdenMuestraRecepcion, insertaOrdenMuestraRecepcion);

                        ordenMaterial.OrdenExamen.worked = 1;
                        orden.ordenExamenList.First(x => x.idOrdenExamen == ordenMaterial.OrdenExamen.idOrdenExamen).worked = 1;
                        //Juan Muga - Cambio de Muga 25Julio
                        if (estatusOrdenMuestraRecepcion == 3)
                        {
                            foreach (var ordenMuestraRecepcion in ordenMaterial.ordenMuestraRecepcionList)
                            {
                                ordenMuestraRecepcion.idOrden = ordenMaterial.idOrden;
                                ordenMuestraRecepcion.OrdenMaterial = ordenMaterial;
                                ordenDal.InsertOrdenMuestraRecepcion(ordenMuestraRecepcion);
                            }
                        }
                    }
                }
                //Si ya se esta finalizando la creacion de la orden se registran los resultados analitos
                ordenDal.InsertOrdenResultadoAnalito(orden);

                if (orden.enfermedadList != null)
                {
                    foreach (var enfermedad in orden.enfermedadList)
                    {
                        foreach (var ordenDatoClinico in enfermedad.categoriaDatoList.Where(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList != null &&
                            categoriaDato.OrdenDatoClinicoList.Count != 0).SelectMany(
                            categoriaDato => categoriaDato.OrdenDatoClinicoList))
                        {
                            ordenDatoClinico.idOrden = orden.idOrden;
                            ordenDatoClinico.IdUsuarioRegistro = orden.IdUsuarioRegistro;
                            ordenDatoClinico.estatus = orden.estatus;
                            ordenDatoClinico.Enfermedad = new Enfermedad { idEnfermedad = enfermedad.idEnfermedad };
                            ordenDal.InsertOrdenDatoClinico(ordenDatoClinico);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab", string.Format("idUsuario: {0} - ", orden.IdUsuarioRegistro), "OrdenBl.InsertOrdenCovid exception - ");
                throw ex;
            }
        }
        /// <summary>
        /// Descripción: Metodo transaccional para insertar orden de tipo banco de sangre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        public void InsertOrdenBancoSangre(Orden orden)
        {
            using (var ordenDal = new OrdenDal())
            {
                ordenDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                //MuestraCepasBancoSangre
                ordenDal.InsertCepaBancoSangre(orden.cepaBancoSangre, 0);

                orden.idCepaBancoSangre = orden.cepaBancoSangre.idCepaBancoSangre;
                orden.estatus = OrdenDal.ESTADO_BORRADOR;
                orden = ordenDal.InsertOrdenCBS(orden);

                //Examenes
                foreach (var ordenExamen in orden.ordenExamenList)
                {
                    ordenExamen.idOrden = orden.idOrden;
                    ordenExamen.estatus = orden.estatus;
                    ordenDal.InsertOrdenExamenBS(ordenExamen);
                }

                var idOrdenMuestra = new Guid();
                //Se inserta Orden Muestra
                foreach (var ordenMuestra in orden.ordenMuestraList)
                {
                    ordenMuestra.idOrden = orden.idOrden;
                    ordenMuestra.estatus = orden.estatus;
                    ordenDal.InsertOrdenMuestraBS(ordenMuestra, 1);
                    idOrdenMuestra = ordenMuestra.idOrdenMuestra;
                }

                //Se inserta Orden Material
                foreach (var ordenMaterial in orden.ordenMaterialList)
                {
                    ordenMaterial.idOrden = orden.idOrden;
                    ordenMaterial.estatus = orden.estatus;
                    ordenDal.InsertOrdenMaterialBS(ordenMaterial, idOrdenMuestra);
                }

                ordenDal.Commit();
            }
        }
        /// <summary>
        /// Descripción: Metodo transaccional para la actualizacion de ordenes de banco de sangre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        public void UpdateOrdenBancoSangre(Orden orden)
        {
            using (var ordenDal = new OrdenDal())
            {
                ordenDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                //MuestraCepasBancoSangre
                ordenDal.InsertCepaBancoSangre(orden.cepaBancoSangre, orden.idEstablecimiento);

                orden.estatus = OrdenDal.ESTADO_PERMANENTE;
                orden.idCepaBancoSangre = orden.cepaBancoSangre.idCepaBancoSangre;

                if (orden.codigoOrden.Equals(""))
                {
                    orden = ordenDal.InsertOrdenCBS(orden);
                }
                else
                {
                    ordenDal.UpdateOrdenCBS(orden);
                    ordenDal.CleanOrdenCBS(orden);
                }

                //Examenes
                foreach (var ordenExamen in orden.ordenExamenList)
                {
                    ordenExamen.idOrden = orden.idOrden;
                    ordenExamen.estatus = orden.estatus;
                    ordenDal.InsertOrdenExamenBS(ordenExamen);
                }

                var idOrdenMuestra = new Guid();
                //Se inserta Orden Muestra
                foreach (var ordenMuestra in orden.ordenMuestraList)
                {
                    ordenMuestra.idOrden = orden.idOrden;
                    ordenMuestra.idMuestraCod = orden.cepaBancoSangre.idMuestraCod;
                    ordenMuestra.estatus = orden.estatus;
                    ordenDal.InsertOrdenMuestraBS(ordenMuestra, 2);
                    foreach (var ordenExamenOrdenMuestra in orden.ordenExamenList.Select(
                        ordenExamen => new OrdenExamenOrdenMuestra
                        {
                            IdUsuarioRegistro = orden.IdUsuarioRegistro,
                            idOrdenMuestra = ordenMuestra.idOrdenMuestra,
                            estatus = orden.estatus,
                            idOrdenExamen = orden.ordenExamenList.FirstOrDefault(x => x.idExamen == ordenExamen.idExamen).idOrdenExamen
                        }))
                    {
                        ordenDal.InsertOrdenExamenOrdenMuestra(ordenExamenOrdenMuestra);
                    }

                    idOrdenMuestra = ordenMuestra.idOrdenMuestra;
                }

                //Se inserta Orden Material
                foreach (var ordenMaterial in orden.ordenMaterialList)
                {
                    ordenMaterial.idOrden = orden.idOrden;
                    ordenMaterial.estatus = orden.estatus;
                    ordenDal.InsertOrdenMaterialBS(ordenMaterial, idOrdenMuestra);
                }

                ordenDal.Commit();

                var mDal = new MuestraDal();
                orden.cepaBancoSangre.muestra = mDal.CodificacionById(orden.cepaBancoSangre.idMuestraCod);
            }
        }
        /// <summary>
        /// Descripción: Metodo que Elimina Orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        public void DeleteOrden(Guid idOrden)
        {
            using (var ordenDal = new OrdenDal())
            {
                ordenDal.DeleteOrden(idOrden);
            }
        }
        /// <summary>
        /// Descripción: Metodo que obtiene toda la informacion de la orden filtrado rango de fechas, nro documento,nro oficio, estado Orden, fecha solicitud y establecimiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="docIdentidad"></param>
        /// <param name="nroOficio"></param>
        /// <param name="estadoOrden"></param>
        /// <param name="fechaSolicitud"></param>
        /// <param name="establecimientoSeleccionado"></param>
        /// <returns></returns>
        public List<Orden> GetOrdenes(DateTime fechaDesde, DateTime fechaHasta, string docIdentidad, string nroOficio, int estadoOrden, int fechaSolicitud, int establecimientoSeleccionado, int idusuariologueado)
        {
            List<Orden> ordenList;

            using (var ordenDal = new OrdenDal())
            {
                ordenList = ordenDal.GetOrdenes(fechaDesde, fechaHasta, docIdentidad, nroOficio, estadoOrden, fechaSolicitud, establecimientoSeleccionado, idusuariologueado);
            }

            return ordenList;
        }
        /// <summary>
        /// Descripción: Metodo que obtiene toda la informacion de la orden filtrado por el Codigo del mismo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public Orden GetOrdenById(Guid idOrden)
        {
            Orden orden;

            using (var ordenDal = new OrdenDal())
            {
                orden = ordenDal.GetOrdenById(idOrden);
            }

            return orden;
        }
        /// <summary>
        /// Descripción: Metodo que obtiene toda la informacion de la orden del paciente generada por medio de una plantilla
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <param name="idEstablecimiento"></param>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        public Orden GetOrdenByIdPlantilla(int idPlantilla, int idEstablecimiento, Guid idPaciente)
        {
            Orden orden;

            using (var ordenDal = new OrdenDal())
            {
                orden = ordenDal.GetOrdenByIdPlantilla(idPlantilla, idEstablecimiento, idPaciente);
            }

            return orden;
        }
        /// <summary>
        /// Autor: Juan Muga
        /// Descripcion: Registro/Modificacion de Nro de Oficio
        /// Fecha: 26/09/2017
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="nroOficio"></param>
        public void UpdateNumeroOficio(Orden _orden)
        {
            using (var ordenDAL = new OrdenDal())
            {
                ordenDAL.UpdateNumeroOficio(_orden);
            }
        }

        private void ValidateOrderStateOnException(Exception ex)
        {
            ErrorMessage = ex.Source.ToString(); //"Ha ocurrido un error al momento de procesar la orden. ";
        }
        /// <summary>
        /// Autor: Juan Muga
        /// Descripcion: Actualización de la orden para las Pruebas Rápidas
        /// Fecha: 20/11/2017
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        public string RecepcionEESSandRecepcionLab(Guid idOrden, int idUsuario)
        {
            using (var ordenDAL = new OrdenDal())
            {
                return ordenDAL.RecepcionEESSandRecepcionLab(idOrden, idUsuario);
            }
        }



        //public int ConsultaSolicitudTropismo(string idPaciente)
        //{
        //    using (var ordenDal = new OrdenDal())
        //    {
        //        return ordenDal.ConsultaSolicitudTropismo(idPaciente);
        //    }
        //}

        /// <summary>
        /// Descripción: Metodo para obtener información de resultados de un codigo de orden para el envío a otros sistemas.
        /// Autor: Juan Muga.
        /// Fecha Creacion: 10/09/2018
        /// </summary>
        /// <param name="codigoOrden"></param>
        /// <returns></returns>
        public List<Resultado> GetTramaResultadobyCodigoOrden(string codigoOrden)
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.GetTramaResultadobyCodigoOrden(codigoOrden);
            }
        }
        public int UpdateTRamaResultadobyCodigoOrden(string codigoOrden)
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.UpdateTRamaResultadobyCodigoOrden(codigoOrden);
            }
        }
        public void UpdateOrdenTramaEnvio(int estado, string codigoOrden)
        {
            using (var ordenDal = new OrdenDal())
            {
                ordenDal.UpdateOrdenTramaEnvio(estado, codigoOrden);
            }
        }
        public void RechazarOrden(string idRechazo, string idOrden, string origen, int idUsuario, string codigoOrden)
        {
            using (var ordenDal = new OrdenDal())
            {
                var usuario = new Usuario();
                ordenDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var item in idRechazo.Split(',').Distinct())
                    {
                        ordenDal.InsertOrdenPacienteRechazo(idOrden, int.Parse(item), idUsuario);
                    }

                    usuario = ordenDal.UpdateOrdenPacienteRechazo(idOrden, idUsuario);

                    new EnvioCorreo().EnviarCorreo(usuario.correo, "Sugerencias NetLabv2", "Estimado " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno + ", " + usuario.nombres + ", su Orden/Muestra: " + codigoOrden + " ha sido rechazada.", "soportenetlabv2@gmail.com");

                    ordenDal.Commit();
                }
                catch (Exception ex)
                {
                    ordenDal.Rollback();
                    ValidateOrderStateOnException(ex);
                }
            }

        }
        public List<MuestrasODKCoronavirus> GetOrdenesConsultaExternaODK()
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.GetOrdenesConsultaExternaODK();
            }
        }
        public Establecimiento GetEstablecimientobyCodigoUnico(string CodigoUnico)
        {
            using (var ordenDal = new OrdenDal())
            {
                return null;//ordenDal.GetEstablecimientobyCodigoUnico(CodigoUnico);
            }
        }

        public Orden ObtenerFicha(string idOrden)
        {
            Orden orden;
            using (var ordenDal = new OrdenDal())
            {
                orden = ordenDal.ObtenerFicha(idOrden);
            }
            return orden;
        }

        public Orden ObtenerIdOrdenPorDocumentoPaciente(int tipoDocumento, string nroDocumento)
        {
            using (var ordendal = new OrdenDal())
            {
                return ordendal.ObtenerIdOrdenPorDocumentoPaciente(tipoDocumento, nroDocumento);
            }
        }

        public Orden InsertarOrdenNuevo(Orden orden, TipoRegistroOrden tipo, int idAnalitoOpcionResultado)
        {
            using (var ordenDal = new OrdenDal())
            {
                ordenDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    InsertarOrden(orden, ordenDal, tipo, covidxls: true, idAnalitoOpcionResultado: idAnalitoOpcionResultado);

                    ordenDal.Commit();
                }
                catch (Exception ex)
                {
                    ordenDal.Rollback();
                    //string stringObj = JsonConvert.SerializeObject(orden.ordenMuestraList, Formatting.Indented, new JsonSerializerSettings()
                    //{
                    //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    //});
                    //string datosExc = string.Format(" - ordenMuestraList: {0}", stringObj);

                    //new bsPage().LogError(ex, "LogNetLab", datosExc, "ordenBl.InsertarOrdenNuevo");
                    ValidateOrderStateOnException(ex);
                }
                finally
                {
                    ordenDal.Dispose();
                }
            }

            return orden;
        }

        public void InsertarOrdenEjecutorPruebaRapida(Guid idOrden, string ejecutordocumento, string ejecutornombres)
        {
            using (var ordenDal = new OrdenDal())
            {
                ordenDal.InsertarOrdenEjecutorPruebaRapida(idOrden, ejecutordocumento, ejecutornombres);
            }
        }

        public bool ValidarExisteOrdenExamen(Guid idOrden, Guid idExamen)
        {
            using (var ordendal = new OrdenDal())
            {
                return ordendal.ValidarExisteOrdenExamen(idOrden, idExamen);
            }
        }

        public Orden GetTempOrdenByDNIPacienteYUsuario(string pacienteDNI, int idUsuarioRegistro)
        {
            Orden orden;

            using (var ordenDal = new OrdenDal())
            {
                orden = ordenDal.GetTempOrdenByDNIPacienteYUsuario(pacienteDNI, idUsuarioRegistro);
            }

            return orden;
        }

        public string CorreccionCodigoOrden(string idOrden, int idEstablecimiento)
        {

            using (var ordendal = new OrdenDal())
            {
                return ordendal.CorreccionCodigoOrden(idOrden, idEstablecimiento);
            }
        }

        public void EliminarRegistroOrden(Guid idorden)
        {
            using (var ordendal = new OrdenDal())
            {
                ordendal.EliminarRegistroOrden(idorden);
            }
        }

        public void Dispose()
        {
            instancia = null;
        }

        public string GenerarCodigoOrden(int idEstablecimiento)
        {
            using (var ordendal = new OrdenDal())
            {
                return ordendal.GenerarCodigoOrden(idEstablecimiento);
            }
        }

        public List<OrdenMuestraLinealkobo> ListOrdenMuestraLinealkobo(string dato)
        {
            using (var ordendal = new OrdenDal())
            {
                return ordendal.ListOrdenMuestraLinealkobo(dato);
            }
        }

        public bool VerificarExisteExamenPorPaciente(Guid idPaciente, int idLaboratorio, Guid idExamen, DateTime fechaColeccion)
        {
            using (var dal = new OrdenDal())
            {
                return dal.VerificarExisteExamenPorPaciente(idPaciente, idLaboratorio, idExamen, fechaColeccion);
            }
        }

        public string ValidaRegistroExamenPaciente(Guid idPaciente, int idLaboratorio, Guid idExamen, DateTime fechaColeccion)
        {
            using (var dal = new OrdenDal())
            {
                return dal.ValidaRegistroExamenPaciente(idPaciente, idLaboratorio, idExamen, fechaColeccion);
            }
        }
    }
}

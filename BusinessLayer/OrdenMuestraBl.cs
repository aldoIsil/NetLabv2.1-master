using DataLayer;
using System;
using System.Collections.Generic;
using Model;
using Model.ViewData;
using System.Linq;
using Model.Enums;

namespace BusinessLayer
{
    public class OrdenMuestraBl
    {
        string ExamenError = string.Empty;
        /// <summary>
        /// Descripción: Metodo para obtener Muestras - Ordenes por Establecimiento y listar informacion para la recepcion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="estadoOrden"></param>
        /// <param name="fechaSolicitud"></param>
        /// <param name="idEstablecimiento"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroOficio"></param>
        /// <param name="tipoOrden"></param>
        /// <param name="idMuestra"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<OrdenRecepcionVd> GetOrdenMuestraByEstablecimiento(int estadoOrden, int fechaSolicitud, int idEstablecimiento, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, int tipoOrden, string idMuestra, int idLaboratorio, string nroDocumento, string apellidopaterno, string apellidomaterno, string nombres)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.GetOrdenMuestraByEstablecimiento(estadoOrden, fechaSolicitud, idEstablecimiento, codigoOrden, fechaDesde, fechaHasta, nroOficio, tipoOrden, idMuestra, idLaboratorio, nroDocumento, apellidopaterno,apellidomaterno, nombres);
            }
        }

        public List<OrdenRecepcionVd> GetOrdenMuestraByEstablecimientoRechazado(int estadoOrden, int fechaSolicitud, int idEstablecimiento, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, int tipoOrden, string idMuestra, int idLaboratorio)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.GetOrdenMuestraByEstablecimientoRechazado(estadoOrden, fechaSolicitud, idEstablecimiento, codigoOrden, fechaDesde, fechaHasta, nroOficio, tipoOrden, idMuestra, idLaboratorio);
            }
        }

        /// <summary>
        /// Descripción: Obtiene informacion de las ordene filtrado por el Id de las ordenes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLabUsuario"></param>
        /// <returns></returns>
        public Orden GetInfoOrden(Guid idOrden, int? idLabUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.GetInfoOrden(idOrden, idLabUsuario);
            }
        }
        /// <summary>
        /// Descripción: Obtener los materiales por el codigo de orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLabUsuario"></param>
        /// <returns></returns>
        public List<OrdenMaterialVd> MaterialesByOrden(Guid idOrden, int idLabUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MaterialesByOrden(idOrden, idLabUsuario);
            }
        }
        /// <summary>
        /// Descripción: Permite obtener materiales referenciados por codigo de orden y codigo de laboratorio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLabUsuario"></param>
        /// <returns></returns>
        public List<OrdenMaterialVd> MaterialesRefenciadosByOrden(Guid idOrden, int idLabUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MaterialesRefenciadosByOrden(idOrden, idLabUsuario);
            }
        }
        /// <summary>
        /// Descripción: Permite obtener información de los materiales ingresados para una orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<OrdenMaterial> MuestrasByOrden(Guid idOrden)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestrasByOrden(idOrden);
            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener el material de las ordenes recepcionadas
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<OrdenMaterial> MuestrasByOrdenRecepcionadas(Guid idOrden)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestrasByOrdenRecepcionadas(idOrden);
            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de las muestras 
        ///              por código de orden para recepcionar.
        /// Estados: 0: no procesada | 1: procesada en un lugar -- 0
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="procesamiento"></param>
        /// <param name="idUsuario"></param>
        /// <param name="estatus"></param>
        /// <param name="conCriteriosRechazo"></param>
        /// <param name="rechazadas"></param>
        /// <param name="idLaboratorioUsuario"></param>
        /// <returns></returns>
        public List<OrdenMuestraRecepcion> MuestraRecepcionadosbyOrden(Guid idOrden, int procesamiento, int idUsuario,
            int estatus, int conCriteriosRechazo, int rechazadas, int idLaboratorioUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestraRecepcionadosbyOrden(idOrden, procesamiento, idUsuario, estatus,
                    conCriteriosRechazo, rechazadas, idLaboratorioUsuario);
            }
        }
        /// <summary>
        /// Descripción: Controlador para la actualizar la orden y generar la Recepcion de Muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaMuestras"></param>
        /// <param name="idOrden"></param>
        /// <param name="registroResultados"></param>
        public void RecepcionarMuestras(List<OrdenMuestraRecepcion> listaMuestras, Guid idOrden, int registroResultados, int idUsuarioRegistro)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.RecepcionarMuestras(listaMuestras, idOrden, registroResultados, idUsuarioRegistro);
            }
        }
        /// <summary>
        /// Descripción: Controlador para la actualizar la orden y generar la Recepcion de Muestra
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 26/11/2017
        /// Fecha Modificación: 26/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="datos"></param>

        public bool RecepcionarMuestrasMasivo(string datos, int idUsuario)
        {

            bool strOk = false;
            var ordenMuestraDal = new OrdenMuestraDal();
            ordenMuestraDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            try
            {
                ordenMuestraDal.RecepcionarMuestrasMasivo(datos, idUsuario);
                ordenMuestraDal.Commit();
                strOk = true;
            }
            catch (Exception ex)
            {
                var mensaje = ex.Message;
                ordenMuestraDal.Rollback();
            }


            return strOk;
        }

        /// <summary>
        /// Descripción: Controlador para la actualizar la orden y generar la Recepcion de Muestra
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 26/11/2017
        /// Fecha Modificación: 26/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="datos"></param>

        public bool VerificarMuestrasMasivo(List<ValidaResultadoMasivo> comentarioList, int idUsuario)
        {
            bool strOk = false;
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                //ordenMuestraDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var item in comentarioList)
                    {
                        int x = ordenMuestraDal.VerificarMuestrasMasivo(item, idUsuario);
                        try
                        {
                            if (x == 1)
                            {
                                //var mail = new ResultadosBl().GetDatosCorreo(item.idOrdenExamen);
                                //if (mail != null)
                                //{
                                //    var correo = new EnvioCorreo();
                                //    string correoSol = mail.CorreoSolicitante;
                                //    if (!string.IsNullOrEmpty(correoSol))
                                //    {
                                //        string asunto = "Resultado informado de Paciente";
                                //        string contenido = "Estimado(a) Dr(a): " + mail.Solicitante + "\n" + "El resultado del paciente con Código de Orden N° " + mail.CodigoOrden + " ya se encuentra ingresado en el Sistema Netlab v2.0";
                                //        correo.EnviarCorreo(correoSol, asunto, contenido);
                                //    }
                                //}
                                strOk = true;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    var mensaje = ex.Message;
                    //ordenMuestraDal.Rollback();
                }
            }
            return strOk;
        }
        public List<OrdenMuestraRecepcion> DerivarReferenciaMuestras(List<OrdenMuestraRecepcion> listaMuestrasReferenciar, int idUsuarioEdicion)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.DerivarReferenciaMuestras(listaMuestrasReferenciar, idUsuarioEdicion);
            }
        }
        /// <summary>
        /// Descripción: Metodo para la actualizar la orden y generar la referenciación de Muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaMuestrasReferenciar"></param>
        /// <param name="idUsuarioEdicion"></param>
        public void ReferenciarMuestras(List<OrdenMuestraRecepcion> listaMuestrasReferenciar, int idUsuarioEdicion)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.ReferenciarMuestras(listaMuestrasReferenciar, idUsuarioEdicion);
            }
        }
        /// <summary>
        /// Descripción: Metodo para REGISTRA Y REFERENCIA MUESTRAS
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaMuestrasRegistrar"></param>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        public void RegistrarMuestras(List<OrdenMuestraRecepcion> listaMuestrasRegistrar, Guid idOrden, int idUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.RegistrarMuestras(listaMuestrasRegistrar, idOrden, idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Metodo para registrar informacion de muestras recepcionadas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaMuestrasRegistrarT"></param>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        public void RegistrarMuestrasConRecepcion(List<OrdenMuestraRecepcion> listaMuestrasRegistrarT, Guid idOrden, int idUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.RegistrarMuestrasConRecepcion(listaMuestrasRegistrarT, idOrden, idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Registrar informacion de muestras sin estar recepcionadas. 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaMuestrasRegistrarT"></param>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        public void RegistrarMuestrasSinRecepcion(List<OrdenMuestraRecepcion> listaMuestrasRegistrarT, Guid idOrden, int idUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.RegistrarMuestrasSinRecepcion(listaMuestrasRegistrarT, idOrden, idUsuario);
            }
        }

        /// <summary>
        /// Descripción: Registrar las muestras que han sido rechazadas, solo se registra si no existe.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaCriteriosRechazos"></param>
        /// <param name="idUsuario"></param>
        public void RegistrarCriteriosRechazos(List<List<CriterioRechazo>> listaCriteriosRechazos, int idUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.RegistrarCriteriosRechazos(listaCriteriosRechazos, idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Obtener informacion de una muestra referenciada por orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idEstablecimiento"></param>
        /// <param name="estatus"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<OrdenMuestraRecepcion> MuestraReferenciadosEditar(Guid idOrden, int idEstablecimiento, int estatus, int idLaboratorio)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestraReferenciadosEditar(idOrden, idEstablecimiento, estatus, idLaboratorio);
            }
        }
        /// <summary>
        /// Descripción: Registra los resultados rechazados por cada prueba en Labortorio 
        /// Estado = 1
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaOrdenMuestraRecepcion"></param>
        public void EditarLaboratoriosMuestras(List<OrdenMuestraRecepcion> listaOrdenMuestraRecepcion)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.EditarLaboratoriosMuestras(listaOrdenMuestraRecepcion);
            }
        }

        /// <summary>
        /// Descripción: Obtiene informacion del laboratorio de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Laboratorio ObtenerLaboratorioUsuario(int idUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.ObtenerLaboratorioUsuario(idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Permite obtener información de los materiales ingresados para una orden para el detalle.
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idEstablecimientoUsuario"></param>
        /// <returns></returns>
        public List<OrdenRecepcionDetalleVd> MuestrasByOrdenDetalle(int idEstablecimientoUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestrasByOrdenDetalle(idEstablecimientoUsuario);
            }

        }

        /// <summary>
        /// Descripción: Permite obtener información de las muestras rechazadas.
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<OrdenMuestrasRechazo> GetOrdenMuestraRechazobyIdOrden(Guid idOrden)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.GetOrdenMuestraRechazobyIdOrden(idOrden);
            }
        }
        /// <summary>
        /// Descripción: Permite obtener información de los materiales ingresados para una orden para el detalle.
        /// Author: Jose Chavez.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idEstablecimientoUsuario"></param>
        /// <returns></returns>
        public List<OrdenRecepcionDetalleVd> MuestrasByResultadoAnalisis(int idEstablecimientoUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestrasByOrdenDetalle(idEstablecimientoUsuario);
            }
        }
        //Autor : Marcos Mejia 
        //Descricpion : Método que obtiene datos del usuario que registra la Orden.
        //Fecha: 25/09/2018
        public UsuarioEnvioSms ConsultaDatosUsuario(Guid idOrden)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.ConsultaDatosUsuario(idOrden);
            }
        }

        public string OrdenMuestraProcesoRom(OrdenMuestraRom xOrdenMuestraRom, Guid id, int idusuario, int IdEstablecimiento)
        {
            string Respuesta = "";
            var totalMuestrasValidasR = 0;
            var totalMuestrasInvalidasR = 0;
            var totalMuestrasRecepcionadas = 0;
            var totalMuestras = 0;
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    //Referenciar muestras 
                    if (xOrdenMuestraRom.listaOrdenesMuestra.Count > 0)
                    {
                        totalMuestrasInvalidasR = ValidaExamenes(1, xOrdenMuestraRom.listaOrdenesMuestra);
                        if (totalMuestrasInvalidasR > 0)
                        {
                            ordenMuestraDal.Rollback();
                            return totalMuestrasInvalidasR + "_" + totalMuestrasRecepcionadas + "_" + totalMuestras + "_" + ExamenError;
                        }
                        else
                        {
                            ordenMuestraDal.ReferenciarMuestras(xOrdenMuestraRom.listaOrdenesMuestra, idusuario);
                        }
                    }
                    //Crear y Referenciar muestras 
                    if (xOrdenMuestraRom.listaOrdenesMuestraCrearReferenciar.Count > 0)
                    {
                        totalMuestrasInvalidasR = ValidaExamenes(2, xOrdenMuestraRom.listaOrdenesMuestraCrearReferenciar);
                        if (totalMuestrasInvalidasR > 0)
                        {
                            ordenMuestraDal.Rollback();
                            return totalMuestrasInvalidasR + "_" + totalMuestrasRecepcionadas + "_" + totalMuestras + "_" + ExamenError;
                        }
                        else
                        {
                            ordenMuestraDal.RegistrarMuestras(xOrdenMuestraRom.listaOrdenesMuestraCrearReferenciar, id, idusuario);
                        }
                    }
                    //Derivar muestras juan muga
                    //revisar si se puede colocar mas abajo, donde se valida la variable lstNewGuid
                    var lstNewGuid = new List<OrdenMuestraRecepcion>();
                    if (xOrdenMuestraRom.listaOrdenesDerivarMuestra.Count > 0)
                    {
                        totalMuestrasInvalidasR = ValidaExamenes(2, xOrdenMuestraRom.listaOrdenesDerivarMuestra);
                        if (totalMuestrasInvalidasR > 0)
                        {
                            ordenMuestraDal.Rollback();
                            return totalMuestrasInvalidasR + "_" + totalMuestrasRecepcionadas + "_" + totalMuestras + "_" + ExamenError;
                        }
                        else
                        {
                            lstNewGuid = ordenMuestraDal.DerivarReferenciaMuestras(xOrdenMuestraRom.listaOrdenesDerivarMuestra, idusuario);
                        }
                    }
                    //Recepcion de Muestras
                    if (xOrdenMuestraRom.listaOrdenesMuestraRecepcionar.Count > 0)
                    {
                        //Obtener Lista de Examenes válidos por el laboratorio seleccionado
                        var laboratorioExamenBl = new LaboratorioExamenBl();
                        var TipoMuestraExamenBl = new ExamenTipoMuestraBl();//juanmuga
                        var listaExamenesValidos = laboratorioExamenBl.GetExamenesByLaboratorio(IdEstablecimiento);

                        if (listaExamenesValidos != null)
                        {
                            foreach (var itemR in xOrdenMuestraRom.listaOrdenesMuestraRecepcionar)
                            {
                                var listaTipoMuestraValidosTmp = TipoMuestraExamenBl.GetTipoMuestraByExamen(itemR.idExamen);//juan muga
                                var examenValido = listaExamenesValidos.FindAll(p => p.IdExamen == itemR.idExamen);
                                var tipomuestraValido = listaTipoMuestraValidosTmp.FindAll(p => p.IdTipoMuestra == itemR.idTipoMuestra);//juan muga

                                //if (examenValido.Any())
                                if (examenValido.Any() && tipomuestraValido.Any())//juanmuga
                                {
                                    xOrdenMuestraRom.listaParaReferenciarCrear.Add(itemR);
                                    totalMuestrasValidasR = totalMuestrasValidasR + 1;
                                }
                                else
                                {
                                    totalMuestrasInvalidasR = totalMuestrasInvalidasR + 1;
                                    ExamenError = ExamenError + ", " + new ExamenBl().GetExamenById(itemR.idExamen).nombre;
                                }
                            }
                        }

                        //Existe muestras a rececepcionar válidas
                        if (xOrdenMuestraRom.listaParaReferenciarCrear.Any())
                        {
                            totalMuestrasRecepcionadas = xOrdenMuestraRom.listaParaReferenciarCrear.Count;
                            //derivar: cambio de guid = idordenmuestrarecpcionar juan muga
                            var listaParaDerivar = new List<OrdenMuestraRecepcion>();
                            foreach (var item in xOrdenMuestraRom.listaParaReferenciarCrear)
                            {
                                foreach (var itemD in lstNewGuid)
                                {
                                    if (item.idOrdenMuestraRecepcion == itemD.idOrdenMuestraRecepcion)
                                    {
                                        item.idOrdenMuestraRecepcion = itemD.NewidOrdenMuestraRecepcion;
                                        listaParaDerivar.Add(item);
                                    }
                                }
                            }
                            if (listaParaDerivar.Count > 0)
                            {
                                ordenMuestraDal.RecepcionarMuestras(listaParaDerivar, id, 1, idusuario);
                            }
                            if (xOrdenMuestraRom.listaParaReferenciarCrear.Count > 0)
                            {
                                ordenMuestraDal.RecepcionarMuestras(xOrdenMuestraRom.listaParaReferenciarCrear, id, 1, idusuario);
                            }

                        }
                    }

                    //Crear y Referenciar muestras 
                    //if (listaOrdenesMuestraCrearReferenciar.Count > 0)
                    //{
                    //    ordenMuestraBl.RegistrarMuestras(listaOrdenesMuestraCrearReferenciar, Guid.Parse(id), Logueado.idUsuario);
                    //}

                    //Crear y Recepcionar Muestras
                    if (xOrdenMuestraRom.listaOrdenesMuestraCrearRecepcionar.Count > 0)
                    {
                        //Obtener Lista de Examenes válidos por el laboratorio seleccionado
                        var laboratorioExamenBl = new LaboratorioExamenBl();
                        var TipoMuestraExamenBl = new ExamenTipoMuestraBl();
                        var listaExamenesValidosTmp = laboratorioExamenBl.GetExamenesByLaboratorio(IdEstablecimiento);

                        if (listaExamenesValidosTmp != null)
                        {
                            foreach (var itemR in xOrdenMuestraRom.listaOrdenesMuestraCrearRecepcionar)
                            {
                                var listaTipoMuestraValidosTmp = TipoMuestraExamenBl.GetTipoMuestraByExamen(itemR.idExamen);
                                var tipomuestraValido = listaTipoMuestraValidosTmp.FindAll(p => p.IdTipoMuestra == itemR.idTipoMuestra);//juanmuga

                                var examenValido = listaExamenesValidosTmp.FindAll(p => p.IdExamen == itemR.idExamen);

                                if (examenValido.Any() && tipomuestraValido.Any())//juan muga
                                {
                                    xOrdenMuestraRom.listaParaRecepcionarCrear.Add(itemR);
                                    totalMuestrasValidasR = totalMuestrasValidasR + 1;
                                }
                                else
                                {
                                    itemR.fechaEnvio = null;
                                    itemR.horaEnvio = null;
                                    itemR.fechaRecepcion = null;
                                    itemR.horaRecepcion = null;
                                    xOrdenMuestraRom.listaParaRecepcionarCrearSinRecepcion.Add(itemR);
                                    totalMuestrasInvalidasR = totalMuestrasInvalidasR + 1;
                                }
                            }
                        }

                        //Crear y Recepcionar Muestras
                        if (xOrdenMuestraRom.listaParaRecepcionarCrear.Any())
                        {
                            totalMuestrasRecepcionadas = xOrdenMuestraRom.listaParaRecepcionarCrear.Count;
                            ordenMuestraDal.RegistrarMuestrasConRecepcion(xOrdenMuestraRom.listaParaRecepcionarCrear, id, idusuario);
                        }

                        //Crear Muestras - NO RECEPCIONA
                        if (!xOrdenMuestraRom.listaParaRecepcionarCrearSinRecepcion.Any())
                        {
                            ordenMuestraDal.Rollback();
                            return totalMuestrasInvalidasR + "_" + totalMuestrasRecepcionadas + "_" + totalMuestras + "_" + ExamenError + "_ ";
                        }
                        totalMuestrasRecepcionadas = xOrdenMuestraRom.listaParaRecepcionarCrearSinRecepcion.Count;
                        ordenMuestraDal.RegistrarMuestrasSinRecepcion(xOrdenMuestraRom.listaParaRecepcionarCrearSinRecepcion, id, idusuario);
                    }
                    if (totalMuestrasInvalidasR == 0)
                    {
                        ordenMuestraDal.Commit();
                    }
                    else
                    {
                        ordenMuestraDal.Rollback();
                        return totalMuestrasInvalidasR + "_" + totalMuestrasRecepcionadas + "_" + totalMuestras + "_" + ExamenError;
                    }

                }
                catch (Exception ex)
                {
                    ordenMuestraDal.Rollback();
                    Respuesta = ex.Message;
                }
            }


            return Respuesta;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipo">1: Referenciar
        ///                    2: Recepcionar
        ///                    3: Derivar
        /// </param>
        /// <param name="LstOrdenes"></param>
        /// <returns></returns>
        public int ValidaExamenes(int tipo, List<OrdenMuestraRecepcion> LstOrdenes)
        {
            int Resultado = 0;
            //Obtener Lista de Examenes válidos por el laboratorio seleccionado
            var laboratorioExamenBl = new LaboratorioExamenBl();
            var TipoMuestraExamenBl = new ExamenTipoMuestraBl();
            List<ExamenLaboratorio> listaExamenesValidos = new List<ExamenLaboratorio>();

            if (LstOrdenes != null)
            {
                foreach (var itemR in LstOrdenes)
                {
                    var listaTipoMuestraValidosTmp = TipoMuestraExamenBl.GetTipoMuestraByExamen(itemR.idExamen);
                    var examenValido = laboratorioExamenBl.GetExamenesByLaboratorio(itemR.idLaboratorioDestino).FindAll(p => p.IdExamen == itemR.idExamen);
                    var tipomuestraValido = listaTipoMuestraValidosTmp.FindAll(p => p.IdTipoMuestra == itemR.idTipoMuestra);

                    if (!examenValido.Any() || !tipomuestraValido.Any() && tipo > 1)
                    {
                        Resultado = Resultado + 1;
                        //JRCR - 18Marzo2018
                        //Session["ExamenError"] = examenValido.FirstOrDefault().Examen.nombre.ToString();
                        ExamenError = new ExamenBl().GetExamenById(itemR.idExamen).nombre;
                    }
                }
            }

            return Resultado;
        }

        /// Descripción: Muestra el detalle de la Orden referenciada.
        /// Author: Marcos Mejia. 
        /// Fecha Creacion: 27/06/2019
        public List<OrdenRecepcionDetalleVd> DetalleOrdenReferencia(int TipoFecha, string codigoOrden, DateTime FechaDesde, DateTime Fechahasta,
                                                                    string NroOficio, string idMuestra, int idLaboratorio)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.DetalleOrdenReferencia(TipoFecha, codigoOrden, FechaDesde, Fechahasta, NroOficio, idMuestra, idLaboratorio);
            }

        }
        public List<OrdenMuestrasRechazo> GetOrdenCriterioRechazo(Guid idOrden)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.GetOrdenCriterioRechazo(idOrden);
            }
        }
        public void RegistrarLevantarObservacion(string hddRECHAZOFECHAINGRESOROM, string hddRECHAZOPACIENTE, string hddRECHAZODATOCLINICO, string hddRECHAZOCODIFICACION, string hddRECHAZOFECHAEVALFICHA, string hddRECHAZOFECHASOL, string hddRECHAZOFECHAOBTENCIONMUESTRA, string hddRECHAZOSOLICITANTE, string hddRECHAZONROOFICIO, Orden orden)
        {
            var bFechaRom = Convert.ToBoolean(hddRECHAZOFECHAINGRESOROM);
            var bPaciente = Convert.ToBoolean(hddRECHAZOPACIENTE);
            var bDatoClinico = Convert.ToBoolean(hddRECHAZODATOCLINICO);
            var bFechaEval = Convert.ToBoolean(hddRECHAZOFECHAEVALFICHA);
            var bFechaSol = Convert.ToBoolean(hddRECHAZOFECHASOL);
            var bFechaObtencion = Convert.ToBoolean(hddRECHAZOFECHAOBTENCIONMUESTRA);
            var bSolicitante = Convert.ToBoolean(hddRECHAZOSOLICITANTE);
            var bNroOficio = Convert.ToBoolean(hddRECHAZONROOFICIO);

            List<int> ListaRechazos = new List<int>();
            if (bFechaRom)
            {
                ListaRechazos.Add((int)Enums.TipoRechazoAlta.RECHAZOFECHAINGRESOROM);
            }
            if (bPaciente)
            {
                ListaRechazos.Add((int)Enums.TipoRechazoAlta.RECHAZOPACIENTE);
            }
            if (bDatoClinico)
            {
                ListaRechazos.Add((int)Enums.TipoRechazoAlta.RECHAZODATOCLINICO);
            }
            if (bFechaEval)
            {
                ListaRechazos.Add((int)Enums.TipoRechazoAlta.RECHAZOFECHAEVALFICHA);
            }
            if (bFechaSol)
            {
                ListaRechazos.Add((int)Enums.TipoRechazoAlta.RECHAZOFECHASOL);
            }
            if (bFechaObtencion)
            {
                ListaRechazos.Add((int)Enums.TipoRechazoAlta.RECHAZOFECHAOBTENCIONMUESTRA);
            }
            if (bSolicitante)
            {
                ListaRechazos.Add((int)Enums.TipoRechazoAlta.RECHAZOSOLICITANTE);
            }
            if (bNroOficio)
            {
                ListaRechazos.Add((int)Enums.TipoRechazoAlta.RECHAZONROOFICIO);
            }
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {

                ordenMuestraDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    if (bFechaRom || bPaciente || bFechaEval || bFechaSol || bSolicitante || bNroOficio)
                    {
                        ordenMuestraDal.UpdateOrdenRechazo(orden); //Error de fecha!!!!!!                        
                    }
                    if (bDatoClinico)
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
                                ordenMuestraDal.InsertOrdenDatoClinicoRechazo(ordenDatoClinico);
                            }

                        }
                    }
                    if (bFechaObtencion)
                    {
                        ordenMuestraDal.UpdateOrdenMuestraRechazo(orden);
                    }
                    foreach (var item in ListaRechazos)
                    {
                        ordenMuestraDal.UpdateOrdenMuestraResultadoRechazo(orden, item);
                    }

                    ordenMuestraDal.Commit();
                }
                catch (Exception ex)
                {
                    ordenMuestraDal.Rollback();
                    //ValidateOrderStateOnException(ex);
                }
            }
        }
        public List<OrdenRecepcionVd> GetOrdenMuestraIngresada(string codigoOrden, string nroOficio, string codigoMuestra)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.GetOrdenMuestraIngresada(codigoOrden, nroOficio, codigoMuestra);
            }
        }

        public string VerificarMuestrasMasivo2(List<ValidaResultadoMasivo> comentarioList, int idUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.VerificarMuestrasMasivo(comentarioList, idUsuario);
            }
        }
    }
}

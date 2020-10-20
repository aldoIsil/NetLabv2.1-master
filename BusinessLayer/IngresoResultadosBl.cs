using DataLayer;
using System;
using System.Collections.Generic;
using Model;
using Model.ViewData;
using System.Data.SqlClient;

namespace BusinessLayer
{
    public class IngresoResultadosBl
    {
        /// <summary>
        /// Descripción: Metodo para obtener muestras-ordenes para ingresar resultados por usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <param name="idUsuario"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroOficio"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="estadoResultado"></param>
        /// <returns></returns>
        public List<OrdenIngresarResultadoVd> GetOrdenMuestraResultadosByUser(
            int tipo, int idEstablecimientoLogin, int idUsuario, string nroDocumento,
            DateTime fechaDesde, DateTime fechaHasta, string nroOficio, string codigoOrden, string codigoMuestra, int estadoResultado, int idEnfermedad, Guid idExamen)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.GetOrdenMuestraResultadosByUser(
                    tipo, idEstablecimientoLogin, idUsuario, nroDocumento, fechaDesde, fechaHasta,
                    nroOficio, codigoOrden, codigoMuestra, estadoResultado, idEnfermedad, idExamen);
            }
        }
        /// <summary>
        /// Descripción: Obtener informacion de muestras validadas para el ingreso de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd GetMuestrasValidarResultados(Guid idOrden, int idUsuario, int idEstablecimientoLogin)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestrasValidarResultados(idOrden, idUsuario, idEstablecimientoLogin);
            }
        }
        /// <summary>
        /// Descripción: Obtener informacion de muestras para rechazar examen
        /// Author: Juan Muga
        /// Fecha Creacion: 04/03/2019
        /// Fecha Modificación: 
        /// Modificación: 
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd GetRechazarExamen(Guid idOrden, int idUsuario, int idEstablecimientoLogin)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.RechazarExamen(idOrden, idUsuario, idEstablecimientoLogin);
            }
        }
        /// <summary>
        /// Descripción: Obtener informacion de muestras pendientes de ingreso de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd GetMuestrasPendientesRecepcionLAB(Guid idOrden, int idUsuario, int idEstablecimientoLogin)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestrasPendientesRecepcionLAB(idOrden, idUsuario, idEstablecimientoLogin);
            }
        }

        /// <summary>
        /// Descripción: Obtener información de muestras validadas para el ingreso de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd GetMuestrasValidadas(Guid idOrden, int idUsuario, int idEstablecimientoLogin)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestrasValidadas(idOrden, idUsuario, idEstablecimientoLogin);
            }
        }
        /// <summary>
        /// Descripción: Obtener información del area de procesamiento a la que pertence una orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd GetAreaProcesamientoOrdenUsuario(Guid idOrdenExamen, int idUsuario, int idEstablecimientoSeleccionado)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.AreaProcesamientoOrdenResultados(idOrdenExamen, idUsuario, idEstablecimientoSeleccionado);
            }
        }
        /// <summary>
        /// Descripción: Obtener informacion de la orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd DatosOrden(Guid idOrden)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.DatosOrden(idOrden);
            }
        }
        /// <summary>
        /// Descripción: Obtener informacion de los resultados de las pruebas para validar el mismo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idAreaProcesamiento"></param>
        /// <param name="idUsuario"></param>
        /// <param name="edad"></param>
        /// <param name="genero"></param>
        /// <param name="idLaboratorioUsuario"></param>
        /// <returns></returns>
        public List<OrdenExamenResultadosVd> ExamenesResultadosAnalito(
            Guid idOrden, Guid idOrdenExamen, int idAreaProcesamiento, int idUsuario, int edad, int genero, int idLaboratorioUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestraResultadosValidar(idOrden, idOrdenExamen, idAreaProcesamiento, idUsuario, edad, genero, idLaboratorioUsuario);
            }
        }
        /// <summary>
        /// Descripción: Obtiene el Id-Guid del resultado de un examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public List<Guid> GetOrdenResultadoAnalito(Guid idOrdenExamen)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.ObtenerResultadoAnalito(idOrdenExamen);
            }
        }
        /// <summary>
        /// Descripción: Registra los resultados de la prueba ejecutada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="resultados"></param>
        /// <param name="idUsuario"></param>
        public void RegistrarResultadosOrdenAnalito(List<OrdenResultadoAnalito> resultados, int idUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.RegistrarResultadosOrdenAnalito(resultados, idUsuario);
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
        /// <param name="criterioRechazo"></param>
        public void RegistroResultados(List<CriterioRechazo> resultados)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                foreach (CriterioRechazo item in resultados)
                {
                    ordenMuestraDal.InsertOrdenMuestraResultadoRechazo(item);
                }
            }
        }
        /// <summary>
        /// Descripción: Registra los resultados de la prueba ejecutada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenMuestraRecepcion"></param>
        /// <param name="conforme"></param>
        /// <param name="idUsuario"></param>
        public void ActualizaRecepcionConformeResultado(Guid idOrdenMuestraRecepcion, int conforme, int idUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.UpdateEstadoRecepcionResultado(idOrdenMuestraRecepcion, conforme, idUsuario);
            }
        }

        /// <summary>
        /// Descripción: Registra los recepcion de la muestra en Labortorio 
        /// Estado = 5
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenMuestraRecepcion"></param>
        /// <param name="idUsuario"></param>
        public string ActualizaOrdenMuestraRecepcionLAB(List<MuestraResultadoVd> oLstMuestraRecepcion)
        {
            var result = "Ok";
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var item in oLstMuestraRecepcion)
                    {
                        ordenMuestraDal.UpdateOrdenMuestraRecepcionLAB(item.idOrdenMuestraRecepcion, item.idUsuario, item.secuenObtencion);
                    }
                    ordenMuestraDal.Commit();
                }
                catch (SqlException sqlex)
                {
                    ordenMuestraDal.Rollback();
                    result = sqlex.Message;
                }
                catch (Exception ex)
                {
                    ordenMuestraDal.Rollback();
                    result = ex.Message;
                }
            }
            return result;
        }
        /// <summary>
        /// Descripción: Registra los resultados rechazados por cada prueba en Labortorio 
        /// Estado = 1
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="criterioRechazo"></param>
        public void RegistroCriterioRechazoResultado(CriterioRechazo criterio)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.InsertOrdenMuestraResultadoRechazo(criterio);
            }
        }

        /// <summary>
        /// Descripción: SOLICITAR NUEVO INGRESO DE RESULTADO POR EL VERIFICADOR.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// /// <param name="estatusSol"></param>
        /// <returns></returns>
        public void SolicitaIngresoNvoResultados(Guid idOrdenExamen, int idUsuario, int estatusSol)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.SolicitaNvoIngresoResultados(idOrdenExamen, idUsuario, estatusSol);
            }
        }

        /// <summary>
        /// Descripción: Permite obtener información de los materiales ingresados para una orden para el detalle.
        /// Author: Jose Chavez.
        /// Fecha Creacion: 05/12/2017
        /// Fecha Modificación: 05/12/2017.
        /// </summary>
        /// <returns></returns>
        public List<OrdenRecepcionDetalleVd> MuestrasByOrdenDetalle(int idEstablecimientoUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                return ordenMuestraDal.MuestrasByOrdenDetalle(idEstablecimientoUsuario);
            }
        }
        public String AddExamenAnalistaPlantilla(Guid idOrden, int idEstablecimiento, int idUsuario, int idplantilla)
        {
            string result = "Proceso Completado.";
            using (var resultadoDal = new ResultadosDal())
            {
                resultadoDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    result = resultadoDal.AddExamenAnalistaPlantilla(idOrden,idEstablecimiento, idUsuario, idplantilla);
                    resultadoDal.Commit();
                }
                catch (Exception ex)
                {
                    resultadoDal.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        public void RegistoResultadoAnalitoDetalle(List<OrdenResultadoAnalito> resultados)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.RegistoResultadoAnalitoDetalle(resultados);
            }
        }

        public void RegistrarFechaSiembra(string idOrdenExamen, int idUsuario)
        {
            using (var ordenMuestraDal = new OrdenMuestraDal())
            {
                ordenMuestraDal.RegistrarFechaSiembra(idOrdenExamen, idUsuario);
            }
        }

        //public void RechazoFechaRom(string nroOficio, int idUsuario, int idCriterioRechazo)
        //{
        //    using (var ordenMuestraDal = new OrdenMuestraDal())
        //    {
        //        ordenMuestraDal.RechazoFechaRom(nroOficio, idUsuario, idCriterioRechazo);
        //    }
        //}

    }
}

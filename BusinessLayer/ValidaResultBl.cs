using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using System;
using Model;

namespace BusinessLayer
{
    public class ValidaResultBl : IValidaResultBl
    {
          /// <summary>
        /// Descripción: Obtiene el listado de las muestras listas para que el analista confirme el resultado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="fechaSolicitud"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroOficio"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="idLaboratorio"></param>
        /// <param name="estadoResultado"></param>
        /// <returns></returns>
        public List<ValidaResult> GetValidaciones(int idUsuarioIngreso, int fechaSolicitud, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, string nroDocumento, int idLaboratorio, int estadoResultado, int idEnfermedad, Guid idExamen)
        {          
            using (var validaResultDal = new ValidaResultDal())
            {
                return validaResultDal.GetValidaciones(idUsuarioIngreso, fechaSolicitud, codigoOrden, fechaDesde, fechaHasta, nroOficio, nroDocumento, idLaboratorio, estadoResultado, idEnfermedad, idExamen);
            }
        }
        /// <summary>
        /// Descripción: Obtiene el listado de las muestras listas para que el RESPONSABLE DE LABORTORIO LIEBRE el resultado.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 28/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="fechaSolicitud"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroOficio"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<ValidaResult> GetValidacionesSolicitudes(int idUsuarioIngreso, int fechaSolicitud, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, string nroDocumento, int idLaboratorio, int estado)
        {
            using (var validaResultDal = new ValidaResultDal())
            {
                return validaResultDal.GetValidacionesSolicitudes(idUsuarioIngreso, fechaSolicitud, codigoOrden, fechaDesde, fechaHasta, nroOficio, nroDocumento, idLaboratorio, estado);
            }
        }

    }
}

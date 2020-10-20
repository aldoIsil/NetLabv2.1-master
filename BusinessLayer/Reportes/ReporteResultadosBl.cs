using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Interfaces;
using DataLayer.Reportes;
using Model;

namespace BusinessLayer.Reportes
{
    public class ReporteResultadosBl : IReporteResultadosBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de la Orden y sus resultados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public OrdenResultado GetOrdenResultado(Guid idOrden)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                return reporteResultadoDal.GetOrdenResultado(idOrden);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de la Orden y sus resultados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorioDestino"></param>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public OrdenResultado GetOrdenResultado(Guid idOrden, int idLaboratorioDestino, string[] idOrdenExamen, int idUsuario)
        {
            if (idOrdenExamen == null || !idOrdenExamen.Any())
                return new OrdenResultado();

            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                return reporteResultadoDal.GetOrdenResultado(idOrden, idLaboratorioDestino, string.Join(",", idOrdenExamen),idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de la Orden y sus resultados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idExamen"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<MuestraResultado> GetMuestras(Guid idOrden, Guid idExamen, int idLaboratorio)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                return reporteResultadoDal.GetMuestras(idOrden, idExamen, idLaboratorio);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las muestras filtrado por el Codigo de Orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorioDestino"></param>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public List<MuestraResultado> GetMuestrasbyIdOrden(Guid idOrden, int idLaboratorioDestino, string[] idOrdenExamen)
        {
            if (idOrdenExamen == null || !idOrdenExamen.Any())
                return new List<MuestraResultado>();

            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                return reporteResultadoDal.GetMuestrasbyIdOrden(idOrden, idLaboratorioDestino, string.Join(",", idOrdenExamen));
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las pruebas realizadas a una orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<ExamenResultado> GetExamenes(Guid idOrden)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                var examenes = reporteResultadoDal.GetExamenes(idOrden);

                foreach (var examen in examenes)
                {
                    examen.Detalle = reporteResultadoDal.GetOldExamenDetalle(idOrden, examen.IdOrdenExamen);
                }

                return examenes;
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion del detalle de las pruebas realizadas a una orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<ExamenResultadoDetalle> GetDetalleExamenes(Guid idOrden)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                var detalles = reporteResultadoDal.GetExamenDetalle(idOrden);

                return detalles;
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion del detalle de las pruebas realizadas a una orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<ExamenResultadoDetalle> GetDetalleExamenes(Guid idOrden, Guid idExamen)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                var detalles = reporteResultadoDal.GetExamenDetalle(idOrden, idExamen);

                return detalles;
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion del detalle de las pruebas realizadas a una orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorioDestino"></param>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public List<ExamenResultadoDetalle> GetDetalleExamenes(Guid idOrden, int idLaboratorioDestino, string[] idOrdenExamen)
        {
            if (idOrdenExamen == null || !idOrdenExamen.Any())
                return new List<ExamenResultadoDetalle>();

            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                var detalles = reporteResultadoDal.GetExamenDetalle(idOrden, idLaboratorioDestino, string.Join(",", idOrdenExamen));

                return detalles;
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion la interpretacion de resultados de una orden
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 10/06/2018
        public List<ExamenResultadoInterpretacion> GetInterpretacionExamen(Guid[] idOrdenExamen)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                var detalles = reporteResultadoDal.GetExamenInterpretacion(string.Join(",", idOrdenExamen));

                return detalles;
            }
        }


        /*yrca*/

        public List<ExamenResultadoInterpretacion> GetProductividadRom(Guid[] idOrdenExamen)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                var detalles = reporteResultadoDal.GetExamenInterpretacion(string.Join(",", idOrdenExamen));

                return detalles;
            }
        }


        public ResultadoKobos GetResultadoKobosId(int id, int idUsuario)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                return reporteResultadoDal.GetResultadoKobosId(id,idUsuario);
            }
        }

        public CargoUsuarioEstablecimiento CargoUsuarioEstablecimiento(int idEstablecimiento, string fechaVerificacion)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                return reporteResultadoDal.CargoUsuarioEstablecimiento(idEstablecimiento, fechaVerificacion.Substring(0,10));
            }
        }
        public OrdenResultado GetOrdenResultadoWS(Guid idOrdenExamen)
        {
            using (var reporteResultadoDal = new ReporteResultadoDal())
            {
                return reporteResultadoDal.GetOrdenResultadoWS(idOrdenExamen);
            }
        }
    }
}
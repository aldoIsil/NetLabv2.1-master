using DataLayer;
using System;
using Model;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class ResultadosBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de la orden y la prueba.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorioUsuario"></param>
        /// <returns></returns>
        public Orden GetOrdenById(Guid idOrden, string idOrdenExamen, int idLaboratorioUsuario, int idusuario)
        {
            Orden orden;

            using (var resultadoDal = new ResultadosDal())
            {
                orden = resultadoDal.GetOrdenById(idOrden, idOrdenExamen, idLaboratorioUsuario, idusuario);
            }

            return orden;
        }
        /// Descripción: Obtiene informacion del solicitante y paciente de la Orden.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 30/04/2018
        public EnvioAlerta GetDatosCorreo(string idOrdenExamen)
        {
            EnvioAlerta alerta;
            using (var correoDal = new ResultadosDal())
            {
                alerta = correoDal.GetDatosCorreo(idOrdenExamen);
            }
            return alerta;
        }
        /// <summary>
        /// Descripción: Ejecuta la transaccion para la actualizacion y validacion de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <param name="comentario"></param>
        /// <param name="esConforme"></param>
        /// <param name="idUsuario"></param>
        public void UpdateDatosResultados(Guid idOrdenExamen, string comentario, int esConforme, int idUsuario)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                resultadosDal.UpdateResultado(idOrdenExamen, comentario, esConforme, idUsuario);
            }
        }

        public string UpdateResultadoPruebaRapida(Guid idOrdenExamen, string comentario, int esConforme, int idUsuario)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                return resultadosDal.UpdateResultadoPruebaRapida(idOrdenExamen, comentario, esConforme, idUsuario);
            }
        }

        public void RegistroEnvioSms(string phone, string message, int tipo)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                resultadosDal.EnvioSMS(phone, message, tipo);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las ordenes
        /// Author: Juan Muga.
        /// Fecha Creacion: 25/07/2018
        /// </summary>
        /// <param name="codigoOrden"></param>
        /// <param name="muestra"></param>
        /// <param name="TipoMuestra"></param>
        /// <param name="idEstablecimientoDestino"></param>
        /// <returns></returns>
        public ResultAnalito OrdenAnalitoResultadobyCodigoOrden(string codigoOrden, string muestra, string TipoMuestra, int idEstablecimientoDestino, string NombreExamen = null)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                return resultadosDal.OrdenAnalitoResultadobyCodigoOrden(codigoOrden, muestra, TipoMuestra, idEstablecimientoDestino, NombreExamen);
            }
        }

        public string ProcesoMasivoLabCodigoMuestra(int tipoProcesoMasivo, string muestraCodificacion, int idestablecimiento, int idUsuario, string UsuarioRom, string metodoKit, string nroSolicitud)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                if (tipoProcesoMasivo == 1)
                {
                    return resultadosDal.RecepcionarCodigoMuestra(muestraCodificacion, idestablecimiento, idUsuario, UsuarioRom, nroSolicitud);
                }
                if (tipoProcesoMasivo == 2)
                {
                    return resultadosDal.ValidarCodigoMuestra(muestraCodificacion, idestablecimiento, idUsuario, metodoKit);
                }
                if (tipoProcesoMasivo == 3)
                {
                    return resultadosDal.RecepcionarValidarCodigoMuestra(muestraCodificacion, idestablecimiento, idUsuario, metodoKit);
                }
                else
                {
                    return null;
                }
            }
        }
        public ResultAnalito getOrdenAnalitoResultadobyCodigoExamen(string codigoExamenNetLab, string idExamen)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                return resultadosDal.getOrdenAnalitoResultadobyCodigoExamen(codigoExamenNetLab, idExamen);
            }
        }

        public ResultadoCovidPaciente ObtenerResultadoCovidPorOrden(Guid idOrden)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                return resultadosDal.ObtenerResultadoCovidPorOrden(idOrden);
            }
        }

        public void InsertarResultadoCovidFallido(ResultadoCovidPaciente datos, int idUsuario, string motivofalla)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                resultadosDal.InsertarResultadoCovidFallido(datos, idUsuario, motivofalla);
            }
        }

        public ResultadoCovidPaciente ObtenerResultadoCovidPorOrdenExamen(Guid idOrdenExamen)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                return resultadosDal.ObtenerResultadoCovidPorOrdenExamen(idOrdenExamen);
            }
        }
        public List<Protocolo> ValidaCodigoLinealProtocolo(string codigoMuestra)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                return resultadosDal.ValidaCodigoLinealProtocolo(codigoMuestra);
            }
        }
        public void RegistraCodigoLinealProtocoloMASED(List<Protocolo> lstProtocolo, int idUsuario)
        {
            using (var resultadosDal = new ResultadosDal())
            {
                foreach (var item in lstProtocolo)
                {
                    if (item.muestra_con_resultado == "SI" )
                    {
                        item.observacion = "muestra_con_resultado";
                    }
                    else
                    {
                        if (item.muestra_sin_recepcionar == "SI")
                        {
                            item.observacion = "muestra_sin_recepcionar";
                        }
                    }
                    resultadosDal.RegistraCodigoLinealProtocoloMASED(item, idUsuario);
                }
                
            }
        }
    }
}

using DataLayer;
using System;
using System.Collections.Generic;
using Model;
using Model.ViewData;
using System.Data;

namespace BusinessLayer
{
    public class ConsultaResultadosBl
    {
        /// <summary>
        /// Descripción: Metodo para obtener información de consultas externas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nombrePaciente"></param>
        /// <param name="apellidoPaciente"></param>
        /// <param name="apellidoPaciente2"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="enfermedades"></param>
        /// <param name="nroOficio"></param>
        /// <param name="esTipoEstablecimiento"></param>
        /// <param name="examen"></param>
        /// <param name="tipoPaciente"></param>
        /// <param name="esTipoUbigueo"></param>
        /// <param name="actualDepartamento"></param>
        /// <param name="actualProvincia"></param>
        /// <param name="actualDistrito"></param>
        /// <param name="estadoResultado"></param>
        /// <param name="ordenarPor"></param>
        /// <param name="tipoOrdenacion"></param>
        /// <param name="EstablecimientoCadena"></param>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="hdnEstablecimiento"></param>
        /// <returns></returns>
        public List<OrdenIngresarResultadoVd> GetOrdenMuestraResultadosByUser(int idUsuario, int esFechaRegistro, string fechaDesde, string fechaHasta,
                                                                              string nroDocumento, string nombrePaciente, string apellidoPaciente, string apellidoPaciente2,
                                                                              string codigoMuestra, string codigoOrden, string enfermedades, string nroOficio, int? esTipoEstablecimiento,
                                                                              string examen, int? tipoPaciente, int? esTipoUbigueo, string actualDepartamento, string actualProvincia,
                                                                              string actualDistrito, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion, string EstablecimientoCadena,
                                                                              string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, string hdnEstablecimiento)
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.GetOrdenesConsultaExterna(idUsuario, esFechaRegistro, fechaDesde, fechaHasta, nroDocumento, nombrePaciente, apellidoPaciente,
                                                          apellidoPaciente2, codigoMuestra, codigoOrden, enfermedades, nroOficio, esTipoEstablecimiento, examen,
                                                          tipoPaciente, esTipoUbigueo, actualDepartamento, actualProvincia, actualDistrito,
                                                          estadoResultado, ordenarPor, tipoOrdenacion, EstablecimientoCadena,
                                                          hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, hdnEstablecimiento);
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
        /// Descripción: Metodo para obtener la informacion de los datos clinicos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nombrePaciente"></param>
        /// <param name="apellidoPaciente"></param>
        /// <param name="apellidoPaciente2"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="enfermedades"></param>
        /// <param name="nroOficio"></param>
        /// <param name="esTipoEstablecimiento"></param>
        /// <param name="examen"></param>
        /// <param name="tipoPaciente"></param>
        /// <param name="esTipoUbigueo"></param>
        /// <param name="actualDepartamento"></param>
        /// <param name="actualProvincia"></param>
        /// <param name="actualDistrito"></param>
        /// <param name="estadoResultado"></param>
        /// <param name="ordenarPor"></param>
        /// <param name="tipoOrdenacion"></param>
        /// <param name="EstablecimientoCadena"></param>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="hdnEstablecimiento"></param>
        /// <returns></returns>
        public DataTable dtDatosClinicos(int idUsuario, int esFechaRegistro, string fechaDesde, string fechaHasta,
            string nroDocumento, string nombrePaciente, string apellidoPaciente, string apellidoPaciente2, string codigoMuestra, string codigoOrden,
            string enfermedades, string nroOficio, int? esTipoEstablecimiento, string examen, int? tipoPaciente, int? esTipoUbigueo, string actualDepartamento,
            string actualProvincia, string actualDistrito, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion, string EstablecimientoCadena,
            string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, string hdnEstablecimiento)
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.DatosClinicos(idUsuario, esFechaRegistro, fechaDesde, fechaHasta, nroDocumento, nombrePaciente, apellidoPaciente, apellidoPaciente2, codigoMuestra,
                                              codigoOrden, enfermedades, nroOficio, esTipoEstablecimiento, examen, tipoPaciente, esTipoUbigueo, actualDepartamento, actualProvincia,
                                              actualDistrito, estadoResultado, ordenarPor, tipoOrdenacion, EstablecimientoCadena, hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed,
                                              hdnEstablecimiento);
            }

        }
        /// <summary>
        /// Descripción: Metodo para obtener información de consultas de resultados de un paciente.
        /// Autor: Marcos Mejia.
        /// Fecha Creacion: 15/07/2018
        public List<OrdenIngresarResultadoVd> ConsultaReporteResultado(string fechaDesde, string fechaHasta, string nroDocumento, string codigoOrden, int idUsuario)
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.ConsultaReporteResultado(fechaDesde, fechaHasta, nroDocumento, codigoOrden, idUsuario);
            }
        }




        //public List<OrdenIngresarResultadoVd> GetOrdenMuestraResultadosByUser(int idUsuario, int esFechaRegistro, DateTime fechaDesdeA, DateTime fechaHastaA,
        //    string nroDocumento, string nombrePaciente, string apellidoPaciente, string apellidoPaciente2, string codigoMuestra, string codigoOrden,
        //    string enfermedades, string nroOficio, int? esTipoEstablecimiento, string examen, int? tipoPaciente, int? esTipoUbigueo, string actualDepartamento,
        //    string actualProvincia, string actualDistrito, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion)
        //{
        //    throw new NotImplementedException();
        //}
        /*
public List<OrdenExamenResultadosVd> ExamenesResultadosAnalito(Guid idOrden, int idAreaProcesamiento, int idUsuario, int edad, int genero)
{
   using (var ordenMuestraDal = new OrdenMuestraDal())
   {
       return ordenMuestraDal.MuestraResultadosValidar(idOrden, idAreaProcesamiento, idUsuario, edad, genero);
   }
}*/
        public DataTable GetPruebaDatosClinicos(int idUsuario, int esFechaRegistro, string fechaDesde, string fechaHasta,
                                                                              string nroDocumento, string nombrePaciente, string apellidoPaciente, string apellidoPaciente2,
                                                                              string codigoMuestra, string codigoOrden, string enfermedades, string nroOficio, int? esTipoEstablecimiento,
                                                                              string examen, int? tipoPaciente, int? esTipoUbigueo, string actualDepartamento, string actualProvincia,
                                                                              string actualDistrito, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion, string EstablecimientoCadena,
                                                                              string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, string hdnEstablecimiento)
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.GetPruebaDatosClinicos(idUsuario, esFechaRegistro, fechaDesde, fechaHasta, nroDocumento, nombrePaciente, apellidoPaciente,
                                                          apellidoPaciente2, codigoMuestra, codigoOrden, enfermedades, nroOficio, esTipoEstablecimiento, examen,
                                                          tipoPaciente, esTipoUbigueo, actualDepartamento, actualProvincia, actualDistrito,
                                                          estadoResultado, ordenarPor, tipoOrdenacion, EstablecimientoCadena,
                                                          hdnInstitucion, hdnDisa, hdnRed, hdnMicroRed, hdnEstablecimiento);
            }
        }

        /// Descripción: Permite realizar la busqueda de resultados según el Semáforo
        /// Author: Terceros.
        /// Fecha Creacion: 10/06/2019
        /// Modificación: Se agregaron comentarios.
        public List<OrdenIngresarResultadoVd> ConsultaSemaforoResultados(int tipoConsulta, int idEstablecimiento, string fechaDesde, string fechaHasta, string codigoOrden,
        string codigoMuestra, string nroOficio, int idEnfermedad, string idExamen, string nroDocumento, string nombre, string apellidoPaterno, string apellidoMaterno,
        int tipoOrden, int Semaforo)
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.ConsultaSemaforoResultados(tipoConsulta, idEstablecimiento, fechaDesde, fechaHasta, codigoOrden, codigoMuestra, nroOficio, idEnfermedad, idExamen,
                    nroDocumento, nombre, apellidoPaterno, apellidoMaterno, tipoOrden, Semaforo);
            }
        }

        //public List<EnfermedadResultados> GetOrdenesConsultaResultados(int tipoFecha, string fechaDesde, string fechaHasta, int idEnfermedad, int idExamenAgrupado, int estado)
        //{
        //    var resultado = new List<EnfermedadResultados>();
        //    using (var ordenDal = new OrdenDal())
        //    {
        //        return ordenDal.GetOrdenesConsultaResultados(tipoFecha, fechaDesde, fechaHasta, idEnfermedad, idExamenAgrupado, estado);
        //    }
            
        //}

        public DataTable GetOrdenesConsultaResultadosDC(int tipoFecha, string fechaDesde, string fechaHasta, string esDatoClinico, int idEnfermedad, int idExamenAgrupado, int estado)
        {
            var resultado = new List<EnfermedadResultados>();
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.GetOrdenesConsultaResultadosDC(tipoFecha, fechaDesde, fechaHasta, esDatoClinico, idEnfermedad, idExamenAgrupado, estado);
            }

        }

        public List<OrdenIngresarResultadoVd> ObtenerDatosNominalesCovidPorUsuario(int idUsuario, int esFechaRegistro, string fechaDesde, string fechaHasta, string codigoMuestra, string codigoOrden, string nroOficio)
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.ObtenerDatosNominalesCovidPorUsuario(idUsuario, esFechaRegistro, fechaDesde, fechaHasta, codigoMuestra, codigoOrden, nroOficio);
            }
        }

        //public List<ResultadosINS> GetConsultaResultadosRecepcionINS(string idEnfermedad, string fechaDesde, string fechaHasta)
        //{
        //    using (var ordenDal = new OrdenDal())
        //    {
        //        return ordenDal.GetConsultaResultadosRecepcionINS(idEnfermedad, fechaDesde, fechaHasta);
        //    }
        //}
        public DataTable GetConsultaResultadosRecepcionINS(string idEnfermedad, string fechaDesde, string fechaHasta)
        {
            using (var ordenDal = new OrdenDal())
            {
                return ordenDal.GetConsultaResultadosRecepcionINS(idEnfermedad, fechaDesde, fechaHasta);
            }
        }
    }
}

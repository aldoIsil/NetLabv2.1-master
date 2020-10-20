using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.DalConverter;
using Model;
using System.Linq;
using Model.ViewData;
using DataLayer.Compartido;

namespace DataLayer.PEED
{
    public class IngresoResultadosControlCalidadDal : DaoBase
    {
        public IngresoResultadosControlCalidadDal(IDalSettings settings) : base(settings)
        {
        }

        public IngresoResultadosControlCalidadDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de algunos resultados ingresados para un eess evaluado
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="objIngresoResultadosPEEC"></param>
        /// <returns></returns>
        public List<IngresoResultadosPEEC> GetIngresoResultadoControlCalidad(IngresoResultadosPEEC objIngresoResultadosPEEC)
        {
            var objCommand = GetSqlCommand("pNLS_IngresoResultadoEvaluacionMaterial");
            //InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oConfiguracionEvalControlCalidad.idConfigEvaluacion);
            InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluado", objIngresoResultadosPEEC.idEESSEvaluado);
            //InputParameterAdd.Int(objCommand, "idEnfermedad", oConfiguracionEvalControlCalidad.Enfermedad.idEnfermedad);
            //InputParameterAdd.Varchar(objCommand, "nombre", oConfiguracionEvalControlCalidad.Nombre);
            //InputParameterAdd.Varchar(objCommand, "descripcion", oConfiguracionEvalControlCalidad.Descripcion);
            //InputParameterAdd.Int(objCommand, "estado", oConfiguracionEvalControlCalidad.estado);


            return Execute(objCommand).AsEnumerable().Select(row => new IngresoResultadosPEEC()
            {
                idConfigEvaluacion = Converter.GetGuid(row, "idConfigEvaluacion"),
                idConfiguracionPanel = Converter.GetGuid(row, "idConfiguracionPanel"),
                EESSEvaluador = Converter.GetString(row, "EESSEvaluador"),
                DescEvaluacion = Converter.GetString(row, "NombreEvaluacion"),
                nroLote = Converter.GetString(row, "nroLote"),
                nombre = Converter.GetString(row, "NombrePanel"),
                DescTipo = new Common().Tipo(Converter.GetInt(row, "idTipoPanel"), "TipoMetodo"),
                idTipo = Converter.GetInt(row, "idTipoPanel"),
                idEESSEvaluador = Converter.GetInt(row, "idEstablecimientoEvaluador")
            }).ToList();
        }
        /// <summary>
        /// Descripción: Metodo para Obtener los resultados registrados de una evaluacion
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="objResultadoControlCalidadVd"></param>
        /// <returns></returns>
        public List<ResultadoControlCalidadVd> GetResultadoControlCalidadVd(ResultadoControlCalidadVd objResultadoControlCalidadVd)
        {
            var objCommand = GetSqlCommand("pNLS_IngresoResultadoEvaluacionMaterial");
            InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", objResultadoControlCalidadVd.idConfigEvaluacion);
            InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", objResultadoControlCalidadVd.idConfiguracionPanel);
            InputParameterAdd.Guid(objCommand, "idConfigEvaluacionMaterial", objResultadoControlCalidadVd.idConfigEvaluacionMaterial);

            return Execute(objCommand).AsEnumerable().Select(row => new ResultadoControlCalidadVd()
            {
                idConfigEvaluacion = Converter.GetGuid(row, "idConfigEvaluacion"),
                idConfiguracionPanel = Converter.GetGuid(row, "idConfiguracionPanel"),
                EESSEvaluador = Converter.GetString(row, "EESSEvaluador")
               
            }).ToList();
        }
        /// <summary>
        /// Descripción: Metodo para registrar resultados de cada material
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oResultadoControlCalidadVd"></param>
        /// <returns></returns>
        public int InsertResultadoControlCalidadVd(ResultadoControlCalidadVd oResultadoControlCalidadVd)
        {
            var objCommand = GetSqlCommand("pNLI_ResultadoEvaluacionMaterial");
            try
            {
                InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oResultadoControlCalidadVd.idConfigEvaluacion);
                InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", oResultadoControlCalidadVd.idConfiguracionPanel);
                InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluador", oResultadoControlCalidadVd.idEstablecimientoEvaluador);
                InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluado", oResultadoControlCalidadVd.idEstablecimientoEvaluado);
                InputParameterAdd.Int(objCommand, "idTipo", oResultadoControlCalidadVd.idTipoPanel);
                InputParameterAdd.Int(objCommand, "idTipoMetodo", oResultadoControlCalidadVd.idTipoMetodo);
                InputParameterAdd.Varchar(objCommand, "nroPregunta", oResultadoControlCalidadVd.NroPregunta);
                InputParameterAdd.Varchar(objCommand, "ValorRespuesta", oResultadoControlCalidadVd.ValorRespuesta);
                InputParameterAdd.Varchar(objCommand, "Respuesta", oResultadoControlCalidadVd.Respuesta);
                InputParameterAdd.Int(objCommand, "idusuarioregistro", oResultadoControlCalidadVd.idusuarioregistro);
                ExecuteNonQuery(objCommand);
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }

            return 1;
        }
        /// <summary>
        /// Descripción: Metodo para actualizar resultados de cada material
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018
        /// </summary>
        /// <param name="oResultadoControlCalidadVd"></param>
        /// <returns></returns>
        public int EditResultadoControlCalidadVd(ResultadoControlCalidadVd oResultadoControlCalidadVd)
        {
            var objCommand = GetSqlCommand("pNLU_ResultadoEvaluacionMaterial");
            try
            {
                InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oResultadoControlCalidadVd.idConfigEvaluacion);
                InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", oResultadoControlCalidadVd.idConfiguracionPanel);
                InputParameterAdd.Int(objCommand, "idTipo", oResultadoControlCalidadVd.idTipoPanel);
                InputParameterAdd.Int(objCommand, "idTipoMetodo", oResultadoControlCalidadVd.idTipoMetodo);
                InputParameterAdd.Varchar(objCommand, "nroPregunta", oResultadoControlCalidadVd.NroPregunta);
                InputParameterAdd.Varchar(objCommand, "Respuesta", oResultadoControlCalidadVd.Respuesta);
                InputParameterAdd.Varchar(objCommand, "ValorRespuesta", oResultadoControlCalidadVd.ValorRespuesta);
                InputParameterAdd.Int(objCommand, "idusuarioedicion", oResultadoControlCalidadVd.idusuarioedicion);
                ExecuteNonQuery(objCommand);
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }

            return 1;
        }
        public int ExistResultadoControlCalidadVd(ResultadoControlCalidadVd oResultadoControlCalidadVd)
        {
            var objCommand = GetSqlCommand("pNLE_ResultadoEvaluacionMaterial");
            var res = 0;
            try
            {
                InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oResultadoControlCalidadVd.idConfigEvaluacion);
                InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", oResultadoControlCalidadVd.idConfiguracionPanel);
                InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluador", oResultadoControlCalidadVd.idEstablecimientoEvaluador);
                InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluado", oResultadoControlCalidadVd.idEstablecimientoEvaluado);
                InputParameterAdd.Int(objCommand, "idTipo", oResultadoControlCalidadVd.idTipoPanel);
                InputParameterAdd.Int(objCommand, "idTipoMetodo", oResultadoControlCalidadVd.idTipoMetodo);
                InputParameterAdd.Varchar(objCommand, "nroPregunta", oResultadoControlCalidadVd.NroPregunta);
                OutputParameterAdd.Int(objCommand, "exists");
                ExecuteNonQuery(objCommand);
                res = (int)objCommand.Parameters["@exists"].Value;
            }   
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }

            return res;
        }
        public List<ResultadoControlCalidadVd> GetResultadoEvaluacionMaterial (ResultadoControlCalidadVd objResultadoControlCalidadVd)
        {
            var objCommand = GetSqlCommand("pNLS_ResultadoEvaluacionMaterial");
            InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", objResultadoControlCalidadVd.idConfigEvaluacion);
            InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", objResultadoControlCalidadVd.idConfiguracionPanel);
            InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluador", objResultadoControlCalidadVd.idEstablecimientoEvaluador);
            InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluado", objResultadoControlCalidadVd.idEstablecimientoEvaluado);

            return Execute(objCommand).AsEnumerable().Select(row => new ResultadoControlCalidadVd()
            {
                idConfigEvaluacion = Converter.GetGuid(row, "idConfigEvaluacion"),
                idConfiguracionPanel = Converter.GetGuid(row, "idConfiguracionPanel"),
                idTipoMetodo = Converter.GetInt(row, "idTipoMetodo"),
                idTipoPanel = Converter.GetInt(row, "idTipo"),
                NroPregunta= Converter.GetString(row, "nroPregunta"),
                ValorRespuesta = Converter.GetString(row, "ValorRespuesta"),
                Respuesta = Converter.GetString(row, "Respuesta"),
            }).ToList();
        }
    }
}

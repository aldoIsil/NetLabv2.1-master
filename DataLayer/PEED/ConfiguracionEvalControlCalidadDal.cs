using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.DalConverter;
using Model;
using System.Linq;

namespace DataLayer.PEED
{
    public class ConfiguracionEvalControlCalidadDal : DaoBase
    {
        public ConfiguracionEvalControlCalidadDal(IDalSettings settings) : base(settings)
        {
        }

        public ConfiguracionEvalControlCalidadDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene las evaluaciones registradas
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionEvalControlCalidad"></param>
        /// <returns></returns>
        public List<ConfiguracionEvalControlCalidad> GetConfiguracionEvalControlCalidad(ConfiguracionEvalControlCalidad oConfiguracionEvalControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLS_ConfiguracionEvaluacionControlCalidad");
            //InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oConfiguracionEvalControlCalidad.idConfigEvaluacion);
            //InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluador", oConfiguracionEvalControlCalidad.EstablecimientoEvaluador.IdEstablecimiento);
            //InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluado", oConfiguracionEvalControlCalidad.EstablecimientoEvaluado.IdEstablecimiento);
            //InputParameterAdd.Int(objCommand, "idEnfermedad", oConfiguracionEvalControlCalidad.Enfermedad.idEnfermedad);
            //InputParameterAdd.Varchar(objCommand, "nombre", oConfiguracionEvalControlCalidad.Nombre);
            //InputParameterAdd.Varchar(objCommand, "descripcion", oConfiguracionEvalControlCalidad.Descripcion);
            //InputParameterAdd.Int(objCommand, "estado", oConfiguracionEvalControlCalidad.estado);


            return Execute(objCommand).AsEnumerable().Select(row => new ConfiguracionEvalControlCalidad()
            {
                idConfigEvaluacion = Converter.GetGuid(row, "idConfigEvaluacion"),
                Nombre = Converter.GetString(row, "NombreEvaluacion"),
                EstablecimientoEvaluado = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimientoEvaluado"),
                    Nombre = Converter.GetString(row, "EESSEvaluado"),
                },
                EstablecimientoEvaluador = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimientoEvaluador"),
                    Nombre = Converter.GetString(row, "EESSEvaluador"),
                },
                Enfermedad = new Enfermedad
                {
                    idEnfermedad = Converter.GetInt(row, "idEnfermedad"),
                    nombre = Converter.GetString(row, "NombreEnfermedad")
                },
                DescEstado = Converter.GetString(row, "EstadoDescripcion"),
                idusuarioregistro = Converter.GetInt(row, "idusuarioregistro"),
                fecharegistro = Converter.GetString(row, "fecharegistro"),
                idusuarioedicion = Converter.GetInt(row, "idusuarioedicion"),
                fechaedicion = Converter.GetString(row, "fechaedicion"),
                UsuarioRegistro = Converter.GetString(row, "NombreUsuarioRegistro"),
                UsuarioEdicion = Converter.GetString(row, "NombreUsuarioEdicion"),
            }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene evaluacion por Id
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionEvalControlCalidad"></param>
        /// <returns></returns>
        public ConfiguracionEvalControlCalidad GetConfiguracionEvalControlCalidadById(ConfiguracionEvalControlCalidad oConfiguracionEvalControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLS_ConfiguracionEvaluacionControlCalidadById");
            InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oConfiguracionEvalControlCalidad.idConfigEvaluacion);


            return Execute(objCommand).AsEnumerable().Select(row => new ConfiguracionEvalControlCalidad()
            {
                idConfigEvaluacion = Converter.GetGuid(row, "idConfigEvaluacion"),
                Nombre = Converter.GetString(row, "NombreEvaluacion"),
                Descripcion = Converter.GetString(row, "Descripcion"),
                EstablecimientoEvaluado = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimientoEvaluado"),
                    Nombre = Converter.GetString(row, "NombreEvaluado")
                },
                EstablecimientoEvaluador = new Establecimiento
                {
                    IdEstablecimiento = Converter.GetInt(row, "idEstablecimientoEvaluador"),
                    Nombre = Converter.GetString(row, "NombreEvaluador")
                },
                Enfermedad = new Enfermedad
                {
                    idEnfermedad = Converter.GetInt(row, "idEnfermedad"),
                    nombre = Converter.GetString(row, "NombreEnfermedad")
                },
                idusuarioregistro = Converter.GetInt(row, "idusuarioregistro"),
                fecharegistro = Converter.GetString(row, "fecharegistro"),
                idusuarioedicion = Converter.GetInt(row, "idusuarioedicion"),
                fechaedicion = Converter.GetString(row, "fechaedicion"),
            }).ToList().FirstOrDefault();
        }
        /// <summary>
        /// Descripción: Realiza el registro de las evaluaciones
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionEvalControlCalidad"></param>
        /// <returns></returns>
        public int InsertConfigEval(ConfiguracionEvalControlCalidad oConfiguracionEvalControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLI_ConfiguracionEvaluacionControlCalidad");
            try
            {
                InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oConfiguracionEvalControlCalidad.idConfigEvaluacion);
                InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluador", oConfiguracionEvalControlCalidad.EstablecimientoEvaluador.IdEstablecimiento);
                InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluado", oConfiguracionEvalControlCalidad.EstablecimientoEvaluado.IdEstablecimiento);
                InputParameterAdd.Int(objCommand, "idenfermedad", oConfiguracionEvalControlCalidad.Enfermedad.idEnfermedad);
                InputParameterAdd.Varchar(objCommand, "nombre", oConfiguracionEvalControlCalidad.Nombre);
                InputParameterAdd.Varchar(objCommand, "descripcion", oConfiguracionEvalControlCalidad.Descripcion);
                InputParameterAdd.Int(objCommand, "idusuarioregistro", oConfiguracionEvalControlCalidad.idusuarioregistro);
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
        /// Descripción: Realiza la actualizacion de las evaluaciones
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionEvalControlCalidad"></param>
        /// <returns></returns>
        public int EditConfigEval(ConfiguracionEvalControlCalidad oConfiguracionEvalControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLU_ConfiguracionEvaluacionControlCalidad");
            try
            {
                InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oConfiguracionEvalControlCalidad.idConfigEvaluacion);
                InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluador", oConfiguracionEvalControlCalidad.EstablecimientoEvaluador.IdEstablecimiento);
                InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluado", oConfiguracionEvalControlCalidad.EstablecimientoEvaluado.IdEstablecimiento);
                InputParameterAdd.Int(objCommand, "idenfermedad", oConfiguracionEvalControlCalidad.Enfermedad.idEnfermedad);
                InputParameterAdd.Varchar(objCommand, "nombre", oConfiguracionEvalControlCalidad.Nombre);
                InputParameterAdd.Varchar(objCommand, "descripcion", oConfiguracionEvalControlCalidad.Descripcion);
                InputParameterAdd.Int(objCommand, "estado", oConfiguracionEvalControlCalidad.estado);
                InputParameterAdd.Int(objCommand, "idusuarioedicion", oConfiguracionEvalControlCalidad.idusuarioedicion);
                ExecuteNonQuery(objCommand);
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }

            return 1;
        }
    }
}

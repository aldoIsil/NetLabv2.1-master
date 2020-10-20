using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using DataLayer.Compartido;
using Model;
using System.Linq;

namespace DataLayer.PEED
{
    public class ConfiguracionMaterialControlCalidadDal : DaoBase
    {
        public ConfiguracionMaterialControlCalidadDal(IDalSettings settings) : base(settings)
        {
        }

        public ConfiguracionMaterialControlCalidadDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene los materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionMaterialControlCalidad"></param>
        /// <returns></returns>
        public List<ConfiguracionMaterialControlCalidad> GetConfiguracionMaterialControlCalidad(ConfiguracionMaterialControlCalidad oConfiguracionMaterialControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLS_ConfiguracionEvaluacionMaterial");
            InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", oConfiguracionMaterialControlCalidad.idConfiguracionPanel);
            InputParameterAdd.Guid(objCommand, "idConfigEvaluacionMaterial", oConfiguracionMaterialControlCalidad.idConfigEvaluacionMaterial);


            return Execute(objCommand).AsEnumerable().Select(row => new ConfiguracionMaterialControlCalidad()
            {
                idConfigEvaluacionMaterial = Converter.GetGuid(row, "idConfigEvaluacionMaterial"),
                idConfiguracionPanel = Converter.GetGuid(row, "idConfiguracionPanel"),
                nroPregunta = Converter.GetString(row, "nroPregunta"),
                Respuesta = new Common().Tipo(Converter.GetString(row, "idTipoMetodo") != "8"? int.Parse(Converter.GetString(row, "Respuesta")): 12, "Respuesta"),
                puntaje = Converter.GetDecimal(row, "puntaje"),
                ValorRespuesta = Converter.GetString(row, "ValorRespuesta"),
                idTipoMetodo = Converter.GetInt(row, "idTipoMetodo"),
                idusuarioregistro = Converter.GetInt(row, "idusuarioregistro"),
                fecharegistro = Converter.GetString(row, "fecharegistro"),
                idusuarioedicion = Converter.GetInt(row, "idusuarioedicion"),
                fechaedicion = Converter.GetString(row, "fechaedidcion"),
                UsuarioRegistro = Converter.GetString(row, "NombreUsuarioRegistro"),
                UsuarioEdicion = Converter.GetString(row, "NombreUsuarioEdicion"),
                DescTipoMetodo = new Common().Tipo(Converter.GetInt(row, "idTipoMetodo"), "TipoMetodoMaterial"),
                DescSubTipoMetodo = new Common().Tipo(Converter.GetInt(row, "idSubTipoMetodo"), "SubTipoMetodoMaterial")
            }).ToList();
        }
        /// <summary>
        /// Descripción: Realiza el registro de los materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionMaterialControlCalidad"></param>
        /// <returns></returns>
        public int InsertConfigEvalMaterial(ConfiguracionMaterialControlCalidad oConfiguracionMaterialControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLI_ConfiguracionEvaluacionMaterial");
            try
            {
                InputParameterAdd.Guid(objCommand, "idConfigEvaluacionMaterial", Guid.NewGuid());
                InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", oConfiguracionMaterialControlCalidad.idConfiguracionPanel);
                InputParameterAdd.Int(objCommand, "idTipo", oConfiguracionMaterialControlCalidad.idTipoMetodo);
                InputParameterAdd.Int(objCommand, "idSubTipo", oConfiguracionMaterialControlCalidad.idSubTipoMetodo);
                InputParameterAdd.Varchar(objCommand, "nroPregunta", oConfiguracionMaterialControlCalidad.nroPregunta);
                InputParameterAdd.Varchar(objCommand, "ValorRespuesta", oConfiguracionMaterialControlCalidad.ValorRespuesta);
                InputParameterAdd.Int(objCommand, "TipoDato", oConfiguracionMaterialControlCalidad.tipoDato);
                InputParameterAdd.Varchar(objCommand, "Respuesta", oConfiguracionMaterialControlCalidad.Respuesta);
                InputParameterAdd.Decimal(objCommand, "puntaje", oConfiguracionMaterialControlCalidad.puntaje);
                InputParameterAdd.Int(objCommand, "idusuarioregistro", oConfiguracionMaterialControlCalidad.idusuarioregistro);
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
        /// Descripción: Realiza las modificaciones de los materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionMaterialControlCalidad"></param>
        /// <returns></returns>
        public int EditConfigEvalMaterial(ConfiguracionMaterialControlCalidad oConfiguracionMaterialControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLU_ConfiguracionEvaluacionMaterial");
            try
            {
                InputParameterAdd.Guid(objCommand, "idConfigEvaluacionMaterial", oConfiguracionMaterialControlCalidad.idConfigEvaluacionMaterial);
                InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", oConfiguracionMaterialControlCalidad.idConfiguracionPanel);
                InputParameterAdd.Int(objCommand, "idTipoMetodo", oConfiguracionMaterialControlCalidad.idTipoMetodo);
                InputParameterAdd.Varchar(objCommand, "nroPregunta", oConfiguracionMaterialControlCalidad.nroPregunta);
                InputParameterAdd.Varchar(objCommand, "Respuesta", oConfiguracionMaterialControlCalidad.Respuesta);
                InputParameterAdd.Varchar(objCommand, "ValorRespuesta", oConfiguracionMaterialControlCalidad.ValorRespuesta);
                InputParameterAdd.Decimal(objCommand, "puntaje", oConfiguracionMaterialControlCalidad.puntaje);
                InputParameterAdd.Int(objCommand, "idusuarioedicion", oConfiguracionMaterialControlCalidad.idusuarioedicion);
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

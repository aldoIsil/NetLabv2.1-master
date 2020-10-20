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
    public class ConfiguracionPanelControlCalidadDal : DaoBase
    {
        public ConfiguracionPanelControlCalidadDal(IDalSettings settings) : base(settings)
        {
        }

        public ConfiguracionPanelControlCalidadDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene los paneles
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public List<ConfiguracionPanelControlCalidad> GetConfiguracionPanelControlCalidad(ConfiguracionPanelControlCalidad oConfiguracionPanelControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLS_ConfiguracionEvaluacionPanelControlCalidad");
            InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", oConfiguracionPanelControlCalidad.idConfiguracionPanel);
            InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oConfiguracionPanelControlCalidad.idConfigEvaluacion);
            //InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluador", oConfiguracionEvalControlCalidad.EstablecimientoEvaluador.IdEstablecimiento);
            //InputParameterAdd.Int(objCommand, "idEstablecimientoEvaluado", oConfiguracionEvalControlCalidad.EstablecimientoEvaluado.IdEstablecimiento);
            //InputParameterAdd.Int(objCommand, "idEnfermedad", oConfiguracionEvalControlCalidad.Enfermedad.idEnfermedad);
            //InputParameterAdd.Varchar(objCommand, "nombre", oConfiguracionEvalControlCalidad.Nombre);
            //InputParameterAdd.Varchar(objCommand, "descripcion", oConfiguracionEvalControlCalidad.Descripcion);
            //InputParameterAdd.Int(objCommand, "estado", oConfiguracionEvalControlCalidad.estado);


            return Execute(objCommand).AsEnumerable().Select(row => new ConfiguracionPanelControlCalidad()
            {
                idConfigEvaluacion = Converter.GetGuid(row, "idConfigEvaluacion"),
                idConfiguracionPanel = Converter.GetGuid(row, "idConfiguracionPanel"),
                nroLote = Converter.GetString(row, "nrolote"),
                nombre = Converter.GetString(row, "nombre"),
                descripcion = Converter.GetString(row, "descripcion"),
                idTipo = Converter.GetInt(row, "idTipo"),
                idTipoEvaluacion = Converter.GetInt(row, "idTipoEvaluacion"),
                idSubTipo = Converter.GetInt(row, "idSubTipo"),
                estado = Converter.GetInt(row, "estado"),
                idusuarioregistro = Converter.GetInt(row, "idusuarioregistro"),
                fecharegistro = Converter.GetString(row, "fecharegistro"),
                idusuarioedicion = Converter.GetInt(row, "idusuarioedicion"),
                fechaedicion = Converter.GetString(row, "fechaedicion"),
                UsuarioRegistro = Converter.GetString(row, "NombreUsuarioRegistro"),
                UsuarioEdicion = Converter.GetString(row, "NombreUsuarioEdicion"),
                DescTipo = new Common().Tipo(Converter.GetInt(row, "idTipo"),"TipoMetodo"),
                DescTipoEvaluacion = new Common().Tipo(Converter.GetInt(row, "idTipoEvaluacion"), "TipoEvaluacion")
            }).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene los Paneles por codigo
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public ConfiguracionPanelControlCalidad GetConfiguracionPanelControlCalidadById(ConfiguracionPanelControlCalidad oConfiguracionPanelControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLS_ConfiguracionEvaluacionControlCalidadById");
            InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oConfiguracionPanelControlCalidad.idConfiguracionPanel);


            return Execute(objCommand).AsEnumerable().Select(row => new ConfiguracionPanelControlCalidad()
            {
                idConfigEvaluacion = Converter.GetGuid(row, "idConfigEvaluacion"),
                nombre = Converter.GetString(row, "NombreEvaluacion"),
                descripcion = Converter.GetString(row, "Descripcion"),
                
                idusuarioregistro = Converter.GetInt(row, "idusuarioregistro"),
                fecharegistro = Converter.GetString(row, "fecharegistro"),
                idusuarioedicion = Converter.GetInt(row, "idusuarioedicion"),
                fechaedicion = Converter.GetString(row, "fechaedicion"),
            }).ToList().FirstOrDefault();
        }
        /// <summary>
        /// Descripción: Registrar los paneles
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public int InsertConfigEvalPanel(ConfiguracionPanelControlCalidad oConfiguracionPanelControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLI_ConfiguracionEvaluacionPanelControlCalidad");
            try
            {
                InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oConfiguracionPanelControlCalidad.idConfigEvaluacion);
                InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", Guid.NewGuid());
                InputParameterAdd.Int(objCommand, "idTipoEvaluacion", oConfiguracionPanelControlCalidad.idTipoEvaluacion);
                InputParameterAdd.Varchar(objCommand, "nroLote", oConfiguracionPanelControlCalidad.nroLote);
                InputParameterAdd.Varchar(objCommand, "nombre", oConfiguracionPanelControlCalidad.nombre);
                InputParameterAdd.Varchar(objCommand, "descripcion", oConfiguracionPanelControlCalidad.descripcion);
                InputParameterAdd.Int(objCommand, "idTipo", oConfiguracionPanelControlCalidad.idTipo);
                InputParameterAdd.Int(objCommand, "idSubTipo", oConfiguracionPanelControlCalidad.idSubTipo);
                InputParameterAdd.Int(objCommand, "idusuarioregistro", oConfiguracionPanelControlCalidad.idusuarioregistro);
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
        /// Descripción: Metodo para la modificacion de paneles
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public int EditConfigEvalPanel(ConfiguracionPanelControlCalidad oConfiguracionPanelControlCalidad)
        {
            var objCommand = GetSqlCommand("pNLU_ConfiguracionEvaluacionPanelControlCalidad");
            try
            {
                InputParameterAdd.Guid(objCommand, "idConfigEvaluacion", oConfiguracionPanelControlCalidad.idConfigEvaluacion);
                InputParameterAdd.Guid(objCommand, "idConfiguracionPanel", oConfiguracionPanelControlCalidad.idConfiguracionPanel);
                InputParameterAdd.Int(objCommand, "idTipoEvaluacion", oConfiguracionPanelControlCalidad.idTipoEvaluacion);
                InputParameterAdd.Varchar(objCommand, "nroLote", oConfiguracionPanelControlCalidad.nroLote);
                InputParameterAdd.Varchar(objCommand, "nombre", oConfiguracionPanelControlCalidad.nombre);
                InputParameterAdd.Varchar(objCommand, "descripcion", oConfiguracionPanelControlCalidad.descripcion);
                InputParameterAdd.Int(objCommand, "idTipo", oConfiguracionPanelControlCalidad.idTipo);
                InputParameterAdd.Int(objCommand, "idSubTipo", oConfiguracionPanelControlCalidad.idSubTipo);
                InputParameterAdd.Int(objCommand, "estado", 1);
                InputParameterAdd.Int(objCommand, "idusuarioedicion", oConfiguracionPanelControlCalidad.idusuarioedicion);
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

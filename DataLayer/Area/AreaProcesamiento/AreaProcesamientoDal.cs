using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;

namespace DataLayer.Area.AreaProcesamiento
{
    public class AreaProcesamientoDal : DaoBase
    {
        public AreaProcesamientoDal(IDalSettings settings) : base(settings)
        {
        }

        public AreaProcesamientoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene las areas de procesamiento filtrada por el nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Model.AreaProcesamiento> GetAreasProcesamiento(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_AreaProcesamiento");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return AreaProcesamientoConvertTo.AreasProcesamiento(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene las areas de procesamiento filtrada por el id del Area de procesamiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.AreaProcesamiento GetAreaProcesamientoById(int id)
        {
            var objCommand = GetSqlCommand("pNLS_AreaProcesamientoById");

            InputParameterAdd.Int(objCommand, "idAreaProcesamiento", id);

            return AreaProcesamientoConvertTo.AreaProcesamiento(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Realiza el registro de un area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="areaProcesamiento"></param>
        public void InsertAreaProcesamiento(Model.AreaProcesamiento areaProcesamiento)
        {
            var objCommand = GetSqlCommand("pNLI_AreaProcesamiento");

            InputParameterAdd.Varchar(objCommand, "nombre", areaProcesamiento.Nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", areaProcesamiento.Descripcion);
            InputParameterAdd.Varchar(objCommand, "sigla", areaProcesamiento.Sigla);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", areaProcesamiento.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Realiza la actualización de un area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="areaProcesamiento"></param>
        public void UpdateAreaProcesamiento(Model.AreaProcesamiento areaProcesamiento)
        {
            var objCommand = GetSqlCommand("pNLU_AreaProcesamiento");

            InputParameterAdd.Int(objCommand, "idAreaProcesamiento", areaProcesamiento.IdAreaProcesamiento);
            InputParameterAdd.Varchar(objCommand, "nombre", areaProcesamiento.Nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", areaProcesamiento.Descripcion);
            InputParameterAdd.Varchar(objCommand, "sigla", areaProcesamiento.Sigla);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", areaProcesamiento.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", areaProcesamiento.Estado);

            ExecuteNonQuery(objCommand);
        }
    }
}
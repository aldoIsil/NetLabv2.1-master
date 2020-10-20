using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;

namespace DataLayer
{
    public class ReactivoDal : DaoBase
    {
        public ReactivoDal(IDalSettings settings) : base(settings)
        {
        }

        public ReactivoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Insertar un tipo de reactivo 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="reactivo"></param>
        public void InsertReactivo(Reactivo reactivo)
        {
            var objCommand = GetSqlCommand("pNLI_Reactivo");

            InputParameterAdd.Varchar(objCommand, "glosa", reactivo.glosa);
            InputParameterAdd.Int(objCommand, "estado", reactivo.Estado);
            
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Insertar/Actualizar un tipo de reactivo a una presentacion 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="pr"></param>
        /// <param name="idPresentacion"></param>
        /// <param name="idReactivo"></param>
        public void InsertReactivoPresentacion(PresentacionReactivo pr, int idPresentacion, int idReactivo)
        {
            var objCommand = GetSqlCommand("pNLI_ReactivoPresentacion");
            InputParameterAdd.Int(objCommand, "idPresentacion", pr.idPresentacion);
            InputParameterAdd.Int(objCommand, "idReactivo", pr.idReactivo);
            InputParameterAdd.Int(objCommand, "estado", pr.Estado);
            //InputParameterAdd.DateTime(objCommand, "fechaRegistro", pm.FechaRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualizar un tipo de reactivo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="reactivo"></param>
        public void UpdateReactivo(Reactivo reactivo)
        {
            var objCommand = GetSqlCommand("pNLU_Reactivo");

            InputParameterAdd.Int(objCommand, "idReactivo", reactivo.idReactivo);
            InputParameterAdd.Varchar(objCommand, "glosa", reactivo.glosa);
            InputParameterAdd.Int(objCommand, "estado", reactivo.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", reactivo.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los reactivos filtrados por la Glosa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="glosa"></param>
        /// <returns></returns>
        public List<Reactivo> GetReactivos(string glosa)
        {
            var objCommand = GetSqlCommand("pNLS_Reactivo");
            InputParameterAdd.Varchar(objCommand, "glosa", glosa);

            return ReactivoConvertTo.Reactivos(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los reactivos filtrados por el codigo del reactivo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idReactivo"></param>
        /// <returns></returns>
        public Reactivo GetReactivoById(int idReactivo)
        {
            var objCommand = GetSqlCommand("pNLS_ReactivoById");
            InputParameterAdd.Int(objCommand, "idReactivo", idReactivo);

            return ReactivoConvertTo.Reacctivo(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene una presentacion/reactivo activa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios. 
        /// </summary>
        /// <param name="idPresentacion"></param> 
        /// <returns></returns>
        public List<Reactivo> GetReactivosByIdPresentacion(int? idPresentacion)
        {
            var objCommand = GetSqlCommand("pNLS_ReactivosByIdPresentacion");
            InputParameterAdd.Int(objCommand, "idPresentacion", idPresentacion);

            return ReactivoConvertTo.Reactivos(Execute(objCommand));
        }

    }
}
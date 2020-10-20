using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;


namespace DataLayer
{
    public class PresentacionReactivoDal: DaoBase
    {
        
        public PresentacionReactivoDal(IDalSettings settings) : base(settings)
        {

        }

        public PresentacionReactivoDal() : this(new NetlabSettings())
        {

        }
        /// <summary>
        /// Descripción: Obtiene una presentacion por el codigo de reactivo activa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idReactivo"></param>
        /// <returns></returns>
        public List<Presentacion> GetPresentacionesByReactivoId(int idReactivo)
        {
            var objCommand = GetSqlCommand("pNLS_PresentacionesByReactivoId");
            InputParameterAdd.Int(objCommand, "idReactivo", idReactivo);

            return PresentacionConvertTo.Presentaciones(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene una presentacion activa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Presentacion> GetPresentacionesActivas(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_PresentacionActivos");
            InputParameterAdd.Varchar(objCommand, "glosa", nombre);

            return PresentacionConvertTo.Presentaciones(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Insertar/Actualizar un tipo de reactivo a una presentacion 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentacionReactivo"></param>
        public void InsertPresentacionByReactivo(PresentacionReactivo presentacionReactivo)
        {
            var objCommand = GetSqlCommand("pNLI_ReactivoPresentacion");

            InputParameterAdd.Int(objCommand, "idReactivo", presentacionReactivo.idReactivo);
            InputParameterAdd.Int(objCommand, "idPresentacion", presentacionReactivo.idPresentacion);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", presentacionReactivo.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza una presenatacion/reactivo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentacionReactivo"></param>
        public void UpdatePresentacionByReactivo(PresentacionReactivo presentacionReactivo)
        {
            var objCommand = GetSqlCommand("pNLU_ReactivoPresentacion");

            InputParameterAdd.Int(objCommand, "idReactivo", presentacionReactivo.idReactivo);
            InputParameterAdd.Int(objCommand, "idPresentacion", presentacionReactivo.idPresentacion);
            InputParameterAdd.Int(objCommand, "estado", presentacionReactivo.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", presentacionReactivo.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
        

    }
}

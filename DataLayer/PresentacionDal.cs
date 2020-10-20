using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;

namespace DataLayer
{
    public class PresentacionDal : DaoBase
    {
        public PresentacionDal(IDalSettings settings) : base(settings)
        {
        }

        public PresentacionDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción:insertar una nueva Presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presenta"></param>
        public void InsertPresentacion(Presentacion presenta)
        {
            var objCommand = GetSqlCommand("pNLI_Presentacion");

            InputParameterAdd.Varchar(objCommand, "glosa", presenta.glosa);
            InputParameterAdd.Int(objCommand, "estado", presenta.Estado);
            //InputParameterAdd.DateTime(objCommand, "fechaRegistro", presenta.FechaRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Registra o Actualiza la Presentacion de la Muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="pm"></param>
        /// <param name="idPresentacion"></param>
        /// <param name="idMuestra"></param>
        public void InsertPresentacionMuestra(PresentacionMuestra pm,int idPresentacion, int idMuestra)
        {
            var objCommand = GetSqlCommand("pNLI_PresentacionTipoMuestra");
            InputParameterAdd.Int(objCommand, "idPresentacion", pm.idPresentacion);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", pm.idTipoMuestra);
            InputParameterAdd.Int(objCommand, "estado", pm.Estado);
            //InputParameterAdd.DateTime(objCommand, "fechaRegistro", pm.FechaRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza una presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presenta"></param>
        public void UpdatePresentacion(Presentacion presenta)
        {
            var objCommand = GetSqlCommand("pNLU_Presentacion");

            InputParameterAdd.Int(objCommand, "idPresentacion", presenta.idPresentacion);
            InputParameterAdd.Varchar(objCommand, "glosa", presenta.glosa);
            InputParameterAdd.Int(objCommand, "estado",presenta.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", presenta.IdUsuarioEdicion);
                                                
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene una presentacion filtrada por la Glosa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="glosa"></param>
        /// <returns></returns>
        public List<Presentacion> GetPresentaciones(string glosa)
        {
            var objCommand = GetSqlCommand("pNLS_Presentacion");
            InputParameterAdd.Varchar(objCommand, "glosa", glosa);

            return PresentacionConvertTo.Presentaciones(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene una presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPresentacion"></param>
        /// <returns></returns>
        public Presentacion GetPresentacionById(int idPresentacion)
        {
            var objCommand = GetSqlCommand("pNLS_PresentacionById");
            InputParameterAdd.Int(objCommand, "idPresentacion", idPresentacion);

            return PresentacionConvertTo.Presentacion(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene las presentaciones por tipo de muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public List<Presentacion> GetPresentacionesByIdTipoMuestra(int? idTipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLS_PresentacionByIdTipoMuestra");
            InputParameterAdd.Int(objCommand, "idTipoMuestra", idTipoMuestra);

            return PresentacionConvertTo.Presentaciones(Execute(objCommand)); 
        }
    }
}


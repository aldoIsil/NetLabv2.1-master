using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;


namespace DataLayer
{
    public class PresentacionTipoMuestraDal : DaoBase
    {

        public PresentacionTipoMuestraDal(IDalSettings settings) : base(settings)
        {

        }

        public PresentacionTipoMuestraDal() : this(new NetlabSettings())
        {

        }
        /// <summary>
        /// Descripción: Obtiene la presentacion de muestras ACTIVOS por codigo de presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPresentacion"></param>
        /// <returns></returns>
        public List<TipoMuestra> GetTipoMuestrasByPresentacionId(int idPresentacion)
        {
            var objCommand = GetSqlCommand("pNLS_TipoMuestrasByPresentacionId");
            InputParameterAdd.Int(objCommand, "idPresentacion", idPresentacion);

            return TipoMuestraConvertTo.TipoMuestras(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registra o Actualiza la Presentacion de la Muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipomuestra"></param>
        public void InsertTipoMuestraByPresentacion(PresentacionTipoMuestra tipomuestra)
        {
            var objCommand = GetSqlCommand("pNLI_PresentacionTipoMuestra");

            InputParameterAdd.Int(objCommand, "idPresentacion", tipomuestra.IdPresentacion);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", tipomuestra.IdTipoMuestra );
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", tipomuestra.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza un tipo de muestra de una presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentaTipoMuestra"></param>
        public void UpdateTipoMuestraByPresentacion (PresentacionTipoMuestra presentaTipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLU_PresentacionMuestra");

            InputParameterAdd.Int(objCommand, "idPresentacion", presentaTipoMuestra.IdPresentacion);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", presentaTipoMuestra.IdTipoMuestra);
            InputParameterAdd.Int(objCommand, "estado", presentaTipoMuestra.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", presentaTipoMuestra.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }        
    }
}

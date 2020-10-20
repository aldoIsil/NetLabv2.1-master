using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer
{
    public class TipoMuestraCriterioRechazoDal : DaoBase
    {
        public TipoMuestraCriterioRechazoDal(IDalSettings settings) : base(settings)
        {
        }

        public TipoMuestraCriterioRechazoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo por tipo de muestra 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public List<CriterioRechazo> GetCriteriosByTipoMuestraId(int idTipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLS_CriteriosByTipoMuestraId");
            InputParameterAdd.Int(objCommand, "idTipoMuestra", idTipoMuestra);

            return CriterioRechazoConvertTo.Criterios(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Registra Tipos de Criterios de Rechazo verificando que no exista.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="criterioRechazo"></param>
        public void InsertCriterioByTipoMuestra(TipoMuestraCriterioRechazo criterioRechazo)
        {
            var objCommand = GetSqlCommand("pNLI_TipoMuestraCriterioRechazo");

            InputParameterAdd.Int(objCommand, "idTipoMuestra", criterioRechazo.IdTipoMuestra);
            InputParameterAdd.Int(objCommand, "idCriterioRechazo", criterioRechazo.IdCriterioRechazo);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", criterioRechazo.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza Tipo de Criterios de Rechazo verificando que no exista.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="muestraCriterioRechazo"></param>
        public void UpdateCriterioByTipoMuestra(TipoMuestraCriterioRechazo muestraCriterioRechazo)
        {
            var objCommand = GetSqlCommand("pNLU_TipoMuestraCriterioRechazo");

            InputParameterAdd.Int(objCommand, "idTipoMuestra", muestraCriterioRechazo.IdTipoMuestra);
            InputParameterAdd.Int(objCommand, "idCriterioRechazo", muestraCriterioRechazo.IdCriterioRechazo);
            InputParameterAdd.Int(objCommand, "estado", muestraCriterioRechazo.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", muestraCriterioRechazo.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}
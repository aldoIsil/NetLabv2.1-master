using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;

namespace DataLayer
{
    public class CriterioRechazoDal : DaoBase
    {
        public CriterioRechazoDal(IDalSettings settings) : base(settings)
        {
        }

        public CriterioRechazoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo ingresados por cada muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<CriterioRechazo> GetCriteriosByTipoMuestra(int idTipoMuestra, int tipo)
        {
            var objCommand = GetSqlCommand("pNLS_CriteriosRechazoByMuestra");
            InputParameterAdd.Int(objCommand, "idTipoMuestra", idTipoMuestra);
            InputParameterAdd.Int(objCommand, "tipo", tipo);

            return CriterioRechazoConvertTo.Criterios(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo ingresados filtrado por el nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<CriterioRechazo> GetCriterios(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_CriterioRechazoByNombre");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return CriterioRechazoConvertTo.Criterios(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo ingresados activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<CriterioRechazo> GetCriteriosActivos(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_CriterioRechazoByNombreActivos");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return CriterioRechazoConvertTo.Criterios(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo ingresados filtrado por el Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idCriterioRechazo"></param>
        /// <returns></returns>
        public CriterioRechazo GetCriterioById(int idCriterioRechazo)
        {
            var objCommand = GetSqlCommand("pNLS_CriterioRechazoById");
            InputParameterAdd.Int(objCommand, "idCriterioRechazo", idCriterioRechazo);

            return CriterioRechazoConvertTo.Criterio(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registro de un criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="criterioRechazo"></param>
        public void InsertCriterio(CriterioRechazo criterioRechazo)
        {
            var objCommand = GetSqlCommand("pNLI_CriterioRechazo");
            InputParameterAdd.Varchar(objCommand, "glosa", criterioRechazo.Glosa);
            InputParameterAdd.Int(objCommand, "tipo", criterioRechazo.Tipo);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", criterioRechazo.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualizacion de un criterio de rechazo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="criterioRechazo"></param>
        public void UpdateCriterio(CriterioRechazo criterioRechazo)
        {
            var objCommand = GetSqlCommand("pNLU_CriterioRechazo");
            InputParameterAdd.Int(objCommand, "idCriterioRechazo", criterioRechazo.IdCriterioRechazo);
            InputParameterAdd.Varchar(objCommand, "glosa", criterioRechazo.Glosa);
            InputParameterAdd.Int(objCommand, "tipo", criterioRechazo.Tipo);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", criterioRechazo.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", criterioRechazo.Estado);

            ExecuteNonQuery(objCommand);
        }
    }
    
}

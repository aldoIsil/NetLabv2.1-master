using System;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer
{
    public class AnalitoExamenDal : DaoBase
    {
        public AnalitoExamenDal(IDalSettings settings) : base(settings)
        {
        }

        public AnalitoExamenDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtener informacion de un analito por el Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<Analito> GetAnalitoByExamenId(Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_AnalitoByExamenId");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);

            return AnalitoConvertTo.Analitos(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registrar información  de un analito y el examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="analito"></param>
        public void InsertAnalitoByExamen(ExamenAnalito analito)
        {
            var objCommand = GetSqlCommand("pNLI_AnalitoExamen");

            InputParameterAdd.Guid(objCommand, "idExamen", analito.IdExamen);
            InputParameterAdd.Guid(objCommand, "idAnalito", analito.IdAnalito);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", analito.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza información  de un analito y el examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenAnalito"></param>
        public void UpdateAnalitoByExamen(ExamenAnalito examenAnalito)
        {
            var objCommand = GetSqlCommand("pNLU_AnalitoExamen");

            InputParameterAdd.Guid(objCommand, "idExamen", examenAnalito.IdExamen);
            InputParameterAdd.Guid(objCommand, "idAnalito", examenAnalito.IdAnalito);
            InputParameterAdd.Int(objCommand, "orden", examenAnalito.OrdenAnalito);
            InputParameterAdd.Int(objCommand, "estado", examenAnalito.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", examenAnalito.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}
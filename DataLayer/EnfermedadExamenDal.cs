using System;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;

namespace DataLayer
{
    public class EnfermedadExamenDal : DaoBase
    {
        public EnfermedadExamenDal(IDalSettings settings) : base(settings)
        {
        }

        public EnfermedadExamenDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene información delas Enfermedades por el Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<Enfermedad> GetEnfermedadByExamenId(Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_EnfermedadByExamenId");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);

            return EnfermedadConvertTo.Enfermedades(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registra/Actualiza información de los Enfermedades por enfermedad.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="enfermedad"></param>
        public void InsertEnfermedadByExamen(EnfermedadExamen enfermedad)
        {
            var objCommand = GetSqlCommand("pNLI_EnfermedadExamen");

            InputParameterAdd.Int(objCommand, "idEnfermedad", enfermedad.IdEnfermedad);
            InputParameterAdd.Guid(objCommand, "idExamen", enfermedad.IdExamen);
            InputParameterAdd.Int(objCommand, "orden", enfermedad.Orden);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", enfermedad.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza información de los Enfermedades por enfermedad.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="enfermedadExamen"></param>
        public void UpdateEnfermedadByExamen(EnfermedadExamen enfermedadExamen)
        {
            var objCommand = GetSqlCommand("pNLU_EnfermedadExamen");

            InputParameterAdd.Int(objCommand, "idEnfermedad", enfermedadExamen.IdEnfermedad);
            InputParameterAdd.Guid(objCommand, "idExamen", enfermedadExamen.IdExamen);
            InputParameterAdd.Int(objCommand, "orden", enfermedadExamen.Orden);
            InputParameterAdd.Int(objCommand, "estado", enfermedadExamen.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", enfermedadExamen.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}

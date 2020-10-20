using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System.Collections.Generic;

namespace DataLayer.Area.AreaProcesamiento
{
    public class AreaExamenDal : DaoBase
    {
        public AreaExamenDal(IDalSettings settings) : base(settings)
        {
        }

        public AreaExamenDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene examanes pertenecientes a un Area
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAreaProcesamiento"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByArea(int idAreaProcesamiento)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenByAreaId");
            InputParameterAdd.Int(objCommand, "idAreaProcesamiento", idAreaProcesamiento);

            return ExamenConvertTo.Examenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Actualiza el examen de un area.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="areaExamen"></param>
        public void UpdateExamenByArea(AreaProcesamientoExamen areaExamen)
        {
            var objCommand = GetSqlCommand("pNLU_ExamenArea");

            InputParameterAdd.Guid(objCommand, "idExamen", areaExamen.IdExamen);
            InputParameterAdd.Int(objCommand, "idAreaProcesamiento", areaExamen.IdAreaProcesamiento);
            InputParameterAdd.Int(objCommand, "estado", areaExamen.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", areaExamen.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Registra un examen para un area.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="areaExamen"></param>
        public void InsertExamenByArea(AreaProcesamientoExamen areaExamen)
        {
            var objCommand = GetSqlCommand("pNLI_ExamenArea");

            InputParameterAdd.Guid(objCommand, "idExamen", areaExamen.IdExamen);
            InputParameterAdd.Int(objCommand, "idAreaProcesamiento", areaExamen.IdAreaProcesamiento);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", areaExamen.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
    }
}

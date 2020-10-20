using System;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer
{
    public class LaboratorioExamenDal : DaoBase
    {
        public LaboratorioExamenDal(IDalSettings settings) : base(settings)
        {
        }

        public LaboratorioExamenDal() : this(new NetlabSettings())
        {
        }
        
        /// <summary>
        /// Descripción: Obtiene informacion de los examene por Id Establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<ExamenLaboratorio> GetExamenesByLaboratorio(int idLaboratorio)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenByLaboratorioId");
            InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);

            return ExamenLaboratorioConvertTo.Examenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene información de los examenes por Codigo de Examen y establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public ExamenLaboratorio GetExamenLaboratorioById(Guid idExamen, int idLaboratorio)
        {
            var objCommand = GetSqlCommand("pNLS_ExamenLaboratorioById");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);

            return ExamenLaboratorioConvertTo.ExamenLaboratorio(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Inserta el establecimiento de un examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenLaboratorio"></param>
        public void InsertExamenByLaboratorio(ExamenLaboratorio examenLaboratorio)
        {
            var objCommand = GetSqlCommand("pNLI_ExamenLaboratorio");

            InputParameterAdd.Guid(objCommand, "idExamen", examenLaboratorio.IdExamen);
            InputParameterAdd.Int(objCommand, "idLaboratorio", examenLaboratorio.IdLaboratorio);
            InputParameterAdd.Int(objCommand, "diasEmisionResultado", examenLaboratorio.DiasEmision);
            InputParameterAdd.Int(objCommand, "diasEntregaResultado", examenLaboratorio.DiasEntrega);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", examenLaboratorio.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza el establecimiento de un examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenLaboratorio"></param>
        public void UpdateExamenByLaboratorio(ExamenLaboratorio examenLaboratorio)
        {
            var objCommand = GetSqlCommand("pNLU_ExamenLaboratorio");

            InputParameterAdd.Guid(objCommand, "idExamen", examenLaboratorio.IdExamen);
            InputParameterAdd.Int(objCommand, "idLaboratorio", examenLaboratorio.IdLaboratorio);
            InputParameterAdd.Int(objCommand, "diasEmisionResultado", examenLaboratorio.DiasEmision);
            InputParameterAdd.Int(objCommand, "diasEntregaResultado", examenLaboratorio.DiasEntrega);
            InputParameterAdd.Int(objCommand, "estado", examenLaboratorio.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", examenLaboratorio.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}
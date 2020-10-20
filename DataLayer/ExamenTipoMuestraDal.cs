using System;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer
{
    public class ExamenTipoMuestraDal : DaoBase
    {
        public ExamenTipoMuestraDal(IDalSettings settings) : base(settings)
        {
        }

        public ExamenTipoMuestraDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtener los tipos de muestra por examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<ExamenTipoMuestra> GetTipoMuestraByExamen(Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_TipoMuestraByExamenId");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);

            return ExamenTipoMuestraConvertTo.TiposMuestra(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Regista o Actualiza Tipo de Muestra/Examen. 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenTipoMuestra"></param>
        public void InsertTipoMuestraByExamen(ExamenTipoMuestra examenTipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLI_ExamenTipoMuestra");

            InputParameterAdd.Guid(objCommand, "idExamen", examenTipoMuestra.IdExamen);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", examenTipoMuestra.IdTipoMuestra);
            InputParameterAdd.Varchar(objCommand, "caracteristicaMuestra", examenTipoMuestra.Caracteristica);
            InputParameterAdd.Varchar(objCommand, "transporteMuestra", examenTipoMuestra.Transporte);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", examenTipoMuestra.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza los tipos de muestra/examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenTipoMuestra"></param>
        public void UpdateTipoMuestraByExamen(ExamenTipoMuestra examenTipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLU_ExamenTipoMuestra");

            InputParameterAdd.Guid(objCommand, "idExamen", examenTipoMuestra.IdExamen);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", examenTipoMuestra.IdTipoMuestra);
            InputParameterAdd.Varchar(objCommand, "caracteristicaMuestra", examenTipoMuestra.Caracteristica);
            InputParameterAdd.Varchar(objCommand, "transporteMuestra", examenTipoMuestra.Transporte);
            InputParameterAdd.Int(objCommand, "estado", examenTipoMuestra.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", examenTipoMuestra.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}
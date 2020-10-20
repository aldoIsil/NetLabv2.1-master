using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer
{
    public class OrdenExamenDal : DaoBase
    {
        public OrdenExamenDal(IDalSettings settings) : base(settings)
        {
        }

        public OrdenExamenDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Registra la finalizacion del ingreso de resultados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenExamen"></param>
        public void UpdateOrdenExamenStatus(OrdenExamen ordenExamen)
        {
            var objCommand = GetSqlCommand("pNLU_ActualizarEstadoOrdenExamen");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", ordenExamen.idOrdenExamen);
            InputParameterAdd.Int(objCommand, "estatusE", ordenExamen.estatusE);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", ordenExamen.IdUsuarioEdicion);
            ExecuteNonQuery(objCommand);
        }
    }
}

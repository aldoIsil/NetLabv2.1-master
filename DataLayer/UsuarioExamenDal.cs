using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System.Data;
using System.Collections.Generic;
using DataLayer.DalConverter;

namespace DataLayer
{
    public class UsuarioExamenDal : DaoBase
    {
        public UsuarioExamenDal(IDalSettings settings) : base(settings)
        {
        }

        public UsuarioExamenDal() : this(new NetlabSettings())
        {
        }
        public List<Examen> GetExamenByUsuarioId(int idUsuario)
        {
            List<Examen> listaExamen = new List<Examen>();

            var objCommand = GetSqlCommand("pNLS_ExamenByUsuarioId");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);

            DataTable dataExamen = Execute(objCommand);
            if (dataExamen.Rows.Count == 0)
                return null;

            return ExamenConvertTo.Examenes(Execute(objCommand));
        }
        public void UpdateExamenByUsuario(ExamenUsuario examenUsuario)
        {
            var objCommand = GetSqlCommand("pNLU_UsuarioEnfermedadExamen");

            InputParameterAdd.Guid(objCommand, "idExamen", examenUsuario.IdExamen);
            InputParameterAdd.Int(objCommand, "idUsuario", examenUsuario.idUsuario);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", examenUsuario.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
        public void InsertExamenByUsuario(ExamenUsuario examenUsuario, int TipoUsuarioExamen)
        {
            var objCommand = GetSqlCommand("pNLI_UsuarioEnfermedadExamen");

            InputParameterAdd.Guid(objCommand, "idExamen", examenUsuario.IdExamen);
            InputParameterAdd.Int(objCommand, "idUsuario", examenUsuario.idUsuarioEnfermedadExamen);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", examenUsuario.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idTipo", TipoUsuarioExamen);

            ExecuteNonQuery(objCommand);
        }
        public void InsertExamenByUsuarioEnfermedad(int idusuario,int idUsuarioRegistro, int TipoUsuarioExamen, int idEnfermedad)
        {
            var objCommand = GetSqlCommand("pNLI_UsuarioEnfermedadExamenbyEnfermedad");
            
            InputParameterAdd.Int(objCommand, "idUsuario", idusuario);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", idUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idTipo", TipoUsuarioExamen);
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);

            ExecuteNonQuery(objCommand);
        }
    }
}

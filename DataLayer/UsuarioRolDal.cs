using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System.Data;

namespace DataLayer
{
    public class UsuarioRolDal : DaoBase
    {
        public UsuarioRolDal(IDalSettings settings) : base(settings)
        {
        }

        public UsuarioRolDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Metodo para obtener los roles de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Rol> GetRolByUsuarioId(int idUsuario)
        {
            List<Rol> listaRol = new List<Rol>();

            var objCommand = GetSqlCommand("pNLS_RolByUsuarioId");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);

            DataTable dataRol = Execute(objCommand);
            if (dataRol.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataRol.Rows.Count; i++)
            {
                var row = dataRol.Rows[i];
                Rol rol = new Rol();
                rol.idRol = int.Parse(row["idRol"].ToString());
                rol.nombre = row["nombre"].ToString();
                rol.descripcion = row["descripcion"].ToString();

                listaRol.Add(rol);
            }

            return listaRol;
        }
        /// <summary>
        /// Descripción: Registra rol para un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuarioRol"></param>
        public void InsertRolByUsuario(UsuarioRol usuarioRol)
        {
            var objCommand = GetSqlCommand("pNLI_UsuarioRol");

            InputParameterAdd.Int(objCommand, "idUsuario", usuarioRol.idUsuario);
            InputParameterAdd.Int(objCommand, "idRol", usuarioRol.idRol);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", usuarioRol.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Edita rol para un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuarioRol"></param>
        public void UpdateRolByUsuario(UsuarioRol usuarioRol)
        {
            var objCommand = GetSqlCommand("pNLU_UsuarioRol");

            InputParameterAdd.Int(objCommand, "idUsuario", usuarioRol.idUsuario);
            InputParameterAdd.Int(objCommand, "idRol", usuarioRol.idRol);
            InputParameterAdd.Int(objCommand, "estado", usuarioRol.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", usuarioRol.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}
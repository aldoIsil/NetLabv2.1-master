using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using System.Data;
using Model;
using DataLayer.DalConverter;

namespace DataLayer
{
    public class RolDal : DaoBase
    {
        public RolDal(IDalSettings settings) : base(settings)
        {
        }

        public RolDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de un rol filtrado por el Nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Rol> GetRoles(string nombre)
        {
            List<Rol> listaRoles = new List<Rol>();
            var objCommand = GetSqlCommand("pNLS_RolByNombre");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            DataTable dataRoles = Execute(objCommand);

            if (dataRoles.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataRoles.Rows.Count; i++)
            {
                var row = dataRoles.Rows[i];
                Rol rol = new Rol();
                rol.idRol = int.Parse(row["idRol"].ToString());
                rol.nombre = row["nombre"].ToString();
                rol.descripcion = row["descripcion"].ToString();
                rol.Estado = int.Parse(row["estado"].ToString());
                rol.tipo = row["tipo"].ToString();
                listaRoles.Add(rol);
            }

            return listaRoles;
        }
        /// <summary>
        /// Descripción: Obtiene informacion de un rol filtrado por el id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public Rol GetRolById(int idRol)
        {
            var objCommand = GetSqlCommand("pNLS_RolById");
            InputParameterAdd.Int(objCommand, "IdRol", idRol);

            return RolConvertTo.Rol(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registra un nuevo Rol
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="rol"></param>
        public void InsertRol(Rol rol)
        {
            var objCommand = GetSqlCommand("pNLI_Rol");
            InputParameterAdd.Varchar(objCommand, "Nombre", rol.nombre);
            InputParameterAdd.Varchar(objCommand, "Descripcion", rol.descripcion);
            InputParameterAdd.Int(objCommand, "IdUsuarioRegistro", rol.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza informacion de un Rol.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="rol"></param>
        public void UpdateRol(Rol rol)
        {
            var objCommand = GetSqlCommand("pNLU_Rol");
            InputParameterAdd.Int(objCommand, "IdRol", rol.idRol);
            InputParameterAdd.Varchar(objCommand, "Nombre", rol.nombre);
            InputParameterAdd.Varchar(objCommand, "Descripcion", rol.descripcion);
            InputParameterAdd.Int(objCommand, "IdUsuarioEdicion", rol .IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "Estado", rol.Estado);

            ExecuteNonQuery(objCommand);
        }
    }
}

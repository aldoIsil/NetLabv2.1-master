using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System.Data;

namespace DataLayer
{
    public class RolMenuDal : DaoBase
    {
        public RolMenuDal(IDalSettings settings) : base(settings)
        {
        }

        public RolMenuDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los menu/rol activos filtrado por Id Rol.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public List<Menu> GetMenuByRolId(int idRol)
        {
            List<Menu> listaMenu = new List<Menu>();

            var objCommand = GetSqlCommand("pNLS_MenuByRolId");
            InputParameterAdd.Int(objCommand, "idRol", idRol);

            DataTable dataMenu = Execute(objCommand);
            if (dataMenu.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataMenu.Rows.Count; i++)
            {
                var row = dataMenu.Rows[i];
                Menu menu = new Menu();
                menu.idMenu = int.Parse(row["idMenu"].ToString());
                menu.nombre = row["nombre"].ToString();
                menu.descripcion = row["descripcion"].ToString();

                listaMenu.Add(menu);
            }

            return listaMenu;
        }
        /// <summary>
        /// Descripción: Registra/Actualiza información de roles por menu.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="rolMenu"></param>
        public void InsertMenuByRol(RolMenu rolMenu)
        {
            var objCommand = GetSqlCommand("pNLI_RolMenu");

            InputParameterAdd.Int(objCommand, "idRol", rolMenu.IdRol);
            InputParameterAdd.Int(objCommand, "idMenu", rolMenu.IdMenu);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", rolMenu.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza informacion del rol/menu.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="rolMenu"></param>
        public void UpdateMenuByRol(RolMenu rolMenu)
        {
            var objCommand = GetSqlCommand("pNLU_RolMenu");

            InputParameterAdd.Int(objCommand, "idRol", rolMenu.IdRol);
            InputParameterAdd.Int(objCommand, "idMenu", rolMenu.IdMenu);
            InputParameterAdd.Int(objCommand, "estado", rolMenu.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", rolMenu.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}
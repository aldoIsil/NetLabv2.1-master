using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;
using System.Data;

namespace DataLayer
{
    public class MenuDal : DaoBase
    {
        public MenuDal(IDalSettings settings) : base(settings)
        {
        }

        public MenuDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los menu/rol activos filtrado por IdRol
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios. 
        /// </summary>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public List<Menu> GetMenuByRol(int idRol)
        {
            List<Menu> listaMenu = new List<Menu>();

            var objCommand = GetSqlCommand("pNLS_MenuByRol");
            InputParameterAdd.Int(objCommand, "IdRol", idRol);

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
        /// Descripción: Obtiene informacion de los menus activos filtrado por Nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Menu> GetMenues(string nombre)
        {
            List<Menu> listaMenu = new List<Menu>();

            var objCommand = GetSqlCommand("pNLS_MenuByNombre");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

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
    }    
}

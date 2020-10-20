using DataLayer;
using System.Collections.Generic;
using BusinessLayer.Interfaces;
using Model;

namespace BusinessLayer
{
    public class MenuBl : IMenuBl
    {
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
            using (var menuDal = new MenuDal())
            {
                return menuDal.GetMenuByRol(idRol);
            }
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
            using (var menuDal = new MenuDal())
            {
                return menuDal.GetMenues(nombre);
            }

        }
    }
}

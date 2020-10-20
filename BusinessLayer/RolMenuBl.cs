using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class RolMenuBl : IRolMenuBl
    {
        /// <summary>
        /// Descripción: Obtiene todos los elementos agregados para registrarlos mediante una transaccion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="rol"></param>
        /// <param name="menues"></param>
        /// <param name="idUsuario"></param>
        public void AgregarMenuPorRol(Rol rol, int[] menues, int idUsuario)
        {
            if (menues == null || !menues.Any()) return;

            var menuByRol = GetMenuByRolId(rol.idRol);

            if (menuByRol != null)
            {
                menues = menues.Where(x => menuByRol.All(y => y.idMenu != x)).ToArray();
                var listMenues = menues.Select(menu => GetMenuRol(rol, menu, idUsuario)).ToList();
                InsertMenuByRol(listMenues);
            }
            else
            {
                var listMenues = menues.Select(menu => GetMenuRol(rol, menu, idUsuario)).ToList();
                InsertMenuByRol(listMenues);
            }
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
            using (var rolMenuDal = new RolMenuDal())
            {
                return rolMenuDal.GetMenuByRolId(idRol);
            }
        }
        /// <summary>
        /// Descripción: Retorna la entidad RolMenu.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="rol"></param>
        /// <param name="idMenu"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private static RolMenu GetMenuRol(Rol rol, int idMenu, int idUsuario)
        {
            return new RolMenu
            {
                IdRol = rol.idRol,
                IdMenu = idMenu,
                IdUsuarioRegistro = idUsuario
            };
        }
        /// <summary>
        /// Descripción: Registra/Actualiza información de roles por menu.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="rolMenu"></param>
        private static void InsertMenuByRol(IEnumerable<RolMenu> menues)
        {
            using (var rolMenuDal = new RolMenuDal())
            {
                rolMenuDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var item in menues)
                    {
                        rolMenuDal.InsertMenuByRol(item);
                    }

                    rolMenuDal.Commit();
                }
                catch (Exception)
                {
                    rolMenuDal.Rollback();
                }
            }
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
            using (var rolMenuDal = new RolMenuDal())
            {
                rolMenuDal.UpdateMenuByRol(rolMenu);
            }
        }


    }
}
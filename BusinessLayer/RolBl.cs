using DataLayer;
using System;
using System.Collections.Generic;
using BusinessLayer.Interfaces;
using Model;

namespace BusinessLayer
{
    public class RolBl : IRolBl
    {
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
            using (var rolDal = new RolDal())
            {
                return rolDal.GetRolById(idRol);
            }
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
            try
            {
                using (var rolDal = new RolDal())
                {

                    return rolDal.GetRoles(nombre);
                }
            }
            catch (Exception)
            {

            }

            return null;
        }

        public List<Rol> GetRolesByUsuario(int idUsuario)
        {
            throw new NotImplementedException();
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
            using (var rolDal = new RolDal())
            {

                rolDal.InsertRol(rol);
            }
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
            using (var rolDal = new RolDal())
            {

                rolDal.UpdateRol(rol);
            }
        }
    }
}

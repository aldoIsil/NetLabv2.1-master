using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class UsuarioRolBl : IUsuarioRolBl
    {
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
            using (var usuarioRolDal = new UsuarioRolDal())
            {
                return usuarioRolDal.GetRolByUsuarioId(idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Metodo para registrar todos los roles seleccionados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="roles"></param>
        /// <param name="idUsuario"></param>
        public void AgregarRolPorUsuario(Usuario usuario, int[] roles, int idUsuario)
        {
            if (roles == null || !roles.Any()) return;

            var rolByUsuario = GetRolByUsuarioId(usuario.idUsuario);

            if (rolByUsuario != null)
            {
                roles = roles.Where(x => rolByUsuario.All(y => y.idRol != x)).ToArray();
                var listRoles = roles.Select(rol => GetRolUsuario(usuario, rol, idUsuario)).ToList();
                InsertRolByUsuario(listRoles);
            }
            else
            {
                var listRoles = roles.Select(rol => GetRolUsuario(usuario, rol, idUsuario)).ToList();
                InsertRolByUsuario(listRoles);
            }
        }
        /// <summary>
        /// Descripción: Devuelve la entidad UsuarioRol cargada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="idRol"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private static UsuarioRol GetRolUsuario(Usuario usuario, int idRol, int idUsuario)
        {
            return new UsuarioRol
            {
                idUsuario = usuario.idUsuario,
                idRol = idRol,
                IdUsuarioRegistro = idUsuario
            };
        }
        /// <summary>
        /// Descripción: Registra roles para un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuarioRol"></param>
        private static void InsertRolByUsuario(IEnumerable<UsuarioRol> roles)
        {
            using (var usuarioRolDal = new UsuarioRolDal())
            {
                usuarioRolDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var item in roles)
                    {
                        usuarioRolDal.InsertRolByUsuario(item);
                    }

                    usuarioRolDal.Commit();
                }
                catch (Exception)
                {
                    usuarioRolDal.Rollback();
                }
            }
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
            using (var usuarioRolDal = new UsuarioRolDal())
            {
                usuarioRolDal.UpdateRolByUsuario(usuarioRol);
            }
        }
    }
}
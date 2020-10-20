using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class UsuarioAreaProcesamientoBl : IUsuarioAreaProcesamientoBl
    {
        /// <summary>
        /// Descripción: Obtiene Informacion de las areas enlazadas a un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Model.AreaProcesamiento> GetAreaByUsuarioId(int idUsuario)
        {
            using (var usuarioAreaProcesamientoDal = new UsuarioAreaProcesamientoDal())
            {
                return usuarioAreaProcesamientoDal.GetAreaProcesamientoByUsuarioId(idUsuario);
            }
        }
        /// <summary>
        /// Descripción:  Brinda mantenimiento a la tabla UsuarioAreaProcesamiento, agreando un nuevo registro
        ///               siempre y cuando no exista.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="areas"></param>
        /// <param name="idUsuario"></param>
        public void AgregarAreaPorUsuario(Usuario usuario, int[] areas, int idUsuario)
        {
            if (areas == null || !areas.Any()) return;

            var areaByUsuario = GetAreaByUsuarioId(usuario.idUsuario);

            if (areaByUsuario != null)
            {
                areas = areas.Where(x => areaByUsuario.All(y => y.IdAreaProcesamiento != x)).ToArray();
                var listAreas = areas.Select(area => GetAreaProcesamientoUsuario(usuario, area, idUsuario)).ToList();
                InsertAreaByUsuario(listAreas);
            }
            else
            {
                var listAreas = areas.Select(area => GetAreaProcesamientoUsuario(usuario, area, idUsuario)).ToList();
                InsertAreaByUsuario(listAreas);
            }
        }
        /// <summary>
        /// Descripción: Devuelve la entidad UsusarioAreaProcesamiento cargada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="idAreaProcesamiento"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private static UsuarioAreaProcesamiento GetAreaProcesamientoUsuario(Usuario usuario, int idAreaProcesamiento, int idUsuario)
        {
            return new UsuarioAreaProcesamiento
            {
                idUsuario = usuario.idUsuario,
                idAreaProcesamiento = idAreaProcesamiento,
                IdUsuarioRegistro = idUsuario
            };
        }
        /// <summary>
        /// Descripción: Registra informacion del area de procesamiento para un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="areas"></param>
        private static void InsertAreaByUsuario(IEnumerable<UsuarioAreaProcesamiento> areas)
        {
            using (var usuarioAreaProcesamientoDal = new UsuarioAreaProcesamientoDal())
            {
                usuarioAreaProcesamientoDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var item in areas)
                    {
                        usuarioAreaProcesamientoDal.InsertAreaByUsuario(item);
                    }

                    usuarioAreaProcesamientoDal.Commit();
                }
                catch (Exception)
                {
                    usuarioAreaProcesamientoDal.Rollback();
                }
            }
        }
        /// <summary>
        /// Descripción: Actualiza informacion del area de procesamiento para un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="usuarioAreaProcesamiento"></param>
        public void UpdateAreaByUsuario(UsuarioAreaProcesamiento usuarioAreaProcesamiento)
        {
            using (var usuarioAreaProcesamientoDal = new UsuarioAreaProcesamientoDal())
            {
                usuarioAreaProcesamientoDal.UpdateAreaByUsuario(usuarioAreaProcesamiento);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;


namespace BusinessLayer
{
    public class ReactivoPresentacionBl : IReactivoPresentacionBl
    {
        /// <summary>
        /// Descripción: Insertar/Actualizar un tipo de reactivo a una presentacion 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="reactivo"></param>
        /// <param name="presentaciones"></param>
        /// <param name="idUsuario"></param>
        public void AgregarPresentacionesPorReactivo(Reactivo reactivo, int[] presentaciones, int idUsuario)
        {
            if (presentaciones == null || !presentaciones.Any()) return;

            var presentacionesByReactivo = GetPresentacionesByReactivoId(reactivo.idReactivo);

            presentaciones = presentaciones.Where(x => presentacionesByReactivo.All(y => y.idPresentacion != x)).ToArray();

            var listPresentaciones = presentaciones.Select(presentacion => GetPresentacionReactivo(reactivo, presentacion, idUsuario)).ToList();

            InsertPresentacionByReactivo(listPresentaciones);
        }
        /// <summary>
        /// Descripción: Obtiene una presentacion por el codigo de reactivo activa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idReactivo"></param>
        /// <returns></returns>
        public List<Presentacion> GetPresentacionesByReactivoId(int idReactivo)
        {
            using (var presentacionReactivoDal = new PresentacionReactivoDal())
            {
                return presentacionReactivoDal.GetPresentacionesByReactivoId(idReactivo);
            }

        }
        /// <summary>
        /// Descripción: Obtiene una presentacion activa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Presentacion> GetPresentacionesActivas(string nombre)
        {
            using (var presentacionReactivoDal = new PresentacionReactivoDal())
            {
                return presentacionReactivoDal.GetPresentacionesActivas(nombre);
            }

        }
        /// <summary>
        /// Descripción: Retorna la entidad PresentacionReactivo 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="reactivo"></param>
        /// <param name="idPresentacion"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private static PresentacionReactivo GetPresentacionReactivo(Reactivo reactivo, int idPresentacion, int idUsuario)
        {
            return new PresentacionReactivo
            {
                idReactivo = reactivo.idReactivo,
                idPresentacion = idPresentacion,
                IdUsuarioRegistro = idUsuario
            };
        }
        /// <summary>
        /// Descripción: Insertar/Actualizar un tipo de reactivo a una presentacion 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentacionReactivo"></param>
        private static void InsertPresentacionByReactivo(IEnumerable<PresentacionReactivo> presentaciones)
        {
            using (var presentacionReactivoDal = new PresentacionReactivoDal())
            {
                presentacionReactivoDal.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    foreach (var item in presentaciones)
                    {
                        presentacionReactivoDal.InsertPresentacionByReactivo(item);
                    }

                    presentacionReactivoDal.Commit();
                }
                catch (Exception)
                {
                    presentacionReactivoDal.Rollback();
                }
            }
        }
        /// <summary>
        /// Descripción: Actualiza una presenatacion/reactivo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="presentacionReactivo"></param>
        public void UpdatePresentacionByReactivo(PresentacionReactivo presentacionReactivo)
        {
            using (var presentacionReactivoDal = new PresentacionReactivoDal())
            {
                presentacionReactivoDal.UpdatePresentacionByReactivo(presentacionReactivo);
            }

        }

    }
}

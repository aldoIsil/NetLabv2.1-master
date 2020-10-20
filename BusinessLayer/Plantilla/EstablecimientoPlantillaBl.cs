using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLayer.Interfaces;
using DataLayer.Plantilla;
using Model;

namespace BusinessLayer.Plantilla
{
    public class EstablecimientoPlantillaBl : IEstablecimientoPlantillaBl
    {
        /// <summary>
        /// Descripción: Agregar establecimientos asignados a una plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="plantilla"></param>
        /// <param name="establecimientos"></param>
        /// <param name="idUsuario"></param>
        public void AgregarEstablecimientosPorPlantilla(Model.Plantilla plantilla, int[] establecimientos, int idUsuario)
        {
            if (establecimientos == null || !establecimientos.Any()) return;

            var establecimientosByPlantilla = GetEstablecimientosByPlantillaId(plantilla.IdPlantilla);

            establecimientos = establecimientos.Where(x => establecimientosByPlantilla.All(y => y.IdEstablecimiento != x)).ToArray();

            var listEstablecimientos = establecimientos.Select(idEstablecimiento => new PlantillaEstablecimiento
            {
                IdPlantilla = plantilla.IdPlantilla,
                IdEstablecimiento = idEstablecimiento,
                IdUsuarioRegistro = idUsuario
            });

            InsertEstablecimientoByPlantilla(listEstablecimientos);
        }
        /// <summary>
        /// Descripción: Metodo que ejecuta el mantenimiento el establecimiento de la plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="establecimientos"></param>
        private static void InsertEstablecimientoByPlantilla(IEnumerable<PlantillaEstablecimiento> establecimientos)
        {
            using (var establecimientoPlantillaDal = new EstablecimientoPlantillaDal())
            {
                establecimientoPlantillaDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var item in establecimientos)
                    {
                        establecimientoPlantillaDal.InsertEstablecimientoByPlantilla(item);
                    }

                    establecimientoPlantillaDal.Commit();
                }
                catch (Exception)
                {
                    establecimientoPlantillaDal.Rollback();
                }
            }
        }
        /// <summary>
        /// Descripción: Obtiene los establecimientos por el codigo de la plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosByPlantillaId(int idPlantilla)
        {
            using (var establecimientoPlantillaDal = new EstablecimientoPlantillaDal())
            {
                return establecimientoPlantillaDal.GetEstablecimientosByPlantillaId(idPlantilla);
            }
        }
        /// <summary>
        /// Descripción: Metodo que actualiza los establecimientos de una plantilla
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="plantillaEstablecimiento"></param>
        public void UpdateEstablecimientoByPlantilla(PlantillaEstablecimiento plantillaEstablecimiento)
        {
            using (var establecimientoPlantillaDal = new EstablecimientoPlantillaDal())
            {
                establecimientoPlantillaDal.UpdateEstablecimientoByPlantilla(plantillaEstablecimiento);
            }
        }
    }
}
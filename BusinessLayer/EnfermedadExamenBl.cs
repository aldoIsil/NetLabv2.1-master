using BusinessLayer.Interfaces;
using System.Collections.Generic;
using DataLayer;
using System;
using System.Data;
using System.Linq;
using Model;

namespace BusinessLayer
{
    public class EnfermedadExamenBl : IEnfermedadExamenBl
    {
        /// <summary>
        /// Descripción: Realiza la insercion de las enfermedades a un examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="examen"></param>
        /// <param name="enfermedades"></param>
        /// <param name="idUsuario"></param>
        public void AgregarEnfermedadesPorExamen(Examen examen, int[] enfermedades, int idUsuario)
        {
            if (enfermedades == null || !enfermedades.Any()) return;

            var enfermedadesByExamen = GetEnfermedadByExamenId(examen.idExamen);

            enfermedades = enfermedades.Where(x => enfermedadesByExamen.All(y => y.idEnfermedad != x)).ToArray();

            var listEnfermedades = enfermedades.Select(enf => GetEnfermedadExamen(examen, enf, idUsuario)).ToList();

            InsertEnfermedadByExamen(listEnfermedades);
        }
        /// <summary>
        /// Descripción: Retorna la entidad EnfermedadExamen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="examen"></param>
        /// <param name="idEnfermedad"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private static EnfermedadExamen GetEnfermedadExamen(Examen examen, int idEnfermedad, int idUsuario)
        {
            return new EnfermedadExamen
            {
                IdExamen = examen.idExamen,
                IdEnfermedad = idEnfermedad,
                Orden = 1, //TODO: Determinar el orden
                IdUsuarioRegistro = idUsuario
            };
        }
        /// <summary>
        /// Descripción: Obtiene información delas Enfermedades por el Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<Enfermedad> GetEnfermedadByExamenId(Guid idExamen)
        {
            using (var enfermedadDal = new EnfermedadExamenDal())
            {
                return enfermedadDal.GetEnfermedadByExamenId(idExamen);
            }
        }
        /// <summary>
        /// Descripción: Registra/Actualiza información de los Enfermedades por enfermedad.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="enfermedades"></param>
        public void InsertEnfermedadByExamen(List<EnfermedadExamen> enfermedades)
        {
            using (var enfermedadDal = new EnfermedadExamenDal())
            {
                enfermedadDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    foreach (var item in enfermedades)
                    {
                        enfermedadDal.InsertEnfermedadByExamen(item);
                    }

                    enfermedadDal.Commit();
                }
                catch (Exception)
                {
                    enfermedadDal.Rollback();
                }
            }
        }
        /// <summary>
        /// Descripción: Actualiza información de los Enfermedades por enfermedad.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="enfermedadExamen"></param>
        public void UpdateEnfermedadByExamen(EnfermedadExamen enfermedadExamen)
        {
            using (var enfermedadDal = new EnfermedadExamenDal())
            {
                enfermedadDal.UpdateEnfermedadByExamen(enfermedadExamen);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessLayer.Interfaces;
using Model;
using DataLayer.Area.AreaProcesamiento;

namespace BusinessLayer.AreaProcesamiento
{
    public class AreaExamenBl : IAreaExamenBl
    {
        /// <summary>
        /// Descripción: Obtiene examanes pertenecientes a un Area
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAreaProcesamiento"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByArea(int idAreaProcesamiento)
        {
            using (var areaExamenDal = new AreaExamenDal())
            {
                return areaExamenDal.GetExamenesByArea(idAreaProcesamiento);
            }
        }
        /// <summary>
        /// Descripción: Registra un examen para un area.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="examenes"></param>
        /// <param name="idUsuario"></param>
        public void InsertExamenesByArea(Model.AreaProcesamiento area, string[] examenes, int idUsuario)
        {
            if (examenes == null || !examenes.Any()) return;

            var examenesByLaboratorio = GetExamenesByArea(area.IdAreaProcesamiento);

            examenes = examenes.Where(x => examenesByLaboratorio.All(y => !string.Equals(y.idExamen.ToString(), x, StringComparison.CurrentCultureIgnoreCase))).ToArray();

            var listExamenes = examenes.Select(examen => GetAreaExamen(area.IdAreaProcesamiento, examen, idUsuario)).ToList();

            InsertExamenByArea(listExamenes);
        }
        /// <summary>
        /// Descripción: Actualiza el examen de un area.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="areaExamen"></param>
        public void UpdateExamenByArea(AreaProcesamientoExamen areaExamen)
        {
            using (var examenDal = new AreaExamenDal())
            {
                examenDal.UpdateExamenByArea(areaExamen);
            }
        }
        /// <summary>
        /// Descripción: Devuelve la entidad AreaProcesamiento cargada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAreaProcesamiento"></param>
        /// <param name="examen"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        private static AreaProcesamientoExamen GetAreaExamen(int idAreaProcesamiento, string examen, int idUsuario)
        {
            return new AreaProcesamientoExamen
            {
                IdAreaProcesamiento = idAreaProcesamiento,
                IdExamen = new Guid(examen),
                IdUsuarioRegistro = idUsuario
            };
        }
        /// <summary>
        /// Descripción: Registra un examen para un area.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenes"></param>
        private static void InsertExamenByArea(IEnumerable<AreaProcesamientoExamen> examenes)
        {
            using (var examenDal = new AreaExamenDal())
            {
                examenDal.BeginTransaction(IsolationLevel.ReadCommitted);

                try
                {
                    foreach (var item in examenes)
                    {
                        examenDal.InsertExamenByArea(item);
                    }

                    examenDal.Commit();
                }
                catch (Exception)
                {
                    examenDal.Rollback();
                }
            }
        }
    }
}

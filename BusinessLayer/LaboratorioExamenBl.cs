using System;
using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class LaboratorioExamenBl : ILaboratorioExamenBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de los examene por Id Establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<ExamenLaboratorio> GetExamenesByLaboratorio(int idLaboratorio)
        {
            using (var examenDal = new LaboratorioExamenDal())
            {
                return examenDal.GetExamenesByLaboratorio(idLaboratorio);
            }
        }
        /// <summary>
        /// Descripción: Obtiene información de los examenes por Codigo de Examen y establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public ExamenLaboratorio GetExamenLaboratorioById(Guid idExamen, int idLaboratorio)
        {
            using (var examenDal = new LaboratorioExamenDal())
            {
                return examenDal.GetExamenLaboratorioById(idExamen, idLaboratorio);
            }
        }
        /// <summary>
        /// Descripción: Inserta el establecimiento de un examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenLaboratorio"></param>
        public void InsertExamenByLaboratorio(ExamenLaboratorio examenLaboratorio)
        {
            using (var examenDal = new LaboratorioExamenDal())
            {
                examenDal.InsertExamenByLaboratorio(examenLaboratorio);
            }
        }
        /// <summary>
        /// Descripción: Actualiza el establecimiento de un examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenLaboratorio"></param>
        public void UpdateExamenByLaboratorio(ExamenLaboratorio examenLaboratorio)
        {
            using (var examenDal = new LaboratorioExamenDal())
            {
                examenDal.UpdateExamenByLaboratorio(examenLaboratorio);
            }
        }
    }
}
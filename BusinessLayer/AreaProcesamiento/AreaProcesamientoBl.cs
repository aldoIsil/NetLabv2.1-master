using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer.Area.AreaProcesamiento;

namespace BusinessLayer.AreaProcesamiento
{
    public class AreaProcesamientoBl : IAreaProcesamientoBl
    {
        /// <summary>
        /// Descripción: Obtiene las areas de procesamiento filtrada por el nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Model.AreaProcesamiento> GetAreasProcesamiento(string nombre)
        {
            using (var areaProcesamientoDal = new AreaProcesamientoDal())
            {
                return areaProcesamientoDal.GetAreasProcesamiento(nombre);
            }
        }
        /// <summary>
        /// Descripción: Obtiene las areas de procesamiento filtrada por el id del Area de procesamiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.AreaProcesamiento GetAreaProcesamientoById(int id)
        {
            using (var areaProcesamientoDal = new AreaProcesamientoDal())
            {
                return areaProcesamientoDal.GetAreaProcesamientoById(id);
            }
        }
        /// <summary>
        /// Descripción: Realiza el registro de un area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="areaProcesamiento"></param>
        public void InsertAreaProcesamiento(Model.AreaProcesamiento areaProcesamiento)
        {
            using (var areaProcesamientoDal = new AreaProcesamientoDal())
            {
                areaProcesamientoDal.InsertAreaProcesamiento(areaProcesamiento);
            }
        }
        /// <summary>
        /// Descripción: Realiza la actualización de un area de procesamiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="areaProcesamiento"></param>
        public void UpdateAreaProcesamiento(Model.AreaProcesamiento areaProcesamiento)
        {
            using (var areaProcesamientoDal = new AreaProcesamientoDal())
            {
                areaProcesamientoDal.UpdateAreaProcesamiento(areaProcesamiento);
            }
        }
    }
}
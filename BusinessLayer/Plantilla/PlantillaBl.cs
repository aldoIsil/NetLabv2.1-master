using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer.Plantilla;
using Model;

namespace BusinessLayer.Plantilla
{
    public class PlantillaBl : IPlantillaBl
    {
        /// <summary>
        /// Descripción: Metodo que obtiene una plantilla filtrado por el nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Model.Plantilla> ObtenerPlantillas(string nombre)
        {
            using (var plantillaDal = new PlantillaDal())
            {
                return plantillaDal.GetPlantillas(nombre);
            }
        }
        /// <summary>
        /// Descripción: Metodo que obtiene una plantilla filtrado por el Codigo de la Plantilla
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        public Model.Plantilla GetPlantillaById(int idPlantilla)
        {
            using (var plantillaDal = new PlantillaDal())
            {
                return plantillaDal.GetPlantillaById(idPlantilla);
            }
        }
        /// <summary>
        /// Descripción: Metodo que Obtiene plantillas de acuerdo a un establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        public List<Model.Plantilla> GetPlantillaByEstablecimiento(int idEstablecimiento)
        {
            using (var plantillaDal = new PlantillaDal())
            {
                return plantillaDal.GetPlantillaByEstablecimiento(idEstablecimiento);
            }
        }
        /// <summary>
        /// Descripción: Metodo que registra la plantilla
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="plantilla"></param>
        public void InsertPlantilla(Model.Plantilla plantilla)
        {
            using (var plantillaDal = new PlantillaDal())
            {
                plantillaDal.InsertPlantilla(plantilla);
            }
        }
        /// <summary>
        /// Descripción: Metodo que actualiza registro de una plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="plantilla"></param>
        public void UpdatePlantilla(Model.Plantilla plantilla)
        {
            using (var plantillaDal = new PlantillaDal())
            {
                plantillaDal.UpdatePlantilla(plantilla);
            }
        }
        /// <summary>
        /// Descripción: Metodo que retorna la lista de las muestras
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios. 
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        public List<PlantillaEnfermedadExamen> ObtenerMuestras(int idPlantilla)
        {
            using (var plantillaDal = new PlantillaEnfermedadExamenDal())
            {
                return plantillaDal.GetMuestras(idPlantilla);
            }
        }
        /// <summary>
        /// Descripción: Metodo que registra la plantilla de enfermedades
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="muestra"></param>
        public void InsertMuestra(PlantillaEnfermedadExamen muestra)
        {
            using (var plantillaDal = new PlantillaEnfermedadExamenDal())
            {
                plantillaDal.InsertMuestra(muestra);
            }
        }
        /// <summary>
        /// Descripción: Metodo que actualiza la plantilla de enfermedades
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="muestra"></param>
        public void UpdateMuestra(PlantillaEnfermedadExamen muestra)
        {
            using (var plantillaDal = new PlantillaEnfermedadExamenDal())
            {
                plantillaDal.UpdateMuestra(muestra);
            }
        }
    }
}

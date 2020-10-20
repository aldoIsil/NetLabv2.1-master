using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IPlantillaBl
    {
        List<Model.Plantilla> ObtenerPlantillas(string nombre);
        void InsertPlantilla(Model.Plantilla plantilla);
        Model.Plantilla GetPlantillaById(int id);
        void UpdatePlantilla(Model.Plantilla plantilla);
        void InsertMuestra(PlantillaEnfermedadExamen muestra);
        List<PlantillaEnfermedadExamen> ObtenerMuestras(int idPlantilla);
        void UpdateMuestra(PlantillaEnfermedadExamen muestra);
        List<Model.Plantilla> GetPlantillaByEstablecimiento(int idEstablecimiento);
    }
}
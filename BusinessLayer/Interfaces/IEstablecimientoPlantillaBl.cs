using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IEstablecimientoPlantillaBl
    {
        List<Establecimiento> GetEstablecimientosByPlantillaId(int idPlantilla);
        void AgregarEstablecimientosPorPlantilla(Model.Plantilla plantilla, int[] establecimientos, int idUsuario);
        void UpdateEstablecimientoByPlantilla(PlantillaEstablecimiento plantillaEstablecimiento);
    }
}
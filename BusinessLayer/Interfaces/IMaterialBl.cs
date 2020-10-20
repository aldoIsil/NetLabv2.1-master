using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IMaterialBl
    {
        List<Material> GetMateriales(string nombre);
        List<Material> GetMaterialesByIdTipoMuestra(int idTipoMuestra);
        List<Material> GetMaterialesByIdTipoMuestraIdPresentacion(int idTipoMuestra,int idPresentacion);
        List<Material> GetMaterialesByIdTipoMuestra(List<int> idTipoMuestraList);
        void InsertMaterial(Material material); 
        Material GetMaterialById(int idMaterial);
        void UpdateMaterial(Material material);
    }
}

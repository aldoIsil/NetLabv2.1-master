using Model;
using System.Collections.Generic;

namespace BusinessLayer.DatoClinico.Interfaces
{
    public interface ICategoriaDatoBl
    {
        List<CategoriaDato> GetCategoriaByEnfermedad(string idEnfermedad, int? idCategoriaPadre);
        void InsertCategoria(CategoriaDato categoria, string idEnfermedad);
        CategoriaDato GetCategoriaById(int id);
        EnfermedadCategoriaDato GetEnfermedadCategoriaById(int id, string idEnfermedad);
        void UpdateCategoria(CategoriaDato categoria);
        void UpdateCategoria(CategoriaDato categoria, EnfermedadCategoriaDato enfermedadCategoria);
    }
}

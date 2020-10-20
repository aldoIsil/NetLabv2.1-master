using System.Collections.Generic;
using Model;

namespace BusinessLayer.DatoClinico.Interfaces
{
    public interface IDatoClinicoBl
    {
        List<CategoriaDato> GetCategoriaByEnfermedad(int idEnfermedad, int idProyecto, string idExamen);
    }
}
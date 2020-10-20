using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface ICriterioRechazoBl
    {
        List<CriterioRechazo> GetCriterios(string nombre);


        List<CriterioRechazo> GetCriteriosActivos(string nombre);

        List<CriterioRechazo> GetCriteriosByTipoMuestra(int idTipoMuestra, int tipo);
        void InsertCriterio(CriterioRechazo criterioRechazo);
        CriterioRechazo GetCriterioById(int idCriterioRechazo);
        void UpdateCriterio(CriterioRechazo criterioRechazo);
    }
}
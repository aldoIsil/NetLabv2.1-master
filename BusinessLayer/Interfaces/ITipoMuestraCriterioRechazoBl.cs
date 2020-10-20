using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface ITipoMuestraCriterioRechazoBl
    {
        List<CriterioRechazo> GetCriteriosByTipoMuestraId(int idTipoMuestra);
        void AgregarCriteriosPorMuestra(TipoMuestra tipoMuestra, int[] criterios, int idUsuario);
        void UpdateCriterioByTipoMuestra(TipoMuestraCriterioRechazo muestraCriterioRechazo);
    }
}
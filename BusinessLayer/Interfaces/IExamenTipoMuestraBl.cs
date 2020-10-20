using System;
using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IExamenTipoMuestraBl
    {
        List<ExamenTipoMuestra> GetTipoMuestraByExamen(Guid idExamen);
        void InsertTipoMuestraByExamen(ExamenTipoMuestra examenTipoMuestra);
        void UpdateTipoMuestraByExamen(ExamenTipoMuestra examenTipoMuestra);
    }
}
using System.Collections.Generic;
using System;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface ITipoMuestraBl 
    {
        List<TipoMuestra> GetTiposMuestraByIdExamen(Guid idExamen);
        List<TipoMuestra> GetTiposMuestraByIdExamen(List<Guid> idExamen);        
        TipoMuestra GetTiposMuestraById(int idTipoMuestra);
        List<TipoMuestra> GetTipoMuestras(string nombre = null);
        List<TipoMuestra> GetTipoMuestrasActivas(string nombre = null);
        TipoMuestra GetTipoMuestraById(int idTipoMuestra);
        void InsertTipoMuestra(TipoMuestra tipoM);
        void UpdateTipoMuestra(TipoMuestra tipoM);
    }
}

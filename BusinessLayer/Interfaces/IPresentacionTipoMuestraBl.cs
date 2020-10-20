using System.Collections.Generic;
using Model;


namespace BusinessLayer.Interfaces
{
    public interface IPresentacionTipoMuestraBl
    {
        List<TipoMuestra> GetTipoMuestrasByPresentacionId(int idPresentacion);

        void AgregarTipoMuestrasPorPresentacion(Presentacion presentacion, int[] tipoMuestras, int idUsuario);

        void UpdateTipoMuestraByPresentacion(PresentacionTipoMuestra presentaTipoMuestra);

    }
}

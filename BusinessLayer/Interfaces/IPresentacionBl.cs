using Model;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IPresentacionBl
    {
        List<Presentacion> GetPresentaciones(string nombre);
        Presentacion GetPresentacionById(int idPresentacion);
        void InsertPresentacion(Presentacion presenta);
        void UpdatePresentacion(Presentacion presenta);
        List<Presentacion> GetPresentacionesByIdTipoMuestra(int? idTipoMuestra);
    }
}

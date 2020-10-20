using Model;
using System;
using Enums;

namespace BusinessLayer.Interfaces
{
    public interface IOrdenBl : IDisposable
    {

        Orden InsertOrden(Orden orden, TipoRegistroOrden tipo);

        bool UpdateOrden(Orden orden, TipoRegistroOrden tipo);
        void DeleteOrden(Guid idOrden);

        void InsertOrdenBancoSangre(Orden orden);
        void UpdateOrdenBancoSangre(Orden orden);

    }
}

using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IReactivoPresentacionBl
    {

        List<Presentacion> GetPresentacionesByReactivoId(int idReactivo);

        List<Presentacion> GetPresentacionesActivas(string nombre);


        void AgregarPresentacionesPorReactivo(Reactivo reactivo, int[] presentaciones, int idUsuario);

        void UpdatePresentacionByReactivo(PresentacionReactivo presentacionReactivo);

    }
}

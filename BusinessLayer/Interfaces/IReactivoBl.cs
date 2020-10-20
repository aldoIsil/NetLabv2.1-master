using Model;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IReactivoBl
    {
        
        List<Reactivo> GetReactivos(string nombre);

        Reactivo GetReactivoById(int idReactivo);

        void InsertReactivo(Reactivo reactivo);

        void UpdateReactivo(Reactivo reactivo);

        List<Reactivo> GetReactivosByIdPresentacion(int? idTPresentacion);

    }
}
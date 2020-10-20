using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IAreaProcesamientoBl
    {
        List<Model.AreaProcesamiento> GetAreasProcesamiento(string nombre);
        void InsertAreaProcesamiento(Model.AreaProcesamiento areaProcesamiento);
        void UpdateAreaProcesamiento(Model.AreaProcesamiento areaProcesamiento);
        Model.AreaProcesamiento GetAreaProcesamientoById(int id);
    }
}
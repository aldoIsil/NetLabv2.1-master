using Model;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IAreaExamenBl
    {
        List<Examen> GetExamenesByArea(int idAreaProcesamiento);
        void InsertExamenesByArea(Model.AreaProcesamiento area, string[] examenes, int idUsuario);
        void UpdateExamenByArea(AreaProcesamientoExamen areaExamen);
    }
}

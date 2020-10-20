using System;
using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface ILaboratorioExamenBl
    {
        void InsertExamenByLaboratorio(ExamenLaboratorio examenLaboratorio);
        void UpdateExamenByLaboratorio(ExamenLaboratorio examenLaboratorio);
        List<ExamenLaboratorio> GetExamenesByLaboratorio(int idLaboratorio);
        ExamenLaboratorio GetExamenLaboratorioById(Guid idExamen, int idLaboratorio);
    }
}
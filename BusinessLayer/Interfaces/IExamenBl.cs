using System.Collections.Generic;
using System;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IExamenBl
    {
        List<Examen> GetExamenesByNombre(string nombre);
        Examen GetExamenById(Guid idExamen);
        void InsertExamen(Examen examen);
        void UpdateExamen(Examen examen);
        List<Examen> GetExamenesByIdEnfermedad(int idEnfermedad, int genero, String data);
     //   List<Examen> GetExamenesByIdLaboratorio(int idLaboratorio, int genero);
        List<Examen> GetExamenesByIdLaboratorio(int idEnfermedad, int genero, string data);
        List<Examen> GetExamenesByGenero(int genero, String data);
        List<ExamenMetodo> GetMetodoByExamen(Guid idExamen);
        void InsertMetodoByExamen(ExamenMetodo examenMetodo);
        void UpdateMetodoByExamen(ExamenMetodo examenMetodo);

        List<Examen> GetExamenesByIdEnfermedadOrden(int idEnfermedad, String data);
        List<Examen> GetExamenesByIdLaboratorioRecepcion(int idLab, int genero, string data);
        List<Examen> GetExamenesPorEnfermedad(int idEnfermedad);
    }
}

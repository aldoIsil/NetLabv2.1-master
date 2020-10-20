using System;
using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IEnfermedadExamenBl
    {
        void AgregarEnfermedadesPorExamen(Examen examen, int[] enfermedades, int idUsuario);
        List<Enfermedad> GetEnfermedadByExamenId(Guid idExamen);
        void InsertEnfermedadByExamen(List<EnfermedadExamen> enfermedades);
        void UpdateEnfermedadByExamen(EnfermedadExamen enfermedadExamen);
    }
}

using System;
using System.Collections.Generic;
using Model;

namespace BusinessLayer.Interfaces
{
    public interface IAnalitoExamenBl
    {
        List<Analito> GetAnalitoByExamenId(Guid idExamen);
        void UpdateAnalitoByExamen(ExamenAnalito examenAnalito);
        void AgregarAnalitosPorExamen(Examen examen, string[] analitos, int idUsuario);
    }
}
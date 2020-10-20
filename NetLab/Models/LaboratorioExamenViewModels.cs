using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class LaboratorioExamenViewModels
    {
        public Laboratorio Laboratorio { get; set; }
        public List<ExamenLaboratorio> Examenes { get; set; }
    }
}
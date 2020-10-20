using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class EnfermedadExamenViewModels
    {
        public Examen Examen { get; set; }
        public List<Enfermedad> Enfermedades { get; set; }
    }
}
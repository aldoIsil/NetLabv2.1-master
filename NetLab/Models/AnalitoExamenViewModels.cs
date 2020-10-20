using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class AnalitoExamenViewModels
    {
        public Examen Examen { get; set; }
        public List<Analito> Analitos { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class OpcionesAnalitoViewModels
    {
        public Analito Analito { get; set; }
        public List<AnalitoOpcionResultado> Opciones { get; set; }
    }
}
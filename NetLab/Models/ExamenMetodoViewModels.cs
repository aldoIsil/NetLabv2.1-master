using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class ExamenMetodoViewModels
    {
        public Examen Examen { get; set; }
        public List<ExamenMetodo> Metodos { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class MetodosAnalitoViewModels
    {
        public Analito Analito { get; set; }
        public List<ExamenMetodo> Metodos { get; set; }
    }
}
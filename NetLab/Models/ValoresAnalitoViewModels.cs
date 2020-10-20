using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class ValoresAnalitoViewModels
    {
        public Analito Analito { get; set; }
        public List<AnalitoValorNormal> Valores { get; set; }
    }
}
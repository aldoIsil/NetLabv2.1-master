using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class ExamenTipoMuestraViewModels
    {
        public Examen Examen { get; set; }
        public List<ExamenTipoMuestra> TiposDeMuestra { get; set; } 
    }
}
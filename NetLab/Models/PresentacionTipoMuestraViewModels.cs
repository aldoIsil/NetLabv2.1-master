using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class PresentacionTipoMuestraViewModels
    {
        public Presentacion Presentacion { get; set; }

        public List<Model.TipoMuestra> Tipomuestras { get; set; }

    }
}
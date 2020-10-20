using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models
{
    [Serializable]
    public class MuestraCriterioRechazoViewModels
    {
        public TipoMuestra TipoMuestra { get; set; }
        public List<Model.CriterioRechazo> Criterios { get; set; }
    }
}
using NetLab.Models.Shared;
using System;

namespace NetLab.Models.CriterioRechazo
{
    [Serializable]
    public class TipoCriterioViewModels : ListaDetalleViewModels
    {
        public int IdTipoCriterio { get; set; }
    }
}
using System;

namespace Model
{
    [Serializable]
    public class TipoMuestraCriterioRechazo : General
    {
        public int IdTipoMuestra { get; set; }
        public int IdCriterioRechazo { get; set; }
    }
}
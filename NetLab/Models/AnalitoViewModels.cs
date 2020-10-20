using Model;
using NetLab.Models.Shared;
using System;

namespace NetLab.Models
{
    [Serializable]
    public class AnalitoViewModels
    {
        public Analito Analito { get; set; }
        public ListaDetalleViewModels TiposRespuesta { get; set; }
        public ListaDetalleViewModels Unidades { get; set; }
    }
}
using Model;
using NetLab.Models.Shared;
using System;

namespace NetLab.Models
{
    [Serializable]
    public class AnalitoOpcionViewModels
    {
        public AnalitoOpcionResultado Opcion { get; set; }
        public ListaDetalleViewModels TiposRespuesta { get; set; }
    }
}
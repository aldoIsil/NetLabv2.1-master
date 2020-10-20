using NetLab.Models.Shared;
using System;

namespace NetLab.Models.Animal
{
    [Serializable]
    public class SexoViewModels : ListaDetalleViewModels
    {
        public int IdSexo { get; set; }
    }
}
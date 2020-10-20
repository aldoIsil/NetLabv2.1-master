
using System;

namespace NetLab.Models.DatoClinico
{
    [Serializable]
    public class CategoriaEditViewModels
    {
        public bool IsEdit { get; set; }
        public string CodigoEnfermedad { get; set; }
        public string NombreEnfermedad { get; set; }
    }
}
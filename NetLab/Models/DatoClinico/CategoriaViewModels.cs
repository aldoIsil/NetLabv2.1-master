using Model;
using NetLab.Models.Shared;
using System;

namespace NetLab.Models.DatoClinico
{
    [Serializable]
    public class CategoriaViewModels
    {
        public string IdEnfermedad { get; set; }
        public int? IdCategoriaPadre { get; set; }
        public string NombrePadre { get; set; }
        public CategoriaDato Categoria { get; set; }
        public ClaseGeneroViewModels Clase { get; set; }
    }
}
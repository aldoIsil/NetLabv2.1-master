using System;
using System.Collections.Generic;
using Model;

namespace NetLab.Models.DatoClinico
{
    [Serializable]
    public class CategoriaListaViewModels
    {
        public string IdEnfermedad { get; set; }
        public string NombreEnfermedad { get; set; }
        public int? IdCategoriaPadre { get; set; }
        public string NombrePadre { get; set; }
        public string FromUrl { get; set; }
        public List<CategoriaDato> Categoria { get; set; }
    }
}
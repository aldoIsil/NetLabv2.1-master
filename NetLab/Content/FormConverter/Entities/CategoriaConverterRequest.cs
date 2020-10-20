using NetLab.Models.DatoClinico;
using System;

namespace NetLab.Controllers.FormConverter.Entities
{
    [Serializable]
    public class CategoriaConverterRequest
    {
        public CategoriaViewModels CategoriaViewModels { get; set; }
        public int IdUsuarioLogueado { get; set; }
    }
}
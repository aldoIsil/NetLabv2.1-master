using NetLab.Models.Animal;
using System;

namespace NetLab.Controllers.FormConverter.Entities
{
    [Serializable]
    public class AnimalConverterRequest
    {
        public AnimalViewModels AnimalViewModels { get; set; }
        public int IdUsuarioLogueado { get; set; }
    }
}
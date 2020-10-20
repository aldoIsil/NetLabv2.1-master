using NetLab.Models.DatoClinico;
using System;

namespace NetLab.Controllers.FormConverter.Entities
{
    [Serializable]
    public class DatoConverterRequest
    {
        public DatoViewModels DatoViewModels { get; set; }
        public int IdUsuarioLogueado { get; set; }
    }
}
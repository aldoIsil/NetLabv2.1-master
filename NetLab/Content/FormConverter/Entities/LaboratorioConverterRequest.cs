using Model;
using NetLab.Models;
using System;

namespace NetLab.Controllers.FormConverter.Entities
{
    [Serializable]
    public class LaboratorioConverterRequest
    {
        public Laboratorio Laboratorio { get; set; }
        public int IdUsuarioLogueado { get; set; }
        public LaboratorioViewModels LaboratorioModel { get; set; }
    }
}
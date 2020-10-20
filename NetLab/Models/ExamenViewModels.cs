using Model;
using NetLab.Models.Shared;
using System;

namespace NetLab.Models
{
    [Serializable]
    public class ExamenViewModels
    {
        public Examen Examen { get; set; }
        public ClaseGeneroViewModels Clase { get; set; } 
    }
}
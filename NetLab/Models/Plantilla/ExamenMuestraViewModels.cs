using Model;
using System;

namespace NetLab.Models.Plantilla
{
    [Serializable]
    public class ExamenMuestraViewModels
    {
        public Enfermedad Enfermedad { get; set; }
        public Examen Examen { get; set; } 
        public TipoMuestra Muestra { get; set; }
        public Material Material { get; set; }
        public int? Cantidad { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class ExamenLaboratorio : General
    {
        public int IdLaboratorio { get; set; }

        [Display(Name = "Examen")]
        [Required(ErrorMessage = "Debe seleccionar el examen")]
        public Guid IdExamen { get; set; }

        public Examen Examen { get; set; }

        [Display(Name = "Tiempo para Emisión de Resultado (en días)")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor entero válido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Debe ingresar solo valores numéricos")]
        [Required(ErrorMessage = "Debe ingresar los dias de emisión")]
        public int? DiasEmision { get; set; }

        [Display(Name = "Tiempo para Entrega de Resultado (en días)")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor entero válido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Debe ingresar solo valores numéricos")]
        [Required(ErrorMessage = "Debe ingresar los dias de entrega")]
        public int? DiasEntrega { get; set; }
    }
}
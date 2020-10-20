using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class OpcionDato : General
    {

        [Required(ErrorMessage = "Debe ingresar el id")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor entero válido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Debe ingresar solo valores numéricos")]
        public int IdOpcionDato { get; set; }

        public int IdListaDato { get; set; }

        [Required(ErrorMessage = "Debe ingresar un valor")]
        public string Valor { get; set; }

        [Required(ErrorMessage = "Debe ingresar el orden")]
        [Range(0, int.MaxValue, ErrorMessage = "Ingrese un valor entero válido")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Debe ingresar solo valores numéricos")]
        public int Orden { get; set; }
    }
}

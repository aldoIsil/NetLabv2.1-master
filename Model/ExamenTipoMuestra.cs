using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class ExamenTipoMuestra : General
    {
        public Guid IdExamen { get; set; }

        [Display(Name = "Tipo de Muestra")]
        [Required(ErrorMessage = "Debe seleccionar el tipo de muestra")]
        public int IdTipoMuestra { get; set; }

        public TipoMuestra TipoMuestra { get; set; }

        [StringLength(2000, ErrorMessage = "Debe tener máximo 2000 letras")]
        [Required(ErrorMessage = "Debe ingresar la característica")]
        public string Caracteristica { get; set; }

        [StringLength(1000, ErrorMessage = "Debe tener máximo 1000 letras")]
        [Required(ErrorMessage = "Debe seleccionar el transporte")]
        public string Transporte { get; set; }
    }
}

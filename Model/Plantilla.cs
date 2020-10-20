using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class Plantilla : General
    {
        public int IdPlantilla { get; set; }
        public int IdTipo { get; set; }
        public string Tipo { get; set; }

        [StringLength(200, ErrorMessage = "Debe tener máximo 200 letras")]
        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public string  idOrden { get; set; }
    }
}

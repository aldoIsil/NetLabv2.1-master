using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class AreaProcesamiento:General
    {
        public int IdAreaProcesamiento { get; set; }
        
        [StringLength(500, ErrorMessage = "Debe tener máximo 500 letras")]
        [Required(ErrorMessage = "Ingrese el nombre del área de procesamiento")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(1000, ErrorMessage = "Debe tener máximo 1000 letras")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Ingrese descripciòn del área de procesamiento")]
        public string Descripcion { get; set; }

        [StringLength(5, ErrorMessage = "Debe tener máximo 5 letras")]
        public string Sigla { get; set; } 
    }
}
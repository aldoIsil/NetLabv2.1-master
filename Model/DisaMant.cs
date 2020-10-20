using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class DisaMant : General
    {

        [Display(Name = "IdDISA")]
        [Required(ErrorMessage = "Se requiere el ID")]
        public string IdDISA { get; set; }

        [Display(Name = "NombreDISA")]
        [Required(ErrorMessage = "Se requiere el nombre")]
        public string NombreDISA { get; set; }

        public int Estado { get; set; }
    }
}

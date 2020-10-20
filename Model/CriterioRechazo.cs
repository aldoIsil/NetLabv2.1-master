using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class CriterioRechazo : General
    {
        public Guid IdOrdenMuestraRecepcion { get; set; }

        public int IdCriterioRechazo { get; set; }

        [Display(Name = "Nombre")]
        [StringLength(200, ErrorMessage = "Debe tener máximo 200 letras")]
        [Required(ErrorMessage = "Ingrese una glosa")]
        public string Glosa { get; set; }

        public int Tipo { get; set; }

        [Display(Name = "Tipo")]
        public string DescripcionTipo { get; set; }


        public bool rechazado { get; set;  }
        public string observacionrechazo { get; set; }
    }
}

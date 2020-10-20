using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class ExamenMetodo : General
    { 
        public Guid IdExamen { get; set; }
        public int IdExamenMetodo { get; set; }
        [Required(ErrorMessage = "Se requiere la glosa")]
        public string Glosa { get; set; }
        public int Orden { get; set; }


        // ESTOS CAMPOS NO SON DEL EXAMENMETODO, PERO SE USAN COMO AUXILIARS, DEJAR ESTOS CAMPOS POR EL MOMENTO
        public Guid IdAnalito { get; set; }
        public int IdAnalitoMetodo { get; set; }
        public string Glosa1 { get; set; }
        public string Glosa2 { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class Ubigeo : General
    {
        public string Id { get; set; }

        public string Descripcion { get; set; }

      //  [Required(ErrorMessage = "Departamento es requerido")]
        [Display(Name = "Departamento:")]
        public string Departamento { get; set; }

        //  [Required(ErrorMessage = "Provincia es requerido")]
        [Display(Name = "Provincia:")]
        public string Provincia { get; set; }

        //  [Required(ErrorMessage = "Distrito es requerido")]
        [Display(Name = "Distrito:")]
        public string Distrito { get; set; }
        

    }
}

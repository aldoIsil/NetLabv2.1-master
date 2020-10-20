using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class AnimalRaza : General
    {
        public int IdRaza { get; set; }

        [Display(Name = "Nombre:")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción:")]
        public string Descripcion { get; set; }

        [Display(Name = "Especie:")]
        public AnimalEspecie Especie { get; set; }
    }
}

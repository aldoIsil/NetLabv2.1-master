using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Model.ViewData;


namespace Model
{
    [Serializable]
    public class Rol:General
    {
        public int idRol {get;set;}//] [int] NOT NULL,

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Ingrese un nombre")]
        public string nombre {get;set;}//] [varchar](500) NULL,

        [Display(Name = "Descripción")]
        public string descripcion {get;set;}//] [varchar](1000) NULL,
        public string tipo { get; set; }
    }
}

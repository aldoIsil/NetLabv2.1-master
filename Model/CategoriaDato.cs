
using System;
using System.Collections.Generic;
﻿using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class CategoriaDato : General
    {
       	public int IdCategoriaDato { get; set; }

        [StringLength(200, ErrorMessage = "Debe tener máximo 200 letras")]
        [Required(ErrorMessage = "Ingrese el nombre de la categoría")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(200, ErrorMessage = "Debe tener máximo 200 letras")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        public int? IdCategoriaDatoPadre { get; set; }

        public int? IdGenero { get; set; }

        public bool Visible { get; set; }

        [Display(Name = "Orientación")]
        //JoseChavez-Noviembre
        [Range(1,int.MaxValue, ErrorMessage ="Ingresar un valor mayor a 0"), Required(ErrorMessage ="Ingrese un valor mayor a 0")]
        public int Orientacion { get; set; }

        public List<OrdenDatoClinico> OrdenDatoClinicoList { get; set; } 

        public List<CategoriaDato> SubCategorias { get; set; }
        public List<Dato> Datos { get; set; } 
    }
}

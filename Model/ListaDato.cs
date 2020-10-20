using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class ListaDato : General
    {
        public int IdListaDato { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        public List<OpcionDato> Opciones { get; set; }
    }
}

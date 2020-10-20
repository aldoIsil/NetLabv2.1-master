using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class Enfermedad : General
    {
        public int idEnfermedad { get; set; }

        [Display(Name = "Enfermedad")]
        public string nombre { get; set; }

        public string Snomed { get; set; }

        public List<CategoriaDato> categoriaDatoList { get; set; }



    }
}

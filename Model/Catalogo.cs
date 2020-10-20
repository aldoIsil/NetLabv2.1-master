using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Catalogo
    {
        public int idEnfermedad { get; set; }

        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        public string Snomed { get; set; }

        public int estado { get; set; }

        public List<CategoriaDato> categoriaDatoList { get; set; }



    }
}

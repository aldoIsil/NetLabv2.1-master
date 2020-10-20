using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class LaboratorioVMSelect 
    {
        public int IdEstablecimiento { get; set; }
        public string CodigoUnico { get; set; }
        public string Nombre { get; set; }//nombre + departamnto + provincia + distrito
        public string NombreEstablecimiento { get; set; }
        public string Ubigeo { get; set; }
        public string Direccion { get; set; }
        public string clasificacion { get; set; }
        public int? IdLabIns { get; set; }
    }

    [Serializable]
    public class Laboratorio : General
    {
        public int IdLaboratorio { get; set; }
        public int CodigoInstitucion { get; set; }

        [Display(Name = "Código Unico")]
        [StringLength(8, ErrorMessage = "Debe tener máximo 8 caráteres")] //mms 17-01-2017
        public string CodigoUnico { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [StringLength(500, ErrorMessage = "Debe tener máximo 500 letras")]
        public string Nombre { get; set; }

        public string Ubigeo { get; set; }

        public Ubigeo UbigeoLaboratorio { get; set; } //mms 19-01-2017

        [Required(ErrorMessage = "Debe ingresar la dirección")]
        [StringLength(500, ErrorMessage = "Debe tener máximo 500 letras")]
        [Display(Name = "Dirección")]
        [DataType(DataType.MultilineText)]
        public string Direccion { get; set; }

        public string IdDisa { get; set; }
        public string IdRed { get; set; }
        public string IdMicroRed { get; set; }
        public int IdCategoria { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public byte[] LogoRegional { get; set; } //MMS 19-01-2017
        public byte[] Logo { get; set; }
        public byte[] Sello { get; set; }

        //[StringLength(200, ErrorMessage = "Debe tener máximo 200 letras")]
        //[Url(ErrorMessage = "Debe ingresar una url válida")]
        public string Website { get; set; }

        public string clasificacion { get; set; }

        public string correoElectronico { get; set; }

        public int tipo { get; set; }


        public string nombreInstitucion { get; set; }

        public string nombreDisa { get; set; }

        public string nombreRed { get; set; }

        public string nombreMicroRED { get; set; }
        public int IdLabIns { get; set; }
    }
}

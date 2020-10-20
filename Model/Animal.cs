using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class Animal : General
    {
        public Guid IdAnimal { get; set; }

        [Display(Name = "Nombre:")]
        [StringLength(200)]
        public string Nombre { get; set; }

        public int Genero { get; set; }

        public string Sexo { get; set; }

        [Display(Name = "Raza:")]
        public AnimalRaza Raza { get; set; }

        public int Origen { get; set; }

        public long Codificacion { get; set; }

        [Display(Name = "Fecha de Nacimiento:")]
        [Required(ErrorMessage = "Debe ingresar la fecha de nacimiento")]
        [DataType(DataType.Date, ErrorMessage = "Debe ingresar un valor de tipo fecha")]
        public DateTime? FechaNacimiento { get; set; }

        [Range(0,99, ErrorMessage = "El rango válido es de 0 a 99 años")]
        [Display(Name = "Edad en Años:")]
        public int? EdadAnio { get; set; }

        [Range(0,11, ErrorMessage = "El rango válido es de 0 a 11 meses")]
        [Display(Name = "Edad en Meses:")]
        public int? EdadMes { get; set; }

        public string IdUbigeo { get; set; }

        [Display(Name = "Dirección:")]
        [StringLength(2000)]
        [Required(ErrorMessage = "Ingrese la ubicación del animal")]
        public string Direccion { get; set; }

        [Display(Name = "Nombre del Refugio:")]
        [StringLength(1000)]
        public string Refugio { get; set; }

        public Paciente Propietario { get; set; }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(Propietario.ApellidoPaterno) ||
                    string.IsNullOrEmpty(Propietario.ApellidoMaterno) ||
                    string.IsNullOrEmpty(Propietario.Nombres))
                {
                    return string.Empty;
                }

                return $"{Propietario.ApellidoPaterno} {Propietario.ApellidoMaterno}, {Propietario.Nombres}";
            }
        }
    }
}

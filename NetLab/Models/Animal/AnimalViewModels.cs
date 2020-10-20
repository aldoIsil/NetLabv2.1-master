using NetLab.Models.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace NetLab.Models.Animal
{
    [Serializable]
    public class AnimalViewModels
    {
        public Model.Animal Animal { get; set; }

        [Display(Name = "Sexo:")]
        public SexoViewModels Sexo { get; set; }

        [Display(Name = "Origen:")]
        public OrigenViewModels Origen { get; set; }

        public EspecieViewModels Especie { get; set; }

        public RazaViewModels Raza { get; set; }

        [Display(Name = "Departamento:")]
        public string IdDepartamento { get; set; }

        [Display(Name = "Provincia:")]
        public string IdProvincia { get; set; }

        [Display(Name = "Distrito:")]
        public string IdDistrito { get; set; }

        public string DepartamentoProp { get; set; }

        public string ProvinciaProp { get; set; }

        public string DistritoProp { get; set; }

        [Display(Name = "Genero:")]
        public GeneroViewModels Genero { get; set; }

        [Display(Name = "Tipo de Documento:")]
        public TipoDocumentoViewModels TipoDocumento { get; set; }

        public bool NoPrecisaFecha => !Animal.FechaNacimiento.HasValue &&
                                      !Animal.EdadAnio.HasValue &&
                                      !Animal.EdadMes.HasValue;

        public bool NoPrecisaUbigeoAnimal => string.IsNullOrEmpty(IdDepartamento) &&
                                             string.IsNullOrEmpty(IdProvincia) &&
                                             string.IsNullOrEmpty(IdDistrito) &&
                                             string.IsNullOrEmpty(Animal.Direccion);

        public bool NoPrecisaUbigeoPropietario => Animal.Propietario?.TipoDocumento == null &&
                                                  Animal.Propietario?.Genero == null &&
                                                  string.IsNullOrEmpty(Animal.Propietario?.ApellidoPaterno) &&
                                                  string.IsNullOrEmpty(Animal.Propietario?.ApellidoMaterno) &&
                                                  string.IsNullOrEmpty(Animal.Propietario?.Nombres) &&
                                                  string.IsNullOrEmpty(Animal.Propietario?.UbigeoActual.Id) &&
                                                  string.IsNullOrEmpty(Animal.Propietario?.DireccionActual) &&
                                                  string.IsNullOrEmpty(Animal.Propietario?.TelefonoFijo) &&
                                                  string.IsNullOrEmpty(Animal.Propietario?.Celular1);
    }
}
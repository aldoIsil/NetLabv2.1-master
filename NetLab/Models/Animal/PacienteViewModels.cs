using System;
using System.ComponentModel.DataAnnotations;
using NetLab.Models.Shared;

namespace NetLab.Models.Animal
{
    [Serializable]
    public class PacienteViewModels
    {
        public Model.Paciente Paciente { get; set; }

        [Display(Name = "Tipo de Documento:")]
        public TipoDocumentoViewModels TipoDocumento { get; set; }

        [Display(Name = "Genero:")]
        public GeneroViewModels Genero { get; set; }

        [Display(Name = "Departamento Reniec:")]
        public DepartamentoViewModels Departamento { get; set; }

        [Display(Name = "Provincia Reniec:")]
        public ProvinciaViewModels Provincia { get; set; }

        [Display(Name = "Distrito Reniec:")]
        public DistritoViewModels Distrito { get; set; }

        [Display(Name = "Departamento Actual:")]
        public DepartamentoViewModels DepartamentoActual { get; set; }

        [Display(Name = "Provincia Actual:")]
        public ProvinciaViewModels ProvinciaActual { get; set; }

        [Display(Name = "Distrito Actual:")]
        public DistritoViewModels DistritoActual { get; set; }
    }
}
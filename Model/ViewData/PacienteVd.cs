using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    public class PacienteVd: Paciente
    {
        [Display(Name = "Departamento Reniec:")]
        public string DepartamentoReniecId { get; set; }

        [Display(Name = "Provincia Reniec:")]
        public string ProvinciaReniecId { get; set; }

        [Display(Name = "Distrito Reniec:")]
        public string DistritoReniecId { get; set; }

        [Display(Name = "Departamento Actual:")]
        public string DepartamentoActualId { get; set; }

        [Display(Name = "Provincia Actual:")]
        public string ProvinciaActualId { get; set; }

        [Display(Name = "Distrito Actual:")]
        public string DistritoActualId { get; set; }

        public string DepartamentoReniec { get; set; }

        public string ProvinciaReniec { get; set; }

        public string DistritoReniec { get; set; }

        public string DepartamentoActual { get; set; }

        public string ProvinciaActual { get; set; }

        public string DistritoActual { get; set; }

    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Serializable]
    public class DISA : General
    {
        public string IdDISA { get; set; }

        [Display(Name = "Disa:")]
        public string NombreDISA { get; set; }
        [NotMapped]
        public int IdInstitucion { get; set; }
    }

    [Serializable]
    public class Red : General
    {
        public string IdDISA { get; set; }
        public string IdRed { get; set; }

        [Display(Name = "Red:")]
        public string NombreRed { get; set; }
        //JRCR-REPORTE01
        [NotMapped]
        public int IdInstitucion { get; set; }
    }

    [Serializable]
    public class MantMicroRed : General
    {
        public string IdDISA { get; set; }
        public string IdRed { get; set; }
        public string IdMicroRed { get; set; }

        [Display(Name = "Disa:")]
        public string NombreMicroRed { get; set; }
    }
}

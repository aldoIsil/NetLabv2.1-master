using Reniec.Entities.Enums;
using System;
using Model.Enums;

namespace Reniec.Entities
{
    public class Persona
    {
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Genero Genero { get; set; }
        public DonacionOrgano DonacionOrgano { get; set; }
        public Direccion Direccion { get; set; }
    }
}

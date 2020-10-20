using System;

namespace NetLab.Models.Login
{
    [Serializable]
    public class LocalViewModel
    {
        public int IdInstitucion { get; set; }

        public string IdDisa { get; set; }

        public string IdRed { get; set; }

        public string IdMicroRed { get; set; }

        public string IdEstablecimiento { get; set; }
    }
}
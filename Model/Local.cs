using System;
using System.Collections.Generic;
using Model.Enums;

namespace Model
{
    [Serializable]
    public class Local : General
    {
        public int IdUsuario { get; set; }

        public int IdInstitucion { get; set; }

        public string IdDisa { get; set; }

        public string IdRed { get; set; }

        public string IdMicroRed { set; get; }

        public int IdEstablecimiento { get; set; }

        public TipoLocal TipoLocal { get; set; }
    }
}

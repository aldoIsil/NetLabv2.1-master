using System;

namespace Model
{
    [Serializable]
    public class UsuarioEstablecimiento:General
    {
        public int ? idUsuario {get;set;}
        public string idDISA { get; set; }
        public string  idRed { get; set; }
        public string  idMicrored { get; set; }
        public int ?  idEstablecimiento { get; set; }
        public string nomDisa { get; set; }
        public string nomRed { get; set; }
        public string nomMicrored { get; set; }
        public string nomEstablecimiento { get; set; }
        public string nomInstitucion { get; set; }
        public int? idInstitucion { get; set; }
    }
}

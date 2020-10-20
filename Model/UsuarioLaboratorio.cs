using System;

namespace Model
{
    [Serializable]
    public class UsuarioLaboratorio:General
    {
        public int ? idUsuario {get;set;}
        public int ? idDISA { get; set; }
        public int ?  idRed { get; set; }
        public int ?  idMicrored { get; set; }
        public int ? idLaboratorio { get; set; }
        public string nomDisa { get; set; }
        public string nomRed { get; set; }
        public string nomMicrored { get; set; }
        public string nomLaboratorio { get; set; }
        public string nomInstitucion { get; set; }
        public int? idInstitucion { get; set; }
    }
}

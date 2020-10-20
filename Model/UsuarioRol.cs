using System;

namespace Model
{
    [Serializable]
    public class UsuarioRol:General
    {
        public int idUsuario {get;set;}//] [int] NOT NULL,
        public int idRol { get; set; }//] [int] NOT NULL,
    }
}

using System;

namespace Model
{
    [Serializable]
    public class UsuarioAreaProcesamiento:General
    {
        public int idUsuario {get;set;}//] [int] NOT NULL,
        public int idAreaProcesamiento { get; set; }//] [int] NOT NULL,
    }
}

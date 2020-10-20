using System;

namespace Model
{
    [Serializable]
    public class UsuarioResultado
    {
        public string Nombre { get; set; } 
        public string Cargo { get; set; }
        public string Colegio { get; set; }
        public byte[] FirmaDigital { get; set; }
    }
}
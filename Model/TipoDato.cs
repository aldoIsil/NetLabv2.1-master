
using System;

namespace Model
{
    [Serializable]
    public class TipoDato : General
    {
        public int IdTipo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}

using System;

namespace Model
{
    [Serializable]
    public class DatoCategoriaDato : General
    {
        public int IdDato { get; set; }
        public int IdCategoriaDato { get; set; }
        public int Orden { get; set; }
    }
}

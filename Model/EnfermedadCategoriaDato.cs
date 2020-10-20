using System;

namespace Model
{
    [Serializable]
    public class EnfermedadCategoriaDato : General
    {
        public int IdEnfermedad { get; set; }
        public int IdCategoriaDato { get; set; }
        public int Orden { get; set; }
    }
}
using System;

namespace Model
{
    [Serializable]
    public class EnfermedadExamen : General
    {
        public int IdEnfermedad { get; set; }
        public Guid IdExamen { get; set; }
        public int Orden { get; set; }
    }
}

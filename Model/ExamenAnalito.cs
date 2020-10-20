using System;

namespace Model
{
    [Serializable]
    public class ExamenAnalito : General
    {
        public Guid IdExamen { get; set; }
        public Guid IdAnalito { get; set; }
        public int OrdenAnalito { get; set; }
    }
}

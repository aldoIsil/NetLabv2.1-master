using System;

namespace Model
{
    [Serializable]
    public class AreaProcesamientoExamen : General
    {
        public int IdAreaProcesamiento { get; set; }
        public Guid IdExamen { get; set; }
    }
}

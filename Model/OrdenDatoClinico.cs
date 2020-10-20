using System;

namespace Model
{
    [Serializable]
    public class OrdenDatoClinico:General
    {
        public Guid idOrdenDatoClinico { get; set; }
        public Dato Dato { get; set; }
        public String ValorString { get; set; }
        public String OrdenGrabado { get; set; }
        public Enfermedad Enfermedad { get; set; }
        public int ordenDatoClinicoEstado { get; set; }
        public Guid idOrden { get; set; }
        public int estatus { get; set; }
        public int idCategoriaDato { get; set; }
        public bool noPrecisa { get; set; }
        public bool esObligatorio { get; set; }
        public int idProyecto { get; set; }
    }
}

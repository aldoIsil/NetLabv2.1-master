using System;
using System.Collections.Generic;

namespace Model
{
    [Serializable]
    public class SolicitudExamen
    {
        public Paciente Paciente { get; set; }
        public Establecimiento Establecimiento { get; set; }
        public OrdenSolicitante Solicitante { get; set; }
        public int Criterio { get; set; }
        public string CodigoOrden { get; set; }
        public ExamenResultadoDetalle Resultado { get; set; }
        public string pResultado { get; set; }
        public string pFechaResultado { get; set; }
        public string ResultadoCD4 { get; set; }
        public string FechaResultadoCD4 { get; set; }
        public string CodigoCD4 { get; set; }
        public List<DrogaGeno> ListaDrogas { get; set; }
        public DrogaGeno drogas { get; set; }
        public int numeroSolicitud { get; set; }
        public int tipoSolicitud { get; set; }
        public string fechaSolicitud { get; set; }
    }

    [Serializable]
    public class DrogaGeno
    {
        public int idDroga { get; set; }
        public string nombreDroga { get; set; }
        public int estado { get; set; }
        public int idGrupo { get; set; }
        public string valor { get; set; }
    }
}
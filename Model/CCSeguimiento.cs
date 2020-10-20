using System;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Serializable]
    public class CCSeguimientoCab
    {
        public int idseguimiento { get; set; }
        public int idEstablecimiento { get; set; }
        public string NombreEstablecimiento { get; set; }
        public int idEnfermedad { get; set; }
        public string NombreEnfermedad { get; set; }
        public Guid idExamen { get; set; }
        public string NombreExamen { get; set; }
        public int ejecutadx { get; set; }
        public int ejecutacc { get; set; }
        public int cumpliocc { get; set; }
        public int mca_inh { get; set; }
        public int idusuarioregistro { get; set; }
        public DateTime fecharegistro { get; set; }
        public int idusuarioedicion { get; set; }
        public DateTime fechaedicion { get; set; }
        public string NombreUsuario { get; set; }
    }
    [Serializable]
    public class CCSEguimientoDetalle
    {
        public Guid idSeguimientoDet { get; set; }
        public Guid idseguimiento { get; set; }
        public int tipoinfraestructura { get; set; }
        public int estadoequipo { get; set; }
        public int personal { get; set; }
        public string otros { get; set; }
        public int estadomaterial { get; set; }
        public int tipopofesional { get; set; }
        public int nroprofesional { get; set; }
        public int mca_inh { get; set; }
        public int idusuarioregistro { get; set; }
        public DateTime fecharegistro { get; set; }
        public int idusuarioedicion { get; set; }
        public DateTime fechaedicion { get; set; }
    }
}

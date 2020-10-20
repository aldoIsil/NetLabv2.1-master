using System;
using System.Collections.Generic;

namespace Model.Temp
{
    public class TempModel
    {
        [Serializable]
        public class TempOrden
        {
            public Guid IdOrden { get; set; }
            public DateTime FechaRegistro { get; set; }
            public int IdEstablecimiento { get; set; }
            public string EstablecimientoCodigoUnico { get; set; }
            public int IdProyecto { get; set; }
            public Guid IdPaciente { get; set; }
            public string CodigoOrden { get; set; }
            public string Solicitante { get; set; }
            public string observacion { get; set; }
            public DateTime fechaSolicitud { get; set; }
            public int estatus { get; set; }
            public int estado { get; set; }
            public int tipoO { get; set; }
            public int IdEstablecimientoEnvio { get; set; }
            public DateTime FechaRecepcionRomINS { get; set; }
            public DateTime FechaReevaluacionFicha { get; set; }
            public int EstadoEdicion { get; set; }
            public int IdLaboratorioDestino { get; set; }
            public int idUsuario { get; set; }
            public List<Enfermedad> enfermedadList { get; set; }
            public List<TempOrdenExamen> OrdenExamenList { get; set; }
            public List<TempOrdenMuestra> OrdenMuestraList { get; set; }
            public List<OrdenDatoClinico> OrdenDatoClinicoList { get; set; }
            public string nroCelularPaciente { get; set; }
            public TempOrden()
            {
                IdOrden = Guid.NewGuid();
                OrdenExamenList = new List<TempOrdenExamen>();
                OrdenMuestraList = new List<TempOrdenMuestra>();
                OrdenDatoClinicoList = new List<OrdenDatoClinico>();
            }
        }
        [Serializable]
        public class TempOrdenExamen
        {
            public Guid IdOrdenExamen { get; set; }
            public Guid IdOrden { get; set; }
            public Guid IdExamen { get; set; }
            public int IdEnfermedad { get; set; }
            public int IdTipoMuestra { get; set; }
            public int estatusE { get; set; }
            public int estatus { get; set; }
            public int estado { get; set; }
            public int estatusSol { get; set; }
            public string codigoExamen { get; set; }
            public int conforme { get; set; }
            public int ingresado { get; set; }
            public int validado { get; set; }
            public TempOrdenExamen()
            {
                IdOrdenExamen = Guid.NewGuid();
            }
        }
        [Serializable]
        public class TempOrdenMuestra
        {
            public Guid IdOrdenMuestra { get; set; }
            public Guid IdOrden { get; set; }
            public int IdTipoMuestra { get; set; }
            public int IdProyecto { get; set; }
            public int IdMuestraCod { get; set; }
            public DateTime FechaColeccion { get; set; }
            public DateTime HoraColeccion { get; set; }
            public int numero { get; set; }
            public int seriado { get; set; }
            public int estatus { get; set; }
            public int estado { get; set; }
            public int idusuario { get; set; }
            public TempOrdenMuestra()
            {
                IdOrdenMuestra = Guid.NewGuid();
            }
        }
        [Serializable]
        public class TempOrdenMaterial
        {
            public Guid IdOrdenMaterial { get; set; }
            public Guid IdOrden { get; set; }
            public int IdMaterial { get; set; }
            public int cantidad { get; set; }
            public Guid codigo { get; set; }
            public Guid IdOrdenMuestra { get; set; }
            public Guid IdOrdenExamen { get; set; }
            public int IdLaboratorio { get; set; }
            public int Tipo { get; set; }
            public DateTime FechaEnvio { get; set; }
            public DateTime HoraEnvio { get; set; }
            public decimal volumenMuestraColectada { get; set; }
            public int noPrecisaVolumen { get; set; }
            public int estatus { get; set; }
            public int estado { get; set; }
            public TempOrdenMaterial()
            {
                IdOrdenMaterial = Guid.NewGuid();
            }
        }
        [Serializable]
        public class TempOrdenExamenOrdenMuestra
        {
            public Guid IdOrdenExamen { get; set; }
            public Guid IdOrdenMuestra { get; set; }
            public int estatus { get; set; }
            public int estado { get; set; }
        }
        [Serializable]
        public class TempOrdenMuestraRecepcion
        {
            public Guid IdOrdenMuestraRecepcion { get; set; }
            public Guid IdOrden { get; set; }
            public Guid IdOrdenMaterial { get; set; }
            public int conformeR { get; set; }
            public DateTime FechaRecepcion { get; set; }
            public DateTime HoraRecepcion { get; set; }
            public int IdLaboratorioOrigen { get; set; }
            public int IdLaboratorioDestino { get; set; }
            public DateTime FechaEnvio { get; set; }
            public DateTime HoraEnvio { get; set; }
            public int estatusR { get; set; }
            public int estatusP { get; set; }
            public int estatus { get; set; }
            public int estado { get; set; }
            public int secuenObtencion { get; set; }
            public TempOrdenMuestraRecepcion()
            {
                IdOrdenMuestraRecepcion = Guid.NewGuid();
            }
        }
        [Serializable]
        public class TempOrdenResultadoAnalito
        {
            public Guid IdOrdenResultadoAnalito { get; set; }
            public Guid IdOrdenExamen { get; set; }
            public Guid IdAnalito { get; set; }
            public int IdSecuen { get; set; }
            public int orden { get; set; }
            public int estado { get; set; }
            public TempOrdenResultadoAnalito()
            {
                IdOrdenResultadoAnalito = Guid.NewGuid();
            }
        }
        [Serializable]
        public class TempOrdenDatoClinico
        {
            public Guid IdOrdenDatoClinico { get; set; }
            public int IdCategoriaDato { get; set; }
            public string ValorString { get; set; }
            public bool noPrecisa { get; set; }
            public int IdDato { get; set; }
            public int IdEnfermedad { get; set; }
            public TempOrdenDatoClinico()
            {
                IdOrdenDatoClinico = Guid.NewGuid();
            }
        }
    }
}

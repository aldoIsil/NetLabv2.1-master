using Enums;
using System;
using System.Collections.Generic;

namespace Model.Entidades
{
    [Serializable]
    public class Orden : EntidadBase
    {
        public Guid IdOrden { get; set; }
        public string CodigoOrden { get; set; }
        public int IdProyecto { get; set; }
        public Guid IdPaciente { get; set; }
        public int IdEstablecimiento { get; set; }
        public int IdEstablecimientoEnvio { get; set; }
        public string Solicitante { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string NroOficio { get; set; }
        public string Observacion { get; set; }
        public DateTime FechaIngresoINS { get; set; }
        public DateTime FechaReevaluacionFicha { get; set; }
        public List<OrdenExamen> OrdenExamenList { get; set; }
        public List<OrdenMuestra> OrdenMuestraList { get; set; }
        public TipoRegistroOrden TipoRegistro { get; set; }
        public int Estado { get; set; }
        public Orden()
        {
            IdOrden = Guid.NewGuid();
            OrdenExamenList = new List<OrdenExamen>();
            OrdenMuestraList = new List<OrdenMuestra>();
            TipoRegistro = TipoRegistroOrden.VACIO;
        }
    }
    [Serializable]
    public class OrdenExamen : EntidadBase
    {
        public Guid IdOrdenExamen { get; set; }
        public Guid IdOrden { get; set; }
        public Guid IdExamen { get; set; }
        public int IdEnfermedad { get; set; }
        public int IdTipoMuestra { get; set; }
        public List<OrdenMuestra> OrdenMuestraList { get; set; }
        public OrdenExamen()
        {
            IdOrdenExamen = Guid.NewGuid();
            OrdenMuestraList = new List<OrdenMuestra>();
        }
    }
    [Serializable]
    public class OrdenMuestra : EntidadBase
    {
        public Guid IdOrdenMuestra { get; set; }
        public Guid IdOrden { get; set; }
        public int IdTipoMuestra { get; set; }
        public int IdProyecto { get; set; }
        public DateTime FechaColeccion { get; set; }
        public string HoraColeccion { get; set; }
        public int Numero { get; set; }
        public int Seriado { get; set; }
        public string Codificacion { get; set; }
        public int Estado { get; set; }
        public int IdEstablecimiento { get; set; }
        public OrdenMuestra()
        {
            IdOrdenMuestra = Guid.NewGuid();
            Numero = 1;
            Seriado = 0;
            Estado = 1;
        }
    }
    [Serializable]
    public class OrdenExamenOrdenMuestra : EntidadBase
    {
        public Guid IdOrdenExamen { get; set; }
        public Guid IdOrdenMuestra { get; set; }
    }
    [Serializable]
    public class OrdenMaterial : EntidadBase
    {
        public Guid IdOrdenMaterial { get; set; }
        public Guid IdOrden { get; set; }
        public Guid IdOrdenMuestra { get; set; }
        public Guid IdOrdenExamen { get; set; }
        public int idMaterial { get; set; }
        public int Cantidad { get; set; }
        public int IdLaboratorio { get; set; }
        public decimal volumenMuestraColectada { get; set; }
        public int NoPrecisaVolumen { get; set; }
        public decimal FechaEnvio { get; set; }
        public string HoraEnvio { get; set; }
        public int Tipo { get; set; }
        public OrdenMaterial()
        {
            IdOrdenMaterial = Guid.NewGuid();
        }
    }
    [Serializable]
    public class OrdenResultadoAnalito
    {
        public Guid IdOrdenResultadoAnalito { get; set; }
        public Guid IdOrdenExamen { get; set; }
        public Guid IdAnalito { get; set; }
        public int IdSecuen { get; set; }
        public int Orden { get; set; }
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public OrdenResultadoAnalito()
        {
            IdOrdenResultadoAnalito = Guid.NewGuid();
            FechaRegistro = DateTime.Now;
        }
    }
    [Serializable]
    public class OrdenDatoClinico : EntidadBase
    {
        public Guid IdOrdenDatoClinico { get; set; }
        public Guid IdOrden { get; set; }
        public int IdEnfermedad { get; set; }
        public int IdCategoriaDato { get; set; }
        public int IdDato { get; set; }
        public int NoPrecisa { get; set; }
        public string Valor { get; set; }
        public int Estado { get; set; }
        public OrdenDatoClinico()
        {
            IdOrdenDatoClinico = Guid.NewGuid();
        }
    }
    [Serializable]
    public class Paciente :EntidadBase
    {
        public Guid IdPaciente { get; set; }
        public string NroDocumento { get; set; }
        public string NombreCompleto { get; set; }
        public string Direccion { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string TelefonoFijo { get; set; }
        public string Celular1 { get; set; }
        public string Celular2 { get; set; }
        public string Email { get; set; }
        public string Ocupacion { get; set; }
        public string TipoSeguroSalud { get; set; }
        public string OtroSeguroSalud { get; set; }
    }
    [Serializable]
    public class Examen : EntidadBase
    {
        public Guid IdExamen { get; set; }
        public string Nombre { get; set; }
        public int IPruebarapida { get; set; }
    }
    [Serializable]
    public class TipoMuestra
    {
        public int idTipoMuestra { get; set; }
        public string nombre { get; set; }
    }
    [Serializable]
    public class EntidadBase
    {
        public int IdUsuarioRegistro { get; set; }
        public int Estatus { get; set; }
        public DateTime FechaRegistro { get; set; }
        public EntidadBase()
        {
            FechaRegistro = DateTime.Now;
        }
    }

}

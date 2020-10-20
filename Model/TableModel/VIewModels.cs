using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;
using Model.Entidades;

namespace Model.ViewModels
{
    public class CrearOrdenVM
    {
        public Entidades.Orden Orden { get; set; }
        public TipoRegistroOrden TipoRegistro { get; set; }
        public Entidades.Paciente Paciente { get; set; }
        public int EstablecimientoOrigenId { get; set; }
        public int EstablecimientoEnvioId { get; set; }
        public DateTime FechaIngresoROM { get; set; }
        public DateTime? FechaEvaluacionFicha { get; set; }
        public int ProyectoId { get; set; }
        public string NroOficio { get; set; }
        public CrearOrdenExamenVM NuevoOrdenExamen { get; set; }
        public string CodigoOrden { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Observacion { get; set; }
        public List<CrearOrdenExamenTabla> OrdenExamenes { get; set; }
        public List<OrdenDatoClinicoCore> OrdenDatosClinicos { get; set; }
        public int SolicitanteId { get; set; }
        public CrearOrdenVM()
        {
            Orden = new Entidades.Orden
            {
                IdOrden = Guid.NewGuid()
            };
            Paciente = new Entidades.Paciente();
            FechaIngresoROM = DateTime.Today;
            //FechaEvaluacionFicha = DateTime.Today;
            FechaSolicitud = DateTime.Today;
            NuevoOrdenExamen = new CrearOrdenExamenVM
            {
                OrdenId = Orden.IdOrden,
                TipoRegistro = Orden.TipoRegistro
            };
            OrdenExamenes = new List<CrearOrdenExamenTabla>();
            OrdenDatosClinicos = new List<OrdenDatoClinicoCore>();
        }
    }

    public class CrearOrdenExamenVM
    {
        public Guid OrdenId { get; set; }
        public Guid OrdenExamenId { get; set; }
        public TipoRegistroOrden TipoRegistro { get; set; }
        public int EnfermedadId { get; set; }
        public int EstablecimientoDestinoId { get; set; }
        public Guid ExamenId { get; set; }
        public int TipoMuestraId { get; set; }
        public string FechaObtencion { get; set; }
        public string HoraObtencion { get; set; }
        public int UsuarioId { get; set; }
        //public string EnfermedadNombre { get; set; }
        //public string EstablecimientoDestinoNombre { get; set; }
        //public string ExamenNombre { get; set; }
        //public string TipoMuestraNombre { get; set; }
        public DateTime FechaObtencionDT
        {
            get
            {
                return string.IsNullOrWhiteSpace(FechaObtencion) ? DateTime.Now : DateTime.Parse(FechaObtencion);
            }
        }
        public DateTime HoraObtencionDT
        {
            get
            {
                var horaIndx = string.IsNullOrWhiteSpace(HoraObtencion) ? ("12:00").Split(':') : HoraObtencion.Split(':');
                var dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, Convert.ToInt32(horaIndx[0]), Convert.ToInt32(horaIndx[1]), 0);
                return dt;
            }
        }

        public CrearOrdenExamenVM()
        {
            OrdenExamenId = Guid.NewGuid();
        }
    }

    public class CrearOrdenExamenTabla
    {
        public Guid OrdenId { get; set; }
        public Guid OrdenExamenId { get; set; }
        public Guid OrdenMuestraId { get; set; }
        public int EnfermedadId { get; set; }
        public int EstablecimientoDestinoId { get; set; }
        public string EstablecimientoDestinoNombre { get; set; }
        public TipoRegistroOrden TipoRegistro { get; set; }
        public string CodigoMuestra { get; set; }
        public string Enfermedad { get; set; }
        public string Examen { get; set; }
        public string TipoMuestra { get; set; }
        public DateTime FechaObtencionDB { get; set; }
        public DateTime HoraObtencionDB { get; set; }
        public string FechaObtencion
        {
            get
            {
                return FechaObtencionDB.ToString("dd/MM/yyyy");
            }
            set
            {
            }
        }
        public string HoraObtencion
        {
            get
            {
                return HoraObtencionDB.ToString("HH:mm");
            }
            set
            {
            }
        }
        public string LaboratorioDestino { get; set; }
        public bool Conforme { get; set; }
        public string[] MotivoRechazo { get; set; }
        public DateTime FechaObtencionDT
        {
            get
            {
                return string.IsNullOrWhiteSpace(FechaObtencion) ? DateTime.Now : DateTime.Parse(FechaObtencion);
            }
        }
        public DateTime HoraObtencionDT
        {
            get
            {
                var horaIndx = string.IsNullOrWhiteSpace(HoraObtencion) ? ("12:00").Split(':') : HoraObtencion.Split(':');
                var dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, Convert.ToInt32(horaIndx[0]), Convert.ToInt32(horaIndx[1]), 0);
                return dt;
            }
        }

        public int IdTipoMuestra { get; set; }
        public int IdCriterioRechazo { get; set; }
        public string CriterioRechazoGlosa { get; set; }
        public int ProyectoId { get; set; }
        public Guid IdOrdenMuestraRecepcion { get; set; }
        public int IdUsuario { get; set; }
        public bool TienePR { get; set; }
        public CrearOrdenExamenTabla()
        {
            MotivoRechazo = new string[] { };
            Conforme = true;
        }
    }

    public class EnfermedadDatos
    {
        public Guid IdOrden { get; set; }
        public Guid IdOrdenDatoClinico { get; set; }
        public int IdEnfermedad { get; set; }
        public string EnfermedadNombre { get; set; }
        public int IdCategoriaDatoPadre { get; set; }
        public int IdCategoriaDato { get; set; }
        public string CategoriaNombre { get; set; }
        public int Orientacion { get; set; }
        public int Visible { get; set; }
        public int IdDato { get; set; }
        public string DatoNombre { get; set; }
        public TipoCampo IdTipoDato { get; set; }
        public string IdsExamen { get; set; }
        public bool Obligatorio { get; set; }
        public int idListaDato { get; set; }
        public int idOpcionDato { get; set; }
        public string OpcionDatoValor { get; set; }
        public int OpcionDatoOrden { get; set; }
    }

    public class OrdenDatoClinicoCore
    {
        public Guid IdOrden { get; set; }
        //public Guid IdOrdenDatoClinico { get; set; }
        public int IdEnfermedad { get; set; }
        //public int IdCategoriaDatoPadre { get; set; }
        public int IdCategoriaDato { get; set; }
        public int IdDato { get; set; }
        public bool NoPrecisa { get; set; }
        //{
        //    get
        //    {
        //        //return !string.IsNullOrWhiteSpace(NoPrecisaString) && NoPrecisaString.ToLower() == "on";
        //    }
        //    set
        //    {
        //    }
        //}
        public string Valor { get; set; }
        public int IdUsuarioRegistro { get; set; }
    }

    public class OrdenMuestraVM
    {
        public string NroOficio { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public DateTime fechaRecepcionRomINS { get; set; }
        public int EstablecimientoDestinoId { get; set; }
        public int EstablecimientoEnvioId { get; set; }
        public string CodigoMuestra { get; set; }
        public int IdUsuario { get; set; }
    }

    public class OrdenMuestraRecepcionado
    {
        public Guid IdOrdenMuestra { get; set; }
        public Guid IdOrdenMuestraRecepcion { get; set; }
        public string CodigoMuestra { get; set; }
        public string CodigoLineal { get; set; }
        public string Paciente { get; set; }
        public List<int> RechazoIds { get; set; }
        public int IdTipoMuestra { get; set; }
        public int IdCriterioRechazo { get; set; }
        public string CriterioRechazoGlosa { get; set; }
    }

    public class OrdenMuestraRecepcionados
    {
        public Guid IdOrdenMuestra { get; set; }
        public Guid IdOrdenMuestraRecepcion { get; set; }
        public string CodigoMuestra { get; set; }
        public string CodigoLineal { get; set; }
        public string Paciente { get; set; }
        public string[] MotivoRechazo { get; set; }
        public int IdUsuario { get; set; }
        public OrdenMuestraRecepcionados()
        {
            MotivoRechazo = new string[] { };
        }
    }

    public class OrdenMuestraRecepcionadosExcel
    {
        public string CodigoMuestra { get; set; }
        public string CodigoLineal { get; set; }
        public string Paciente { get; set; }
    }

    public class ExamenesVM
    {
        public Guid IdExamen { get; set; }
        public string NombreExamen { get; set; }
    }
}

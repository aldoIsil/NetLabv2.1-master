using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ReportesDTO
{
    [Serializable]
    public class ReporteResult
    {
        public int NumeroDias { get; set; }
        public int EstablecimientoCodigoUnico { get; set; }
        public int NumeroMuestras { get; set; }
    }
    //JRCR - 2doProducto
    [Serializable]
    public class ReporteRequest
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string NombreFiltro { get; set; }
        public string IdJurisdiccion { get; set; }
        public int IdEnfermedad { get; set; }
        public string IdExamen { get; set; }
        public int IdUsuario { get; set; } //Usuario analista o verificador
        public int TipoReporte { get; set; } //1: General; 2: Detalle
    }

    [Serializable]
    public class ReportResponse
    {
        public int DiferenciaDias { get; set; }
        public int EstablecimientoCodigoUnico { get; set; }
        public string Muestras { get; set; }
        public List<int> EESSDias { get; set; }
    }

    [Serializable]
    public class ReportePorcentajeResponse
    {
        public string Establecimiento { get; set; }
        public decimal TotalRV { get; set; }
        public decimal TotalMuestras { get; set; }
        public decimal Porcentaje { get; set; }
    }

    [Serializable]
    public class ReporteTableValues
    {
        public string Indicador { get; set; }
        //public int Total { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Median { get; set; }
        public decimal FirstQuartile { get; set; }
        public decimal ThirdQuartile { get; set; }
    }

    [Serializable]
    public class ReporteRadarDatos
    {
        public int EstablecimientoId { get; set; }
        public string Establecimiento { get; set; }
        public string CriterioRechazo { get; set; }
        public int CantidadMuestras { get; set; }
    }

    [Serializable]
    public class MotivoRechazoDatos
    {
        public int EstablecimientoId { get; set; }
        public string Establecimiento { get; set; }
        public int IdOrden { get; set; }
        public string CodigoOrden { get; set; }
        public int IdOrdenMuestraRecepcion { get; set; }
        public int IdEnfermedad { get; set; }
        public string Enfermedad { get; set; }
        public int IdCriterioRechazo { get; set; }
        public string CriterioRechazo { get; set; }
        public int IdOrdenExamen { get; set; }
        public int TipoCriterioRechazo { get; set; }//CR.Tipo
        public int CantidadMuestras { get; set; }
        public int IdUsuarioRegistro { get; set; }
    }

    [Serializable]
    public class RadarReporteObject
    {
        public string label { get; set; }
        public string data { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string pointBackgroundColor { get; set; }
        public string pointBorderColor { get; set; }
        public string pointHoverBackgroundColor { get; set; }
        public RadarReporteObject()
        {
            pointBorderColor = "#fff";
            pointHoverBackgroundColor = "#fff";
        }
    }

    [Serializable]
    public class LineReporteObject
    {
        public string label { get; set; }
        public string data { get; set; }
    }

    [Serializable]
    public class ReportePieResponse
    {
        public string Indicador { get; set; } //Establecimiento(laboratorio, estable, etc), analista, enfermedad,etc
        public int ValorIndicadorNumero { get; set; }//numero
        public decimal ValorIndicadorPorcentaje { get; set; }//porcentaje
    }

    [Serializable]
    public class ReporteCantidadResultado
    {
        public string valor { get; set; }
        public int cantidad { get; set; }
        public int cantidadAcumulada { get; set; }
        public int canPos { get; set; }
        public int canAcumPos { get; set; }
        public int canTriRi { get; set; }
        public int canAcumRi { get; set; }
        public int canTriRri { get; set; }
        public int canAcumRri { get; set; }
    }

    [Serializable]
    public class ConsultaReporteResultado
    {
        public string Usuario { get; set; }
        public string Establecimiento { get; set; }
        public string Enfermedad { get; set; }
        public string Examen { get; set; }
        public int NumeroInforme { get; set; }
        public DateTime FechaVerificacion { get; set; }
        public DateTime FechaImpresion { get; set; }
        public int HorasRetraso { get; set; }
        public int CantidadConsulta { get; set; }
        public int totalResultado { get; set; }
    }

    [Serializable]
    public class ReporteResultadoInformado
    {
        public string Examen { get; set; }
        public string Paciente { get; set; }
        public int CantidadExamen { get; set; }
        public int CantidadExamenTotal { get; set; }
        public int CantidadPaciente { get; set; }
    }


    /* yrca */
    [Serializable]
    public class reporteProductividadRom
    {
        public string codigoOrden { get; set; }
        public string codigoMuestra { get; set; }
        public string Muestra { get; set; }
        public string Paciente { get; set; }
        public string nombreEstablecimientoOrigen { get; set; }
        public string fechaSolicitud { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string fechaRecepcion { get; set; }
        public string Enfermedad { get; set; }
        //public string nombreDestino { get; set; }
        public string nombreExamen { get; set; }

    }

    [Serializable]
    public class ReporteOportunidadRom
    {
        public string Laboratorio { get; set; }
        public string codificacion { get; set; }
        public string fechaRecepcionRomINS { get; set; }
        public string fechaEnvio { get; set; }
        public string fechaRecepcionP { get; set; }
        public int DiasTranscurridos { get; set; }
        public string Oportunidad { get; set; }
        public int totalEnvios { get; set; }
        public int totalOportunos { get; set; }
    }

    [Serializable]
    public class ReportePacienteCoronavirus
    {
        public string codigoOrden { get; set; }        
        public string codigoMuestra { get; set; }
        public string nroOficio { get; set; }
        public string EESSDepOrigen { get; set; }
        public string EESSProvOrigen { get; set; }
        public string EESSDistOrigen { get; set; }
        public string EESSDisaOrigen { get; set; }
        public string EESSRedOrigen { get; set; }
        public string EESSMicroRedOrigen { get; set; }
        public string EstablecimientoOrigen { get; set; }
        public string DocIdentidad { get; set; }
        public string fechaNacimiento { get; set; }
        public string nombrePaciente { get; set; }
        public string edad { get; set; }
        public string SexoPaciente { get; set; }
        public string TipoMuestra { get; set; }
        public string FechaColeccion { get; set; }
        public string FechaRecepcionROM { get; set; }
        public DateTime? FechaValidado { get; set; }
        public string FechaHoraColeccion { get; set; }
        public string FechaHoraRecepcionROM { get; set; }
        public string FechaHoraRecepcionLAB { get; set; }
        public string FechaHoraValidado { get; set; }
        public string MuestraConforme { get; set; }
        public string CriteriosRechazo { get; set; }
        public string convResultado { get; set; }
        public int estatusP { get; set; }
        public int estatusR { get; set; }
        public int estatus { get; set; }
        public int conformeR { get; set; }
        public int conformeP { get; set; }
        public string EstatusResultado { get; set; }
        public string DireccionPaciente { get; set; }
        public string AntecedenteViaje { get; set; }
        public string FechHoraValidado2 { get; set; }
        public string celular { get; set; }
        public CoronavirusDatosClinicos oCoronavirus { get; set; }
    }

    [Serializable]
    public class CoronavirusDatosClinicos
    {
        public string asma { get; set; }
        public string cardiopatia { get; set; }
        public string contactoAves { get; set; }
        public string contactoCerdo { get; set; }
        public string contactocasoIRAG{ get; set; }
        public string IRAG7dias { get; set; }
        public string crianzaAves { get; set; }
        public string crianzaCerdo { get; set; }
        public string diabetesMiellitus { get; set; }
        public string dxPresuntivo { get; set; }
        public string dificultadRespiratoria { get; set; }
        public string enferNeurologica { get; set; }
        public string enferRenalCronica { get; set; }
        public string fallecimiento { get; set; }
        public string fechaAlta { get; set; }
        public string fechaenvioMuestra { get; set; }
        public string fechaHospitalizacion { get; set; }
        public string fechaIngresoUCI { get; set; }
        public string FecInicioAdmin { get; set; }
        public string FecIniSintomas { get; set; }
        public string FecObtencion { get; set; }
        public string FecDefuncion { get; set; }
        public string fiebre { get; set; }
        public string gestacion { get; set; }
        public string hepatopatiacronica { get; set; }
        public string hospitalizacion { get; set; }
        public string irag { get; set; }
        public string uci { get; set; }
        public string inmunodeficiencia { get; set; }
        public string visitaPaises { get; set; }
        public string obesidad { get; set; }
        public string oseltamivir { get; set; }
        public string enfpulmonarcronica { get; set; }
        public string otro { get; set; }
        public string otrosSignos { get; set; }
        public string pacde5a60 { get; set; }
        public string puerperio { get; set; }
        public string sindromegripal { get; set; }
        public string tempeatura { get; set; }
        public string tipoMuestra { get; set; }
        public string tomamuestra { get; set; }
        public string trabajadorsalud { get; set; }
        public string tos { get; set; }
        public string trimestre { get; set; }
        public string vacunacionantigripal { get; set; }
        public string viajesultimos15dias { get; set; }
        public string IRAG_INUSCITADA { get; set; }
        public string Muerte_IRAG_causa_desconocida { get; set; }
        public string Sindrome { get; set; }
        public string TomaMuestras { get; set; }
    }

    [Serializable]
    public class ReporteMuestrasCoronavirus
    {
        public string codigoOrden { get; set; }
        public string codigoMuestra { get; set; }
        public string nroOficio { get; set; }
        public string EESSDepOrigen { get; set; }
        public string EESSProvOrigen { get; set; }
        public string EESSDistOrigen { get; set; }
        public string EESSDisaOrigen { get; set; }
        public string EESSRedOrigen { get; set; }
        public string EESSMicroRedOrigen { get; set; }
        public string EstablecimientoOrigen { get; set; }
        public string DocIdentidad { get; set; }
        public string fechaNacimiento { get; set; }
        public string nombrePaciente { get; set; }
        public string edad { get; set; }
        public string SexoPaciente { get; set; }
        public string TipoMuestra { get; set; }
        public string FechaColeccion { get; set; }
        public string FechaRecepcionROM { get; set; }
        public string FechaValidado { get; set; }
        public string FechaHoraColeccion { get; set; }
        public string FechaHoraRecepcionROM { get; set; }
        public string FechaHoraRecepcionLAB { get; set; }
        public string FechaHoraValidado { get; set; }
        public string MuestraConforme { get; set; }
        public string CriteriosRechazo { get; set; }
        public string convResultado { get; set; }
        public int estatusP { get; set; }
        public int estatusR { get; set; }
        public int estatus { get; set; }
        public int conformeR { get; set; }
        public int conformeP { get; set; }
        public string EstatusResultado { get; set; }
        public string FechHoraValidado2 { get; set; }
    }

    [Serializable]
    public class PacienteCoronavirus
    {
        public int NroPaciente { get; set; }
        public string EESSDepOrigen { get; set; }
        public string EstablecimientoOrigen { get; set; }
        public string nombrePaciente { get; set; }
        public string DocIdentidad { get; set; }
        public string direccion { get; set; }
        public string codigoMuestra { get; set; }
        public string TipoMuestra { get; set; }
        public string edad { get; set; }
        public string SexoPaciente { get; set; }
        public DateTime FechaValidado { get; set; }
        public string Resultado { get; set; }
        public string AntecedenteViaje { get; set; }
        public string celular { get; set; }

        public string codigoOrden { get; set; }
        public string nroOficio { get; set; }
        public string EESSProvOrigen { get; set; }
        public string EESSDistOrigen { get; set; }
        public string EESSDisaOrigen { get; set; }
        public string EESSRedOrigen { get; set; }
        public string EESSMicroRedOrigen { get; set; }
        public string fechaNacimiento { get; set; }
        public string FechaColeccion { get; set; }
        public string FechaRecepcionROM { get; set; }
        public string FechaHoraColeccion { get; set; }
        public string FechaHoraRecepcionROM { get; set; }
        public string FechaHoraRecepcionLAB { get; set; }
        public string FechaHoraValidado { get; set; }
        public string MuestraConforme { get; set; }
        public string CriteriosRechazo { get; set; }
        public int Positivo { get; set; }
        public int Negativo { get; set; }
        public int TotalDepNeg { get; set; }
        public string FechHoraValidado2 { get; set; }
    }

    [Serializable]
    public class PacienteCoronavirusxDep
    {
        public string Region { get; set; }
        public int PositivoRegion { get; set; }
        public int NegativoRegion { get; set; }
    }

    [Serializable]
    public class DTOReporteCoronavirus
    {
        public virtual List<ReportePacienteCoronavirus> LstReporteMuestrasCoronavirus { get; set; }
        public virtual List<ReportePacienteCoronavirus> LstReportePacienteCoronavirus { get; set; }
        public virtual List<PacienteCoronavirus> LstPacienteCoronavirus { get; set; }
        public virtual List<PacienteCoronavirusxDep> LstPacienteCoronavirusxDep { get; set; }
        public int TotaPacientesVerificados { get; set; }
        public int TotalMuestrasNegativos { get; set; }
        public int TotalMuestrasPositivo { get; set; }
        public int TotalPacientesNegativos { get; set; }
        public int TotalPacientesPositivos { get; set; }
        public int TotalMuestrasPendientes { get; set; }
        public int TotalMuestrasIngresadas { get; set; }
        public int TotalMuestrasRechzosRom { get; set; }
        public int TotalMuestrasRechzosLab { get; set; }
        public int TotalMuestrasPendienteVerificacion { get; set; }
        public int TotalMuestrasVerificacion { get; set; }
    }
}

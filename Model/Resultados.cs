using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
    [Serializable]
    public class Resultados
    {
        public Guid idOrden { get; set; }


        public Guid idOrdenExamen { get; set; }

        public string Examen { get; set; }

        public Guid idExamen { get; set; }

        public string NombreExamen { get; set; }

        public string Enfermedad { get; set; }

        public string CodigoMuestra { get; set; }

        public string NombreMuestra { get; set; }

        public DateTime fechaColeccion { get; set; }

        public DateTime horaColeccion { get; set; }

        public DateTime fechaRecepcion { get; set; }

        public DateTime horaRecepcion { get; set; }

        public DateTime fechaRecepcionP { get; set; }

        public DateTime horaRecepcionP { get; set; }

        public DateTime fechaResultado { get; set; }

        public string Analista { get; set; }

        public DateTime FechaProceso { get; set; }

        public int codificacion { get; set; }

        public int idAreaProcesamiento { get; set; }

        public DateTime fechaNacimiento { get; set; }

        public string Analito { get; set; }

        public string resultado { get; set; }

        public string NombEstab { get; set; }

        public int validado { get; set; }

        public int idUsuarioValidado { get; set; }

        public string CodifMuestra { get; set; }

        public int liberado { get; set; }

        public string Unidad { get; set; }

        public string observacion { get; set; }

        public string Metodo { get; set; }

        public string ValorNormal { get; set; }

        public DateTime fechaSolicitud { get; set; }

        public string estiloColor { get; set; }

        public string horaColeccionStr { get; set; }

        public string horaRecepcionStr { get; set; }

        public string horaRecepcionPStr { get; set; }

        public int conforme { get; set; }

        public List<Metodo> Metodos { get; set; }
    }

    [Serializable]
    public class ResultadoWebService
    {

        public class Resultado
        {
            public string num_solicitud { get; set; }
            public string num_orden { get; set; }
            public string est_orden { get; set; }
            public int cod_enfermedad { get; set; }
            public int cod_tip_muestra { get; set; }
            public string cod_muestra { get; set; }
            public string est_muestra { get; set; }
            public string cod_tip_examen { get; set; }
            public string cod_examen { get; set; }
            public string est_examen { get; set; }
            public string des_resultado { get; set; }
            public string des_rechazo_muestra { get; set; }
            public string des_rechazo_examen { get; set; }
            public string fec_rechazo { get; set; }
            public string fec_resultados { get; set; }
            public string fec_rechazo_muestra { get; set; }
            public string fec_rechazo_examen { get; set; }
        }
    }

    [Serializable]
    public static class EstadoRecepcionMuestra
    {
        public const string MUESTRA_RECIBIDA = "MUESTRA RECIBIDA";
        public const string MUESTRA_NORECIBIDA = "MUESTRA NO RECIBIDA";
        public const string ERROR = "ERROR";
    }

    [Serializable]
    public class Protocolo
    {
        public int nroSecuencia { get; set; }
        public string nroProtocolo { get; set; }
        public string codigoMuestra { get; set; }
        public string kit { get; set; }
        public string muestra_sin_recepcionar { get; set; }
        public string muestra_con_resultado { get; set; }
        public string observacion { get; set; }
    }
    [Serializable]
    public class ProtocoloEvaluado
    {
        public int nroSecuencia { get; set; }
        public int nroProtocolo { get; set; }
        public string codigoMuestra { get; set; }
        public DateTime fechaExtraccion { get; set; }
        public string equipo { get; set; }
        public string loteMarca { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int idUsuario { get; set; }
        public string estadoMuestra { get; set; }
        public string Kit { get; set; }
    }
    [Serializable]
    public class ResultadoKobos
    {
        public int idPruebaRapidaKobo { get; set; }
        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string Nombres { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string direccion { get; set; }
        public string fechaNacimiento { get; set; }
        public string ejecutorPrueba { get; set; }
        public string establecimientoEjecutor { get; set; }
        public string fechaObtencion { get; set; }
        public string FechaResultado { get; set; }
        public string Resultado { get; set; }
        public int edad { get; set; }
        public string nroInforme { get; set; }
    }

    [Serializable]
    public class ResultadoCovidPaciente
    {
        public string tip_documento { get; set; }
        public string nro_documento { get; set; }
        public string nombres { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public string nro_celular { get; set; }
        public string email { get; set; }
        public string ubigeo { get; set; }
        //public string idUbigeoActual { get; set; }
        public string des_departamento { get; set; }
        public string des_provincia { get; set; }
        public string des_distrito { get; set; }
        public string direccion { get; set; }
        public string fec_coleccion { get; set; }
        public string hor_coleccion { get; set; }
        public string fec_validacion { get; set; }
        public string rest_prueba { get; set; }
        public string tip_prueba { get; set; }
    }

    [Serializable]
    public class ResultadosINS
    {
        public string FechaObtencionMuestra { get; set; }
        public string FechaRecepcionINS { get; set; }
        public string CodigoOrden { get; set; }
        public string CodigoMuestra { get; set; }
        public string DepartamentoOrigen { get; set; }
        public string DisaOrigen { get; set; }
        public string EstablecimientoOrigen { get; set; }
        public int EdadPaciente { get; set; }
        public string SexoPaciente { get; set; }
        public string Enfermedad { get; set; }
        public string Examen { get; set; }
        public string Resultado { get; set; }
        public string Estado { get; set; }
    }
}

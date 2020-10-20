using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Model.ViewData;
using System.Globalization;
namespace Model
{
    [Serializable]
    public class Orden : General
    {
        public Guid idOrden { get; set; }

        // [EstablecimientoValidacion(ErrorMessage = "Se requiere Establecimiento")]

        //[EstablecimientoValidacion(ErrorMessage = "Se requiere Establecimiento")]
        [Display(Name = "EESS/LAB origen:")]
        public int idEstablecimiento { get; set; }
        [Display(Name = "EESS/LAB Envío:")]
        public int idEstablecimientoEnvio { get; set; }
        [Display(Name = "EESS/LAB Destino")]
        public int IdLaboratorioDestino { get; set; }

        [Display(Name = "Proyecto:")]
        public int idProyecto { get; set; }//] [int] NULL,
        public Guid idPaciente { get; set; }//] [uniqueidentifier] NULL,
        public Guid idAnimal { get; set; }//] [uniqueidentifier] NULL,
        public Guid idCepaBancoSangre { get; set; }//] [uniqueidentifier] NULL,
                                                   //public string idEnfermedad {get;set;}//] [varchar](8) NULL,
        public string codigoOrden { get; set; }//] [uniqueidentifier] NULL,
        [Display(Name = "Observaciones:")]
        public string observacion { get; set; }//] [nchar](10) NULL,
        public Paciente Paciente { get; set; }
        public List<Paciente> listPaciente { get; set; }

        [Required(ErrorMessage = "Se requiere el Proyecto")]
        [Display(Name = "Proyecto:")]
        public Proyecto Proyecto { get; set; }


        public Guid codigo { get; set; }//] [uniqueidentifier] NULL,

        //  [Required(ErrorMessage = "Se requiere el N° Oficio")]
        [Display(Name = "Nro Oficio / Documento:")]
        public string nroOficio { get; set; }//] [varchar](50) NULL,

        [Display(Name = "Fecha Ingreso ROM-INS:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fechaIngresoINS { get; set; }
        public DateTime FechaObtencion { get; set; }
        [Display(Name = "Fecha Evaluación de la Ficha:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fechaReevaluacionFicha { get; set; }

        [Display(Name = "Enfermedad:")]
        public Enfermedad enfermedad { get; set; }

        [Display(Name = "EESS/LAB:")]
        public Establecimiento establecimiento { get; set; }
        public Establecimiento establecimientoEnvio { get; set; }
        public List<OrdenDatoClinico> ordenDatoClinicoList { get; set; }

        public List<Enfermedad> enfermedadList { get; set; }
        public List<OrdenMaterial> ordenMaterialList { get; set; }
        public List<OrdenExamen> ordenExamenList { get; set; }
        public List<OrdenMuestra> ordenMuestraList { get; set; }
        public int estatus { get; set; }
        public List<TipoMuestra> tipoMuestraList { get; set; }


        public List<Material> materialList { get; set; }

        public List<OrdenVd> ordenInfoList { get; set; }
        public List<OrdenMaterialVd> ordenMaterialVdList { get; set; }

        public List<CepaBancoSangre> ordenCepaBancoSangre { get; set; }

        public CepaBancoSangre cepaBancoSangre { get; set; }

        public List<OrdenMuestraRecepcion> ordenMuestraRecepcionadaList { get; set; }

        //public int status { get; set; }

        public int genero { get; set; }

        public List<Resultados> resultadosList { get; set; }

        public List<Resultados> analitosList { get; set; }

        public List<ResultAnalito> detResultList { get; set; }

        public List<Liberados> liberadosList { get; set; }

        public ResultAnalito ResultAnalito { get; set; }

        public int? idLaboratorioRecepcion { get; set; }

        [Display(Name = "Solicitante:")]
        public String solicitante { get; set; }

        [Display(Name = "Fecha de Solicitud:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? fechaSolicitud { get; set; }
        public Usuario usuario { get; set; }
        public UsuarioEnvioSms usuarioEnvio { get; set; }

        [Display(Name = "Subir Archivo:")]
        public byte[] file { get; set; }
        public string Procedencia { get; set; }
        public List<Ciudad> ProcedenciaPaciente { get; set; }

        public String fechaRegistroShow
        {
            get
            {

                //día Domingo 20 de Marzo 2016 a las 16:56

                //var input = "Tuesday, July 26, 2011";
                //var format = "dddd, MMMM dd, yyyy";

                //String.Format("{0:dd MMM yyyy a las hh:mm}", this.FechaRegistro);  // "Sun, Mar 9, 2008"

                //var dt = DateTime.ParseExact(input, format, new CultureInfo("en-US"));

                var dia = this.FechaRegistro.ToString("dddd dd", new CultureInfo("es-ES"));
                dia = dia.Substring(0, 1).ToUpper() + dia.Substring(1, dia.Length - 1);

                var mes = this.FechaRegistro.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                mes = mes.Substring(0, 1).ToUpper() + mes.Substring(1, mes.Length - 1);

                var hora = this.FechaRegistro.ToString("HH:mm", new CultureInfo("es-ES"));

                return dia + " de " + mes + " a las " + hora;
            }
        }

        public string EstablecimientoCodigoUnico { get; set; }
        public string ClasificacionEstablecimiento { get; set; }
        //Autor: Juan Muga
        //Descripcion: Se agrega campos de estados pertenecientes a la orden.
        public int ConformeR { get; set; }
        public int EstadoOrdenExamen { get; set; }
        public int EstadoOrdenMuestra { get; set; }
        public int EstadoOrdenExamenOrdenMuestra { get; set; }
        public int EstadoOrdenMuestraRecepcion { get; set; }
        //Fin
        public string dniEjecutor { get; set; }
        public string ejecutorPr { get; set; }
        public string ApePatEjecutor { get; set; }
        public string ApeMatEjecutor { get; set; }
        //
        public string editarEstablecimiento { get; set; }
    }
    [Serializable]
    public class OrdenMuestraLinealkobo
    {
        public string codigoOrden { get; set; }
        public string codigoMuestra { get; set; }
        public string codigoLineal { get; set; }
        public string dni { get; set; }
        public string apepat { get; set; }
        public string apemat { get; set; }
        public string nombre { get; set; }
    }

    [Serializable]
    public class DatosOrdenExamenSesison
    {
        public OrdenMuestra ordenMuestra { get; set; }
        public OrdenExamen ordenExamen { get; set; }
        public OrdenMaterial ordenMaterial { get; set; }
        public int idUsuarioLogin { get; set; }
    }
}

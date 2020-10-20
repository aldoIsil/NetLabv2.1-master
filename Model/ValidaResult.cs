using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class ValidaResult
    {
        [Display(Name = "EESS/LAB")]
        public string Establecimiento { get; set; }

        [Display(Name = "Código Orden")]
        public string CodigoOrden { get; set; }

        [Display(Name = "Número Documento")]
        public string nroDocumento { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime fechaRegistro { get; set; }

        public Guid idOrden { get; set; }

        public string nroOficio { get; set; }
        public string paciente { get; set; }

        public int idUsuarioIngreso { get; set; }

        public int idLaboratorio { get; set; }

        public string NomLab { get; set; }

        [Display(Name = "Fecha de Solicitud")]
        public DateTime fechaSolicitud { get; set; }

        public int genero { get; set; }

        public DateTime fechaNacimiento { get; set; }

        public int validado { get; set; }

        public string tipoDocumento { get; set; }
        public int EXISTE_PENDIENTE { get; set; }

        public int EXISTE_VALIDADO { get; set; }

        //SOTERO AGRGEADO PARA SOLICITUD DE INGRESO DE NUEVOS RESULTDOS EN MUESTRAS VALIDADAS

        public int SOLICITA_INGRESO { get; set; }
        public int SOLICITA_OK { get; set; }
        public int SOLICITA_NO { get; set; }
             
        public int idProyecto { get; set; }

        public string idExamen { get; set; }
        public string NombreExamen { get; set; }

        public string idOrdenExamen { get; set; }
        public string Resultado { get; set; }
        public string Conforme { get; set; }
        public string Comentarios { get; set; }
        public string codigoMuestra { get; set; }
        public string celular { get; set; }
        public string ExamenComun { get; set; }
        public DateTime fechaColeccion { get; set; }
    }
    [Serializable]
    public class ValidaResultadoMasivo
    {
        public string idOrdenExamen { get; set; }
        public string Conforme { get; set; }
        public string Comentarios { get; set; }
    }
}

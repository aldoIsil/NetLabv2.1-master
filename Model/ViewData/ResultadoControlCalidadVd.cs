using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewData
{
    [Serializable]
    public class ResultadoControlCalidadVd
    {
        public Guid idConfigEvaluacion { get; set; }
        public Guid idConfiguracionPanel { get; set; }
        public Guid idConfigEvaluacionMaterial { get; set; }
        public int idEstablecimientoEvaluador { get; set; }
        public int idEstablecimientoEvaluado { get; set; }
        public string EESSEvaluado { get; set; }
        public string EESSEvaluador { get; set; }
        public string NroPregunta { get; set; }
        public string Respuesta { get; set; }
        public string ValorRespuesta { get; set; }
        public decimal puntaje { get; set; }
        public string TipoPanel { get; set; }
        public string TipoMetodo { get; set; }

        public int idTipoPanel { get; set; }
        [DisplayName("Tipo Metodo Material:")]
        public int idTipoMetodo { get; set; }

        public int idusuarioregistro { get; set; }
        [DisplayName("Usuario Registro")]
        public string UsuarioRegistro { get; set; }
        [DisplayName("Usuario Edicion")]
        public string UsuarioEdicion { get; set; }
        [DisplayName("Fecha Registro")]
        public string fecharegistro { get; set; }
        public int idusuarioedicion { get; set; }
        [DisplayName("Fecha Edicion")]
        public string fechaedicion { get; set; }
        [DisplayName("Usuario Envío")]
        public int idusuarioenvio { get; set; }
        [DisplayName("Fecha Evío")]
        public string fechaenvio { get; set; }
    }
}

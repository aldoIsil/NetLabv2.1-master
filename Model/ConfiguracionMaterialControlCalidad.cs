using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class ConfiguracionMaterialControlCalidad
    {
        public Guid idConfigEvaluacionMaterial { get; set; }
        public Guid idConfiguracionPanel { get; set; }
        public int idTipoPanel { get; set; }
        public int idTipoEvaluacionPanel { get; set; }
        [DisplayName("Tipo Evaluación")]
        public int idTipoEvaluacion { get; set; }
        [DisplayName("Valor Respuesta")]
        public string ValorRespuesta { get; set; }
        [DisplayName("Tipo Método")]
        public int idTipoMetodo { get; set; }
        [DisplayName("Tipo Sub Método")]
        public int idSubTipoMetodo { get; set; }
        [DisplayName("Nro Pregunta")]
        public string nroPregunta { get; set; }
        [DisplayName("Tipo Dato")]
        public int tipoDato { get; set; }
        [DisplayName("Puntaje")]
        public decimal puntaje { get; set; }
        [DisplayName("Respuesta")]
        public string Respuesta { get; set; }

        public int idUsuarioRecepcion { get; set; }
        [DisplayName("Usuario Recepción")]
        public string UsuarioRecepcion { get; set; }
        [DisplayName("Fecha Recepción")]
        public string fechaRecepcion { get; set; }
        [DisplayName("Estado Recepción")]
        public int estadoRecepcion { get; set; }
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
        public string DescTipoEvaluacion { get; set; }
        public string DescTipoMetodo { get; set; }
        public string DescSubTipoMetodo { get; set; }
    }
}

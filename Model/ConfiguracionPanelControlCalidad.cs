using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class ConfiguracionPanelControlCalidad
    {
        public Guid idConfiguracionPanel { get; set; }
        public Guid idConfigEvaluacion { get; set; }
        [DisplayName("Tipo Evaluación")]
        public int idTipoEvaluacion { get; set; }
        public string DescTipoEvaluacion { get; set; }
        [DisplayName("Nro Lote")]
        public string nroLote { get; set; }
        [DisplayName("Nombre Panel")]
        public string nombre { get; set; }
        [DisplayName("Descripción")]
        public string descripcion { get; set; }
        [DisplayName("Tipo")]
        public int idTipo { get; set; }
        public string DescTipo { get; set; }

        [DisplayName("Sub Tipo")]
        public int idSubTipo { get; set; }
        public int estado { get; set; }
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
    }
}

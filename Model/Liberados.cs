using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Model
{
    [Serializable]
    public class Liberados
    {
        public Guid idOrdenExamen { get; set; }

        public Guid idOrdenResultadoAnalito { get; set; }

        public Guid idOrden { get; set; }

        public string CodOrden { get; set; }

        public string NombreExamen { get; set; }

        public DateTime FechaProceso { get; set; }

        public int idAreaProcesamiento { get; set; }

        public string NombreMuestra { get; set; }

        [Display(Name = "Id Paciente")]
        public int codificacion { get; set; }

        public DateTime fechaNacimiento { get; set; }

        public string Analito { get; set; }

        public string resultado { get; set; }

        public string NombEstab { get; set; }

        public string NomProy { get; set; }

        public string NomComp { get; set; }

        public int validado { get; set; }

        public int idUsuarioValidado { get; set; }

        public int liberado { get; set; }

        public int idUsuarioLiberado { get; set; }

        public string CodifMuestra { get; set; }

        public string Unidad { get; set; }

        public string observacion { get; set; }

        public string Metodo { get; set; }

        public string ValorNormal { get; set; }

        public List<AnalitoOpcionResultado> ListJsResultados { get; set; }

    }
}

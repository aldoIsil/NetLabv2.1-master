using System;
using System.Collections.Generic;

namespace Model.ViewData
{
    [Serializable]
    public class OrdenExamenResultadosVd
    {
        public Guid IdOrdenExamen { get; set; }
        public int IdExamenMetodo { get; set; }
        public Examen Item { get; set; }
        public MuestraResultadoVd Muestra { get; set; }
        public int estatusE { get; set; }

        public List<ExamenMetodo> Metodos { get; set; }
        public string CodigoMuestra { get; set; }
        public string NombreEnfermedad { get; set; }
        public List<OrdenResultadoAnalitoVd> AnalitosValidar { get; set; }
        public string CodigoBarra { get; set; }//Sotero 03/07/2019 Generacion Codigo de Barra.
        public string Examen { get; set; }
        public List<ExamenPlataforma> Plataformas { get; set; }
        public int idPlataforma { get; set; }
    }
}

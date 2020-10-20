using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class ExamenResultadoDetalleConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad ExamenResultadoDetalle
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<ExamenResultadoDetalle> DetalleExamenes(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetDetalle).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad ExamenResultadoDetalle
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Se agregó el campo ListJsResultados  - Juan Muga
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static ExamenResultadoDetalle GetDetalle(DataRow row)
        {
            return new ExamenResultadoDetalle
            {
                CodigoMuestra = GetString(row, "codificacion"),
                TipoMuestra = GetString(row, "nombre"),
                Analito = GetString(row, "analito"),
                Resultado = GetString(row, "resultado"),
                ValorInferior = GetDecimal(row, "valorInferior"),
                ValorSuperior = GetDecimal(row, "valorSuperior"),
                Observacion = GetString(row, "observacion"),
                FechaEmision = GetDateTime(row, "fechaValidado"),
                Validador = GetString(row, "validador"),
                Liberador = GetString(row, "liberador"),
                TipodeMuestra = GetString(row, "TipoMuestra"),
                Enfermedad = GetString(row, "Enfermedad"),
                nombreExamen = GetString(row, "nombreExamen"),
                Componente = GetString(row, "Componente"),
                tagResultado = GetString(row, "Resultado"),
                ValorReferencia = GetString(row, "ValorReferencia"),
                Analista = GetString(row, "Analista"),
                Verificador = GetString(row, "Verificador"),
                FechaHoraEmision = GetString(row, "FechaHoraEmision"),
                ListJsResultados = new DetalleAnalitoDal().GetAnalitosbyIdAnalitoResultado(new SerializarResultado().DeserializarResultados(Converter.GetString(row, "Resultado"))),
                NroSecuencia = GetInt(row,"NroSecuencia"),
                estatusSol = GetInt(row, "estatusSol"),
                nroInforme = GetInt(row, "nroInforme"),
                interpretacion = GetString(row, "interpretacion"),
                metodo = GetString(row, "Metodo"),
                iPruebaRapida = GetInt(row, "iPruebaRapida"),
                EjecutorPr = GetString(row, "nombreEjecutor"),
                Plataforma = GetString(row, "Plataforma"),
                Inacal = GetInt(row,"Inacal")
            };
        }

        public static List<ExamenResultadoInterpretacion> ExamenInterpretacion(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetInterpretacion).ToList();
        }

        private static ExamenResultadoInterpretacion GetInterpretacion(DataRow row)
        {
            return new ExamenResultadoInterpretacion
            {
                GlosaParent = GetString(row, "GlosaParent"),
                Glosa = GetString(row, "Glosa"),
                Interpretacion = GetString(row, "Interpretacion")
            };
        }
    }
}
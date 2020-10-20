using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;

namespace DataLayer.DalConverter 
{
    public class LiberadosConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Liberados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Liberados> ListLiberados(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetLiberado).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Liberados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Liberados GetLiberado(DataRow row)
        {
            Liberados lb = new Liberados
            {
                idOrdenExamen = GetGuid(row,"idOrdenExamen"),
                idOrdenResultadoAnalito = GetGuid(row, "idOrdenResultadoAnalito"),
                idOrden = GetGuid(row, "idOrden"),
                NombreExamen = GetString(row, "NombreExamen"),
                FechaProceso = GetDateTime(row, "FechaProceso"),
                NombreMuestra = GetString(row, "NombreMuestra"),
                codificacion = GetInt(row, "codificacion"),
                idAreaProcesamiento = GetInt(row, "idAreaProcesamiento"),
                fechaNacimiento = GetDateTime(row, "fechaNacimiento"),
                Analito = GetString(row, "Analito"),
                resultado = GetString(row, "resultado"),
                NombEstab = GetString(row, "NombEstab"),
                validado = GetInt(row, "validado"),
                idUsuarioValidado = GetInt(row, "idUsuarioValidado"),
                liberado = GetInt(row, "liberado"),
                idUsuarioLiberado = GetInt(row, "idUsuarioLiberado"),
                CodifMuestra = GetString(row, "CodifMuestra"),
                Unidad = GetString(row, "Unidad"),
                observacion = GetString(row, "observacion"),
                Metodo = GetString(row, "Metodo"),
                ValorNormal = GetString(row, "ValorNormal"),
                ListJsResultados = new DetalleAnalitoDal().GetAnalitosbyIdAnalitoResultado(new SerializarResultado().DeserializarResultados(Converter.GetString(row, "resultado")))
            };
            return lb;
        }


    }
}

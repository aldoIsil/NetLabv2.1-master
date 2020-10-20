using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class AnalitoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static List<Analito> Analitos(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetAnalito).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Analito GetAnalito(DataRow row)
        {
            return new Analito
            {
                IdAnalito = GetGuid(row, "idAnalito"),
                Nombre = GetString(row, "nombre"),
                Descripcion = GetString(row, "descripcion"),
                Tipo = GetInt(row, "tipo"),
                IdListaUnidad = GetInt(row, "idListaUnidad")
            };
        }
    }
}
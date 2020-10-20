using System.Collections.Generic;
using Framework.DAL;
using Model;
using System.Data;
using System.Linq;

namespace DataLayer.DalConverter
{
    public class TipoDatoConvertTo : Converter
    {
        public static TipoDato TipoDato(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new TipoDato
            {
                IdTipo = GetInt(row, "idTipo"),
                Nombre = GetString(row, "nombre"),
                Descripcion = GetString(row, "descripcion"),
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad TipoDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<TipoDato> TipoDatos(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetTipoDato).ToList();
        }

        private static TipoDato GetTipoDato(DataRow row)
        {
            return new TipoDato
            {
                IdTipo = GetInt(row, "idTipo"),
                Nombre = GetString(row, "nombre"),
                Descripcion = GetString(row, "descripcion")
            };
        }
    }
}

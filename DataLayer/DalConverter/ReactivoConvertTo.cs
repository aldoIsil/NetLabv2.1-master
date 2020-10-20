using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class ReactivoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Reactivo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Reactivo> Reactivos(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetReactivo).ToList();
        }

        public static Reactivo Reacctivo (DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetReactivo(row);
        }

        private static Reactivo GetReactivo(DataRow row)
        {
            return new Reactivo
            {
                idReactivo = GetInt(row, "idReactivo"),
                glosa = GetString(row, "glosa"),
                Estado = GetInt(row,"Estado")
            };
        }
    }
}
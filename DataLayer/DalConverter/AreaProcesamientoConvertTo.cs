using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;

namespace DataLayer.DalConverter
{
    public class AreaProcesamientoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte Data Table en una lista de tipo AreaProcesamiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static List<AreaProcesamiento> AreasProcesamiento(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetAreaProcesamiento).ToList();
        }
        /// <summary>
        /// Descripción: Convierte Data Table en una lista de tipo AreaProcesamiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static AreaProcesamiento AreaProcesamiento(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row =  dataTable.Rows[0];

            return GetAreaProcesamiento(row);
        }
        /// <summary>
        /// Descripción: Convierte Data Table en una lista de tipo AreaProcesamiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static AreaProcesamiento GetAreaProcesamiento(DataRow row)
        {
            return new AreaProcesamiento
            {
                IdAreaProcesamiento = GetInt(row, "idAreaProcesamiento"),
                Nombre = GetString(row, "nombre"),
                Descripcion = GetString(row, "descripcion"),
                Sigla = GetString(row, "sigla"),
                Estado = GetInt(row, "estado")
            };
        }
    }
}
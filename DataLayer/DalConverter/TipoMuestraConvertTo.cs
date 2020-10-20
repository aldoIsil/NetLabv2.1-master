using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;

namespace DataLayer.DalConverter
{
    public class TipoMuestraConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte Data Table a una lista de tipo TipoMuestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<TipoMuestra> TipoMuestras(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetTipoMuestra).ToList();
        }
        /// <summary>
        /// Descripción: Convierte Data Table a una lista de tipo TipoMuestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static TipoMuestra TipoMuestra(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetTipoMuestra(row);
        }
        /// <summary>
        /// Descripción: Convierte Data Table a una lista de tipo TipoMuestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static TipoMuestra GetTipoMuestra(DataRow row)
        {
            return new TipoMuestra
            {
                idTipoMuestra = GetInt(row, "idTipoMuestra"),
                nombre = GetString(row, "nombre"),
                descripcion = GetString(row, "descripcion"),
                nemo = GetString(row, "nemo"),
                Estado = GetInt(row, "Estado")
            };
        }
    }
}

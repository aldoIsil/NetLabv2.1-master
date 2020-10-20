using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;

namespace DataLayer.DalConverter
{
    public class DetalleCatalogoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad DetalleCatalogo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<DetalleCatalogo> DetallesCatalogo(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetDetalleCatalogo).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad DetalleCatalogo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static DetalleCatalogo DetalleCatalogo(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetDetalleCatalogo(row);
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad DetalleCatalogo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static DetalleCatalogo GetDetalleCatalogo(DataRow row)
        {
            return new DetalleCatalogo
            {
                Prueba = GetString(row, "Prueba"),
                Muestra = GetString(row, "Muestra"),
                Caracteristica = GetString(row, "Caracteristica"),
                Transporte = GetString(row, "Transporte"),
                Laboratorio = GetString(row, "Laboratorio"),
                DiasEmision = GetInt(row, "DiasEmision"),
                DiasEntrega = GetInt(row,"DiasEntrega")
            };
        }

    }
}

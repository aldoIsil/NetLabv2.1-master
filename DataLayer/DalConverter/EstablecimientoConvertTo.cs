using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class EstablecimientoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Establecimiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Establecimiento> Establecimientos(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetEstablecimiento).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Establecimiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Establecimiento GetEstablecimiento(DataRow row)
        {
            return new Establecimiento
            {
                IdEstablecimiento = GetInt(row, "idEstablecimiento"),
                Nombre = GetString(row, "nombre")
            };
        }
    }
}
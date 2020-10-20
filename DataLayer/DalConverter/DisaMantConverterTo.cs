using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;

namespace DataLayer.DalConverter
{
    public class DisaMantConverterTo :Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad DisaMant
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<DisaMant> Disas(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetDisas).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad DisaMant
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static DisaMant GetDisas(DataRow row)
        {
            return new DisaMant
            {
                IdDISA = GetString(row, "idDISA"),
                NombreDISA = GetString(row, "nombreDISA"),
                Estado = GetInt(row, "Estado")
            };
        }




    }
}

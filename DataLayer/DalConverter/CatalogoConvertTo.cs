using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;

namespace DataLayer.DalConverter
{
    public class CatalogoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Enfermedad
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static List<Enfermedad> Enfermedades(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetEnfermedad).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Enfermedad
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Enfermedad Enfermedad(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetEnfermedad (row);
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Enfermedad
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Enfermedad GetEnfermedad(DataRow row)
        {
            return new Enfermedad
            {
                idEnfermedad = GetInt(row, "idEnfermedad"),                
                nombre = GetString (row, "nombre"),
                Snomed = GetString(row,"SNOMED")
            };
        }

    }
}

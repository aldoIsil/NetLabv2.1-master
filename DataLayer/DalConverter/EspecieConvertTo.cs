using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class EspecieConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad AnimalEspecie
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<AnimalEspecie> Especies(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetEspecie).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad AnimalEspecie
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static AnimalEspecie GetEspecie(DataRow row)
        {
            return new AnimalEspecie
            {
                IdEspecie = GetInt(row, "idEspecie"),
                Nombre = row["nombre"].ToString(),
                Descripcion = row["descripcion"].ToString()
            };
        }
    }
}
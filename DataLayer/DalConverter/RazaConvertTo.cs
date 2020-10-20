using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class RazaConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad AnimalRaza
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<AnimalRaza> Razas(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetRaza).ToList();
        }

        public static AnimalRaza GetRaza(DataRow row)
        {
            return new AnimalRaza
            {
                IdRaza = GetInt(row, "idRaza"),
                Nombre = row["nombre"].ToString(),
                Descripcion = row["descripcion"].ToString()
            };
        }
    }
}
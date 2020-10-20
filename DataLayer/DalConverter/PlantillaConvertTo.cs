using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;

namespace DataLayer.DalConverter
{
    public class PlantillaConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Plantilla
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Model.Plantilla> Plantillas(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetPlantilla).ToList();
        }

        public static Model.Plantilla Plantilla(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetPlantilla(row);
        }

        private static Model.Plantilla GetPlantilla(DataRow row)
        {
            return new Model.Plantilla
            {
                IdPlantilla = GetInt(row, "idPlantilla"),
                Nombre = GetString(row, "nombre"),
                Descripcion = GetString(row, "descripcion") ?? string.Empty,
                Tipo = GetString(row, "tipo"),
                IdTipo = GetInt(row, "idTipo")
            };
        }
    }
}
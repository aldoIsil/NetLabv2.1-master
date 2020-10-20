using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;


namespace DataLayer.DalConverter
{
    public class PresentacionConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Presentacion> Presentaciones(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetPresentacion).ToList();
        }

        public static Presentacion Presentacion(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetPresentacion(row);
        }

        private static Presentacion GetPresentacion(DataRow row)
        {
            return new Presentacion
            {
                idPresentacion = GetInt(row, "idPresentacion"),
                glosa = GetString(row, "glosa"),
                Estado = GetInt(row, "Estado")
            };
        }
    }
}

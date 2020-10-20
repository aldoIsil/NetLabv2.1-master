using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class UbigeoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Ubigeo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Ubigeo> Ubigeos(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetUbigeo).ToList();
        }

        private static Ubigeo GetUbigeo(DataRow row)
        {
            return new Ubigeo
            {
                Id = row["id"].ToString(),
                Descripcion = row["descripcion"].ToString()
            };
        }
    }
}
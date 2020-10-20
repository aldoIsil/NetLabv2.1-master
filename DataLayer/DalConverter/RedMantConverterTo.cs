using Framework.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DalConverter
{
    public class RedMantConverterTo:Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad RedMant
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<RedMant> Redes(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetRedes).ToList();
        }

        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad RedMant
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<RedMant> RedesCombo(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetRedesCombo).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad RedMant
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static RedMant GetRedesCombo(DataRow row)
        {
            return new RedMant
            {
                idred = GetString(row, "idred"),
                nombrered = GetString(row, "nombrered"),
            };
        }


        private static RedMant GetRedes(DataRow row)
        {
            return new RedMant
            {
                idDisa = GetString(row, "iddisa"),
                idred = GetString(row, "idred"),
                nombredisa = GetString(row, "nombredisa"),
                  nombrered = GetString(row, "nombrered"),
                  Estado=GetInt(row,"Estado")
            };
        }


    }
}

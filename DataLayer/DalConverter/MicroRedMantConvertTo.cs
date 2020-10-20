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
    class MicroRedMantConvertTo:Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<MicroRedMant> MicroRedes(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetMicroRedes).ToList();
        }

        private static MicroRedMant GetMicroRedes(DataRow row)
        {
            return new MicroRedMant
            {

                iddisa = GetString(row, "iddisa"),
                idred = GetString(row, "idred"),
                idmicrored = GetString(row, "idmicrored"),
                nombremicrored = GetString(row, "nombremicrored"),
                estado = GetInt(row, "estado"),
                nombredisa=GetString(row, "nombredisa"),
                nombrered = GetString(row, "nombrered")
            };
        }
    }
}

using Framework.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Model;

namespace DataLayer.DalConverter
{
    public class EnfermedadExamenConvertTo : Converter
    {
        /// <summary>
        ///  Descripción: Convierte un data table a una lista tipada con la entidad EnfermedadExamen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<EnfermedadExamen> EnfermedadesPorExamen(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetEnfermedadExamen).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad EnfermedadExamen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static EnfermedadExamen GetEnfermedadExamen(DataRow row)
        {
            return new EnfermedadExamen
            {
                IdEnfermedad = GetInt(row,"idEnfermedad"),
                IdExamen = GetGuid(row, "idExamen"),
                Orden = GetInt(row, "orden")
            };
        }
    }
}

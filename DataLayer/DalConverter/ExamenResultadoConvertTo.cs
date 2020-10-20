using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class ExamenResultadoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Obtiene informacion de un data table y lo transforma a una lista de tipo ExamenResultado
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<ExamenResultado> Examenes(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetExamen).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene informacion de un data table y lo transforma a una lista de tipo ExamenResultado
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static ExamenResultado GetExamen(DataRow row)
        {
            return new ExamenResultado
            {
                IdOrdenExamen = GetGuid(row, "idOrdenExamen"),
                Examen = GetString(row, "examen"),
                Enfermedad = GetString(row, "enfermedad"),
                Metodo = GetString(row, "metodo"),
                FechaLiberacion = GetDateTime(row, "fechaLiberado")
            };
        }
    }
}
using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;

namespace DataLayer.DalConverter
{
    public class ExamenConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte Data Table en una lista de tipo Examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Examen> Examenes(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetExamen).ToList();
        }
        /// <summary>
        /// Descripción: Convierte Data Table en una lista de tipo Examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static Examen Examen(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetExamen(row);
        }
        /// <summary>
        /// Descripción: Convierte Data Table en una lista de tipo Examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Juan Muga - 04/11/2017 - Se agrega el campo PruebaRapida.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static Examen GetExamen(DataRow row)
        {
            return new Examen
            {
                idExamen = GetGuid(row, "idExamen"),
                nombre = GetString(row, "nombre"),
                descripcion = GetString(row, "descripcion"),
                Cpt = GetIntNullable(row, "CPT"),
                Loinc = GetString(row, "LOINC"),
                IdGenero = GetIntNullable(row,"idGenero"),
                Estado = GetInt(row, "estado"),
                PruebaRapida = GetInt(row, "pruebarapida"),
                Tipo = GetInt(row, "idTipo"),
                nombreEnfermedad = GetString(row, "nombreEnfermedad")
            };
        }
        /// <summary>
        /// Descripción: Convierte Data Table en una lista de tipo Examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<ExamenMetodo> ExamenMetodo(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetMetodo).ToList();
        }
        /// <summary>
        /// Descripción: Convierte Data Table en una lista de tipo Examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static ExamenMetodo GetMetodo(DataRow row)
        {
            return new ExamenMetodo
            {
                IdExamen = GetGuid(row, "idExamen"),
                IdExamenMetodo = GetInt(row,"idExamenMetodo"),
                Glosa = GetString(row,"glosa"),
                Orden = GetInt(row,"ordenMetodo"),
                Estado = GetInt(row, "estado")
            };
        }

        public static List<Examen> ExamenAgrupado(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetExamenAgrupado).ToList();
        }
        private static Examen GetExamenAgrupado(DataRow row)
        {
            return new Examen
            {
                Tipo = GetInt(row, "idExamenAgrupado"),
                nombre = GetString(row, "nombre"),
            };
        }
    }
}

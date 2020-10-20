using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class DISAConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad DISA
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<DISA> DISAs(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetDISA).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad DISA
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static DISA GetDISA(DataRow row)
        {
            return new DISA
            {
                IdDISA = row["idDISA"].ToString(),
                NombreDISA = row["nombreDISA"].ToString(),
                IdInstitucion = row["codigoInstitucion"] != null ? int.Parse(row["codigoInstitucion"].ToString()) : 0
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Red
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Red> Redes(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetRed).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Red
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Red GetRed(DataRow row)
        {
            return new Red
            {
                IdDISA = row["idDISA"] != null ? row["idDISA"].ToString() : string.Empty,
                IdRed = row["idRed"].ToString(),
                NombreRed = row["nombreRed"].ToString(),
                IdInstitucion = row["codigoInstitucion"] != null ? int.Parse(row["codigoInstitucion"].ToString()) : 0
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad MicroRed
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<MicroRed> MicroRedes(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetMicroRed).ToList();
        }
        /// <summary>
        ///  Descripción: Convierte un data table a una lista tipada con la entidad MicroRed
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static MicroRed GetMicroRed(DataRow row)
        {
            return new MicroRed
            {
                //IdDISA = row["idDISA"].ToString(),
                //IdRed = row["idRed"].ToString(),
                IdMicroRed = row["idMicroRed"].ToString(),
                NombreMicroRed = row["nombreMicroRed"].ToString()
            };
        }
    }
}
using System.Collections.Generic;
using Framework.DAL;
using System.Data;
using System.Linq;
using Model;

namespace DataLayer
{
    public class MuestraConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Retorna el valor de la fila Cantidad.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static int cantidad(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return 0;

            var row = dataTable.Rows[0];

            int cantidad = GetInt(row, "cantidad");
            
            return cantidad;
        }
        /// <summary>
        /// Descripción: Convierte un data table a una entidad MuestraCodificacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static MuestraCodificacion Codificacion(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new MuestraCodificacion
            {
                idMuestraCod = GetGuid(row, "idMuestraCod"),
                idEstablecimiento = GetInt(row, "idEstablecimiento"),
                codificacion = row["codificacion"].ToString(),
                IdUsuarioRegistro = GetInt(row, "idUsuarioRegistro"),
                FechaRegistro = GetDateTime(row, "fechaRegistro"),
                codificacionLineal = row["codigoLineal"].ToString(),
                Estado = GetInt(row,"estado")
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista de tipo MuestraCodificacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<MuestraCodificacion> Codificaciones(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;
            
            return (from DataRow row in dataTable.Rows
                select new MuestraCodificacion
                {
                    idMuestraCod = GetGuid(row, "idMuestraCod"),
                    idEstablecimiento = GetInt(row, "idEstablecimiento"),
                    codificacion = row["codificacion"].ToString(),
                    IdUsuarioRegistro = GetInt(row, "idUsuarioRegistro"),
                    FechaRegistro = GetDateTime(row, "fechaRegistro"),
                }).ToList();
        }


        public static List<MuestraCodificacion> ConsultaCodigosMuestra(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            return (from DataRow row in dataTable.Rows
                    select new MuestraCodificacion
                    {
                        codificacion = row["codificacion"].ToString()
                    }).ToList();
        }

        public static List<MuestraCodificacion> ConsultaCodigosGenerados(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            return (from DataRow row in dataTable.Rows
                    select new MuestraCodificacion
                    {
                        codificacion = row["codificacion"].ToString(),
                        codificacionLineal = row["codigoLineal"].ToString(),
                        estado = row["Estado"].ToString(),
                        fecha = row["fechaCodigo"].ToString(),
                        usuario = row["Usuario"].ToString()
                    }).ToList();
        }

    }
}

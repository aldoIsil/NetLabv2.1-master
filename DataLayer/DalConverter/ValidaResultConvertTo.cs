using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;


namespace DataLayer.DalConverter
{
    public class ValidaResultConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad ValidaResult
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<ValidaResult> Validaciones(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetValidacion).ToList();
        }


        private static ValidaResult GetValidacion(DataRow row)
        {
            ValidaResult vr = new ValidaResult
            {
                Establecimiento = row["NombEstab"].ToString(),
                CodigoOrden = row["CodOrden"].ToString(),
                nroDocumento = row["nroDocumento"].ToString(),
                fechaRegistro = GetDateTime(row, "fechaRegistro"),
                idOrden = GetGuid(row, "idOrden"),
                idUsuarioIngreso = GetInt(row, "idUsuarioIngreso"),
                idLaboratorio = GetInt(row, "idLaboratorio"),
                NomLab = row["NomLab"].ToString(),
                fechaSolicitud = GetDateTime(row, "fechaSolicitud"),
                genero = GetInt(row, "genero"),
                fechaNacimiento = GetDateTime(row, "fechaNacimiento"),
                nroOficio = row["nroOficio"].ToString(),
                validado = GetInt(row,"validado")

            };
            return vr;
        }
        
    }
}




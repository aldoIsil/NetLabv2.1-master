using Framework.DAL;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;


namespace DataLayer.DalConverter
{
    public class ResultadosConvertTo : Converter    
    {
        /// <summary>
        /// Descripción: Convierte el DataTable de la lista de resultadso a una Lista.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Resultados> ListResultados(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetResultado).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene el data row de un Data Table y convierte a una Entidad en este caso a Resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Resultados GetResultado(DataRow row)
        {
            return new Resultados
            {
                idOrden = GetGuid(row, "idOrden"),
                idOrdenExamen = GetGuid(row, "idOrdenExamen"),
                Examen = GetString(row, "Examen"),
                Enfermedad = GetString(row, "Enfermedad"),
                CodigoMuestra = GetString(row, "CodigoMuestra"),
                NombreMuestra = GetString(row, "NombreMuestra"),
                fechaColeccion = GetDateTime(row, "fechaColeccion"),
                horaColeccion = GetDateTime(row, "horaColeccion"),
                fechaRecepcion = GetDateTime(row, "fechaRecepcion"),
                fechaRecepcionP = GetDateTime(row, "fechaRecepcionP"),
                horaRecepcion = GetDateTime(row, "horaRecepcion"),
                horaRecepcionP = GetDateTime(row, "horaRecepcionP"),
                fechaResultado = GetDateTime(row, "fechaResultado"),
                Analista = GetString(row, "Analista"),
                idExamen = GetGuid(row, "idExamen"),
                NombreExamen = GetString(row, "NombreExamen"),
                FechaProceso = GetDateTime(row, "FechaProceso"),
                codificacion = GetInt(row, "codificacion"),
                idAreaProcesamiento = GetInt(row, "idAreaProcesamiento"),
                fechaNacimiento = GetDateTime(row, "fechaNacimiento"),
                Analito = GetString(row, "Analito"),
                resultado = GetString(row, "resultado"),
                NombEstab = GetString(row, "NombEstab"),
                validado = GetInt(row, "validado"),
                idUsuarioValidado = GetInt(row, "idUsuarioValidado"),
                CodifMuestra = GetString(row, "CodifMuestra"),
                liberado = GetInt(row, "liberado"),
                Unidad = GetString(row, "Unidad"),
                observacion = GetString(row, "observacion"),
                Metodo = GetString(row, "Metodo"),
                ValorNormal = GetString(row, "ValorNormal"),
                fechaSolicitud = GetDateTime(row, "fechaSolicitud"),
                conforme = GetInt(row, "conforme"),
                horaColeccionStr = GetDateTime(row, "horaColeccion").ToString("HH:mm"),
                horaRecepcionStr = GetDateTime(row, "horaRecepcion").ToString("HH:mm"),
                horaRecepcionPStr = GetDateTime(row, "horaRecepcionP").ToString("HH:mm"),
                estiloColor = GetInt(row, "conforme") == 1 ? "background-color:#ccc" : "background-color:#fff"
            };
        }
    }
}

using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class MuestraResultadoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Obtiene informacion de un data table y lo transforma a un alista de tipo MuestraResultado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<MuestraResultado> Muestras(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetMuestra).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene informacion de un data table y lo transforma a un alista de tipo MuestraResultado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private static MuestraResultado GetMuestra(DataRow row)
        {
            return new MuestraResultado
            {
                TipoMuestra = GetString(row, "nombre"),
                Remitente = GetString(row, "remitente"),
                FechaColeccion = GetDateTime(row, "fechaColeccion"),
                Receptor = GetString(row, "receptor"),
                FechaRecepcion = GetDateTime(row, "fechaRecepcion"),
                codigomuestra = GetString(row, "ID_Muestra"),
                EESS_LAB_Destino = GetString(row, "EESS_LAB_Destino"),
                FechaHoraColeccion = GetString(row, "FechaHoraColeccion"),
                FechaHoraRecepcionROM = GetString(row, "FechaHoraRecepcionROM"),
                FechaHoraRecepcionLAB = GetString(row, "FechaHoraRecepcionLAB"),
                tipodeMuestra = GetString(row, "TipoMuestra")
            };
        }
    }
}
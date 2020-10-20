using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class UsuarioEstablecimientoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad UsuarioEstablecimiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<UsuarioEstablecimiento> Establecimientos(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetUsuarioEstablecimiento).ToList();
        }

        private static UsuarioEstablecimiento GetUsuarioEstablecimiento(DataRow row)
        {
            return new UsuarioEstablecimiento
            {
                nomInstitucion = GetString(row, "nombreInstitucion"),
                nomDisa = GetString(row, "nombreDISA"),
                nomRed = GetString(row, "nombreRed"),
                nomMicrored = GetString(row, "nombreMicroRed"),
                idEstablecimiento = GetIntNullable(row, "idEstablecimiento"),
                nomEstablecimiento = GetString(row, "nombre"),
            };
        }
    }
}
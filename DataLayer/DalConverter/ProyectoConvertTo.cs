using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class ProyectoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Proyecto 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Proyecto> Proyectos(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetProyecto).ToList();
        }

        private static Proyecto GetProyecto(DataRow row)
        {
            return new Proyecto
            {
                IdProyecto = GetInt(row, "idProyecto"),
                Nombre = GetString(row, "nombre"),
                Descripcion = GetString(row, "descripcion")
            };
        }
    }
}
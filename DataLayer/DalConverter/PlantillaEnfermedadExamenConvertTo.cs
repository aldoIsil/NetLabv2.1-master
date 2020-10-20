using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class PlantillaEnfermedadExamenConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Metodo que retorna la lista cargada con los datos de plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<PlantillaEnfermedadExamen> Muestras(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetMuestras).ToList();
        }
        /// <summary>
        /// Descripción: Metodo que retorna la entidad cargada con los datos de plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static PlantillaEnfermedadExamen GetMuestras(DataRow row)
        {
            return new PlantillaEnfermedadExamen
            {
                IdPlantilla = GetInt(row, "idPlantilla"),
                IdEnfermedad = GetInt(row, "idEnfermedad"),
                Enfermedad = GetString(row, "enfermedad"),
                IdExamen = GetGuid(row, "idExamen"),
                Examen = GetString(row, "examen"),
                IdTipoMuestra = GetInt(row, "idTipoMuestra"),
                Muestra = GetString(row, "muestra"),
                IdMaterial = GetInt(row, "idMaterial"),
                Presentacion = GetString(row, "presentacion"),
                Reactivo = GetString(row, "reactivo"),
                Volumen = GetDecimalNullable(row, "volumen"),
                Cantidad = GetIntNullable(row, "cantidad")
            };
        }
    }
}
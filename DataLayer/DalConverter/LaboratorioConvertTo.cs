using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class LaboratorioConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte informacion de un data table en una lista
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Laboratorio> Laboratorios(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetLaboratorio).ToList();
        }
        /// <summary>
        /// Descripción: Obtiene informacion de un data row y lo transforma en una entidad Laboratorio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Actualizado por: Marcos Mejia
        /// Fecha: 19/01/2017
        /// Modificación: 1. Se agrega dato del logo Regional.
        ///               2. Se agregó Departamento, Provincia y Distrito  
        ///               3. Se modificó el nombreInstitucion por "descripcion" - Marcos Mejia 10-05-2018
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Laboratorio GetLaboratorio(DataRow row)
        {
            return new Laboratorio
            {
                IdLaboratorio = GetInt(row, "idLaboratorio"),
                CodigoInstitucion = GetInt(row, "codigoInstitucion"),
                CodigoUnico = GetString(row, "codigoUnico"),
                Nombre = GetString(row, "nombre"),
                Ubigeo = GetString(row, "ubigeo"),
                Direccion = GetString(row, "direccion"),
                IdDisa = GetString(row, "idDisa"),
                IdRed = GetString(row, "idRed"),
                IdMicroRed = GetString(row, "idMicroRed"),
                IdCategoria = GetInt(row, "idCategoria"),
                Latitud = GetString(row, "latitud"),
                Longitud = GetString(row, "longitud"),
                LogoRegional = GetBytes(row, "logoReg"),
                Logo = GetBytes(row, "logo"),
                Sello = GetBytes(row, "sello"),
                Website = GetString(row, "website"),
                Estado = GetInt(row, "estado"),
                correoElectronico = GetString(row, "correoElectronico"),
                clasificacion = GetString(row, "clasificacion"),
                tipo = GetInt(row, "tipo"),
                nombreInstitucion = GetString(row, "descripcion"),
                nombreDisa = GetString(row, "nombreDisa"),
                nombreRed = GetString(row, "nombreRed"),
                nombreMicroRED = GetString(row, "nombreMicroRed"),
                UbigeoLaboratorio = new Ubigeo
                {
                    Departamento = GetString(row,"Departamento"),
                    Provincia = GetString(row,"Provincia"),
                    Distrito = GetString(row,"Distrito"),
                },                                
            };
        }
        /// <summary>
        /// Descripción: Convierte informacion de un data table en una entidad de tipo Laboratorio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static Laboratorio Laboratorio(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return GetLaboratorio(row);
        }
    }
}
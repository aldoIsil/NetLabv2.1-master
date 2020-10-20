using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class AnimalConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Animal
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static List<Animal> Animales(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetAnimal).ToList();
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Animal
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Animal GetAnimal(DataRow row)
        {
            return new Animal
            {
                IdAnimal = GetGuid(row, "idAnimal"),
                Codificacion = GetLong(row, "codificacion"),
                Raza = GetRaza(row),
                Sexo = GetString(row, "sexo"),
                Nombre = GetString(row, "nombresAnimal") ?? string.Empty,
                Propietario = GetPropietario(row),
                Refugio = GetString(row, "nombreRefugio") ?? string.Empty
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Paciente
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static Paciente GetPropietario(DataRow row)
        {
            return new Paciente
            {
                TipoDocumento = GetIntNullable(row, "tipoDocumento"),
                NroDocumento = GetString(row, "nroDocumento") ?? string.Empty,
                ApellidoPaterno = GetString(row, "apellidoPaterno") ?? string.Empty,
                ApellidoMaterno = GetString(row, "apellidoMaterno") ?? string.Empty,
                Nombres = GetString(row, "nombrePropietario") ?? string.Empty,
                Genero = GetIntNullable(row, "genero"),
                TelefonoFijo = GetString(row, "telefonoFijo") ?? string.Empty,
                Celular1 = GetString(row, "celular") ?? string.Empty,
                UbigeoActual = new Ubigeo { Id = GetString(row, "idUbigeoPropietario") ?? string.Empty },
                DireccionActual = GetString(row, "direccionPropietario") ?? string.Empty
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad AnimalRaza
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static AnimalRaza GetRaza(DataRow row)
        {
            return new AnimalRaza
            {
                IdRaza = GetInt(row, "idRaza"),
                Nombre = GetString(row, "raza") ?? string.Empty,
                Especie = GetEspecie(row)
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad AnimalEspecie
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static AnimalEspecie GetEspecie(DataRow row)
        {
            return new AnimalEspecie
            {
                IdEspecie = GetInt(row, "idEspecie"),
                Nombre = GetString(row, "especie") ?? string.Empty
            };
        }
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Animal
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Animal Animal(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new Animal
            {
                IdAnimal = GetGuid(row, "idAnimal"),
                Nombre = GetString(row, "nombresAnimal"),
                Genero = GetInt(row, "generoAnimal"),
                Origen = GetInt(row, "codigoOrigen"),
                Raza = GetRaza(row),
                Codificacion = GetLong(row, "codificacion"),
                FechaNacimiento = GetDateTimeNullable(row, "fechaNacimiento"),
                EdadAnio = GetIntNullable(row, "edadAnio"),
                EdadMes = GetIntNullable(row, "edadMes"),
                IdUbigeo = GetString(row, "idUbigeoAnimal"),
                Direccion = GetString(row, "direccionAnimal"),
                Refugio = GetString(row, "nombreRefugio"),
                Propietario = GetPropietario(row)
            };
        }
    }
}

using System;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;

namespace DataLayer.Area.Animal
{
    public class AnimalDal : DaoBase
    {
        public AnimalDal(IDalSettings settings) : base(settings)
        {
        }

        public AnimalDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Registra informacion de muestras de animales.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="animal"></param>
        public void InsertAnimal(Model.Animal animal)
        {
            var objCommand = GetSqlCommand("pNLI_Animal");

            InputParameterAdd.Int(objCommand, "idRaza", animal.Raza.IdRaza);
            InputParameterAdd.Int(objCommand, "codigoOrigen", animal.Origen);
            InputParameterAdd.Varchar(objCommand, "nombresAnimal", animal.Nombre);
            InputParameterAdd.Int(objCommand, "generoAnimal", animal.Genero);
            InputParameterAdd.DateTime(objCommand, "fechaNacimiento", animal.FechaNacimiento);
            InputParameterAdd.Int(objCommand,"edadAnio",animal.EdadAnio);
            InputParameterAdd.Int(objCommand, "edadMes", animal.EdadMes);
            InputParameterAdd.Varchar(objCommand, "idUbigeoAnimal", animal.IdUbigeo);
            InputParameterAdd.Varchar(objCommand, "direccionAnimal", animal.Direccion);
            InputParameterAdd.Varchar(objCommand, "nombreRefugio", animal.Refugio);
            InputParameterAdd.Int(objCommand, "tipoDocumento", animal.Propietario?.TipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", animal.Propietario?.NroDocumento);
            InputParameterAdd.Varchar(objCommand, "apellidoPaterno", animal.Propietario?.ApellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apellidoMaterno", animal.Propietario?.ApellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombres", animal.Propietario?.Nombres);
            InputParameterAdd.Int(objCommand, "genero", animal.Propietario?.Genero);
            InputParameterAdd.Varchar(objCommand, "idUbigeoPropietario", animal.Propietario?.UbigeoActual.Id);
            InputParameterAdd.Varchar(objCommand, "direccionPropietario", animal.Propietario?.DireccionActual);
            InputParameterAdd.Varchar(objCommand, "telefonoFijo", animal.Propietario?.TelefonoFijo);
            InputParameterAdd.Varchar(objCommand, "celular", animal.Propietario?.Celular1);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", animal.IdUsuarioRegistro);
            OutputParameterAdd.UniqueIdentifier(objCommand,"idAnimal");

            ExecuteNonQuery(objCommand);

            animal.IdAnimal = OutputParameterGet.Value<Guid>(objCommand, "idAnimal");
        }
        /// <summary>
        /// Descripción: Actualiza informacion de muestras de animales.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="animal"></param>
        public void UpdateAnimal(Model.Animal animal)
        {
            var objCommand = GetSqlCommand("pNLU_Animal");

            InputParameterAdd.Guid(objCommand, "idAnimal", animal.IdAnimal);
            InputParameterAdd.Int(objCommand, "idRaza", animal.Raza.IdRaza);
            InputParameterAdd.Int(objCommand, "codigoOrigen", animal.Origen);
            InputParameterAdd.Varchar(objCommand, "nombresAnimal", animal.Nombre);
            InputParameterAdd.Int(objCommand, "generoAnimal", animal.Genero);
            InputParameterAdd.DateTime(objCommand, "fechaNacimiento", animal.FechaNacimiento);
            InputParameterAdd.Int(objCommand, "edadAnio", animal.EdadAnio);
            InputParameterAdd.Int(objCommand, "edadMes", animal.EdadMes);
            InputParameterAdd.Varchar(objCommand, "idUbigeoAnimal", animal.IdUbigeo);
            InputParameterAdd.Varchar(objCommand, "direccionAnimal", animal.Direccion);
            InputParameterAdd.Varchar(objCommand, "nombreRefugio", animal.Refugio);
            InputParameterAdd.Int(objCommand, "tipoDocumento", animal.Propietario?.TipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", animal.Propietario?.NroDocumento);
            InputParameterAdd.Varchar(objCommand, "apellidoPaterno", animal.Propietario?.ApellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apellidoMaterno", animal.Propietario?.ApellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombres", animal.Propietario?.Nombres);
            InputParameterAdd.Int(objCommand, "genero", animal.Propietario?.Genero);
            InputParameterAdd.Varchar(objCommand, "idUbigeoPropietario", animal.Propietario?.UbigeoActual.Id);
            InputParameterAdd.Varchar(objCommand, "direccionPropietario", animal.Propietario?.DireccionActual);
            InputParameterAdd.Varchar(objCommand, "telefonoFijo", animal.Propietario?.TelefonoFijo);
            InputParameterAdd.Varchar(objCommand, "celular", animal.Propietario?.Celular1);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", animal.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene informacion de muestras de animales.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <returns></returns>
        public List<Model.Animal> GetAnimales()
        {
            var objCommand = GetSqlCommand("pNLS_Animal");

            return AnimalConvertTo.Animales(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de muestras de animales por Id
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idAnimal"></param>
        /// <returns></returns>
        public Model.Animal GetAnimalById(Guid idAnimal)
        {
            var objCommand = GetSqlCommand("pNLS_AnimalById");
            InputParameterAdd.Guid(objCommand, "idAnimal", idAnimal);

            return AnimalConvertTo.Animal(Execute(objCommand));
        }
    }
}
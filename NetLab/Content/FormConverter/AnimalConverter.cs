using Model;
using NetLab.Controllers.FormConverter.Entities;
using NetLab.Controllers.FormConverter.Interfaces;
using System;

namespace NetLab.Controllers.FormConverter
{
    [Serializable]
    public class AnimalConverter : IAnimalConverter
    {
        /// <summary>
        /// Descripción: Convierte una entidad de tipo AnimalConverterRequest a Animal
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Animal ConvertFrom(AnimalConverterRequest request)
        {
            return new Animal
            {
                IdAnimal = request.AnimalViewModels.Animal.IdAnimal,
                Raza = new AnimalRaza { IdRaza = request.AnimalViewModels.Raza.IdRaza },
                Origen = request.AnimalViewModels.Origen.IdOrigen,
                Nombre = request.AnimalViewModels.Animal.Nombre,
                Genero = request.AnimalViewModels.Sexo.IdSexo,
                FechaNacimiento = request.AnimalViewModels.Animal.FechaNacimiento,
                EdadAnio = request.AnimalViewModels.Animal.EdadAnio,
                EdadMes = request.AnimalViewModels.Animal.EdadMes,
                IdUbigeo = GetUbigeo(request),
                Direccion = request.AnimalViewModels.Animal.Direccion,
                Refugio = request.AnimalViewModels.Animal.Refugio,
                Propietario = GetPropietario(request),
                IdUsuarioRegistro = request.IdUsuarioLogueado
            };
        }
        /// <summary>
        /// Descripción: Devuelve un la concatenacion de IdDepartamento , IdProvincia, IdDistrito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetUbigeo(AnimalConverterRequest request)
        {
            var ubigeo = request.AnimalViewModels;

            if (ubigeo.IdDepartamento == null || ubigeo.IdProvincia == null || ubigeo.IdDistrito == null)
                return null;

            return ubigeo.IdDepartamento + ubigeo.IdProvincia+ ubigeo.IdDistrito;
        }
        /// <summary>
        /// Descripción: Convierte una entidad de tipo AnimalConverterRequest a Paciente
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static Paciente GetPropietario(AnimalConverterRequest request)
        {
            if (request.AnimalViewModels.Animal.Propietario == null) return null;

            return new Paciente
            {
                TipoDocumento = request.AnimalViewModels.TipoDocumento.IdTipoDocumento,
                NroDocumento = request.AnimalViewModels.Animal.Propietario.NroDocumento,
                ApellidoPaterno = request.AnimalViewModels.Animal.Propietario.ApellidoPaterno,
                ApellidoMaterno = request.AnimalViewModels.Animal.Propietario.ApellidoMaterno,
                Nombres = request.AnimalViewModels.Animal.Propietario.Nombres,
                Genero = request.AnimalViewModels.Genero.IdGenero,
                UbigeoActual = GetUbigeoActual(request),
                DireccionActual = request.AnimalViewModels.Animal.Propietario.DireccionActual,
                TelefonoFijo = request.AnimalViewModels.Animal.Propietario.TelefonoFijo,
                Celular1 = request.AnimalViewModels.Animal.Propietario.Celular1
            };
        }
        /// <summary>
        /// Descripción: Devuelve un la concatenacion de DepartamentoProp , ProvinciaProp, DistritoProp
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static Ubigeo GetUbigeoActual(AnimalConverterRequest request)
        {
            var ubigeo = request.AnimalViewModels;

            return new Ubigeo()
            {
                Id = ubigeo.DepartamentoProp + ubigeo.ProvinciaProp+ ubigeo.DistritoProp
            };
        }
    }
}
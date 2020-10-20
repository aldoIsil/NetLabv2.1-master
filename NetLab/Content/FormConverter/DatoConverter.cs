using Model;
using NetLab.Controllers.FormConverter.Entities;
using NetLab.Controllers.FormConverter.Interfaces;
using System;

namespace NetLab.Controllers.FormConverter
{
    [Serializable]
    public class DatoConverter : IDatoConverter
    {
        /// <summary>
        /// Descripción: Convierte una entidad de tipo DatoConverterRequest a Dato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Dato ConvertFrom(DatoConverterRequest request)
        {
            return new Dato
            {
                IdDato = request.DatoViewModels.Dato.IdDato,
                Prefijo = request.DatoViewModels.Dato.Prefijo,
                Sufijo = request.DatoViewModels.Dato.Sufijo,
                IdTipo = request.DatoViewModels.Tipo.IdTipoDato,
                IdDatoDependiente = request.DatoViewModels.Dato.IdDatoDependiente,
                idProyecto= request.DatoViewModels.Dato.idProyecto,
                Visible = request.DatoViewModels.Dato.Visible,
                Obligatorio = request.DatoViewModels.Dato.Obligatorio,
                IdListaDato = request.DatoViewModels.Lista.IdListaDato,
                IdGenero = request.DatoViewModels.Clase.IdClase,
                Estado = request.DatoViewModels.Dato.Estado,
                IdUsuarioRegistro = request.IdUsuarioLogueado,
                IdUsuarioEdicion = request.IdUsuarioLogueado
            };
        }
    }
}
using Model;
using NetLab.Controllers.FormConverter.Entities;
using NetLab.Controllers.FormConverter.Interfaces;
using System;

namespace NetLab.Controllers.FormConverter
{
    [Serializable]
    public class CategoriaConverter : ICategoriaConverter
    {
        /// <summary>
        /// Descripción: Convierte una entidad de tipo CategoriaConverterRequest a CategoriaDato
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CategoriaDato ConvertFrom(CategoriaConverterRequest request)
        {
            return new CategoriaDato
            {
                IdCategoriaDato = request.CategoriaViewModels.Categoria.IdCategoriaDato,
                Nombre = request.CategoriaViewModels.Categoria.Nombre,
                Descripcion = request.CategoriaViewModels.Categoria.Descripcion,
                IdCategoriaDatoPadre = request.CategoriaViewModels.IdCategoriaPadre,
                IdGenero = request.CategoriaViewModels.Clase.IdClase,
                Visible = request.CategoriaViewModels.Categoria.Visible,
                Orientacion = request.CategoriaViewModels.Categoria.Orientacion,
                Estado = request.CategoriaViewModels.Categoria.Estado,
                IdUsuarioRegistro = request.IdUsuarioLogueado
            };
        }
    }
}
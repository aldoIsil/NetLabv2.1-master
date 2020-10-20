using System;
using Model;
using System.Collections.Generic;
using System.Data;
using BusinessLayer.DatoClinico.Interfaces;
using DataLayer.Area.DatoClinico;

namespace BusinessLayer.DatoClinico
{
    public class CategoriaDatoBl : ICategoriaDatoBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de los datos de la categoria Padre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <param name="idCategoriaPadre"></param>
        /// <returns></returns>
        public List<CategoriaDato> GetCategoriaByEnfermedad(string idEnfermedad, int? idCategoriaPadre)
        {
            using (var categoriaDatoDal = new CategoriaDatoDal())
            {
                return categoriaDatoDal.GetCategoriaByEnfermedad(idEnfermedad, idCategoriaPadre);
            }
        }
        /// <summary>
        /// Descripción: Registra información de la Categoria y Enfermedad Categoria Dato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="categoria"></param>
        /// <param name="idEnfermedad"></param>
        public void InsertCategoria(CategoriaDato categoria, string idEnfermedad)
        {
            using (var categoriaDatoDal = new CategoriaDatoDal())
            {
                categoriaDatoDal.InsertCategoria(categoria, idEnfermedad);
            }
        }
        /// <summary>
        /// Descripción: Registra información de la Categoria filtrado por el codigo de la mimsa tabla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CategoriaDato GetCategoriaById(int id)
        {
            using (var categoriaDatoDal = new CategoriaDatoDal())
            {
                return categoriaDatoDal.GetCategoriaById(id);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las enferedades/categorias filtrado por la categoria y la enfermedad
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public EnfermedadCategoriaDato GetEnfermedadCategoriaById(int id, string idEnfermedad)
        {
            using (var categoriaDatoDal = new CategoriaDatoDal())
            {
                return categoriaDatoDal.GetEnfermedadCategoriaById(id, idEnfermedad);
            }
        }
        /// <summary>
        /// Descripción: Actualiza informacion de los datos de la categoria.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="categoria"></param>
        public void UpdateCategoria(CategoriaDato categoria)
        {
            using (var categoriaDatoDal = new CategoriaDatoDal())
            {
                categoriaDatoDal.UpdateCategoria(categoria);
            }
        }
        /// <summary>
        /// Descripción: Actualiza informacion de los datos de la categoria y de las categorias/enfermedades.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="categoria"></param>
        /// <param name="enfermedadCategoria"></param>
        public void UpdateCategoria(CategoriaDato categoria, EnfermedadCategoriaDato enfermedadCategoria)
        {
            using (var categoriaDatoDal = new CategoriaDatoDal())
            {
                categoriaDatoDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    categoriaDatoDal.UpdateCategoria(categoria);
                    categoriaDatoDal.UpdateEnfermedadCategoria(enfermedadCategoria);

                    categoriaDatoDal.Commit();
                }
                catch (Exception)
                {
                    categoriaDatoDal.Rollback();
                }
            }
        }
    }
}

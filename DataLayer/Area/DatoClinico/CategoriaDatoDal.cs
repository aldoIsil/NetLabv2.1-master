using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using Model;

namespace DataLayer.Area.DatoClinico
{
    public class CategoriaDatoDal : DaoBase
    {
        public CategoriaDatoDal(IDalSettings settings) : base(settings)
        {
        }

        public CategoriaDatoDal() : this(new NetlabSettings())
        {
        }
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
            var objCommand = GetSqlCommand("pNLS_CategoriaDatoByIdEnfermedad");
            InputParameterAdd.Varchar(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Int(objCommand, "idCategoriaPadre", idCategoriaPadre);

            return CategoriaDatoConvertTo.Categorias(Execute(objCommand));
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
            var objCommand = GetSqlCommand("pNLI_CategoriaByEnfermedad");

            InputParameterAdd.Varchar(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Varchar(objCommand, "nombre", categoria.Nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", categoria.Descripcion);
            InputParameterAdd.Int(objCommand, "idCategoriaDatoPadre", categoria.IdCategoriaDatoPadre);
            InputParameterAdd.Int(objCommand, "idGenero", categoria.IdGenero);
            InputParameterAdd.Int(objCommand, "visible", System.Convert.ToInt32(categoria.Visible));
            InputParameterAdd.Int(objCommand, "orientacion", categoria.Orientacion);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", categoria.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
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
            var objCommand = GetSqlCommand("pNLS_CategoriaById");
            InputParameterAdd.Int(objCommand, "idCategoriaDato", id);

            return CategoriaDatoConvertTo.Categoria(Execute(objCommand));
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
            var objCommand = GetSqlCommand("pNLS_EnfermedadCategoriaById");
            InputParameterAdd.Int(objCommand, "idCategoriaDato", id);
            InputParameterAdd.Varchar(objCommand, "idEnfermedad", idEnfermedad);

            return EnfermedadCategoriaDatoConvertTo.EnfermedadCategoria(Execute(objCommand));
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
            var objCommand = GetSqlCommand("pNLU_Categoria");

            InputParameterAdd.Int(objCommand, "idCategoriaDato", categoria.IdCategoriaDato);
            InputParameterAdd.Varchar(objCommand, "nombre", categoria.Nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", categoria.Descripcion);
            InputParameterAdd.Int(objCommand, "idGenero", categoria.IdGenero);
            InputParameterAdd.Int(objCommand, "visible", System.Convert.ToInt32(categoria.Visible));
            InputParameterAdd.Int(objCommand, "orientacion", categoria.Orientacion);
            InputParameterAdd.Int(objCommand, "estado", categoria.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", categoria.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza informacion de los datos de la categoria/enfermedad.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="enfermedadCategoria"></param>
        public void UpdateEnfermedadCategoria(EnfermedadCategoriaDato enfermedadCategoria)
        {
            var objCommand = GetSqlCommand("pNLU_EnfermedadCategoria");

            InputParameterAdd.Int(objCommand, "idCategoriaDato", enfermedadCategoria.IdCategoriaDato);
            InputParameterAdd.Int(objCommand, "idEnfermedad", enfermedadCategoria.IdEnfermedad);
            InputParameterAdd.Int(objCommand, "orden", enfermedadCategoria.Orden);
            InputParameterAdd.Int(objCommand, "estado", enfermedadCategoria.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", enfermedadCategoria.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
    }
}

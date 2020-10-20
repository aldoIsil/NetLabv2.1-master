using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer.Area.DatoClinico
{
    public class DatoClinicoDal : DaoBase
    {
        public DatoClinicoDal(IDalSettings settings) : base(settings)
        {
        }

        public DatoClinicoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene información de la Categoria y Enfermedad Categoria Dato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public List<CategoriaDato> GetCategoriaByEnfermedad(int idEnfermedad)
        {
            var objCommand = GetSqlCommand("pNLS_CategoriaByEnfermedad");
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);

            return CategoriaDatoConvertTo.Categorias(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los datos clinicos por categoria
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idCategoria"></param>
        /// <returns></returns>
        public List<Dato> GetDatoByCategoria(int idCategoria, string idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_DatoByCategoria");
            InputParameterAdd.Int(objCommand, "idCategoriaDato", idCategoria);
            InputParameterAdd.Varchar(objCommand, "idExamen", idExamen);

            return DatoConvertTo.Datos(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene los tipo de datos activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<TipoDato> GetTipoDato()
        {
            var objCommand = GetSqlCommand("pNLS_TipoDato");

            return TipoDatoConvertTo.TipoDatos(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene los tipo de datos activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        public TipoDato GetTipoDatoById(int idTipo)
        {
            var objCommand = GetSqlCommand("pNLS_TipoDatoById");
            InputParameterAdd.Int(objCommand, "idTipoDato", idTipo);

            return TipoDatoConvertTo.TipoDato(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registra los datos por categoria.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dato"></param>
        /// <param name="idCategoria"></param>
        public void InsertDato(Dato dato, int idCategoria)
        {
            var objCommand = GetSqlCommand("pNLI_DatoByCategoria");

            InputParameterAdd.Int(objCommand, "idCategoria", idCategoria);
            InputParameterAdd.Varchar(objCommand, "prefijo", dato.Prefijo);
            InputParameterAdd.Varchar(objCommand, "sufijo", dato.Sufijo);
            InputParameterAdd.Int(objCommand, "idTipo", dato.IdTipo);
            InputParameterAdd.Int(objCommand, "idDatoDependiente", dato.IdDatoDependiente);
            InputParameterAdd.Int(objCommand, "idProyecto", string.IsNullOrEmpty(dato.idProyecto.ToString()) ? 0 : dato.idProyecto);
            InputParameterAdd.Int(objCommand, "visible", System.Convert.ToInt32(dato.Visible));
            InputParameterAdd.Int(objCommand, "obligatorio", System.Convert.ToInt32(dato.Obligatorio));
            InputParameterAdd.Int(objCommand, "idLista", dato.IdListaDato);
            InputParameterAdd.Int(objCommand, "idGenero", dato.IdGenero);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", dato.IdUsuarioRegistro);
            InputParameterAdd.Varchar(objCommand, "idsExamen", dato.IdsExamen);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene los datos filtrado por su codigo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dato GetDatoById(int id)
        {
            var objCommand = GetSqlCommand("pNLS_DatoById");
            InputParameterAdd.Int(objCommand, "idDato", id);

            return DatoConvertTo.Dato(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los datos y categorias enlazadas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idCategoria"></param>
        /// <returns></returns>
        public DatoCategoriaDato GetDatoCategoriaById(int id, int idCategoria)
        {
            var objCommand = GetSqlCommand("pNLS_DatoCategoriaById");
            InputParameterAdd.Int(objCommand, "idDato", id);
            InputParameterAdd.Int(objCommand, "idCategoriaDato", idCategoria);

            return DatoCategoriaDatoConvertTo.DatoCategoria(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Actualiza informacion de los Datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dato"></param>
        public void UpdateDato(Dato dato)
        {
            var objCommand = GetSqlCommand("pNLU_Dato");

            InputParameterAdd.Int(objCommand, "idDato", dato.IdDato);
            InputParameterAdd.Varchar(objCommand, "prefijo", dato.Prefijo);
            InputParameterAdd.Varchar(objCommand, "sufijo", dato.Sufijo);
            InputParameterAdd.Int(objCommand, "idTipo", dato.IdTipo);
            InputParameterAdd.Int(objCommand, "idDatoDependiente", dato.IdDatoDependiente);
            InputParameterAdd.Int(objCommand, "idProyecto", string.IsNullOrEmpty(dato.idProyecto.ToString())? 0 : dato.idProyecto);
            InputParameterAdd.Int(objCommand, "visible", System.Convert.ToInt32(dato.Visible));
            InputParameterAdd.Int(objCommand, "obligatorio", System.Convert.ToInt32(dato.Obligatorio));
            InputParameterAdd.Int(objCommand, "idListaDato", dato.IdListaDato);
            InputParameterAdd.Int(objCommand, "idGenero", dato.IdGenero);
            InputParameterAdd.Int(objCommand, "estado", dato.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", dato.IdUsuarioRegistro);
            InputParameterAdd.Varchar(objCommand, "idsExamen", dato.IdsExamen);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza informacion de los Datos/Categorias.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="datoCategoria"></param>
        public void UpdateDatoCategoria(DatoCategoriaDato datoCategoria)
        {
            var objCommand = GetSqlCommand("pNLU_DatoCategoria");

            InputParameterAdd.Int(objCommand, "idDato", datoCategoria.IdDato);
            InputParameterAdd.Int(objCommand, "idCategoriaDato", datoCategoria.IdCategoriaDato);
            InputParameterAdd.Int(objCommand, "orden", datoCategoria.Orden);
            InputParameterAdd.Int(objCommand, "estado", datoCategoria.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", datoCategoria.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
    }
}
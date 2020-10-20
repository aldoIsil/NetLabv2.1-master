using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;

namespace DataLayer
{
    public class AnalitoOpcionDal : DaoBase
    {
        public AnalitoOpcionDal(IDalSettings settings) : base(settings)
        {
        }

        public AnalitoOpcionDal() : this(new NetlabSettings())
        {
        }

        /// <summary>
        /// Descripción: Obtiene información de AnalitosOpcion de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoOpcion"></param>
        /// <returns></returns>
        public AnalitoOpcionResultado GetAnalitoOpcionById(Guid idAnalito, int idAnalitoOpcion)
        {
            var objCommand = GetSqlCommand("pNLS_AnalitoOpcionById");
            InputParameterAdd.Guid(objCommand, "idAnalito", idAnalito);
            InputParameterAdd.Int(objCommand, "idAnalitoOpcion", idAnalitoOpcion);

            DataTable dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new AnalitoOpcionResultado
            {
                IdAnalito = Converter.GetGuid(row, "idAnalito"),
                IdAnalitoOpcionResultado = Converter.GetInt(row, "IdAnalitoOpcionResultado"),
                Glosa = Converter.GetString(row, "glosa"),
                IdAnalitoOpcionParent = Converter.GetString(row, "idOpcionParent"),
                Orden = Converter.GetInt(row, "ordenOpcR"),
                Tipo = Converter.GetInt(row, "tipo"),
                Estado = Converter.GetInt(row, "estado")
            };
        }
        /// <summary>
        /// Descripción: Obtiene las opciones ingresados por cada analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public List<AnalitoOpcionResultado> GetAnalitoOpcionByAnalito(Guid idAnalito)
        {
            var objCommand = GetSqlCommand("pNLS_OpcionesByAnalito");
            InputParameterAdd.Guid(objCommand, "idAnalito", idAnalito);
            DataTable dataTable = Execute(objCommand);
            List<AnalitoOpcionResultado> lista = new List<AnalitoOpcionResultado>();

            foreach (DataRow row in dataTable.Rows)
            {
                AnalitoOpcionResultado analito = new AnalitoOpcionResultado
                {
                    IdAnalito = Converter.GetGuid(row, "idAnalito"),
                    IdAnalitoOpcionResultado = Converter.GetInt(row, "IdAnalitoOpcionResultado"),
                    Glosa = Converter.GetString(row, "glosa"),
                    Orden = Converter.GetInt(row, "ordenOpcR"),
                    Estado = Converter.GetInt(row, "estado"),
                    IdAnalitoOpcionParent = Converter.GetString(row, "idOpcionParent"),
                    Tipo = Converter.GetInt(row,"tipo"),
                    Plataforma = Converter.GetString(row, "Platafroma")
                };
                analito.TipoRespuesta = new ListaDetalle();
                analito.TipoRespuesta.IdDetalleLista = analito.Tipo;
                analito.TipoRespuesta.Glosa = Converter.GetString(row, "TipoRespuesta").ToUpper();
                lista.Add(analito);
            }
            return lista;
        }
        /// <summary>
        /// Descripción: Registra información de Analitos y sus opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcion"></param>
        public void InsertAnalitoOpcion(AnalitoOpcionResultado opcion)
        {
            var objCommand = GetSqlCommand("pNLI_AnalitoOpcion");

            InputParameterAdd.Guid(objCommand, "idAnalito", opcion.IdAnalito);
            //InputParameterAdd.Varchar(objCommand, "glosa", opcion.Glosa.ToUpper());
            InputParameterAdd.Varchar(objCommand, "glosa", opcion.Glosa);
            InputParameterAdd.Int(objCommand, "orden", opcion.Orden);
            InputParameterAdd.Varchar(objCommand, "idopcionParent", opcion.IdAnalitoOpcionParent);
            InputParameterAdd.Int(objCommand, "idTipo", opcion.Tipo);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", opcion.IdUsuarioRegistro);
            InputParameterAdd.Varchar(objCommand, "plataforma", opcion.Plataforma);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza información de Analitos y sus opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcion"></param>
        public void UpdateAnalitoOpcion(AnalitoOpcionResultado opcion)
        {
            var objCommand = GetSqlCommand("pNLU_AnalitoOpcion");

            InputParameterAdd.Guid(objCommand, "idAnalito", opcion.IdAnalito);
            InputParameterAdd.Int(objCommand, "idAnalitoOpcion", opcion.IdAnalitoOpcionResultado);
            //InputParameterAdd.Varchar(objCommand, "glosa", opcion.Glosa.ToUpper());
            InputParameterAdd.Varchar(objCommand, "glosa", opcion.Glosa);
            InputParameterAdd.Int(objCommand, "orden", opcion.Orden);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", opcion.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", opcion.Estado);
            InputParameterAdd.Varchar(objCommand, "idopcionParent", opcion.IdAnalitoOpcionParent);
            InputParameterAdd.Int(objCommand, "idTipo", opcion.Tipo);
            InputParameterAdd.Varchar(objCommand, "plataforma", opcion.Plataforma);
            ExecuteNonQuery(objCommand);
        }

    }
}

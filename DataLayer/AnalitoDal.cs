using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;

namespace DataLayer
{
    public class AnalitoDal : DaoBase
    {
        public AnalitoDal(IDalSettings settings) : base(settings)
        {
        }

        public AnalitoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obteniene todos los analitos activos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Analito> GetAnalitos()
        {
            var objCommand = GetSqlCommand("pNLS_Analito");
            DataTable dataTable = Execute(objCommand);
            List<Analito> analitoList = new List<Analito>();

            foreach (DataRow row in dataTable.Rows)
            {
                Analito analito = new Analito
                {
                    IdAnalito = Converter.GetGuid(row, "idAnalito"),
                    Nombre = Converter.GetString(row, "nombre"),
                    Descripcion = Converter.GetString(row, "descripcion")
                };
                analitoList.Add(analito);
            }
            return analitoList;
        }
        /// <summary>
        /// Descripción: Obteniene todos los analitos activos filtrado por nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Analito> GetAnalitos(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_AnalitoByNombre");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);
            DataTable dataTable = Execute(objCommand);
            List<Analito> analitoList = new List<Analito>();

            foreach (DataRow row in dataTable.Rows)
            {
                Analito analito = new Analito
                {
                    IdAnalito = Converter.GetGuid(row, "idAnalito"),
                    Nombre = Converter.GetString(row, "nombre"),
                    Descripcion = Converter.GetString(row, "descripcion")
                };
                analitoList.Add(analito);
            }
            return analitoList;
        }
        /// <summary>
        /// Descripción: Obteniene todos los analitos activos filtrado por Codigo del examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<Analito> GetAnalitosByIdExamen(Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_AnalitosByIdExamen");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            DataTable dataTable = Execute(objCommand);
            List<Analito> analitoList = new List<Analito>();

            foreach (DataRow row in dataTable.Rows)
            {
                Analito analito = new Analito
                {
                    IdAnalito = Converter.GetGuid(row, "idAnalito"),
                };
                analitoList.Add(analito);
            }
            return analitoList;
        }

        /// <summary>
        /// Descripción: Obteniene todos los analitos activos filtrado por el codigo del analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public Analito GetAnalitoById(Guid idAnalito)
        {
            var objCommand = GetSqlCommand("pNLS_AnalitoById");
            InputParameterAdd.Guid(objCommand, "idAnalito", idAnalito);

            DataTable dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new Analito
            {
                IdAnalito = Converter.GetGuid(row, "idAnalito"),
                Nombre = Converter.GetString(row, "nombre"),
                Descripcion = Converter.GetString(row, "descripcion"),
                Tipo = Converter.GetInt(row, "tipo"),
                IdListaUnidad = Converter.GetInt(row, "idListaUnidad"),

                Estado = Converter.GetInt(row, "estado")
            };
        }
        /// <summary>
        /// Descripción: Obtener información de Analitos filtrado por nombre y/o descripcion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Analito> SearchAnalitos(string search)
        {
            var objCommand = GetSqlCommand("pNLS_BuscarAnalito");
            InputParameterAdd.Varchar(objCommand, "search", "%" + search + "%");
            DataTable dataTable = Execute(objCommand);
            List<Analito> analitoList = new List<Analito>();

            foreach (DataRow row in dataTable.Rows)
            {
                Analito analito = new Analito
                {
                    IdAnalito = Converter.GetGuid(row, "idAnalito"),
                    Nombre = Converter.GetString(row, "nombre"),
                    Descripcion = Converter.GetString(row, "descripcion"),
                    Tipo = Converter.GetInt(row, "tipo"),
                    Estado = Converter.GetInt(row, "estado"),
                    IdListaUnidad = Converter.GetInt(row, "idListaUnidad")
                };

                analito.Unidad = new ListaDetalle();
                analito.TipoRespuesta = new ListaDetalle();

                analito.Unidad.IdDetalleLista = analito.IdListaUnidad;
                analito.Unidad.Glosa = Converter.GetString(row, "Unidad");
                analito.TipoRespuesta.IdDetalleLista = analito.Tipo;
                analito.TipoRespuesta.Glosa = Converter.GetString(row, "TipoRespuesta").ToUpper();
                analitoList.Add(analito);
            }
            return analitoList;
        }
        /// <summary>
        /// Descripción: Registra información de Analitos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="analito"></param>
        public void InsertAnalito(Analito analito)
        {
            var objCommand = GetSqlCommand("pNLI_Analito");

            InputParameterAdd.Varchar(objCommand, "nombre", analito.Nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", analito.Descripcion);
            InputParameterAdd.Int(objCommand, "tipo", analito.Tipo);
            InputParameterAdd.Int(objCommand, "idListaUnidad", analito.IdListaUnidad);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", analito.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza información de Analitos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="analito"></param>
        public void UpdateAnalito(Analito analito)
        {
            var objCommand = GetSqlCommand("pNLU_Analito");

            InputParameterAdd.Guid(objCommand, "idAnalito", analito.IdAnalito);
            InputParameterAdd.Varchar(objCommand, "nombre", analito.Nombre.ToUpper());
            InputParameterAdd.Varchar(objCommand, "descripcion", analito.Descripcion.ToUpper());
            InputParameterAdd.Int(objCommand, "tipo", analito.Tipo);
            InputParameterAdd.Int(objCommand, "idListaUnidad", analito.IdListaUnidad);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", analito.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", analito.Estado);

            ExecuteNonQuery(objCommand);
        }

    }
}

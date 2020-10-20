using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;

namespace DataLayer
{
    public class AreaProcesamientoDal : DaoBase
    {
        public AreaProcesamientoDal(IDalSettings settings) : base(settings)
        {
        }

        public AreaProcesamientoDal() : this(new NetlabSettings())
        {
        }

        /// <summary>
        /// Descripción: Obtiene informacion de las areas de procesamiento correspondiente a una Orden de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<AreaProcesamiento> GetByOrdenUser(Guid idOrden, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_AreaProcesamientoOrdenUsuario");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            DataTable dataTable = Execute(objCommand);
            List<AreaProcesamiento> list = new List<AreaProcesamiento>();

            foreach (DataRow row in dataTable.Rows)
            {
                AreaProcesamiento item = new AreaProcesamiento
                {
                    IdAreaProcesamiento = Converter.GetInt(row, "idAreaProcesamiento"), 
                    Nombre = Converter.GetString(row, "nombre"), 
                    Descripcion = Converter.GetString(row, "descripcion"), 
                    Sigla = Converter.GetString(row, "sigla") 
                };
                list.Add(item);
            }
            return list;
        }
        /// <summary>
        /// Descripción: Obtiene información de Analitos por el Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
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
                Nombre = Converter.GetString(row,"nombre"),
                Descripcion = Converter.GetString(row, "descripcion"),
                Estado = Converter.GetInt(row, "estado")
            };
        }

        



    }
}

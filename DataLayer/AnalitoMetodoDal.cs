using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;

namespace DataLayer
{
    public class AnalitoMetodoDal : DaoBase
    {
        public AnalitoMetodoDal(IDalSettings settings) : base(settings)
        {
        }

        public AnalitoMetodoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene información de los metodos de un Analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public List<ExamenMetodo> GetAnalitoMetodoByAnalito(Guid idAnalito)
        {
            var objCommand = GetSqlCommand("pNLS_MetodosByAnalito");
            InputParameterAdd.Guid(objCommand, "idAnalito", idAnalito);
            DataTable dataTable = Execute(objCommand);
            List<ExamenMetodo> lista = new List<ExamenMetodo>();

            foreach (DataRow row in dataTable.Rows)
            {
                ExamenMetodo analito = new ExamenMetodo
                {
                    IdAnalito = Converter.GetGuid(row, "idAnalito"),
                    IdAnalitoMetodo = Converter.GetInt(row, "idAnalitoMetodo"),
                    Glosa1 = Converter.GetString(row, "glosa1"),
                    Glosa2 = Converter.GetString(row, "glosa2"),
                    Orden = Converter.GetInt(row, "ordenMetodo"),
                };

                lista.Add(analito);
            }
            return lista;
        }
        /// <summary>
        /// Descripción: Obtiene información de los metodos de un Analito
        /// No existe el SP y el metodo no se utiliza!!!!!
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="metodo"></param>
        public void InsertAnalitoMetodo(ExamenMetodo metodo)
        {
            var objCommand = GetSqlCommand("pNLI_AnalitoMetodo");

            InputParameterAdd.Guid(objCommand, "idAnalito", metodo.IdAnalito);
            InputParameterAdd.Varchar(objCommand, "glosa1", metodo.Glosa1.ToUpper());
            InputParameterAdd.Varchar(objCommand, "glosa2", metodo.Glosa2.ToUpper());
            InputParameterAdd.Int(objCommand, "orden", metodo.Orden);
            
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", metodo.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        ///  Descripción: Modifica datos del metodos de un analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="metodo"></param>
        public void UpdateAnalitoMetodo(ExamenMetodo metodo)
        {
            var objCommand = GetSqlCommand("pNLU_AnalitoMetodo");

            InputParameterAdd.Guid(objCommand, "idAnalito", metodo.IdAnalito);
            InputParameterAdd.Int(objCommand, "idAnalitoMetodo", metodo.IdAnalitoMetodo);
            InputParameterAdd.Varchar(objCommand, "glosa1", metodo.Glosa1.ToUpper());
            InputParameterAdd.Varchar(objCommand, "glosa2", metodo.Glosa2.ToUpper());            
            InputParameterAdd.Int(objCommand, "orden", metodo.Orden);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", metodo.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", metodo.Estado);

            ExecuteNonQuery(objCommand);
        }

    }
}

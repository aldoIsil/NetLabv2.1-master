using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;

namespace DataLayer
{
    public class DatoDal : DaoBase
    {
        public DatoDal(IDalSettings settings) : base(settings)
        {
        }

        public DatoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las opciones de datos por examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Dato> GetDatos()
        {
            var objCommand = GetSqlCommand("pNLS_Dato");
            DataTable dataTable = Execute(objCommand);
            List<Dato> datoList = new List<Dato>();

            foreach (DataRow row in dataTable.Rows)
            {
                Dato dato = new Dato
                {
                    IdDato = Converter.GetInt(row, "idDato"),
                    Prefijo = Converter.GetString(row,"prefijo"),
                    Sufijo = Converter.GetString(row, "sufijo"),
                    IdTipo = Converter.GetInt(row,"idTipo")
                };
                datoList.Add(dato);
            }
            return datoList;
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las opciones de datos por examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public List<Dato> GetDatosByIdEnfermedad(String idEnfermedad)
        {
            var objCommand = GetSqlCommand("pNLS_DatoByIdEnfermedad");
            InputParameterAdd.Varchar(objCommand, "idEnfermedad", idEnfermedad);
            DataTable dataTable = Execute(objCommand);
            List<Dato> datoList = new List<Dato>();

            foreach (DataRow row in dataTable.Rows)
            {
                Dato dato = new Dato
                {
                    IdDato = Converter.GetInt(row, "idDato"),
                    Prefijo = Converter.GetString(row, "prefijo"),
                    Sufijo = Converter.GetString(row, "sufijo"),
                    IdTipo = Converter.GetInt(row, "idTipo")
                };
                datoList.Add(dato);
            }
            return datoList;
        }


        
    }
}

using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;
using DataLayer.DalConverter;

namespace DataLayer
{
    public class SolicitanteDal : DaoBase
    {
        public SolicitanteDal(IDalSettings settings) : base(settings)
        {
        }

        public SolicitanteDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Registra nuevo solicitante de una orden
        /// Author: Juan Muga.
        /// Fecha Creacion: 05/06/2017
        /// Fecha Modificación: 
        /// Modificación:
        /// </summary>
        /// <param name="oSolicitante"></param>
        public int InsertSolicitante(OrdenSolicitante oSolicitante)
        {
            int valor = 0;
            var objCommand = GetSqlCommand("pNLI_Solicitante");
            InputParameterAdd.Int(objCommand, "codigoColegio", oSolicitante.codigoColegio);
            InputParameterAdd.Varchar(objCommand, "ApeMat", oSolicitante.apellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "ApePat", oSolicitante.apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "Nombres", oSolicitante.Nombres);
            InputParameterAdd.Varchar(objCommand, "Correo", oSolicitante.correo);
            InputParameterAdd.Varchar(objCommand, "idEstablecimiento", oSolicitante.CodigoUnico);
            InputParameterAdd.Varchar(objCommand, "numeroDni", oSolicitante.Dni);
            InputParameterAdd.Int(objCommand, "profesion", oSolicitante.idProfesion);
            InputParameterAdd.Varchar(objCommand, "telefono", oSolicitante.telefonoContacto);
            InputParameterAdd.Int(objCommand, "idUsuario", oSolicitante.idUsuarioRegistro);
            OutputParameterAdd.Int(objCommand,"valor");
            ExecuteNonQuery(objCommand);
            valor = (int)objCommand.Parameters["@valor"].Value;
            return valor;
        }
        public bool GetCodigoSolicitante(string idSolicitante)
        {
            //bool Resultado = false;
            var objCommand = GetSqlCommand("pNLS_SolicitantePorIdSolicitante");
            InputParameterAdd.Varchar(objCommand, "idSolicitante", idSolicitante);
            DataTable dataTable = Execute(objCommand);
            return dataTable.Rows.Count > 0;
        }

        public bool GetCodigoSolicitantePorCodigo(string CodigoColegio)
        {
            //bool Resultado = false;
            var objCommand = GetSqlCommand("pNLS_SolicitantePorCodigoColegio");
            InputParameterAdd.Varchar(objCommand, "CodigoColegio", CodigoColegio);
            DataTable dataTable = Execute(objCommand);
            return dataTable.Rows.Count > 0;
        }

        public List<OrdenSolicitante> GetListaSOlicitante(string textoBusqueda)
        {
            List<OrdenSolicitante> result = new List<OrdenSolicitante>();
            var objCommand = GetSqlCommand("pNLS_Solicitantes");
            InputParameterAdd.Varchar(objCommand, "textoBusqueda", textoBusqueda);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                OrdenSolicitante solicitante = new OrdenSolicitante
                {
                    idSolicitante = Converter.GetInt(row, "IdSolicitnte"),
                    apellidoPaterno = Converter.GetString(row, "ApePat"),
                    apellidoMaterno = Converter.GetString(row, "ApeMat"),
                    codigoColegio = Converter.GetInt(row, "codigoColegio"),
                    Nombres = Converter.GetString(row, "Nombres")
                };

                result.Add(solicitante);
            }

            return result;
        }

        public string GetSolicitanteById(int idSolicitante)
        {
            string result = string.Empty;
            var objCommand = GetSqlCommand("pNLS_SolicitanteByIdSolicitante");
            InputParameterAdd.Int(objCommand, "idSolicitante", idSolicitante);
            DataTable dataTable = Execute(objCommand);
            foreach (DataRow item in dataTable.Rows)
            {
                result = Converter.GetString(item, "solicitante");
            }

            return result;
        }

        public List<OrdenSolicitante> BuscarSolicitante(string cadena)
        {
            List<OrdenSolicitante> result = new List<OrdenSolicitante>();
            var objCommand = GetSqlCommand("pNLM_MantenedorSolicitante");
            InputParameterAdd.Int(objCommand, "iOperacion", 1);
            InputParameterAdd.Varchar(objCommand, "cadena", cadena);
            DataTable dataTable = Execute(objCommand);
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {

                    OrdenSolicitante solicitante = new OrdenSolicitante
                    {
                        idSolicitante = Converter.GetInt(row, "IdSolicitnte"),
                        CodigoUnico = Converter.GetString(row, "CodigoColegio"),
                        Nombres = Converter.GetString(row, "Nombres")
                    };
                    result.Add(solicitante);
                }
            }
            return result;
        }

        public OrdenSolicitante ListarSolicitante(int idSolicitante)
        {
            var objCommand = GetSqlCommand("pNLM_MantenedorSolicitante");
            InputParameterAdd.Int(objCommand, "iOperacion", 3);
            InputParameterAdd.Int(objCommand, "idSolicitante", idSolicitante);

            DataTable dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new OrdenSolicitante
            {
                idSolicitante = Converter.GetInt(row, "IdSolicitnte"),
                CodigoUnico = Converter.GetString(row, "CodigoColegio"),
                Nombres = Converter.GetString(row, "Nombres"),
                apellidoPaterno = Converter.GetString(row, "ApePat"),
                apellidoMaterno = Converter.GetString(row, "ApeMat"),
                correo = Converter.GetString(row, "Correo"),
                Dni = Converter.GetString(row, "numeroDNI"),
                Establecimiento = Converter.GetString(row, "nombre"),
                idProfesion = Converter.GetInt(row, "idProfesion"),
                telefonoContacto = Converter.GetString(row, "Telefono"),
                idEstablecimiento = Converter.GetInt(row, "idEstablecimiento")
            };
        }

        public void UpdateSolicitante(int id, string codigoColegio, string Nombre, string ApePat, string ApeMat, 
            string Correo, string dni, string telefono, int profesion, int idEstablecimiento, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLM_MantenedorSolicitante");
            InputParameterAdd.Int(objCommand, "iOperacion", 2);
            InputParameterAdd.Int(objCommand, "idSolicitante", id);
            InputParameterAdd.Varchar(objCommand, "CodigoColegio", codigoColegio);
            InputParameterAdd.Varchar(objCommand, "Nombres", Nombre);
            InputParameterAdd.Varchar(objCommand, "ApePat", ApePat);
            InputParameterAdd.Varchar(objCommand, "ApeMat", ApeMat);
            InputParameterAdd.Varchar(objCommand, "Correo", Correo);
            InputParameterAdd.Varchar(objCommand, "numeroDNI", dni);
            InputParameterAdd.Int(objCommand, "idProfesion", profesion);
            InputParameterAdd.Varchar(objCommand, "Telefono", telefono);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            ExecuteNonQuery(objCommand);
        }
    }
}

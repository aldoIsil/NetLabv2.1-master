using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;

namespace DataLayer
{
    public class AnalitoValorDal : DaoBase
    {
        public AnalitoValorDal(IDalSettings settings) : base(settings)
        {
        }

        public AnalitoValorDal() : this(new NetlabSettings())
        {
        }

        /// <summary>
        /// Descripción: Obtener información de los valores standar de los Analitos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoValor"></param>
        /// <returns></returns>
        public AnalitoValorNormal GetAnalitoValorById(Guid idAnalito, int idAnalitoValor)
        {
            var objCommand = GetSqlCommand("pNLS_AnalitoValorById");
            InputParameterAdd.Guid(objCommand, "idAnalito", idAnalito);
            InputParameterAdd.Int(objCommand, "idAnalitoValor", idAnalitoValor);

            DataTable dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new AnalitoValorNormal
            {
                idAnalito = Converter.GetGuid(row, "idAnalito"),
                idAnalitoValorNormal = Converter.GetInt(row, "IdAnalitoValorNormal"),
                glosa = Converter.GetString(row, "glosa"),
                orden = Converter.GetInt(row, "orden"),
                edadInferior = Converter.GetInt(row, "edadInferior"),
                edadSuperior = Converter.GetInt(row, "edadSuperior"),
                valorInferior = Converter.GetDecimal(row, "valorInferior"),
                valorSuperior = Converter.GetDecimal(row, "valorSuperior"),
                valorAlarmaInferior = Converter.GetDecimal(row, "valorAlarmaInferior"),
                valorAlarmaSuperior = Converter.GetDecimal(row, "valorAlarmaSuperior"),
                grupoGenero = Converter.GetInt(row, "grupoGenero"),
                //idLista = Converter.GetInt(row, "idLista"),

                Estado = Converter.GetInt(row, "estado")
            };
        }
        /// <summary>
        /// Descripción: Obtiene información de los valores de los Analitos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public List<AnalitoValorNormal> GetAnalitoValorByAnalito(Guid idAnalito)
        {
            var objCommand = GetSqlCommand("pNLS_ValoresByAnalito");
            InputParameterAdd.Guid(objCommand, "idAnalito", idAnalito);
            DataTable dataTable = Execute(objCommand);
            List<AnalitoValorNormal> lista = new List<AnalitoValorNormal>();

            foreach (DataRow row in dataTable.Rows)
            {
                AnalitoValorNormal analito = new AnalitoValorNormal
                {
                    idAnalito = Converter.GetGuid(row, "idAnalito"),
                    idAnalitoValorNormal = Converter.GetInt(row, "IdAnalitoValorNormal"),
                    glosa = Converter.GetString(row, "glosa"),
                    orden = Converter.GetInt(row, "orden"),
                    edadInferior = Converter.GetInt(row, "edadInferior"),
                    edadSuperior = Converter.GetInt(row, "edadSuperior"),
                    valorInferior = Converter.GetDecimal(row, "valorInferior"),
                    valorSuperior = Converter.GetDecimal(row, "valorSuperior"),
                    valorAlarmaInferior = Converter.GetDecimal(row, "valorAlarmaInferior"),
                    valorAlarmaSuperior = Converter.GetDecimal(row, "valorAlarmaSuperior"),
                    grupoGenero = Converter.GetInt(row, "grupoGenero"),
                    //idLista = Converter.GetInt(row, "idLista"),
                    Genero = new ListaDetalle
                    {
                        IdDetalleLista = Converter.GetInt(row, "grupoGenero"),
                        Glosa = Converter.GetString(row, "Genero"),
                    },
                    Estado = Converter.GetInt(row, "estado")
                };

                lista.Add(analito);
            }
            return lista;
        }
        /// <summary>
        /// Descripción: Registra información de los valores normales del resultado de un Analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="Valor"></param>
        public void InsertAnalitoValor(AnalitoValorNormal Valor)
        {
            var objCommand = GetSqlCommand("pNLI_AnalitoValor");
            
            InputParameterAdd.Guid(objCommand, "idAnalito", Valor.idAnalito);
            //InputParameterAdd.Int(objCommand, "@idLista", Valor.idLista);
            InputParameterAdd.Int(objCommand, "grupoGenero", Valor.grupoGenero);
            InputParameterAdd.Decimal(objCommand, "valorInferior", Valor.valorInferior);
            InputParameterAdd.Decimal(objCommand, "valorSuperior", Valor.valorSuperior);
            InputParameterAdd.Decimal(objCommand, "valorAlarmaInferior", Valor.valorAlarmaInferior);
            InputParameterAdd.Decimal(objCommand, "valorAlarmaSuperior", Valor.valorAlarmaSuperior);
            InputParameterAdd.Int(objCommand, "edadInferior", Valor.edadInferior);
            InputParameterAdd.Int(objCommand, "edadSuperior", Valor.edadSuperior);
            InputParameterAdd.Varchar(objCommand, "glosa", Valor.glosa.ToUpper());
            InputParameterAdd.Int(objCommand, "orden", Valor.orden);
            
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", Valor.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza información de los valores normales del resultado de un Analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="Valor"></param>
        public void UpdateAnalitoValor(AnalitoValorNormal Valor)
        {
            var objCommand = GetSqlCommand("pNLU_AnalitoValor");

            InputParameterAdd.Guid(objCommand, "idAnalito", Valor.idAnalito);
            //InputParameterAdd.Int(objCommand, "@idLista", Valor.idLista);
            InputParameterAdd.Int(objCommand, "idAnalitoValor", Valor.idAnalitoValorNormal);
            InputParameterAdd.Int(objCommand, "grupoGenero", Valor.grupoGenero);
            InputParameterAdd.Decimal(objCommand, "valorInferior", Valor.valorInferior);
            InputParameterAdd.Decimal(objCommand, "valorSuperior", Valor.valorSuperior);
            InputParameterAdd.Decimal(objCommand, "valorAlarmaInferior", Valor.valorAlarmaInferior);
            InputParameterAdd.Decimal(objCommand, "valorAlarmaSuperior", Valor.valorAlarmaSuperior);
            InputParameterAdd.Int(objCommand, "edadInferior", Valor.edadInferior);
            InputParameterAdd.Int(objCommand, "edadSuperior", Valor.edadSuperior);
            InputParameterAdd.Varchar(objCommand, "glosa", Valor.glosa.ToUpper());
            InputParameterAdd.Int(objCommand, "orden", Valor.orden);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", Valor.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", Valor.Estado);

            ExecuteNonQuery(objCommand);
        }

    }
}

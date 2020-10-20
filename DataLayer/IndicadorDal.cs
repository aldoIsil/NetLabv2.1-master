using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;

namespace DataLayer
{
    public class IndicadorDal : DaoBase
    {
        public IndicadorDal(IDalSettings settings) : base(settings)
        {
        }

        public IndicadorDal() : this(new NetlabSettings())
        {
        }
        public List<Model.ViewData.OportunidadResultado> GetOportunidadResultado(string tipoIdicador, DateTime fechaDesde, DateTime fechaHasta, int idExamenAgrupado, int idLaboratorio)
        {
            var objCommand = GetSqlCommand("pNLS_OportunidadResultadoLabINS");
            InputParameterAdd.DateTime(objCommand, "fechaini", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "fechafin", fechaHasta);
            InputParameterAdd.Int(objCommand, "idExamenAgrupado", idExamenAgrupado);
            DataTable dataTable = Execute(objCommand);
            List<Model.ViewData.OportunidadResultado> OportunidadResultadoList = new List<Model.ViewData.OportunidadResultado>();

            foreach (DataRow row in dataTable.Rows)
            {
                Model.ViewData.OportunidadResultado OportunidadResultado = new Model.ViewData.OportunidadResultado
                {
                    total = Converter.GetInt(row, "total"),
                    anio = Converter.GetInt(row, "anio"),
                    mes = Converter.GetString(row, "mes"),
                    nroOportuno = Converter.GetInt(row, "nroOportuno"),
                    porcOportunidad = Converter.GetString(row, "porcOportunidad"),
                    tiempoCatalogo = Converter.GetInt(row, "tiempoCatalogo")
                };
                OportunidadResultadoList.Add(OportunidadResultado);
            }
            return OportunidadResultadoList;
        }
        public List<Model.ViewData.OportunidadResultado> GetOportunidadResultadoCovid(string tipoIdicador, DateTime fechaDesde, DateTime fechaHasta, int idExamenAgrupado, int idLaboratorio)
        {
            var objCommand = GetSqlCommand("pNLS_OportunidadResultadoCovid19");
            InputParameterAdd.DateTime(objCommand, "fechaini", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "fechafin", fechaHasta);
            InputParameterAdd.Int(objCommand, "idExamenAgrupado", idExamenAgrupado);
            DataTable dataTable = Execute(objCommand);
            List<Model.ViewData.OportunidadResultado> OportunidadResultadoList = new List<Model.ViewData.OportunidadResultado>();

            foreach (DataRow row in dataTable.Rows)
            {
                Model.ViewData.OportunidadResultado OportunidadResultado = new Model.ViewData.OportunidadResultado
                {
                    total = Converter.GetInt(row, "total"),
                    anio = Converter.GetInt(row, "anio"),
                    mes = Converter.GetString(row, "mes"),
                    nroOportuno = Converter.GetInt(row, "nroOportuno"),
                    porcOportunidad = Converter.GetString(row, "porcOportunidad"),
                    tiempoCatalogo = Converter.GetInt(row, "tiempoCatalogo")
                };
                OportunidadResultadoList.Add(OportunidadResultado);
            }
            return OportunidadResultadoList;
        }
    }
}

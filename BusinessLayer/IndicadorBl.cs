using Model;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
namespace BusinessLayer
{
    public class IndicadorBl
    {
        public List<Model.ViewData.OportunidadResultado> GetOportunidadResultado(string tipoIdicador, DateTime fechaDesde, DateTime fechaHasta, int idExamenAgrupado, int idLaboratorio)
        {
            using (var Indicador = new IndicadorDal())
            {
               return Indicador.GetOportunidadResultado(tipoIdicador, fechaDesde, fechaHasta, idExamenAgrupado,idLaboratorio);
            }
        }
        public List<Model.ViewData.OportunidadResultado> GetOportunidadResultadoCovid(string tipoIdicador, DateTime fechaDesde, DateTime fechaHasta, int idExamenAgrupado, int idLaboratorio)
        {
            using (var Indicador = new IndicadorDal())
            {
                return Indicador.GetOportunidadResultado(tipoIdicador, fechaDesde, fechaHasta, idExamenAgrupado, idLaboratorio);
            }
        }
    }
}

using DataLayer;
using Model.ReportesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface IReporteBl
    {
        //JRCR - 2doProducto
        List<ReporteResult> GenerarReporte(ReporteRequest request);
    }

   public class ReporteBl
    {
        //JRCR - 2doProducto
        public List<ReporteResult> GenerarReporte(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            List<ReporteResult> result = new List<ReporteResult>();
            ReporteDal dal = new ReporteDal();
            result = dal.GenerarReporte(request, tipoReporte);
            return result;
        }

        //JRCR - 2doProducto
        public List<ReportePorcentajeResponse> GenerarReportePorcentaje(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            List<ReportePorcentajeResponse> result = new List<ReportePorcentajeResponse>();
            ReporteDal dal = new ReporteDal();
            result = dal.GenerarReportePorcentaje(request, tipoReporte);
            return result;
        }

        public List<MotivoRechazoDatos> GenerarReporteRadar(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            ReporteDal dal = new ReporteDal();
            return dal.GenerarReporteRadar(request, tipoReporte);
        }

        public List<ReportePieResponse> GenerarReportePorcentajeMuestrasRechazadas(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            List<ReportePieResponse> result = new List<ReportePieResponse>();
            ReporteDal dal = new ReporteDal();
            result = dal.GenerarReportePorcentajeMuestrasRechazadas(request, tipoReporte);

            return result;
        }

        public List<ReportePieResponse> GenerarReporteMuestrasCorregidas(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            List<ReportePieResponse> result = new List<ReportePieResponse>();
            ReporteDal dal = new ReporteDal();
            result = dal.GenerarReporteMuestrasCorregidas(request, tipoReporte);

            return result;
        }

        public List<ReporteCantidadResultado> CantidadBaciloscopia(int examen, int trimestre, string año, string nombrefiltro, string IdJurisdiccion)
        {
            List<ReporteCantidadResultado> result = new List<ReporteCantidadResultado>();
            ReporteDal dal = new ReporteDal();
            result = dal.CantidadBaciloscopia(examen, trimestre, año,nombrefiltro,IdJurisdiccion);

            return result;
        }
        public List<ConsultaReporteResultado> GetCantidadResultadosConsultados(ReporteRequest request)
        {
            List<ConsultaReporteResultado> result = new List<ConsultaReporteResultado>();
            ReporteDal dal = new ReporteDal();
            result = dal.GetCantidadResultadosConsultados(request);

            return result;
        }

        public List<ConsultaReporteResultado> GetCantidadResultadosConsultadosPorSolicitante(ReporteRequest request)
        {
            List<ConsultaReporteResultado> result = new List<ConsultaReporteResultado>();
            ReporteDal dal = new ReporteDal();
            result = dal.GetCantidadResultadosConsultadosPorSolicitante(request);

            return result;
        }

        public List<ReporteResultadoInformado> GetCantidadResultadosInformados(ReporteRequest request)
        {
            List<ReporteResultadoInformado> result = new List<ReporteResultadoInformado>();
            ReporteDal dal = new ReporteDal();
            result = dal.GetCantidadResultadosInformados(request);

            return result;
        }

        /*yrca */

        public List<reporteProductividadRom> GenerarReporteROM(ReporteRequest request)
        {
            List<reporteProductividadRom> result = new List<reporteProductividadRom>();
            ReporteDal dal = new ReporteDal();
            result = dal.GenerarReporteROM(request);

            return result;
        }

        public List<ReporteOportunidadRom> ReporteOportunidadRom(DateTime fechaDesde, DateTime fechaHasta, int idEstablecimiento)
        {
            List<ReporteOportunidadRom> reporte = new List<ReporteOportunidadRom>();
            ReporteDal dal = new ReporteDal();
            reporte = dal.ReporteOportunidadRom(fechaDesde,fechaHasta,idEstablecimiento);
            return reporte;
        }
        public List<ReporteOportunidadRom> ReporteOportunidadLaboratorio(DateTime fechaDesde, DateTime fechaHasta, int idEstablecimiento)
        {
            List<ReporteOportunidadRom> reporte = new List<ReporteOportunidadRom>();
            ReporteDal dal = new ReporteDal();
            reporte = dal.ReporteOportunidadLaboratorio(fechaDesde, fechaHasta, idEstablecimiento);
            return reporte;
        }
        public List<ReportePacienteCoronavirus> ReportePacienteCoronavirus(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<ReportePacienteCoronavirus> reporte = new List<ReportePacienteCoronavirus>();
            ReporteDal dal = new ReporteDal();
            reporte = dal.ReportePacienteCoronavirus(fechaDesde, fechaHasta);
            return reporte;
        }
    }
}

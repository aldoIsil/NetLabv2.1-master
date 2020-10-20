using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model.ReportesDTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReporteDal : DaoBase
    {
        public ReporteDal(IDalSettings settings) : base(settings)
        {
        }

        public ReporteDal() : this(new NetlabSettings())
        {
        }

        //JRCR - 2doProducto
        public List<ReporteResult> GenerarReporte(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            string spName = string.Empty;
            switch (tipoReporte)
            {
                case Enums.TipoReporte.OportunidadObtencionMuestras:
                    spName = "pNLS_ReporteOportunidadObtencionMuestras";
                    break;
                case Enums.TipoReporte.OportunidadEnvioMuestras:
                    spName = "pNLS_ReporteOportunidadEnvioMuestras";
                    break;
                case Enums.TipoReporte.OportunidadAnalisisMuestras:
                    spName = "pNLS_ReporteOportunidadAnalisisMuestras";
                    break;
            }

            List<ReporteResult> result = new List<ReporteResult>();
            var objCommand = GetSqlCommand(spName);
            InputParameterAdd.Varchar(objCommand, "nombrefiltro", request.NombreFiltro);
            InputParameterAdd.Varchar(objCommand, "idjurisdiccion", request.IdJurisdiccion);
            InputParameterAdd.DateTime(objCommand, "fechainicio", request.FechaInicio);
            InputParameterAdd.DateTime(objCommand, "fechafin", request.FechaFin);
            InputParameterAdd.Int(objCommand, "enfermedad", request.IdEnfermedad);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                ReporteResult item = new ReporteResult
                {
                    NumeroDias = Converter.GetInt(row, "diferenciadias"),
                    EstablecimientoCodigoUnico = Converter.GetInt(row, "establecimientocodigounico"),
                    NumeroMuestras = Converter.GetInt(row, "totalmuestras")
                };
                result.Add(item);
            }

            return result;
        }

        //JRCR - 2doProducto
        public List<ReportePorcentajeResponse> GenerarReportePorcentaje(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            string spName = string.Empty;
            switch (tipoReporte)
            {
                case Enums.TipoReporte.PorcentajeResultadosEmitidos:
                    spName = "pNLS_ReportePorcentajeResultadosEmitidos";
                    break;
            }

            List<ReportePorcentajeResponse> result = new List<ReportePorcentajeResponse>();
            var objCommand = GetSqlCommand(spName);
            InputParameterAdd.Varchar(objCommand, "nombrefiltro", request.NombreFiltro);
            InputParameterAdd.Varchar(objCommand, "idjurisdiccion", request.IdJurisdiccion);
            InputParameterAdd.DateTime(objCommand, "fechainicio", request.FechaInicio);
            InputParameterAdd.DateTime(objCommand, "fechafin", request.FechaFin);
            InputParameterAdd.Int(objCommand, "enfermedad", request.IdEnfermedad);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                ReportePorcentajeResponse item = new ReportePorcentajeResponse
                {
                    Establecimiento = Converter.GetString(row, "Establecimiento"),
                    TotalRV = Converter.GetDecimal(row, "TotalRV"),
                    TotalMuestras = Converter.GetDecimal(row, "TotalMuestras"),
                    Porcentaje = Converter.GetDecimal(row, "Porcentaje")
                };

                result.Add(item);
            }

            return result;
        }

        //JRCR - 2doProducto
        public List<MotivoRechazoDatos> GenerarReporteRadar(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            List<MotivoRechazoDatos> result = new List<MotivoRechazoDatos>();
            string spName = string.Empty;
            switch (tipoReporte)
            {
                case Enums.TipoReporte.MotivoRechazoROM:
                    spName = "pNLS_ReporteMotivoRechazoROM";
                    break;
                case Enums.TipoReporte.MotivoRechazoLaboratorio:
                    spName = "pNLS_ReporteMotivoRechazoLaboratorio";
                    break;
            }
            
            var objCommand = GetSqlCommand(spName);
            InputParameterAdd.Varchar(objCommand, "nombrefiltro", request.NombreFiltro);
            InputParameterAdd.Varchar(objCommand, "idjurisdiccion", request.IdJurisdiccion);
            InputParameterAdd.DateTime(objCommand, "fechainicio", request.FechaInicio);
            InputParameterAdd.DateTime(objCommand, "fechafin", request.FechaFin);
            InputParameterAdd.Int(objCommand, "enfermedad", request.IdEnfermedad);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                //ReporteRadarDatos item = new ReporteRadarDatos
                //{
                //    CriterioRechazo = Converter.GetString(row, "MotivoRechazo"),
                //    Establecimiento = Converter.GetString(row, "Establecimiento"),
                //    CantidadMuestras = Converter.GetInt(row, "CantidadMuestras")
                //};
                result.Add(new MotivoRechazoDatos
                {
                    EstablecimientoId = Converter.GetInt(row, "idEstablecimiento"),
                    Establecimiento = Converter.GetString(row, "Establecimiento"),
                    IdCriterioRechazo = Converter.GetInt(row, "idCriterioRechazo"),
                    IdEnfermedad = Converter.GetInt(row, "idEnfermedad"),
                    Enfermedad = Converter.GetString(row, "Enfermedad"),
                    IdOrden = Converter.GetInt(row, "idOrden"),
                    CodigoOrden = Converter.GetString(row, "codigoOrden"),
                    IdOrdenExamen = Converter.GetInt(row, "idOrdenExamen"),
                    IdOrdenMuestraRecepcion = Converter.GetInt(row, "idOrdenMuestraRecepcion"),
                    TipoCriterioRechazo = Converter.GetInt(row, "Tipo"),
                    CriterioRechazo = Converter.GetString(row, "MotivoRechazo"),
                    CantidadMuestras = Converter.GetInt(row, "CantidadMuestras"),
                    IdUsuarioRegistro = Converter.GetInt(row, "idUsuarioRegistro")
                });
            }

            return result;
        }

        public List<ReportePieResponse> GenerarReportePorcentajeMuestrasRechazadas(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            List<ReportePieResponse> result = new List<ReportePieResponse>();
            string spName = string.Empty;
            switch (tipoReporte)
            {
                case Enums.TipoReporte.NroPorcentajeMuestrasRechazadasEnROM:
                    spName = "pNLS_ReportePorcentajeMuestrasRechazadasROM";
                    break;
                case Enums.TipoReporte.NroPorcentajeResultadosRechazadosPorVerificador:
                    spName = "pNLS_ReporteNroPorcentajeResultadosRechazadosPorVerificador";
                    break;
            }
            
            var objCommand = GetSqlCommand(spName);
            InputParameterAdd.Varchar(objCommand, "nombrefiltro", request.NombreFiltro);
            InputParameterAdd.Varchar(objCommand, "idjurisdiccion", request.IdJurisdiccion);
            InputParameterAdd.DateTime(objCommand, "fechainicio", request.FechaInicio);
            InputParameterAdd.DateTime(objCommand, "fechafin", request.FechaFin);
            InputParameterAdd.Int(objCommand, "enfermedad", request.IdEnfermedad);
            InputParameterAdd.Varchar(objCommand, "examen", request.IdExamen);
            InputParameterAdd.Int(objCommand, "usuario", request.IdUsuario);//analista
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                ReportePieResponse item = new ReportePieResponse
                {
                    Indicador = Converter.GetString(row, "Indicador"),
                    ValorIndicadorPorcentaje = Converter.GetDecimal(row, "ValorDecimal"),
                    ValorIndicadorNumero = Converter.GetInt(row, "ValorIndicador")
                };

                result.Add(item);
            }

            return result;
        }

        public List<ReportePieResponse> GenerarReporteMuestrasCorregidas(ReporteRequest request, Enums.TipoReporte tipoReporte)
        {
            List<ReportePieResponse> result = new List<ReportePieResponse>();
            string spName = string.Empty;
            switch (tipoReporte)
            {
                case Enums.TipoReporte.NroResultadosCorregidosDespuesPublicacion:
                    spName = "pNLS_ReporteResultadosCorregidosDespuesPublicacion";
                    break;
            }

            var objCommand = GetSqlCommand(spName);
            InputParameterAdd.Varchar(objCommand, "nombrefiltro", request.NombreFiltro);
            InputParameterAdd.Varchar(objCommand, "idjurisdiccion", request.IdJurisdiccion);
            InputParameterAdd.DateTime(objCommand, "fechainicio", request.FechaInicio);
            InputParameterAdd.DateTime(objCommand, "fechafin", request.FechaFin);
            InputParameterAdd.Int(objCommand, "enfermedad", request.IdEnfermedad);
            InputParameterAdd.Varchar(objCommand, "examen", request.IdExamen);
            InputParameterAdd.Int(objCommand, "usuario", request.IdUsuario);//verificador
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                ReportePieResponse item = new ReportePieResponse
                {
                    Indicador = Converter.GetString(row, "Indicador"),
                    ValorIndicadorNumero = Converter.GetInt(row, "ValorIndicador")
                };

                result.Add(item);
            }

            return result;
        }

        public List<ReporteCantidadResultado> CantidadBaciloscopia(int examen, int trimestre, string año, string nombrefiltro, string IdJurisdiccion)
        {
            List<ReporteCantidadResultado> result = new List<ReporteCantidadResultado>();
            var objCommand = GetSqlCommand("pNLS_ConsultaCantidadExamenBaciloscopia");
            InputParameterAdd.Int(objCommand, "examen", examen);
            InputParameterAdd.Int(objCommand, "trimestre", trimestre);
            InputParameterAdd.Varchar(objCommand, "año", año);
            InputParameterAdd.Varchar(objCommand, "nombrefiltro", nombrefiltro);
            InputParameterAdd.Varchar(objCommand, "idjurisdiccion", IdJurisdiccion);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                if(examen != 3)
                {
                    ReporteCantidadResultado item = new ReporteCantidadResultado
                    {
                        valor = Converter.GetString(row, "valor"),
                        cantidad = Converter.GetInt(row, "triProc"),
                        cantidadAcumulada = Converter.GetInt(row, "acumProc"),
                        canPos = Converter.GetInt(row, "triPos"),
                        canAcumPos = Converter.GetInt(row, "acumPos")
                    };
                    result.Add(item);
                }
                else
                {
                    ReporteCantidadResultado item = new ReporteCantidadResultado
                    {
                        valor = Converter.GetString(row, "nombrePS"),
                        cantidad = Converter.GetInt(row, "rTrimestral"),
                        cantidadAcumulada = Converter.GetInt(row, "rAcumulado"),
                        canPos = Converter.GetInt(row, "sriTrimestral"),
                        canAcumPos = Converter.GetInt(row, "sriAcumulado"),
                        canTriRi = Converter.GetInt(row, "riTrimestral"),
                        canAcumRi = Converter.GetInt(row, "riAcumulado"),
                        canTriRri = Converter.GetInt(row, "rriTrimestral"),
                        canAcumRri = Converter.GetInt(row, "rriAcumulado")
                    };
                    result.Add(item);
                }
                
            }

            return result;
        }

        public List<ConsultaReporteResultado> GetCantidadResultadosConsultados(ReporteRequest request)
        {
            List<ConsultaReporteResultado> result = new List<ConsultaReporteResultado>();
            var objCommand = GetSqlCommand("pNLS_ConsultaCantidadResultadosConsultados");
            InputParameterAdd.Int(objCommand, "tipoReporte", request.TipoReporte);
            InputParameterAdd.Varchar(objCommand, "nombrefiltro", request.NombreFiltro);
            InputParameterAdd.Varchar(objCommand, "idjurisdiccion", request.IdJurisdiccion);
            InputParameterAdd.DateTime(objCommand, "fechaDesde", request.FechaInicio);
            InputParameterAdd.DateTime(objCommand, "fechaHasta", request.FechaFin);
            InputParameterAdd.Int(objCommand, "idEnfermedad", request.IdEnfermedad);
            InputParameterAdd.Varchar(objCommand, "idExamen", request.IdExamen);
            DataTable dataTable = Execute(objCommand);

            if (request.TipoReporte == 1) //Reporte General
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    ConsultaReporteResultado item = new ConsultaReporteResultado
                    {
                        Usuario = Converter.GetString(row, "Usuario"),
                        CantidadConsulta = Converter.GetInt(row, "Cantidad"),
                        Establecimiento = Converter.GetString(row, "EESS_Origen"),
                        Enfermedad = Converter.GetString(row, "Enfermedad"),
                        Examen = Converter.GetString(row, "Examen"),
                        totalResultado = Converter.GetInt(row, "Total"),
                    };
                    result.Add(item);
                }
            }
            else //Reporte Detallado
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    ConsultaReporteResultado item = new ConsultaReporteResultado
                    {
                        Usuario = Converter.GetString(row, "Usuario"),
                        Establecimiento = Converter.GetString(row, "EESS_Origen"),
                        FechaVerificacion = Converter.GetDateTime(row, "fechaValidacion"),
                        FechaImpresion = Converter.GetDateTime(row, "fechaImpresion"),
                        Enfermedad = Converter.GetString(row, "Enfermedad"),
                        Examen = Converter.GetString(row, "Examen"),
                        HorasRetraso = Converter.GetInt(row, "Horas"),
                        NumeroInforme = Converter.GetInt(row, "numeroInforme")
                    };

                    result.Add(item);
                }
            }
            
            return result;
        }

        public List<ConsultaReporteResultado> GetCantidadResultadosConsultadosPorSolicitante(ReporteRequest request)
        {
            List<ConsultaReporteResultado> result = new List<ConsultaReporteResultado>();
            var objCommand = GetSqlCommand("pNLS_ConsultaCantidadResultadosConsultadosPorSolicitante");
            InputParameterAdd.Int(objCommand, "tipoReporte", request.TipoReporte);
            InputParameterAdd.Varchar(objCommand, "nombrefiltro", request.NombreFiltro);
            InputParameterAdd.Varchar(objCommand, "idjurisdiccion", request.IdJurisdiccion);
            InputParameterAdd.DateTime(objCommand, "fechaDesde", request.FechaInicio);
            InputParameterAdd.DateTime(objCommand, "fechaHasta", request.FechaFin);
            InputParameterAdd.Int(objCommand, "idEnfermedad", request.IdEnfermedad);
            InputParameterAdd.Varchar(objCommand, "idExamen", request.IdExamen);
            DataTable dataTable = Execute(objCommand);

            if (request.TipoReporte == 1) //Reporte General
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    ConsultaReporteResultado item = new ConsultaReporteResultado
                    {
                        Usuario = Converter.GetString(row, "Usuario"),
                        CantidadConsulta = Converter.GetInt(row, "Cantidad"),
                        Establecimiento = Converter.GetString(row, "EESS_Origen"),
                        Enfermedad = Converter.GetString(row, "Enfermedad"),
                        Examen = Converter.GetString(row, "Examen"),
                        totalResultado = Converter.GetInt(row, "Total"),
                    };
                    result.Add(item);
                }
            }
            else //Reporte Detallado
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    ConsultaReporteResultado item = new ConsultaReporteResultado
                    {
                        Usuario = Converter.GetString(row, "Usuario"),
                        Establecimiento = Converter.GetString(row, "EESS_Origen"),
                        FechaVerificacion = Converter.GetDateTime(row, "fechaValidacion"),
                        FechaImpresion = Converter.GetDateTime(row, "fechaImpresion"),
                        Enfermedad = Converter.GetString(row, "Enfermedad"),
                        Examen = Converter.GetString(row, "Examen"),
                        HorasRetraso = Converter.GetInt(row, "Horas"),
                        NumeroInforme = Converter.GetInt(row, "numeroInforme")
                    };

                    result.Add(item);
                }
            }

            return result;
        }

        public List<ReporteResultadoInformado> GetCantidadResultadosInformados(ReporteRequest request)
        {
            List<ReporteResultadoInformado> result = new List<ReporteResultadoInformado>();
            var objCommand = GetSqlCommand("pNLS_GetCantidadResultadosInformados");
            InputParameterAdd.Int(objCommand, "tipoReporte", request.TipoReporte);
            InputParameterAdd.Varchar(objCommand, "nombrefiltro", request.NombreFiltro);
            InputParameterAdd.Varchar(objCommand, "idjurisdiccion", request.IdJurisdiccion);
            InputParameterAdd.DateTime(objCommand, "fechaDesde", request.FechaInicio);
            InputParameterAdd.DateTime(objCommand, "fechaHasta", request.FechaFin);
            InputParameterAdd.Int(objCommand, "idEnfermedad", request.IdEnfermedad);
            InputParameterAdd.Varchar(objCommand, "idExamen", request.IdExamen);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow item in dataTable.Rows)
            {
                ReporteResultadoInformado rpt = new ReporteResultadoInformado
                {
                    Examen = Converter.GetString(item, "nombre"),
                    Paciente = Converter.GetString(item, "Paciente"),
                    CantidadExamen = Converter.GetInt(item, "CantidadExamen"),
                    CantidadExamenTotal = Converter.GetInt(item, "CantidadExamenTotal"),
                    CantidadPaciente = Converter.GetInt(item, "CantidadPacienteTotal")
                };
                result.Add(rpt);
            }
            return result;
        }

        /* YRCA*/
        public List<reporteProductividadRom> GenerarReporteROM(ReporteRequest request)
        {
            List<reporteProductividadRom> result = new List<reporteProductividadRom>();
            var objCommand = GetSqlCommand("pNLS_ConsultaProductividadROM");
            InputParameterAdd.DateTime(objCommand, "fechasinicio", request.FechaInicio);
            InputParameterAdd.DateTime(objCommand, "Fechafin", request.FechaFin);
            InputParameterAdd.Int(objCommand, "idusuario", request.IdUsuario);//analista
            DataTable dataTable = Execute(objCommand);
            foreach (DataRow row in dataTable.Rows)
            {
                reporteProductividadRom item = new reporteProductividadRom
                {
                    codigoMuestra = Converter.GetString(row, "CodMuestra"),
                    Muestra = Converter.GetString(row, "Muestra"),
                    Paciente = Converter.GetString(row, "Paciente"),
                    codigoOrden = Converter.GetString(row, "codigoOrden"),
                    nombreEstablecimientoOrigen = Converter.GetString(row, "EstablecimientoOrigen"),
                    fechaSolicitud = Converter.GetString(row, "fechaSolicitud"),
                    fechaRegistro = Converter.GetDateTime(row, "fechaRegistro"),
                    fechaRecepcion = Converter.GetString(row, "fechaRecepcion"),
                    Enfermedad = Converter.GetString(row, "Enfermedad"),
                    //nombreDestino = Converter.GetString(row, "EstablecimientoDestino"),
                    nombreExamen = Converter.GetString(row, "Examen")
                };
                result.Add(item);
            }
            return result;
        }

        public List<ReporteOportunidadRom> ReporteOportunidadRom(DateTime fechaDesde, DateTime fechaHasta, int idEstablecimiento)
        {
            List<ReporteOportunidadRom> reporte = new List<ReporteOportunidadRom>();
            var objCommand = GetSqlCommand("pNLS_ReporteOportunidadRom");
            InputParameterAdd.DateTime(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            DataTable DetalleOportunidad = Execute(objCommand);

            foreach (DataRow row in DetalleOportunidad.Rows)
            {
                ReporteOportunidadRom item = new ReporteOportunidadRom
                {
                    Laboratorio = Converter.GetString(row, "laboratorio"),
                    codificacion = Converter.GetString(row, "codificacion"),
                    fechaRecepcionRomINS = Converter.GetString(row, "fechaRecepcionRomINS"),
                    fechaEnvio = Converter.GetString(row, "fechaEnvio"),
                    fechaRecepcionP = Converter.GetString(row, "fechaRecepcionP"),
                    DiasTranscurridos = Converter.GetInt(row, "DiasTranscurridos"),
                    Oportunidad = Converter.GetString(row, "Porcentaje"),
                    totalEnvios = Converter.GetInt(row, "totalEnvios"),
                    totalOportunos = Converter.GetInt(row, "Oportunos")
                };
                reporte.Add(item);
            }
            return reporte;
        }

        public List<ReporteOportunidadRom> ReporteOportunidadLaboratorio(DateTime fechaDesde, DateTime fechaHasta, int idEstablecimiento)
        {
            List<ReporteOportunidadRom> reporte = new List<ReporteOportunidadRom>();
            var objCommand = GetSqlCommand("pNLS_ReporteOportunidadLaboratorio");
            InputParameterAdd.DateTime(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            DataTable DetalleOportunidad = Execute(objCommand);

            foreach (DataRow row in DetalleOportunidad.Rows)
            {
                ReporteOportunidadRom item = new ReporteOportunidadRom
                {
                    Laboratorio = Converter.GetString(row, "laboratorio"),
                    codificacion = Converter.GetString(row, "codificacion"),
                    fechaRecepcionP = Converter.GetString(row, "fechaRecepcionP"),
                    fechaEnvio = Converter.GetString(row, "fechaValidado"),
                    DiasTranscurridos = Converter.GetInt(row, "DiasTranscurridos"),
                    Oportunidad = Converter.GetString(row, "Porcentaje"),
                    totalEnvios = Converter.GetInt(row, "totalEnvios"),
                    totalOportunos = Converter.GetInt(row, "Oportunos")
                };
                reporte.Add(item);
            }
            return reporte;
        }
        public List<ReportePacienteCoronavirus> ReportePacienteCoronavirus(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<ReportePacienteCoronavirus> reporte = new List<ReportePacienteCoronavirus>();
            var objCommand = GetSqlCommand("pNLS_ReportePacienteCoronavirus");
            //InputParameterAdd.DateTime(objCommand, "fechaDesde", fechaDesde);
            //InputParameterAdd.DateTime(objCommand, "fechaHasta", fechaHasta);
            DataTable DetalleOportunidad = Execute(objCommand);

            foreach (DataRow row in DetalleOportunidad.Rows)
            {
                DateTime? fecha = new DateTime();
                if (string.IsNullOrEmpty(Converter.GetDateTime(row, "FechaValidado").ToString()))
                {
                    fecha = null;
                }
                else
                {
                    fecha = Converter.GetDateTime(row, "FechaValidado");
                }
                ReportePacienteCoronavirus item = new ReportePacienteCoronavirus
                {
                    codigoOrden = Converter.GetString(row, "codigoOrden"),
                    codigoMuestra = Converter.GetString(row, "CodigoMuestra"),
                    nroOficio = Converter.GetString(row, "nroOficio"),
                    EESSDepOrigen = Converter.GetString(row, "EESSDepOrigen"),
                    EESSProvOrigen = Converter.GetString(row, "EESSProvOrigen"),
                    EESSDistOrigen = Converter.GetString(row, "EESSDistOrigen"),
                    celular = Converter.GetString(row, "celular"),
                    EESSDisaOrigen = Converter.GetString(row, "EESSDisaOrigen"),
                    EESSRedOrigen = Converter.GetString(row, "EESSRedOrigen"),
                    EESSMicroRedOrigen = Converter.GetString(row, "EESSMicroRedOrigen"),
                    EstablecimientoOrigen = Converter.GetString(row, "EstablecimientoOrigen"),
                    DocIdentidad = Converter.GetString(row, "DocIdentidad"),
                    fechaNacimiento = Converter.GetString(row, "fechaNacimiento"),
                    nombrePaciente = Converter.GetString(row, "nombrePaciente"),
                    edad = Converter.GetString(row, "edad"),
                    SexoPaciente = Converter.GetString(row, "SexoPaciente"),
                    TipoMuestra = Converter.GetString(row, "TipoMuestra"),
                    FechaColeccion = Converter.GetString(row, "FechaColeccion"),
                    FechaRecepcionROM = Converter.GetString(row, "FechaRecepcionROM"),
                    
                    FechaValidado = fecha,
                    //MuestraConforme = Converter.GetString(row, "MuestraConforme"),
                    //CriteriosRechazo = Converter.GetString(row, "CriteriosRechazo"),
                    convResultado = Converter.GetString(row, "convResultado"),
                    estatus = Converter.GetInt(row, "estatus"),
                    estatusP = Converter.GetInt(row, "estatusP"),
                    estatusR = Converter.GetInt(row, "estatusR"),
                    conformeP = Converter.GetInt(row, "conformeP"),
                    conformeR = Converter.GetInt(row, "conformeR"),
                    EstatusResultado = "Resultado Verificado",

                    //DireccionPaciente = Converter.GetString(row, "Direccion"),
                    //AntecedenteViaje = Converter.GetString(row, "AntecedenteViaje"),
                    //FechHoraValidado2 = Converter.GetString(row, "FechaHoraValidado")
                };
                reporte.Add(item);
            }
            return reporte;
        }

        public CoronavirusDatosClinicos ReportePacienteCoronavirusDatoClinico(string codigoOrden)
        {
            var reporte = new CoronavirusDatosClinicos();
            var objCommand = GetSqlCommand("pNLS_ReportePacienteCoronavirusDatoClinico");
            InputParameterAdd.Varchar(objCommand, "codigoOrden", codigoOrden);
            //InputParameterAdd.DateTime(objCommand, "fechaHasta", fechaHasta);
            DataTable DetalleOportunidad = Execute(objCommand);

            foreach (DataRow row in DetalleOportunidad.Rows)
            {

                CoronavirusDatosClinicos item = new CoronavirusDatosClinicos
                {
                    asma = Converter.GetString(row, "Asma"),
                    cardiopatia = Converter.GetString(row, "Cardiopatia_cronica"),
                    contactoAves = Converter.GetString(row, "Contacto_con_aves"),
                    contactocasoIRAG = Converter.GetString(row, "Contacto_con_caso_de_IRAG"),
                    crianzaAves = Converter.GetString(row, "Contacto_crianza_aves"),
                    crianzaCerdo = Converter.GetString(row, "Contacto_crianza_cerdos"),
                    diabetesMiellitus = Converter.GetString(row, "Diabetes_miellitus"),
                    dxPresuntivo = Converter.GetString(row, "Diagnostico_presuntivo"),
                    dificultadRespiratoria = Converter.GetString(row, "Dificultad respiratoria"),
                    enferNeurologica = Converter.GetString(row, "Enfermedad_neurologica_cronica"),
                    enferRenalCronica = Converter.GetString(row, "Enfermedad_renal_cronica"),
                    fallecimiento = Converter.GetString(row, "Fallecimiento"),
                    fechaAlta = Converter.GetString(row, "Fecha_alta"),
                    fechaHospitalizacion = Converter.GetString(row, "Fecha_hospitalización"),
                    fechaIngresoUCI = Converter.GetString(row, "Fecha_Ingreso_UCI"),
                    FecInicioAdmin = Converter.GetString(row, "Fecha_Inicio_administracion"),
                    FecIniSintomas = Converter.GetString(row, "Fecha_Inicio_Sintomas"),

                    FecObtencion = Converter.GetString(row, "Fecha_Obtencion"),
                    FecDefuncion = Converter.GetString(row, "Fecha_defuncion"),
                    fiebre = Converter.GetString(row, "Fiebre_antecedente_fiebre"),
                    gestacion = Converter.GetString(row, "Gestacion"),
                    hepatopatiacronica = Converter.GetString(row, "Hepatopatia_cronica"),
                    hospitalizacion = Converter.GetString(row, "Hospitalizacion"),
                    irag = Converter.GetString(row, "Irag"),
                    uci = Converter.GetString(row, "Ingreso_UCI"),
                    inmunodeficiencia = Converter.GetString(row, "Inmunodeficiencia"),
                    IRAG_INUSCITADA = Converter.GetString(row, "IRAG_INUSCITADA"),
                    Muerte_IRAG_causa_desconocida = Converter.GetString(row, "Muerte_IRAG_causa_desconocida"),
                    visitaPaises = Converter.GetString(row, "Nombre_pais_visita_ultimos_15_dias"),

                    obesidad = Converter.GetString(row, "Obesidad"),
                    oseltamivir = Converter.GetString(row, "Oseltamivir"),
                    enfpulmonarcronica = Converter.GetString(row, "Otra_enfermedad_pulmonar_cronica"),

                    otro = Converter.GetString(row, "Otro"),
                    otrosSignos = Converter.GetString(row, "Otros_signos_sintomas"),
                    pacde5a60 = Converter.GetString(row, "Paciente_entre_5_60"),
                    puerperio = Converter.GetString(row, "Puerperio"),
                    sindromegripal = Converter.GetString(row, "SINDROME_GRIPAL"),
                    Sindrome = Converter.GetString(row, "Sindrome"),
                    tempeatura = Converter.GetString(row, "Temperatura"),
                    tipoMuestra = Converter.GetString(row, "tipoMuestra"),
                    TomaMuestras = Converter.GetString(row, "TomaMuestras"),
                    trabajadorsalud = Converter.GetString(row, "FechaHoraValidado"),
                    tos = Converter.GetString(row, "Tos"),
                    trimestre = Converter.GetString(row, "Trimestre"),
                    vacunacionantigripal = Converter.GetString(row, "VacunacionAntigripal"),
                    viajesultimos15dias = Converter.GetString(row, "viajes15")
                };
            }
            return reporte;
        }

    }
}

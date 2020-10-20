using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using DataLayer.DalConverter;
using System;
using Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataLayer
{
    public class ResultadosDal : DaoBase
    {

        public ResultadosDal(IDalSettings settings) : base(settings)
        {
        }

        public ResultadosDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de la orden y la prueba.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorioUsuario"></param>
        /// <returns></returns>
        public Orden GetOrdenById(Guid idOrden, string idOrdenExamen,int idLaboratorioUsuario, int idusuario)
        {
            var objCommand = GetSqlCommand("pNLS_ValidarResultados");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);
            InputParameterAdd.Int(objCommand, "IdLaboratorioUsuario", idLaboratorioUsuario);
            InputParameterAdd.Int(objCommand, "IdUsuario", idusuario);
            var dataSet = ExecuteDataSet(objCommand);

            var ordenDataTable = dataSet.Tables[0];
            var resultadosDataTable = dataSet.Tables[1];
            //var histPaciente = dataSet.Tables[2];

            #region Orden

            Orden orden = null;

            if (ordenDataTable.Rows.Count > 0)
            {
                var row = ordenDataTable.Rows[0];

                orden = new Orden
                {
                    codigoOrden = Converter.GetString(row, "codigoOrden"),
                    Paciente = new Paciente
                    {
                        NroDocumento = Converter.GetString(row, "paciente_nrodocumento"),
                        tipoDocumen = Converter.GetString(row, "TipoDocumen"),
                        Nombres = Converter.GetString(row, "paciente_nombres"),
                        FechaNacimiento = Converter.GetDateTime(row, "paciente_fechNac"),
                        edadPaciente = Converter.GetString(row, "Edad"),
                        codificacion = Converter.GetInt(row, "paciente_codificacion"),
                        TipoDocumento = Converter.GetInt(row, "tipoDocumento")
                    },
                    Proyecto = new Proyecto
                    {
                        Nombre = Converter.GetString(row, "proyecto_nombre"),
                        IdProyecto = Converter.GetInt(row, "proyecto_idProyecto")
                    },
                    establecimiento = new Establecimiento
                    {
                        Nombre = Converter.GetString(row, "establecimiento_nombre"),
                        IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento")
                    },
                    idEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                    nroOficio = Converter.GetString(row, "nroOficio"),
                    observacion = Converter.GetString(row, "observacion"),
                    estatus = Converter.GetInt(row, "estatus"),
                    idOrden = idOrden,
                    fechaSolicitud = Converter.GetDateTime(row, "fechaSolicitud")
                };
            }

            #endregion

            #region Resultados

            if (orden != null)
                orden.resultadosList = ResultadosConvertTo.ListResultados(resultadosDataTable);

            #endregion

            //#region Paciente
            //List<Paciente> listaPaciente = new List<Paciente>();

            //if (histPaciente.Rows.Count == 0) 
            //    return null;

            //listaPaciente = (from DataRow row in histPaciente.Rows
            //                 select new Paciente
            //                 {
            //                     Enfermedad = Converter.GetString(row, "Enfermedad"),
            //                     FechaObtencion = Converter.GetDateTime(row, "FechaObtencion"),
            //                     FechaValidacion = Converter.GetString(row, "FechaValidado"),
            //                     Examen = Converter.GetString(row, "Examen"),
            //                     Componente = Converter.GetString(row, "Componente"),
            //                     Resultado = Converter.GetString(row, "Resultado"),
            //                     EESSOrigen = Converter.GetString(row, "EstablecimientoOrigen"),
            //                     EstadoResultado = Converter.GetString(row, "EstatusResultado")
            //                 }).ToList();
            //orden.listPaciente = listaPaciente.ToList();
            //#endregion

            return orden;
        }
        /// Descripción: Obtiene informacion del solicitante de la Orden y paciente.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 30/04/2018
        public EnvioAlerta GetDatosCorreo(string idOrdenExamen)
        {
            var objCommand = GetSqlCommand("pNLS_ObtenerDatosCorreo");
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);
            DataTable dataTable = Execute(objCommand);
            var datos = new EnvioAlerta();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    datos.Solicitante = Converter.GetString(row, "solicitante");
                    datos.CorreoSolicitante = Converter.GetString(row, "correo");
                    datos.CelularSolicitante = Converter.GetString(row, "Telefono");
                    datos.Paciente = Converter.GetString(row, "paciente");
                    datos.CorreoPaciente = Converter.GetString(row, "correoElectronico");
                    datos.CelularPaciente = Converter.GetString(row, "Celular1");
                    datos.CodigoOrden = Converter.GetString(row, "codigoOrden");
                    datos.Resultado = Converter.GetString(row, "Resultado");
                    datos.Envio = Converter.GetInt(row, "EnvioSMS");
                }
            }
            return datos;
        }
        /// <summary>
        /// Descripción: Ejecuta la transaccion para la actualizacion y validacion de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <param name="comentario"></param>
        /// <param name="esConforme"></param>
        /// <param name="idUsuario"></param>
        public void UpdateResultado(Guid idOrdenExamen, string comentario, int esConforme, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLU_ResultadosValidados");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", idOrdenExamen);
            InputParameterAdd.Varchar(objCommand, "comentario", comentario);
            InputParameterAdd.Int(objCommand, "esConforme", esConforme);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            ExecuteNonQuery(objCommand);
        }

        public string UpdateResultadoPruebaRapida(Guid idOrdenExamen, string comentario, int esConforme, int idUsuario)
        {
            string result = string.Empty;
            var objCommand = GetSqlCommand("pNLU_ResultadosValidados");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", idOrdenExamen);
            InputParameterAdd.Varchar(objCommand, "comentario", comentario);
            InputParameterAdd.Int(objCommand, "esConforme", esConforme);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            DataTable dataTable = Execute(objCommand);
            foreach (DataRow item in dataTable.Rows)
            {
                result = Converter.GetString(item, "idLaboratorioDestino");
            }

            return result;
        }

        public void EnvioSMS(string phone, string message, int tipo)
        {
            var objCommand = GetSqlCommand("pNLI_RegistrarEnvioSms");
            InputParameterAdd.Char(objCommand, "numero", phone);
            InputParameterAdd.Varchar(objCommand, "mensaje", message);
            InputParameterAdd.Int(objCommand, "tipoUsuario", tipo);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las ordenes
        /// Author: Juan Muga.
        /// Fecha Creacion: 25/07/2018
        /// </summary>
        /// <param name="codigoOrden"></param>
        /// <param name="muestra"></param>
        /// <param name="TipoMuestra"></param>
        /// <param name="idEstablecimientoDestino"></param>
        /// <returns></returns>
        public ResultAnalito OrdenAnalitoResultadobyCodigoOrden(string codigoOrden, string muestra, string TipoMuestra, int idEstablecimientoDestino, string NombreExamen = null)
        {
            var objCommand = GetSqlCommand("pNLS_OrdenAnalitoResultadobyCodigoOrden");
            InputParameterAdd.Varchar(objCommand, "codigoOrden", codigoOrden);
            InputParameterAdd.Varchar(objCommand, "muestra", muestra);
            InputParameterAdd.Varchar(objCommand, "TipoMuestra", TipoMuestra);
            InputParameterAdd.Int(objCommand, "idEstablecimientoDestino", idEstablecimientoDestino);
            InputParameterAdd.Varchar(objCommand, "NombreExamen", NombreExamen);

            var dataSet = ExecuteDataSet(objCommand);

            var ordenDataTable = dataSet.Tables[0];
            //var resultadosDataTable = dataSet.Tables[1];

            ResultAnalito ResultAnalito = null;

            if (ordenDataTable.Rows.Count > 0)
            {
                var row = ordenDataTable.Rows[0];
                ResultAnalito = new ResultAnalito
                {
                    IdAnalito = Converter.GetGuid(row, "idAnalito"),
                    IdOrdenExamen = Converter.GetGuid(row, "idOrdenExamen"),
                    IdOrdenResultadoAnalito = Converter.GetGuid(row, "IdOrdenResultadoAnalito"),
                    Metodos = new DetalleAnalitoDal().GetAnalitosbyIdAnalito(Converter.GetGuid(row, "idAnalito"), Converter.GetGuid(row, "IdOrdenResultadoAnalito"), Converter.GetGuid(row, "idOrdenExamen"))
                };
            }
            return ResultAnalito;
        }
            
        public String AddExamenAnalistaPlantilla(Guid idOrden, int idEstablecimiento, int idUsuario, int idplantilla)
        {
            string result = string.Empty;
            var objCommand = GetSqlCommand("pNLI_AddExamenAnalistaPlantilla");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "idLaboratorio", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "idPlantilla", idplantilla);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow item in dataTable.Rows)
            {
                result = Converter.GetString(item, "Resultado");
            }

            return result;
        }
        public string RecepcionarCodigoMuestra(string muestraCodificacion, int idestablecimiento, int idUsuario, string UsuarioRom, string nroSolicitud)
        {
            string result = string.Empty;
            var objCommand = GetSqlCommand("pNLU_OrdenMuestraRecepcionResultadoMasivo");
            InputParameterAdd.Varchar(objCommand, "codificacion", muestraCodificacion);
            InputParameterAdd.Varchar(objCommand, "solicitud", nroSolicitud);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idestablecimiento);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Varchar(objCommand, "UsuarioRom", UsuarioRom);
            return objCommand.ExecuteScalar().ToString();
        }
        public string ValidarCodigoMuestra(string muestraCodificacion, int idestablecimiento, int idUsuario, string metodoKit)
        {
            string result = string.Empty;
            var objCommand = GetSqlCommand("pNLU_OrdenMuestraRecepcionValidarMasivo");
            InputParameterAdd.Varchar(objCommand, "codificacion", muestraCodificacion);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idestablecimiento);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Char(objCommand, "metodoKit", metodoKit);
            OutputParameterAdd.Varchar(objCommand, "Resultado", 1250);
            ExecuteNonQuery(objCommand);
            return (string)objCommand.Parameters["@Resultado"].Value;
        }
        public string RecepcionarValidarCodigoMuestra(string muestraCodificacion, int idestablecimiento, int idUsuario, string metodoKit)
        {
            string result = string.Empty;
            var objCommand = GetSqlCommand("pNLU_OrdenMuestraRecepcionyValidacionMasivo");
            InputParameterAdd.Varchar(objCommand, "codificacion", muestraCodificacion);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idestablecimiento);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Char(objCommand, "metodoKit", metodoKit);
            OutputParameterAdd.Varchar(objCommand, "Resultado", 250);
            ExecuteNonQuery(objCommand);
            return (string)objCommand.Parameters["@Resultado"].Value;
        }

        public ResultAnalito getOrdenAnalitoResultadobyCodigoExamen(string codigoExamenNetLab, string idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_TramaOrdenAnalitoResultadobyCodigoExamen");
            InputParameterAdd.Varchar(objCommand, "codigoExamen", codigoExamenNetLab);
            InputParameterAdd.Varchar(objCommand, "idExamen", idExamen);

            var dataSet = ExecuteDataSet(objCommand);

            var ordenDataTable = dataSet.Tables[0];
            //var resultadosDataTable = dataSet.Tables[1];

            ResultAnalito ResultAnalito = null;

            if (ordenDataTable.Rows.Count > 0)
            {
                var row = ordenDataTable.Rows[0];
                ResultAnalito = new ResultAnalito
                {
                    Resultado = Converter.GetString(row, "resultado"),
                    IdAnalito = Converter.GetGuid(row, "idAnalito"),
                    estatusE = Converter.GetInt(row, "estatusE"),
                    IdOrdenExamen = Converter.GetGuid(row, "idOrdenExamen"),
                    IdOrdenResultadoAnalito = Converter.GetGuid(row, "IdOrdenResultadoAnalito"),
                    Metodos = new DetalleAnalitoDal().GetAnalitosbyIdAnalito(Converter.GetGuid(row, "idAnalito"), Converter.GetGuid(row, "IdOrdenResultadoAnalito"), Converter.GetGuid(row, "idOrdenExamen"))
                };
            }
            return ResultAnalito;
        }

        public ResultadoCovidPaciente ObtenerResultadoCovidPorOrden(Guid idOrden)
        {
            var objCommand = GetSqlCommand("pNLS_CovidResultadobyCodigoOrden");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            DataTable dataTable = Execute(objCommand);
            var resultado = new ResultadoCovidPaciente();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    resultado = new ResultadoCovidPaciente
                    {
                        ape_materno = this.ObtenerDatosSinEspacios(row, "ape_materno"),
                        ape_paterno = this.ObtenerDatosSinEspacios(row, "ape_paterno"),
                        des_departamento = this.ObtenerDatosSinEspacios(row, "des_departamento"),
                        des_distrito = this.ObtenerDatosSinEspacios(row, "des_distrito"),
                        des_provincia = this.ObtenerDatosSinEspacios(row, "des_provincia"),
                        direccion = this.ObtenerDatosSinEspacios(row, "direccion"),
                        email = this.ObtenerDatosSinEspacios(row, "email"),
                        fec_coleccion = this.ObtenerDatosSinEspacios(row, "fec_colecciOn"),
                        fec_validacion = this.ObtenerDatosSinEspacios(row, "fec_validacion"),
                        hor_coleccion = this.ObtenerDatosSinEspacios(row, "hor_coleccion"),
                        //idUbigeoActual = this.ObtenerDatosSinEspacios(row, "idUbigeoActual"),
                        nombres = this.ObtenerDatosSinEspacios(row, "nombres"),
                        nro_celular = this.ObtenerDatosSinEspacios(row, "nro_celular"),
                        nro_documento = this.ObtenerDatosSinEspacios(row, "nro_documento"),
                        rest_prueba = this.ObtenerDatosSinEspacios(row, "rest_prueba"),
                        tip_documento = this.ObtenerDatosSinEspacios(row, "tip_documento"),
                        ubigeo = this.ObtenerDatosSinEspacios(row, "ubigeo"),
                        tip_prueba = this.ObtenerDatosSinEspacios(row, "tip_prueba")
                    };
                }
            }

            return resultado;
        }

        public void InsertarResultadoCovidFallido(ResultadoCovidPaciente datos, int idUsuario, string motivofalla)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarRegistroCovidFallido");
            InputParameterAdd.Varchar(objCommand, "tip_documento", datos.tip_documento);
            InputParameterAdd.Varchar(objCommand, "nro_documento", datos.nro_documento);
            InputParameterAdd.Varchar(objCommand, "nombres", datos.nombres);
            InputParameterAdd.Varchar(objCommand, "ape_paterno", datos.ape_paterno);
            InputParameterAdd.Varchar(objCommand, "ape_materno", datos.ape_materno);
            InputParameterAdd.Varchar(objCommand, "nro_celular", datos.nro_celular);
            InputParameterAdd.Varchar(objCommand, "email", datos.email);
            InputParameterAdd.Varchar(objCommand, "ubigeo", datos.ubigeo);
            //InputParameterAdd.Varchar(objCommand, "idUbigeoActual", datos.idUbigeoActual);
            InputParameterAdd.Varchar(objCommand, "des_departamento", datos.des_departamento);
            InputParameterAdd.Varchar(objCommand, "des_provincia", datos.des_provincia);
            InputParameterAdd.Varchar(objCommand, "des_distrito", datos.des_distrito);
            InputParameterAdd.Varchar(objCommand, "direccion", datos.direccion);
            InputParameterAdd.Varchar(objCommand, "fec_colección", datos.fec_coleccion);
            InputParameterAdd.Varchar(objCommand, "hor_coleccion", datos.hor_coleccion);
            InputParameterAdd.Varchar(objCommand, "fec_validacion", datos.fec_validacion);
            InputParameterAdd.Varchar(objCommand, "rest_prueba", datos.rest_prueba);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", idUsuario);
            InputParameterAdd.Varchar(objCommand, "motivofalla", motivofalla);
            DataTable dataTable = Execute(objCommand);
        }

        private string ObtenerDatosSinEspacios(DataRow row, string columna)
        {
            return string.IsNullOrWhiteSpace(Converter.GetString(row, columna)) ? string.Empty : Converter.GetString(row, columna).Trim();
        }

        public ResultadoCovidPaciente ObtenerResultadoCovidPorOrdenExamen(Guid idOrdenExamen)
        {
            var objCommand = GetSqlCommand("pNLS_CovidResultadobyCodigoOrdenExamen");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", idOrdenExamen);
            DataTable dataTable = Execute(objCommand);
            var resultado = new ResultadoCovidPaciente();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    resultado = new ResultadoCovidPaciente
                    {
                        ape_materno = this.ObtenerDatosSinEspacios(row, "ape_materno"),
                        ape_paterno = this.ObtenerDatosSinEspacios(row, "ape_paterno"),
                        des_departamento = this.ObtenerDatosSinEspacios(row, "des_departamento"),
                        des_distrito = this.ObtenerDatosSinEspacios(row, "des_distrito"),
                        des_provincia = this.ObtenerDatosSinEspacios(row, "des_provincia"),
                        direccion = this.ObtenerDatosSinEspacios(row, "direccion"),
                        email = this.ObtenerDatosSinEspacios(row, "email"),
                        fec_coleccion = this.ObtenerDatosSinEspacios(row, "fec_colecciOn"),
                        fec_validacion = this.ObtenerDatosSinEspacios(row, "fec_validacion"),
                        hor_coleccion = this.ObtenerDatosSinEspacios(row, "hor_coleccion"),
                        //idUbigeoActual = this.ObtenerDatosSinEspacios(row, "idUbigeoActual"),
                        nombres = this.ObtenerDatosSinEspacios(row, "nombres"),
                        nro_celular = this.ObtenerDatosSinEspacios(row, "nro_celular"),
                        nro_documento = this.ObtenerDatosSinEspacios(row, "nro_documento"),
                        rest_prueba = this.ObtenerDatosSinEspacios(row, "rest_prueba"),
                        tip_documento = this.ObtenerDatosSinEspacios(row, "tip_documento"),
                        ubigeo = this.ObtenerDatosSinEspacios(row, "ubigeo"),
                        tip_prueba = this.ObtenerDatosSinEspacios(row, "tip_prueba")
                    };
                }
            }

            return resultado;
        }
        public List<Protocolo> ValidaCodigoLinealProtocolo(string codigoMuestra)
        {
            var objCommand = GetSqlCommand("pNLS_ValidaCodigoLinealProtocolo");
            InputParameterAdd.Varchar(objCommand, "codigoLineal", codigoMuestra);
            DataTable dataTable = Execute(objCommand);
            var lstresultado = new List<Protocolo>();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    var protocolo = new Protocolo
                    {
                        nroSecuencia = Converter.GetInt(row, "nroSecuencia"),
                        codigoMuestra = Converter.GetString(row, "CodigoLineal"),
                        muestra_sin_recepcionar = Converter.GetString(row, "muestra_sin_recepcionar"),
                        muestra_con_resultado = Converter.GetString(row, "muestra_con_resultado")
                    };
                    lstresultado.Add(protocolo);
                }
            }

            return lstresultado;
        }
        public string RegistraCodigoLinealProtocoloMASED(Protocolo oProtocolo, int idUsuario)
        {
            string result = "";
            var objCommand = GetSqlCommand("pNLI_CodigoLinealProtocoloMASED");
            InputParameterAdd.Varchar(objCommand, "codigoLineal", oProtocolo.codigoMuestra);
            InputParameterAdd.Varchar(objCommand, "nroProtocolo",oProtocolo.nroProtocolo);
            InputParameterAdd.Int(objCommand, "tipoMetodo", int.Parse(oProtocolo.kit));
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", idUsuario);
            InputParameterAdd.Varchar(objCommand, "observacion", oProtocolo.observacion);
            try
            {
                ExecuteNonQuery(objCommand);
                result = "ok";
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;

        }
    }

}

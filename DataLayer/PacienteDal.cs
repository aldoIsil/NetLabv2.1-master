using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using Model;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;

namespace DataLayer
{
    public class PacienteDal : DaoBase
    {
        private const String ID_PACIENTE = "idPaciente";
        private const String APELLIDO_PATERNO = "apellidoPaterno";
        private const String APELLIDO_MATERNO = "apellidoMaterno";
        private const String NOMBRES = "nombres";
        private const String FECHA_NACIMIENTO = "fechaNacimiento";
        private const String TIPO_DOCUMENTO = "tipoDocumento";
        private const String NRO_DOCUMENTO = "nroDocumento";
        private const String GENERO = "genero";
        private const String ID_UBIGEO_RENIEC = "idUbigeoReniec";
        private const String DIRECCION_RENIEC = "direccionReniec";
        private const String ID_UBIGEO_ACTUAL = "idUbigeoActual";
        private const String DIRECCION_ACTUAL = "direccionActual";
        private const String TELEFONO_FIJO = "telefonoFijo";
        private const String CELULAR_1 = "celular1";
        private const String CELULAR_2 = "celular2";
        private const String CORREO_ELECTRONICO = "correoElectronico";
        private const String OCUPACION = "ocupacion";
        private const String TIPO_SEGURO_SALUD = "tipoSeguroSalud";
        private const String OTRO_SEGURO_SALUD = "otroSeguroSalud";
        private const String ID_USUARIO_REGISTRO = "idUsuarioRegistro";
        private const String ID_USUARIO_EDICION = "idUsuarioEdicion";
        private const String ESTADO = "estado";
        private const String ESTATUS = "estatus";
        private const String EDAD_ANIOS = "edadAnios";
        private const String EDAD_MESES = "edadMeses";
        private const String COMENTARIO = "comentario";
        private const String ID_ESTABLECIMIENTO = "idestablecimiento";
        private const String mcaDatoTutor = "mcaDatoTutor";
        private const String IDUSUARIO = "idUsuario";

        public PacienteDal(IDalSettings settings) : base(settings)
        {
        }

        public PacienteDal() : this(new NetlabSettings())
        {

        }
        /// <summary>
        /// Descripción: Lista todos los pacientes por tipo de documento y nro de documento, RETORNANDO TODA LA INFORMACION DEL PACIENTE.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        ///                     09/10/2017
        /// Modificación: Se agregaron comentarios.
        ///               JJMR - Se agregaron parametros de búsqueda: apellidoPaterno, apellidoMaterno
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="numRegPorPagima"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <returns>List<Paciente></returns>
        public List<Paciente> GetPacientes(int pagina, int numRegPorPagima, int tipoDocumento, String nroDocumento, String apellidoPaterno, String apellidoMaterno)
        {
            var objCommand = GetSqlCommand("pNLS_PacientesByDocumento");
            InputParameterAdd.Int(objCommand, "tipoDocumento", tipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "apePaterno", apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apeMaterno", apellidoMaterno);
            DataTable dataTable = Execute(objCommand);
            List<Paciente> pacienteList = new List<Paciente>();

            foreach (DataRow row in dataTable.Rows)
            {
                Paciente paciente = new Paciente
                {
                    IdPaciente = Converter.GetGuid(row, PacienteDal.ID_PACIENTE),
                    ApellidoPaterno = Converter.GetString(row, PacienteDal.APELLIDO_PATERNO),
                    ApellidoMaterno = Converter.GetString(row, PacienteDal.APELLIDO_MATERNO),
                    Nombres = Converter.GetString(row, PacienteDal.NOMBRES),
                    FechaNacimiento = Converter.GetDateTime(row, PacienteDal.FECHA_NACIMIENTO),
                    TipoDocumento = Converter.GetInt(row, PacienteDal.TIPO_DOCUMENTO),
                    NroDocumento = Converter.GetString(row, PacienteDal.NRO_DOCUMENTO),
                    DireccionReniec = Converter.GetString(row, PacienteDal.DIRECCION_RENIEC) ?? string.Empty,
                    listaDetalleTipoDocumento = new ListaDetalle { Glosa = Converter.GetString(row, "glosaTipoDocumento") },
                    UbigeoReniec = new Ubigeo { Id = Converter.GetString(row, PacienteDal.ID_UBIGEO_RENIEC) ?? string.Empty }
                };
                pacienteList.Add(paciente);
            }
            return pacienteList;
        }
        public List<Paciente> GetPacientes2(int pagina, int numRegPorPagima, int tipoDocumento, string nroDocumento)
        {
            var objCommand = GetSqlCommand("pNLS_PacientesByDocumento2");
            InputParameterAdd.Int(objCommand, "tipoDocumento", tipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            DataTable dataTable = Execute(objCommand);
            List<Paciente> pacienteList = new List<Paciente>();

            foreach (DataRow row in dataTable.Rows)
            {
                Paciente paciente = new Paciente
                {
                    IdPaciente = Converter.GetGuid(row, PacienteDal.ID_PACIENTE),
                    ApellidoPaterno = Converter.GetString(row, PacienteDal.APELLIDO_PATERNO),
                    ApellidoMaterno = Converter.GetString(row, PacienteDal.APELLIDO_MATERNO),
                    Nombres = Converter.GetString(row, PacienteDal.NOMBRES),
                    FechaNacimiento = Converter.GetDateTime(row, PacienteDal.FECHA_NACIMIENTO),
                    TipoDocumento = Converter.GetInt(row, PacienteDal.TIPO_DOCUMENTO),
                    NroDocumento = Converter.GetString(row, PacienteDal.NRO_DOCUMENTO),
                    DireccionReniec = Converter.GetString(row, PacienteDal.DIRECCION_RENIEC) ?? string.Empty,
                    listaDetalleTipoDocumento = new ListaDetalle { Glosa = Converter.GetString(row, "glosaTipoDocumento") },
                    UbigeoReniec = new Ubigeo { Id = Converter.GetString(row, PacienteDal.ID_UBIGEO_RENIEC) ?? string.Empty }
                };
                pacienteList.Add(paciente);
            }
            return pacienteList;
        }
        /// <summary>
        /// Descripción: Lista todos los pacientes por tipo de documento y nro de documento, RETORNANDO SOLO EL ID DEL PACIENTE.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public List<Paciente> GetPacientesByTipoNroDocumento(int tipoDocumento, String nroDocumento)
        {
            var objCommand = GetSqlCommand("pNLS_GetPacientesByTipoNroDocumento");
            InputParameterAdd.Int(objCommand, "tipoDocumento", tipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            DataTable dataTable = Execute(objCommand);
            List<Paciente> pacienteList = new List<Paciente>();

            foreach (DataRow row in dataTable.Rows)
            {
                Paciente paciente = new Paciente
                {
                    IdPaciente = Converter.GetGuid(row, PacienteDal.ID_PACIENTE),
                    NroDocumento = Converter.GetString(row, "nroDocumento"),

                };
                pacienteList.Add(paciente);
            }
            return pacienteList;
        }

        /// <summary>
        /// Descripción: Lista todos los pacientes por validar  con el web service de la reniec.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="numRegPorPagima"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        public List<Paciente> GetPacientesSinValidar(int pagina, int numRegPorPagima, DateTime fechaDesde, DateTime fechaHasta)
        {
            var objCommand = GetSqlCommand("pNLS_PacientesSinValidarByDocumento");
            InputParameterAdd.DateTime(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "fechaHasta", fechaHasta);
            DataTable dataTable = Execute(objCommand);
            List<Paciente> pacienteList = new List<Paciente>();

            foreach (DataRow row in dataTable.Rows)
            {
                Paciente paciente = new Paciente
                {
                    IdPaciente = Converter.GetGuid(row, PacienteDal.ID_PACIENTE),
                    ApellidoPaterno = Converter.GetString(row, PacienteDal.APELLIDO_PATERNO),
                    ApellidoMaterno = Converter.GetString(row, PacienteDal.APELLIDO_MATERNO),
                    Nombres = Converter.GetString(row, PacienteDal.NOMBRES),
                    FechaNacimiento = Converter.GetDateTime(row, PacienteDal.FECHA_NACIMIENTO),
                    TipoDocumento = Converter.GetInt(row, PacienteDal.TIPO_DOCUMENTO),
                    NroDocumento = Converter.GetString(row, PacienteDal.NRO_DOCUMENTO),
                    DireccionReniec = Converter.GetString(row, PacienteDal.DIRECCION_RENIEC) ?? string.Empty,
                    listaDetalleTipoDocumento = new ListaDetalle { Glosa = Converter.GetString(row, "glosaTipoDocumento") },
                    UbigeoReniec = new Ubigeo { Id = Converter.GetString(row, PacienteDal.ID_UBIGEO_RENIEC) ?? string.Empty }
                };
                pacienteList.Add(paciente);
            }
            return pacienteList;
        }
        /// <summary>
        /// Descripción: Lista todos los pacientes por Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        public Paciente GetPacienteById(Guid idPaciente)
        {
            var objCommand = GetSqlCommand("pNLS_PacienteById");
            InputParameterAdd.Guid(objCommand, PacienteDal.ID_PACIENTE, idPaciente);
            DataTable dataTable = Execute(objCommand);

            Paciente paciente = null;
            foreach (DataRow row in dataTable.Rows)
            {
                paciente = new Paciente
                {
                    IdPaciente = idPaciente,
                    ApellidoPaterno = Converter.GetString(row, PacienteDal.APELLIDO_PATERNO),
                    ApellidoMaterno = Converter.GetString(row, PacienteDal.APELLIDO_MATERNO),
                    Nombres = Converter.GetString(row, PacienteDal.NOMBRES),
                    Genero = Converter.GetInt(row, PacienteDal.GENERO),
                    FechaNacimiento = Converter.GetDateTime(row, PacienteDal.FECHA_NACIMIENTO),
                    TipoDocumento = Converter.GetInt(row, PacienteDal.TIPO_DOCUMENTO),
                    NroDocumento = Converter.GetString(row, PacienteDal.NRO_DOCUMENTO),
                    UbigeoReniec = new Ubigeo
                    {
                        Id = Converter.GetString(row, PacienteDal.ID_UBIGEO_RENIEC),
                        Departamento = Converter.GetString(row, "departamentoReniec"),
                        Provincia = Converter.GetString(row, "provinciaReniec"),
                        Distrito = Converter.GetString(row, "distritoReniec"),
                    },
                    DireccionReniec = Converter.GetString(row, PacienteDal.DIRECCION_RENIEC),
                    UbigeoActual = new Ubigeo
                    {
                        Id = Converter.GetString(row, PacienteDal.ID_UBIGEO_ACTUAL),
                        Departamento = Converter.GetString(row, "departamentoActual"),
                        Provincia = Converter.GetString(row, "provinciaActual"),
                        Distrito = Converter.GetString(row, "distritoActual")
                    },
                    DireccionActual = Converter.GetString(row, PacienteDal.DIRECCION_ACTUAL),
                    TelefonoFijo = Converter.GetString(row, PacienteDal.TELEFONO_FIJO),
                    Celular1 = Converter.GetString(row, PacienteDal.CELULAR_1),
                    Celular2 = Converter.GetString(row, PacienteDal.CELULAR_2),
                    correoElectronico = Converter.GetString(row, PacienteDal.CORREO_ELECTRONICO),
                    ocupacion = Converter.GetString(row, PacienteDal.OCUPACION),
                    tipoSeguroSalud = Converter.GetInt(row, PacienteDal.TIPO_SEGURO_SALUD),
                    otroSeguroSalud = Converter.GetString(row, PacienteDal.OTRO_SEGURO_SALUD),
                    tipoSeguro = Converter.GetString(row, "tipoSeguro"),
                    estatus = Converter.GetInt(row, PacienteDal.ESTATUS),
                    //edadAnios = Converter.GetInt(row,PacienteDal.EDAD_ANIOS),
                    edadAnios = Converter.GetInt(row, "Edad"),
                    edadMeses = Converter.GetInt(row, PacienteDal.EDAD_MESES),
                    mcaDatoTutor = Converter.GetInt(row, PacienteDal.mcaDatoTutor),
                    EstatusE = Converter.GetInt(row, "estatusE")
                };
            }
            return paciente;
        }
        /// <summary>
        /// Descripción: Registra la informacion del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="paciente"></param>
        /// <returns></returns>
        public Guid InsertPaciente(Paciente paciente)
        {
            var objCommand = GetSqlCommand("pNLI_Paciente");
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_PATERNO, paciente.ApellidoPaterno);
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_MATERNO, paciente.ApellidoMaterno);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NOMBRES, paciente.Nombres);
            InputParameterAdd.Int(objCommand, PacienteDal.GENERO, paciente.Genero);
            InputParameterAdd.DateTime(objCommand, PacienteDal.FECHA_NACIMIENTO, paciente.FechaNacimiento);
            InputParameterAdd.Int(objCommand, PacienteDal.TIPO_DOCUMENTO, paciente.TipoDocumento);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NRO_DOCUMENTO, paciente.NroDocumento);
            InputParameterAdd.Varchar(objCommand, PacienteDal.ID_UBIGEO_RENIEC, paciente.UbigeoReniec.Id);
            InputParameterAdd.Varchar(objCommand, PacienteDal.DIRECCION_RENIEC, paciente.DireccionReniec);
            InputParameterAdd.Varchar(objCommand, PacienteDal.ID_UBIGEO_ACTUAL, paciente.UbigeoActual.Id);
            InputParameterAdd.Varchar(objCommand, PacienteDal.DIRECCION_ACTUAL, paciente.DireccionActual);
            InputParameterAdd.Varchar(objCommand, PacienteDal.TELEFONO_FIJO, paciente.TelefonoFijo);
            InputParameterAdd.Varchar(objCommand, PacienteDal.CELULAR_1, paciente.Celular1);
            InputParameterAdd.Varchar(objCommand, PacienteDal.CELULAR_2, paciente.Celular2);
            InputParameterAdd.Varchar(objCommand, PacienteDal.CORREO_ELECTRONICO, paciente.correoElectronico);
            InputParameterAdd.Varchar(objCommand, PacienteDal.OCUPACION, paciente.ocupacion);
            InputParameterAdd.Int(objCommand, PacienteDal.TIPO_SEGURO_SALUD, paciente.tipoSeguroSalud);
            InputParameterAdd.Varchar(objCommand, PacienteDal.OTRO_SEGURO_SALUD, paciente.otroSeguroSalud);
            InputParameterAdd.Int(objCommand, PacienteDal.ESTATUS, paciente.estatus);
            InputParameterAdd.Int(objCommand, PacienteDal.ID_USUARIO_REGISTRO, paciente.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, PacienteDal.mcaDatoTutor, paciente.mcaDatoTutor);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdPaciente");
            ExecuteNonQuery(objCommand);
            return (Guid)objCommand.Parameters["@newIdPaciente"].Value;
        }
        /// <summary>
        /// Descripción: Actualiza toda la informacion del paciente
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="paciente"></param>
        public void UpdatePaciente(Paciente paciente)
        {
            var objCommand = GetSqlCommand("pNLU_Paciente");
            InputParameterAdd.Guid(objCommand, PacienteDal.ID_PACIENTE, paciente.IdPaciente);
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_PATERNO, paciente.ApellidoPaterno);
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_MATERNO, paciente.ApellidoMaterno);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NOMBRES, paciente.Nombres);
            InputParameterAdd.Int(objCommand, PacienteDal.GENERO, paciente.Genero);
            InputParameterAdd.DateTime(objCommand, PacienteDal.FECHA_NACIMIENTO, paciente.FechaNacimiento);
            InputParameterAdd.Int(objCommand, PacienteDal.TIPO_DOCUMENTO, paciente.TipoDocumento);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NRO_DOCUMENTO, paciente.NroDocumento);
            /*   InputParameterAdd.Varchar(objCommand, PacienteDal.ID_UBIGEO_RENIEC, paciente.UbigeoReniec.Id);
                 InputParameterAdd.Varchar(objCommand, PacienteDal.DIRECCION_RENIEC, paciente.DireccionReniec);*/
            InputParameterAdd.Varchar(objCommand, PacienteDal.ID_UBIGEO_ACTUAL, paciente.UbigeoActual.Id);
            InputParameterAdd.Varchar(objCommand, PacienteDal.DIRECCION_ACTUAL, paciente.DireccionActual);
            InputParameterAdd.Varchar(objCommand, PacienteDal.TELEFONO_FIJO, paciente.TelefonoFijo);
            InputParameterAdd.Varchar(objCommand, PacienteDal.CELULAR_1, paciente.Celular1);
            InputParameterAdd.Varchar(objCommand, PacienteDal.CELULAR_2, paciente.Celular2);
            InputParameterAdd.Varchar(objCommand, PacienteDal.CORREO_ELECTRONICO, paciente.correoElectronico);
            InputParameterAdd.Varchar(objCommand, PacienteDal.OCUPACION, paciente.ocupacion);
            InputParameterAdd.Int(objCommand, PacienteDal.TIPO_SEGURO_SALUD, paciente.tipoSeguroSalud);
            InputParameterAdd.Varchar(objCommand, PacienteDal.OTRO_SEGURO_SALUD, paciente.otroSeguroSalud);
            InputParameterAdd.Int(objCommand, PacienteDal.ESTADO, 1);
            InputParameterAdd.Int(objCommand, PacienteDal.ESTATUS, paciente.estatus);
            InputParameterAdd.Int(objCommand, PacienteDal.ID_USUARIO_EDICION, paciente.IdUsuarioEdicion);
            ExecuteNonQuery(objCommand);
        }

        /// <summary>
        /// Descripción: Actualiza solo el ubigeo del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="paciente"></param>
        public void UpdateDatosPaciente(Paciente paciente)
        {
            var objCommand = GetSqlCommand("pNLU_PacienteDireccionActual");
            InputParameterAdd.Guid(objCommand, PacienteDal.ID_PACIENTE, paciente.IdPaciente);
            InputParameterAdd.Varchar(objCommand, PacienteDal.ID_UBIGEO_ACTUAL, paciente.UbigeoActual.Id);
            InputParameterAdd.Varchar(objCommand, PacienteDal.DIRECCION_ACTUAL, paciente.DireccionActual);
            InputParameterAdd.Varchar(objCommand, PacienteDal.TELEFONO_FIJO, paciente.TelefonoFijo);
            InputParameterAdd.Varchar(objCommand, PacienteDal.CELULAR_1, paciente.Celular1);
            InputParameterAdd.Varchar(objCommand, PacienteDal.CELULAR_2, paciente.Celular2);
            InputParameterAdd.Varchar(objCommand, PacienteDal.CORREO_ELECTRONICO, paciente.correoElectronico);
            InputParameterAdd.Varchar(objCommand, PacienteDal.OCUPACION, paciente.ocupacion);
            InputParameterAdd.Int(objCommand, PacienteDal.TIPO_SEGURO_SALUD, paciente.tipoSeguroSalud);
            InputParameterAdd.Varchar(objCommand, PacienteDal.OTRO_SEGURO_SALUD, paciente.otroSeguroSalud);
            InputParameterAdd.Int(objCommand, PacienteDal.ID_USUARIO_EDICION, paciente.IdUsuarioEdicion);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza datos del paciente obtenidos del web service de la reniec.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="paciente"></param>
        public void UpdateDatosReniec(Paciente paciente)
        {
            var objCommand = GetSqlCommand("pNLU_PacienteDatosReniec");
            InputParameterAdd.Guid(objCommand, PacienteDal.ID_PACIENTE, paciente.IdPaciente);
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_PATERNO, paciente.ApellidoPaterno);
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_MATERNO, paciente.ApellidoMaterno);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NOMBRES, paciente.Nombres);
            InputParameterAdd.Varchar(objCommand, PacienteDal.DIRECCION_RENIEC, paciente.DireccionReniec);
            InputParameterAdd.Varchar(objCommand, PacienteDal.ID_UBIGEO_RENIEC, paciente.UbigeoReniec.Id);
            InputParameterAdd.Int(objCommand, PacienteDal.GENERO, paciente.Genero);
            InputParameterAdd.DateTime(objCommand, PacienteDal.FECHA_NACIMIENTO, paciente.FechaNacimiento);
            InputParameterAdd.Int(objCommand, PacienteDal.ID_USUARIO_EDICION, paciente.IdUsuarioEdicion);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Lee datos del paciente en los laboratorios.
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/11/2017
        /// Fecha Modificación: 
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public List<Paciente> GetDatosPacienteByNroDocumento(string nroDocumento, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_DatosPacienteByNroDocumento");
            InputParameterAdd.Varchar(objCommand, PacienteDal.NRO_DOCUMENTO, nroDocumento);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            //DataTable dataTable = Execute(objCommand);

            DataSet dataSet = ExecuteDataSet(objCommand);
            DataTable dataTable = dataSet.Tables[0];
            //DataTable prKobo = dataSet.Tables[1];

            List<Paciente> pacienteList = new List<Paciente>();

            Paciente paciente = null;
            foreach (DataRow row in dataTable.Rows)
            {
                paciente = new Paciente
                {
                    Enfermedad = Converter.GetString(row, "Enfermedad"),
                    FechaObtencion = Converter.GetDateTime(row, "FechaObtencion"),
                    FechaValidacion = Converter.GetString(row, "FechaValidado"),
                    idExamen = Converter.GetString(row, "idExamen"),
                    Examen = Converter.GetString(row, "Examen"),
                    Componente = Converter.GetString(row, "Componente"),
                    Resultado = Converter.GetString(row, "Resultado"),
                    //Resultado = RetornaResultado(new DetalleAnalitoDal().GetAnalitosbyIdAnalitoResultado(new DalConverter.SerializarResultado().DeserializarResultados(row["RESULTADO"].ToString()))),
                    EESSOrigen = Converter.GetString(row, "EstablecimientoOrigen"),
                    EstablecimientoDestino = Converter.GetString(row, "EstablecimientoDestino"),
                    EstadoResultado = Converter.GetString(row, "EstatusResultado"),
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    idEstablecimiento = Converter.GetInt(row, "idLaboratorioDestino"),
                    EstatusE = Converter.GetInt(row, "EstatusE"),
                    CodigoOrden = Converter.GetString(row, "Orden"),
                    NroOficio = Converter.GetString(row, "NroOficio"),
                    CriteriosRechazo = Converter.GetString(row, "CriterioRechazo"),
                    TienePruebaRapidaSiscovid = Converter.GetInt(row, "TienePruebaRapidaSiscovid")
                };
                pacienteList.Add(paciente);
            }

            //if (prKobo != null && prKobo.Rows.Count > 0)
            //{
            //    foreach (DataRow row2 in prKobo.Rows)
            //    {
            //        paciente = new Paciente
            //        {
            //            Enfermedad = Converter.GetString(row2, "Enfermedad"),
            //            Examen = Converter.GetString(row2, "Examen"),
            //            Componente = Converter.GetString(row2, "Componente"),
            //            estatus = Converter.GetInt(row2, "idPruebaRapidaKobo"),
            //            //tipoDocumen = Converter.GetString(row2, "tipoDocumento"),
            //            //NroDocumento = Converter.GetString(row2, "nroDocumento"),
            //            //Nombres = Converter.GetString(row2, "Nombres"),
            //            //ApellidoPaterno = Converter.GetString(row2, "apellidoPaterno"),
            //            //ApellidoMaterno = Converter.GetString(row2, "apellidoMaterno"),
            //            //FechaNacimiento = Converter.GetDateTime(row2, "fechaNacimiento"),
            //            //UsuarioReferencia = Converter.GetString(row2, "ejecutorPrueba"),
            //            EESSOrigen = Converter.GetString(row2, "establecimientoEjecutor"),
            //            EstablecimientoDestino = Converter.GetString(row2, "establecimientoEjecutor"),
            //            FechaObtencion = Converter.GetDateTime(row2, "fechaObtencion"),
            //            FechaValidacion = Converter.GetString(row2, "FechaResultado"),
            //            Resultado = Converter.GetString(row2, "Resultado"),
            //            EstadoResultado = Converter.GetString(row2, "EstatusResultado"),
            //        };
            //        pacienteList.Add(paciente);
            //    }
            //}


            return pacienteList;
        }

        public string RetornaResultado(List<AnalitoOpcionResultado> ListaResultadoJson)
        {
            string ResultadoFinal = "";
            string GlosaPadre = "";
            var CodigoParent = "";
            var printPadre = 0;
            foreach (var opcion in ListaResultadoJson.OrderBy(a => a.IdAnalitoOpcionParent).ToList())
            {
                if (opcion.Tipo == 1 || opcion.Tipo == 2 || opcion.Tipo == 5)
                {
                    if (CodigoParent != opcion.IdAnalitoOpcionParent && opcion.IdAnalitoOpcionParent.ToString() == opcion.IdAnalito.ToString().ToUpper())
                    {
                        CodigoParent = opcion.IdAnalitoOpcionParent;
                        if (printPadre == 0)
                        {
                            ResultadoFinal = ResultadoFinal + " " + opcion.GlosaParent + " - ";
                            printPadre++;
                        }
                        string IdAnalitoOpcionParent = opcion.IdAnalitoOpcionParent;
                    }
                    foreach (var ValOpcion in ListaResultadoJson.Where(p => p.IdAnalitoOpcionParent == opcion.IdAnalitoOpcionParent).ToList())
                    {
                        foreach (var ValOpcion2 in ListaResultadoJson.Where(p => p.IdAnalitoOpcionParent.ToString() == ValOpcion.IdAnalitoOpcionResultado.ToString()).ToList())
                        {
                            ResultadoFinal = ResultadoFinal + " " + ValOpcion2.Glosa + " - ";
                            foreach (var ValOpcion3 in ListaResultadoJson.Where(p => p.IdAnalitoOpcionParent.ToString() == ValOpcion2.IdAnalitoOpcionResultado.ToString()).ToList())
                            {
                                if (!String.IsNullOrEmpty(ValOpcion3.Glosa))
                                {
                                    ResultadoFinal = ResultadoFinal + " " + ValOpcion3.Glosa + " - ";
                                }
                                foreach (var ValOpcion4 in ListaResultadoJson.Where(p => p.IdAnalitoOpcionParent.ToString() == ValOpcion3.IdAnalitoOpcionResultado.ToString()).ToList())
                                {
                                    if (!String.IsNullOrEmpty(ValOpcion4.Glosa))
                                    {
                                        ResultadoFinal = ResultadoFinal + " " + ValOpcion4.Glosa + " - ";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    GlosaPadre = opcion.GlosaParent;
                    if (CodigoParent != opcion.IdAnalitoOpcionParent && opcion.IdAnalitoOpcionParent.ToString() == opcion.IdAnalito.ToString().ToUpper())
                    {
                        CodigoParent = opcion.IdAnalitoOpcionParent;
                        if (printPadre == 0)
                        {
                            ResultadoFinal = ResultadoFinal + " " + opcion.GlosaParent + " - ";
                            printPadre++;
                        }
                        foreach (var ValOpcion in ListaResultadoJson.Where(p => p.IdAnalitoOpcionParent == opcion.IdAnalitoOpcionParent).ToList())
                        {
                            foreach (var ValOpcion2 in ListaResultadoJson.Where(p => p.IdAnalitoOpcionParent.ToString() == ValOpcion.IdAnalitoOpcionResultado.ToString()).ToList())
                            {
                                ResultadoFinal = ResultadoFinal + " " + ValOpcion2.Glosa + " - ";
                                foreach (var ValOpcion3 in ListaResultadoJson.Where(p => p.IdAnalitoOpcionParent.ToString() == ValOpcion2.IdAnalitoOpcionResultado.ToString()).ToList())
                                {
                                    if (!String.IsNullOrEmpty(ValOpcion3.Glosa))
                                    {
                                        ResultadoFinal = ResultadoFinal + " " + ValOpcion3.Glosa + " - ";
                                    }
                                    foreach (var ValOpcion4 in ListaResultadoJson.Where(p => p.IdAnalitoOpcionParent.ToString() == ValOpcion3.IdAnalitoOpcionResultado.ToString()).ToList())
                                    {
                                        if (!String.IsNullOrEmpty(ValOpcion4.Glosa))
                                        {
                                            ResultadoFinal = ResultadoFinal + " " + ValOpcion4.Glosa;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ResultadoFinal;
        }
        /// <summary>
        /// Descripción: Lee datos del paciente en NetLab v1.0.
        /// Author: Juan Muga.
        /// Fecha Creacion: 20/08/2018
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <param name="cCodMuestra"></param>
        /// <returns></returns>
        public List<Paciente> GetDatosPacienteNetLab1(string nroDocumento, string cCodMuestra)
        {
            var lstpaciente = new List<Paciente>();
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionNetLabPrueba1"].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("NFL_ConsultaPacienteNetLab2", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@sNumeroDocumento", System.Data.SqlDbType.VarChar)).Value = nroDocumento;
                    comando.Parameters.Add(new SqlParameter("@cCodMuestra", System.Data.SqlDbType.VarChar)).Value = cCodMuestra;

                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        lstpaciente.Add(new Paciente()
                        {
                            ApellidoMaterno = reader.GetString(reader.GetOrdinal("sApellidoMaterno")),
                            ApellidoPaterno = reader.GetString(reader.GetOrdinal("sApellidoPaterno")),
                            Nombres = reader.GetString(reader.GetOrdinal("sNombres")),
                            sApellidoMaterno = reader.GetString(reader.GetOrdinal("sApellidoMaterno")),
                            sApellidoPaterno = reader.GetString(reader.GetOrdinal("sApellidoPaterno")),
                            sNombres = reader.GetString(reader.GetOrdinal("sNombres")),
                            sNumeroDocumento = reader.GetString(reader.GetOrdinal("sNumeroDocumento")),
                            cCodMuestra = reader.GetString(reader.GetOrdinal("cCodMuestra")),
                            dFechaObtencionMuestra = reader.GetDateTime(reader.GetOrdinal("dFechaObtencionMuestra")),
                            Renipress = reader.GetString(reader.GetOrdinal("Renipress")),
                            NombreEESS = reader.GetString(reader.GetOrdinal("NombresEESS")),
                            sEnfermedad = reader.GetString(reader.GetOrdinal("Enfermedad")),
                            Prueba = reader.GetString(reader.GetOrdinal("Prueba")),
                            NombreVariable = reader.GetString(reader.GetOrdinal("NombreVariable")),
                            sResultado = reader.GetString(reader.GetOrdinal("Resultado")),
                            cCodPrueba = reader.GetInt32(reader.GetOrdinal("cCodPrueba"))
                        });
                    }
                    conexion.Close();
                    conexion.Dispose();
                }

            }
            return lstpaciente;
        }


        /// Descripción: Busca historial de los pacientes NetLab v2.0.
        /// Author: Yonatan Clemente
        /// Fecha Creacion: 08/05/2018

        public List<Paciente> ObtenerHistorialPaciente(int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_BuscarHistorialPaciente");
            InputParameterAdd.Int(objCommand, "tipoDocumento", tipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "apePaterno", apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apeMaterno", apellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);
            DataTable dataTable = Execute(objCommand);
            List<Paciente> LisPaciente = new List<Paciente>();

            if (dataTable.Rows.Count == 0)
            {
                return LisPaciente;
            }

            LisPaciente = (from DataRow row in dataTable.Rows
                           select new Paciente
                           {
                               IdPaciente = Converter.GetGuid(row, PacienteDal.ID_PACIENTE),
                               ApellidoPaterno = row["apellidoPaterno"].ToString(),
                               ApellidoMaterno = row["apellidoMaterno"].ToString(),
                               Nombres = row["nombres"].ToString(),
                               tipoDocumentoNombre = row["glosaTipoDocumento"].ToString(),
                               NroDocumento = row["nroDocumento"].ToString()
                           }).ToList();

            return LisPaciente;
        }

        /// <summary>
        /// Variante de metodo ObtenerHistorialPaciente agregando parametro idUsuario para la busqueda por el tipo de acceso
        /// </summary>
        /// <param name="tipoDocumento"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <param name="nombre"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Paciente> ObtenerHistorialPacientePorUsuario(int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_BuscarHistorialPacientePorUsuario");
            InputParameterAdd.Int(objCommand, "tipoDocumento", tipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "apePaterno", apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apeMaterno", apellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            DataTable dataTable = Execute(objCommand);
            List<Paciente> LisPaciente = new List<Paciente>();

            if (dataTable.Rows.Count == 0)
            {
                return LisPaciente;
            }

            LisPaciente = (from DataRow row in dataTable.Rows
                           select new Paciente
                           {
                               IdPaciente = Converter.GetGuid(row, PacienteDal.ID_PACIENTE),
                               ApellidoPaterno = row["apellidoPaterno"].ToString(),
                               ApellidoMaterno = row["apellidoMaterno"].ToString(),
                               Nombres = row["nombres"].ToString(),
                               tipoDocumentoNombre = row["glosaTipoDocumento"].ToString(),
                               NroDocumento = row["nroDocumento"].ToString(),
                               TienePruebaRapidaSiscovid = Converter.GetInt(row, "TienePruebaRapidaSiscovid")
                           }).ToList();

            var agrupados = LisPaciente
                    .GroupBy(u => u.NroDocumento)
                    .Select(grp => grp.First())
                    .ToList();

            //return LisPaciente;
            return agrupados;
        }

        public SolicitudExamen GetResultadoPacienteVIH(string idPaciente, int tipoSolicitud)
        {
            SolicitudExamen paciente = new SolicitudExamen();
            paciente.Resultado = new ExamenResultadoDetalle();
            var objCommand = GetSqlCommand("pNLS_ObtenerResultadosPacienteVIH");
            InputParameterAdd.Varchar(objCommand, "idPaciente", idPaciente);
            DataSet dataSet = ExecuteDataSet(objCommand);

            DataTable DataPaciente = dataSet.Tables[0];
            DataTable DataCV = dataSet.Tables[1];
            DataTable DataCD = dataSet.Tables[2];
            DataTable Drogas = dataSet.Tables[3];
            DataTable numSol = dataSet.Tables[4];

            if (DataPaciente.Rows.Count > 0)
            {
                var row = DataPaciente.Rows[0];
                paciente.Paciente = new Paciente
                {
                    IdPaciente = Converter.GetGuid(row, PacienteDal.ID_PACIENTE),
                    ApellidoPaterno = Converter.GetString(row, PacienteDal.APELLIDO_PATERNO),
                    ApellidoMaterno = Converter.GetString(row, PacienteDal.APELLIDO_MATERNO),
                    Nombres = Converter.GetString(row, PacienteDal.NOMBRES),
                    generoNombre = Converter.GetString(row, PacienteDal.GENERO),
                    FechaNacimiento = Converter.GetDateTime(row, PacienteDal.FECHA_NACIMIENTO),
                    NroDocumento = Converter.GetString(row, PacienteDal.NRO_DOCUMENTO),
                    UbigeoActual = new Ubigeo
                    {
                        Id = Converter.GetString(row, PacienteDal.ID_UBIGEO_ACTUAL),
                        Departamento = Converter.GetString(row, "departamentoActual"),
                        Provincia = Converter.GetString(row, "provinciaActual"),
                        Distrito = Converter.GetString(row, "distritoActual")
                    },
                    DireccionActual = Converter.GetString(row, PacienteDal.DIRECCION_ACTUAL),
                    Celular1 = Converter.GetString(row, PacienteDal.CELULAR_1),
                    edadAnios = Converter.GetInt(row, "Edad"),
                };
            }

            if (DataCV.Rows.Count > 0)
            {
                var cv = DataCV.Rows[0];
                paciente.Resultado.Resultado = Converter.GetString(cv, "Cv");
                paciente.Resultado.FechaHoraEmision = Converter.GetString(cv, "fechaResultado");
                paciente.CodigoOrden = Converter.GetString(cv, "codigoOrden");
            }
            else
            {
                paciente.Resultado.Resultado = "No presenta";
                paciente.Resultado.FechaHoraEmision = "No presenta";
                paciente.pResultado = "No presenta";
                paciente.pFechaResultado = "No presenta";
            }

            if (DataCD.Rows.Count > 0)
            {
                var cd = DataCD.Rows[0];
                paciente.ResultadoCD4 = Converter.GetString(cd, "CD4");
                paciente.FechaResultadoCD4 = Converter.GetString(cd, "fechaResultado");
                paciente.CodigoCD4 = Converter.GetString(cd, "codigoOrden");
            }
            else
            {
                paciente.ResultadoCD4 = "No presenta";
                paciente.FechaResultadoCD4 = "No presenta";
                paciente.CodigoCD4 = "No presenta";
            }

            if (Drogas.Rows.Count > 0)
            {
                paciente.ListaDrogas = (from DataRow row in Drogas.Rows
                                        select new DrogaGeno
                                        {
                                            idDroga = (int)row["idDroga"],
                                            nombreDroga = row["nombreDroga"].ToString(),
                                            estado = (int)row["estado"],
                                            idGrupo = (int)row["idGrupo"]
                                        }).ToList();
            }
            var sol = numSol.Rows[0];
            paciente.numeroSolicitud = Converter.GetInt(sol, "numeroSolicitud");

            return paciente;
        }
        public void RegistroSolicitudGenotipificacion(SolicitudExamen solicitud)
        {
            try
            {
                var objCommand = GetSqlCommand("pNLI_RegistrarDatosGenotipificacion");
                InputParameterAdd.Int(objCommand, "numeroSolicitud", solicitud.numeroSolicitud);
                InputParameterAdd.Guid(objCommand, "idPaciente", solicitud.Paciente.IdPaciente);
                InputParameterAdd.Int(objCommand, "idCriterio", solicitud.Criterio);
                InputParameterAdd.Varchar(objCommand, "CodigoOrden", solicitud.CodigoOrden);
                InputParameterAdd.Int(objCommand, "idDroga", solicitud.drogas.idDroga);
                InputParameterAdd.Varchar(objCommand, "valor", solicitud.drogas.valor);
                InputParameterAdd.Int(objCommand, "idUsuario", solicitud.Solicitante.idSolicitante);
                InputParameterAdd.Int(objCommand, "idEstablecimiento", solicitud.Establecimiento.IdEstablecimiento);
                ExecuteNonQuery(objCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegistroSolicitudTropismo(SolicitudExamen solicitud)
        {
            try
            {
                var objCommand = GetSqlCommand("pNLI_RegistrarDatosTropismo");
                InputParameterAdd.Int(objCommand, "numeroSolicitud", solicitud.numeroSolicitud);
                InputParameterAdd.Guid(objCommand, "idPaciente", solicitud.Paciente.IdPaciente);
                InputParameterAdd.Int(objCommand, "tipoSolicitud", solicitud.tipoSolicitud);
                InputParameterAdd.Int(objCommand, "idCriterio", solicitud.Criterio);
                InputParameterAdd.Varchar(objCommand, "CodigoOrden", solicitud.CodigoOrden);
                InputParameterAdd.Int(objCommand, "idUsuario", solicitud.Solicitante.idSolicitante);
                InputParameterAdd.Int(objCommand, "idEstablecimiento", solicitud.Establecimiento.IdEstablecimiento);
                ExecuteNonQuery(objCommand);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Descripción: Registra referencia de un paciente
        /// Author: Jose Chavez
        /// Fecha Creacion: 15/07/2019
        /// </summary>
        /// <param name="paciente"></param>
        /// <returns></returns>
        public int InsertPacienteReferencia(Paciente paciente, int idestablecimiento)
        {
            int res = 1;
            var objCommand = GetSqlCommand("pNLI_PacienteReferencia");
            InputParameterAdd.Guid(objCommand, PacienteDal.ID_PACIENTE, paciente.IdPaciente);
            InputParameterAdd.Varchar(objCommand, PacienteDal.COMENTARIO, paciente.comentario);
            InputParameterAdd.Int(objCommand, "idusuarioregistro", paciente.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idestablecimientoOrigen", idestablecimiento);
            InputParameterAdd.Int(objCommand, "idestablecimientoDestino", paciente.idEstablecimiento);
            try
            {
                ExecuteNonQuery(objCommand);
            }
            catch (Exception ex)
            {
                res = 0;
            }
            return res;
        }
        /// <summary>
        /// Descripción: Busca las referencias y contrareferencias de un paciente
        /// Author: Jose Chavez
        /// Fecha Creacion: 15/07/2019
        /// </summary>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        public Paciente GetPacienteReferencia(string idPaciente)
        {
            var objCommand = GetSqlCommand("pNLS_PacienteReferencia");
            InputParameterAdd.Varchar(objCommand, "idPaciente", idPaciente);
            DataTable dataTable = Execute(objCommand);
            Paciente xPaciente = new Paciente();

            if (dataTable.Rows.Count == 0)
            {
                return xPaciente;
            }
            xPaciente.IdPaciente = Guid.Parse(idPaciente);
            xPaciente.ListPaciente = (from DataRow row in dataTable.Rows
                                      select new Paciente
                                      {
                                          FechaReferencia = Convert.ToDateTime(row["Fecha_Referencia"].ToString()),
                                          //Converter.GetDateTime(row["Fecha_Referencia"].ToString(),),
                                          EstablecimientoOrigen = row["Establecimiento_Origen"].ToString(),
                                          EstablecimientoDestino = row["Establecimiento_Referencia"].ToString(),
                                          comentario = row["Observacion"].ToString(),
                                          UsuarioReferencia = row["Usuario_Referencia"].ToString()
                                      }).ToList();
            return xPaciente;
        }
        /// <summary>
        /// Descripción: Lee datos del paciente en los laboratorios.
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/11/2017
        /// Fecha Modificación: 
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public List<Paciente> GetDatosPacienteByNroDocumentoCoronavirus(string nroDocumento, string nombres, string apePat, string apeMat, string idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_DatosPacienteByNroDocumentoCoronavirus");
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_PATERNO, apePat);
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_MATERNO, apeMat);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NOMBRES, nombres);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NRO_DOCUMENTO, nroDocumento);
            InputParameterAdd.Varchar(objCommand, PacienteDal.IDUSUARIO, idUsuario);
            //DataTable dataTable = Execute(objCommand);
            DataSet PacienteCovid = ExecuteDataSet(objCommand);
            DataTable dataTable = PacienteCovid.Tables[0];
            DataTable dataTable2 = PacienteCovid.Tables[1];
            List<Paciente> pacienteList = new List<Paciente>();

            Paciente paciente = null;
            foreach (DataRow row in dataTable.Rows)
            {
                paciente = new Paciente
                {
                    Enfermedad = Converter.GetString(row, "Enfermedad"),
                    FechaObtencion = Converter.GetDateTime(row, "FechaObtencion"),
                    FechaValidacion = Converter.GetString(row, "FechaValidado"),
                    idExamen = Converter.GetString(row, "idExamen"),
                    Examen = Converter.GetString(row, "Examen"),
                    Componente = Converter.GetString(row, "Componente"),
                    Resultado = Converter.GetString(row, "Resultado"),
                    EESSOrigen = Converter.GetString(row, "EstablecimientoOrigen"),
                    EstablecimientoDestino = Converter.GetString(row, "EstablecimientoDestino"),
                    EstadoResultado = Converter.GetString(row, "EstatusResultado"),
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    idEstablecimiento = Converter.GetInt(row, "idLaboratorioDestino"),
                    EstatusE = Converter.GetInt(row, "EstatusE"),
                    CodigoOrden = Converter.GetString(row, "Orden"),
                    NroOficio = Converter.GetString(row, "NroOficio"),
                    CriteriosRechazo = Converter.GetString(row, "CriterioRechazo"),
                    Nombres = Converter.GetString(row, "Paciente")
                };
                pacienteList.Add(paciente);
            }

            if (dataTable2 != null && dataTable2.Rows.Count > 0)
            {
                foreach (DataRow row2 in dataTable2.Rows)
                {
                    paciente = new Paciente
                    {
                        estatus = Converter.GetInt(row2, "idPruebaRapidaKobo"),
                        NroOficio = Converter.GetString(row2, "NroOficio"),
                        CodigoOrden = Converter.GetString(row2, "Orden"),
                        FechaObtencion = Converter.GetDateTime(row2, "FechaObtencion"),
                        Enfermedad = Converter.GetString(row2, "Enfermedad"),
                        Examen = Converter.GetString(row2, "Examen"),
                        Componente = Converter.GetString(row2, "Componente"),
                        Resultado = Converter.GetString(row2, "Resultado"),
                        FechaValidacion = Converter.GetString(row2, "fechaResultado"),
                        EESSOrigen = Converter.GetString(row2, "EstablecimientoOrigen"),
                        EstablecimientoDestino = Converter.GetString(row2, "EstablecimientoDestino"),
                        Nombres = Converter.GetString(row2, "Paciente")
                    };
                    pacienteList.Add(paciente);
                }
            }

            return pacienteList;
        }
        //seguimiento de pacientes solo para enfermedad TBC - idEnfermedad 1005634
        public List<Paciente> GetDatosPacienteByNroDocumentoTbc(string nroDocumento, string nombres, string apePat, string apeMat, string idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_DatosPacienteByNroDocumentoTbc");
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_PATERNO, apePat);
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_MATERNO, apeMat);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NOMBRES, nombres);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NRO_DOCUMENTO, nroDocumento);
            InputParameterAdd.Varchar(objCommand, PacienteDal.IDUSUARIO, idUsuario);
            DataTable dataTable = Execute(objCommand);
            List<Paciente> pacienteList = new List<Paciente>();

            Paciente paciente = null;
            foreach (DataRow row in dataTable.Rows)
            {
                paciente = new Paciente
                {
                    Enfermedad = Converter.GetString(row, "Enfermedad"),
                    FechaObtencion = Converter.GetDateTime(row, "FechaObtencion"),
                    FechaValidacion = Converter.GetString(row, "FechaValidado"),
                    idExamen = Converter.GetString(row, "idExamen"),
                    Examen = Converter.GetString(row, "Examen"),
                    Componente = Converter.GetString(row, "Componente"),
                    Resultado = Converter.GetString(row, "Resultado"),
                    EESSOrigen = Converter.GetString(row, "EstablecimientoOrigen"),
                    EstablecimientoDestino = Converter.GetString(row, "EstablecimientoDestino"),
                    EstadoResultado = Converter.GetString(row, "EstatusResultado"),
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    idEstablecimiento = Converter.GetInt(row, "idLaboratorioDestino"),
                    EstatusE = Converter.GetInt(row, "EstatusE"),
                    CodigoOrden = Converter.GetString(row, "Orden"),
                    NroOficio = Converter.GetString(row, "NroOficio"),
                    CriteriosRechazo = Converter.GetString(row, "CriterioRechazo"),
                    Nombres = Converter.GetString(row, "Paciente")
                };
                pacienteList.Add(paciente);
            }

            return pacienteList;
        }
        //seguimiento de pacientes solo para enfermedad VIH - idEnfermedad 1005635
        public List<Paciente> GetDatosPacienteByNroDocumentoVIH(string nroDocumento, string nombres, string apePat, string apeMat, string idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_DatosPacienteByNroDocumentoVIH");
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_PATERNO, apePat);
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_MATERNO, apeMat);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NOMBRES, nombres);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NRO_DOCUMENTO, nroDocumento);
            InputParameterAdd.Varchar(objCommand, PacienteDal.IDUSUARIO, idUsuario);
            DataTable dataTable = Execute(objCommand);
            List<Paciente> pacienteList = new List<Paciente>();

            Paciente paciente = null;
            foreach (DataRow row in dataTable.Rows)
            {
                paciente = new Paciente
                {
                    Enfermedad = Converter.GetString(row, "Enfermedad"),
                    FechaObtencion = Converter.GetDateTime(row, "FechaObtencion"),
                    FechaValidacion = Converter.GetString(row, "FechaValidado"),
                    idExamen = Converter.GetString(row, "idExamen"),
                    Examen = Converter.GetString(row, "Examen"),
                    Componente = Converter.GetString(row, "Componente"),
                    Resultado = Converter.GetString(row, "Resultado"),
                    EESSOrigen = Converter.GetString(row, "EstablecimientoOrigen"),
                    EstablecimientoDestino = Converter.GetString(row, "EstablecimientoDestino"),
                    EstadoResultado = Converter.GetString(row, "EstatusResultado"),
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    idEstablecimiento = Converter.GetInt(row, "idLaboratorioDestino"),
                    EstatusE = Converter.GetInt(row, "EstatusE"),
                    CodigoOrden = Converter.GetString(row, "Orden"),
                    NroOficio = Converter.GetString(row, "NroOficio"),
                    CriteriosRechazo = Converter.GetString(row, "CriterioRechazo"),
                    Nombres = Converter.GetString(row, "Paciente")
                };
                pacienteList.Add(paciente);
            }

            return pacienteList;
        }

        public List<Ciudad> GetCiudadProcedencia(String textoBusqueda)
        {
            var objCommand = GetSqlCommand("pNLS_CiudadTextoBusqueda");
            InputParameterAdd.Varchar(objCommand, "texto", textoBusqueda);
            DataTable dataTable = Execute(objCommand);
            List<Ciudad> ciudadList = new List<Ciudad>();

            foreach (DataRow row in dataTable.Rows)
            {
                Ciudad ciudad = new Ciudad
                {
                    idPais = Converter.GetInt(row, "idPais"),
                    NombrePais = Converter.GetString(row, "paisnombre"),
                    idCiudad = Converter.GetInt(row, "idCiudad"),
                    NombreCiudad = Converter.GetString(row, "estadonombre")
                };
                ciudadList.Add(ciudad);
            }
            return ciudadList;
        }
        public void UpdateReniec(Paciente paciente)
        {
            var objCommand = GetSqlCommand("PNLU_PacienteReniec");
            //InputParameterAdd.Int(objCommand, "idp", paciente.id);
            InputParameterAdd.Varchar(objCommand, "apePaterno", paciente.ApellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apeMaterno", paciente.ApellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "Nombres", paciente.Nombres);

            ExecuteNonQuery(objCommand);
        }
        public List<Paciente> xLstPaciente()
        {
            var objCommand = GetSqlCommand("xPacineteKobo");
            var dataTable = Execute(objCommand);

            return (from DataRow row in dataTable.Rows
                    select new Paciente
                    {
                        //id = Converter.GetInt(row, "id"),
                        NroDocumento = Converter.GetString(row, "nroDocumento"),
                        tipoDocumen = Converter.GetString(row, "TipoDocumento")
                    }).ToList();
        }

        public List<Paciente> GetDatosPacienteByNroDocumentoCoronavirusExterno(string nroDocumento, string nombres, string apePat, string apeMat, string idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_DatosPacienteByNroDocumentoCoronavirusExterno");
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_PATERNO, apePat);
            InputParameterAdd.Varchar(objCommand, PacienteDal.APELLIDO_MATERNO, apeMat);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NOMBRES, nombres);
            InputParameterAdd.Varchar(objCommand, PacienteDal.NRO_DOCUMENTO, nroDocumento);
            InputParameterAdd.Varchar(objCommand, PacienteDal.IDUSUARIO, idUsuario);
            //DataTable dataTable = Execute(objCommand);
            DataSet PacienteCovid = ExecuteDataSet(objCommand);
            DataTable dataTable = PacienteCovid.Tables[0];
            DataTable dataTable2 = PacienteCovid.Tables[1];
            List<Paciente> pacienteList = new List<Paciente>();

            Paciente paciente = null;
            foreach (DataRow row in dataTable.Rows)
            {
                paciente = new Paciente
                {
                    Enfermedad = Converter.GetString(row, "Enfermedad"),
                    FechaObtencion = Converter.GetDateTime(row, "FechaObtencion"),
                    FechaValidacion = Converter.GetString(row, "FechaValidado"),
                    idExamen = Converter.GetString(row, "idExamen"),
                    Examen = Converter.GetString(row, "Examen"),
                    Componente = Converter.GetString(row, "Componente"),
                    Resultado = Converter.GetString(row, "Resultado"),
                    EESSOrigen = Converter.GetString(row, "EstablecimientoOrigen"),
                    EstablecimientoDestino = Converter.GetString(row, "EstablecimientoDestino"),
                    EstadoResultado = Converter.GetString(row, "EstatusResultado"),
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    idEstablecimiento = Converter.GetInt(row, "idLaboratorioDestino"),
                    EstatusE = Converter.GetInt(row, "EstatusE"),
                    CodigoOrden = Converter.GetString(row, "Orden"),
                    NroOficio = Converter.GetString(row, "NroOficio"),
                    CriteriosRechazo = Converter.GetString(row, "CriterioRechazo")
                };
                pacienteList.Add(paciente);
            }

            if (dataTable2 != null && dataTable2.Rows.Count > 0)
            {
                foreach (DataRow row2 in dataTable2.Rows)
                {
                    paciente = new Paciente
                    {
                        estatus = Converter.GetInt(row2, "idPruebaRapidaKobo"),
                        NroOficio = Converter.GetString(row2, "NroOficio"),
                        CodigoOrden = Converter.GetString(row2, "Orden"),
                        FechaObtencion = Converter.GetDateTime(row2, "FechaObtencion"),
                        Enfermedad = Converter.GetString(row2, "Enfermedad"),
                        Examen = Converter.GetString(row2, "Examen"),
                        Componente = Converter.GetString(row2, "Componente"),
                        Resultado = Converter.GetString(row2, "Resultado"),
                        FechaValidacion = Converter.GetString(row2, "fechaResultado"),
                        EESSOrigen = Converter.GetString(row2, "EstablecimientoOrigen"),
                        EstablecimientoDestino = Converter.GetString(row2, "EstablecimientoDestino")
                    };
                    pacienteList.Add(paciente);
                }
            }

            return pacienteList;
        }

        public List<Paciente> ObtenerSiscovidPruebaRapidaPorNroDocumento(string nroDocumento)
        {
            var objCommand = GetSqlCommand("pNLS_DatosSiscovidPruebaRapidaPorNroDocumento");
            InputParameterAdd.Varchar(objCommand, PacienteDal.NRO_DOCUMENTO, nroDocumento);
            //DataTable dataTable = Execute(objCommand);

            DataSet dataSet = ExecuteDataSet(objCommand);
            DataTable prKobo = dataSet.Tables[0];

            List<Paciente> pacienteList = new List<Paciente>();

            Paciente paciente = new Paciente();

            if (prKobo != null && prKobo.Rows.Count > 0)
            {
                foreach (DataRow row2 in prKobo.Rows)
                {
                    paciente = new Paciente
                    {
                        estatus = Converter.GetInt(row2, "idPruebaRapidaKobo"),
                        NroDocumento = Converter.GetString(row2, "nroDocumento"),
                        FechaValidacion = Converter.GetString(row2, "FechaResultado"),
                        Resultado = Converter.GetString(row2, "Resultado")
                    };

                    pacienteList.Add(paciente);
                }
            }

            return pacienteList;
        }

        public string BuscarPaciente(string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_ConsultaExternaPacienteLaboratorio");
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "apePaterno", apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apeMaterno", apellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);
            string paciente = "";
            paciente = objCommand.ExecuteScalar().ToString();
            return paciente;
        }

        public string ConsultaExternaPaciente(Guid idPaciente, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLS_OrdenConsultaExternaPaciente");
            InputParameterAdd.Guid(objCommand, "idPaciente", idPaciente);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            string resultado = objCommand.ExecuteScalar().ToString();
            return resultado;
        }

        public bool BuscarPacientePorNroDocumentoEnPRK(string nroDocumento)
        {
            var objCommand = GetSqlCommand("pNLS_BuscarPacientePorNroDocumentoEnPRK");
            InputParameterAdd.Varchar(objCommand, "nrodocumento", nroDocumento);
            OutputParameterAdd.Bit(objCommand, "existepaciente");
            ExecuteNonQuery(objCommand);
            bool response = false;
            var output = objCommand.Parameters["@existepaciente"].Value;
            //validar si no viene vacio
            if (output != DBNull.Value)
            {
                response = (bool)output;
            }

            return response;
        }

        public string ConsultaDatoPaciente(string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_ConsultaExternaPaciente");
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "apePaterno", apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apeMaterno", apellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            string paciente = "";
            paciente = objCommand.ExecuteScalar().ToString();
            return paciente;
        }

        public string ResultadosConsultaExternaPaciente(Guid idPaciente)
        {
            var objCommand = GetSqlCommand("pNLS_ResultadosConsultaExternaPaciente");
            InputParameterAdd.Guid(objCommand, "idPaciente", idPaciente);
            string resultado = objCommand.ExecuteScalar().ToString();
            return resultado;
        }

        public Boolean EstadoReniec()
        {
            Boolean estado = true;
            var objCommand = GetSqlCommand("pNLS_ConsultaEstadoReniec");
            var reniec = objCommand.ExecuteScalar();
            estado = (Boolean)reniec;
            return estado;
        }

        public List<Paciente> ConsultaPacienteSolicitudVIH(int? tipoDocumento, string nroDocumento, string apellidoPaterno, string apellidoMaterno, string nombres)
        {
            var ListaPaciente = new List<Paciente>();
            var cnn = "";
            foreach (ConnectionStringSettings c in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (c.Name.StartsWith("DefaultConnection"))
                {
                    cnn = c.Name;
                }

            }
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[cnn].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("pNLS_ConsultaPacienteSolicitudVIH", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    //comando.BeginExecuteNonQuery();
                    comando.Parameters.Add(new SqlParameter("@tipoDocumento", System.Data.SqlDbType.Int)).Value = tipoDocumento;
                    comando.Parameters.Add(new SqlParameter("@nroDocumento", System.Data.SqlDbType.VarChar)).Value = nroDocumento;
                    comando.Parameters.Add(new SqlParameter("@apellidoPaterno", System.Data.SqlDbType.VarChar)).Value = apellidoPaterno;
                    comando.Parameters.Add(new SqlParameter("@apellidoMaterno", System.Data.SqlDbType.VarChar)).Value = apellidoMaterno;
                    comando.Parameters.Add(new SqlParameter("@nombres", System.Data.SqlDbType.VarChar)).Value = nombres;
                    comando.CommandTimeout = 0;
                    try
                    {
                        var reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Paciente();
                                obj.IdPaciente = (Guid)reader["idPaciente"];
                                obj.ApellidoPaterno = reader["apellidoPaterno"].ToString();
                                obj.ApellidoMaterno = reader["apellidoMaterno"].ToString();
                                obj.Nombres = reader["nombres"].ToString();
                                //obj.generoNombre = reader["genero"].ToString();
                                //obj.FechaNacimiento = (DateTime)reader["fechaNacimiento"];
                                //obj.edadAnios = (int)reader["Edad"];
                                obj.tipoDocumentoNombre = reader["tipoDocumento"].ToString();
                                obj.NroDocumento = reader["nroDocumento"].ToString();
                                //obj.Celular1 = reader["celular1"].ToString();
                                //obj.UbigeoActual = new Ubigeo();
                                //obj.UbigeoActual.Departamento = reader["departamentoActual"].ToString();
                                //obj.UbigeoActual.Provincia = reader["provinciaActual"].ToString();
                                //obj.UbigeoActual.Distrito = reader["distritoActual"].ToString();
                                ListaPaciente.Add(obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        conexion.Close();
                        conexion.Dispose();
                        throw ex;
                    }
                    finally
                    {
                        conexion.Close();
                        conexion.Dispose();
                    }
                }
            }
            return ListaPaciente;
        }

        public string ConsultaHistorialPacienteVIH(Guid idPaciente)
        {
            var objCommand = GetSqlCommand("pNLS_ConsultaHistorialResultadosPacienteVIH");
            InputParameterAdd.Guid(objCommand, "idPaciente", idPaciente);
            string resultado = objCommand.ExecuteScalar().ToString();
            return resultado;
        }

        public string GetDatoSolicitudExamenVIH(Guid idPaciente, Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_GetDatoSolicitudExamenVIH");
            InputParameterAdd.Guid(objCommand, "idPaciente", idPaciente);
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            string resultado = objCommand.ExecuteScalar().ToString();
            return resultado;
        }

        public string ValidaResultadoVih(Guid idPaciente)
        {
            var objCommand = GetSqlCommand("pNLS_ValidarDatoResultadoVih");
            InputParameterAdd.Guid(objCommand, "idPaciente", idPaciente);
            string valor = objCommand.ExecuteScalar().ToString();
            return valor;
        }
    }
}


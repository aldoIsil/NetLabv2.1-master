using Enums;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Microsoft.SqlServer.Server;
using Model.Entidades;
using Model.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class CoreDal : DaoBase
    {
        public CoreDal(IDalSettings settings) : base(settings)
        {
        }

        public CoreDal() : this(new NetlabSettings())
        {
        }

        public void CrearOrdenInicial(Guid ordenId, Guid pacienteId, int idUsuario, TipoRegistroOrden tipoRegistro)
        {
            var objCommand = GetSqlCommand("pNLI_CrearNuevaOrdenInicialCore");
            InputParameterAdd.Guid(objCommand, "ordenId", ordenId);
            InputParameterAdd.Guid(objCommand, "pacienteId", pacienteId);
            InputParameterAdd.Int(objCommand, "usuarioId", idUsuario);
            InputParameterAdd.Bit(objCommand, "tipoRegistro", tipoRegistro == TipoRegistroOrden.ORDEN_RECEPCION);
            objCommand.ExecuteNonQuery();
        }

        public void CrearOrdenExamenCore(CrearOrdenExamenVM model)
        {
            var objCommand = GetSqlCommand("pNLI_CrearOrdenExamenCore");
            InputParameterAdd.Guid(objCommand, "ordenexamenId", model.OrdenExamenId);
            InputParameterAdd.Guid(objCommand, "ordenId", model.OrdenId);
            InputParameterAdd.Int(objCommand, "enfermedadId", model.EnfermedadId);
            InputParameterAdd.Guid(objCommand, "examenId", model.ExamenId);
            InputParameterAdd.Int(objCommand, "tipoMuestraId", model.TipoMuestraId);
            //estatus se esta asignando -1 por defecto para todas las tablas dentro de este SP. Actualizarlo con el boton Guardar de la orden
            InputParameterAdd.Int(objCommand, "usuarioId", model.UsuarioId);
            InputParameterAdd.DateTime(objCommand, "fechaColeccion", model.FechaObtencionDT);
            InputParameterAdd.DateTime(objCommand, "horaColeccion", model.HoraObtencionDT);
            InputParameterAdd.Int(objCommand, "establecimientoId", model.EstablecimientoDestinoId);
            InputParameterAdd.Bit(objCommand, "tipoRegistro", model.TipoRegistro == TipoRegistroOrden.ORDEN_RECEPCION);
            objCommand.ExecuteNonQuery();
        }

        public Orden ObtenerOrdenPorId(Guid idorden)
        {
            Orden resultado = new Orden();
            var objCommand = GetSqlCommand("pNLS_ObtenerOrdenCorePorId");
            InputParameterAdd.Guid(objCommand, "ordenId", idorden);
            DataTable dataTable = Execute(objCommand);
            foreach (DataRow row in dataTable.Rows)
            {
                resultado = new Orden()
                {
                    IdOrden = idorden,
                    CodigoOrden= Converter.GetString(row, "CodigoOrden"),
                    TipoRegistro = Converter.GetBool(row, "TipoRegistro") ? TipoRegistroOrden.ORDEN_RECEPCION : TipoRegistroOrden.SOLO_ORDEN,
                    Estatus = Converter.GetInt(row, "estatus"),
                    Estado = Converter.GetInt(row, "Estado"),
                    FechaRegistro = Converter.GetDateTime(row, "FechaRegistro"),
                    IdPaciente = Converter.GetGuid(row, "IdPaciente"),
                };
            }

            return resultado;
        }

        public Paciente ObtenerPacientePorId(Guid idpaciente)
        {
            Paciente resultado = new Paciente();
            var objCommand = GetSqlCommand("pNLS_ObtenerPacientePorId");
            InputParameterAdd.Guid(objCommand, "pacienteId", idpaciente);
            DataTable dataTable = Execute(objCommand);
            foreach (DataRow row in dataTable.Rows)
            {
                resultado = new Paciente()
                {
                    Celular1 = Converter.GetString(row, "celular1"),
                    Celular2 = Converter.GetString(row, "celular2"),
                    Departamento = Converter.GetString(row, "departamento"),
                    Direccion = Converter.GetString(row, "direccion"),
                    Distrito = Converter.GetString(row, "distrito"),
                    Edad = Converter.GetInt(row, "edad"),
                    Email = Converter.GetString(row, "email"),
                    Estatus = Converter.GetInt(row, "estatus"),
                    FechaRegistro = Converter.GetDateTime(row, "fecharegistro"),
                    Genero = Converter.GetString(row, "genero"),
                    IdPaciente = Converter.GetGuid(row, "idpaciente"),
                    NombreCompleto = Converter.GetString(row, "nombrecompleto"),
                    NroDocumento = Converter.GetString(row, "nrodocumento"),
                    Ocupacion = Converter.GetString(row, "ocupacion"),
                    Provincia = Converter.GetString(row, "provincia"),
                    TelefonoFijo = Converter.GetString(row, "telefonofijo"),
                    TipoSeguroSalud = Converter.GetString(row, "tiposegurosalud"),
                };
            }

            return resultado;
        }

        public List<Examen> GetExamenesPorEnfermedad(int idEnfermedad)
        {
            var resultado = new List<Examen>();
            var objCommand = GetSqlCommand("pNLS_ExamenesPorEnfermedad");
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            var dataTable = Execute(objCommand);
            foreach (DataRow row in dataTable.Rows)
            {
                resultado.Add(new Examen
                {
                    IdExamen = Converter.GetGuid(row, "idExamen"),
                    Nombre = Converter.GetString(row, "nombre"),
                    IPruebarapida = Converter.GetInt(row, "ipruebarapida")
                });
            }

            return resultado;
        }

        public List<TipoMuestra> ObtenerTipoMuestraPorExamen(Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_TipoMuestraByIdExamen");
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            var dataTable = Execute(objCommand);
            var resultado = new List<TipoMuestra>();

            foreach (DataRow row in dataTable.Rows)
            {
                resultado.Add(new TipoMuestra
                {
                    idTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                    nombre = Converter.GetString(row, "nombre")
                });
            }

            return resultado;
        }

        public List<CrearOrdenExamenTabla> ObtenerOrdenExamenesActivos(Guid idOrden)
        {
            var resultado = new List<CrearOrdenExamenTabla>();
            var objCommand = GetSqlCommand("pNLS_ObtenerOrdenExamenLista");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            var dataTable = Execute(objCommand);
            foreach (DataRow row in dataTable.Rows)
            {
                resultado.Add(new CrearOrdenExamenTabla
                {
                    OrdenId = Converter.GetGuid(row, "idOrden"),
                    OrdenExamenId = Converter.GetGuid(row, "idOrdenExamen"),
                    OrdenMuestraId = Converter.GetGuid(row, "idOrdenMuestra"),
                    Enfermedad = Converter.GetString(row, "Enfermedad"),
                    Examen = Converter.GetString(row, "Examen"),
                    TipoMuestra = Converter.GetString(row, "TipoMuestra"),
                    FechaObtencionDB = Converter.GetDateTime(row, "FechaObtencionDB"),
                    HoraObtencionDB = Converter.GetDateTime(row, "HoraObtencionDB"),
                    Conforme = Converter.GetInt(row, "conforme") == 1,
                    EstablecimientoDestinoId = Converter.GetInt(row, "EstablecimientoDestinoId"),
                    EstablecimientoDestinoNombre = Converter.GetString(row, "EstablecimientoNombre"),
                    IdTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                    IdCriterioRechazo = Converter.GetInt(row, "idCriterioRechazo"),
                    CriterioRechazoGlosa = Converter.GetString(row, "Glosa"),
                    IdOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion"),
                    TienePR = Converter.GetInt(row, "iPruebaRapida") == 1
                });
            }

            return resultado;
        }

        public List<EnfermedadDatos> ObtenerDatosClinicosPorEnfermedad(Guid idOrden)
        {
            var resultado = new List<EnfermedadDatos>();
            var objCommand = GetSqlCommand("pNLS_DatosPorEnfermedadExamen");
            //InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            //InputParameterAdd.Varchar(objCommand, "idExamen", idExamen);
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            var dataTable = Execute(objCommand);
            foreach (DataRow row in dataTable.Rows)
            {
                resultado.Add(new EnfermedadDatos
                {
                    IdOrden = idOrden,
                    IdEnfermedad = Converter.GetInt(row, "idEnfermedad"),
                    EnfermedadNombre = Converter.GetString(row, "NombreEnfermedad"),
                    IdCategoriaDatoPadre = Converter.GetInt(row, "idCategoriaDatoPadre"),
                    IdCategoriaDato = Converter.GetInt(row, "idCategoriaDato"),
                    CategoriaNombre = Converter.GetString(row, "Nombre"),
                    Orientacion = Converter.GetInt(row, "Orientacion"),
                    Visible = Converter.GetInt(row, "Visible"),
                    IdDato = Converter.GetInt(row, "idDato"),
                    DatoNombre = Converter.GetString(row, "prefijo"),
                    IdsExamen = Converter.GetString(row, "idsExamen"),
                    IdTipoDato = (TipoCampo)Converter.GetInt(row, "idTipo"),
                    Obligatorio = Converter.GetBool(row, "obligatorio"),
                    idListaDato = Converter.GetInt(row, "idListaDato"),
                    idOpcionDato = Converter.GetInt(row, "idOpcionDato"),
                    OpcionDatoValor = Converter.GetString(row, "valor"),
                    OpcionDatoOrden = Converter.GetInt(row, "OpcionDatoOrden")
                });
            }

            return resultado;
        }

        public void EliminarOrdenExamen(Guid ordenExamenId)
        {
            var objCommand = GetSqlCommand("pNLD_EliminarOrdenExamenCore");
            InputParameterAdd.Guid(objCommand, "ordenexamenId", ordenExamenId);
            objCommand.ExecuteNonQuery();
        }

        public void CrearOrdenDatosClinicos(List<OrdenDatoClinicoCore> datosclinicos, List<CrearOrdenExamenTabla> ordenexamenes)
        {
            var objCommand = GetSqlCommand("pNLI_CrearOrdenDatoClinicoActualizarOrdenExamenesCore");

            //objCommand.Parameters.AddWithValue("@datosclinicos", CreateDataTableDatosClinicos(datosclinicos));
            //objCommand.Parameters.AddWithValue("@ordenexamenes", CreateDataTableOrdenExamen(ordenexamenes));
            SqlParameter datosclinicosTable = new SqlParameter();
            datosclinicosTable.ParameterName = "@datosclinicos";
            datosclinicosTable.TypeName = "dbo.OrdenDatoClinicoTableType";
            datosclinicosTable.SqlDbType = SqlDbType.Structured;
            datosclinicosTable.Value = new OrdenDatoClinicoDataRecord(datosclinicos);
            objCommand.Parameters.Add(datosclinicosTable);

            SqlParameter ordenexamenesTable = new SqlParameter();
            ordenexamenesTable.ParameterName = "@ordenexamenes";
            ordenexamenesTable.TypeName = "dbo.OrdenExamenTableType";
            ordenexamenesTable.SqlDbType = SqlDbType.Structured;
            ordenexamenesTable.Value = new OrdenExamenDataRecord(ordenexamenes);
            objCommand.Parameters.Add(ordenexamenesTable);
            objCommand.ExecuteNonQuery();
        }

        public string FinalizarCreacionOrden(CrearOrdenVM orden)
        {
            var objCommand = GetSqlCommand("pNLU_FinalizarOrdenCore");
            InputParameterAdd.Guid(objCommand, "idOrden", orden.Orden.IdOrden);
            InputParameterAdd.Int(objCommand, "idProyecto", orden.ProyectoId);
            InputParameterAdd.DateTime(objCommand, "fechaIngresoROM", orden.FechaIngresoROM);
            InputParameterAdd.Int(objCommand, "idSolicitante", orden.SolicitanteId);
            InputParameterAdd.DateTime(objCommand, "fechaSolicitud", orden.FechaSolicitud);
            //estatus se esta asignando -1 por defecto para todas las tablas dentro de este SP. Actualizarlo con el boton Guardar de la orden
            InputParameterAdd.DateTime(objCommand, "fechaEvaluacionFicha", orden.FechaEvaluacionFicha);
            InputParameterAdd.Varchar(objCommand, "nroOficio", orden.NroOficio);
            InputParameterAdd.Varchar(objCommand, "observaciones", orden.Observacion);
            InputParameterAdd.Int(objCommand, "idEstablecimientoOrigen", orden.EstablecimientoOrigenId);
            InputParameterAdd.Int(objCommand, "idEstablecimientoEnvio", orden.EstablecimientoEnvioId);
            OutputParameterAdd.Varchar(objCommand, "mensajeresultado", 200);
            objCommand.ExecuteNonQuery();
            //ExecuteNonQuery(objCommand);
            return (string)objCommand.Parameters["@mensajeresultado"].Value;

        }

        public void DesactivarReniec()
        {
            var objCommand = GetSqlCommand("pNLU_DesactivarReniec");
            objCommand.ExecuteNonQuery();
        }

        public void ActivarReniec()
        {
            var objCommand = GetSqlCommand("pNLU_ActivarReniec");
            objCommand.ExecuteNonQuery();
        }

        public void CrearOrdenDatosClinicosTest(int i)
        {
            List<OrdenDatoClinicoCore> datosclinicos = new List<OrdenDatoClinicoCore>();
            //for (int i = 0; i < 100; i++)
            //{
                for (int j = 0; j < 25; j++)
                {
                    datosclinicos.Add(new OrdenDatoClinicoCore
                    {
                        IdCategoriaDato = j,
                        IdDato = j,
                        IdEnfermedad = j,
                        IdOrden = Guid.NewGuid(),
                        IdUsuarioRegistro = i,
                        NoPrecisa = true,
                        Valor = string.Format("{0:D3}-{1:D3}", i,j)
                    });
                }
            //}

            var objCommand = GetSqlCommand("pNLI_CrearOrdenDatoClinicoTest");

            //objCommand.Parameters.AddWithValue("@datosclinicos", CreateDataTableDatosClinicos(datosclinicos));
            //objCommand.Parameters.AddWithValue("@ordenexamenes", CreateDataTableOrdenExamen(ordenexamenes));
            SqlParameter datosclinicosTable = new SqlParameter();
            datosclinicosTable.ParameterName = "@datosclinicos";
            datosclinicosTable.TypeName = "dbo.OrdenDatoClinicoTableType";
            datosclinicosTable.SqlDbType = SqlDbType.Structured;
            datosclinicosTable.Value = new OrdenDatoClinicoDataRecord(datosclinicos);
            objCommand.Parameters.Add(datosclinicosTable);

            objCommand.ExecuteNonQuery();
        }
    }

    public class OrdenDatoClinicoDataRecord : IEnumerable<SqlDataRecord>
    {
        List<OrdenDatoClinicoCore> lista;
        public OrdenDatoClinicoDataRecord(List<OrdenDatoClinicoCore> _lista)
        {
            lista = _lista;
        }
        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            SqlMetaData[] columnStructure = new SqlMetaData[7];
            columnStructure[0] = new SqlMetaData("idOrden", SqlDbType.UniqueIdentifier);
            columnStructure[1] = new SqlMetaData("idEnfermedad", SqlDbType.Int);
            columnStructure[2] = new SqlMetaData("idCategoriaDato", SqlDbType.Int);
            columnStructure[3] = new SqlMetaData("idDato", SqlDbType.Int);
            columnStructure[4] = new SqlMetaData("noPrecisa", SqlDbType.Bit);
            columnStructure[5] = new SqlMetaData("valor", SqlDbType.VarChar, 1000);
            columnStructure[6] = new SqlMetaData("idUsuarioRegistro", SqlDbType.Int);
            try
            {
                List<SqlDataRecord> result = new List<SqlDataRecord>();
                //lista.ForEach(x =>
                //{
                //    SqlDataRecord dataRecord = new SqlDataRecord(columnStructure);
                //    dataRecord.SetGuid(0, x.IdOrden);
                //    dataRecord.SetInt32(1, x.IdEnfermedad);
                //    result.Add(dataRecord);
                //});

                foreach (var x in lista)
                {
                    SqlDataRecord dataRecord = new SqlDataRecord(columnStructure);
                    dataRecord.SetGuid(0, x.IdOrden);
                    dataRecord.SetInt32(1, x.IdEnfermedad);
                    dataRecord.SetInt32(2, x.IdCategoriaDato);
                    dataRecord.SetInt32(3, x.IdDato);
                    dataRecord.SetBoolean(4, x.NoPrecisa);
                    dataRecord.SetString(5, x.Valor);
                    dataRecord.SetInt32(6, x.IdUsuarioRegistro);
                    yield return dataRecord; //result.Add(dataRecord);
                }

                //return result;
            }
            // no catch block allowed due to the "yield" command
            finally
            {
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class OrdenExamenDataRecord : IEnumerable<SqlDataRecord>
    {
        List<CrearOrdenExamenTabla> lista;
        public OrdenExamenDataRecord(List<CrearOrdenExamenTabla> _lista)
        {
            lista = _lista;
        }
        public IEnumerator<SqlDataRecord> GetEnumerator()
        {
            SqlMetaData[] columnStructure = new SqlMetaData[9];
            columnStructure[0] = new SqlMetaData("idOrden", SqlDbType.UniqueIdentifier);
            columnStructure[1] = new SqlMetaData("idOrdenExamen", SqlDbType.UniqueIdentifier);
            columnStructure[2] = new SqlMetaData("idOrdenMuestra", SqlDbType.UniqueIdentifier);
            columnStructure[3] = new SqlMetaData("codigoMuestra", SqlDbType.VarChar, 20);
            columnStructure[4] = new SqlMetaData("conforme", SqlDbType.Bit);
            columnStructure[5] = new SqlMetaData("laboratorioRecepcion", SqlDbType.Int);
            columnStructure[6] = new SqlMetaData("fechaColeccion", SqlDbType.DateTime);
            columnStructure[7] = new SqlMetaData("horaColeccion", SqlDbType.DateTime);
            columnStructure[8] = new SqlMetaData("idProyecto", SqlDbType.Int);
            try
            {
                List<SqlDataRecord> result = new List<SqlDataRecord>();

                foreach (var x in lista)
                {
                    SqlDataRecord dataRecord = new SqlDataRecord(columnStructure);
                    dataRecord.SetGuid(0, x.OrdenId);
                    dataRecord.SetGuid(1, x.OrdenExamenId);
                    dataRecord.SetGuid(2, x.OrdenMuestraId);
                    dataRecord.SetString(3, x.CodigoMuestra);
                    dataRecord.SetBoolean(4, x.Conforme);
                    dataRecord.SetInt32(5, x.EstablecimientoDestinoId);
                    dataRecord.SetDateTime(6, x.FechaObtencionDT);
                    dataRecord.SetDateTime(7, x.HoraObtencionDT);
                    dataRecord.SetInt32(8, x.ProyectoId);
                    yield return dataRecord;
                }

                //return result;
            }
            // no catch block allowed due to the "yield" command
            finally
            {
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
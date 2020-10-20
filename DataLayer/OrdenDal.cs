using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Collections.Generic;
using Model;
using System.Data;
using Model.ViewData;
using System.Linq;
using DataLayer.DalConverter;
using System.Configuration;
using System.Data.SqlClient;
using static Model.ResultadoWebService;
using System.Net;
using System.IO;
using Utilitario;

namespace DataLayer
{
    public class OrdenDal : DaoBase
    {
        public static int ESTADO_BORRADOR = 0;
        public static int ESTADO_PERMANENTE = 1;


        public OrdenDal(IDalSettings settings) : base(settings)
        {
        }

        public OrdenDal() : this(new NetlabSettings())
        {
        }

        /// <summary>
        /// Descripción: Metodo que Inserta orden paciente
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        /// <returns></returns>
        public Orden InsertOrden(Orden orden)
        {
            var fechaSolicitud = orden.fechaSolicitud ?? orden.fechaSolicitud;
            DateTime aDate = DateTime.Now;
            var objCommand = GetSqlCommand("pNLI_OrdenPaciente");
            InputParameterAdd.Varchar(objCommand, "observacion", orden.observacion);
            InputParameterAdd.Varchar(objCommand, "nroOficio", string.IsNullOrEmpty(orden.nroOficio) ? "" : orden.nroOficio);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", orden.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", orden.idEstablecimiento);
            InputParameterAdd.Int(objCommand, "idEstablecimientoEnvio", orden.idEstablecimientoEnvio);//no esta en SP
            InputParameterAdd.Int(objCommand, "idProyecto", orden.Proyecto.IdProyecto);
            InputParameterAdd.Guid(objCommand, "idPaciente", orden.Paciente.IdPaciente);
            InputParameterAdd.Varchar(objCommand, "solicitante", orden.solicitante);
            InputParameterAdd.DateTime(objCommand, "fechaSolicitud", fechaSolicitud);
            InputParameterAdd.DateTime(objCommand, "fechaRecepcionRomINS", orden.fechaIngresoINS);
            InputParameterAdd.DateTime(objCommand, "fechaReevaluacionFicha", orden.fechaReevaluacionFicha);
            InputParameterAdd.Int(objCommand, "estatus", orden.estatus);
            InputParameterAdd.Varchar(objCommand, "codigoOrden", orden.codigoOrden);
            OutputParameterAdd.Varchar(objCommand, "newCodigoOrden", 11);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrden");
            OutputParameterAdd.Varchar(objCommand, "nombreEstablecimiento", 255);
            OutputParameterAdd.Varchar(objCommand, "nombreProyecto", 500);
            OutputParameterAdd.DateTime(objCommand, "fechaRegistro");
            ExecuteNonQuery(objCommand);
            orden.idOrden = (Guid)objCommand.Parameters["@newIdOrden"].Value;
            //orden.codigoOrden = (string)objCommand.Parameters["@newCodigoOrden"].Value;
            orden.establecimiento = new Establecimiento();
            orden.establecimiento.IdEstablecimiento = orden.idEstablecimiento;
            orden.establecimiento.Nombre = (string)objCommand.Parameters["@nombreEstablecimiento"].Value;
            orden.Proyecto.Nombre = (string)objCommand.Parameters["@nombreProyecto"].Value;
            orden.FechaRegistro = (DateTime)objCommand.Parameters["@fechaRegistro"].Value;



            return orden;
        }
        /// <summary>
        /// Descripción: Metodo para registrar Nuevo examene VERIFICADOR.
        /// enlazado con el metodo InsertarOrden
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 11/11/2017
        /// Fecha Modificación: 11/11/2017.
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="idUsuario"></param>
        /// /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        public void InsertOrdenVerificador(string datos, int idUsuario, int idEstablecimiento)
        {
            var objCommand2 = GetSqlCommand("pNLI_InsertarNuevoExamenVerificador");
            InputParameterAdd.Varchar(objCommand2, "Lista", datos);
            InputParameterAdd.Int(objCommand2, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand2, "idEstablecimiento", idEstablecimiento);
            ExecuteNonQuery(objCommand2);

        }
        /// <summary>
        /// Descripción: Metodo para registrar Nuevo examene ANALISTA.
        /// enlazado con el metodo InsertarOrden
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 11/11/2017
        /// Fecha Modificación: 11/11/2017.
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="idUsuario"></param>
        /// /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        public void InsertOrdenAnalista(string datos, int idUsuario, int idEstablecimiento)
        {
            var objCommand2 = GetSqlCommand("pNLI_InsertarNuevoExamenAnalista");
            InputParameterAdd.Varchar(objCommand2, "Lista", datos);
            InputParameterAdd.Int(objCommand2, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand2, "idEstablecimiento", idEstablecimiento);
            ExecuteNonQuery(objCommand2);

        }
        public void InsertOrdenRomINS(string datos, int idUsuario, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarNuevoExamenRom");
            InputParameterAdd.Varchar(objCommand, "Lista", datos);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que Actualiza Orden Paciente
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        public void UpdateOrden(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenPaciente");
            InputParameterAdd.Varchar(objCommand, "observacion", orden.observacion);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", orden.IdUsuarioEdicion);
            InputParameterAdd.Guid(objCommand, "idOrden", orden.idOrden);
            InputParameterAdd.Guid(objCommand, "idPaciente", orden.idPaciente);
            InputParameterAdd.Int(objCommand, "estatus", orden.estatus);
            InputParameterAdd.Varchar(objCommand, "nroOficio", orden.nroOficio);
            InputParameterAdd.Varchar(objCommand, "solicitante", orden.solicitante);
            InputParameterAdd.DateTime(objCommand, "fechaRecepcionRomINS", orden.fechaIngresoINS);
            InputParameterAdd.DateTime(objCommand, "fechaReevaluacionFicha", orden.fechaReevaluacionFicha);
            InputParameterAdd.DateTime(objCommand, "fechaSolicitud", orden.fechaSolicitud);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que Actualiza Orden Muestra, fecha y hora de obtencion de muestra
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/01/2020
        /// </summary>
        /// <param name="ordenMuestra"></param>
        public void UpdateOrdenMuestra(OrdenMuestra ordenMuestra)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenMuestraRechazo");
            InputParameterAdd.Int(objCommand, "idUsuario", ordenMuestra.IdUsuarioEdicion);
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenMuestra.idOrdenMuestra);
            InputParameterAdd.Varchar(objCommand, "fechaColeccion", ordenMuestra.fechaColeccionToString);
            InputParameterAdd.Varchar(objCommand, "horaColeccion", ordenMuestra.horaColeccionToString);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que Actualiza Orden Material, establecimiento destino
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/01/2020
        /// </summary>
        /// <param name="ordenMaterial"></param>
        public void UpdateOrdenMaterial(OrdenMaterial ordenMaterial)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenMaterial");
            InputParameterAdd.Int(objCommand, "idUsuario", ordenMaterial.IdUsuarioEdicion);
            InputParameterAdd.Guid(objCommand, "idOrdenMaterial", ordenMaterial.idOrdenMaterial);
            InputParameterAdd.Int(objCommand, "idLaboratorio", ordenMaterial.Laboratorio.IdLaboratorio);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que Elimina Orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        public void DeleteOrden(Guid idOrden)
        {
            var objCommand = GetSqlCommand("pNLD_Orden");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que obtiene toda la informacion de la orden filtrado rango de fechas, nro documento,nro oficio, estado Orden, fecha solicitud y establecimiento
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="docIdentidad"></param>
        /// <param name="nroOficio"></param>
        /// <param name="estadoOrden"></param>
        /// <param name="fechaSolicitud"></param>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        public List<Orden> GetOrdenes(DateTime fechaDesde, DateTime fechaHasta, string docIdentidad, string nroOficio, int estadoOrden, int fechaSolicitud, int idEstablecimiento, int idusuariologueado)
        {
            List<Orden> ordenList = new List<Orden>();

            var objCommand = GetSqlCommand("pNLS_Ordenes");
            InputParameterAdd.DateTime(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", docIdentidad);
            InputParameterAdd.Varchar(objCommand, "nroOficio", nroOficio);
            InputParameterAdd.Int(objCommand, "estadoOrden", estadoOrden);
            InputParameterAdd.Int(objCommand, "fechaSolicitud", fechaSolicitud);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "IdUsuarioLogueado", idusuariologueado);
            DataTable dataTable = Execute(objCommand);

            foreach (DataRow row in dataTable.Rows)
            {
                Orden orden = new Orden
                {
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    codigoOrden = Converter.GetString(row, "codigoOrden"),
                    nroOficio = Converter.GetString(row, "nroOficio"),
                    Paciente = new Paciente
                    {
                        NroDocumento = Converter.GetString(row, "nroDocumento"),
                        Nombres = Converter.GetString(row, "nombrePaciente"),
                        FechaNacimiento = Converter.GetDateTime(row, "fechaNacimiento"),
                        tipoDocumentoNombre = Converter.GetString(row, "tipoDocumentoNombre"),
                        generoNombre = Converter.GetString(row, "nombreGenero")
                    },
                    establecimiento = new Establecimiento
                    {
                        Nombre = Converter.GetString(row, "nombreEstablecimiento")
                    },
                    FechaRegistro = Converter.GetDateTime(row, "fechaRegistro"),
                    estatus = Converter.GetInt(row, "estatus"),
                    fechaSolicitud = Converter.GetDateTime(row, "fechaSolicitud"),
                };
                ordenList.Add(orden);
            }

            return ordenList;
        }

        /// <summary>
        /// Descripción: Metodo que registra los datos de orden Orden
        ///              y obtiene el nuevo Codigo de muestra,Codigo de la Orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenMuestra"></param>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        public OrdenMuestra InsertOrdenMuestra(OrdenMuestra ordenMuestra, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMuestra");
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenMuestra.idOrdenMuestra);
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMuestra.idOrden);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", ordenMuestra.TipoMuestra.idTipoMuestra);
            InputParameterAdd.Int(objCommand, "idProyecto", ordenMuestra.idProyecto);
            InputParameterAdd.DateTime(objCommand, "fechaColeccion", ordenMuestra.fechaColeccion);
            InputParameterAdd.Varchar(objCommand, "horaColeccion", ordenMuestra.horaColeccion.ToString("HH':'mm"));
            InputParameterAdd.Int(objCommand, "numero", 1);
            InputParameterAdd.Int(objCommand, "seriado", 0);
            InputParameterAdd.Varchar(objCommand, "codificacion", ordenMuestra.MuestraCodificacion.codificacion);
            InputParameterAdd.Int(objCommand, "estado", 1);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMuestra.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "estatus", ordenMuestra.estatus);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdMuestraCod");
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenMuestra");
            OutputParameterAdd.Varchar(objCommand, "codificacionGenerada", 50);
            ExecuteNonQuery(objCommand);
            ordenMuestra.idOrdenMuestra = (Guid)objCommand.Parameters["@newIdOrdenMuestra"].Value;

            if (ordenMuestra.estatus > OrdenDal.ESTADO_BORRADOR)
            {
                ordenMuestra.MuestraCodificacion = new MuestraCodificacion();
                //Juan Muga - inicio
                if (objCommand.Parameters["@newIdMuestraCod"].Value != DBNull.Value)
                {
                    ordenMuestra.idMuestraCod = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                }
                //ordenMuestra.idMuestraCod  = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                if (objCommand.Parameters["@codificacionGenerada"].Value != DBNull.Value)
                {
                    ordenMuestra.MuestraCodificacion.codificacion = (String)objCommand.Parameters["@codificacionGenerada"].Value;
                }
                ////Juan Muga - fin
                //ordenMuestra.idMuestraCod = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                //ordenMuestra.MuestraCodificacion.codificacion = (String)objCommand.Parameters["@codificacionGenerada"].Value;
            }

            return ordenMuestra;
        }
        public OrdenMuestra InsertOrdenMuestraKobo(OrdenMuestra ordenMuestra, int idEstablecimiento)
        {
            try {
            var objCommand = GetSqlCommand("pNLI_OrdenMuestra_Kobo");
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenMuestra.idOrdenMuestra);
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMuestra.idOrden);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", ordenMuestra.TipoMuestra.idTipoMuestra);
            InputParameterAdd.Int(objCommand, "idProyecto", ordenMuestra.idProyecto);
            InputParameterAdd.DateTime(objCommand, "fechaColeccion", ordenMuestra.fechaColeccion);
            InputParameterAdd.Varchar(objCommand, "horaColeccion", ordenMuestra.horaColeccion.ToString("HH':'mm"));
            InputParameterAdd.Int(objCommand, "numero", 1);
            InputParameterAdd.Int(objCommand, "seriado", 0);
            InputParameterAdd.Varchar(objCommand, "codificacion", ordenMuestra.MuestraCodificacion.codificacion);
            InputParameterAdd.Int(objCommand, "estado", 1);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMuestra.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "estatus", ordenMuestra.estatus);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdMuestraCod");
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenMuestra");
            OutputParameterAdd.Varchar(objCommand, "codificacionGenerada", 50);
            OutputParameterAdd.Varchar(objCommand, "codificacionLinealGenerada", 50);
            ExecuteNonQuery(objCommand);
            ordenMuestra.idOrdenMuestra = (Guid)objCommand.Parameters["@newIdOrdenMuestra"].Value;
            ordenMuestra.MuestraCodificacion.codificacion = (String)objCommand.Parameters["@codificacionGenerada"].Value;
            ordenMuestra.MuestraCodificacion.codificacionLineal = (String)objCommand.Parameters["@codificacionLinealGenerada"].Value;
            if (ordenMuestra.estatus > OrdenDal.ESTADO_BORRADOR)
            {
                ordenMuestra.MuestraCodificacion = new MuestraCodificacion();
                //Juan Muga - inicio
                if (objCommand.Parameters["@newIdMuestraCod"].Value != DBNull.Value)
                {
                    ordenMuestra.idMuestraCod = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                }
                //ordenMuestra.idMuestraCod  = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                if (objCommand.Parameters["@codificacionGenerada"].Value != DBNull.Value)
                {
                    ordenMuestra.MuestraCodificacion.codificacion = (String)objCommand.Parameters["@codificacionGenerada"].Value;
                }
                //ordenMuestra.idMuestraCod  = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                if (objCommand.Parameters["@codificacionLinealGenerada"].Value != DBNull.Value)
                {
                    ordenMuestra.MuestraCodificacion.codificacionLineal = (String)objCommand.Parameters["@codificacionLinealGenerada"].Value;
                }
                ////Juan Muga - fin
                //ordenMuestra.idMuestraCod = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                //ordenMuestra.MuestraCodificacion.codificacion = (String)objCommand.Parameters["@codificacionGenerada"].Value;
                //ordenMuestra.MuestraCodificacion.codificacionLineal = (String)objCommand.Parameters["@codificacionLinealGenerada"].Value;
            }

            return ordenMuestra;
            }
            catch (Exception ex)
            {
                string.Format("idOrdenMuestra: {0} - idOrden:{1} - idTipoMuestra is null: {2} - idUsuarioregistro: {3} - ", ordenMuestra.idOrdenMuestra, ordenMuestra.idOrden, ordenMuestra.TipoMuestra == null, ordenMuestra.IdUsuarioRegistro);
                new bsPage().LogError(ex, "LogNetLab", string.Empty, "OrdenDal.InsertOrdenMuestraKobo - ");
                throw ex;
            }
        }
        /// <summary>
        /// Descripción: Metodo que registra ordenes con tipo de muestra Banco de Sangre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenMuestra"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public OrdenMuestra InsertOrdenMuestraBS(OrdenMuestra ordenMuestra, int tipo)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMuestraBS");
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMuestra.idOrden);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", ordenMuestra.TipoMuestra.idTipoMuestra);
            InputParameterAdd.Int(objCommand, "idProyecto", ordenMuestra.idProyecto);
            InputParameterAdd.Varchar(objCommand, "fechaColeccion", ordenMuestra.fechaColeccion.ToString("yyyy'-'MM'-'dd"));
            InputParameterAdd.Varchar(objCommand, "horaColeccion", ordenMuestra.horaColeccion.ToString("HH':'mm"));
            InputParameterAdd.Int(objCommand, "numero", 1);
            InputParameterAdd.Int(objCommand, "seriado", 0);
            InputParameterAdd.Int(objCommand, "estado", 1);
            InputParameterAdd.Int(objCommand, "estatus", ordenMuestra.estatus);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMuestra.IdUsuarioRegistro);
            if (tipo == 2)
            {
                InputParameterAdd.Guid(objCommand, "idMuestraCod", ordenMuestra.idMuestraCod);
            }
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenMuestra");
            ExecuteNonQuery(objCommand);
            ordenMuestra.idOrdenMuestra = (Guid)objCommand.Parameters["@newIdOrdenMuestra"].Value;
            return ordenMuestra;
        }

        /// <summary>
        /// Descripción: Metodo que registra ordenes con tipo de muestra Banco de Sangre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenExamenOrdenMuestra"></param>
        public void InsertOrdenExamenOrdenMuestra(OrdenExamenOrdenMuestra ordenExamenOrdenMuestra)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenExamenOrdenMuestra");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", ordenExamenOrdenMuestra.idOrdenExamen);
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenExamenOrdenMuestra.idOrdenMuestra);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenExamenOrdenMuestra.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "estatus", ordenExamenOrdenMuestra.estatus);
            ExecuteNonQuery(objCommand);
        }
        public void InsertOrdenExamenLineal(OrdenExamenOrdenMuestra ordenExamenOrdenMuestra)
        {
            try
            {
                var objCommand = GetSqlCommand("pNLI_OrdenExamenLineal");
                InputParameterAdd.Guid(objCommand, "idOrdenExamen", ordenExamenOrdenMuestra.idOrdenExamen);
                InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenExamenOrdenMuestra.idOrdenMuestra);
                ExecuteNonQuery(objCommand);
            }
            catch (Exception ex)
            {
                string.Format("idOrdenExamen: {0} - idOrdenMuestra:{1} - ", ordenExamenOrdenMuestra.idOrdenMuestra, ordenExamenOrdenMuestra.idOrdenExamen);
                new bsPage().LogError(ex, "LogNetLab", string.Empty, "OrdenDal.InsertOrdenExamenLineal - ");
                throw ex;
            }
        }
        /// <summary>
        /// Descripción: Metodo que registra ordenes con tipo de muestra Cepa Banco de Sangre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        /// <returns></returns>
        public Orden InsertOrdenCBS(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenCBS");
            InputParameterAdd.Varchar(objCommand, "observacion", orden.observacion);
            InputParameterAdd.Varchar(objCommand, "nroOficio", orden.nroOficio);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", orden.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", orden.idEstablecimiento);
            InputParameterAdd.Int(objCommand, "idProyecto", orden.idProyecto);
            InputParameterAdd.Guid(objCommand, "idCepaBancoSangre", orden.idCepaBancoSangre);
            InputParameterAdd.Int(objCommand, "estatus", orden.estatus);
            OutputParameterAdd.Varchar(objCommand, "newCodigoOrden", 6);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrden");
            ExecuteNonQuery(objCommand);
            orden.idOrden = (Guid)objCommand.Parameters["@newIdOrden"].Value;
            orden.codigoOrden = (string)objCommand.Parameters["@newCodigoOrden"].Value;
            return orden;
        }

        /// <summary>
        /// Descripción: Metodo que registra examenes selecionados para una orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenExamen"></param>
        /// <returns></returns>
        public Guid InsertOrdenExamen(OrdenExamen ordenExamen)
        {
            try
            {
            var objCommand = GetSqlCommand("pNLI_OrdenExamen");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", ordenExamen.idOrdenExamen);
            InputParameterAdd.Guid(objCommand, "idOrden", ordenExamen.idOrden);
            InputParameterAdd.Guid(objCommand, "idExamen", ordenExamen.Examen.idExamen);
            InputParameterAdd.Int(objCommand, "idEnfermedad", ordenExamen.Enfermedad.idEnfermedad);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", ordenExamen.IdTipoMuestra);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenExamen.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "estatus", ordenExamen.estatus);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenExamen");
            ExecuteNonQuery(objCommand);
            return (Guid)objCommand.Parameters["@newIdOrdenExamen"].Value;
            }
            catch (Exception ex)
            {
                string.Format("idOrdenExamen: {0} - idOrden:{1} - idExamen: {2} - idEnfermedad: {3} - idTipoMuestra: {4} - idUsuarioregistro: {5} - ", ordenExamen.idOrdenExamen, ordenExamen.idOrden, ordenExamen.Examen.idExamen, ordenExamen.Enfermedad.idEnfermedad, ordenExamen.IdTipoMuestra, ordenExamen.IdUsuarioRegistro);
                new bsPage().LogError(ex, "LogNetLab", string.Empty, "OrdenDal.InsertOrdenExamen - ");
                throw ex;
            }
        }
        /// <summary>
        /// Descripción: Metodo que registra examenes selecionados para una orden con tipo de muestra banco de sangre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenExamen"></param>
        /// <returns></returns>
        public Guid InsertOrdenExamenBS(OrdenExamen ordenExamen)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenExamen");

            InputParameterAdd.Guid(objCommand, "idOrden", ordenExamen.idOrden);
            InputParameterAdd.Guid(objCommand, "idExamen", ordenExamen.idExamen);
            InputParameterAdd.Int(objCommand, "idEnfermedad", ordenExamen.idEnfermedad);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenExamen.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "estatus", ordenExamen.estatus);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenExamen");
            ExecuteNonQuery(objCommand);
            return (Guid)objCommand.Parameters["@newIdOrdenExamen"].Value;
        }
        /// <summary>
        /// Descripción: Metodo que registra materiales selecionados para una orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenMaterial"></param>
        /// <param name="estatusOrdenMuestraRecepcion"></param>
        /// <param name="insertaOrdenMuestraRecepcion"></param>
        public void InsertOrdenMaterial(OrdenMaterial ordenMaterial, int estatusOrdenMuestraRecepcion, bool insertaOrdenMuestraRecepcion)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMaterial");
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMaterial.idOrden);
            InputParameterAdd.Int(objCommand, "idMaterial", ordenMaterial.Material.idMaterial);
            InputParameterAdd.Int(objCommand, "cantidad", ordenMaterial.cantidad);
            InputParameterAdd.Int(objCommand, "idLaboratorio", ordenMaterial.Laboratorio.IdLaboratorio);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMaterial.IdUsuarioRegistro);
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenMaterial.ordenMuestra.idOrdenMuestra);
            InputParameterAdd.Int(objCommand, "estatus", ordenMaterial.estatus);
            InputParameterAdd.Decimal(objCommand, "volumenMuestraColectada", ordenMaterial.volumenMuestraColectada);
            InputParameterAdd.Int(objCommand, "noPrecisaVolumen", ordenMaterial.noPrecisaVolumen);
            InputParameterAdd.DateTime(objCommand, "fechaEnvio", ordenMaterial.fechaEnvio);
            InputParameterAdd.Varchar(objCommand, "horaEnvio", ordenMaterial.horaEnvio.ToString("HH':'mm"));
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", ordenMaterial.OrdenExamen.idOrdenExamen);
            InputParameterAdd.Int(objCommand, "tipo", ordenMaterial.Tipo);
            InputParameterAdd.Int(objCommand, "estatusOrdenMuestraRecepcion", estatusOrdenMuestraRecepcion);
            InputParameterAdd.Int(objCommand, "insertarOrdenMuestraRecepcion", insertaOrdenMuestraRecepcion ? 1 : 0);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenMaterial");
            ExecuteNonQuery(objCommand);
            ordenMaterial.idOrdenMaterial = (Guid)objCommand.Parameters["@newIdOrdenMaterial"].Value;
        }
        /// <summary>
        /// Descripción: Metodo que registra la recepcion de una orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenMuestraRecepcion"></param>
        public void InsertOrdenMuestraRecepcion(OrdenMuestraRecepcion ordenMuestraRecepcion)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMuestraRecepcion");
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMuestraRecepcion.idOrden);
            InputParameterAdd.Guid(objCommand, "idOrdenMaterial", ordenMuestraRecepcion.OrdenMaterial.idOrdenMaterial);
            InputParameterAdd.Int(objCommand, "idLaboratorio", ordenMuestraRecepcion.OrdenMaterial.Laboratorio.IdLaboratorio);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMuestraRecepcion.OrdenMaterial.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "estatus", ordenMuestraRecepcion.OrdenMaterial.estatus);
            InputParameterAdd.DateTime(objCommand, "fechaEnvio", ordenMuestraRecepcion.OrdenMaterial.fechaEnvio);
            InputParameterAdd.Varchar(objCommand, "horaEnvio", ordenMuestraRecepcion.OrdenMaterial.horaEnvio.ToString("HH':'mm"));
            InputParameterAdd.Int(objCommand, "conformeR", ordenMuestraRecepcion.conforme);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenMuestraRecepcion");
            ExecuteNonQuery(objCommand);
            ordenMuestraRecepcion.idOrdenMuestraRecepcion = (Guid)objCommand.Parameters["@newIdOrdenMuestraRecepcion"].Value;
        }

        /// <summary>
        /// Descripción: Metodo que registra el rechazo de una orden en la recepcion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="criterioRechazo"></param>
        public void InsertOrdenMuestraRecepcionRechazo(CriterioRechazo criterioRechazo)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMuestraRecepcionRechazo");
            InputParameterAdd.Guid(objCommand, "idOrdenMuestraRecepcion", criterioRechazo.IdOrdenMuestraRecepcion);
            InputParameterAdd.Int(objCommand, "idCriterioRechazo", criterioRechazo.IdCriterioRechazo);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", criterioRechazo.IdUsuarioRegistro);
            ExecuteNonQuery(objCommand);
        }

        /// <summary>
        /// Descripción: Metodo que registra el resultado de una orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        public void InsertOrdenResultadoAnalito(Orden orden)
        {
            var objCommand2 = GetSqlCommand("pNLI_OrdenResultadoAnalito");
            InputParameterAdd.Guid(objCommand2, "idOrden", orden.idOrden);
            Execute(objCommand2);
        }

        /// <summary>
        /// Descripción: Metodo que registra el material de una orden con tipo de muestra banco de sangre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenMaterial"></param>
        /// <param name="idOrdenMuestra"></param>
        public void InsertOrdenMaterialBS(OrdenMaterial ordenMaterial, Guid idOrdenMuestra)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMaterialBS");
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMaterial.idOrden);
            InputParameterAdd.Int(objCommand, "idMaterial", ordenMaterial.idMaterial);
            //     InputParameterAdd.Int(objCommand, "ordenGrabado", ordenMaterial.ordenGrabado);
            InputParameterAdd.Int(objCommand, "cantidad", ordenMaterial.cantidad);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", ordenMaterial.ordenMuestra.Establecimiento.IdEstablecimiento);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMaterial.IdUsuarioRegistro);
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", idOrdenMuestra);
            //InputParameterAdd.Int(objCommand, "estatus", ordenMaterial.estatus;
            InputParameterAdd.Int(objCommand, "estatus", 1);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que registra los datos clinicos de una orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenDatoClinico"></param>
        public void InsertOrdenDatoClinico(OrdenDatoClinico ordenDatoClinico)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenDatoClinico");
            InputParameterAdd.Guid(objCommand, "idOrden", ordenDatoClinico.idOrden);
            InputParameterAdd.Int(objCommand, "idDato", ordenDatoClinico.Dato.IdDato);
            InputParameterAdd.Int(objCommand, "idCategoriaDato", ordenDatoClinico.idCategoriaDato);
            InputParameterAdd.Varchar(objCommand, "valor", ordenDatoClinico.ValorString);
            InputParameterAdd.Int(objCommand, "estado", 0);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenDatoClinico.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idEnfermedad", ordenDatoClinico.Enfermedad.idEnfermedad);
            InputParameterAdd.Int(objCommand, "noPrecisa", ordenDatoClinico.noPrecisa ? 1 : 0);
            InputParameterAdd.Int(objCommand, "estatus", ordenDatoClinico.estatus);

            ExecuteNonQuery(objCommand);
        }
        public void InsertOrdenDatoClinicoEdicion(OrdenDatoClinico ordenDatoClinico)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenDatoClinicoEdicion");
            InputParameterAdd.Guid(objCommand, "idOrden", ordenDatoClinico.idOrden);
            InputParameterAdd.Int(objCommand, "idDato", ordenDatoClinico.Dato.IdDato);
            InputParameterAdd.Int(objCommand, "idCategoriaDato", ordenDatoClinico.idCategoriaDato);
            InputParameterAdd.Varchar(objCommand, "valor", ordenDatoClinico.ValorString);
            InputParameterAdd.Int(objCommand, "estado", 0);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenDatoClinico.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "idEnfermedad", ordenDatoClinico.Enfermedad.idEnfermedad);
            InputParameterAdd.Int(objCommand, "noPrecisa", ordenDatoClinico.noPrecisa ? 1 : 0);
            InputParameterAdd.Int(objCommand, "estatus", ordenDatoClinico.estatus);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que registra las muestras en la tabla CepaBancoSangre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        public CepaBancoSangre InsertCepaBancoSangre(CepaBancoSangre item, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLI_CepaBancoSangre");
            //InputParameterAdd.Guid(objCommand, "idOrden", item.idOrden);
            InputParameterAdd.Varchar(objCommand, "codificacion", item.codificacion);
            InputParameterAdd.Varchar(objCommand, "fechaColeccion", item.fechaColeccion.ToString("yyyy'-'MM'-'dd"));
            InputParameterAdd.Varchar(objCommand, "horaColeccion", item.horaColeccion.ToString("HH':'mm"));
            InputParameterAdd.Int(objCommand, "tipo", item.tipo);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", item.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idMaterial", item.idMaterial);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdCepaBancoSangre");
            OutputParameterAdd.UniqueIdentifier(objCommand, "idMuestraCodificacion");
            ExecuteNonQuery(objCommand);
            item.idCepaBancoSangre = (Guid)objCommand.Parameters["@newIdCepaBancoSangre"].Value;

            try
            {
                item.idMuestraCod = Guid.Parse(objCommand.Parameters["@idMuestraCodificacion"].Value.ToString());
            }
            catch (Exception)
            {

            }
            return item;
        }
        /// <summary>
        /// Descripción: Metodo que elimina las ordenes de las siguientes tablas :
        /// OrdenExamenOrdenMuestra
        /// OrdenExamen
        /// OrdenMuestraRecepcion
        /// OrdenMuestra
        /// OrdenMaterial
        /// CepaBancoSangre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        public void CleanOrdenCBS(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLD_CleanOrdenCBS");
            InputParameterAdd.Guid(objCommand, "idOrden", orden.idOrden);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que actualiza la informacion de las ordenes de Cepa Banco de Sangre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="orden"></param>
        public void UpdateOrdenCBS(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenCBS");
            InputParameterAdd.Varchar(objCommand, "observacion", orden.observacion);
            InputParameterAdd.Varchar(objCommand, "nroOficio", orden.nroOficio);
            InputParameterAdd.Int(objCommand, "idProyecto", orden.idProyecto);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", orden.IdUsuarioEdicion);
            InputParameterAdd.Guid(objCommand, "idCepaBancoSangre", orden.idCepaBancoSangre);
            InputParameterAdd.Int(objCommand, "estatus", orden.estatus);
            InputParameterAdd.Guid(objCommand, "idOrden", orden.idOrden);
            ExecuteNonQuery(objCommand);
        }

        /// <summary>
        /// Descripción: Metodo que otiene toda la informacion de la orden filtrado por el Codigo del mismo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public Orden GetOrdenById(Guid idOrden)
        {
            List<Orden> ordenList = new List<Orden>();

            var objCommand = GetSqlCommand("pNLS_OrdenById");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            DataSet dataSet = ExecuteDataSet(objCommand);

            DataTable ordenDataTable = dataSet.Tables[0];
            DataTable ordenExamenDataTable = dataSet.Tables[1];
            DataTable ordenMuestraDataTable = dataSet.Tables[2];
            DataTable ordenExamenOrdenMuestraTable = dataSet.Tables[3];
            DataTable ordenMaterialDataTable = dataSet.Tables[4];
            DataTable enfermedadDataTable = dataSet.Tables[5];
            DataTable opcionDataTable = dataSet.Tables[6];
            DataTable ordenMuestraRecepcionarTable = dataSet.Tables[7]; //Juan Muga

            #region Orden
            Orden orden = null;
            foreach (DataRow row in ordenDataTable.Rows)
            {
                orden = new Orden();

                orden.codigoOrden = Converter.GetString(row, "codigoOrden");

                orden.Paciente = new Paciente();
                orden.Paciente.NroDocumento = Converter.GetString(row, "paciente_nrodocumento");
                orden.Paciente.Nombres = Converter.GetString(row, "paciente_nombres");
                orden.Paciente.ApellidoMaterno = Converter.GetString(row, "paciente_apellidoMaterno");
                orden.Paciente.ApellidoPaterno = Converter.GetString(row, "paciente_apellidoPaterno");
                orden.Paciente.UbigeoActual = new Ubigeo();
                orden.Paciente.UbigeoActual.Id = Converter.GetString(row, "idUbigeoActual");
                orden.Paciente.UbigeoActual.Departamento = Converter.GetString(row, "paciente_ubigeoactual_departamento");
                orden.Paciente.UbigeoActual.Provincia = Converter.GetString(row, "paciente_ubigeoactual_provincia");
                orden.Paciente.UbigeoActual.Distrito = Converter.GetString(row, "paciente_ubigeoactual_distrito");
                orden.Paciente.DireccionActual = Converter.GetString(row, "paciente_direccionactual");
                orden.Paciente.edadAnios = Converter.GetInt(row, "Edad");
                orden.Paciente.TelefonoFijo = Converter.GetString(row, "telefonoFijo");
                orden.Paciente.Celular1 = Converter.GetString(row, "celular1");
                orden.Paciente.Celular2 = Converter.GetString(row, "celular2");
                orden.Paciente.correoElectronico = Converter.GetString(row, "correoElectronico");
                orden.Paciente.ocupacion = Converter.GetString(row, "ocupacion");
                orden.Paciente.tipoSeguroSalud = Converter.GetInt(row, "tipoSeguroSalud");
                orden.Paciente.otroSeguroSalud = Converter.GetString(row, "otroSeguroSalud");
                orden.Paciente.tipoSeguro = Converter.GetString(row, "tipoSeguro");
                orden.usuario = new Usuario();
                orden.usuario.nombres = Converter.GetString(row, "UsuarioRegistro");


                //orden.idProyecto
                orden.Proyecto = new Proyecto();
                orden.Proyecto.Nombre = Converter.GetString(row, "proyecto_nombre");
                orden.Proyecto.IdProyecto = Converter.GetInt(row, "proyecto_idProyecto");
                //orden.idEstablecimiento
                orden.establecimiento = new Establecimiento();
                orden.establecimiento.Nombre = Converter.GetString(row, "establecimiento_nombre");
                orden.establecimiento.IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento");
                orden.idEstablecimiento = orden.establecimiento.IdEstablecimiento;

                orden.establecimientoEnvio = new Establecimiento();
                orden.establecimientoEnvio.Nombre = Converter.GetString(row, "establecimientoEnvio_nombre");
                orden.establecimientoEnvio.IdEstablecimiento = Converter.GetInt(row, "idEstablecimientoEnvio");
                orden.idEstablecimientoEnvio = orden.establecimiento.IdEstablecimiento;


                orden.nroOficio = Converter.GetString(row, "nroOficio");
                orden.observacion = Converter.GetString(row, "observacion");
                orden.fechaSolicitud = Converter.GetDateTime(row, "fechaSolicitud");
                orden.FechaRegistro = Converter.GetDateTime(row, "fechaRegistro");
                orden.solicitante = Converter.GetString(row, "solicitante");
                orden.estatus = Converter.GetInt(row, "estatus");
                orden.fechaIngresoINS = Converter.GetDateTimeNullable(row, "fechaRecepcionRomINS");
                orden.fechaReevaluacionFicha = Converter.GetDateTimeNullable(row, "fechaReevaluacionFicha");
                orden.idOrden = idOrden;
            }

            #endregion

            #region OrdenExamen
            orden.ordenExamenList = new List<OrdenExamen>();
            foreach (DataRow row in ordenExamenDataTable.Rows)
            {
                OrdenExamen ordenExamen = new OrdenExamen();
                ordenExamen.idOrdenExamen = Converter.GetGuid(row, "idOrdenExamen");
                ordenExamen.Enfermedad = new Enfermedad();
                ordenExamen.Enfermedad.idEnfermedad = Converter.GetInt(row, "enfermedad_idEnfermedad");
                ordenExamen.Enfermedad.nombre = Converter.GetString(row, "enfermedad_nombre");
                ordenExamen.Enfermedad.Snomed = Converter.GetString(row, "snomed");
                ordenExamen.Examen = new Examen();
                ordenExamen.Examen.nombre = Converter.GetString(row, "examen_nombre");
                ordenExamen.Examen.idExamen = Converter.GetGuid(row, "examen_idExamen");
                ordenExamen.Examen.PruebaRapida = Converter.GetInt(row, "iPruebaRapida");
                ordenExamen.estatusE = Converter.GetInt(row, "estatusE");
                ordenExamen.ordenMuestraList = new List<OrdenMuestra>();
                orden.ordenExamenList.Add(ordenExamen);
            }


            #endregion

            #region OrdenMuestra
            orden.ordenMuestraList = new List<OrdenMuestra>();
            foreach (DataRow row in ordenMuestraDataTable.Rows)
            {
                OrdenMuestra ordenMuestra = new OrdenMuestra();
                ordenMuestra.idOrdenMuestra = Converter.GetGuid(row, "idOrdenMuestra");
                ordenMuestra.MuestraCodificacion = new MuestraCodificacion();
                ordenMuestra.MuestraCodificacion.codificacion = Converter.GetString(row, "muestracodificacion_codificacion");
                ordenMuestra.TipoMuestra = new TipoMuestra();
                ordenMuestra.TipoMuestra.nombre = Converter.GetString(row, "tipomuestra_nombre");
                ordenMuestra.TipoMuestra.idTipoMuestra = Converter.GetInt(row, "idTipoMuestra");
                //     ordenMuestra.Laboratorio = new Laboratorio();
                //ordenMuestra.Laboratorio.Nombre = Converter.GetString(row, "laboratorio_nombre");
                //ordenMuestra.Laboratorio.IdLaboratorio = Converter.GetInt(row, "laboratorio_idLaboratorio");
                ordenMuestra.fechaColeccion = Converter.GetDateTime(row, "fechaColeccion");
                string[] horaColeccion = Converter.GetString(row, "horaColeccion").Split(':');
                ordenMuestra.horaColeccion = new DateTime(1, 1, 1, Convert.ToInt32(horaColeccion[0]), Convert.ToInt32(horaColeccion[1]), Convert.ToInt32(horaColeccion[2]));
                //       ordenMuestra.fechaEnvio = Converter.GetDateTime(row, "fechaEnvio");
                ordenMuestra.ordenExamenList = new List<OrdenExamen>();
                orden.ordenMuestraList.Add(ordenMuestra);
            }




            #endregion

            #region ordenExamenOrdenMuestraTable

            foreach (DataRow row in ordenExamenOrdenMuestraTable.Rows)
            {
                OrdenExamenOrdenMuestra ordenExamenOrdenMuestra = new OrdenExamenOrdenMuestra();
                ordenExamenOrdenMuestra.idOrdenExamen = Converter.GetGuid(row, "idOrdenExamen");
                ordenExamenOrdenMuestra.idOrdenMuestra = Converter.GetGuid(row, "idOrdenMuestra");
                //Se agrega la Muestra al Examen
                //Juan Muga
                //OrdenExamen ordenExamen = orden.ordenExamenList.Where(x => x.idOrdenExamen == ordenExamenOrdenMuestra.idOrdenExamen).First();
                //OrdenMuestra ordenMuestra = orden.ordenMuestraList.Where(x => x.idOrdenMuestra == ordenExamenOrdenMuestra.idOrdenMuestra).First();
                OrdenExamen ordenExamen = orden.ordenExamenList.Any() ? orden.ordenExamenList.First(x => x.idOrdenExamen == ordenExamenOrdenMuestra.idOrdenExamen) : new OrdenExamen();
                OrdenMuestra ordenMuestra = orden.ordenMuestraList.Any() ? orden.ordenMuestraList.First(x => x.idOrdenMuestra == ordenExamenOrdenMuestra.idOrdenMuestra) : new OrdenMuestra();

                ordenExamen.ordenMuestraList.Add(ordenMuestra);
                ordenMuestra.ordenExamenList.Add(ordenExamen);
            }

            #endregion

            //Juan Muga - inicio
            List<OrdenMuestraRecepcion> ordenMuestraRecepcionList = new List<OrdenMuestraRecepcion>();
            foreach (DataRow row in ordenMuestraRecepcionarTable.Rows)
            {
                ordenMuestraRecepcionList.Add(new OrdenMuestraRecepcion
                {
                    idOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion"),
                    idOrdenMaterial = Converter.GetGuid(row, "idOrdenMaterial"),
                    idOrden = Converter.GetGuid(row, "idOrdenMaterial"),
                    fechaRecepcion = Converter.GetDateTimeNullable(row, "fechaRecepcion"),
                    horaRecepcion = Converter.GetDateTimeNullable(row, "horaRecepcion"),
                    idLaboratorioOrigen = Converter.GetInt(row, "idLaboratorioOrigen"),
                    idUsuarioRegistro = Converter.GetInt(row, "idUsuarioRegistro"),
                    idLaboratorioDestino = Converter.GetInt(row, "idLaboratorioDestino"),
                    estatusR = Converter.GetInt(row, "estatusR"),
                    estatus = Converter.GetInt(row, "estatus"),
                    fechaEnvio = Converter.GetDateTimeNullable(row, "fechaEnvio"),
                    horaEnvio = Converter.GetDateTimeNullable(row, "horaEnvio"),
                    criterioRechazoList = new List<CriterioRechazo>()
                });
            }
            //Juan Muga - Fin


            #region OrdenMaterial

            orden.ordenMaterialList = new List<OrdenMaterial>();
            foreach (DataRow row in ordenMaterialDataTable.Rows)
            {
                OrdenMaterial ordenMaterial = new OrdenMaterial();
                ordenMaterial.idOrdenMaterial = Converter.GetGuid(row, "idOrdenMaterial");
                ordenMaterial.Material = new Material
                {
                    idMaterial = Converter.GetInt(row, "idMaterial"),
                    Presentacion = new Presentacion
                    {
                        idPresentacion = Converter.GetInt(row, "idPresentacion"),
                        glosa = Converter.GetString(row, "glosaPresentacion")
                    },
                    Reactivo = new Reactivo
                    {
                        idReactivo = Converter.GetInt(row, "idReactivo"),
                        glosa = Converter.GetString(row, "glosaReactivo")
                    },
                    TipoMuestra = new TipoMuestra
                    {
                        idTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                        nombre = Converter.GetString(row, "nombreTipoMuestra")
                    },
                    volumen = Converter.GetDecimal(row, "volumen")
                };
                ordenMaterial.ordenMuestra = new OrdenMuestra();
                ordenMaterial.Laboratorio = new Laboratorio();
                ordenMaterial.Laboratorio.IdLaboratorio = Converter.GetInt(row, "ordenmuestra_laboratorio_idLaboratorio");
                ordenMaterial.Laboratorio.Nombre = Converter.GetString(row, "ordenmuestra_laboratorio_nombre");

                ordenMaterial.OrdenExamen = new OrdenExamen();
                ordenMaterial.OrdenExamen.Examen = new Examen();
                ordenMaterial.OrdenExamen.idOrdenExamen = Converter.GetGuid(row, "idOrdenExamen"); //Juan Muga
                ordenMaterial.OrdenExamen.Examen.nombre = Converter.GetString(row, "examen");
                ordenMaterial.OrdenExamen.Examen.idExamen = Converter.GetGuid(row, "idExamen");
                ordenMaterial.OrdenExamen.Enfermedad = new Enfermedad();
                ordenMaterial.OrdenExamen.Enfermedad.nombre = Converter.GetString(row, "enfermedad");
                ordenMaterial.OrdenExamen.Enfermedad.idEnfermedad = Converter.GetInt(row, "idEnfermedad");

                ordenMaterial.cantidad = Converter.GetInt(row, "cantidad");
                ordenMaterial.volumenMuestraColectada = Converter.GetDecimal(row, "volumenMuestraColectada");
                ordenMaterial.noPrecisaVolumen = Converter.GetInt(row, "noPrecisaVolumen");
                ordenMaterial.fechaEnvio = Converter.GetDateTime(row, "fechaEnvio");
                ordenMaterial.Tipo = Converter.GetInt(row, "tipo");

                ordenMaterial.ordenMuestraRecepcionList = new List<OrdenMuestraRecepcion>();
                //Juan Muga - inicio
                if (ordenMuestraRecepcionList.Any(a => a.idOrdenMaterial == ordenMaterial.idOrdenMaterial))
                {
                    ordenMaterial.ordenMuestraRecepcionList = ordenMuestraRecepcionList.Where(a => a.idOrdenMaterial == ordenMaterial.idOrdenMaterial).ToList();
                }
                //Juan Muga - fin
                /*
                if (radioRecepcion == 0 && (Enums.TipoRegistroOrden)this.Session["tipoRegistro"] != Enums.TipoRegistroOrden.SOLO_ORDEN)
                {
                    for (int i = 0; i < cantidad; i++)
                    {
                        OrdenMuestraRecepcion ordenMuestraRecepcion = new OrdenMuestraRecepcion();
                         ordenMuestraRecepcion.
                        ordenMuestraRecepcion.conforme = 1;
                        ordenMuestraRecepcion.idOrdenMuestraRecepcion = Guid.NewGuid();

                        CriterioRechazoBl criterioRechazoBL = new CriterioRechazoBl();
                        ordenMuestraRecepcion.criterioRechazoList = criterioRechazoBL.GetCriteriosByTipoMuestra(ordenMuestra.TipoMuestra.idTipoMuestra, 2);

                        foreach (CriterioRechazo criterioRechazo in ordenMuestraRecepcion.criterioRechazoList)
                        {
                            criterioRechazo.rechazado = false;
                        }

                        ordenMaterial.ordenMuestraRecepcionList.Add(ordenMuestraRecepcion);
                    }
                }

    */


                string[] horaEnvio = Converter.GetString(row, "horaEnvio").Split(':');
                ordenMaterial.horaEnvio = new DateTime(1, 1, 1, Convert.ToInt32(horaEnvio[0]), Convert.ToInt32(horaEnvio[1]), Convert.ToInt32(horaEnvio[2]));

                orden.ordenMaterialList.Add(ordenMaterial);
            }
            #endregion
            #region DatosClinicos

            int idListaDato = 0;
            List<ListaDato> listaDatoList = new List<ListaDato>();

            ListaDato listaDato = null;
            foreach (DataRow row in opcionDataTable.Rows)
            {
                int idListaDatoNew = Converter.GetInt(row, "idListaDato");

                if (idListaDato != idListaDatoNew)
                {
                    idListaDato = idListaDatoNew;
                    listaDato = new ListaDato
                    {
                        IdListaDato = idListaDatoNew
                    };
                    listaDato.Opciones = new List<OpcionDato>();
                    listaDatoList.Add(listaDato);
                }
                OpcionDato opcionDato = new OpcionDato();
                opcionDato.IdOpcionDato = Converter.GetInt(row, "IdOpcionDato");
                opcionDato.Valor = Converter.GetString(row, "Valor");

                listaDato.Opciones.Add(opcionDato);
            }

            orden.enfermedadList = new List<Enfermedad>();

            int idEnfermedad = 0;
            int idCategoriaDato = 0;
            Enfermedad enfermedad = null;
            CategoriaDato categoriaDato = null;
            foreach (DataRow row in enfermedadDataTable.Rows)
            {
                //   List<Enfermedad> enfermedadList = new List<Enfermedad>();

                int idEnfermedadNew = Converter.GetInt(row, "idEnfermedad");


                if (!idEnfermedadNew.Equals(idEnfermedad))
                {
                    idEnfermedad = idEnfermedadNew;
                    enfermedad = new Enfermedad();
                    enfermedad.idEnfermedad = idEnfermedadNew;
                    enfermedad.nombre = Converter.GetString(row, "nombreEnfermedad");
                    enfermedad.categoriaDatoList = new List<CategoriaDato>();
                    orden.enfermedadList.Add(enfermedad);
                }

                int idCategoriaDatoNew = Converter.GetInt(row, "idCategoriaDato");


                if (idCategoriaDatoNew != idCategoriaDato)
                {
                    idCategoriaDato = idCategoriaDatoNew;
                    categoriaDato = new CategoriaDato();

                    categoriaDato.IdCategoriaDato = idCategoriaDatoNew;
                    categoriaDato.Nombre = Converter.GetString(row, "nombreCategoriaDato");
                    categoriaDato.Orientacion = Converter.GetInt(row, "orientacion");
                    categoriaDato.OrdenDatoClinicoList = new List<OrdenDatoClinico>();
                    enfermedad.categoriaDatoList.Add(categoriaDato);

                }


                OrdenDatoClinico ordenDatoClinico = new OrdenDatoClinico();
                ordenDatoClinico.noPrecisa = Converter.GetInt(row, "noPrecisa") == 1;
                ordenDatoClinico.ValorString = Converter.GetString(row, "valor");
                ordenDatoClinico.Enfermedad = enfermedad;
                ordenDatoClinico.Dato = new Dato();
                ordenDatoClinico.Dato.IdDato = Converter.GetInt(row, "idDato");
                ordenDatoClinico.Dato.IdListaDato = Converter.GetInt(row, "idListaDato");
                ordenDatoClinico.Dato.IdTipo = Converter.GetInt(row, "idTipo");
                if ((int)Enums.TipoCampo.COMBO == ordenDatoClinico.Dato.IdTipo)
                {
                    ordenDatoClinico.Dato.ListaDato = listaDatoList.Where(l => l.IdListaDato == ordenDatoClinico.Dato.IdListaDato).FirstOrDefault();
                }

                ordenDatoClinico.Dato.Prefijo = Converter.GetString(row, "prefijo");
                categoriaDato.OrdenDatoClinicoList.Add(ordenDatoClinico);
            }

            #endregion

            return orden;
        }
        /// <summary>
        /// Descripción: Metodo que otiene toda la informacion de la orden del paciente generada por medio de una plantilla
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <param name="idEstablecimiento"></param>
        /// <param name="idPaciente"></param>
        /// <returns></returns>
        public Orden GetOrdenByIdPlantilla(int idPlantilla, int idEstablecimiento, Guid idPaciente)
        {
            var objCommand = GetSqlCommand("pNLS_OrdenFromPlantilla");
            InputParameterAdd.Int(objCommand, "idPlantilla", idPlantilla);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Guid(objCommand, "idpaciente", idPaciente);

            var dataSet = ExecuteDataSet(objCommand);

            var ordenDataTable = dataSet.Tables[0];
            var ordenExamenDataTable = dataSet.Tables[1];
            var ordenMuestraDataTable = dataSet.Tables[2];
            var ordenMaterialDataTable = dataSet.Tables[3];
            var enfermedadDataTable = dataSet.Tables[4];
            var opcionDataTable = dataSet.Tables[5];

            #region Orden

            Orden orden = null;

            foreach (DataRow row in ordenDataTable.Rows)
            {
                orden = new Orden
                {
                    codigoOrden = null,
                    Paciente = new Paciente
                    {
                        IdPaciente = Converter.GetGuid(row, "idPaciente"),
                        ApellidoPaterno = Converter.GetString(row, "apellidoPaterno"),
                        ApellidoMaterno = Converter.GetString(row, "apellidoMaterno"),
                        Nombres = Converter.GetString(row, "nombres"),
                        Genero = Converter.GetInt(row, "genero"),
                        FechaNacimiento = Converter.GetDateTime(row, "fechaNacimiento"),
                        TipoDocumento = Converter.GetInt(row, "tipoDocumento"),
                        NroDocumento = Converter.GetString(row, "nroDocumento"),
                        UbigeoActual = new Ubigeo
                        {
                            Id = Converter.GetString(row, "idUbigeoActual"),
                            Departamento = Converter.GetString(row, "departamentoActual"),
                            Provincia = Converter.GetString(row, "provinciaActual"),
                            Distrito = Converter.GetString(row, "distritoActual")
                        },
                        DireccionActual = Converter.GetString(row, "direccionActual"),
                        TelefonoFijo = Converter.GetString(row, "telefonoFijo"),
                        Celular1 = Converter.GetString(row, "celular1"),
                        Celular2 = Converter.GetString(row, "celular2"),
                        correoElectronico = Converter.GetString(row, "correoElectronico"),
                        ocupacion = Converter.GetString(row, "ocupacion"),
                        tipoSeguroSalud = Converter.GetInt(row, "tipoSeguroSalud"),
                        otroSeguroSalud = Converter.GetString(row, "otroSeguroSalud"),
                        tipoSeguro = Converter.GetString(row, "tipoSeguro"),
                        estatus = Converter.GetInt(row, "estatus"),
                        edadAnios = Converter.GetInt(row, "Edad")
                    },
                    Proyecto = new Proyecto(),
                    establecimiento = new Establecimiento
                    {
                        Nombre = Converter.GetString(row, "establecimiento_nombre"),
                        IdEstablecimiento = idEstablecimiento
                    },
                    idEstablecimiento = idEstablecimiento,
                    fechaSolicitud = new DateTime(),
                    nroOficio = "",
                    observacion = "",
                    estatus = 0
                };
            }

            if (orden == null) return null;
            #endregion

            #region OrdenExamen

            orden.ordenExamenList = new List<OrdenExamen>();

            foreach (DataRow row in ordenExamenDataTable.Rows)
            {
                var ordenExamen = new OrdenExamen
                {
                    idOrdenExamen = Guid.NewGuid(),
                    Enfermedad = new Enfermedad
                    {
                        idEnfermedad = Converter.GetInt(row, "enfermedad_idEnfermedad"),
                        nombre = Converter.GetString(row, "enfermedad_nombre"),
                        Snomed = Converter.GetString(row, "enfermedad_snomed")
                    },
                    Examen = new Examen
                    {
                        nombre = Converter.GetString(row, "examen_nombre"),
                        idExamen = Converter.GetGuid(row, "examen_idExamen")
                    },
                    IdTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                    ordenMuestraList = new List<OrdenMuestra>()
                };

                orden.ordenExamenList.Add(ordenExamen);
            }

            #endregion

            #region OrdenMuestra

            orden.ordenMuestraList = new List<OrdenMuestra>();

            foreach (DataRow row in ordenMuestraDataTable.Rows)
            {
                var horaColeccion = Converter.GetString(row, "horaColeccion").Split(':');

                var ordenMuestra = new OrdenMuestra
                {
                    idOrdenMuestra = Guid.NewGuid(),
                    MuestraCodificacion = new MuestraCodificacion
                    {
                        codificacion = Converter.GetString(row, "muestracodificacion_codificacion")
                    },
                    TipoMuestra = new TipoMuestra
                    {
                        nombre = Converter.GetString(row, "tipomuestra_nombre"),
                        idTipoMuestra = Converter.GetInt(row, "idTipoMuestra")
                    },
                    fechaColeccion = Converter.GetDateTime(row, "fechaColeccion"),
                    horaColeccion = new DateTime(1, 1, 1, Convert.ToInt32(horaColeccion[0]), Convert.ToInt32(horaColeccion[1]), Convert.ToInt32(horaColeccion[2])),
                    idTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                    ordenExamenList = new List<OrdenExamen>()
                };

                orden.ordenMuestraList.Add(ordenMuestra);

                //Se asocian todos los examenes a todas las muestras
                foreach (var ordenExamen in orden.ordenExamenList)
                {
                    ordenMuestra.ordenExamenList.Add(ordenExamen);
                    if (ordenExamen.IdTipoMuestra == ordenMuestra.idTipoMuestra)
                    {
                        ordenExamen.ordenMuestraList.Add(ordenMuestra);
                    }
                }
            }

            #endregion

            #region OrdenMaterial

            orden.ordenMaterialList = new List<OrdenMaterial>();

            foreach (DataRow row in ordenMaterialDataTable.Rows)
            {
                var ordenMaterial = new OrdenMaterial
                {
                    idOrdenMaterial = Guid.NewGuid(),
                    Material = new Material
                    {
                        idMaterial = Converter.GetInt(row, "idMaterial"),
                        Presentacion = new Presentacion
                        {
                            idPresentacion = Converter.GetInt(row, "idPresentacion"),
                            glosa = Converter.GetString(row, "glosaPresentacion")
                        },
                        Reactivo = new Reactivo
                        {
                            idReactivo = Converter.GetInt(row, "idReactivo"),
                            glosa = Converter.GetString(row, "glosaReactivo")
                        },
                        TipoMuestra = new TipoMuestra
                        {
                            idTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                            nombre = Converter.GetString(row, "nombreTipoMuestra")
                        },
                        volumen = Converter.GetDecimal(row, "volumen")
                    },
                    ordenMuestra = new OrdenMuestra(),
                    Laboratorio = new Laboratorio(),
                    OrdenExamen = new OrdenExamen
                    {
                        Enfermedad = new Enfermedad
                        {
                            idEnfermedad = Converter.GetInt(row, "enfermedad_idEnfermedad"),
                            nombre = Converter.GetString(row, "enfermedad_nombre"),
                            Snomed = Converter.GetString(row, "enfermedad_snomed")
                        },
                        Examen = new Examen
                        {
                            nombre = Converter.GetString(row, "examen_nombrematerial"),
                            idExamen = Converter.GetGuid(row, "examen_idExamenmaterial")
                        },
                    },
                    cantidad = Converter.GetInt(row, "cantidad"),
                    volumenMuestraColectada = 1,
                    noPrecisaVolumen = '0',
                    fechaEnvio = DateTime.Now,
                    horaEnvio = DateTime.Now,
                    ExamenNombreCompleto = Converter.GetString(row, "examen_nombrematerial")
                };

                orden.ordenMaterialList.Add(ordenMaterial);
            }

            var idListaDato = 0;
            var listaDatoList = new List<ListaDato>();
            ListaDato listaDato = null;

            foreach (DataRow row in opcionDataTable.Rows)
            {
                var idListaDatoNew = Converter.GetInt(row, "idListaDato");

                if (idListaDato != idListaDatoNew)
                {
                    idListaDato = idListaDatoNew;
                    listaDato = new ListaDato
                    {
                        IdListaDato = idListaDato,
                        Opciones = new List<OpcionDato>()
                    };
                    listaDatoList.Add(listaDato);
                }

                var opcionDato = new OpcionDato
                {
                    IdOpcionDato = Converter.GetInt(row, "IdOpcionDato"),
                    Valor = Converter.GetString(row, "Valor")
                };

                listaDato?.Opciones.Add(opcionDato);
            }

            orden.enfermedadList = new List<Enfermedad>();

            var idEnfermedad = 0;
            var idCategoriaDato = 0;
            Enfermedad enfermedad = null;
            CategoriaDato categoriaDato = null;

            foreach (DataRow row in enfermedadDataTable.Rows)
            {
                var idEnfermedadNew = Converter.GetInt(row, "idEnfermedad");

                if (!idEnfermedadNew.Equals(idEnfermedad))
                {
                    idEnfermedad = idEnfermedadNew;
                    enfermedad = new Enfermedad
                    {
                        idEnfermedad = idEnfermedadNew,
                        nombre = Converter.GetString(row, "nombreEnfermedad"),
                        Snomed = Converter.GetString(row, "snomed"),
                        categoriaDatoList = new List<CategoriaDato>()
                    };

                    orden.enfermedadList.Add(enfermedad);
                }

                var idCategoriaDatoNew = Converter.GetInt(row, "idCategoriaDato");

                if (idCategoriaDatoNew != idCategoriaDato)
                {
                    idCategoriaDato = idCategoriaDatoNew;
                    categoriaDato = new CategoriaDato
                    {
                        IdCategoriaDato = idCategoriaDatoNew,
                        Nombre = Converter.GetString(row, "nombreCategoriaDato"),
                        Orientacion = Converter.GetInt(row, "orientacion"),
                        OrdenDatoClinicoList = new List<OrdenDatoClinico>()
                    };

                    enfermedad?.categoriaDatoList.Add(categoriaDato);
                }

                var ordenDatoClinico = new OrdenDatoClinico
                {
                    noPrecisa = false,
                    ValorString = Converter.GetString(row, "valor"),
                    Enfermedad = enfermedad,
                    Dato = new Dato
                    {
                        IdDato = Converter.GetInt(row, "idDato"),
                        IdListaDato = Converter.GetInt(row, "idListaDato"),
                        IdTipo = Converter.GetInt(row, "idTipo"),
                        Prefijo = Converter.GetString(row, "prefijo"),
                        ListaDato = (int)Enums.TipoCampo.COMBO == Converter.GetInt(row, "idTipo") ?
                            listaDatoList.FirstOrDefault(l => l.IdListaDato == Converter.GetInt(row, "idListaDato")) :
                            null
                    }
                };

                categoriaDato?.OrdenDatoClinicoList.Add(ordenDatoClinico);
            }

            #endregion

            return orden;
        }

        /// <summary>
        /// Descripción: Metodo para obtener la informacion de los datos clinicos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nombrePaciente"></param>
        /// <param name="apellidoPaciente"></param>
        /// <param name="apellidoPaciente2"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="enfermedades"></param>
        /// <param name="nroOficio"></param>
        /// <param name="esTipoEstablecimiento"></param>
        /// <param name="examen"></param>
        /// <param name="tipoPaciente"></param>
        /// <param name="esTipoUbigueo"></param>
        /// <param name="actualDepartamento"></param>
        /// <param name="actualProvincia"></param>
        /// <param name="actualDistrito"></param>
        /// <param name="estadoResultado"></param>
        /// <param name="ordenarPor"></param>
        /// <param name="tipoOrdenacion"></param>
        /// <param name="EstablecimientoCadena"></param>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="hdnEstablecimiento"></param>
        /// <returns></returns>
        public DataTable DatosClinicos(int idUsuario, int esFechaRegistro, string fechaDesde, string fechaHasta,
            string nroDocumento, string nombrePaciente, string apellidoPaciente, string apellidoPaciente2, string codigoMuestra, string codigoOrden,
            string enfermedades, string nroOficio, int? esTipoEstablecimiento, string examen, int? tipoPaciente, int? esTipoUbigueo, string actualDepartamento,
            string actualProvincia, string actualDistrito, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion, string EstablecimientoCadena,
            string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed,
            string hdnEstablecimiento)
        {


            var objCommand = GetSqlCommand("pNLS_OrdenesConsultaExternaDatosClinicos");
            InputParameterAdd.Int(objCommand, "TipoFecha", esFechaRegistro);
            InputParameterAdd.Varchar(objCommand, "FechaDesde", fechaDesde);
            InputParameterAdd.Varchar(objCommand, "FechaHasta", fechaHasta);
            InputParameterAdd.Int(objCommand, "TipoPaciente", tipoPaciente);
            InputParameterAdd.Varchar(objCommand, "CodigoOrden", codigoOrden);
            InputParameterAdd.Varchar(objCommand, "CodigoMuestra", codigoMuestra);
            InputParameterAdd.Varchar(objCommand, "NroOficio", nroOficio);
            InputParameterAdd.Varchar(objCommand, "NombreEnfermedad", enfermedades);
            InputParameterAdd.Varchar(objCommand, "Examen", examen);
            InputParameterAdd.Varchar(objCommand, "NroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "NombrePaciente", nombrePaciente);
            InputParameterAdd.Varchar(objCommand, "ApepatPaciente", apellidoPaciente);
            InputParameterAdd.Varchar(objCommand, "ApematPaciente", apellidoPaciente2);
            InputParameterAdd.Int(objCommand, "TipoUbigeo", esTipoUbigueo);
            InputParameterAdd.Varchar(objCommand, "Departamento", actualDepartamento);
            InputParameterAdd.Varchar(objCommand, "Provincia", actualProvincia);
            InputParameterAdd.Varchar(objCommand, "Distrito", actualDistrito);
            InputParameterAdd.Int(objCommand, "TipoEstablecimiento", esTipoEstablecimiento);

            InputParameterAdd.Int(objCommand, "Institucion", int.Parse(hdnInstitucion));
            InputParameterAdd.Varchar(objCommand, "disa", hdnDisa);
            InputParameterAdd.Varchar(objCommand, "red", hdnRed);
            InputParameterAdd.Varchar(objCommand, "microred", hdnMicroRed);
            InputParameterAdd.Varchar(objCommand, "Establecimiento", hdnEstablecimiento);

            InputParameterAdd.Varchar(objCommand, "EESSnombre", EstablecimientoCadena);
            InputParameterAdd.Int(objCommand, "EstadoResultado", estadoResultado);
            InputParameterAdd.Int(objCommand, "OrdenarPor", ordenarPor);
            InputParameterAdd.Int(objCommand, "TipoOrdenacion", tipoOrdenacion);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);

            DataTable dataTable = Execute(objCommand);

            return dataTable;
        }
        public List<OrdenIngresarResultadoVd> GetDatos(int idUsuario, int esFechaRegistro, string fechaDesde, string fechaHasta,
            string nroDocumento, string nombrePaciente, string apellidoPaciente, string apellidoPaciente2, string codigoMuestra, string codigoOrden,
            string enfermedades, string nroOficio, int? esTipoEstablecimiento, string examen, int? tipoPaciente, int? esTipoUbigueo, string actualDepartamento,
            string actualProvincia, string actualDistrito, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion, string EstablecimientoCadena,
            string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, string hdnEstablecimiento, string Semaforo)
        {
            var listaOrdenes = new List<OrdenIngresarResultadoVd>();
            var objCommand = GetSqlCommand("pNLS_OrdenesConsultaExterna");

            InputParameterAdd.Int(objCommand, "TipoFecha", esFechaRegistro);
            InputParameterAdd.Varchar(objCommand, "FechaDesde", fechaDesde);
            InputParameterAdd.Varchar(objCommand, "FechaHasta", fechaHasta);
            InputParameterAdd.Int(objCommand, "TipoPaciente", tipoPaciente);
            InputParameterAdd.Varchar(objCommand, "CodigoOrden", codigoOrden);
            InputParameterAdd.Varchar(objCommand, "CodigoMuestra", codigoMuestra);
            InputParameterAdd.Varchar(objCommand, "NroOficio", nroOficio);
            InputParameterAdd.Varchar(objCommand, "NombreEnfermedad", enfermedades);
            InputParameterAdd.Varchar(objCommand, "Examen", examen);
            InputParameterAdd.Varchar(objCommand, "NroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "NombrePaciente", nombrePaciente);
            InputParameterAdd.Varchar(objCommand, "ApepatPaciente", apellidoPaciente);
            InputParameterAdd.Varchar(objCommand, "ApematPaciente", apellidoPaciente2);
            InputParameterAdd.Int(objCommand, "TipoUbigeo", esTipoUbigueo);
            InputParameterAdd.Varchar(objCommand, "Departamento", actualDepartamento);
            InputParameterAdd.Varchar(objCommand, "Provincia", actualProvincia);
            InputParameterAdd.Varchar(objCommand, "Distrito", actualDistrito);
            InputParameterAdd.Int(objCommand, "TipoEstablecimiento", esTipoEstablecimiento);

            InputParameterAdd.Int(objCommand, "Institucion", int.Parse(hdnInstitucion));
            InputParameterAdd.Varchar(objCommand, "disa", hdnDisa);
            InputParameterAdd.Varchar(objCommand, "red", hdnRed);
            InputParameterAdd.Varchar(objCommand, "microred", hdnMicroRed);
            InputParameterAdd.Varchar(objCommand, "Establecimiento", hdnEstablecimiento);

            InputParameterAdd.Varchar(objCommand, "EESSnombre", EstablecimientoCadena);
            InputParameterAdd.Int(objCommand, "EstadoResultado", estadoResultado);
            InputParameterAdd.Int(objCommand, "OrdenarPor", ordenarPor);
            InputParameterAdd.Int(objCommand, "TipoOrdenacion", tipoOrdenacion);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);

            InputParameterAdd.Varchar(objCommand, "Semaforo", Semaforo);

            DataTable dataTable = Execute(objCommand);
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
                using (SqlCommand comando = new SqlCommand("pNLS_OrdenesConsultaExterna", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@TipoFecha", System.Data.SqlDbType.Int)).Value = esFechaRegistro;
                    comando.Parameters.Add(new SqlParameter("@FechaDesde", System.Data.SqlDbType.VarChar)).Value = fechaDesde;
                    comando.Parameters.Add(new SqlParameter("@FechaHasta", System.Data.SqlDbType.VarChar)).Value = fechaHasta;
                    comando.Parameters.Add(new SqlParameter("@TipoPaciente", System.Data.SqlDbType.Int)).Value = tipoPaciente;
                    comando.Parameters.Add(new SqlParameter("@CodigoOrden", System.Data.SqlDbType.VarChar)).Value = codigoOrden;
                    comando.Parameters.Add(new SqlParameter("@CodigoMuestra", System.Data.SqlDbType.VarChar)).Value = codigoMuestra;
                    comando.Parameters.Add(new SqlParameter("@NroOficio", System.Data.SqlDbType.VarChar)).Value = nroOficio;
                    comando.Parameters.Add(new SqlParameter("@NombreEnfermedad", System.Data.SqlDbType.VarChar)).Value = enfermedades;
                    comando.Parameters.Add(new SqlParameter("@Examen", System.Data.SqlDbType.VarChar)).Value = examen;
                    comando.Parameters.Add(new SqlParameter("@NroDocumento", System.Data.SqlDbType.VarChar)).Value = nroDocumento;
                    comando.Parameters.Add(new SqlParameter("@NombrePaciente", System.Data.SqlDbType.VarChar)).Value = nombrePaciente;
                    comando.Parameters.Add(new SqlParameter("@ApepatPaciente", System.Data.SqlDbType.VarChar)).Value = apellidoPaciente;
                    comando.Parameters.Add(new SqlParameter("@ApematPaciente", System.Data.SqlDbType.VarChar)).Value = apellidoPaciente2;
                    comando.Parameters.Add(new SqlParameter("@TipoUbigeo", System.Data.SqlDbType.Int)).Value = esTipoUbigueo;
                    comando.Parameters.Add(new SqlParameter("@Departamento", System.Data.SqlDbType.VarChar)).Value = actualDepartamento;
                    comando.Parameters.Add(new SqlParameter("@Provincia", System.Data.SqlDbType.VarChar)).Value = actualProvincia;
                    comando.Parameters.Add(new SqlParameter("@Distrito", System.Data.SqlDbType.VarChar)).Value = actualDistrito;
                    comando.Parameters.Add(new SqlParameter("@TipoEstablecimiento", System.Data.SqlDbType.Int)).Value = esTipoEstablecimiento;
                    comando.Parameters.Add(new SqlParameter("@Institucion", System.Data.SqlDbType.Int)).Value = int.Parse(hdnInstitucion);
                    comando.Parameters.Add(new SqlParameter("@disa", System.Data.SqlDbType.VarChar)).Value = hdnDisa;
                    comando.Parameters.Add(new SqlParameter("@red", System.Data.SqlDbType.VarChar)).Value = hdnRed;
                    comando.Parameters.Add(new SqlParameter("@microred", System.Data.SqlDbType.VarChar)).Value = hdnMicroRed;
                    comando.Parameters.Add(new SqlParameter("@Establecimiento", System.Data.SqlDbType.VarChar)).Value = hdnEstablecimiento;
                    comando.Parameters.Add(new SqlParameter("@EESSnombre", System.Data.SqlDbType.VarChar)).Value = EstablecimientoCadena;
                    comando.Parameters.Add(new SqlParameter("@EstadoResultado", System.Data.SqlDbType.Int)).Value = estadoResultado;
                    comando.Parameters.Add(new SqlParameter("@OrdenarPor", System.Data.SqlDbType.Int)).Value = ordenarPor;
                    comando.Parameters.Add(new SqlParameter("@TipoOrdenacion", System.Data.SqlDbType.Int)).Value = tipoOrdenacion;
                    comando.Parameters.Add(new SqlParameter("@IdUsuario", System.Data.SqlDbType.VarChar)).Value = idUsuario;
                    comando.Parameters.Add(new SqlParameter("@Semaforo", System.Data.SqlDbType.VarChar)).Value = Semaforo;
                    try
                    {
                        var reader = comando.ExecuteReader();

                        while (reader.Read())
                        {
                            listaOrdenes.Add(null);
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
            return null;
        }

        static string GetIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }

        /// <summary>
        /// Descripción: Metodo para obtener información de consultas externas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="esFechaRegistro"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="nombrePaciente"></param>
        /// <param name="apellidoPaciente"></param>
        /// <param name="apellidoPaciente2"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="enfermedades"></param>
        /// <param name="nroOficio"></param>
        /// <param name="esTipoEstablecimiento"></param>
        /// <param name="examen"></param>
        /// <param name="tipoPaciente"></param>
        /// <param name="esTipoUbigueo"></param>
        /// <param name="actualDepartamento"></param>
        /// <param name="actualProvincia"></param>
        /// <param name="actualDistrito"></param>
        /// <param name="estadoResultado"></param>
        /// <param name="ordenarPor"></param>
        /// <param name="tipoOrdenacion"></param>
        /// <param name="EstablecimientoCadena"></param>
        /// <param name="hdnInstitucion"></param>
        /// <param name="hdnDisa"></param>
        /// <param name="hdnRed"></param>
        /// <param name="hdnMicroRed"></param>
        /// <param name="hdnEstablecimiento"></param>
        /// <returns></returns>
        ///
        public List<OrdenIngresarResultadoVd> GetOrdenesConsultaExterna(int idUsuario, int esFechaRegistro, string fechaDesde, string fechaHasta,
            string nroDocumento, string nombrePaciente, string apellidoPaciente, string apellidoPaciente2, string codigoMuestra, string codigoOrden,
            string enfermedades, string nroOficio, int? esTipoEstablecimiento, string examen, int? tipoPaciente, int? esTipoUbigueo, string actualDepartamento,
            string actualProvincia, string actualDistrito, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion, string EstablecimientoCadena,
            string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, string hdnEstablecimiento)
        {
            //IPHostEntry host;
            //string localIP = "";
            //host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily.ToString() == "InterNetwork")
            //    {
            //        localIP = ip.ToString();
            //    }
            //}
            //localIP = GetIPAddress();
            var listaOrdenes = new List<OrdenIngresarResultadoVd>();
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
                using (SqlCommand comando = new SqlCommand("pNLS_OrdenesConsultaExterna", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    //comando.BeginExecuteNonQuery();
                    comando.Parameters.Add(new SqlParameter("@TipoFecha", System.Data.SqlDbType.Int)).Value = esFechaRegistro;
                    comando.Parameters.Add(new SqlParameter("@FechaDesde", System.Data.SqlDbType.VarChar)).Value = fechaDesde;
                    comando.Parameters.Add(new SqlParameter("@FechaHasta", System.Data.SqlDbType.VarChar)).Value = fechaHasta;
                    comando.Parameters.Add(new SqlParameter("@TipoPaciente", System.Data.SqlDbType.Int)).Value = tipoPaciente;
                    comando.Parameters.Add(new SqlParameter("@CodigoOrden", System.Data.SqlDbType.VarChar)).Value = codigoOrden;
                    comando.Parameters.Add(new SqlParameter("@CodigoMuestra", System.Data.SqlDbType.VarChar)).Value = codigoMuestra;
                    comando.Parameters.Add(new SqlParameter("@NroOficio", System.Data.SqlDbType.VarChar)).Value = nroOficio;
                    comando.Parameters.Add(new SqlParameter("@NombreEnfermedad", System.Data.SqlDbType.VarChar)).Value = enfermedades;
                    comando.Parameters.Add(new SqlParameter("@Examen", System.Data.SqlDbType.VarChar)).Value = examen;
                    comando.Parameters.Add(new SqlParameter("@NroDocumento", System.Data.SqlDbType.VarChar)).Value = nroDocumento;
                    comando.Parameters.Add(new SqlParameter("@NombrePaciente", System.Data.SqlDbType.VarChar)).Value = nombrePaciente;
                    comando.Parameters.Add(new SqlParameter("@ApepatPaciente", System.Data.SqlDbType.VarChar)).Value = apellidoPaciente;
                    comando.Parameters.Add(new SqlParameter("@ApematPaciente", System.Data.SqlDbType.VarChar)).Value = apellidoPaciente2;
                    comando.Parameters.Add(new SqlParameter("@TipoUbigeo", System.Data.SqlDbType.Int)).Value = esTipoUbigueo;
                    comando.Parameters.Add(new SqlParameter("@Departamento", System.Data.SqlDbType.VarChar)).Value = actualDepartamento;
                    comando.Parameters.Add(new SqlParameter("@Provincia", System.Data.SqlDbType.VarChar)).Value = actualProvincia;
                    comando.Parameters.Add(new SqlParameter("@Distrito", System.Data.SqlDbType.VarChar)).Value = actualDistrito;
                    comando.Parameters.Add(new SqlParameter("@TipoEstablecimiento", System.Data.SqlDbType.Int)).Value = esTipoEstablecimiento;
                    comando.Parameters.Add(new SqlParameter("@Institucion", System.Data.SqlDbType.Int)).Value = int.Parse(hdnInstitucion);
                    comando.Parameters.Add(new SqlParameter("@disa", System.Data.SqlDbType.VarChar)).Value = hdnDisa;
                    comando.Parameters.Add(new SqlParameter("@red", System.Data.SqlDbType.VarChar)).Value = hdnRed;
                    comando.Parameters.Add(new SqlParameter("@microred", System.Data.SqlDbType.VarChar)).Value = hdnMicroRed;
                    comando.Parameters.Add(new SqlParameter("@Establecimiento", System.Data.SqlDbType.VarChar)).Value = hdnEstablecimiento;
                    comando.Parameters.Add(new SqlParameter("@EESSnombre", System.Data.SqlDbType.VarChar)).Value = EstablecimientoCadena;
                    comando.Parameters.Add(new SqlParameter("@EstadoResultado", System.Data.SqlDbType.Int)).Value = estadoResultado;
                    comando.Parameters.Add(new SqlParameter("@OrdenarPor", System.Data.SqlDbType.Int)).Value = ordenarPor;
                    comando.Parameters.Add(new SqlParameter("@TipoOrdenacion", System.Data.SqlDbType.Int)).Value = tipoOrdenacion;
                    comando.Parameters.Add(new SqlParameter("@IdUsuario", System.Data.SqlDbType.VarChar)).Value = idUsuario;
                    //comando.Parameters.Add(new SqlParameter("@localIP", System.Data.SqlDbType.VarChar)).Value = localIP;
                    comando.CommandTimeout = 0;
                    try
                    {
                        var reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new OrdenIngresarResultadoVd();
                                obj.ConFechaSolicitud = reader["FechaSolicitud"].ToString();
                                obj.ConcodigoOrden = reader["codigoOrden"].ToString();
                                obj.ConnroOficio = reader["nroOficio"].ToString();
                                obj.ConEstablecimientoSolicitante = reader["EstablecimientoSolicitante"].ToString();
                                obj.ConEstablecimientoLatitud = reader["Latitud"].ToString();
                                obj.ConEstablecimientoLongitud = reader["Longitud"].ToString();
                                obj.ConEESS_LAB_Destino = reader["EESS_LAB_Destino"].ToString();
                                obj.tipoDocumento = reader["tipoDocumento"].ToString();
                                obj.ConDocIdentidad = reader["DocIdentidad"].ToString();
                                obj.ConfechaNacimiento = reader["fechaNacimiento"].ToString();
                                obj.ConnombrePaciente = reader["nombrePaciente"].ToString();
                                obj.ConID_Muestra = reader["ID_Muestra"].ToString();
                                obj.ConTipoMuestra = reader["TipoMuestra"].ToString();
                                obj.ConEnfermedad = reader["Enfermedad"].ToString();
                                obj.ConnombreExamen = reader["nombreExamen"].ToString();
                                obj.ConnResultado = reader["convResultado"] == DBNull.Value ? "" : reader["convResultado"].ToString();
                                obj.ConFechaHoraColeccion = reader["FechaColeccion"] == DBNull.Value ? "" : reader["FechaColeccion"].ToString().Substring(0, 10);
                                obj.ConHoraColeccion = reader["HoraColeccion"] == DBNull.Value ? "" : reader["HoraColeccion"].ToString().Substring(0, 5);
                                //obj.ConFechaHoraRecepcionInsROM = reader["FechaRecepcionInsRom"] == DBNull.Value ? "" : reader["FechaRecepcionInsRom"].ToString().Substring(0, 10);
                                obj.ConFechaHoraRecepcionROM = reader["FechaRecepcionROM"] == DBNull.Value ? "" : reader["FechaRecepcionROM"].ToString().Substring(0, 10);
                                //obj.ConHoraRecepcionROM = reader["HoraRecepcionROM"] == DBNull.Value ? "" : reader["HoraRecepcionROM"].ToString().Substring(0, 5);
                                obj.ConFechaHoraRecepcionLAB = reader["FechaRecepcionLAB"] == DBNull.Value ? "" : reader["FechaRecepcionLAB"].ToString().Substring(0, 10);
                                obj.ConHoraRecepcionLAB = reader["HoraRecepcionLAB"] == DBNull.Value ? "" : reader["HoraRecepcionLAB"].ToString().Substring(0, 5);
                                obj.ConMuestraConforme = reader["MuestraConforme"] == DBNull.Value ? "" : reader["MuestraConforme"].ToString();
                                obj.criteriosRechazo = reader["CriteriosRechazo"].ToString();
                                obj.ObservacionRechazo = reader["ObservacionRechazo"].ToString();
                                //if (reader["CriteriosRechazo"] != DBNull.Value && reader["CriteriosRechazoRom"] == DBNull.Value)
                                //{
                                //    obj.criteriosRechazo = reader["CriteriosRechazo"].ToString();
                                //}
                                //else if (reader["CriteriosRechazoRom"] != DBNull.Value && reader["CriteriosRechazo"] == DBNull.Value)
                                //{
                                //    obj.criteriosRechazo = reader["CriteriosRechazoRom"].ToString();
                                //}else
                                //{
                                //    obj.criteriosRechazo = "";
                                //}
                                obj.ConFechaHoravalidado = reader["FechaValidado"] == DBNull.Value ? "" : reader["FechaValidado"].ToString().Substring(0, 10);
                                obj.ConHoravalidado = reader["HoraValidado"] == DBNull.Value ? "" : reader["HoraValidado"].ToString().Substring(0, 5);
                                obj.fechaRegistroOrden = reader["fechaRegistroOrden"] == DBNull.Value ? "" : reader["fechaRegistroOrden"].ToString().Substring(0, 10);
                                obj.HoraRegistroOrden = reader["HoraRegistroOrden"] == DBNull.Value ? "" : reader["HoraRegistroOrden"].ToString().Substring(0,5);
                                obj.fechaRegistroRecepcionMuestra = reader["fechaRegistroRecepcionMuestra"] == DBNull.Value ? "" : reader["fechaRegistroRecepcionMuestra"].ToString().Substring(0, 10);
                                obj.HoraRegistroRecepcionMuestra = reader["HoraRegistroRecepcionMuestra"] == DBNull.Value ? "" : reader["HoraRegistroRecepcionMuestra"].ToString().Substring(0, 5);
                                obj.ConEstatusResultado = reader["EstatusResultado"] == DBNull.Value ? "" : reader["EstatusResultado"].ToString();
                                obj.ConEstatusE = reader["EstatusE"].ToString();
                                obj.idOrden = (Guid)reader["idorden"];
                                obj.idEstablecimiento = (int)reader["idEstablecimientoDestino"];
                                obj.idExamen = (Guid)reader["idOrdenExamen"];
                                obj.ConDepartamentoOrigen = reader["EESSDepOrigen"].ToString();
                                obj.ConProvinciaOrigen = reader["EESSProvOrigen"].ToString();
                                obj.ConDistritoOrigen = reader["EESSDistOrigen"].ToString();
                                obj.ConDisaOrigen = reader["EESSDisaOrigen"].ToString();
                                obj.ConRedOrigen = reader["EESSRedOrigen"].ToString();
                                obj.ConMicroRedOrigen = reader["EESSMicroRedOrigen"].ToString();
                                obj.ConDepartamentoDestino = reader["EESSDepDestino"].ToString();
                                obj.ConProvinciaDestino = reader["EESSProvDestino"].ToString();
                                obj.ConDistritoDestino = reader["EESSDistDestino"].ToString();
                                obj.ConDisaDestino = reader["EESSDisaDestino"].ToString();
                                obj.ConRedDestino = reader["EESSRedDestino"].ToString();
                                obj.ConMicroRedDestino = reader["EESSMicroRedDestino"].ToString();
                                //AGREGADO YRCA
                                obj.ConEdad = reader["EdadPaciente"].ToString();
                                obj.ConSexo = reader["SexoPaciente"].ToString();
                                obj.ConDireccionPaciente = reader["DireccionPaciente"].ToString();
                                obj.ConMotivo = reader["Motivo"].ToString();

                                obj.ConComponente = reader["Componente"] == DBNull.Value ? "" : reader["Componente"].ToString();
                                //obj.ConFechAddExamen = reader["FechaAgregarExamenLAB"] == DBNull.Value ? "" : reader["FechaAgregarExamenLAB"].ToString();
                                obj.Telefono = reader["celular1"].ToString();
                                listaOrdenes.Add(obj);
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
            return listaOrdenes;
        }

        #region HelpReader
        //public static string SafeGetString(this SqlDataReader reader, int colIndex)
        //{
        //    if (!reader.IsDBNull(colIndex))
        //    {
        //        return reader.GetString(colIndex);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public static string SafeGetString(this SqlDataReader reader, string colName)
        //{
        //    int colIndex = reader.GetOrdinal(colName);

        //    if (!reader.IsDBNull(colIndex))
        //    {
        //        return reader.GetString(colIndex);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public static int? SafeGetInt(this SqlDataReader reader, int colIndex)
        //{
        //    if (!reader.IsDBNull(colIndex))
        //    {
        //        return reader.GetInt32(colIndex);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public static int? SafeGetInt(this SqlDataReader reader, string colName)
        //{
        //    int colIndex = reader.GetOrdinal(colName);

        //    if (!reader.IsDBNull(colIndex))
        //    {
        //        return reader.GetInt32(colIndex);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion


        public string RetornaResultado(List<AnalitoOpcionResultado> ListaResultadoJson)
        {
            string ResultadoFinal = "";
            string GlosaPadre = "";
            var CodigoParent = "";
            var printPadre = 0;
            foreach (var opcion in ListaResultadoJson.OrderBy(a => a.IdAnalitoOpcionParent).ToList())
            {
                if (opcion.Tipo == 2 || opcion.Tipo == 5)
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
                                ResultadoFinal = ResultadoFinal + " " + ValOpcion2.GlosaParent + " : " + ValOpcion2.Glosa + " - ";
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
                                        foreach (var ValOpcion5 in ListaResultadoJson.Where(p => p.IdAnalitoOpcionParent.ToString() == ValOpcion4.IdAnalitoOpcionResultado.ToString()).ToList())
                                        {
                                            if (!String.IsNullOrEmpty(ValOpcion5.Glosa))
                                            {
                                                ResultadoFinal = ResultadoFinal + " " + ValOpcion5.Glosa + " - ";
                                            }
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
        /// Autor: Juan Muga
        /// Descripcion: Registro/Modificacion de Nro de Oficio
        /// Fecha: 26/09/2017
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="nroOficio"></param>
        public void UpdateNumeroOficio(Orden _orden)
        {
            try
            {
                var objCommand = GetSqlCommand("pNLU_OrdenNroOficio");
                InputParameterAdd.Guid(objCommand, "idOrden", _orden.idOrden);
                InputParameterAdd.Varchar(objCommand, "nroOficio", _orden.nroOficio);
                InputParameterAdd.Int(objCommand, "idestablecimientoEnvio", _orden.idEstablecimientoEnvio);
                InputParameterAdd.DateTime(objCommand, "fechaIngresoRom", _orden.fechaIngresoINS);
                InputParameterAdd.DateTime(objCommand, "fechaReevaluacionFicha", _orden.fechaReevaluacionFicha);
                ExecuteNonQuery(objCommand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Autor: Juan Muga
        /// Descripcion: Actualización de la orden para las Pruebas Rápidas
        /// Fecha: 20/11/2017
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        public string RecepcionEESSandRecepcionLab(Guid idOrden, int idUsuario)
        {
            var idOrdenExamen = Guid.Empty;
            try
            {
                var objCommand = GetSqlCommand("pNLP_ProcesoPruebaRapida");
                InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
                InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
                OutputParameterAdd.UniqueIdentifier(objCommand, "idordenExamen");
                ExecuteNonQuery(objCommand);
                idOrdenExamen = (Guid)objCommand.Parameters["@idordenExamen"].Value;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return idOrdenExamen.ToString();
        }
        ///<summary>
        ///Autor: SOTERO BUSTAMANTE
        ///dESCRIPCION: Consulta para nro de muestras
        ///Fecha: 11/11/2017
        ///</summary>
        ///<param name= "idPaciente"></param>
        ///<param name= "idEnfermedad"></param>
        ///<param name= "idTipoMuestra"></param>
        ///<param name= "idExamen"></param>

        public int GetNumeroExamenByMuestra(Guid idPaciente, int idEnfermedad, int idTipoMuestra, Guid idExamen)
        {
            var nroMuestra = 0;
            try
            {
                var objCommand = GetSqlCommand("pNLS_GetNumeroExamenByMuestra");
                InputParameterAdd.Guid(objCommand, "idPaciente", idPaciente);
                InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
                InputParameterAdd.Int(objCommand, "idTipoMuestra", idTipoMuestra);
                InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
                OutputParameterAdd.Int(objCommand, "nroMuestra");
                ExecuteNonQuery(objCommand);
                nroMuestra = (int)objCommand.Parameters["@nroMuestra"].Value;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return nroMuestra;
        }
        /// <summary>
        /// Descripción: Metodo para obtener información de consultas de resultados de un paciente.
        /// Autor: Marcos Mejia.
        /// Fecha Creacion: 15/07/2018
        public List<OrdenIngresarResultadoVd> ConsultaReporteResultado(string fechaDesde, string fechaHasta, string nroDocumento, string codigoOrden, int idUsuario)
        {
            List<OrdenIngresarResultadoVd> listaOrdenes = new List<OrdenIngresarResultadoVd>();


            var objCommand = GetSqlCommand("pNLS_ReporteConsultaResultado");
            InputParameterAdd.Varchar(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.Varchar(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "codigoOrden", codigoOrden);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            DataTable dataTable = Execute(objCommand);

            if (dataTable.Rows.Count == 0)
                return listaOrdenes;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                OrdenIngresarResultadoVd item = new OrdenIngresarResultadoVd();
                item.ConcodigoOrden = row["codigoOrden"].ToString();
                item.nombreEstablecimiento = row["EESS_Origen"].ToString();
                item.tipoDocumento = row["tipoDocumento"].ToString();
                item.ConnombrePaciente = row["Paciente"].ToString();
                item.ConFechaHoraColeccion = row["fechaColeccion"].ToString();
                item.ConEstablecimientoSolicitante = row["Solicitante"].ToString();
                item.nombreExamen = row["Examen"].ToString();
                Guid idOrdenIn = new Guid(row["idOrden"].ToString());
                item.idOrden = idOrdenIn;
                item.idEstablecimiento = int.Parse(row["idEstablecimientoDestino"].ToString());
                Guid idExamenIn = new Guid(row["idOrdenExamen"].ToString());
                item.idExamen = idExamenIn;
                listaOrdenes.Add(item);
            }
            return listaOrdenes;
        }

        /// <summary>
        /// Descripción: Metodo para obtener información de resultados de un codigo de orden para el envío a otros sistemas.
        /// Autor: Juan Muga.
        /// Fecha Creacion: 10/09/2018
        /// </summary>
        /// <param name="codigoOrden"></param>
        /// <returns></returns>
        public List<Resultado> GetTramaResultadobyCodigoOrden(string codigoOrden)
        {
            List<Resultado> LstDato = new List<Resultado>();
            var objCommand = GetSqlCommand("pNLS_TramaResultadobyCodigoOrden");
            InputParameterAdd.Varchar(objCommand, "idOrden", codigoOrden);
            DataTable dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
            {
                return LstDato;
            }
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var dato = new Resultado();
                var row = dataTable.Rows[i];
                dato.num_solicitud = row["num_solicitud"].ToString();
                dato.num_orden = row["num_orden"].ToString();
                dato.cod_enfermedad = int.Parse(row["idenfermedad"].ToString());
                dato.cod_tip_muestra = int.Parse(row["idtipomuestra"].ToString());
                dato.est_muestra = row["estado_muestra"].ToString();
                dato.cod_muestra = row["cod_muestra"].ToString();
                dato.est_orden = row["est_orden"].ToString();
                dato.cod_tip_examen = row["cpt"].ToString();
                //dato.cod_examen = row["loinc"].ToString();
                dato.est_examen = row["estado_examen"].ToString();
                dato.des_rechazo_muestra = row["desc_rechazo_muestra"].ToString();
                dato.des_rechazo_examen = row["desc_rechazo_examen"].ToString();
                dato.des_resultado = row["Resultado"].ToString();
                dato.fec_resultados = row["fec_resultado"].ToString();
                dato.fec_rechazo_examen = row["fecha_rechazo_examen"].ToString();
                dato.fec_rechazo_muestra = row["fecha_rechazo_muestra"].ToString();
                LstDato.Add(dato);
            }
            return LstDato;
        }
        public int UpdateTRamaResultadobyCodigoOrden(string codigoOrden)
        {
            try
            {
                var objCommand = GetSqlCommand("pNLU_TramaResultadobyCodigoOrden");
                InputParameterAdd.Varchar(objCommand, "idOrden", codigoOrden);
                OutputParameterAdd.Int(objCommand, "rowcount");
                ExecuteNonQuery(objCommand);
                return (int)objCommand.Parameters["@rowcount"].Value;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public DataTable GetPruebaDatosClinicos(int idUsuario, int esFechaRegistro, string fechaDesde, string fechaHasta,
            string nroDocumento, string nombrePaciente, string apellidoPaciente, string apellidoPaciente2, string codigoMuestra, string codigoOrden,
            string enfermedades, string nroOficio, int? esTipoEstablecimiento, string examen, int? tipoPaciente, int? esTipoUbigueo, string actualDepartamento,
            string actualProvincia, string actualDistrito, int? estadoResultado, int? ordenarPor, int? tipoOrdenacion, string EstablecimientoCadena,
            string hdnInstitucion, string hdnDisa, string hdnRed, string hdnMicroRed, string hdnEstablecimiento)
        {
            //IPHostEntry host;
            //string localIP = "";
            //host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily.ToString() == "InterNetwork")
            //    {
            //        localIP = ip.ToString();
            //    }
            //}

            //localIP = GetIPAddress();

            var objCommand = GetSqlCommand("pNLS_OrdenConsultaDatoClinico");
            InputParameterAdd.Int(objCommand, "TipoFecha", esFechaRegistro);
            InputParameterAdd.Varchar(objCommand, "FechaDesde", fechaDesde);
            InputParameterAdd.Varchar(objCommand, "FechaHasta", fechaHasta);
            InputParameterAdd.Int(objCommand, "TipoPaciente", tipoPaciente);
            InputParameterAdd.Varchar(objCommand, "CodigoOrden", codigoOrden);
            InputParameterAdd.Varchar(objCommand, "CodigoMuestra", codigoMuestra);
            InputParameterAdd.Varchar(objCommand, "NroOficio", nroOficio);
            InputParameterAdd.Varchar(objCommand, "NombreEnfermedad", enfermedades);
            InputParameterAdd.Varchar(objCommand, "Examen", examen);
            InputParameterAdd.Varchar(objCommand, "NroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "NombrePaciente", nombrePaciente);
            InputParameterAdd.Varchar(objCommand, "ApepatPaciente", apellidoPaciente);
            InputParameterAdd.Varchar(objCommand, "ApematPaciente", apellidoPaciente2);
            InputParameterAdd.Int(objCommand, "TipoUbigeo", esTipoUbigueo);
            InputParameterAdd.Varchar(objCommand, "Departamento", actualDepartamento);
            InputParameterAdd.Varchar(objCommand, "Provincia", actualProvincia);
            InputParameterAdd.Varchar(objCommand, "Distrito", actualDistrito);
            InputParameterAdd.Int(objCommand, "TipoEstablecimiento", esTipoEstablecimiento);

            InputParameterAdd.Int(objCommand, "Institucion", int.Parse(hdnInstitucion));
            InputParameterAdd.Varchar(objCommand, "disa", hdnDisa);
            InputParameterAdd.Varchar(objCommand, "red", hdnRed);
            InputParameterAdd.Varchar(objCommand, "microred", hdnMicroRed);
            InputParameterAdd.Varchar(objCommand, "Establecimiento", hdnEstablecimiento);

            InputParameterAdd.Varchar(objCommand, "EESSnombre", EstablecimientoCadena);
            InputParameterAdd.Int(objCommand, "EstadoResultado", estadoResultado);
            InputParameterAdd.Int(objCommand, "OrdenarPor", ordenarPor);
            InputParameterAdd.Int(objCommand, "TipoOrdenacion", tipoOrdenacion);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            //InputParameterAdd.Varchar(objCommand, "localIP", localIP);
            DataTable dataTable = Execute(objCommand);

            if (dataTable.Rows.Count == 0)
                return null;
            return dataTable;
        }


        /// Descripción: Permite realizar la busqueda de resultados según el Semáforo
        /// Author: Terceros.
        /// Fecha Creacion: 10/06/2019
        /// Modificación: Se agregaron comentarios.

        public List<OrdenIngresarResultadoVd> ConsultaSemaforoResultados(int tipoConsulta, int idEstablecimiento, string fechaDesde, string fechaHasta, string codigoOrden,
            string codigoMuestra, string nroOficio, int idEnfermedad, string idExamen, string nroDocumento, string nombre, string apellidoPaterno, string apellidoMaterno,
            int tipoOrden, int Semaforo)
        {
            var listaOrdenes = new List<OrdenIngresarResultadoVd>();
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
                using (SqlCommand comando = new SqlCommand("pNLS_ConsultaSemaforoResultados", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@tipoConsulta", System.Data.SqlDbType.Int)).Value = tipoConsulta;
                    comando.Parameters.Add(new SqlParameter("@idEstablecimiento", System.Data.SqlDbType.VarChar)).Value = idEstablecimiento;
                    comando.Parameters.Add(new SqlParameter("@fechaDesde", System.Data.SqlDbType.VarChar)).Value = fechaDesde;
                    comando.Parameters.Add(new SqlParameter("@fechaHasta", System.Data.SqlDbType.VarChar)).Value = fechaHasta;
                    comando.Parameters.Add(new SqlParameter("@codigoOrden", System.Data.SqlDbType.VarChar)).Value = codigoOrden;
                    comando.Parameters.Add(new SqlParameter("@codigoMuestra", System.Data.SqlDbType.VarChar)).Value = codigoMuestra;
                    comando.Parameters.Add(new SqlParameter("@nroOficio", System.Data.SqlDbType.VarChar)).Value = nroOficio;
                    comando.Parameters.Add(new SqlParameter("@idEnfermedad", System.Data.SqlDbType.VarChar)).Value = idEnfermedad;
                    comando.Parameters.Add(new SqlParameter("@idExamen", System.Data.SqlDbType.VarChar)).Value = idExamen;
                    comando.Parameters.Add(new SqlParameter("@nroDni", System.Data.SqlDbType.VarChar)).Value = nroDocumento;
                    comando.Parameters.Add(new SqlParameter("@apellidoPaterno", System.Data.SqlDbType.VarChar)).Value = apellidoPaterno;
                    comando.Parameters.Add(new SqlParameter("@apellidoMaterno", System.Data.SqlDbType.VarChar)).Value = apellidoMaterno;
                    comando.Parameters.Add(new SqlParameter("@nombre", System.Data.SqlDbType.VarChar)).Value = nombre;
                    comando.Parameters.Add(new SqlParameter("@tipoOrden", System.Data.SqlDbType.Int)).Value = tipoOrden;
                    comando.Parameters.Add(new SqlParameter("@Semaforo", System.Data.SqlDbType.Int)).Value = Semaforo;
                    comando.CommandTimeout = 0;
                    try
                    {
                        var reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new OrdenIngresarResultadoVd();
                                obj.ConEstablecimientoSolicitante = reader["EstablecimientoOrigen"].ToString();
                                obj.ConnroOficio = reader["DocumentoReferencia"].ToString();
                                obj.ConDocIdentidad = reader["DocumentoIdentidad"].ToString();
                                obj.ConnombrePaciente = reader["Paciente"].ToString();
                                obj.ConfechaNacimiento = reader["FechaNacimiento"].ToString();
                                obj.ConcodigoOrden = reader["CodigoOrden"].ToString();
                                obj.ConID_Muestra = reader["CodigoMuestra"].ToString();
                                obj.ConTipoMuestra = reader["CodigoCultivo"].ToString();
                                obj.ConFechaHoraColeccion = reader["FechaHoraColeccion"] == DBNull.Value ? "" : reader["FechaHoraColeccion"].ToString();
                                obj.ConFechaHoraRecepcionROM = reader["FechaHoraRecepcionROM"] == DBNull.Value ? "" : reader["FechaHoraRecepcionROM"].ToString();
                                obj.ConFechaHoraRecepcionLAB = reader["FechaHoraRecepcionLAB"] == DBNull.Value ? "" : reader["FechaHoraRecepcionLAB"].ToString();
                                obj.ConFechaHoravalidado = reader["FechaHoraVerificacion"] == DBNull.Value ? "" : reader["FechaHoraVerificacion"].ToString();
                                obj.ConEnfermedad = reader["Enfermedad"].ToString();
                                obj.ConnombreExamen = reader["Examen"].ToString();
                                obj.dias = reader["Dias"].ToString();
                                obj.catalogo = reader["Catalogo"].ToString();
                                obj.ConColor = reader["Color"].ToString();
                                listaOrdenes.Add(obj);
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
            return listaOrdenes;
        }

        /// <summary>
        /// Descripción: 
        /// Autor: Juan Muga.
        /// Fecha Creacion: 10/01/2019
        /// </summary>
        /// <param name="estado"></param>
        /// <param name="codigoOrden"></param>
        public void UpdateOrdenTramaEnvio(int estado, string codigoOrden)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenTrama");
            InputParameterAdd.Varchar(objCommand, "CodigoOrden", codigoOrden);
            InputParameterAdd.Int(objCommand, "codestado", estado);
            ExecuteNonQuery(objCommand);
        }
        public void InsertOrdenPacienteRechazo(string idOrden, int idRechazo, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenPacienteRechazo");
            InputParameterAdd.Varchar(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idMotivoRechazo", idRechazo);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", idUsuario);
            ExecuteNonQuery(objCommand);
        }
        public Usuario UpdateOrdenPacienteRechazo(string idOrden, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenPacienteRechazo");
            InputParameterAdd.Varchar(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", idUsuario);
            DataTable dataTable = Execute(objCommand);

            if (dataTable.Rows.Count == 0)
                return null;
            Usuario user = null;
            foreach (DataRow item in dataTable.Rows)
            {
                user = new Usuario
                {
                    idUsuario = Converter.GetInt(item, "idUsuario"),
                    documentoIdentidad = Converter.GetString(item, "documentoIdentidad"),
                    apellidoPaterno = Converter.GetString(item, "apellidoPaterno"),
                    apellidoMaterno = Converter.GetString(item, "apellidoMaterno"),
                    nombres = Converter.GetString(item, "nombres"),
                    telefono = Converter.GetString(item, "telefonoContacto"),
                    correo = Converter.GetString(item, "correo")
                };
            }
            return user;
        }
        /// <summary>
        /// Descripción: Actualiza la orden con las fechas de ingreso a rom y de reevalucaion de fichas
        /// Autor: Juan Muga.
        /// Fecha Creacion: 16/10/2019
        /// </summary>
        /// <param name="orden"></param>
        public void UpdateOrdenbyNroOficio(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenbyNroOficio");
            InputParameterAdd.Guid(objCommand, "idOrden", orden.idOrden);
            InputParameterAdd.DateTime(objCommand, "fechaRecepcionRomINS", orden.fechaIngresoINS);
            InputParameterAdd.DateTime(objCommand, "fechaReevaluacionFicha", orden.fechaReevaluacionFicha);
            ExecuteNonQuery(objCommand);
        }

        public DataTable GetOrdenesConsultaResultados2(int tipoFecha, string fechaDesde, string fechaHasta, int idEnfermedad, int idExamenAgrupado, int estado)
        {
            var resultado = new List<OrdenIngresarResultadoVd>();
            var objCommand = GetSqlCommand("pNLS_OrdenConsultaResultado2");
            InputParameterAdd.Int(objCommand, "tipoFecha", tipoFecha);
            InputParameterAdd.Varchar(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.Varchar(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Int(objCommand, "idExamenAgrupado", idExamenAgrupado);
            InputParameterAdd.Int(objCommand, "Estado", estado);
            DataTable dataTable = Execute(objCommand);
            return dataTable;
        }
        //public List<string> GetOrdenesConsultaResultados()
        //{
        //    List<string> resultado = new List<string>();
        //    var objCommand = GetSqlCommand("pNLS_OrdenConsultaResultado");
        //    //InputParameterAdd.Int(objCommand, "tipoFecha", tipoFecha);
        //    //InputParameterAdd.Varchar(objCommand, "fechaDesde", fechaDesde);
        //    //InputParameterAdd.Varchar(objCommand, "fechaHasta", fechaHasta);
        //    //InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
        //    //InputParameterAdd.Int(objCommand, "idExamenAgrupado", idExamenAgrupado);
        //    //InputParameterAdd.Int(objCommand, "Estado", estado);
        //    var reader = objCommand.ExecuteReader();
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            resultado.Add(reader["Resultados"].ToString());
        //        }
        //    }
        //    return resultado;
        //}

        public string GetOrdenesConsultaResultados()
        {
            var objCommand = GetSqlCommand("pNLS_OrdenConsultaResultado");
            string resultado = objCommand.ExecuteScalar().ToString();
            return resultado;
        }

        public DataTable GetOrdenesConsultaResultadosDC(int tipoFecha, string fechaDesde, string fechaHasta, string esDatoClinico, int idEnfermedad, int idExamenAgrupado, int estado)
        {
            var objCommand = GetSqlCommand("pNLS_OrdenConsultaResultado");
            InputParameterAdd.Int(objCommand, "tipoFecha", tipoFecha);
            InputParameterAdd.Varchar(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.Varchar(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Int(objCommand, "idExamenAgrupado", idExamenAgrupado);
            InputParameterAdd.Int(objCommand, "Estado", estado);
            DataTable dataTable = Execute(objCommand);

            if (dataTable.Rows.Count == 0)
                return null;

            return dataTable;
        }

        public List<MuestrasODKCoronavirus> GetOrdenesConsultaExternaODK()
        {
            var listaOrdenes = new List<MuestrasODKCoronavirus>();
            var cnn = "";
            foreach (ConnectionStringSettings c in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (c.Name.StartsWith("ConnectionNetLabODK"))
                {
                    cnn = c.Name;
                }

            }
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[cnn].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("pNLS_ObtenerDatosODK", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.CommandTimeout = 0;
                    try
                    {
                        var reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new MuestrasODKCoronavirus();
                                obj.ident = reader["ident"].ToString();
                                obj.fec_notificacion = reader["fec_notificacion"].ToString();
                                obj.cod_renipres = reader["cod_renipres"].ToString();
                                obj.tipo_doc = reader["tipo_doc"].ToString();
                                obj.nro_documento = reader["nro_documento"].ToString();
                                obj.ape_nom_paciente = reader["nom_paciente"].ToString();
                                obj.ape_mat_paciente = reader["ape_mat_paciente"].ToString();
                                obj.ape_pat_paciente = reader["ape_pat_paciente"].ToString();
                                obj.fec_nac_pac = reader["fec_nac_pac"].ToString();
                                obj.Sexo_pac = reader["Sexo_pac"].ToString();
                                obj.celular_pac = reader["celular_pac"].ToString();
                                obj.email_pc = reader["email_pac"].ToString();
                                obj.direccion_o_residencia = reader["direccion_o_residencia"].ToString();
                                obj.direccion_pac = reader["direccion_pac"].ToString();
                                obj.cod_dept_pac = reader["cod_dept_pac"].ToString();
                                obj.cod_prov_pac = reader["cod_prov_pac"].ToString();
                                obj.cod_dist_pac = reader["cod_dist_pac"].ToString();
                                obj.ocupacion = reader["prof_salud"].ToString();

                                obj.prueba_rapida = reader["pru_rapida_res"].ToString();
                                obj.fec_prueba_rapida = reader["pru_rapida_fec"].ToString();
                                obj.fec_ini_sintomas = reader["fec_ini_sintomas"].ToString();//
                                obj.esta_hipostilizado = reader["esta_hispotilizado"].ToString();//
                                obj.fec_hospitalizacion = reader["fec_hispotilizado"].ToString();
                                obj.tiene_sintomas = reader["tiene_sintomas"].ToString();
                                obj.sintomas = reader["sintomas"].ToString();//Otros signos y síntomas
                                obj.dolor_sintomas = reader["dolor_sintomas"].ToString();
                                obj.otros_sintomas = reader["otros_sintomas"].ToString();//Factores de Riesgo - Otros 
                                obj.temperatura = reader["temperatura"].ToString();//temperatura
                                obj.signos = reader["signos"].ToString();
                                obj.otros_signos = reader["otros_signos"].ToString();
                                obj.comorbilidad = reader["comorbilidad"].ToString();
                                obj.ha_viajado = reader["ha_viajado"].ToString();
                                obj.viaje_1_pais = reader["viaje_1_pais"].ToString();
                                obj.viaje_2_pais = reader["viaje_2_pais"].ToString();
                                obj.viaje_3_pais = reader["viaje_3_pais"].ToString();
                                obj.viaje_1_ciudad = reader["viaje_1_ciudad"].ToString();
                                obj.viaje_2_ciudad = reader["viaje_2_ciudad"].ToString();
                                obj.viaje_3_ciudad = reader["viaje_3_ciudad"].ToString();
                                obj.tuvo_contacto = reader["tuvo_contacto"].ToString();

                                obj.fec_muestra = reader["fec_muestra"].ToString();
                                obj.tipo_muestra = reader["tipo_muestra"].ToString();

                                obj.tiene_dni = reader["tiene_dni"].ToString();
                                obj.dni_inv = reader["dni_inv"].ToString();
                                obj.ubicacion_latitud = reader["ubicacion_latitud"].ToString();
                                obj.ubicacion_longitud = reader["ubicacion_longitud"].ToString();

                                listaOrdenes.Add(obj);
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
            return listaOrdenes;
        }
        public Establecimiento GetEstablecimientobyCodigoUnico(string CodigoUnico)
        {
            var objCommand = GetSqlCommand("pNLS_GetEstablecimientobyCodigoUnico");
            InputParameterAdd.Varchar(objCommand, "CodigoUnico", CodigoUnico);
            DataTable dataTable = Execute(objCommand);
            var res = new Establecimiento();
            try
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    res = new Establecimiento()
                    {
                        IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento"),
                        CodigoUnico = Converter.GetString(row, "codigounico")
                    };
                }
            }
            catch (Exception)
            {
                return null;
            }

            return res;
        }
        public List<TramaData> GetTramaData()
        {
            var objCommand = GetSqlCommand("pNLS_TramaDato");

            DataTable dataTable = Execute(objCommand);
            List<TramaData> LstTramaData = new List<TramaData>();

            foreach (DataRow row in dataTable.Rows)
            {
                TramaData dato = new TramaData
                {
                    IdTabla = Converter.GetString(row, "IdTabla"),
                    IdTablaHija = Converter.GetString(row, "IdTablaHija"),
                    Campo1 = Converter.GetString(row, "Campo1"),
                    Campo2 = Converter.GetString(row, "Campo2"),
                    Campo3 = Converter.GetString(row, "Campo3"),
                    Campo4 = Converter.GetString(row, "Campo4"),
                    Campo5 = Converter.GetString(row, "Campo5"),
                    McaInh = Converter.GetInt(row, "McaInh"),
                };
                LstTramaData.Add(dato);
            }
            return LstTramaData;
        }

        public Orden ObtenerFicha(string idOrden)
        {
            Orden orden = null;
            var objCommand = GetSqlCommand("pNLS_ObtenerFicha");
            InputParameterAdd.Varchar(objCommand, "idOrden", idOrden);
            DataTable dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
                return null;
            foreach (DataRow item in dataTable.Rows)
            {
                orden = new Orden
                {
                    file = Converter.GetBytes(item, "Ficha")
                };
            }
            return orden;
        }

        public void InsertOrdenProcedenciaPaciente(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLI_InsertOrdenProcedenciaPaciente");
            InputParameterAdd.Varchar(objCommand, "data", orden.Procedencia);
            InputParameterAdd.Guid(objCommand, "idOrden", orden.idOrden);
            ExecuteNonQuery(objCommand);
        }

        public void InsertOrdenEjecutorPruebaRapida(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLI_InsertOrdenEjecutorPruebaRapida");
            InputParameterAdd.Guid(objCommand, "idOrden", orden.idOrden);
            InputParameterAdd.Varchar(objCommand, "dni", orden.dniEjecutor);
            InputParameterAdd.Varchar(objCommand, "ejecutor", orden.ejecutorPr);
            ExecuteNonQuery(objCommand);
        }

        public Orden ObtenerIdOrdenPorDocumentoPaciente(int tipoDocumento, string nroDocumento)
        {
            var orden = new Orden();
            string cnnstring = string.Empty;
            foreach (ConnectionStringSettings c in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (c.Name.StartsWith("DefaultConnection"))
                {
                    cnnstring = c.Name;
                }

            }

            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[cnnstring].ConnectionString))
            {
                using (var comando = new SqlCommand("pNLS_OrdenByNroDocumentoPaciente", cnn))
                {
                    try
                    {
                        cnn.Open();
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tipoDocumento", System.Data.SqlDbType.Int)).Value = tipoDocumento;
                        comando.Parameters.Add(new SqlParameter("@nroDocumento", System.Data.SqlDbType.VarChar)).Value = nroDocumento;
                        comando.CommandTimeout = 0;
                        DataTable reader = Execute(comando);
                        if (reader.Rows.Count == 0)
                        {
                            return orden;
                        }

                        foreach (DataRow i in reader.Rows)
                        {
                            orden.idOrden = Converter.GetGuid(i, "idOrden");
                            orden.idEstablecimiento = Converter.GetInt(i, "idEstablecimiento");
                            orden.IdUsuarioRegistro = Converter.GetInt(i, "IdUsuarioRegistro");
                            orden.estatus = Converter.GetInt(i, "estatus");
                            orden.ordenMaterialList = new List<OrdenMaterial> { new OrdenMaterial { Laboratorio = new Laboratorio { IdLaboratorio = Converter.GetInt(i, "idLaboratorio") } } };
                        }
                    }
                    catch (Exception ex)
                    {
                        cnn.Close();
                        cnn.Dispose();
                        throw ex;
                    }
                    finally
                    {
                        cnn.Close();
                        cnn.Dispose();
                    }
                }
            }

            return orden;
        }

        public void InsertOrdenResultadoAnalitoODK(Orden orden, Guid idOrdenExamen, int idAnalitoOpcionResultado)
        {
            var objCommand2 = GetSqlCommand("pNLI_OrdenResultadoAnalitoODK");
            InputParameterAdd.Guid(objCommand2, "IdOrden", orden.idOrden);
            InputParameterAdd.Guid(objCommand2, "IdOrdenExamen", idOrdenExamen);
            InputParameterAdd.Int(objCommand2, "IdAnalitoOpcionResultado", idAnalitoOpcionResultado);
            Execute(objCommand2);
        }

        public void InsertarOrdenEjecutorPruebaRapida(Guid idOrden, string ejecutordocumento, string ejecutornombres)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenEjecutorPruebasRapidas");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            InputParameterAdd.Varchar(objCommand, "EjecutorDocumento", ejecutordocumento);
            InputParameterAdd.Varchar(objCommand, "EjecutorNombres", ejecutornombres);
            Execute(objCommand);
        }

        public bool ValidarExisteOrdenExamen(Guid idOrden, Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_ValidarOrdenExamenPorOrden");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            InputParameterAdd.Guid(objCommand, "IdExamen", idExamen);
            DataTable dataTable = Execute(objCommand);
            //dataTable.Rows.Count > 0 entonces si existe OrdenExamen
            return dataTable.Rows.Count != 0;
        }

        public Guid InsertOrdenExamenODK(OrdenExamen ordenExamen)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenExamenODK");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", ordenExamen.idOrdenExamen);
            InputParameterAdd.Guid(objCommand, "idOrden", ordenExamen.idOrden);
            InputParameterAdd.Guid(objCommand, "idExamen", ordenExamen.Examen.idExamen);
            InputParameterAdd.Int(objCommand, "idEnfermedad", ordenExamen.Enfermedad.idEnfermedad);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", ordenExamen.IdTipoMuestra);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenExamen.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "estatus", ordenExamen.estatus);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenExamen");
            ExecuteNonQuery(objCommand);
            return (Guid)objCommand.Parameters["@newIdOrdenExamen"].Value;
        }

        public OrdenMuestra InsertOrdenMuestraODK(OrdenMuestra ordenMuestra, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMuestraODK");
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenMuestra.idOrdenMuestra);
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMuestra.idOrden);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", ordenMuestra.TipoMuestra.idTipoMuestra);
            InputParameterAdd.Int(objCommand, "idProyecto", ordenMuestra.idProyecto);
            InputParameterAdd.DateTime(objCommand, "fechaColeccion", ordenMuestra.fechaColeccion);
            InputParameterAdd.Varchar(objCommand, "horaColeccion", ordenMuestra.horaColeccion.ToString("HH':'mm"));
            InputParameterAdd.Int(objCommand, "numero", 1);
            InputParameterAdd.Int(objCommand, "seriado", 0);
            InputParameterAdd.Varchar(objCommand, "codificacion", ordenMuestra.MuestraCodificacion.codificacion);
            InputParameterAdd.Int(objCommand, "estado", 1);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMuestra.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "estatus", ordenMuestra.estatus);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdMuestraCod");
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenMuestra");
            OutputParameterAdd.Varchar(objCommand, "codificacionGenerada", 50);
            ExecuteNonQuery(objCommand);
            ordenMuestra.idOrdenMuestra = (Guid)objCommand.Parameters["@newIdOrdenMuestra"].Value;

            if (ordenMuestra.estatus > OrdenDal.ESTADO_BORRADOR)
            {
                ordenMuestra.MuestraCodificacion = new MuestraCodificacion();
                if (objCommand.Parameters["@newIdMuestraCod"].Value != DBNull.Value)
                {
                    ordenMuestra.idMuestraCod = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                }
                //ordenMuestra.idMuestraCod  = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                if (objCommand.Parameters["@codificacionGenerada"].Value != DBNull.Value)
                {
                    ordenMuestra.MuestraCodificacion.codificacion = (String)objCommand.Parameters["@codificacionGenerada"].Value;
                }
                //Juan Muga - fin
                //ordenMuestra.idMuestraCod = (Guid)objCommand.Parameters["@newIdMuestraCod"].Value;
                //ordenMuestra.MuestraCodificacion.codificacion = (String)objCommand.Parameters["@codificacionGenerada"].Value;
            }

            return ordenMuestra;
        }

        public void InsertOrdenMaterialODK(OrdenMaterial ordenMaterial, int estatusOrdenMuestraRecepcion, bool insertaOrdenMuestraRecepcion)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMaterialODK");
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMaterial.idOrden);
            InputParameterAdd.Int(objCommand, "idMaterial", ordenMaterial.Material.idMaterial);
            InputParameterAdd.Int(objCommand, "cantidad", ordenMaterial.cantidad);
            InputParameterAdd.Int(objCommand, "idLaboratorio", ordenMaterial.Laboratorio.IdLaboratorio);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMaterial.IdUsuarioRegistro);
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenMaterial.ordenMuestra.idOrdenMuestra);
            InputParameterAdd.Int(objCommand, "estatus", ordenMaterial.estatus);
            InputParameterAdd.Decimal(objCommand, "volumenMuestraColectada", ordenMaterial.volumenMuestraColectada);
            InputParameterAdd.Int(objCommand, "noPrecisaVolumen", ordenMaterial.noPrecisaVolumen);
            InputParameterAdd.DateTime(objCommand, "fechaEnvio", ordenMaterial.fechaEnvio);
            InputParameterAdd.Varchar(objCommand, "horaEnvio", ordenMaterial.horaEnvio.ToString("HH':'mm"));
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", ordenMaterial.OrdenExamen.idOrdenExamen);
            InputParameterAdd.Int(objCommand, "tipo", ordenMaterial.Tipo);
            InputParameterAdd.Int(objCommand, "estatusOrdenMuestraRecepcion", estatusOrdenMuestraRecepcion);
            InputParameterAdd.Int(objCommand, "insertarOrdenMuestraRecepcion", 1);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenMaterial");
            ExecuteNonQuery(objCommand);
            ordenMaterial.idOrdenMaterial = (Guid)objCommand.Parameters["@newIdOrdenMaterial"].Value;
        }
        public void InsertOrdenMaterialKOBO(OrdenMaterial ordenMaterial, int estatusOrdenMuestraRecepcion, bool insertaOrdenMuestraRecepcion)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMaterialkobo");
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMaterial.idOrden);
            InputParameterAdd.Int(objCommand, "idMaterial", ordenMaterial.Material.idMaterial);
            InputParameterAdd.Int(objCommand, "cantidad", ordenMaterial.cantidad);
            InputParameterAdd.Int(objCommand, "idLaboratorio", ordenMaterial.Laboratorio.IdLaboratorio);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMaterial.IdUsuarioRegistro);
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenMaterial.ordenMuestra.idOrdenMuestra);
            InputParameterAdd.Int(objCommand, "estatus", ordenMaterial.estatus);
            InputParameterAdd.Decimal(objCommand, "volumenMuestraColectada", ordenMaterial.volumenMuestraColectada);
            InputParameterAdd.Int(objCommand, "noPrecisaVolumen", ordenMaterial.noPrecisaVolumen);
            InputParameterAdd.DateTime(objCommand, "fechaEnvio", ordenMaterial.fechaEnvio);
            InputParameterAdd.Varchar(objCommand, "horaEnvio", ordenMaterial.horaEnvio.ToString("HH':'mm"));
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", ordenMaterial.OrdenExamen.idOrdenExamen);
            InputParameterAdd.Int(objCommand, "tipo", ordenMaterial.Tipo);
            InputParameterAdd.Int(objCommand, "estatusOrdenMuestraRecepcion", estatusOrdenMuestraRecepcion);
            InputParameterAdd.Int(objCommand, "insertarOrdenMuestraRecepcion", 1);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenMaterial");
            ExecuteNonQuery(objCommand);
            ordenMaterial.idOrdenMaterial = (Guid)objCommand.Parameters["@newIdOrdenMaterial"].Value;
        }
        public void InsertOrdenMaterialLaboratoriosDistintos(OrdenMaterial ordenMaterial, int estatusOrdenMuestraRecepcion, bool insertaOrdenMuestraRecepcion, int laborigenDiris)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMaterialLaboratoriosDistintos");
            InputParameterAdd.Guid(objCommand, "idOrden", ordenMaterial.idOrden);
            InputParameterAdd.Int(objCommand, "idMaterial", ordenMaterial.Material.idMaterial);
            InputParameterAdd.Int(objCommand, "cantidad", ordenMaterial.cantidad);
            InputParameterAdd.Int(objCommand, "idLaboratorioOrigen", ordenMaterial.Laboratorio.IdLaboratorio);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", ordenMaterial.IdUsuarioRegistro);
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", ordenMaterial.ordenMuestra.idOrdenMuestra);
            InputParameterAdd.Int(objCommand, "estatus", ordenMaterial.estatus);
            InputParameterAdd.Decimal(objCommand, "volumenMuestraColectada", ordenMaterial.volumenMuestraColectada);
            InputParameterAdd.Int(objCommand, "noPrecisaVolumen", ordenMaterial.noPrecisaVolumen);
            InputParameterAdd.DateTime(objCommand, "fechaEnvio", ordenMaterial.fechaEnvio);
            InputParameterAdd.Varchar(objCommand, "horaEnvio", ordenMaterial.horaEnvio.ToString("HH':'mm"));
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", ordenMaterial.OrdenExamen.idOrdenExamen);
            InputParameterAdd.Int(objCommand, "tipo", ordenMaterial.Tipo);
            InputParameterAdd.Int(objCommand, "estatusOrdenMuestraRecepcion", estatusOrdenMuestraRecepcion);
            InputParameterAdd.Int(objCommand, "insertarOrdenMuestraRecepcion", insertaOrdenMuestraRecepcion ? 1 : 0);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdOrdenMaterial");
            ExecuteNonQuery(objCommand);

        }

        public Orden GetTempOrdenByDNIPacienteYUsuario(string pacienteDNI, int idUsuarioRegistro)
        {
            List<Orden> ordenList = new List<Orden>();

            var objCommand = GetSqlCommand("pNLS_TempOrdenByIdPacienteYUsuario");
            InputParameterAdd.Varchar(objCommand, "pacienteDNI", pacienteDNI);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", idUsuarioRegistro);
            DataSet dataSet = ExecuteDataSet(objCommand);

            DataTable ordenDataTable = dataSet.Tables[0];
            DataTable ordenExamenDataTable = dataSet.Tables[1];
            DataTable ordenMuestraDataTable = dataSet.Tables[2];
            DataTable enfermedadDataTable = dataSet.Tables[3];
            DataTable opcionDataTable = dataSet.Tables[4];
            DataTable ordenMuestraRecepcionarTable = dataSet.Tables[5]; //Juan Muga

            #region Orden
            Orden orden = null;
            foreach (DataRow row in ordenDataTable.Rows)
            {
                orden = new Orden();

                orden.codigoOrden = Converter.GetString(row, "codigoOrden");

                orden.Paciente = new Paciente();
                orden.Paciente.NroDocumento = Converter.GetString(row, "paciente_nrodocumento");
                orden.Paciente.Nombres = Converter.GetString(row, "paciente_nombres");
                orden.Paciente.ApellidoMaterno = Converter.GetString(row, "paciente_apellidoMaterno");
                orden.Paciente.ApellidoPaterno = Converter.GetString(row, "paciente_apellidoPaterno");
                orden.Paciente.UbigeoActual = new Ubigeo();
                orden.Paciente.UbigeoActual.Id = Converter.GetString(row, "idUbigeoActual");
                orden.Paciente.UbigeoActual.Departamento = Converter.GetString(row, "paciente_ubigeoactual_departamento");
                orden.Paciente.UbigeoActual.Provincia = Converter.GetString(row, "paciente_ubigeoactual_provincia");
                orden.Paciente.UbigeoActual.Distrito = Converter.GetString(row, "paciente_ubigeoactual_distrito");
                orden.Paciente.DireccionActual = Converter.GetString(row, "paciente_direccionactual");
                orden.Paciente.edadAnios = Converter.GetInt(row, "Edad");
                orden.Paciente.TelefonoFijo = Converter.GetString(row, "telefonoFijo");
                orden.Paciente.Celular1 = Converter.GetString(row, "celular1");
                orden.Paciente.Celular2 = Converter.GetString(row, "celular2");
                orden.Paciente.correoElectronico = Converter.GetString(row, "correoElectronico");
                orden.Paciente.ocupacion = Converter.GetString(row, "ocupacion");
                orden.Paciente.tipoSeguroSalud = Converter.GetInt(row, "tipoSeguroSalud");
                orden.Paciente.otroSeguroSalud = Converter.GetString(row, "otroSeguroSalud");
                orden.Paciente.tipoSeguro = Converter.GetString(row, "tipoSeguro");
                orden.usuario = new Usuario();
                orden.usuario.nombres = Converter.GetString(row, "UsuarioRegistro");


                //orden.idProyecto
                orden.Proyecto = new Proyecto();
                orden.Proyecto.Nombre = Converter.GetString(row, "proyecto_nombre");
                orden.Proyecto.IdProyecto = Converter.GetInt(row, "proyecto_idProyecto");
                //orden.idEstablecimiento
                orden.establecimiento = new Establecimiento();
                orden.establecimiento.Nombre = Converter.GetString(row, "establecimiento_nombre");
                orden.establecimiento.IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento");
                orden.idEstablecimiento = orden.establecimiento.IdEstablecimiento;

                orden.establecimientoEnvio = new Establecimiento();
                orden.establecimientoEnvio.Nombre = Converter.GetString(row, "establecimientoEnvio_nombre");
                orden.establecimientoEnvio.IdEstablecimiento = Converter.GetInt(row, "idEstablecimientoEnvio");
                orden.idEstablecimientoEnvio = orden.establecimiento.IdEstablecimiento;


                orden.nroOficio = Converter.GetString(row, "nroOficio");
                orden.observacion = Converter.GetString(row, "observacion");
                orden.fechaSolicitud = Converter.GetDateTime(row, "fechaSolicitud");
                orden.FechaRegistro = Converter.GetDateTime(row, "fechaRegistro");
                orden.solicitante = Converter.GetString(row, "solicitante");
                orden.estatus = Converter.GetInt(row, "estatus");
                orden.fechaIngresoINS = Converter.GetDateTimeNullable(row, "fechaRecepcionRomINS");
                orden.fechaReevaluacionFicha = Converter.GetDateTimeNullable(row, "fechaReevaluacionFicha");
                orden.idOrden = Converter.GetGuid(row, "idOrden");
            }

            #endregion
            if (orden != null)
            {
                #region OrdenExamen
                orden.ordenExamenList = new List<OrdenExamen>();
                foreach (DataRow row in ordenExamenDataTable.Rows)
                {
                    OrdenExamen ordenExamen = new OrdenExamen();
                    ordenExamen.idOrdenExamen = Converter.GetGuid(row, "idOrdenExamen");
                    ordenExamen.Enfermedad = new Enfermedad();
                    ordenExamen.Enfermedad.idEnfermedad = Converter.GetInt(row, "enfermedad_idEnfermedad");
                    ordenExamen.Enfermedad.nombre = Converter.GetString(row, "enfermedad_nombre");
                    ordenExamen.Enfermedad.Snomed = Converter.GetString(row, "snomed");
                    ordenExamen.Examen = new Examen();
                    ordenExamen.Examen.nombre = Converter.GetString(row, "examen_nombre");
                    ordenExamen.Examen.idExamen = Converter.GetGuid(row, "examen_idExamen");
                    ordenExamen.Examen.PruebaRapida = Converter.GetInt(row, "iPruebaRapida");
                    ordenExamen.estatusE = Converter.GetInt(row, "estatusE");
                    ordenExamen.ordenMuestraList = new List<OrdenMuestra>();
                    orden.ordenExamenList.Add(ordenExamen);
                }

                #endregion

                #region OrdenMuestra
                orden.ordenMuestraList = new List<OrdenMuestra>();
                foreach (DataRow row in ordenMuestraDataTable.Rows)
                {
                    OrdenMuestra ordenMuestra = new OrdenMuestra();
                    ordenMuestra.idOrdenMuestra = Converter.GetGuid(row, "idOrdenMuestra");
                    ordenMuestra.MuestraCodificacion = new MuestraCodificacion();
                    ordenMuestra.MuestraCodificacion.codificacion = Converter.GetString(row, "muestracodificacion_codificacion");
                    ordenMuestra.TipoMuestra = new TipoMuestra();
                    ordenMuestra.TipoMuestra.nombre = Converter.GetString(row, "tipomuestra_nombre");
                    ordenMuestra.TipoMuestra.idTipoMuestra = Converter.GetInt(row, "idTipoMuestra");
                    //     ordenMuestra.Laboratorio = new Laboratorio();
                    //ordenMuestra.Laboratorio.Nombre = Converter.GetString(row, "laboratorio_nombre");
                    //ordenMuestra.Laboratorio.IdLaboratorio = Converter.GetInt(row, "laboratorio_idLaboratorio");
                    ordenMuestra.fechaColeccion = Converter.GetDateTime(row, "fechaColeccion");
                    string[] horaColeccion = Converter.GetString(row, "horaColeccion").Split(':');
                    ordenMuestra.horaColeccion = new DateTime(1, 1, 1, Convert.ToInt32(horaColeccion[0]), Convert.ToInt32(horaColeccion[1]), Convert.ToInt32(horaColeccion[2]));
                    //       ordenMuestra.fechaEnvio = Converter.GetDateTime(row, "fechaEnvio");
                    ordenMuestra.ordenExamenList = new List<OrdenExamen>();
                    orden.ordenMuestraList.Add(ordenMuestra);
                }

                #endregion

                //Juan Muga - inicio
                List<OrdenMuestraRecepcion> ordenMuestraRecepcionList = new List<OrdenMuestraRecepcion>();
                foreach (DataRow row in ordenMuestraRecepcionarTable.Rows)
                {
                    ordenMuestraRecepcionList.Add(new OrdenMuestraRecepcion
                    {
                        idOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion"),
                        idOrdenMaterial = Converter.GetGuid(row, "idOrdenMaterial"),
                        idOrden = Converter.GetGuid(row, "idOrdenMaterial"),
                        fechaRecepcion = Converter.GetDateTimeNullable(row, "fechaRecepcion"),
                        horaRecepcion = Converter.GetDateTimeNullable(row, "horaRecepcion"),
                        idLaboratorioOrigen = Converter.GetInt(row, "idLaboratorioOrigen"),
                        idUsuarioRegistro = Converter.GetInt(row, "idUsuarioRegistro"),
                        idLaboratorioDestino = Converter.GetInt(row, "idLaboratorioDestino"),
                        estatusR = Converter.GetInt(row, "estatusR"),
                        estatus = Converter.GetInt(row, "estatus"),
                        fechaEnvio = Converter.GetDateTimeNullable(row, "fechaEnvio"),
                        horaEnvio = Converter.GetDateTimeNullable(row, "horaEnvio"),
                        criterioRechazoList = new List<CriterioRechazo>()
                    });
                }
                //Juan Muga - Fin

                #region DatosClinicos

                int idListaDato = 0;
                List<ListaDato> listaDatoList = new List<ListaDato>();

                ListaDato listaDato = null;
                foreach (DataRow row in opcionDataTable.Rows)
                {
                    int idListaDatoNew = Converter.GetInt(row, "idListaDato");

                    if (idListaDato != idListaDatoNew)
                    {
                        idListaDato = idListaDatoNew;
                        listaDato = new ListaDato
                        {
                            IdListaDato = idListaDatoNew
                        };
                        listaDato.Opciones = new List<OpcionDato>();
                        listaDatoList.Add(listaDato);
                    }
                    OpcionDato opcionDato = new OpcionDato();
                    opcionDato.IdOpcionDato = Converter.GetInt(row, "IdOpcionDato");
                    opcionDato.Valor = Converter.GetString(row, "Valor");

                    listaDato.Opciones.Add(opcionDato);
                }

                orden.enfermedadList = new List<Enfermedad>();

                int idEnfermedad = 0;
                int idCategoriaDato = 0;
                Enfermedad enfermedad = null;
                CategoriaDato categoriaDato = null;
                foreach (DataRow row in enfermedadDataTable.Rows)
                {
                    //   List<Enfermedad> enfermedadList = new List<Enfermedad>();

                    int idEnfermedadNew = Converter.GetInt(row, "idEnfermedad");

                    //cambioj
                    if (idEnfermedadNew != idEnfermedad)
                    {
                        if (orden.enfermedadList.Any(x => x.idEnfermedad == idEnfermedadNew))
                        {
                            enfermedad = orden.enfermedadList.First(x => x.idEnfermedad == idEnfermedadNew);
                        }
                        else
                        {
                            idEnfermedad = idEnfermedadNew;
                            enfermedad = new Enfermedad();
                            enfermedad.idEnfermedad = idEnfermedadNew;
                            enfermedad.nombre = Converter.GetString(row, "nombreEnfermedad");
                            enfermedad.categoriaDatoList = new List<CategoriaDato>();
                            orden.enfermedadList.Add(enfermedad);
                        }
                    }

                    int idCategoriaDatoNew = Converter.GetInt(row, "idCategoriaDato");

                    //cambioj
                    if (idCategoriaDatoNew != idCategoriaDato)
                    {
                        if (enfermedad.categoriaDatoList.Any(x => x.IdCategoriaDato == idCategoriaDatoNew))
                        {
                            categoriaDato = enfermedad.categoriaDatoList.First(x => x.IdCategoriaDato == idCategoriaDatoNew);
                        }
                        else
                        {
                            idCategoriaDato = idCategoriaDatoNew;
                            categoriaDato = new CategoriaDato();

                            categoriaDato.IdCategoriaDato = idCategoriaDatoNew;
                            categoriaDato.Nombre = Converter.GetString(row, "nombreCategoriaDato");
                            categoriaDato.Orientacion = Converter.GetInt(row, "orientacion");
                            categoriaDato.OrdenDatoClinicoList = new List<OrdenDatoClinico>();
                            enfermedad.categoriaDatoList.Add(categoriaDato);
                        }
                    }


                    OrdenDatoClinico ordenDatoClinico = new OrdenDatoClinico();
                    ordenDatoClinico.noPrecisa = Converter.GetInt(row, "noPrecisa") == 1;
                    ordenDatoClinico.ValorString = Converter.GetString(row, "valor");
                    ordenDatoClinico.Enfermedad = enfermedad;
                    ordenDatoClinico.Dato = new Dato();
                    ordenDatoClinico.Dato.IdDato = Converter.GetInt(row, "idDato");
                    ordenDatoClinico.Dato.IdListaDato = Converter.GetInt(row, "idListaDato");
                    ordenDatoClinico.Dato.IdTipo = Converter.GetInt(row, "idTipo");
                    if ((int)Enums.TipoCampo.COMBO == ordenDatoClinico.Dato.IdTipo)
                    {
                        ordenDatoClinico.Dato.ListaDato = listaDatoList.Where(l => l.IdListaDato == ordenDatoClinico.Dato.IdListaDato).FirstOrDefault();
                    }

                    ordenDatoClinico.Dato.Prefijo = Converter.GetString(row, "prefijo");
                    categoriaDato.OrdenDatoClinicoList.Add(ordenDatoClinico);
                }

                #endregion
            }
            else
            {
                orden = new Orden();
            }

            return orden;
        }
        public string CorreccionCodigoOrden(string idOrden, int idEstablecimiento)
        {
            var newcodigoOrden = "";
            var objCommand = GetSqlCommand("pNLS_CorreccionCodigoOrden");
            InputParameterAdd.Varchar(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            DataTable dataTable = Execute(objCommand);
            //dataTable.Rows.Count > 0 entonces si existe OrdenExamen
            foreach (DataRow row in dataTable.Rows)
            {
                newcodigoOrden = Converter.GetString(row, "codigoOrden");
            }
            return newcodigoOrden;
        }

        public void EliminarRegistroOrden(Guid idorden)
        {
            var objCommand = GetSqlCommand("deleteOrden");
            InputParameterAdd.Guid(objCommand, "codigoOrden", idorden);
            DataTable dataTable = Execute(objCommand);
            
        }

        public string GenerarCodigoOrden(int idEstablecimiento)
        {
            string nuevocodigoorden = string.Empty;
            var objCommand = GetSqlCommand("GenerarCodigoOrdenPorEstablecimiento");
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            OutputParameterAdd.Varchar(objCommand, "nuevocodigoorden", 20);
            ExecuteNonQuery(objCommand);
            return (string)objCommand.Parameters["@nuevocodigoorden"].Value;
        }
        public void UpdateOrdenCovid(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenCovid");
            InputParameterAdd.Varchar(objCommand, "codigoOrden", orden.codigoOrden);
            InputParameterAdd.Guid(objCommand, "idOrden", orden.idOrden);
            InputParameterAdd.Varchar(objCommand, "fechaColeccion", orden.ordenMuestraList.First().fechaColeccionToString);
            InputParameterAdd.Int(objCommand, "idtipoMuestra", orden.ordenMuestraList.First().TipoMuestra.idTipoMuestra);
            InputParameterAdd.Guid(objCommand, "idMuestraCod", orden.ordenMuestraList.First().MuestraCodificacion.idMuestraCod);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", orden.idEstablecimiento);
            InputParameterAdd.Varchar(objCommand, "nroCelular", string.IsNullOrEmpty(orden.Paciente.Celular1) ? null: orden.Paciente.Celular1);
            InputParameterAdd.Guid(objCommand, "idPaciente", orden.Paciente.IdPaciente);
            InputParameterAdd.Varchar(objCommand, "editaEESS", orden.editarEstablecimiento);
            ExecuteNonQuery(objCommand);
        }
        public List<OrdenMuestraLinealkobo> ListOrdenMuestraLinealkobo(string dato)
        {
            var objCommand = GetSqlCommand("pNLS_GetOrdenExcel");
            InputParameterAdd.Varchar(objCommand, "dato", dato);
            DataTable dataTable = Execute(objCommand);
            List <OrdenMuestraLinealkobo> ListOrdenMuestraLinealkobo= new List<OrdenMuestraLinealkobo>();
            foreach (DataRow row in dataTable.Rows)
            {
                ListOrdenMuestraLinealkobo.Add(new OrdenMuestraLinealkobo
                {
                    codigoOrden = Converter.GetString(row, "codigoOrden"),
                    codigoMuestra = Converter.GetString(row, "codigoMuestra"),
                    codigoLineal = Converter.GetString(row, "codigoLineal"),
                    apepat = Converter.GetString(row, "apepat"),
                    apemat = Converter.GetString(row, "apemat"),
                    nombre = Converter.GetString(row, "nombre"),
                    dni = Converter.GetString(row, "dni")
                });
            }
            return ListOrdenMuestraLinealkobo;
        }

        public List<OrdenIngresarResultadoVd> ObtenerDatosNominalesCovidPorUsuario(int idUsuario, int esFechaRegistro, string fechaDesde, string fechaHasta, string codigoMuestra, string codigoOrden, string nroOficio)
        {
            //IPHostEntry host;
            //string localIP = "";
            //host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily.ToString() == "InterNetwork")
            //    {
            //        localIP = ip.ToString();
            //    }
            //}
            //localIP = GetIPAddress();
            var listaOrdenes = new List<OrdenIngresarResultadoVd>();
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
                using (SqlCommand comando = new SqlCommand("pNLS_ObtenerDatosNominalesCovid", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    //comando.BeginExecuteNonQuery();
                    comando.Parameters.Add(new SqlParameter("@TipoFecha", System.Data.SqlDbType.Int)).Value = esFechaRegistro;
                    comando.Parameters.Add(new SqlParameter("@FechaDesde", System.Data.SqlDbType.VarChar)).Value = fechaDesde;
                    comando.Parameters.Add(new SqlParameter("@FechaHasta", System.Data.SqlDbType.VarChar)).Value = fechaHasta;
                    comando.Parameters.Add(new SqlParameter("@CodigoOrden", System.Data.SqlDbType.VarChar)).Value = codigoOrden;
                    comando.Parameters.Add(new SqlParameter("@CodigoMuestra", System.Data.SqlDbType.VarChar)).Value = codigoMuestra;
                    comando.Parameters.Add(new SqlParameter("@NroOficio", System.Data.SqlDbType.VarChar)).Value = nroOficio;
                    comando.Parameters.Add(new SqlParameter("@IdUsuario", System.Data.SqlDbType.VarChar)).Value = idUsuario;
                    //comando.Parameters.Add(new SqlParameter("@localIP", System.Data.SqlDbType.VarChar)).Value = localIP;
                    comando.CommandTimeout = 0;
                    try
                    {
                        var reader = comando.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new OrdenIngresarResultadoVd();
                                obj.ConFechaSolicitud = reader["FechaSolicitud"].ToString();
                                obj.fechaRegistroOrden = reader["fechaRegistroOrden"].ToString();
                                obj.ConcodigoOrden = reader["codigoOrden"].ToString();
                                //obj.ConnroOficio = reader["nroOficio"].ToString();
                                obj.ConEstablecimientoSolicitante = reader["EstablecimientoSolicitante"].ToString();
                                //obj.ConEstablecimientoLatitud = reader["Latitud"].ToString();
                                //obj.ConEstablecimientoLongitud = reader["Longitud"].ToString();
                                obj.ConEESS_LAB_Destino = reader["EESS_LAB_Destino"].ToString();
                                obj.tipoDocumento = reader["tipoDocumento"].ToString();
                                //obj.ConDocIdentidad = reader["DocIdentidad"].ToString();
                                //obj.ConfechaNacimiento = reader["fechaNacimiento"].ToString();
                                //obj.ConnombrePaciente = reader["nombrePaciente"].ToString();
                                obj.ConID_Muestra = reader["ID_Muestra"].ToString();
                                obj.ConTipoMuestra = reader["TipoMuestra"].ToString();
                                obj.ConEnfermedad = reader["Enfermedad"].ToString();
                                obj.ConnombreExamen = reader["nombreExamen"].ToString();
                                obj.ConnResultado = reader["convResultado"] == DBNull.Value ? "" : reader["convResultado"].ToString();
                                obj.ConFechaHoraColeccion = reader["FechaHoraColeccion"] == DBNull.Value ? "" : reader["FechaHoraColeccion"].ToString();
                                obj.ConFechaHoraRecepcionInsROM = reader["fechaRecepcionRomINS"] == DBNull.Value ? "" : reader["fechaRecepcionRomINS"].ToString();
                                obj.ConFechaHoraRecepcionROM = reader["FechaHoraRecepcionROM"] == DBNull.Value ? "" : reader["FechaHoraRecepcionROM"].ToString();
                                obj.ConFechaHoraRecepcionLAB = reader["FechaHoraRecepcionLAB"] == DBNull.Value ? "" : reader["FechaHoraRecepcionLAB"].ToString();
                                obj.ConMuestraConforme = reader["MuestraConforme"] == DBNull.Value ? "" : reader["MuestraConforme"].ToString();
                                obj.criteriosRechazo = reader["CriteriosRechazo"].ToString();
                                obj.ObservacionRechazo = reader["ObservacionRechazo"].ToString();
                                obj.ConFechaHoravalidado = reader["FechaHoraValidado"] == DBNull.Value ? "" : reader["FechaHoraValidado"].ToString();
                                obj.ConEstatusResultado = reader["EstatusResultado"] == DBNull.Value ? "" : reader["EstatusResultado"].ToString();
                                obj.ConEstatusE = reader["EstatusE"].ToString();
                                obj.idOrden = (Guid)reader["idorden"];
                                obj.idEstablecimiento = (int)reader["idEstablecimientoDestino"];
                                obj.idExamen = (Guid)reader["idOrdenExamen"];
                                obj.ConDepartamentoOrigen = reader["EESSDepOrigen"].ToString();
                                obj.ConProvinciaOrigen = reader["EESSProvOrigen"].ToString();
                                obj.ConDistritoOrigen = reader["EESSDistOrigen"].ToString();
                                obj.ConDisaOrigen = reader["EESSDisaOrigen"].ToString();
                                obj.ConRedOrigen = reader["EESSRedOrigen"].ToString();
                                obj.ConMicroRedOrigen = reader["EESSMicroRedOrigen"].ToString();
                                obj.ConInstitucionOrigen = reader["nombreInstitucion"].ToString();
                                obj.ConDepartamentoDestino = reader["EESSDepDestino"].ToString();
                                obj.ConProvinciaDestino = reader["EESSProvDestino"].ToString();
                                obj.ConDistritoDestino = reader["EESSDistDestino"].ToString();
                                obj.ConDisaDestino = reader["EESSDisaDestino"].ToString();
                                obj.ConRedDestino = reader["EESSRedDestino"].ToString();
                                obj.ConMicroRedDestino = reader["EESSMicroRedDestino"].ToString();
                                obj.ConEdad = reader["EdadPaciente"].ToString();
                                obj.ConSexo = reader["SexoPaciente"].ToString();
                                //obj.ConDireccionPaciente = reader["DireccionPaciente"].ToString();
                                obj.ConMotivo = reader["Motivo"].ToString();

                                obj.ConComponente = reader["Componente"] == DBNull.Value ? "" : reader["Componente"].ToString();
                                //obj.Telefono = reader["celular1"].ToString();
                                listaOrdenes.Add(obj);
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
            return listaOrdenes;
        }

        public bool VerificarExisteExamenPorPaciente(Guid idPaciente, int idLaboratorio, Guid idExamen, DateTime fechaColeccion)
        {
            var objCommand = GetSqlCommand("pNLS_VerificarExisteExamenPorPaciente");
            InputParameterAdd.Guid(objCommand, "idpaciente", idPaciente);
            InputParameterAdd.Guid(objCommand, "idexamen", idExamen);
            InputParameterAdd.Int(objCommand, "idestablecimiento", idLaboratorio);
            InputParameterAdd.DateTime(objCommand, "fechacoleccion", fechaColeccion);
            DataTable dataTable = Execute(objCommand);
            
            //true = si existe examen para paciente.
            return dataTable.Rows.Count != 0;
        }

        public string ValidaRegistroExamenPaciente(Guid idPaciente, int idLaboratorio, Guid idExamen, DateTime fechaColeccion)
        {
            var objCommand = GetSqlCommand("pNLS_VerificarExisteExamenPorPaciente");
            InputParameterAdd.Guid(objCommand, "idpaciente", idPaciente);
            InputParameterAdd.Guid(objCommand, "idexamen", idExamen);
            InputParameterAdd.Int(objCommand, "idestablecimiento", idLaboratorio);
            InputParameterAdd.DateTime(objCommand, "fechacoleccion", fechaColeccion);
            string respuesta = objCommand.ExecuteScalar().ToString();
            return respuesta;
        }

        //public List<ResultadosINS> GetConsultaResultadosRecepcionINS(string idEnfermedad, string fechaDesde, string fechaHasta)
        //{
        //    var listaOrdenes = new List<ResultadosINS>();
        //    var cnn = "";
        //    foreach (ConnectionStringSettings c in System.Configuration.ConfigurationManager.ConnectionStrings)
        //    {
        //        if (c.Name.StartsWith("DefaultConnection"))
        //        {
        //            cnn = c.Name;
        //        }

        //    }
        //    using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[cnn].ConnectionString))
        //    {
        //        using (SqlCommand comando = new SqlCommand("pNLS_ConsultaResultadosRecepionINS", conexion))
        //        {
        //            conexion.Open();
        //            comando.CommandType = System.Data.CommandType.StoredProcedure;
        //            //comando.BeginExecuteNonQuery();
        //            comando.Parameters.Add(new SqlParameter("@idEnfermedad", System.Data.SqlDbType.VarChar)).Value = idEnfermedad;
        //            comando.Parameters.Add(new SqlParameter("@FechaDesde", System.Data.SqlDbType.VarChar)).Value = fechaDesde;
        //            comando.Parameters.Add(new SqlParameter("@FechaHasta", System.Data.SqlDbType.VarChar)).Value = fechaHasta;
        //            comando.CommandTimeout = 0;
        //            try
        //            {
        //                var reader = comando.ExecuteReader();
        //                if (reader.HasRows)
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var obj = new ResultadosINS();
        //                        obj.FechaObtencionMuestra = reader["FechaHoraColeccion"].ToString();
        //                        obj.FechaRecepcionINS = reader["FechaRecepcionROM"].ToString();
        //                        obj.CodigoOrden = reader["codigoOrden"].ToString();
        //                        obj.CodigoMuestra = reader["codificacion"].ToString();
        //                        obj.DepartamentoOrigen = reader["Departamento"].ToString();
        //                        obj.DisaOrigen = reader["EESSDisaOrigen"].ToString();
        //                        obj.EstablecimientoOrigen = reader["EESS_LAB_Origen"].ToString();
        //                        obj.EdadPaciente = (int)reader["edad"];
        //                        obj.SexoPaciente = reader["SexoPaciente"].ToString();
        //                        obj.Enfermedad = reader["Enfermedad"].ToString();
        //                        obj.Examen = reader["nombreExamen"].ToString();
        //                        obj.Resultado = reader["convResultado"].ToString();
        //                        obj.Estado = reader["EstatusResultado"].ToString();
        //                        listaOrdenes.Add(obj);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                conexion.Close();
        //                conexion.Dispose();
        //                throw ex;
        //            }
        //            finally
        //            {
        //                conexion.Close();
        //                conexion.Dispose();
        //            }
        //        }
        //    }
        //    return listaOrdenes;
        //}

        public DataTable GetConsultaResultadosRecepcionINS(string idEnfermedad, string fechaDesde, string fechaHasta)
        {
            var resultado = new List<OrdenIngresarResultadoVd>();
            var objCommand = GetSqlCommand("pNLS_ConsultaResultadosRecepionINS");
            InputParameterAdd.Varchar(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Varchar(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.Varchar(objCommand, "fechaHasta", fechaHasta);
            DataTable dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            return dataTable;
        }
    }
}
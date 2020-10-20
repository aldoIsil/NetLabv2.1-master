using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;
using Model;
using Model.ViewData;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using Utilitario;

namespace DataLayer
{
    public class OrdenMuestraDal : DaoBase
    {

        public OrdenMuestraDal(IDalSettings settings) : base(settings)
        {
        }

        public OrdenMuestraDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Metodo para obtener Muestras - Ordenes por Establecimiento y listar informacion para la recepcion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="estadoOrden"></param>
        /// <param name="fechaSolicitud"></param>
        /// <param name="idUsuarioLogueado"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroOficio"></param>
        /// <param name="tipoOrden"></param>
        /// <param name="idMuestra"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        /// 
        public DataTable OrdenMuestraRecepcion(int estadoOrden, int fechaSolicitud, int idUsuarioLogueado, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, int tipoOrden, string idMuestra, int idLaboratorio, string nroDocumento, string apellidopaterno, string apellidomaterno, string nombres)
        {
            fechaDesde = new DateTime(fechaDesde.Year, fechaDesde.Month, fechaDesde.Day);
            fechaHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day);
            var cnn = "";
            foreach (ConnectionStringSettings c in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                if (c.Name.StartsWith("DefaultConnection"))
                {
                    cnn = c.Name;
                }

            }
            DataTable muestra = new DataTable();
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[cnn].ConnectionString))
            {
                //pNLS_OrdenMuestraByEstablecimiento
                //pNLS_OrdenMuestraByEstablecimientoAndPaciente
                using (SqlCommand comando = new SqlCommand("pNLS_OrdenMuestraByEstablecimientoAndPaciente", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.CommandTimeout = 0;
                    comando.Parameters.Add(new SqlParameter("@fechaSolicitud", System.Data.SqlDbType.Int)).Value = fechaSolicitud;
                    comando.Parameters.Add(new SqlParameter("@idUsuarioLogueado", System.Data.SqlDbType.Int)).Value = idUsuarioLogueado;
                    comando.Parameters.Add(new SqlParameter("@codigoOrden", System.Data.SqlDbType.VarChar)).Value = codigoOrden;
                    comando.Parameters.Add(new SqlParameter("@fechaDesde", System.Data.SqlDbType.DateTime)).Value = fechaDesde;
                    comando.Parameters.Add(new SqlParameter("@fechaHasta", System.Data.SqlDbType.DateTime)).Value = fechaHasta;
                    comando.Parameters.Add(new SqlParameter("@nroOficio", System.Data.SqlDbType.VarChar)).Value = nroOficio;
                    comando.Parameters.Add(new SqlParameter("@tipoOrden", System.Data.SqlDbType.Int)).Value = tipoOrden;
                    comando.Parameters.Add(new SqlParameter("@estatus", System.Data.SqlDbType.Int)).Value = estadoOrden;
                    comando.Parameters.Add(new SqlParameter("@idMuestra", System.Data.SqlDbType.VarChar)).Value = idMuestra;
                    comando.Parameters.Add(new SqlParameter("@idLaboratorio", System.Data.SqlDbType.Int)).Value = idLaboratorio;

                    comando.Parameters.Add(new SqlParameter("@nrodocumento", System.Data.SqlDbType.VarChar)).Value = nroDocumento;
                    comando.Parameters.Add(new SqlParameter("@nombres", System.Data.SqlDbType.VarChar)).Value = nombres;
                    comando.Parameters.Add(new SqlParameter("@appaterno", System.Data.SqlDbType.VarChar)).Value = apellidopaterno;
                    comando.Parameters.Add(new SqlParameter("@apmaterno", System.Data.SqlDbType.VarChar)).Value = apellidomaterno;

                    var reader = comando.ExecuteReader();

                    muestra.Load(reader);

                    conexion.Close();
                    conexion.Dispose();
                }
            }
            return muestra;
        }

        public List<OrdenRecepcionVd> GetOrdenMuestraByEstablecimiento(int estadoOrden, int fechaSolicitud, int idUsuarioLogueado, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, int tipoOrden, string idMuestra, int idLaboratorio, string nroDocumento, string apellidopaterno, string apellidomaterno, string nombres)
        {
            List<OrdenRecepcionVd> listaOrdenes = new List<OrdenRecepcionVd>();

            //var objCommand = GetSqlCommand("pNLS_OrdenMuestraByEstablecimiento");

            //InputParameterAdd.Int(objCommand, "fechaSolicitud", fechaSolicitud);
            //InputParameterAdd.Int(objCommand, "idUsuarioLogueado", idUsuarioLogueado);
            //InputParameterAdd.Varchar(objCommand, "codigoOrden", codigoOrden);
            //InputParameterAdd.DateTime(objCommand, "fechaDesde", fechaDesde);
            //InputParameterAdd.DateTime(objCommand, "fechaHasta", fechaHasta);
            //InputParameterAdd.Varchar(objCommand, "nroOficio", nroOficio);
            //InputParameterAdd.Int(objCommand, "tipoOrden", 1);
            //InputParameterAdd.Int(objCommand, "estatus", estadoOrden);
            //InputParameterAdd.Varchar(objCommand, "idMuestra", idMuestra);
            //InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);


            DataTable dataOrdenMuestra = null;
            //dataOrdenMuestra = Execute(objCommand);
            dataOrdenMuestra = OrdenMuestraRecepcion(estadoOrden,fechaSolicitud,idUsuarioLogueado,codigoOrden,fechaDesde,fechaHasta,nroOficio,tipoOrden,idMuestra,idLaboratorio, nroDocumento, apellidopaterno, apellidomaterno,nombres);
            String nroDoc = "";
            if (dataOrdenMuestra.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataOrdenMuestra.Rows.Count; i++)
            {
                var row = dataOrdenMuestra.Rows[i];
                OrdenRecepcionVd ordenRecepcion = new OrdenRecepcionVd();
                ordenRecepcion.idOrden = Converter.GetGuid(row, "idOrden");
                ordenRecepcion.codigo = row["codigoOrden"].ToString();
                ordenRecepcion.nroOficio = row["nroOficio"].ToString();

                if (tipoOrden != 0)
                {
                    if (!row.IsNull("nroDocumento"))
                        nroDoc = row["nroDocumento"].ToString();
                    ordenRecepcion.nroDocumento = nroDoc;

                    switch (tipoOrden)
                    {
                        case 1:
                            ordenRecepcion.tipoOrden = "Persona";
                            break;
                        case 2:
                            ordenRecepcion.tipoOrden = "Animal";
                            break;
                        case 3:
                            ordenRecepcion.tipoOrden = "Cepa/Banco Sangre";
                            break;
                    }
                }
                else
                {
                    if (!row.IsNull("nroDocPaciente"))
                        ordenRecepcion.nroDocPaciente = row["nroDocPaciente"].ToString();
                    else
                        ordenRecepcion.nroDocPaciente = "";

                    if (!row.IsNull("nroDocAnimal"))
                        ordenRecepcion.nroDocAnimal = row["nroDocAnimal"].ToString();
                    else
                        ordenRecepcion.nroDocAnimal = "";

                    if (!row.IsNull("nroDocCepaBanco"))
                        ordenRecepcion.nroDocCepaBanco = row["nroDocCepaBanco"].ToString();
                    else
                        ordenRecepcion.nroDocCepaBanco = "";

                    if (ordenRecepcion.nroDocPaciente != "")
                    {
                        ordenRecepcion.tipoOrden = "Persona";
                        ordenRecepcion.nroDocumento = ordenRecepcion.nroDocPaciente;
                    }
                    else if (ordenRecepcion.nroDocAnimal != "")
                    {
                        ordenRecepcion.tipoOrden = "Animal";
                        ordenRecepcion.nroDocumento = ordenRecepcion.nroDocAnimal;
                    }
                    else if (ordenRecepcion.nroDocCepaBanco != "")
                    {
                        ordenRecepcion.tipoOrden = "Cepa/Banco Sangre";
                        ordenRecepcion.nroDocumento = ordenRecepcion.nroDocCepaBanco;
                    }
                }

                if (!row.IsNull("estadoOrden"))
                    ordenRecepcion.estadoOrden = int.Parse(row["estadoOrden"].ToString());
                else
                    ordenRecepcion.estadoOrden = 0;

                if (!row.IsNull("fechaSolicitud"))
                {
                    ordenRecepcion.fechaSolicitud = DateTime.Parse(row["fechaSolicitud"].ToString());
                    ordenRecepcion.fechaSolicitudStr = ordenRecepcion.fechaSolicitud.ToShortDateString();
                }
                else
                    ordenRecepcion.fechaSolicitud = new DateTime();


                if (!row.IsNull("nombreEstablecimiento"))
                {
                    ordenRecepcion.nombreEstablecimiento = row["nombreEstablecimiento"].ToString();
                }
                else
                    ordenRecepcion.nombreEstablecimiento = "";

                if (!row.IsNull("EXISTE_RECIBIDO"))
                    ordenRecepcion.EXISTE_RECIBIDO = int.Parse(row["EXISTE_RECIBIDO"].ToString());
                else
                    ordenRecepcion.EXISTE_RECIBIDO = 0;

                if (!row.IsNull("EXISTE_REFERENCIADO"))
                    ordenRecepcion.EXISTE_REFERENCIADO = int.Parse(row["EXISTE_REFERENCIADO"].ToString());
                else
                    ordenRecepcion.EXISTE_REFERENCIADO = 0;

                if (!row.IsNull("EXISTE_RECHAZOLAB"))
                    ordenRecepcion.EXISTE_RECHAZOLAB = int.Parse(row["EXISTE_RECHAZOLAB"].ToString());
                else
                    ordenRecepcion.EXISTE_RECHAZOLAB = 0;

                if (!row.IsNull("EXISTE_PENDIENTE"))
                    ordenRecepcion.EXISTE_PENDIENTE = int.Parse(row["EXISTE_PENDIENTE"].ToString());
                else
                    ordenRecepcion.EXISTE_PENDIENTE = 0;

                if (!row.IsNull("EXISTE_ORDENRECHAZO"))
                    ordenRecepcion.EXISTE_ORDENRECHAZO = int.Parse(row["EXISTE_ORDENRECHAZO"].ToString());
                else
                    ordenRecepcion.EXISTE_ORDENRECHAZO = 0;

                if (!row.IsNull("genero"))
                    ordenRecepcion.genero = int.Parse(row["genero"].ToString());
                else
                    ordenRecepcion.genero = 1;

                //nuevas columnas
                if (!row.IsNull("nombreGenero"))
                    ordenRecepcion.nombreGenero = row["nombreGenero"].ToString();
                else
                    ordenRecepcion.nombreGenero = "";

                if (!row.IsNull("tipoDocumento"))
                    ordenRecepcion.tipoDocumento = row["tipoDocumento"].ToString();
                else
                    ordenRecepcion.tipoDocumento = "";

                if (!row.IsNull("fechaNacimiento"))
                {
                    ordenRecepcion.fechaNacimiento = DateTime.Parse(row["fechaNacimiento"].ToString());
                }
                else
                    ordenRecepcion.fechaNacimiento = null;

                if (!row.IsNull("codificacion"))
                    ordenRecepcion.codigoMuestra = row["codificacion"].ToString();
                else
                    ordenRecepcion.codigoMuestra = "";

                listaOrdenes.Add(ordenRecepcion);
            }

            //using (SqlConnection conexion = new SqlConnection("Data Source=INSSQL2_3\\S2K8R204;Initial Catalog=Netlab_Nuevo; user=userSisNetlab; password=$N3tlabv2$*; MultipleActiveResultSets=False;Persist Security Info=false;Connect Timeout=9000"))
            //{
            //    using (SqlCommand comando = new SqlCommand("pNLS_OrdenMuestraByEstablecimiento", conexion))
            //    {
            //        conexion.Open();
            //        comando.CommandType = System.Data.CommandType.StoredProcedure;
            //        //comando.BeginExecuteNonQuery();
            //        comando.Parameters.Add(new SqlParameter("@FechaSolicitud", System.Data.SqlDbType.Int)).Value = fechaSolicitud;
            //        comando.Parameters.Add(new SqlParameter("@IdUsuarioLogueado", System.Data.SqlDbType.Int)).Value = idUsuarioLogueado;
            //        comando.Parameters.Add(new SqlParameter("@CodigoOrden", System.Data.SqlDbType.VarChar)).Value = codigoOrden;
            //        comando.Parameters.Add(new SqlParameter("@FechaDesde", System.Data.SqlDbType.DateTime)).Value = fechaDesde;
            //        comando.Parameters.Add(new SqlParameter("@FechaHasta", System.Data.SqlDbType.DateTime)).Value = fechaHasta;
            //        comando.Parameters.Add(new SqlParameter("@NroOficio", System.Data.SqlDbType.VarChar)).Value = nroOficio;
            //        comando.Parameters.Add(new SqlParameter("@TipoOrden", System.Data.SqlDbType.Int)).Value = tipoOrden;
            //        comando.Parameters.Add(new SqlParameter("@Estatus", System.Data.SqlDbType.Int)).Value = estadoOrden;
            //        comando.Parameters.Add(new SqlParameter("@IdMuestra", System.Data.SqlDbType.VarChar)).Value = idMuestra;
            //        comando.Parameters.Add(new SqlParameter("@IdLaboratorio", System.Data.SqlDbType.Int)).Value = idLaboratorio;
            //        comando.CommandTimeout = 0;
            //        try
            //        {
            //            SqlDataReader reader = comando.ExecuteReader();
            //            if (reader.HasRows)
            //            {
            //                while (reader.Read())
            //                {
            //                    var obj = new OrdenRecepcionVd();
            //                    obj.idOrden = (Guid)reader["idOrden"];
            //                    obj.codigo = reader["codigoOrden"].ToString();
            //                    obj.nroOficio = reader["nroOficio"].ToString();

            //                    if (tipoOrden != 0)
            //                    {
            //                        if (reader["nroOficio"].ToString() != null)
            //                        {
            //                            obj.nroOficio = reader["nroOficio"].ToString();
            //                        }
            //                        switch (tipoOrden)
            //                        {
            //                            case 1:
            //                                obj.tipoOrden = "Persona";
            //                                break;
            //                            case 2:
            //                                obj.tipoOrden = "Animal";
            //                                break;
            //                            case 3:
            //                                obj.tipoOrden = "Cepa/Banco Sangre";
            //                                break;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (reader["nroDocPaciente"].ToString() != null)
            //                            obj.nroDocPaciente = reader["nroDocPaciente"].ToString();
            //                        else
            //                            obj.nroDocPaciente = "";

            //                        if (reader["nroDocAnimal"].ToString() != null)
            //                            obj.nroDocAnimal = reader["nroDocAnimal"].ToString();
            //                        else
            //                            obj.nroDocAnimal = "";

            //                        if (reader["nroDocCepaBanco"].ToString() != null)
            //                            obj.nroDocCepaBanco = reader["nroDocCepaBanco"].ToString();
            //                        else
            //                            obj.nroDocCepaBanco = "";
            //                        if (obj.nroDocPaciente != "")
            //                        {
            //                            obj.tipoOrden = "Persona";
            //                            obj.nroDocumento = obj.nroDocPaciente;
            //                        }
            //                        else if (obj.nroDocAnimal != "")
            //                        {
            //                            obj.tipoOrden = "Animal";
            //                            obj.nroDocumento = obj.nroDocAnimal;
            //                        }
            //                        else if (obj.nroDocCepaBanco != "")
            //                        {
            //                            obj.tipoOrden = "Cepa/Banco Sangre";
            //                            obj.nroDocumento = obj.nroDocCepaBanco;
            //                        }
            //                    }

            //                    if (reader["estadoOrden"].ToString() != null)
            //                        obj.estadoOrden = (int)reader["estadoOrden"];
            //                    else
            //                        obj.estadoOrden = 0;

            //                    if (reader["fechaSolicitud"].ToString() != null)
            //                    {
            //                        obj.fechaSolicitud = (DateTime)reader["fechaSolicitud"];
            //                        obj.fechaSolicitudStr = obj.fechaSolicitud.ToShortDateString();
            //                    }
            //                    else
            //                        obj.fechaSolicitud = new DateTime();

            //                    if (reader["nombreEstablecimiento"].ToString() != null)
            //                    {
            //                        obj.nombreEstablecimiento = reader["nombreEstablecimiento"].ToString();
            //                    }
            //                    else
            //                        obj.nombreEstablecimiento = "";

            //                    if (reader["EXISTE_RECIBIDO"].ToString() != "")
            //                        obj.EXISTE_RECIBIDO = (int)reader["EXISTE_RECIBIDO"];
            //                    else
            //                        obj.EXISTE_RECIBIDO = 0;

            //                    if (reader["EXISTE_REFERENCIADO"].ToString() != "")
            //                        obj.EXISTE_REFERENCIADO = (int)reader["EXISTE_REFERENCIADO"];
            //                    else
            //                        obj.EXISTE_REFERENCIADO = 0;

            //                    if (reader["EXISTE_RECHAZOLAB"].ToString() != "")
            //                        obj.EXISTE_RECHAZOLAB = (int)reader["EXISTE_RECHAZOLAB"];
            //                    else
            //                        obj.EXISTE_RECHAZOLAB = 0;

            //                    if (reader["EXISTE_PENDIENTE"].ToString() != "")
            //                        obj.EXISTE_PENDIENTE = (int)reader["EXISTE_PENDIENTE"];
            //                    else
            //                        obj.EXISTE_PENDIENTE = 0;

            //                    if (reader["genero"].ToString() != null)
            //                        obj.genero = (int)reader["genero"];
            //                    else
            //                        obj.genero = 1;

            //                    if (reader["nombreGenero"].ToString() != null)
            //                        obj.nombreGenero = reader["nombreGenero"].ToString();
            //                    else
            //                        obj.nombreGenero = "";

            //                    if (reader["tipoDocumento"].ToString() != null)
            //                        obj.tipoDocumento = reader["tipoDocumento"].ToString();
            //                    else
            //                        obj.tipoDocumento = "";

            //                    if (reader["fechaNacimiento"].ToString() != null)
            //                        obj.fechaNacimiento = (DateTime)reader["fechaNacimiento"];
            //                    else
            //                        obj.fechaNacimiento = null;
            //                    listaOrdenes.Add(obj);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            conexion.Close();
            //            conexion.Dispose();
            //            throw ex;
            //        }
            //        finally
            //        {
            //            conexion.Close();
            //            conexion.Dispose();
            //        }

            //    }
            //}
            return listaOrdenes;
        }
        /// <summary>
        /// Descripción: Metodo para obtener muestras-ordenes para ingresar resultados por usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <param name="idUsuario"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroOficio"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="codigoMuestra"></param>
        /// <param name="estadoResultado"></param>
        /// <returns></returns>
        public List<OrdenIngresarResultadoVd> GetOrdenMuestraResultadosByUser(
            int tipo, int idEstablecimientoLogin, int idUsuario, string nroDocumento, DateTime fechaDesde, DateTime fechaHasta,
            string nroOficio, string codigoOrden, string codigoMuestra, int estadoResultado, int idEnfermedad, Guid idExamen)
        {
            var listaOrdenes = new List<OrdenIngresarResultadoVd>();

            var objCommand = GetSqlCommand("pNLS_OrdenesParaResultados");

            InputParameterAdd.Int(objCommand, "Tipo", tipo);
            InputParameterAdd.Int(objCommand, "IdEstablecimientoLogin", idEstablecimientoLogin);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Varchar(objCommand, "NroDocumento", nroDocumento);
            InputParameterAdd.DateTime(objCommand, "FechaDesde", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "FechaHasta", fechaHasta);
            InputParameterAdd.Varchar(objCommand, "NroOficio", nroOficio);
            InputParameterAdd.Varchar(objCommand, "CodigoOrden", codigoOrden);
            InputParameterAdd.Varchar(objCommand, "CodigoMuestra", codigoMuestra);
            InputParameterAdd.Int(objCommand, "Estatus", estadoResultado);
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);


            var dataOrdenMuestra = Execute(objCommand);

            if (dataOrdenMuestra.Rows.Count == 0)
                return listaOrdenes;
            int x = 0;
            for (var i = 0; i < dataOrdenMuestra.Rows.Count; i++)
            {
                var row = dataOrdenMuestra.Rows[i];

                var item = new OrdenIngresarResultadoVd
                {
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    fechaSolicitud = Converter.GetDateTime(row, "fechaSolicitud"),
                    fechaNacimiento = Converter.GetDateTime(row, "fechaNacimiento"),
                    idPaciente = Converter.GetGuid(row, "idPaciente"),
                    idAnimal = Converter.GetGuid(row, "idAnimal"),
                    idCBS = Converter.GetGuid(row, "idCepaBancoSangre"),
                    idEstablecimiento = int.Parse(row["idEstablecimiento"].ToString()),
                    nombreEstablecimiento = row["nombre"].ToString(),
                    nroDocumento = row["nroDocumento"].ToString(),
                    codigoOrden = row["codigoOrden"].ToString(),
                    //status = int.Parse(row["estatus"].ToString()) == 2 ? "Recepcionado Parcial" : "Recepcionado",
                    statusP = int.Parse(row["estatusP"].ToString()),
                    nroOficio = row["nroOficio"].ToString(),
                    estadoOrden = 1,
                    tipoDocumento = row["TipoDocumento"].ToString(),
                    step = 0,
                    flagResultado = int.Parse(row["FlagResultado"].ToString()),
                    nombreExamen = row["Examen"].ToString(),
                    tipoExamen = Converter.GetInt(row, "tipoExamen"),
                    FechaSiembraCultivo = row["FechaSiembraCultivo"].ToString(),
                    idExamen = Guid.Parse(row["idExamen"].ToString()),
                    estatusE = int.Parse(row["estatusE"].ToString()),
                    conformeP = int.TryParse(row["conformeP"].ToString(), out x) ? x : default(int?),
                    nombrePaciente = row["Paciente"].ToString(),
                    idOrdenExamen = Converter.GetGuid(row, "idOrdenExamen"),
                    codigoMuestra = row["codificacion"].ToString(),
                    ConnombreExamen = row["codigoExamen"].ToString(),
                    ConComponente = row["Usuario"].ToString(),
                    fechaColeccion = Converter.GetDateTime(row, "fechaColeccion"),
                    FormatoSolicitud = row["codigoFormato"].ToString()
                };

                var muestras = int.Parse(row["muestras"].ToString());
                var validadas = int.Parse(row["validadas"].ToString());

                if (muestras == validadas)
                {
                    item.step = 1;
                }

                listaOrdenes.Add(item);
            }

            return listaOrdenes;
        }

        /// <summary>
        /// Descripción: Obtiene informacion de las ordene filtrado por el Id de las ordenes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLabUsuario"></param>
        /// <returns></returns>
        public Orden GetInfoOrden(Guid idOrden, int? idLabUsuario)
        {
            var ordenObjInfo = new Orden();
            var listaOrdenInfo = new List<OrdenVd>();

            var objCommand = GetSqlCommand("pNLS_InformacionOrdenByOrdenId");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            InputParameterAdd.Int(objCommand, "IdLaboratorioDestino", idLabUsuario);
            var dataOrdenMuestraInfo = Execute(objCommand);

            if (dataOrdenMuestraInfo.Rows.Count == 0)
                return null;

            var codigoOrden = "";
            var nroOficio = "";
            var fechaINS = "";
            var fechaEvaluacion = "";

            for (var i = 0; i < dataOrdenMuestraInfo.Rows.Count; i++)
            {
                var row = dataOrdenMuestraInfo.Rows[i];

                var ordenInfo = new OrdenVd
                {
                    idExamen = Guid.Parse(row["idExamen"].ToString()),
                    nombreEnfermedad = row["nombreEnfermedad"].ToString(),
                    nombreExamen = row["nombreExamen"].ToString(),
                    nombreTipoMuestra = row["nombreTipoMuestra"].ToString(),
                    ConformeR = string.IsNullOrEmpty(row["ConformeR"].ToString()) ? -1 : int.Parse(row["ConformeR"].ToString())
                };

                codigoOrden = row["codigoOrden"].ToString();
                nroOficio = row["nroOficio"].ToString();
                fechaINS = row["fechaRecepcionRomINS"].ToString();

                fechaEvaluacion = row["fechaReevaluacionFicha"].ToString();
                ordenObjInfo.usuario = new Usuario();
                //Autor: Juan Muga
                //Descripción: Se agrega campos de estados pertenecientes a la orden.
                ordenObjInfo.ConformeR = string.IsNullOrEmpty(row["ConformeR"].ToString()) ? -1 : int.Parse(row["ConformeR"].ToString());
                ordenObjInfo.EstadoOrdenExamen = string.IsNullOrEmpty(row["EstadoOrdenExamen"].ToString()) ? -1 : int.Parse(row["EstadoOrdenExamen"].ToString());
                ordenObjInfo.EstadoOrdenExamenOrdenMuestra = string.IsNullOrEmpty(row["EstadoOrdenExamenOrdenMuestra"].ToString()) ? -1 : int.Parse(row["EstadoOrdenExamenOrdenMuestra"].ToString());
                ordenObjInfo.EstadoOrdenMuestra = string.IsNullOrEmpty(row["EstadoOrdenMuestra"].ToString()) ? -1 : int.Parse(row["EstadoOrdenMuestra"].ToString());
                ordenObjInfo.EstadoOrdenMuestraRecepcion = string.IsNullOrEmpty(row["EstadoOrdenMuestraRecepcion"].ToString()) ? -1 : int.Parse(row["EstadoOrdenMuestraRecepcion"].ToString());
                ordenObjInfo.idOrden = idOrden;
                //fin

                listaOrdenInfo.Add(ordenInfo);
            }
            if (!string.IsNullOrEmpty(fechaINS))
            {
                ordenObjInfo.fechaIngresoINS = DateTime.Parse(fechaINS);
            }
            if (!string.IsNullOrEmpty(fechaEvaluacion))
            {
                ordenObjInfo.fechaReevaluacionFicha = DateTime.Parse(fechaEvaluacion);
            }
            ordenObjInfo.ordenInfoList = listaOrdenInfo;
            ordenObjInfo.codigoOrden = codigoOrden;
            ordenObjInfo.nroOficio = nroOficio;
            return ordenObjInfo;
        }
        /// <summary>
        /// Descripción: Obtener los materiales por el codigo de orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLabUsuario"></param>
        /// <returns></returns>
        public List<OrdenMaterialVd> MaterialesByOrden(Guid idOrden, int idLabUsuario)
        {
            List<OrdenMaterialVd> listaOrdenMaterial = new List<OrdenMaterialVd>();

            var objCommand = GetSqlCommand("pNLS_MaterialesByOrden");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idLaboratorioDestino", idLabUsuario);

            DataTable dataOrdenMuestraMaterial = Execute(objCommand);

            if (dataOrdenMuestraMaterial.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataOrdenMuestraMaterial.Rows.Count; i++)
            {
                var row = dataOrdenMuestraMaterial.Rows[i];
                OrdenMaterialVd ordenMaterial = new OrdenMaterialVd();
                ordenMaterial.tipoMuestraNombre = row["tipoMuestraNombre"].ToString();
                ordenMaterial.presentacionNombre = row["presentacionNombre"].ToString();
                ordenMaterial.cantidad = row["cantidad"].ToString();
                ordenMaterial.reactivoNombre = row["reactivoNombre"].ToString();

                listaOrdenMaterial.Add(ordenMaterial);
            }

            return listaOrdenMaterial;
        }
        /// <summary>
        /// Descripción: Permite obtener materiales referenciados por codigo de orden y codigo de laboratorio.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLabUsuario"></param>
        /// <returns></returns>
        public List<OrdenMaterialVd> MaterialesRefenciadosByOrden(Guid idOrden, int idLabUsuario)
        {
            List<OrdenMaterialVd> listaOrdenMaterial = new List<OrdenMaterialVd>();

            var objCommand = GetSqlCommand("pNLS_MaterialesReferenciadosByOrden");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idLaboratorioUsuario", idLabUsuario);

            DataTable dataOrdenMuestraMaterial = Execute(objCommand);

            if (dataOrdenMuestraMaterial.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataOrdenMuestraMaterial.Rows.Count; i++)
            {
                var row = dataOrdenMuestraMaterial.Rows[i];
                OrdenMaterialVd ordenMaterial = new OrdenMaterialVd();
                ordenMaterial.tipoMuestraNombre = row["tipoMuestraNombre"].ToString();
                ordenMaterial.presentacionNombre = row["presentacionNombre"].ToString();
                ordenMaterial.cantidad = row["cantidad"].ToString();
                ordenMaterial.reactivoNombre = row["reactivoNombre"].ToString();

                listaOrdenMaterial.Add(ordenMaterial);
            }

            return listaOrdenMaterial;
        }
        /// <summary>
        /// Descripción: Permite obtener información de los materiales ingresados para una orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<OrdenMaterial> MuestrasByOrden(Guid idOrden)
        {
            List<OrdenMaterial> listaMaterial = new List<OrdenMaterial>();

            var objCommand = GetSqlCommand("pNLS_MaterialOrden");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            DataTable dataOrdenMuestral = Execute(objCommand);

            if (dataOrdenMuestral.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataOrdenMuestral.Rows.Count; i++)
            {
                var row = dataOrdenMuestral.Rows[i];
                OrdenMaterial ordenMaterial = new OrdenMaterial();

                if (!row.IsNull("tipoMuestraNombre"))
                    ordenMaterial.tipoMuestraNombre = row["tipoMuestraNombre"].ToString();
                else
                    ordenMaterial.tipoMuestraNombre = String.Empty;

                if (!row.IsNull("idMaterial"))
                    ordenMaterial.idMaterial = int.Parse(row["idMaterial"].ToString());
                else
                    ordenMaterial.idMaterial = 0;

                if (!row.IsNull("idTipoMuestra"))
                    ordenMaterial.idTipoMuestra = int.Parse(row["idTipoMuestra"].ToString());
                else
                    ordenMaterial.idTipoMuestra = 0;
                if (!row.IsNull("idMuestraCod"))
                    ordenMaterial.idMuestraCod = Guid.Parse(row["idMuestraCod"].ToString());

                listaMaterial.Add(ordenMaterial);
            }

            return listaMaterial;
        }
        /// <summary>
        /// Descripción: Metodo para obtener el material de las ordenes recepcionadas
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<OrdenMaterial> MuestrasByOrdenRecepcionadas(Guid idOrden)
        {
            List<OrdenMaterial> listaMaterial = new List<OrdenMaterial>();

            var objCommand = GetSqlCommand("pNLS_MaterialOrdenRecepcionados");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            DataTable dataOrdenMuestral = Execute(objCommand);

            if (dataOrdenMuestral.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataOrdenMuestral.Rows.Count; i++)
            {
                var row = dataOrdenMuestral.Rows[i];
                OrdenMaterial ordenMaterial = new OrdenMaterial();

                if (!row.IsNull("tipoMuestraNombre"))
                    ordenMaterial.tipoMuestraNombre = row["tipoMuestraNombre"].ToString();
                else
                    ordenMaterial.tipoMuestraNombre = String.Empty;

                if (!row.IsNull("idMaterial"))
                    ordenMaterial.idMaterial = int.Parse(row["idMaterial"].ToString());
                else
                    ordenMaterial.idMaterial = 0;

                if (!row.IsNull("idTipoMuestra"))
                    ordenMaterial.idTipoMuestra = int.Parse(row["idTipoMuestra"].ToString());
                else
                    ordenMaterial.idTipoMuestra = 0;


                listaMaterial.Add(ordenMaterial);
            }

            return listaMaterial;
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de las muestras 
        ///              por código de orden para recepcionar.
        /// Estados: 0: no procesada | 1: procesada en un lugar -- 0
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="procesamiento"></param>
        /// <param name="idUsuario"></param>
        /// <param name="estatus"></param>
        /// <param name="conCriteriosRechazo"></param>
        /// <param name="rechazadas"></param>
        /// <param name="idLaboratorioUsuario"></param>
        /// <returns></returns>
        public List<OrdenMuestraRecepcion> MuestraRecepcionadosbyOrden(
            Guid idOrden, int procesamiento, int idUsuario, int estatus, int conCriteriosRechazo, int rechazadas,
            int idLaboratorioUsuario)
        {
            var listaMuestraRecepcion = new List<OrdenMuestraRecepcion>();

            var objCommand = GetSqlCommand("pNLS_MaterialRecepcionByOrden");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "procesamiento", procesamiento);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "estatus", estatus);
            InputParameterAdd.Int(objCommand, "rechazadas", rechazadas); //1: agrege filtro rechazdas | 0: ningun filtro
            InputParameterAdd.Int(objCommand, "idLaboratorioUsuario", idLaboratorioUsuario);

            var dataOrdenMuestraRecepcion = Execute(objCommand);

            if (dataOrdenMuestraRecepcion.Rows.Count == 0)
                return null;

            for (var i = 0; i < dataOrdenMuestraRecepcion.Rows.Count; i++)
            {
                var row = dataOrdenMuestraRecepcion.Rows[i];

                var ordenRecepcion = new OrdenMuestraRecepcion
                {
                    idOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion"),
                    idMaterial = int.Parse(row["idMaterial"].ToString()),
                    presentacionNombreNro = row["presentacion"].ToString(),
                    idOrdenMuestra = Converter.GetGuid(row, "idOrdenMuestra"),
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    fechaRecepcion = DateTime.Parse(row["fechaRecepcion"].ToString()),
                    horaRecepcion = DateTime.Parse(row["horaRecepcion"].ToString()),
                    examenNombre = row["examenNombre"].ToString(),
                    idExamen = Guid.Parse(row["idExamen"].ToString()),
                    Tipo = int.Parse(row["Tipo"].ToString()),
                    nombreLaboratorioDestino = row["LaboratorioDestino"].ToString(),
                    codigoMuestra = row["codigoMuestra"].ToString(),
                    nombreLaboratorioEnvio = row["laboratorioEnvio"].ToString(),
                    fechaColeccion = DateTime.Parse(row["fechaColeccion"].ToString()),
                    horaColeccion = DateTime.Parse(row["horaColeccion"].ToString()),
                    horaRecepcionStr = DateTime.Parse(row["horaRecepcion"].ToString()).ToString("HH:mm"),
                    fechaHoraColeccionStr = DateTime.Parse(row["fechaColeccion"].ToString()).ToShortDateString() + " - " +
                        DateTime.Parse(row["horaColeccion"].ToString()).ToString("HH:mm")
                };

                listaMuestraRecepcion.Add(ordenRecepcion);
            }

            if (conCriteriosRechazo == 1)
            {
                foreach (var itemOrdenMuestra in listaMuestraRecepcion)
                {
                    var rechazos = "";
                    var objCommand1 = GetSqlCommand("pNLS_CriterioRechazoByRecepcionId");
                    InputParameterAdd.Guid(objCommand1, "IdOrdenMuestraRecepcion", itemOrdenMuestra.idOrdenMuestraRecepcion);
                    var dataRechazos = Execute(objCommand1);

                    for (var i = 0; i < dataRechazos.Rows.Count; i++)
                    {
                        var row = dataRechazos.Rows[i];
                        var tmpRechazo = row["glosa"].ToString();
                        rechazos = rechazos + tmpRechazo + ",";
                    }

                    if (rechazos != "")
                    {
                        var tmpRechazo = rechazos.Substring(0, rechazos.Length - 1);
                        itemOrdenMuestra.rechazos = tmpRechazo;
                    }
                    else
                    {
                        itemOrdenMuestra.rechazos = "";
                    }
                }
            }

            return listaMuestraRecepcion;
        }
        /// <summary>
        /// Descripción: Obtener informacion de los examenes o pruebas por codigo de orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<OrdenExamen> OrdenExamenByOrden(Guid idOrden)
        {
            List<OrdenExamen> listaOrdenExamen = new List<OrdenExamen>();

            var objCommand = GetSqlCommand("pNLS_OrdenExamenByOrden");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            DataTable dataOrdenExamen = Execute(objCommand);

            if (dataOrdenExamen.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataOrdenExamen.Rows.Count; i++)
            {
                var row = dataOrdenExamen.Rows[i];
                OrdenExamen ordenExamen = new OrdenExamen();
                ordenExamen.idOrdenExamen = Guid.Parse(row["idOrdenExamen"].ToString());
                ordenExamen.idOrden = Guid.Parse(row["idOrden"].ToString());
                ordenExamen.idExamen = Guid.Parse(row["idExamen"].ToString());
                ordenExamen.idEnfermedad = int.Parse(row["idEnfermedad"].ToString());
                ordenExamen.estatus = int.Parse(row["estatus"].ToString());
                //SOTERO
                ordenExamen.IdTipoMuestra = int.Parse(row["idTipoMuestra"].ToString());
                ordenExamen.nombreTipoMuestra = row["NombreTM"].ToString();
                // 

                Examen examen = new Examen();
                examen.idExamen = Guid.Parse(row["idExamen"].ToString());
                examen.nombre = row["nombreExamen"].ToString();
                ordenExamen.Examen = examen;

                Enfermedad enfermedad = new Enfermedad();
                enfermedad.idEnfermedad = int.Parse(row["idEnfermedad"].ToString());
                enfermedad.nombre = row["nombreEnfermedad"].ToString();
                //SOTERO
                enfermedad.Snomed = row["snomed"].ToString();
                ordenExamen.Enfermedad = enfermedad;

                var objCommand1 = GetSqlCommand("pNLS_OrdenMuestraByOrden");
                InputParameterAdd.Guid(objCommand1, "idOrden", idOrden);
                DataTable dataOrdenMuestra = Execute(objCommand1);


                listaOrdenExamen.Add(ordenExamen);
            }

            return listaOrdenExamen;
        }

        /******************************************************************************************************************/
        public List<Correo> GetDatosCorreoMasivo(Guid idOrden)
        {
            List<Correo> listaTelefonos = new List<Correo>();
            var objCommand = GetSqlCommand("pNLS_ObtenerDatosCorreoMasivo");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            DataTable dataTelefono = Execute(objCommand);

            if (dataTelefono.Rows.Count == 0)
            {
                return null;
            }

            for (int i = 0; i < dataTelefono.Rows.Count; i++)
            {
                var row = dataTelefono.Rows[i];
                Correo lista = new Correo();
                lista.Paciente = new Paciente();
                lista.Solicitante = new OrdenSolicitante();
                lista.Orden = new Orden();
                lista.Paciente.Nombres = row["Paciente"].ToString();
                lista.Solicitante.Nombres = row["solicitante"].ToString();
                lista.Paciente.Celular1 = row["NumeroPaciente"].ToString();
                lista.Solicitante.telefonoContacto = row["NumeroSolicitante"].ToString();
                lista.Solicitante.correo = row["correo"].ToString();
                lista.Orden.codigoOrden = row["codigoOrden"].ToString();
                listaTelefonos.Add(lista);
            }
            return listaTelefonos;
        }
        /******************************************************************************************************************/

        #region recepcionar muestra
        /// <summary>
        /// Descripción: Controlador para la actualizar la orden y generar la Recepcion de Muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="listaMuestras"></param>
        /// <param name="idOrden"></param>
        /// <param name="registroResultados"></param>
        public void RecepcionarMuestras(List<OrdenMuestraRecepcion> listaMuestras, Guid idOrden, int registroResultados, int idUsuarioRegistro)
        {
            if (listaMuestras.Count > 0)
            {
                foreach (var item in listaMuestras)
                {
                    try
                    {
                        var objCommands = GetSqlCommand("pNLU_RecepcionarOrdenMuestra");
                        InputParameterAdd.Guid(objCommands, "idOrdenMuestraRecepcion", item.idOrdenMuestraRecepcion);
                        InputParameterAdd.DateTime(objCommands, "fechaRecep", item.fechaRecepcion);
                        InputParameterAdd.DateTime(objCommands, "horaRecep", item.horaRecepcion);
                        InputParameterAdd.Int(objCommands, "conforme", item.conforme);
                        InputParameterAdd.Int(objCommands, "idUsuario", idUsuarioRegistro);

                        DataTable dataMuestras = Execute(objCommands);

                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }

                var objCommand1 = GetSqlCommand("pNLS_ObtenerCantidadMuetrasPorRecepcionar");
                InputParameterAdd.Guid(objCommand1, "idOrder", idOrden);
                Execute(objCommand1);

                if (registroResultados == 1) //1: registra Analistos | 0: NO registra Analitos
                {
                    var objCommand2 = GetSqlCommand("pNLI_OrdenResultadoAnalito");
                    InputParameterAdd.Guid(objCommand2, "idOrden", idOrden);
                    Execute(objCommand2);
                }

            }

            return;
        }
        /// <summary>
        /// Descripción: Controlador para la actualizar la orden y generar la Recepcion de Muestra
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 26/11/2017
        /// Fecha Modificación: 26/11/2017.
        /// </summary>
        /// <param name="listaMuestras"></param>
        /// <param name="idOrden"></param>
        /// <param name="registroResultados"></param>
        public void RecepcionarMuestrasMasivo(string datos, int idUsuario)
        {

            var objCommands = GetSqlCommand("pNLU_RecepcionarOrdenMuestraMasiva");
            InputParameterAdd.Varchar(objCommands, "Lista", datos);
            InputParameterAdd.Int(objCommands, "idUsuario", idUsuario);

            ExecuteNonQuery(objCommands);

        }

        /// <summary>
        /// Descripción: Controlador para la actualizar la orden y generar la Recepcion de Muestra
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 26/11/2017
        /// Fecha Modificación: 26/11/2017.
        /// </summary>
        /// <param name="listaMuestras"></param>
        /// <param name="idOrden"></param>
        /// <param name="registroResultados"></param>
        public int VerificarMuestrasMasivo(ValidaResultadoMasivo ordenValidacion, int idUsuario)
        {
            try
            {
                var objCommands = GetSqlCommand("pNLU_ValidacionResultadosMasiva");
                InputParameterAdd.Guid(objCommands, "IdOrdenExamen", Guid.Parse(ordenValidacion.idOrdenExamen));
                InputParameterAdd.Varchar(objCommands, "Comentarios", ordenValidacion.Comentarios);
                InputParameterAdd.Int(objCommands, "Conforme", int.Parse(ordenValidacion.Conforme));
                InputParameterAdd.Int(objCommands, "idUsuario", idUsuario);

                ExecuteNonQuery(objCommands);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        #endregion


        #region referenciar muestra
        /// <summary>
        /// Descripción: Metodo para la actualizar la orden y generar la referenciación de Muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="listaMuestrasReferenciar"></param>
        /// <param name="idUsuarioEdicion"></param>
        public void ReferenciarMuestras(List<OrdenMuestraRecepcion> listaMuestrasReferenciar, int idUsuarioEdicion)
        {
            if (listaMuestrasReferenciar.Count > 0)
            {
                foreach (var item in listaMuestrasReferenciar)
                {
                    var objCommandr = GetSqlCommand("pNLU_ReferenciarOrdenMuestra");
                    InputParameterAdd.Guid(objCommandr, "idOrdenMuestraRecepcion", item.idOrdenMuestraRecepcion);
                    InputParameterAdd.DateTime(objCommandr, "fechaEnvio", item.fechaEnvio);
                    InputParameterAdd.DateTime(objCommandr, "horaEnvio", item.horaEnvio);
                    InputParameterAdd.Int(objCommandr, "idLaboratorioDestino", item.idLaboratorioDestino);
                    InputParameterAdd.Int(objCommandr, "idLaboratorioOrigen", item.idLaboratorioOrigen);
                    InputParameterAdd.Int(objCommandr, "idUsuarioEdicion", idUsuarioEdicion);
                    InputParameterAdd.Int(objCommandr, "derivar", item.derivar);//juan muga
                    InputParameterAdd.DateTime(objCommandr, "fechaRecepcion", item.fechaRecepcion);

                    DataTable dataMuestrasReferenciar = Execute(objCommandr);
                }
            }

            return;
        }

        public List<OrdenMuestraRecepcion> DerivarReferenciaMuestras(List<OrdenMuestraRecepcion> listaMuestrasReferenciar, int idUsuarioEdicion)
        {
            List<OrdenMuestraRecepcion> lstOrdenRecepcionGuid = new List<OrdenMuestraRecepcion>();

            if (listaMuestrasReferenciar.Count > 0)
            {
                foreach (var item in listaMuestrasReferenciar)
                {
                    var objCommandr = GetSqlCommand("pNLU_ReferenciarOrdenMuestra");
                    InputParameterAdd.Guid(objCommandr, "idOrdenMuestraRecepcion", item.idOrdenMuestraRecepcion);
                    InputParameterAdd.DateTime(objCommandr, "fechaEnvio", item.fechaEnvio);
                    InputParameterAdd.DateTime(objCommandr, "horaEnvio", item.horaEnvio);
                    InputParameterAdd.Int(objCommandr, "idLaboratorioDestino", item.idLaboratorioDestino);
                    InputParameterAdd.Int(objCommandr, "idLaboratorioOrigen", item.idLaboratorioOrigen);
                    InputParameterAdd.Int(objCommandr, "idUsuarioEdicion", idUsuarioEdicion);
                    InputParameterAdd.Int(objCommandr, "derivar", item.derivar);//juan muga
                    InputParameterAdd.DateTime(objCommandr, "fechaRecepcion", item.fechaRecepcion);

                    DataTable dataMuestrasReferenciar = Execute(objCommandr);

                    if (dataMuestrasReferenciar.Rows.Count == 0) continue;

                    for (int i = 0; i < dataMuestrasReferenciar.Rows.Count; i++)
                    {
                        var row = dataMuestrasReferenciar.Rows[i];
                        OrdenMuestraRecepcion ordenRecepcion = new OrdenMuestraRecepcion();
                        ordenRecepcion.idOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion");
                        ordenRecepcion.NewidOrdenMuestraRecepcion = Converter.GetGuid(row, "NewidOrdenMuestraRecepcion");
                        lstOrdenRecepcionGuid.Add(ordenRecepcion);
                    }
                }
            }

            return lstOrdenRecepcionGuid;
        }

        /// <summary>
        /// Descripción: Metodo para REGISTRA Y REFERENCIA MUESTRAS
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaMuestrasRegistar"></param>
        /// <param name="idOrden"></param>
        /// <param name="idUsuarioLogueado"></param>
        public void RegistrarMuestras(List<OrdenMuestraRecepcion> listaMuestrasRegistar, Guid idOrden, int idUsuarioLogueado)
        {
            if (listaMuestrasRegistar.Count > 0)
            {
                foreach (var item in listaMuestrasRegistar)
                {
                    try
                    {
                        var objCommandr2 = GetSqlCommand("pNLU_RegistrarMuestraRecepcion");
                        InputParameterAdd.Guid(objCommandr2, "IdOrdenMuestraPadre", item.idOrdenMuestraRecepcionPadre);
                        InputParameterAdd.Guid(objCommandr2, "IdExamen", item.idExamen);
                        InputParameterAdd.Guid(objCommandr2, "IdOrden", idOrden);
                        InputParameterAdd.Int(objCommandr2, "idUsuarioRegistro", idUsuarioLogueado);
                        InputParameterAdd.Int(objCommandr2, "idLaboratorioDestino", item.idLaboratorioDestino);
                        InputParameterAdd.DateTime(objCommandr2, "fechaEnvio", item.fechaEnvio);
                        InputParameterAdd.DateTime(objCommandr2, "horaEnvio", item.horaEnvio);
                        InputParameterAdd.Int(objCommandr2, "idTipoMuestra", item.idTipoMuestra);//juan muga
                        InputParameterAdd.Int(objCommandr2, "Derivar", item.derivar);//juan muga

                        DataTable dataMuestrasReferenciar = Execute(objCommandr2);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }

            return;
        }

        /// <summary>
        /// Descripción: Metodo para registrar informacion de muestras recepcionadas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios. Cambiar para Derivar
        /// </summary>
        /// <param name="listaMuestrasRegistarRecepcion"></param>
        /// <param name="idOrden"></param>
        /// <param name="idUsuarioLogueado"></param>
        public void RegistrarMuestrasConRecepcion(List<OrdenMuestraRecepcion> listaMuestrasRegistarRecepcion, Guid idOrden, int idUsuarioLogueado)
        {
            if (listaMuestrasRegistarRecepcion.Count > 0)
            {
                foreach (var item in listaMuestrasRegistarRecepcion)
                {
                    var objCommandr = GetSqlCommand("pNLU_RegistrarMuestraRecepcionar");
                    InputParameterAdd.Guid(objCommandr, "IdOrdenMuestraPadre", item.idOrdenMuestraRecepcion);
                    InputParameterAdd.Guid(objCommandr, "IdExamen", item.idExamen);
                    InputParameterAdd.Guid(objCommandr, "IdOrden", idOrden);
                    InputParameterAdd.Int(objCommandr, "idUsuarioRegistro", idUsuarioLogueado);
                    InputParameterAdd.Int(objCommandr, "idLaboratorioDestino", item.idLaboratorioDestino);
                    InputParameterAdd.DateTime(objCommandr, "fechaRecepcion", item.fechaRecepcion);
                    InputParameterAdd.DateTime(objCommandr, "horaRecepcion", item.horaRecepcion);
                    InputParameterAdd.Int(objCommandr, "idTipoMuestra", item.idTipoMuestra);

                    DataTable dataMuestrasReferenciar = Execute(objCommandr);
                }

                var objCommand1 = GetSqlCommand("pNLS_ObtenerCantidadMuetrasPorRecepcionar");
                InputParameterAdd.Guid(objCommand1, "idOrder", idOrden);
                Execute(objCommand1);
            }

            return;
        }
        /// <summary>
        /// Descripción: Registrar informacion de muestras sin estar recepcionadas. 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaMuestrasRegistar"></param>
        /// <param name="idOrden"></param>
        /// <param name="idUsuarioLogueado"></param>
        public void RegistrarMuestrasSinRecepcion(List<OrdenMuestraRecepcion> listaMuestrasRegistar, Guid idOrden, int idUsuarioLogueado)
        {
            if (listaMuestrasRegistar.Count > 0)
            {
                foreach (var item in listaMuestrasRegistar)
                {
                    var objCommandr = GetSqlCommand("pNLU_RegistrarMuestraSinRecepcion");
                    InputParameterAdd.Guid(objCommandr, "IdOrdenMuestraPadre", item.idOrdenMuestraRecepcion);
                    InputParameterAdd.Int(objCommandr, "idUsuarioRegistro", idUsuarioLogueado);
                    InputParameterAdd.Guid(objCommandr, "IdExamen", item.idExamen);
                    InputParameterAdd.Guid(objCommandr, "IdOrden", idOrden);
                    InputParameterAdd.Int(objCommandr, "idLaboratorioDestino", item.idLaboratorioDestino);

                    DataTable dataMuestrasReferenciar = Execute(objCommandr);
                }
            }

            return;
        }


        #endregion
        /// <summary>
        /// Descripción: Registrar las muestras que han sido rechazadas, solo se registra si no existe.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaCriteriosRechazos"></param>
        /// <param name="idUsuario"></param>
        public void RegistrarCriteriosRechazos(List<List<CriterioRechazo>> listaCriteriosRechazos, int idUsuario)
        {
            foreach (var item in listaCriteriosRechazos)
            {
                foreach (CriterioRechazo criterioRechazo in item)
                {
                    var objCommand = GetSqlCommand("pNLI_OrdenMuestraRechazo");
                    InputParameterAdd.Guid(objCommand, "idOrdenMuestraRecepcion", criterioRechazo.IdOrdenMuestraRecepcion);
                    InputParameterAdd.Int(objCommand, "idCriterioRechazo", criterioRechazo.IdCriterioRechazo);
                    InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
                    DataTable dataOrdenMuestra = Execute(objCommand);
                }
            }
        }
        /// <summary>
        /// Descripción: Obtener informacion de una muestra referenciada por orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <param name="estatus"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<OrdenMuestraRecepcion> MuestraReferenciadosEditar(Guid idOrden, int idUsuario, int estatus, int idLaboratorio)
        {
            List<OrdenMuestraRecepcion> listaMuestraRecepcion = new List<OrdenMuestraRecepcion>();

            var objCommand = GetSqlCommand("pNLS_ReferenciaRecepciones");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "estatus", estatus);
            InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);

            DataTable dataOrdenMuestraRecepcion = Execute(objCommand);

            if (dataOrdenMuestraRecepcion.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataOrdenMuestraRecepcion.Rows.Count; i++)
            {
                var row = dataOrdenMuestraRecepcion.Rows[i];
                OrdenMuestraRecepcion ordenRecepcion = new OrdenMuestraRecepcion();
                ordenRecepcion.idOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion");
                ordenRecepcion.idMaterial = int.Parse(row["idMaterial"].ToString());
                ordenRecepcion.presentacionNombreNro = row["presentacion"].ToString();

                ordenRecepcion.idOrdenMuestra = Converter.GetGuid(row, "idOrdenMuestra");
                ordenRecepcion.idOrden = Converter.GetGuid(row, "idOrden");
                ordenRecepcion.fechaRecepcion = DateTime.Parse(row["fechaRecepcion"].ToString());
                ordenRecepcion.horaRecepcion = DateTime.Parse(row["horaRecepcion"].ToString());
                ordenRecepcion.examenNombre = row["examenNombre"].ToString();
                ordenRecepcion.Tipo = int.Parse(row["Tipo"].ToString());
                ordenRecepcion.nombreLaboratorioDestino = row["LaboratorioDestino"].ToString();

                listaMuestraRecepcion.Add(ordenRecepcion);
            }

            return listaMuestraRecepcion;
        }

        /// <summary>
        /// Descripción: Obtener informacion de muestras validadas para el ingreso de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd MuestrasValidarResultados(Guid idOrden, int idUsuario, int idEstablecimientoLogin)
        {
            var objCommand = GetSqlCommand("pNLS_MuestrasValidarResultados");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdEstablecimientoLogin", idEstablecimientoLogin);

            var dataOrdenMuestraMaterial = Execute(objCommand);

            var orden = new OrdenIngresarResultadoVd
            {
                muestrasValidar = new List<MuestraResultadoVd>()
            };

            if (dataOrdenMuestraMaterial.Rows.Count == 0)
                return orden;

            for (var i = 0; i < dataOrdenMuestraMaterial.Rows.Count; i++)
            {
                var row = dataOrdenMuestraMaterial.Rows[i];

                if (i == 0)
                {
                    orden.codigoOrden = row["codigoOrden"].ToString();
                    orden.nroOficio = row["nroOficio"].ToString();
                    orden.nroDocumento = row["nroDocumento"].ToString();
                    orden.tipoDocumento = row["tipoDocumento"].ToString();
                    orden.nombrePaciente = row["nombrePaciente"] + " " + row["apellidoPaterno"] + " " + row["apellidoMaterno"];
                    orden.nombreEstablecimiento = row["nombreEstablecimiento"].ToString();
                    orden.idEstablecimiento = int.Parse(row["idEstablecimiento"].ToString());
                    orden.idOrden = Converter.GetGuid(row, "idOrden");
                    orden.codigoPaciente = row["codificacion"].ToString();
                    //orden.fechaNacimiento = DateTime.Parse(row["fechaNacimiento"].ToString());
                    orden.fechaNacimiento = (row["fechaNacimiento"]).ToString() != "" ? DateTime.Parse(row["fechaNacimiento"].ToString()) : DateTime.Parse("01/01/0001");
                    orden.fechaRegistro = DateTime.Parse(row["fechaRegistroOrden"].ToString());
                    orden.nombreProyecto = row["nombreProyecto"].ToString();
                    orden.edadPaciente = row["Edad"].ToString();
                }

                var muestra = new MuestraResultadoVd
                {
                    idOrdenMaterial = Converter.GetGuid(row, "idOrdenMaterial"),
                    idOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion"),
                    idTipoMuestra = int.Parse(row["idTipoMuestra"].ToString()),
                    tipoMuestra = row["nombreTipoMuestra"].ToString(),
                    presentacion = row["nombrePresentacion"].ToString(),
                    reactivo = row["nombreReactivo"].ToString(),
                    codificacion = row["codigoMuestra"].ToString(),
                    volumen = decimal.Parse(row["volumen"].ToString()),
                    fechaColeccion = DateTime.Parse(row["fechaColeccion"].ToString()),
                    fechaHoraColeccion = row["fechaHoraColeccion"].ToString(),
                    fechaHoraRecepcionLAB = row["fechaHoraRecepcionLAB"].ToString(),
                    fechaHoraRecepcionROM = row["fechaHoraRecepcionROM"].ToString(),
                    nombreExamen = row["ExamenNombre"].ToString(),
                    idExamen = Guid.Parse(row["idExamen"].ToString())
                };

                orden.muestrasValidar.Add(muestra);
            }

            return orden;
        }
        /// <summary>
        /// Descripción: Obtener informacion de examen para rechazo
        /// Author: Juan Muga.
        /// Fecha Creacion: 04/03/2019
        /// Fecha Modificación: 
        /// Modificación: 
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd RechazarExamen(Guid idOrden, int idUsuario, int idEstablecimientoLogin)
        {
            var objCommand = GetSqlCommand("pNLS_RechazarExamen");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdEstablecimientoLogin", idEstablecimientoLogin);

            var dataOrdenMuestraMaterial = Execute(objCommand);

            var orden = new OrdenIngresarResultadoVd
            {
                muestrasValidar = new List<MuestraResultadoVd>()
            };

            if (dataOrdenMuestraMaterial.Rows.Count == 0)
                return orden;

            for (var i = 0; i < dataOrdenMuestraMaterial.Rows.Count; i++)
            {
                var row = dataOrdenMuestraMaterial.Rows[i];

                if (i == 0)
                {
                    orden.codigoOrden = row["codigoOrden"].ToString();
                    orden.nroOficio = row["nroOficio"].ToString();
                    orden.nroDocumento = row["nroDocumento"].ToString();
                    orden.tipoDocumento = row["tipoDocumento"].ToString();
                    orden.nombrePaciente = row["nombrePaciente"] + " " + row["apellidoPaterno"] + " " + row["apellidoMaterno"];
                    orden.nombreEstablecimiento = row["nombreEstablecimiento"].ToString();
                    orden.idEstablecimiento = int.Parse(row["idEstablecimiento"].ToString());
                    orden.idOrden = Converter.GetGuid(row, "idOrden");
                    orden.codigoPaciente = row["codificacion"].ToString();
                    //orden.fechaNacimiento = DateTime.Parse(row["fechaNacimiento"].ToString());
                    orden.fechaNacimiento = (row["fechaNacimiento"]).ToString() != "" ? DateTime.Parse(row["fechaNacimiento"].ToString()) : DateTime.Parse("01/01/0001");
                    orden.fechaRegistro = DateTime.Parse(row["fechaRegistroOrden"].ToString());
                    orden.nombreProyecto = row["nombreProyecto"].ToString();
                    orden.edadPaciente = row["Edad"].ToString();
                }

                var muestra = new MuestraResultadoVd
                {
                    idOrdenMaterial = Converter.GetGuid(row, "idOrdenMaterial"),
                    idOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion"),
                    idTipoMuestra = int.Parse(row["idTipoMuestra"].ToString()),
                    tipoMuestra = row["nombreTipoMuestra"].ToString(),
                    presentacion = row["nombrePresentacion"].ToString(),
                    reactivo = row["nombreReactivo"].ToString(),
                    codificacion = row["codigoMuestra"].ToString(),
                    volumen = decimal.Parse(row["volumen"].ToString()),
                    fechaColeccion = DateTime.Parse(row["fechaColeccion"].ToString()),
                    fechaHoraColeccion = row["fechaHoraColeccion"].ToString(),
                    fechaHoraRecepcionLAB = row["fechaHoraRecepcionLAB"].ToString(),
                    fechaHoraRecepcionROM = row["fechaHoraRecepcionROM"].ToString(),
                    nombreExamen = row["ExamenNombre"].ToString(),
                    idExamen = Guid.Parse(row["idExamen"].ToString())
                };

                orden.muestrasValidar.Add(muestra);
            }

            return orden;
        }
        /// <summary>
        /// Descripción: Obtener informacion de muestras pendientes de ingreso de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd MuestrasPendientesRecepcionLAB(Guid idOrden, int idUsuario, int idEstablecimientoLogin)
        {
            var objCommand = GetSqlCommand("pNLS_MuestrasPendientesRecepcionLAB");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdEstablecimientoLogin", idEstablecimientoLogin);

            var dataOrdenMuestraMaterial = Execute(objCommand);

            var orden = new OrdenIngresarResultadoVd
            {
                muestrasPendientesRecepcion = new List<MuestraResultadoVd>()
            };

            if (dataOrdenMuestraMaterial.Rows.Count == 0)
                return orden;
            var x = 1;
            for (var i = 0; i < dataOrdenMuestraMaterial.Rows.Count; i++)
            {
                var row = dataOrdenMuestraMaterial.Rows[i];

                if (i == 0)
                {
                    orden.codigoOrden = row["codigoOrden"].ToString();
                    orden.nroOficio = row["nroOficio"].ToString();
                    orden.nroDocumento = row["nroDocumento"].ToString();
                    orden.tipoDocumento = row["tipoDocumento"].ToString();
                    orden.nombrePaciente = row["nombrePaciente"] + " " + row["apellidoPaterno"] + " " + row["apellidoMaterno"];
                    orden.nombreEstablecimiento = row["nombreEstablecimiento"].ToString();
                    orden.idEstablecimiento = int.Parse(row["idEstablecimiento"].ToString());
                    orden.idOrden = Converter.GetGuid(row, "idOrden");
                    orden.codigoPaciente = row["codificacion"].ToString();
                    //orden.fechaNacimiento = DateTime.Parse(row["fechaNacimiento"].ToString());
                    orden.fechaNacimiento = (row["fechaNacimiento"]).ToString() != "" ? DateTime.Parse(row["fechaNacimiento"].ToString()) : DateTime.Parse("01/01/0001");
                    orden.fechaRegistro = DateTime.Parse(row["fechaRegistroOrden"].ToString());
                    orden.nombreProyecto = row["nombreProyecto"].ToString();
                    orden.edadPaciente = row["Edad"].ToString();

                }

                var muestra = new MuestraResultadoVd
                {
                    idOrdenMaterial = Converter.GetGuid(row, "idOrdenMaterial"),
                    idOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion"),
                    idTipoMuestra = int.Parse(row["idTipoMuestra"].ToString()),
                    tipoMuestra = row["nombreTipoMuestra"].ToString(),
                    presentacion = row["nombrePresentacion"].ToString(),
                    reactivo = row["nombreReactivo"].ToString(),
                    codificacion = row["codigoMuestra"].ToString(),
                    volumen = decimal.Parse(row["volumen"].ToString()),
                    fechaHoraColeccion = row["fechaHoraColeccion"].ToString(),
                    nombreExamen = row["ExamenNombre"].ToString(),
                    ExamenComun = row["ExamenComun"].ToString(),
                    //AGREGADO SOTERO BUSTAMANTE ORDEN LLEGADA MUESTRA RECPCION 28/10/2017
                    secuenObtencion = int.Parse(row["secuenObtencion"].ToString()) + x,
                    idExamen = Guid.Parse(row["idExamen"].ToString())
                };

                orden.muestrasPendientesRecepcion.Add(muestra);
                x++;
            }

            return orden;
        }
        /// <summary>
        /// Descripción: Obtener información de muestras validadas para el ingreso de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimientoLogin"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd MuestrasValidadas(Guid idOrden, int idUsuario, int idEstablecimientoLogin)
        {
            var objCommand = GetSqlCommand("pNLS_MuestrasValidadas");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdEstablecimientoLogin", idEstablecimientoLogin);
           //InputParameterAdd.Int(objCommand, "existeRechazo", existeRechazo);

            var dataOrdenMuestraMaterial = Execute(objCommand);

            var orden = new OrdenIngresarResultadoVd
            {
                muestrasValidar = new List<MuestraResultadoVd>()
            };

            if (dataOrdenMuestraMaterial.Rows.Count == 0)
                return orden;

            for (var i = 0; i < dataOrdenMuestraMaterial.Rows.Count; i++)
            {
                var row = dataOrdenMuestraMaterial.Rows[i];

                if (i == 0)
                {
                    orden.codigoOrden = row["codigoOrden"].ToString();
                    orden.nroOficio = row["nroOficio"].ToString();
                    orden.nroDocumento = row["nroDocumento"].ToString();
                    orden.tipoDocumento = row["tipoDocumento"].ToString();
                    orden.nombrePaciente = row["nombrePaciente"] + " " + row["apellidoPaterno"] + " " + row["apellidoMaterno"];
                    orden.nombreEstablecimiento = row["nombreEstablecimiento"].ToString();
                    orden.idEstablecimiento = int.Parse(row["idEstablecimiento"].ToString());
                    orden.idOrden = Converter.GetGuid(row, "idOrden");
                    orden.codigoPaciente = row["codificacion"].ToString();
                    orden.fechaNacimiento = (row["fechaNacimiento"]).ToString() != "" ? DateTime.Parse(row["fechaNacimiento"].ToString()) : DateTime.Parse("01/01/0001");
                    orden.fechaRegistro = DateTime.Parse(row["fechaRegistroOrden"].ToString());
                    orden.nombreProyecto = row["nombreProyecto"].ToString();
                    orden.edadPaciente = row["Edad"].ToString();
                }

                var muestra = new MuestraResultadoVd
                {
                    idOrdenMaterial = Converter.GetGuid(row, "idOrdenMaterial"),
                    idOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion"),
                    idExamen = Converter.GetGuid(row, "idExamen"),
                    idTipoMuestra = int.Parse(row["idTipoMuestra"].ToString()),
                    tipoMuestra = row["nombreTipoMuestra"].ToString(),
                    presentacion = row["nombrePresentacion"].ToString(),
                    reactivo = row["nombreReactivo"].ToString(),
                    codificacion = row["codigoMuestra"].ToString(),
                    volumen = decimal.Parse(row["volumen"].ToString()),
                    fechaColeccion = DateTime.Parse(row["fechaColeccion"].ToString()),
                    fechaHoraColeccion = row["fechaHoraColeccion"].ToString(),
                    fechaHoraRecepcionLAB = row["fechaHoraRecepcionLAB"].ToString(),
                    fechaHoraRecepcionROM = row["fechaHoraRecepcionROM"].ToString(),
                    muestraConforme = row["MuestraConforme"].ToString(),
                    criteriosRechazo = row["criteriosRechazo"].ToString(),
                    observacionrechazo = row["ObservacionRechazo"].ToString()
                };

                orden.muestrasValidar.Add(muestra);
            }

            return orden;
        }

        /// <summary>
        /// Descripción: Obtener información del area de procesamiento a la que pertence una orden.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd AreaProcesamientoOrdenResultados(Guid idOrdenExamen, int idUsuario, int idEstablecimientoSeleccionado)
        {
            var objCommand = GetSqlCommand("pNLS_AreaProcesamientoUsuarioOrden");
            InputParameterAdd.Guid(objCommand, "IdOrdenExamen", idOrdenExamen);
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            DataTable dataOrdenMuestraMaterial = Execute(objCommand);

            if (dataOrdenMuestraMaterial.Rows.Count == 0)
                return null;

            OrdenIngresarResultadoVd orden = new OrdenIngresarResultadoVd();
            orden.areas = new List<AreaProcesamiento>();

            for (int i = 0; i < dataOrdenMuestraMaterial.Rows.Count; i++)
            {
                var row = dataOrdenMuestraMaterial.Rows[i];
                orden.idOrden = Converter.GetGuid(row, "idOrden");
                orden.idOrdenExamen = Converter.GetGuid(row, "idOrdenExamen");
                orden.codigoOrden = row["codigoOrden"].ToString();
                orden.nroOficio = row["nroOficio"].ToString();
                orden.fechaRegistro = DateTime.Parse(row["fechaRegistroOrden"].ToString());
                orden.idExamen = Guid.Parse(row["idExamen"].ToString());
                AreaProcesamiento area = new AreaProcesamiento();
                area.IdAreaProcesamiento = int.Parse(row["idAreaProcesamiento"].ToString());
                area.Nombre = row["nombreAP"].ToString();
                orden.areas.Add(area);

                //if (Guid.Parse(row["idExamen"].ToString()) == idExamen)
                //{
                //    orden.idOrden = Converter.GetGuid(row, "idOrden");
                //    orden.idOrdenExamen = Converter.GetGuid(row, "idOrdenExamen");
                //    orden.codigoOrden = row["codigoOrden"].ToString();
                //    orden.nroOficio = row["nroOficio"].ToString();
                //    orden.fechaRegistro = DateTime.Parse(row["fechaRegistroOrden"].ToString());
                //    orden.idExamen = Guid.Parse(row["idExamen"].ToString());

                //    AreaProcesamiento area = new AreaProcesamiento();
                //    area.IdAreaProcesamiento = int.Parse(row["idAreaProcesamiento"].ToString());
                //    area.Nombre = row["nombreAP"].ToString();
                //    orden.areas.Add(area);
                //}
                //if (idExamen == null || idExamen == Guid.Empty)                
                //{
                //    orden.idOrden = Converter.GetGuid(row, "idOrden");
                //    orden.idOrdenExamen = Converter.GetGuid(row, "idOrdenExamen");
                //    orden.codigoOrden = row["codigoOrden"].ToString();
                //    orden.nroOficio = row["nroOficio"].ToString();
                //    orden.fechaRegistro = DateTime.Parse(row["fechaRegistroOrden"].ToString());
                //    orden.idExamen = Guid.Parse(row["idExamen"].ToString());

                //    AreaProcesamiento area = new AreaProcesamiento();
                //    area.IdAreaProcesamiento = int.Parse(row["idAreaProcesamiento"].ToString());
                //    area.Nombre = row["nombreAP"].ToString();
                //    orden.areas.Add(area);
                //}
            }

            return orden;
        }
        /// <summary>
        /// Descripción: Obtener informacion de la orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public OrdenIngresarResultadoVd DatosOrden(Guid idOrden)
        {
            var objCommand = GetSqlCommand("pNLS_DatosOrden");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);

            var dataOrdenMuestraMaterial = Execute(objCommand);

            if (dataOrdenMuestraMaterial.Rows.Count == 0)
                return null;

            var orden = new OrdenIngresarResultadoVd();

            for (var i = 0; i < dataOrdenMuestraMaterial.Rows.Count; i++)
            {
                var row = dataOrdenMuestraMaterial.Rows[i];

                if (i != 0) continue;

                orden.codigoOrden = row["codigoOrden"].ToString();
                orden.nroOficio = row["nroOficio"].ToString();
                orden.nroDocumento = row["nroDocumento"].ToString();
                orden.nombrePaciente = row["nombrePaciente"] + " " + row["apellidoPaterno"] + " " + row["apellidoMaterno"];
                orden.nombreEstablecimiento = row["nombreEstablecimiento"].ToString();
                orden.idEstablecimiento = int.Parse(row["idEstablecimiento"].ToString());
                orden.idOrden = Converter.GetGuid(row, "idOrden");
                orden.codigoPaciente = row["codificacion"].ToString();
                //orden.fechaNacimiento = DateTime.Parse(row["fechaNacimiento"].ToString());
                orden.fechaNacimiento = (row["fechaNacimiento"]).ToString() != "" ? DateTime.Parse(row["fechaNacimiento"].ToString()) : DateTime.Parse("01/01/0001");
                orden.fechaRegistro = DateTime.Parse(row["fechaRegistroOrden"].ToString());
                orden.nombreProyecto = row["nombreProyecto"].ToString();
                orden.Genero = int.Parse(row["genero"].ToString());
                orden.edadPaciente = row["Edad"].ToString();

            }

            return orden;
        }
        /// <summary>
        /// Descripción: Obtener informacion de los resultados de las pruebas para validar el mismo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idAreaProcesamiento"></param>
        /// <param name="idUsuario"></param>
        /// <param name="edad"></param>
        /// <param name="genero"></param>
        /// <param name="idLaboratorioUsuario"></param>
        /// <returns></returns>
        public List<OrdenExamenResultadosVd> MuestraResultadosValidar(Guid idOrden, Guid idOrdenExamen2, int idAreaProcesamiento, int idUsuario,
                                                                      int edad, int genero, int idLaboratorioUsuario)
        {
            var lista = new List<OrdenExamenResultadosVd>();

            var objCommand = GetSqlCommand("pNLS_OrdenIngresoResultadosAPUsuario");
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdAreaProcesamiento", idAreaProcesamiento);
            InputParameterAdd.Int(objCommand, "EdadPaciente", edad);
            InputParameterAdd.Int(objCommand, "GeneroPaciente", genero);
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", idOrdenExamen2);
            InputParameterAdd.Int(objCommand, "IdLaboratorioUsuario", idLaboratorioUsuario);

            var data = Execute(objCommand);

            if (data.Rows.Count == 0)
                return null;

            var examen = new OrdenExamenResultadosVd { IdOrdenExamen = new Guid() };

            //a.idAnalito, a.nombre as Analito,uni.glosa unidad, am.glosa1 Metodo, av.glosa ValorNormal, av.valorInferior,av.valorSuperior,
            //omr.idOrdenMuestraRecepcion, mc.idMuestraCod 

            for (var i = 0; i < data.Rows.Count; i++)
            {
                var row = data.Rows[i];

                var idOrdenExamen = Converter.GetGuid(row, "idOrdenExamen");

                if (idOrdenExamen != examen.IdOrdenExamen)
                {
                    if (i > 0)
                    {
                        lista.Add(examen);
                    }

                    examen = new OrdenExamenResultadosVd
                    {
                        Item = new Examen
                        {
                            idExamen = Converter.GetGuid(row, "idExamen"),
                            nombre = row["Examen"].ToString(),
                            descripcion= row["descripcion"].ToString()
                        },
                        Muestra = new MuestraResultadoVd
                        {
                            idOrdenMaterial = Converter.GetGuid(row, "idOrdenMaterial"),
                            idTipoMuestra = int.Parse(row["idTipoMuestra"].ToString()),
                            tipoMuestra = row["nombreTipoMuestra"].ToString(),
                            presentacion = row["nombrePresentacion"].ToString(),
                            reactivo = row["nombreReactivo"].ToString(),
                            volumen = decimal.Parse(row["volumen"].ToString()),
                            fechaColeccion =
                                Converter.GetDateTimeFromDateAndTime(row, "fechaColeccion", "horaColeccion"),
                            fechaRecepcion =
                                Converter.GetDateTimeFromDateAndTime(row, "fechaRecepcion", "horaRecepcion"),
                            fechaRecepcionP =
                                Converter.GetDateTimeFromDateAndTime(row, "fechaRecepcionP", "horaRecepcionP"),
                            MotivoNoConforme = Converter.GetString(row, "motivoNoConforme")
                        },
                        IdOrdenExamen = idOrdenExamen,
                        CodigoBarra = row["CodigoBarra"].ToString(),//Sotero 03/07/2019 Generacion Codigo de Barra.
                        CodigoMuestra = row["codificacion"].ToString(),
                        IdExamenMetodo = -1,
                        NombreEnfermedad = row["nombreEnfermedad"].ToString(),
                        estatusE = Converter.GetInt(row, "estatusE"),
                        idPlataforma = Converter.GetInt(row, "idPlataforma")
                    };

                    if (row["idExamenMetodo"].ToString() != "")
                    {
                        examen.IdExamenMetodo = int.Parse(row["idExamenMetodo"].ToString());
                    }

                    var objCommandMetodos = GetSqlCommand("pNLS_MetodosByExamen");
                    InputParameterAdd.Guid(objCommandMetodos, "idExamen", examen.Item.idExamen);

                    var dataMetodos = Execute(objCommandMetodos);

                    examen.Metodos = new List<ExamenMetodo>();

                    for (var j = 0; j < dataMetodos.Rows.Count; j++)
                    {
                        var rowMetodo = dataMetodos.Rows[j];
                        var estado = int.Parse(rowMetodo["estado"].ToString());
                        if (estado == 0) continue;

                        var metodo = new ExamenMetodo
                        {
                            IdExamen = examen.Item.idExamen,
                            IdExamenMetodo = int.Parse(rowMetodo["idExamenMetodo"].ToString()),
                            Glosa = rowMetodo["glosa"].ToString(),
                            Orden = int.Parse(rowMetodo["ordenMetodo"].ToString())
                        };

                        examen.Metodos.Add(metodo);
                    }

                    examen.Plataformas = new List<ExamenPlataforma>();
                    var objCommandPlataforma = GetSqlCommand("pNLS_ListaPlataformaExamen");
                    InputParameterAdd.Guid(objCommandPlataforma, "idExamen", examen.Item.idExamen);
                    string plataforma = objCommandPlataforma.ExecuteScalar().ToString();
                    if (plataforma.Length > 0)
                    {
                        var filas = plataforma.Split('|');
                        for (int k = 0; k < filas.Length; k++)
                        {
                            var exp = new ExamenPlataforma();
                            var campo = filas[k].Split(',');
                            exp.idPlataforma = Convert.ToInt32(campo[0]);
                            exp.Plataforma = campo[1].ToString();
                            examen.Plataformas.Add(exp);
                        }
                    }
                    examen.AnalitosValidar = new List<OrdenResultadoAnalitoVd>();
                }

                var analito = new OrdenResultadoAnalitoVd
                {
                    OItem = new OrdenResultadoAnalito
                    {
                        idOrdenResultadoAnalisis = Converter.GetGuid(row, "idOrdenResultadoAnalito")
                    },
                    Item = new Analito
                    {
                        IdAnalito = Converter.GetGuid(row, "idAnalito"),
                        Nombre = row["Analito"].ToString()
                    },
                    VItem = new AnalitoValorNormal()
                };

                var valorSuperior = "";
                var valorInferior = "";
                var valorNormal = "-";

                if (row["valorSuperior"] != null) valorSuperior = row["valorSuperior"].ToString();
                if (row["valorInferior"] != null) valorInferior = row["valorInferior"].ToString();
                if (row["ValorNormal"] != null) valorNormal = row["ValorNormal"].ToString();

                if (valorSuperior.Trim() != "") { analito.VItem.valorSuperior = decimal.Parse(valorSuperior); }
                else { analito.VItem.valorSuperior = -1; }
                if (valorInferior.Trim() != "") { analito.VItem.valorInferior = decimal.Parse(valorInferior); }
                else { analito.VItem.valorInferior = -1; }

                analito.VItem.glosa = valorNormal;
                analito.Item.Tipo = Converter.GetInt(row, "TipoAnalito");

                if (analito.Item.Tipo == 3)
                {
                    var opcionDal = new AnalitoOpcionDal();
                    analito.Opciones = opcionDal.GetAnalitoOpcionByAnalito(analito.Item.IdAnalito);
                }

                analito.unidad = row["unidad"].ToString();

                analito.OItem.resultado = row["resultado"].ToString();
                analito.OItem.observacion = row["observacion"].ToString();

                analito.OItem.ingresado = 0;
                if (row["ingresado"].ToString() != "")
                {
                    analito.OItem.ingresado = int.Parse(row["ingresado"].ToString());
                }

                analito.OItem.validado = 0;
                if (row["validado"].ToString() != "")
                {
                    analito.OItem.validado = int.Parse(row["validado"].ToString());
                }

                examen.AnalitosValidar.Add(analito);
            }

            if (data.Rows.Count > 0)
            {
                lista.Add(examen);
            }

            return lista;
        }
        /// <summary>
        /// Descripción: Registra los resultados de la prueba ejecutada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="resultados"></param>
        /// <param name="idUsuario"></param>
        public void RegistrarResultadosOrdenAnalito(List<OrdenResultadoAnalito> resultados, int idUsuario)
        {
            try
            {
                foreach (OrdenResultadoAnalito item in resultados)
                {
                    var objCommand = GetSqlCommand("pNLU_OrdenResultadoAnalitoResultado");
                    InputParameterAdd.Guid(objCommand, "IdOrdenResultadoAnalito", item.idOrdenResultadoAnalisis);
                    InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
                    InputParameterAdd.Int(objCommand, "idExamenMetodo", item.idExamenMetodo);
                    InputParameterAdd.Varchar(objCommand, "Observacion", item.observacion);
                    InputParameterAdd.Varchar(objCommand, "Resultado", item.resultado);
                    InputParameterAdd.Varchar(objCommand, "CodigoOpcion", item.codigoOpcion);
                    InputParameterAdd.Varchar(objCommand, "convResultado", item.convResultado);
                    InputParameterAdd.Varchar(objCommand, "interpretacion", item.interpretacion);
                    InputParameterAdd.Int(objCommand, "RegistraryFinalizar", item.RegistroyFinalizacion);
                    InputParameterAdd.Int(objCommand, "idPlataforma", item.Plataforma);
                    ExecuteNonQuery(objCommand);
                }
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab");
            }
            
        }
        /// <summary>
        /// Descripción: Registra los resultados de la prueba ejecutada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenMuestraRecepcion"></param>
        /// <param name="conforme"></param>
        /// <param name="idUsuario"></param>
        public void UpdateEstadoRecepcionResultado(Guid idOrdenMuestraRecepcion, int conforme, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenMuestraRecepcionResultado");
            InputParameterAdd.Guid(objCommand, "idOrdenMuestraRecepcion", idOrdenMuestraRecepcion);
            InputParameterAdd.Int(objCommand, "conformeP", conforme);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Registra los recepcion de la muestra en Labortorio 
        /// Estado = 5
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenMuestraRecepcion"></param>
        /// <param name="idUsuario"></param>
        public void UpdateOrdenMuestraRecepcionLAB(Guid idOrdenMuestraRecepcion, int idUsuario, int secuenciaObtencion)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenMuestraRecepcionLAB");
            InputParameterAdd.Guid(objCommand, "idOrdenMuestraRecepcion", idOrdenMuestraRecepcion);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "secuenObtencion", secuenciaObtencion);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Registra los resultados rechazados por cada prueba en Labortorio 
        /// Estado = 1
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="criterioRechazo"></param>
        public void InsertOrdenMuestraResultadoRechazo(CriterioRechazo criterioRechazo)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMuestraResultadoRechazo");
            InputParameterAdd.Guid(objCommand, "idOrdenMuestraRecepcion", criterioRechazo.IdOrdenMuestraRecepcion);
            InputParameterAdd.Int(objCommand, "idCriterioRechazo", criterioRechazo.IdCriterioRechazo);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", criterioRechazo.IdUsuarioRegistro);
            InputParameterAdd.Varchar(objCommand, "ObservacionRechazo", criterioRechazo.observacionrechazo);
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Registra los resultados rechazados por cada prueba en Labortorio 
        /// Estado = 1
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaOrdenMuestraRecepcion"></param>
        public void EditarLaboratoriosMuestras(List<OrdenMuestraRecepcion> listaOrdenMuestraRecepcion)
        {
            if (listaOrdenMuestraRecepcion != null)
            {
                foreach (OrdenMuestraRecepcion item in listaOrdenMuestraRecepcion)
                {
                    var objCommand = GetSqlCommand("pNLU_LaboratorioDestinoOrdenMuestraRecepcion");
                    InputParameterAdd.Guid(objCommand, "idOrdenMuestraRecepcion", item.idOrdenMuestraRecepcion);
                    InputParameterAdd.Int(objCommand, "idLaboratorio", item.idLaboratorioDestino);
                    ExecuteNonQuery(objCommand);
                }
            }
            return;
        }
        /// <summary>
        /// Descripción: Obtiene informacion del laboratorio de un usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Laboratorio ObtenerLaboratorioUsuario(int idUsuario)
        {
            Laboratorio laboratorioTmp = new Laboratorio();
            var objCommand = GetSqlCommand("pNLS_LaboratorioUsuario");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);

            DataTable dataLaboratorio = Execute(objCommand);

            if (dataLaboratorio.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataLaboratorio.Rows.Count; i++)
            {
                var row = dataLaboratorio.Rows[i];

                laboratorioTmp.IdLaboratorio = int.Parse(row["idLaboratorio"].ToString());
                laboratorioTmp.Nombre = row["nombre"].ToString();
            }

            return laboratorioTmp;
        }

        /// <summary>
        /// Descripción: Obtiene el Id-Guid del resultado de un examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public List<Guid> ObtenerResultadoAnalito(Guid idOrdenExamen)
        {
            var objCommand = GetSqlCommand("pNLS_OrdenResultadoAnalitoByOrdenExamen");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", idOrdenExamen);

            var resultadoAnalito = Execute(objCommand);

            return (from DataRow row
                    in resultadoAnalito.Rows
                    select Converter.GetGuid(row, "idOrdenResultadoAnalito")).
                    ToList();
        }

        /// <summary>
        /// Descripción: Solicitud nuevo Ingreso Resultados
        /// Estado = 5
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 21/10/2017
        /// Fecha Modificación: 21/10/2017.
        /// Modificación: SOLICITUD INGRESO NUEVO RESULTDOS POR VERIFICADOR.
        /// </summary>
        /// <param name="idOrdenMuestraRecepcion"></param>
        /// <param name="idUsuario"></param>
        /// /// <param name="estatusSol"></param>
        public void SolicitaNvoIngresoResultados(Guid idOrdenExamen, int idUsuario, int estatusSol)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenSolicitaIngresoResultados");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", idOrdenExamen);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "estatusSol", estatusSol);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Permite obtener información de los materiales ingresados para una orden para el detalle.
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<OrdenRecepcionDetalleVd> MuestrasByOrdenDetalle(int idEstablecimientoUsuario)
        {
            List<OrdenRecepcionDetalleVd> LstOrdenRecepcionDetalle = new List<OrdenRecepcionDetalleVd>();

            var objCommand = GetSqlCommand("pNLS_DetalleOrdenRecepcion");
            InputParameterAdd.Int(objCommand, "IdLaboratorioUsuario", idEstablecimientoUsuario);
            DataTable DataOrdenRecepcionDetalle = Execute(objCommand);

            if (DataOrdenRecepcionDetalle.Rows.Count == 0)
                return null;

            for (int i = 0; i < DataOrdenRecepcionDetalle.Rows.Count; i++)
            {
                var row = DataOrdenRecepcionDetalle.Rows[i];
                OrdenRecepcionDetalleVd BEOrdenRecepcionDetalle = new OrdenRecepcionDetalleVd();

                if (!row.IsNull("IdOrden"))
                    BEOrdenRecepcionDetalle.idOrden = Guid.Parse(row["IdOrden"].ToString());
                if (!row.IsNull("idOrdenMuestraRecepcion"))
                    BEOrdenRecepcionDetalle.CodigoOrdenMuestraRecepcion = Guid.Parse(row["idOrdenMuestraRecepcion"].ToString());

                if (!row.IsNull("FechaSolicitud"))
                    BEOrdenRecepcionDetalle.FechaSolicitud = row["FechaSolicitud"].ToString();

                else
                    BEOrdenRecepcionDetalle.FechaSolicitud = string.Empty;

                if (!row.IsNull("FechaObtencion"))
                    BEOrdenRecepcionDetalle.FechaObtencion = row["FechaObtencion"].ToString();
                else
                    BEOrdenRecepcionDetalle.FechaObtencion = string.Empty;
                if (!row.IsNull("Hora"))
                    BEOrdenRecepcionDetalle.HoraObtencion = DateTime.Parse(row["hora"].ToString()).ToString("HH:mm");
                else
                    BEOrdenRecepcionDetalle.HoraObtencion = string.Empty;
                if (!row.IsNull("CodigoOrden"))
                    BEOrdenRecepcionDetalle.codigoOrden = row["CodigoOrden"].ToString();
                else
                    BEOrdenRecepcionDetalle.codigoOrden = string.Empty;
                if (!row.IsNull("EstablecimientoOrigen"))
                    BEOrdenRecepcionDetalle.EstablecimientoOrigen = row["EstablecimientoOrigen"].ToString();
                else
                    BEOrdenRecepcionDetalle.EstablecimientoOrigen = string.Empty;
                if (!row.IsNull("CodigoMuestra"))
                    BEOrdenRecepcionDetalle.codigoMuestra = row["CodigoMuestra"].ToString();
                else
                    BEOrdenRecepcionDetalle.codigoMuestra = string.Empty;
                if (!row.IsNull("Enfermedad"))
                    BEOrdenRecepcionDetalle.Enfermedad = row["Enfermedad"].ToString();
                else
                    BEOrdenRecepcionDetalle.Enfermedad = string.Empty;
                if (!row.IsNull("Examen"))
                    BEOrdenRecepcionDetalle.Examen = row["Examen"].ToString();
                else
                    BEOrdenRecepcionDetalle.Examen = string.Empty;
                if (!row.IsNull("TipoMuestra"))
                    BEOrdenRecepcionDetalle.TipoMuestra = row["TipoMuestra"].ToString();
                else
                    BEOrdenRecepcionDetalle.TipoMuestra = string.Empty;
                if (!row.IsNull("Tipo"))
                    BEOrdenRecepcionDetalle.Tipo = row["Tipo"].ToString();
                else
                    BEOrdenRecepcionDetalle.Tipo = string.Empty;

                //if (!row.IsNull("EXISTE_PENDIENTE"))
                //    BEOrdenRecepcionDetalle.ExistePendiente = int.Parse(row["EXISTE_PENDIENTE"].ToString());
                //else
                //    BEOrdenRecepcionDetalle.ExistePendiente = 0;

                //if (!row.IsNull("EXISTE_RECIBIDO"))
                //    BEOrdenRecepcionDetalle.ExisteRecibido = int.Parse(row["EXISTE_RECIBIDO"].ToString());
                //else
                //    BEOrdenRecepcionDetalle.ExisteRecibido = 0;
                //if (!row.IsNull("EXISTE_RECHAZO"))
                //    BEOrdenRecepcionDetalle.ExisteRechazo = int.Parse(row["EXISTE_RECHAZO"].ToString());
                //else
                //    BEOrdenRecepcionDetalle.ExisteRechazo = 0;
                if (!row.IsNull("estatus"))
                    BEOrdenRecepcionDetalle.estatus = row["estatus"].ToString();
                else
                    BEOrdenRecepcionDetalle.estatus = string.Empty;
                if (!row.IsNull("conformeR"))
                    BEOrdenRecepcionDetalle.ConformeR = row["conformeR"].ToString();
                else
                    BEOrdenRecepcionDetalle.ConformeR = string.Empty;
                if (!row.IsNull("idLaboratorioDestino"))
                    BEOrdenRecepcionDetalle.IdLaboratorioDestino = int.Parse(row["idLaboratorioDestino"].ToString());
                else
                    BEOrdenRecepcionDetalle.IdLaboratorioDestino = 0;
                if (!row.IsNull("fecharecepcion"))
                    BEOrdenRecepcionDetalle.FechaRecepcion = row["fecharecepcion"].ToString();
                else
                    BEOrdenRecepcionDetalle.FechaRecepcion = string.Empty;
                if (!row.IsNull("horarecepcion"))
                    BEOrdenRecepcionDetalle.HoraRecepcion = DateTime.Parse(row["horarecepcion"].ToString()).ToString("HH:mm");
                else
                    BEOrdenRecepcionDetalle.HoraRecepcion = string.Empty;
                if (!row.IsNull("EstablecimientoDestino"))
                    BEOrdenRecepcionDetalle.EstablecimientoDestino = row["EstablecimientoDestino"].ToString();
                else
                    BEOrdenRecepcionDetalle.EstablecimientoDestino = string.Empty;
                if (!row.IsNull("idMuestraCod"))
                    BEOrdenRecepcionDetalle.IdMuestraCod = row["idMuestraCod"].ToString();
                else
                    BEOrdenRecepcionDetalle.IdMuestraCod = string.Empty;

                LstOrdenRecepcionDetalle.Add(BEOrdenRecepcionDetalle);
            }

            return LstOrdenRecepcionDetalle;
        }



        public List<OrdenRecepcionVd> GetOrdenMuestraRechazar(string lista)
        {
            List<OrdenRecepcionVd> listaOrdenes = new List<OrdenRecepcionVd>();

            var objCommand = GetSqlCommand("pNLS_OrdenMuestraRechazada");

            InputParameterAdd.Varchar(objCommand, "sLista", lista);

            DataTable dataOrdenMuestraRechazada = Execute(objCommand);
            String nroDoc = "";
            int tipoOrden = 0;

            if (dataOrdenMuestraRechazada.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataOrdenMuestraRechazada.Rows.Count; i++)
            {
                var row = dataOrdenMuestraRechazada.Rows[i];
                OrdenRecepcionVd ordenRecepcion = new OrdenRecepcionVd();

                ordenRecepcion.nroOficio = row["nroOficio"].ToString();

                if (tipoOrden != 0)
                {
                    if (!row.IsNull("nroDocumento"))
                        nroDoc = row["nroDocumento"].ToString();
                    ordenRecepcion.nroDocumento = nroDoc;

                    switch (tipoOrden)
                    {
                        case 1:
                            ordenRecepcion.tipoOrden = "Persona";
                            break;
                        case 2:
                            ordenRecepcion.tipoOrden = "Animal";
                            break;
                        case 3:
                            ordenRecepcion.tipoOrden = "Cepa/Banco Sangre";
                            break;
                    }
                }
                else
                {
                    if (!row.IsNull("nroDocPaciente"))
                        ordenRecepcion.nroDocPaciente = row["nroDocPaciente"].ToString();
                    else
                        ordenRecepcion.nroDocPaciente = "";

                    if (!row.IsNull("nroDocAnimal"))
                        ordenRecepcion.nroDocAnimal = row["nroDocAnimal"].ToString();
                    else
                        ordenRecepcion.nroDocAnimal = "";

                    if (!row.IsNull("nroDocCepaBanco"))
                        ordenRecepcion.nroDocCepaBanco = row["nroDocCepaBanco"].ToString();
                    else
                        ordenRecepcion.nroDocCepaBanco = "";

                    if (ordenRecepcion.nroDocPaciente != "")
                    {
                        ordenRecepcion.tipoOrden = "Persona";
                        ordenRecepcion.nroDocumento = ordenRecepcion.nroDocPaciente;
                    }
                    else if (ordenRecepcion.nroDocAnimal != "")
                    {
                        ordenRecepcion.tipoOrden = "Animal";
                        ordenRecepcion.nroDocumento = ordenRecepcion.nroDocAnimal;
                    }
                    else if (ordenRecepcion.nroDocCepaBanco != "")
                    {
                        ordenRecepcion.tipoOrden = "Cepa/Banco Sangre";
                        ordenRecepcion.nroDocumento = ordenRecepcion.nroDocCepaBanco;
                    }
                }

                if (!row.IsNull("fechaSolicitud"))
                {
                    ordenRecepcion.fechaSolicitud = DateTime.Parse(row["fechaSolicitud"].ToString());
                    ordenRecepcion.fechaSolicitudStr = ordenRecepcion.fechaSolicitud.ToShortDateString();
                }
                else
                    ordenRecepcion.fechaSolicitud = new DateTime();


                if (!row.IsNull("nombre"))
                {
                    ordenRecepcion.nombreEstablecimiento = row["nombre"].ToString();
                }
                else
                    ordenRecepcion.nombreEstablecimiento = "";

                if (!row.IsNull("genero"))
                    ordenRecepcion.genero = int.Parse(row["genero"].ToString());
                else
                    ordenRecepcion.genero = 1;
                //nuevas columnas
                if (!row.IsNull("nombreGenero"))
                    ordenRecepcion.nombreGenero = row["nombreGenero"].ToString();
                else
                    ordenRecepcion.nombreGenero = "";

                if (!row.IsNull("tipoDocumento"))
                    ordenRecepcion.tipoDocumento = row["tipoDocumento"].ToString();
                else
                    ordenRecepcion.tipoDocumento = "";

                if (!row.IsNull("fechaNacimiento"))
                {
                    ordenRecepcion.fechaNacimiento = DateTime.Parse(row["fechaNacimiento"].ToString());
                }
                else
                    ordenRecepcion.fechaNacimiento = null;


                listaOrdenes.Add(ordenRecepcion);
            }

            return listaOrdenes;
        }

        /// Descripción: Metodo para obtener Muestras Rechazadas por ROM.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 01/12/2017
        public List<OrdenRecepcionVd> GetOrdenMuestraByEstablecimientoRechazado(int estadoOrden, int fechaSolicitud, int idUsuarioLogueado, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, int tipoOrden, string idMuestra, int idLaboratorio)
        {
            List<OrdenRecepcionVd> listaOrdenes = new List<OrdenRecepcionVd>();

            //var objCommand = GetSqlCommand("pNLS_OrdenMuestraByEstablecimientoRechazado");
            var objCommand = GetSqlCommand("pNLS_OrdenMuestraByEstablecimientoRechazado2");

            InputParameterAdd.Int(objCommand, "fechaSolicitud", fechaSolicitud);
            //InputParameterAdd.Int(objCommand, "idUsuarioLogueado", idUsuarioLogueado);
            InputParameterAdd.Varchar(objCommand, "codigoOrden", codigoOrden);
            InputParameterAdd.DateTime(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Varchar(objCommand, "nroOficio", nroOficio);
            //InputParameterAdd.Int(objCommand, "tipoOrden", 1);
            InputParameterAdd.Int(objCommand, "estatus", estadoOrden);
            InputParameterAdd.Varchar(objCommand, "idMuestra", idMuestra);
            InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);


            DataTable dataOrdenMuestra = Execute(objCommand);
            //String nroDoc = "";
            if (dataOrdenMuestra.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataOrdenMuestra.Rows.Count; i++)
            {
                var row = dataOrdenMuestra.Rows[i];
                OrdenRecepcionVd ordenRecepcion = new OrdenRecepcionVd();

                ordenRecepcion.paciente = row["Paciente"].ToString();
                ordenRecepcion.nroDocPaciente = row["DocumentoIdenidad"].ToString();
                ordenRecepcion.nroOficio = row["nroOficio"].ToString();
                ordenRecepcion.codigo = row["CodigoOrden"].ToString();
                ordenRecepcion.fechaSolicitudStr = row["fechaSolicitud"].ToString();
                ordenRecepcion.nombreEstablecimiento = row["EESSOrigen"].ToString();
                ordenRecepcion.nombreEstablecimientoDestino = row["EESSDestino"].ToString();
                ordenRecepcion.fechaRegistroStr = row["FechaRechazo"].ToString();
                ordenRecepcion.fechaColeccion = row["FechaTomaMuestra"].ToString();
                ordenRecepcion.examen = row["Examen"].ToString();
                ordenRecepcion.criterioRechazo = row["CriteriosRechazo"].ToString();
                ordenRecepcion.codigoMuestra = row["CodigoMuestra"].ToString();
                ordenRecepcion.EstadoRechazo = row["EstatusResultado"].ToString();
                ordenRecepcion.usuario = row["UsuarioRechazo"].ToString();

                listaOrdenes.Add(ordenRecepcion);
            }


            //for (int i = 0; i < dataOrdenMuestra.Rows.Count; i++)
            //{
            //    var row = dataOrdenMuestra.Rows[i];
            //    OrdenRecepcionVd ordenRecepcion = new OrdenRecepcionVd();
            //    ordenRecepcion.idOrden = Converter.GetGuid(row, "idOrden");
            //    ordenRecepcion.codigo = row["codigoOrden"].ToString();
            //    ordenRecepcion.nroOficio = row["nroOficio"].ToString();

            //    if (tipoOrden != 0)
            //    {
            //        if (!row.IsNull("nroDocumento"))
            //            nroDoc = row["nroDocumento"].ToString();
            //        ordenRecepcion.nroDocumento = nroDoc;

            //        switch (tipoOrden)
            //        {
            //            case 1:
            //                ordenRecepcion.tipoOrden = "Persona";
            //                break;
            //            case 2:
            //                ordenRecepcion.tipoOrden = "Animal";
            //                break;
            //            case 3:
            //                ordenRecepcion.tipoOrden = "Cepa/Banco Sangre";
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        if (!row.IsNull("nroDocPaciente"))
            //            ordenRecepcion.nroDocPaciente = row["nroDocPaciente"].ToString();
            //        else
            //            ordenRecepcion.nroDocPaciente = "";
            //        if (ordenRecepcion.nroDocPaciente != "")
            //        {
            //            ordenRecepcion.tipoOrden = "Persona";
            //            ordenRecepcion.nroDocumento = ordenRecepcion.nroDocPaciente;
            //        }
            //        else if (ordenRecepcion.nroDocAnimal != "")
            //        {
            //            ordenRecepcion.tipoOrden = "Animal";
            //            ordenRecepcion.nroDocumento = ordenRecepcion.nroDocAnimal;
            //        }
            //        else if (ordenRecepcion.nroDocCepaBanco != "")
            //        {
            //            ordenRecepcion.tipoOrden = "Cepa/Banco Sangre";
            //            ordenRecepcion.nroDocumento = ordenRecepcion.nroDocCepaBanco;
            //        }
            //    }

            //    if (!row.IsNull("fechaSolicitud"))
            //    {
            //        ordenRecepcion.fechaSolicitud = DateTime.Parse(row["fechaSolicitud"].ToString());
            //        ordenRecepcion.fechaSolicitudStr = ordenRecepcion.fechaSolicitud.ToShortDateString();
            //    }
            //    else
            //        ordenRecepcion.fechaSolicitud = new DateTime();


            //    if (!row.IsNull("nombreEstablecimiento"))
            //    {
            //        ordenRecepcion.nombreEstablecimiento = row["nombreEstablecimiento"].ToString();
            //    }
            //    else
            //        ordenRecepcion.nombreEstablecimiento = "";

            //    //nuevas columnas
            //    if (!row.IsNull("nombreGenero"))
            //        ordenRecepcion.nombreGenero = row["nombreGenero"].ToString();
            //    else
            //        ordenRecepcion.nombreGenero = "";


            //    if (!row.IsNull("fechaNacimiento"))
            //    {
            //        ordenRecepcion.fechaNacimiento = DateTime.Parse(row["fechaNacimiento"].ToString());
            //    }
            //    else
            //        ordenRecepcion.fechaNacimiento = null;

            //    listaOrdenes.Add(ordenRecepcion);
            //}

            return listaOrdenes;
        }
        /// <summary>
        /// Descripción: Permite obtener información de las muestras rechazadas.
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<OrdenMuestrasRechazo> GetOrdenMuestraRechazobyIdOrden(Guid idOrden)
        {
            var listaOrdenMuestraRechazo = new List<OrdenMuestrasRechazo>();

            var objCommand = GetSqlCommand("pNLS_OrdenMuestraRechazo");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);

            var dataOrdenMuestraRechazo = Execute(objCommand);

            if (dataOrdenMuestraRechazo.Rows.Count == 0)
                return null;

            for (var i = 0; i < dataOrdenMuestraRechazo.Rows.Count; i++)
            {
                var row = dataOrdenMuestraRechazo.Rows[i];
                var ordenmuestraRechazo = new OrdenMuestrasRechazo
                {
                    idOrden = Guid.Parse(row["idorden"].ToString()),
                    nombreEstablecimiento = row["Establecimiento"].ToString(),
                    CriterioRechazo = row["CriterioRechazo"].ToString(),
                    nombreTipoMuestra = row["TipoMuestra"].ToString(),
                    nombrePaciente = row["Paciente"].ToString(),
                    nombreUsuario = row["Usuario"].ToString(),
                    fechaRechazo = DateTime.Parse(row["FechaRechazo"].ToString()),
                    Codificacion = row["Codificacion"].ToString(),
                    nombreExamen = row["Examen"].ToString()
                };
                listaOrdenMuestraRechazo.Add(ordenmuestraRechazo);
            }

            return listaOrdenMuestraRechazo;
        }

        public List<OrdenRecepcionDetalleVd> MuestrasByResultadoAnalisis(int idEstablecimientoUsuario)
        {
            List<OrdenRecepcionDetalleVd> LstOrdenRecepcionDetalle = new List<OrdenRecepcionDetalleVd>();

            var objCommand = GetSqlCommand("pNLS_DetalleResultadosAnalisis");
            InputParameterAdd.Int(objCommand, "IdLaboratorioUsuario", idEstablecimientoUsuario);
            DataTable DataOrdenRecepcionDetalle = Execute(objCommand);

            if (DataOrdenRecepcionDetalle.Rows.Count == 0)
                return null;

            for (int i = 0; i < DataOrdenRecepcionDetalle.Rows.Count; i++)
            {
                var row = DataOrdenRecepcionDetalle.Rows[i];
                OrdenRecepcionDetalleVd BEOrdenRecepcionDetalle = new OrdenRecepcionDetalleVd();

                if (!row.IsNull("IdOrden"))
                    BEOrdenRecepcionDetalle.idOrden = Guid.Parse(row["IdOrden"].ToString());
                if (!row.IsNull("idOrdenMuestraRecepcion"))
                    BEOrdenRecepcionDetalle.CodigoOrdenMuestraRecepcion = Guid.Parse(row["idOrdenMuestraRecepcion"].ToString());

                if (!row.IsNull("FechaSolicitud"))
                    BEOrdenRecepcionDetalle.FechaSolicitud = row["FechaSolicitud"].ToString();

                else
                    BEOrdenRecepcionDetalle.FechaSolicitud = string.Empty;

                if (!row.IsNull("FechaObtencion"))
                    BEOrdenRecepcionDetalle.FechaObtencion = row["FechaObtencion"].ToString();
                else
                    BEOrdenRecepcionDetalle.FechaObtencion = string.Empty;
                if (!row.IsNull("Hora"))
                    BEOrdenRecepcionDetalle.HoraObtencion = DateTime.Parse(row["hora"].ToString()).ToString("HH:mm");
                else
                    BEOrdenRecepcionDetalle.HoraObtencion = string.Empty;
                if (!row.IsNull("CodigoOrden"))
                    BEOrdenRecepcionDetalle.codigoOrden = row["CodigoOrden"].ToString();
                else
                    BEOrdenRecepcionDetalle.codigoOrden = string.Empty;
                if (!row.IsNull("EstablecimientoOrigen"))
                    BEOrdenRecepcionDetalle.EstablecimientoOrigen = row["EstablecimientoOrigen"].ToString();
                else
                    BEOrdenRecepcionDetalle.EstablecimientoOrigen = string.Empty;
                if (!row.IsNull("CodigoMuestra"))
                    BEOrdenRecepcionDetalle.codigoMuestra = row["CodigoMuestra"].ToString();
                else
                    BEOrdenRecepcionDetalle.codigoMuestra = string.Empty;
                if (!row.IsNull("Enfermedad"))
                    BEOrdenRecepcionDetalle.Enfermedad = row["Enfermedad"].ToString();
                else
                    BEOrdenRecepcionDetalle.Enfermedad = string.Empty;
                if (!row.IsNull("Examen"))
                    BEOrdenRecepcionDetalle.Examen = row["Examen"].ToString();
                else
                    BEOrdenRecepcionDetalle.Examen = string.Empty;
                if (!row.IsNull("TipoMuestra"))
                    BEOrdenRecepcionDetalle.TipoMuestra = row["TipoMuestra"].ToString();
                else
                    BEOrdenRecepcionDetalle.TipoMuestra = string.Empty;
                if (!row.IsNull("Tipo"))
                    BEOrdenRecepcionDetalle.Tipo = row["Tipo"].ToString();
                else
                    BEOrdenRecepcionDetalle.Tipo = string.Empty;

                if (!row.IsNull("EXISTE_PENDIENTE"))
                    BEOrdenRecepcionDetalle.ExistePendiente = int.Parse(row["EXISTE_PENDIENTE"].ToString());
                else
                    BEOrdenRecepcionDetalle.ExistePendiente = 0;

                if (!row.IsNull("EXISTE_RECIBIDO"))
                    BEOrdenRecepcionDetalle.ExisteRecibido = int.Parse(row["EXISTE_RECIBIDO"].ToString());
                else
                    BEOrdenRecepcionDetalle.ExisteRecibido = 0;
                if (!row.IsNull("EXISTE_RECHAZO"))
                    BEOrdenRecepcionDetalle.ExisteRechazo = int.Parse(row["EXISTE_RECHAZO"].ToString());
                else
                    BEOrdenRecepcionDetalle.ExisteRechazo = 0;
                if (!row.IsNull("estatus"))
                    BEOrdenRecepcionDetalle.estatus = row["estatus"].ToString();
                else
                    BEOrdenRecepcionDetalle.estatus = string.Empty;
                if (!row.IsNull("conformeR"))
                    BEOrdenRecepcionDetalle.ConformeR = row["conformeR"].ToString();
                else
                    BEOrdenRecepcionDetalle.ConformeR = string.Empty;
                if (!row.IsNull("idLaboratorioDestino"))
                    BEOrdenRecepcionDetalle.IdLaboratorioDestino = int.Parse(row["idLaboratorioDestino"].ToString());
                else
                    BEOrdenRecepcionDetalle.IdLaboratorioDestino = 0;
                if (!row.IsNull("fecharecepcion"))
                    BEOrdenRecepcionDetalle.FechaRecepcion = row["fecharecepcion"].ToString();
                else
                    BEOrdenRecepcionDetalle.FechaRecepcion = string.Empty;
                if (!row.IsNull("horarecepcion"))
                    BEOrdenRecepcionDetalle.HoraRecepcion = DateTime.Parse(row["horarecepcion"].ToString()).ToString("HH:mm");
                else
                    BEOrdenRecepcionDetalle.HoraRecepcion = string.Empty;
                if (!row.IsNull("EstablecimientoDestino"))
                    BEOrdenRecepcionDetalle.EstablecimientoDestino = row["EstablecimientoDestino"].ToString();
                else
                    BEOrdenRecepcionDetalle.EstablecimientoDestino = string.Empty;
                if (!row.IsNull("idMuestraCod"))
                    BEOrdenRecepcionDetalle.IdMuestraCod = row["idMuestraCod"].ToString();
                else
                    BEOrdenRecepcionDetalle.IdMuestraCod = string.Empty;

                //BEOrdenRecepcionDetalle.FechaRecepcionLab
                BEOrdenRecepcionDetalle.EstatusP = !row.IsNull("EstatusP") ? int.Parse(row.IsNull("EstatusP").ToString()) : 0;

                string stringFechaRecepcionLab = row.IsNull("fechaRecepcionP") ? string.Empty : row["fechaRecepcionP"].ToString();
                string stringHoraRecepcionLab = row.IsNull("horaRecepcionP") ? string.Empty : row["horaRecepcionP"].ToString();

                BEOrdenRecepcionDetalle.FechaRecepcionLab = DateTime.Parse(string.Format("{0}{1}", stringFechaRecepcionLab, stringHoraRecepcionLab)).ToString("dd/MM/YYYY HH:mm");

                BEOrdenRecepcionDetalle.IdOrdenMuestra = !row.IsNull("idOrdenMuestra") ? Guid.Parse(row.IsNull("idOrdenMuestra").ToString()) : Guid.Empty;

                BEOrdenRecepcionDetalle.flagResultado = int.Parse(row["FlagResultado"].ToString());

                LstOrdenRecepcionDetalle.Add(BEOrdenRecepcionDetalle);
            }

            return LstOrdenRecepcionDetalle;
        }
        //Autor : Marcos Mejia 
        //Descricpion : Método que obtiene datos del usuario que registra la Orden.
        //Fecha: 25/09/2018
        public UsuarioEnvioSms ConsultaDatosUsuario(Guid idOrden)
        {
            var usuario = new UsuarioEnvioSms();
            var objCommand = GetSqlCommand("pNLS_ConsultaDatosUsuario");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);
            var dataUsuario = Execute(objCommand);

            if (dataUsuario.Rows.Count == 0)
                return null;
            for (int i = 0; i < dataUsuario.Rows.Count; i++)
            {
                var row = dataUsuario.Rows[i];
                usuario.idUsuario = Convert.ToInt16(row["idUsuarioRegistro"]);
                usuario.Nombre = row["NombreUsuario"].ToString();
                usuario.Celular = row["telefonoContacto"].ToString();
                usuario.Correo = row["correo"].ToString();
                usuario.codigoOrden = row["codigoOrden"].ToString();
                usuario.idUsuarioReceptor = row["idUsuarioReceptor"].ToString();
            }
            return usuario;
        }

        /// Descripción: Muestra el detalle de la Orden referenciada.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 27/06/2019
        public List<OrdenRecepcionDetalleVd> DetalleOrdenReferencia(int TipoFecha, string codigoOrden, DateTime FechaDesde, DateTime Fechahasta,
                                                                    string NroOficio, string idMuestra, int idLaboratorio)
        {
            List<OrdenRecepcionDetalleVd> LstOrdenReferenciaDetalle = new List<OrdenRecepcionDetalleVd>();

            var objCommand = GetSqlCommand("pNLS_DetalleOrdenReferencia");
            InputParameterAdd.Int(objCommand, "TipoFecha", TipoFecha);
            InputParameterAdd.Varchar(objCommand, "CodigoOrden", codigoOrden);
            InputParameterAdd.DateTime(objCommand, "FechaDesde", FechaDesde);
            InputParameterAdd.DateTime(objCommand, "FechaHasta", Fechahasta);
            InputParameterAdd.Varchar(objCommand, "NroOficio", NroOficio);
            InputParameterAdd.Varchar(objCommand, "idMuestra", idMuestra);
            InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);

            DataTable DataOrdenRecepcionDetalle = Execute(objCommand);

            if (DataOrdenRecepcionDetalle.Rows.Count == 0)
                return null;

            for (int i = 0; i < DataOrdenRecepcionDetalle.Rows.Count; i++)
            {
                OrdenRecepcionDetalleVd BEOrdenRecepcionDetalle = new OrdenRecepcionDetalleVd();
                var row = DataOrdenRecepcionDetalle.Rows[i];
                BEOrdenRecepcionDetalle.idOrden = Guid.Parse(row["idOrden"].ToString());
                BEOrdenRecepcionDetalle.FechaObtencion = row["fechaColeccion"].ToString();
                BEOrdenRecepcionDetalle.Enfermedad = row["Enfermedad"].ToString();
                BEOrdenRecepcionDetalle.Examen = row["Examen"].ToString();
                BEOrdenRecepcionDetalle.TipoMuestra = row["TipoMuestra"].ToString();
                BEOrdenRecepcionDetalle.EstablecimientoDestino = row["LaboratorioDestino"].ToString();
                BEOrdenRecepcionDetalle.FechaSolicitud = row["FechaHora"].ToString();
                BEOrdenRecepcionDetalle.codigoMuestra = row["codificacion"].ToString();
                LstOrdenReferenciaDetalle.Add(BEOrdenRecepcionDetalle);
            }
            return LstOrdenReferenciaDetalle;
        }

        public void RegistoResultadoAnalitoDetalle(List<OrdenResultadoAnalito> resultados)
        {
            try
            {
                foreach (OrdenResultadoAnalito item in resultados)
                {
                    var objCommand = GetSqlCommand("pNLI_RegistoResultadoAnalitoDetalle");
                    InputParameterAdd.Guid(objCommand, "idOrdenResultadoAnalito", item.idOrdenResultadoAnalisis);
                    InputParameterAdd.Int(objCommand, "idAnalitoCabeceraVariable", item.idAnalisis);
                    InputParameterAdd.Int(objCommand, "idSecuencia", item.idSecuencia);
                    InputParameterAdd.Varchar(objCommand, "valor", item.convResultado);
                    ExecuteNonQuery(objCommand);
                }
            }
            catch (Exception ex)
            {
                new bsPage().LogError(ex, "LogNetLab");
            }
            
        }

        public UsuarioEnvioSms ObtenerDatosUsuarioLiberador(string idOrdenExamen)
        {
            UsuarioEnvioSms usuario = new UsuarioEnvioSms();
            var objCommand = GetSqlCommand("pNLS_ConsultaDatosUsuarioAlerta");
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);
            var dataUsuario = Execute(objCommand);

            if (dataUsuario.Rows.Count == 0)
                return null;
            for (int i = 0; i < dataUsuario.Rows.Count; i++)
            {
                var row = dataUsuario.Rows[i];
                usuario.Nombre = row["liberador"].ToString();
                usuario.Correo = row["correo"].ToString();
                usuario.Celular = row["telefonoContacto"].ToString();
                usuario.codigoMuestra = row["CodigoMuestra"].ToString();
                usuario.Establecimiento = row["nombreExamen"].ToString();
                usuario.mensaje = row["Mensaje"].ToString();
            }
            return usuario;
        }

        /*
          Descripción: Registra la fecha de siembra del cultivo
          Autor: Marcos Mejia
          Fecha: 24/10/2018
          */
        public void RegistrarFechaSiembra(string idOrdenExamen, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLU_RegistrarFechaSiembraCultivo");
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            ExecuteNonQuery(objCommand);
        }

        public void RechazoFechaRom(string nroOficio, int idUsuario, int idCriterioRechazo)
        {
            var objCommand = GetSqlCommand("pNLI_OrdenMuestraRechazoFechaRom");
            InputParameterAdd.Varchar(objCommand, "nroOficio", nroOficio);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "idCriterioRechazo", idCriterioRechazo);
            ExecuteNonQuery(objCommand);
        }
        public List<OrdenMuestrasRechazo> GetOrdenCriterioRechazo(Guid idOrden)
        {
            var listaOrdenMuestraRechazo = new List<OrdenMuestrasRechazo>();

            var objCommand = GetSqlCommand("pNLS_OrdenCriterioRechazo");
            InputParameterAdd.Guid(objCommand, "IdOrden", idOrden);

            var dataOrdenMuestraRechazo = Execute(objCommand);

            if (dataOrdenMuestraRechazo.Rows.Count == 0)
                return null;

            for (var i = 0; i < dataOrdenMuestraRechazo.Rows.Count; i++)
            {
                var row = dataOrdenMuestraRechazo.Rows[i];
                var ordenmuestraRechazo = new OrdenMuestrasRechazo
                {
                    CriterioRechazo = row["Glosa"].ToString(),
                    tipoRechaazoAlta = int.Parse(row["tiporechazoalta"].ToString()),
                    idCriterioRechazo = int.Parse(row["idCriterioRechazo"].ToString())
                };
                listaOrdenMuestraRechazo.Add(ordenmuestraRechazo);
            }

            return listaOrdenMuestraRechazo;
        }
       
        public List<OrdenRecepcionVd> GetOrdenMuestraIngresada(string codigoOrden, string nroOficio, string codigoMuestra)
        {
            List<OrdenRecepcionVd> listaOrdenes = new List<OrdenRecepcionVd>();

            var objCommand = GetSqlCommand("pNLS_OrdenMuestra");

            InputParameterAdd.Varchar(objCommand, "NroOficio", nroOficio);
            InputParameterAdd.Varchar(objCommand, "codigoMuestra", codigoMuestra);
            InputParameterAdd.Varchar(objCommand, "CodigoOrden", codigoOrden);

            DataTable dataOrdenMuestra = Execute(objCommand);

            if (dataOrdenMuestra.Rows.Count == 0)
                return null;
            foreach (DataRow itemRow in dataOrdenMuestra.Rows)
            {
                OrdenRecepcionVd item = new OrdenRecepcionVd();
                item.idOrden = Converter.GetGuid(itemRow, "idOrden");
                item.idOrdenExamen = Converter.GetString(itemRow, "idOrdenExamen");
                item.idEstablecimiento = Converter.GetInt(itemRow, "idEstablecimiento");
                item.fechaSolicitud = Converter.GetDateTime(itemRow, "fechaSolicitud");
                item.oPaciente = new Paciente
                {
                    sexo = Converter.GetString(itemRow, "SexoPaciente"),
                    edadPaciente = Converter.GetString(itemRow, "EdadPaciente"),
                    FechaNacimiento = Converter.GetDateTime(itemRow,"fechaNacimiento"),
                    Nombres = Converter.GetString(itemRow, "nombrePaciente")
                };
                item.codigoOrden = Converter.GetString(itemRow, "codigoOrden");
                item.nroOficio = Converter.GetString(itemRow, "nroOficio");
                item.codigoMuestra = Converter.GetString(itemRow, "Muestra");
                item.tipoMuestra = Converter.GetString(itemRow, "TipoMuestra");
                item.enfermedad = Converter.GetString(itemRow, "Enfermedad");
                item.examen = Converter.GetString(itemRow, "nombreExamen");
                item.resultado = Converter.GetString(itemRow, "convResultado");
                item.criterioRechazo = Converter.GetString(itemRow, "CriteriosRechazo");
                item.observacionRechazoLab = Converter.GetString(itemRow, "ObservacionRechazo");
                item.estadoOrdenResultado = Converter.GetString(itemRow, "EstatusResultado");
                item.idOrdenMuestraRecepcion = Converter.GetString(itemRow, "idOrdenMuestraRecepcion");
                item.idOrdenMaterial = Converter.GetString(itemRow, "idOrdenMaterial");

                listaOrdenes.Add(item);
            }
            return listaOrdenes;
        }

        public void InsertOrdenDatoClinicoRechazo(OrdenDatoClinico ordenDatoClinico)
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
        public void UpdateOrdenRechazo(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenRechazo");
            InputParameterAdd.Int(objCommand, "idUsuario", orden.IdUsuarioEdicion);
            InputParameterAdd.Guid(objCommand, "idOrden", orden.idOrden);
            InputParameterAdd.Guid(objCommand, "idPaciente", orden.Paciente.IdPaciente);
            InputParameterAdd.Varchar(objCommand, "nroOficio", orden.nroOficio);
            InputParameterAdd.Varchar(objCommand, "solicitante", orden.solicitante);
            InputParameterAdd.DateTime(objCommand, "fechaRecepcionRomINS", orden.fechaIngresoINS);
            InputParameterAdd.DateTime(objCommand, "fechaReevaluacionFicha", orden.fechaReevaluacionFicha);
            InputParameterAdd.DateTime(objCommand, "fechaSolicitud", orden.fechaSolicitud);
            ExecuteNonQuery(objCommand);
        }
        public void UpdateOrdenMuestraRechazo(Orden orden)
        {
            var objCommand = GetSqlCommand("pNLU_OrdenMuestraRechazo");
            InputParameterAdd.Int(objCommand, "idUsuario", orden.IdUsuarioEdicion);
            InputParameterAdd.Guid(objCommand, "idOrdenMuestra", orden.ordenMuestraList.FirstOrDefault().idOrdenMuestra);
            InputParameterAdd.Varchar(objCommand, "fechaColeccion", orden.ordenMuestraList.FirstOrDefault().fechaColeccionToString);
            InputParameterAdd.Varchar(objCommand, "horaColeccion", orden.ordenMuestraList.FirstOrDefault().horaColeccionToString);
            ExecuteNonQuery(objCommand);
        }
        public void UpdateOrdenMuestraResultadoRechazo (Orden orden, int idTipoRechazo)
        {
            var objCommand = GetSqlCommand("pNLI_ActualizarOrdenMuestraResultadoRechazoOrdenMuestraRecepcion");
            InputParameterAdd.Int(objCommand, "idUsuario", orden.IdUsuarioEdicion);
            InputParameterAdd.Guid(objCommand, "idOrden", orden.idOrden);
            InputParameterAdd.Guid(objCommand, "idOrdenMuestraRecepcion", orden.ordenMuestraRecepcionadaList.FirstOrDefault().idOrdenMuestraRecepcion);
            InputParameterAdd.Int(objCommand, "tipoRechazoAlta", idTipoRechazo);
            ExecuteNonQuery(objCommand);
        }

        public string ListarSolicitudSalidaMuestraRom(DateTime fechaDesde, DateTime fechaHasta, string codigoMuestra, string formulario, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLS_ListarSolicitudSalidaMuestra");
            InputParameterAdd.DateTime(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Varchar(objCommand, "formato", formulario);
            InputParameterAdd.Varchar(objCommand, "codigoMuestra", codigoMuestra);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            string solicitud = objCommand.ExecuteScalar().ToString();
            return solicitud;
        }

        public string ListarSolicitudSalidaMuestraRomPorFormato(string formulario, int tipo, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_ListarSolicitudSalidaMuestraPorFormato");
            InputParameterAdd.Varchar(objCommand, "formato", formulario);
            InputParameterAdd.Int(objCommand, "tipo", tipo);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            string solicitud = objCommand.ExecuteScalar().ToString();
            return solicitud;
        }

        public string EnvioMuestraRom(string codigoMuestra,string nroOficio, int idUsuario, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLI_RegistroSolicitudSalidaMuestra");
            InputParameterAdd.Varchar(objCommand, "codigo", codigoMuestra);
            InputParameterAdd.Varchar(objCommand, "nroOficio", nroOficio);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            string muestra = objCommand.ExecuteScalar().ToString();
            return muestra;
        }

        public string GeneraSolicitudSalidaMuestrasRom(string codigoMuestra, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLU_GenerarSolicitudSalidaMuestra");
            InputParameterAdd.Varchar(objCommand, "codigoMuestra", codigoMuestra);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            string solicitud = objCommand.ExecuteScalar().ToString();
            return solicitud;
        }

        public string VerificarMuestrasMasivo(List<ValidaResultadoMasivo> comentarioList, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLU_ValidacionResultadosMasiva2");
            objCommand.Parameters.AddWithValue("@resultados", CreateDataTableVerificacionMasiva(comentarioList));
            objCommand.Parameters.AddWithValue("@idUsuario", idUsuario);
            return objCommand.ExecuteScalar().ToString();
        }

        private static DataTable CreateDataTableVerificacionMasiva(List<ValidaResultadoMasivo> tabla)
        {
            DataTable table = new DataTable();
            table.Columns.Add("idOrdenExamen", typeof(Guid));
            table.Columns.Add("conforme", typeof(int));
            table.Columns.Add("motivo", typeof(string));
            table.Columns.Add("estatusE", typeof(int));
            foreach (ValidaResultadoMasivo item in tabla)
            {
                var row = table.NewRow();
                row["idOrdenExamen"] = item.idOrdenExamen;
                row["conforme"] = item.Conforme;
                row["motivo"] = item.Comentarios;
                row["estatusE"] = (item.Conforme == "1") ? 11:9;
                table.Rows.Add(row);
            }

            return table;
        }
    }
}

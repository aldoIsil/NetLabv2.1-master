using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using System;
using Model;

namespace DataLayer
{
    public class ValidaResultDal : DaoBase
    {

        public ValidaResultDal(IDalSettings settings) : base(settings)
        {
        }

        public ValidaResultDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene el listado de las muestras listas para que el analista confirme el resultado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="fechaSolicitud"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroOficio"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<ValidaResult> GetValidaciones(int idUsuario, int fechaSolicitud, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, string nroDocumento, int idLaboratorio, int estadoResultado, int idEnfermedad, Guid idExamen)
        {
            var listaValidaciones = new List<ValidaResult>();

            var objCommand = GetSqlCommand("pNLS_Validaciones");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "fechaSolicitud", fechaSolicitud);
            InputParameterAdd.DateTime(objCommand, "FechaDesde", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "FechaHasta", fechaHasta);
            InputParameterAdd.Varchar(objCommand, "codigoOrden", codigoOrden);
            InputParameterAdd.Varchar(objCommand, "nroOficio", nroOficio);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            InputParameterAdd.Int(objCommand, "idLaboratorioUsuario", idLaboratorio);
            InputParameterAdd.Int(objCommand, "Estado", estadoResultado);
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);

            var dataValidaciones = Execute(objCommand);

            if (dataValidaciones.Rows.Count == 0)
                return null;

            for (var i = 0; i < dataValidaciones.Rows.Count; i++)
            {
                var row = dataValidaciones.Rows[i];
                
                var vr = new ValidaResult
                {
                    Establecimiento = row["NombEstab"].ToString(),
                    CodigoOrden = row["CodOrden"].ToString(),
                    nroDocumento = row["nroDocumento"].ToString(),
                    tipoDocumento = row["TipoDocumento"].ToString(),
                    fechaRegistro = DateTime.Parse(row["fechaRegistro"].ToString()),
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    fechaSolicitud = DateTime.Parse(row["fechaSolicitud"].ToString()),
                    genero = int.Parse(row["genero"].ToString()),
                    //fechaNacimiento = DateTime.Parse(row["fechaNacimiento"].ToString()),
                    fechaNacimiento = (row["fechaNacimiento"]).ToString() != "" ? DateTime.Parse(row["fechaNacimiento"].ToString()) : DateTime.Parse("01/01/0001"),
                    nroOficio = row["nroOficio"].ToString(),
                    EXISTE_PENDIENTE = !row.IsNull("EXISTE_PENDIENTE") ? int.Parse(row["EXISTE_PENDIENTE"].ToString()) : 0,
                    EXISTE_VALIDADO = !row.IsNull("EXISTE_VALIDADO") ? int.Parse(row["EXISTE_VALIDADO"].ToString()) : 0,
                    //SOTERO BUSTAMANTE SOLCITUD INGRESO DE NUEVO RESULTADO VERIFICADOR
                    SOLICITA_INGRESO = int.Parse(row["estatusSol"].ToString()),
                    idExamen = row["idExamen"].ToString(),
                    NombreExamen = row["NombreExamen"].ToString(),
                    idOrdenExamen = row["idOrdenExamen"].ToString(),
                    Resultado = row["Resultado"].ToString(),
                    Conforme = row["Conforme"].ToString(),
                    paciente = row["Paciente"].ToString(),
                    Comentarios = row["motivoNoConforme"].ToString(),
                    codigoMuestra = row["codigoMuestra"].ToString(),
                    NomLab = row["codigoExamen"].ToString(),
                    ExamenComun = row["descripcion"].ToString(),
                    fechaColeccion = DateTime.Parse(row["fechaColeccion"].ToString())
                };

                listaValidaciones.Add(vr);
            }

            return listaValidaciones; 
        }

        /// <summary>
        /// Descripción: Obtiene el listado de las muestras listas para que el analista libere el resultado.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 28/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="fechaSolicitud"></param>
        /// <param name="codigoOrden"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="nroOficio"></param>
        /// <param name="nroDocumento"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<ValidaResult> GetValidacionesSolicitudes(int idUsuario, int fechaSolicitud, string codigoOrden, DateTime fechaDesde, DateTime fechaHasta, string nroOficio, string nroDocumento, int idLaboratorio, int estado)
        {
            var listaValidaciones = new List<ValidaResult>();

            var objCommand = GetSqlCommand("pNLS_ValidacionesSolicitudes");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "fechaSolicitud", fechaSolicitud);
            InputParameterAdd.DateTime(objCommand, "FechaDesde", fechaDesde);
            InputParameterAdd.DateTime(objCommand, "FechaHasta", fechaHasta);
            InputParameterAdd.Varchar(objCommand, "codigoOrden", codigoOrden);
            InputParameterAdd.Varchar(objCommand, "nroOficio", nroOficio);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            InputParameterAdd.Int(objCommand, "idLaboratorioUsuario", idLaboratorio);
            InputParameterAdd.Int(objCommand, "EstadoSol", estado);

            var dataValidaciones = Execute(objCommand);

            if (dataValidaciones.Rows.Count == 0)
                return null;

            for (var i = 0; i < dataValidaciones.Rows.Count; i++)
            {
                var row = dataValidaciones.Rows[i];

                var vr = new ValidaResult
                {
                    Establecimiento = row["NombEstab"].ToString(),
                    CodigoOrden = row["CodOrden"].ToString(),
                    nroDocumento = row["nroDocumento"].ToString(),
                    tipoDocumento = row["TipoDocumento"].ToString(),
                    fechaRegistro = DateTime.Parse(row["fechaRegistro"].ToString()),
                    idOrden = Converter.GetGuid(row, "idOrden"),
                    fechaSolicitud = DateTime.Parse(row["fechaSolicitud"].ToString()),
                    genero = int.Parse(row["genero"].ToString()),
                    fechaNacimiento = !row.IsNull("fechaNacimiento") ? DateTime.Parse(row["fechaNacimiento"].ToString()) : DateTime.Now,
                    nroOficio = row["nroOficio"].ToString(),
                    EXISTE_PENDIENTE = !row.IsNull("EXISTE_PENDIENTE") ? int.Parse(row["EXISTE_PENDIENTE"].ToString()) : 0,
                    EXISTE_VALIDADO = !row.IsNull("EXISTE_VALIDADO") ? int.Parse(row["EXISTE_VALIDADO"].ToString()) : 0,

                    //SOTERO BUSTAMANTE SOLCITUD INGRESO DE NUEVO RESULTADO VERIFICADOR
                    SOLICITA_INGRESO = int.Parse(row["estatusSol"].ToString()),
                    NombreExamen = row["NombreExamen"].ToString(),
                    idOrdenExamen = row["idOrdenExamen"].ToString(),
                };

                listaValidaciones.Add(vr);
            }

            return listaValidaciones;
        }
    }
}

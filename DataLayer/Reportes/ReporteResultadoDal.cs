using System;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System.Net;
using System.IO;
using System.Web;

namespace DataLayer.Reportes
{
    public class ReporteResultadoDal : DaoBase
    {
        public ReporteResultadoDal(IDalSettings settings) : base(settings)
        {
        }

        public ReporteResultadoDal() : this(new NetlabSettings())
        {
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
        /// Descripción: Obtiene informacion de la Orden y sus resultados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorioDestino"></param>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public OrdenResultado GetOrdenResultado(Guid idOrden, int idLaboratorioDestino, string idOrdenExamen, int idUsuario)
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

            var objCommand = GetSqlCommand("pNLS_ReportexOrden_DatosOrden");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idLaboratorioDestino", idLaboratorioDestino);
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            //InputParameterAdd.Varchar(objCommand, "localIP", localIP);

            return OrdenResultadoConvertTo.Orden(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene informacion de la Orden y sus resultados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public OrdenResultado GetOrdenResultado(Guid idOrden)
        {
            var objCommand = GetSqlCommand("pNLS_ReportexOrden_DatosOrden_Old");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);

            return OrdenResultadoConvertTo.Orden(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de la Orden y sus resultados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idExamen"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<MuestraResultado> GetMuestras(Guid idOrden, int idLaboratorio)
        {
            var objCommand = GetSqlCommand("pNLS_NewReporteResultado_Muestras");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);

            return MuestraResultadoConvertTo.Muestras(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de la Orden y sus resultados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idExamen"></param>
        /// <param name="idLaboratorio"></param>
        /// <returns></returns>
        public List<MuestraResultado> GetMuestras(Guid idOrden, Guid idExamen, int idLaboratorio)
        {
            var objCommand = GetSqlCommand("pNLS_NewReporteResultadoByExamen_Muestras");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            InputParameterAdd.Int(objCommand, "idLaboratorio", idLaboratorio);

            return MuestraResultadoConvertTo.Muestras(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene informacion de las muestras filtrado por el Codigo de Orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorioDestino"></param>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public List<MuestraResultado> GetMuestrasbyIdOrden(Guid idOrden, int idLaboratorioDestino, string idOrdenExamen)
        {
            var objCommand = GetSqlCommand("pNLS_ReportexOrden_DatosMuestra");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idLaboratorioDestino", idLaboratorioDestino);
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);

            return MuestraResultadoConvertTo.Muestras(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes de los resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<ExamenResultado> GetExamenes(Guid idOrden)
        {
            var objCommand = GetSqlCommand("pNLS_ReporteResultado_Examen");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);

            return ExamenResultadoConvertTo.Examenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las pruebas realizadas a una orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<ExamenResultadoDetalle> GetOldExamenDetalle(Guid idOrden, Guid idOrdenExamen)
        {
            var objCommand = GetSqlCommand("pNLS_ReporteResultado_Detalle");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", idOrdenExamen);

            return ExamenResultadoDetalleConvertTo.DetalleExamenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion del detalle de las pruebas realizadas a una orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idLaboratorioDestino"></param>
        /// <param name="idOrdenExamen"></param>
        /// <returns></returns>
        public List<ExamenResultadoDetalle> GetExamenDetalle(Guid idOrden, int idLaboratorioDestino, string idOrdenExamen)
        {
            var objCommand = GetSqlCommand("pNLS_ReportexOrden_DatosExamenes");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idLaboratorioDestino", idLaboratorioDestino);
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);

            return ExamenResultadoDetalleConvertTo.DetalleExamenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion del detalle de las pruebas realizadas a una orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public List<ExamenResultadoDetalle> GetExamenDetalle(Guid idOrden)
        {
            var objCommand = GetSqlCommand("pNLS_ReportexOrden_DatosExamenes_Old");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);

            return ExamenResultadoDetalleConvertTo.DetalleExamenes(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion del detalle de las pruebas realizadas a una orden
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<ExamenResultadoDetalle> GetExamenDetalle(Guid idOrden, Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_NewReporteResultadoByExamen_Detalle");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);

            return ExamenResultadoDetalleConvertTo.DetalleExamenes(Execute(objCommand));
        }

        public List<ExamenResultadoInterpretacion> GetExamenInterpretacion(string idOrdenExamen)
        {
            var objCommand = GetSqlCommand("pNLS_InterpretacionPorExamen");
            InputParameterAdd.Varchar(objCommand, "idOrdenExamen", idOrdenExamen);
            return ExamenResultadoDetalleConvertTo.ExamenInterpretacion(Execute(objCommand));
        }

        public ResultadoKobos GetResultadoKobosId(int id, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_GetResultadoKobosId");
            InputParameterAdd.Int(objCommand, "idPruebaRapidaKobo", id);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            return OrdenResultadoConvertTo.Kobos(Execute(objCommand));

        }
        public CargoUsuarioEstablecimiento CargoUsuarioEstablecimiento(int idEstablecimiento, string fechaVerificacion )
        {
            var objCommand = GetSqlCommand("pNLS_CargoUsuarioEstablecimientoId");
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Varchar(objCommand, "fechaVerificacion", fechaVerificacion);
            return OrdenResultadoConvertTo.CargoUsuarioEstablecimiento(Execute(objCommand));
        }
        public OrdenResultado GetOrdenResultadoWS(Guid idOrdenExamen)
        {
            var objCommand = GetSqlCommand("pNLS_ReportexOrden_DatosOrden_WS");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", idOrdenExamen);
            InputParameterAdd.Int(objCommand, "idUsuario", 72);

            return OrdenResultadoConvertTo.Orden(Execute(objCommand));
        }
    }
}
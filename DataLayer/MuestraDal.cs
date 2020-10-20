using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using Model;
using System;
using System.Data;

namespace DataLayer
{
    public class MuestraDal : DaoBase
    {
        public MuestraDal(IDalSettings settings) : base(settings)
        {
        }

        public MuestraDal() : this(new NetlabSettings())
        {
        }

        /// <summary>
        /// Descripción: Genera Codigos de muestra por establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="cantidad"></param>
        /// <param name="idEstablecimiento"></param>
        /// <param name="idUsuario"></param>
        public void GenerarCodigos(int cantidad, int idEstablecimiento, int idUsuario, int icodigoLineal)
        {
            for (var i = 0; i < cantidad; i++)
            {
                GenerarCodigos(idEstablecimiento, idUsuario, icodigoLineal);
            }
        }
        /// <summary>
        /// Descripción: Inserta nevos codigos por establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEstablecimiento"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public string GenerarCodigos(int idEstablecimiento, int idUsuario, int icodigoLineal)
        {
            var codificacion = ObtenerCodificacion(idEstablecimiento);

            if (!string.IsNullOrEmpty(codificacion))
                InsertarCodificacionDeMuestra(idEstablecimiento, codificacion, idUsuario, icodigoLineal);

            return codificacion;
        }
        public MuestraCodificacion GenerarCodigosKobo(int idEstablecimiento, int idUsuario, int icodigoLineal)
        {
            var o = new MuestraCodificacion();
            
            var codificacion = ObtenerCodificacion(idEstablecimiento);

            if (!string.IsNullOrEmpty(codificacion))
                o = InsertarCodificacionDeMuestraKobo(idEstablecimiento, codificacion, idUsuario, icodigoLineal);

            return o;
        }
        /// <summary>
        /// Descripción: Obtiene el ultimo codigo de muestra creado en un EESS
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        private string ObtenerCodificacion(int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLI_GenerarCodigosMuestra");

            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            OutputParameterAdd.Varchar(objCommand, "codificacion", 50);

            ExecuteNonQuery(objCommand);

            var codificacion = objCommand.Parameters["@codificacion"].Value;

            return codificacion?.ToString();
        }
        /// <summary>
        /// Descripción: Registra el nuevo codigo de muesta para un establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEstablecimiento"></param>
        /// <param name="codificacion"></param>
        /// <param name="idUsuario"></param>
        public void InsertarCodificacionDeMuestra(int idEstablecimiento, string codificacion, int idUsuario, int icodigoLineal)
        {
            var objCommand = GetSqlCommand("pNLI_MuestraCodificacion");

            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Varchar(objCommand, "codificacion", codificacion);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", idUsuario);
            InputParameterAdd.Int(objCommand, "icodigoLineal", icodigoLineal);

            ExecuteNonQuery(objCommand);
        }
        public MuestraCodificacion InsertarCodificacionDeMuestraKobo(int idEstablecimiento, string codificacion, int idUsuario, int icodigoLineal)
        {
            var objCommand = GetSqlCommand("pNLI_MuestraCodificacionKobo");
            var oMuestraCodificacion = new MuestraCodificacion();
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Varchar(objCommand, "codificacion", codificacion);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", idUsuario);
            InputParameterAdd.Int(objCommand, "icodigoLineal", icodigoLineal);
            OutputParameterAdd.Varchar(objCommand, "newcodigoQR",10);
            OutputParameterAdd.Varchar(objCommand, "newcodigoLineal",12);
            OutputParameterAdd.UniqueIdentifier(objCommand, "idMuestraCod");
            ExecuteNonQuery(objCommand);
            oMuestraCodificacion.codificacionLineal = objCommand.Parameters["@newcodigoLineal"].Value.ToString();
            oMuestraCodificacion.idMuestraCod = Guid.Parse(objCommand.Parameters["@idMuestraCod"].Value.ToString());
            oMuestraCodificacion.codificacion = codificacion;

            return oMuestraCodificacion;
        }
        /// <summary>
        /// Descripción: Obtiene codigos de muestra por Codigo y Establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no es usado.
        /// </summary>
        /// <param name="idEstablecimiento"></param>
        /// <param name="codigoRenaes"></param>
        /// <returns></returns>
        public int CantidadGenerados(int idEstablecimiento, string codigoRenaes)
        {
            var objCommand = GetSqlCommand("pNLS_MuestraCantidadCodigosGenerados");
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Varchar(objCommand, "codigoRenaes", codigoRenaes);

            return MuestraConvertTo.cantidad(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene codigos de muestra por Codigo y Establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        public MuestraCodificacion CodificacionByCodigo(string codigo, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLS_MuestraCodificacionByCodigo");
            InputParameterAdd.Varchar(objCommand, "codigo", codigo);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);

            return MuestraConvertTo.Codificacion(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los codigos de muestra creados por establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MuestraCodificacion CodificacionById(Guid id)
        {
            var objCommand = GetSqlCommand("pNLS_MuestraCodificacionById");
            InputParameterAdd.Guid(objCommand, "idMuestracod", id);

            return MuestraConvertTo.Codificacion(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los codigos de muestra creados por establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, procedimiento con errores.
        /// </summary>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        public List<MuestraCodificacion> MuestraCodificacionDisponiblesByEstablecimiento(int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLS_MuestraCodificacionDisponiblesByEstablecimiento");
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);

            return MuestraConvertTo.Codificaciones(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Obtiene los ultimos codigos de muestra generado por EESS
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimiento"></param>
        /// <param name="cantidad"></param>
        /// <returns></returns>
        public List<string> UltimosGenerados(int idUsuario, int idEstablecimiento, int cantidad, string columna = "codificacion")
        {
            var objCommand = GetSqlCommand("pNLS_UltimasMuestraCodificacionGeneradas");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "cantidad", cantidad);

            return MuestraConvertTo.GetListaCadenas(Execute(objCommand), columna);
        }
        public DataTable UltimosGeneradosExcel(int idUsuario, int idEstablecimiento, int cantidad)
        {
            var objCommand = GetSqlCommand("pNLS_UltimasMuestraCodificacionGeneradas");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "cantidad", cantidad);
            DataTable dataTable = Execute(objCommand);
            dataTable.Columns.Remove("idMuestraCod");
            dataTable.Columns.Remove("idEstablecimiento");
            dataTable.Columns.Remove("idCodificacion");
            dataTable.Columns.Remove("fechaRegistro");
            dataTable.Columns.Remove("idUsuarioRegistro");
            dataTable.Columns.Remove("fechaEdicion");
            dataTable.Columns.Remove("idUsuarioEdicion");
            dataTable.TableName = "Registros";
            dataTable.Columns[0].ColumnName = "Codigos";
            if (idEstablecimiento == 991)
            {
                dataTable.Columns[1].ColumnName = "CodigoLineal";
                dataTable.Columns[2].ColumnName = "estado";
            }
            else
            {
                dataTable.Columns[1].ColumnName = "estado";
            }


            if (dataTable.Rows.Count == 0)
                return null;
            return dataTable;
        }
        /// <summary>
        /// Descripción: Controlador para obtener los codigos de muestra por EESS.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimiento"></param>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        public List<string> GetGeneradosByEstablecimiento(int idEstablecimiento, string inicio, string fin, string solicitud=null, string columna = "codificacion")
        {
            var objCommand = GetSqlCommand("pNLS_GetMuestraCodificacionGeneradas");
            //InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Varchar(objCommand, "inicio", inicio);
            InputParameterAdd.Varchar(objCommand, "fin", fin);
            InputParameterAdd.Varchar(objCommand, "solicitud", solicitud);

            return MuestraConvertTo.GetListaCadenas(Execute(objCommand), columna);
        }

        /// Descripción: Consulta los códigos de muestra generados pero no utilizados.
        /// Author: Marcos Mejia.
        /// Fecha Creacion: 02/01/2019
        public List<MuestraCodificacion> ConsultaCodigosMuestra(int cantidad, int idEstablecimiento, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_ConsultaCodigosMuestra");
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Int(objCommand, "cantidad", cantidad);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            return MuestraConvertTo.ConsultaCodigosMuestra(Execute(objCommand));
        }

        public List<MuestraCodificacion> ConsultaCodigosGenerados(string fechaDesde, string fechaHasta, string idEstablecimiento, string codigo, string estado, int laboratorio)
        {
            var objCommand = GetSqlCommand("pNLS_ConsultaCodigosGenerados");
            InputParameterAdd.Varchar(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.Varchar(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Varchar(objCommand, "idEstablecimiento", idEstablecimiento);
            InputParameterAdd.Varchar(objCommand, "codificacion", codigo);
            InputParameterAdd.Varchar(objCommand, "estado", estado);
            InputParameterAdd.Int(objCommand, "Laboratorio", laboratorio);
            return MuestraConvertTo.ConsultaCodigosGenerados(Execute(objCommand));
        }
        public List<MuestraCodificacion> ConsultaCodigosMuestraEstado(string inicio, string fin, int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLS_ConsultaCodigosEstado");
            InputParameterAdd.Varchar(objCommand, "inicio", inicio);
            InputParameterAdd.Varchar(objCommand, "fin", fin);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);
            return MuestraConvertTo.ConsultaCodigosGenerados(Execute(objCommand));
        }
    }
}

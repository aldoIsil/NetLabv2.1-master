using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using DataLayer.DalConverter;
using System;
using System.Data;
using Model;


namespace DataLayer
{
    public class LiberadosDal : DaoBase
    {
        
        public LiberadosDal(IDalSettings settings) : base(settings)
        {
        }

        public LiberadosDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene las ordenes por Codigo de Orden a ser liberados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        /// </summary>
        /// <param name="idOrden"></param>
        /// <returns></returns>
        public Orden GetOrdenById(Guid idOrdenExamen)
        {
            List<Orden> ordenList = new List<Orden>();

            var objCommand = GetSqlCommand("pNLS_LiberarResultados");
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", idOrdenExamen);
                        DataSet dataSet = ExecuteDataSet(objCommand);

            DataTable ordenDataTable = dataSet.Tables[0];
            DataTable resultadosDataTable = dataSet.Tables[1];

            #region Orden

            Orden orden = null;
            foreach (DataRow row in ordenDataTable.Rows)
            {
                orden = new Orden();

                orden.codigoOrden = Converter.GetString(row, "codigoOrden");

                orden.Paciente = new Paciente();
                orden.Paciente.NroDocumento = Converter.GetString(row, "paciente_nrodocumento");
                orden.Paciente.Nombres = Converter.GetString(row, "paciente_nombres");
                orden.Paciente.FechaNacimiento = Converter.GetDateTime(row, "paciente_fechNac");
                orden.Paciente.edadAnios = Converter.GetInt(row, "edadAnios");
                orden.Paciente.edadMeses = Converter.GetInt(row, "edadMeses");
                orden.Paciente.codificacion = Converter.GetInt(row, "paciente_codificacion");

                //orden.idProyecto
                orden.Proyecto = new Proyecto();
                orden.Proyecto.Nombre = Converter.GetString(row, "proyecto_nombre");
                orden.Proyecto.IdProyecto = Converter.GetInt(row, "proyecto_idProyecto");

                //orden.idEstablecimiento
                orden.establecimiento = new Establecimiento();
                orden.establecimiento.Nombre = Converter.GetString(row, "establecimiento_nombre");
                orden.establecimiento.IdEstablecimiento = Converter.GetInt(row, "idEstablecimiento");
                orden.idEstablecimiento = orden.establecimiento.IdEstablecimiento;
                orden.nroOficio = Converter.GetString(row, "nroOficio");
                orden.observacion = Converter.GetString(row, "observacion");
                orden.estatus = Converter.GetInt(row, "estatus");
                orden.idOrden = Converter.GetGuid(row, "idOrden");
            }

            #endregion

            #region Resultados
            if (orden != null)
                orden.liberadosList = LiberadosConvertTo.ListLiberados(resultadosDataTable);

            #endregion

            return orden;
        }
        /// <summary>
        /// Descripción: ACtualiza los datos de la orden liberada.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, no se utiliza esta opcion.
        ///                SP no compila.
        /// </summary>
        /// <param name="libera"></param>
        public void UpdateLiberado(Liberados libera)
        {
            //var objCommand = GetSqlCommand("pNLU_ResultadosLiberados");
            var objCommand = GetSqlCommand("pNLU_OrdenSolicitaIngresoResultados");
            //InputParameterAdd.Guid(objCommand, "idOrdenExamen", libera.idOrdenExamen);
            //InputParameterAdd.Int(objCommand, "idUsuarioLiberado", libera.idUsuarioLiberado);
            InputParameterAdd.Guid(objCommand, "idOrdenExamen", libera.idOrdenExamen);
            InputParameterAdd.Int(objCommand, "idUsuario", libera.idUsuarioLiberado);
            InputParameterAdd.Int(objCommand, "estatusSol", libera.liberado);
            InputParameterAdd.Varchar(objCommand, "observaSol", libera.observacion);
            ExecuteNonQuery(objCommand);

        }


    }
}

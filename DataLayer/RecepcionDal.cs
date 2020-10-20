using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class RecepcionDal : DaoBase
    {
        public RecepcionDal(IDalSettings settings) : base(settings)
        {
        }
        public RecepcionDal() : this(new NetlabSettings())
        {
        }

        public List<OrdenMuestraRecepcionado> RecepcionarMuestra(OrdenMuestraVM model)
        {
            var objCommand = GetSqlCommand("pNLU_RecepcionMuestraROM");
            InputParameterAdd.Varchar(objCommand, "codigoMuestra", model.CodigoMuestra);
            InputParameterAdd.Int(objCommand, "establecimientoDestinoId", model.EstablecimientoDestinoId);
            InputParameterAdd.Int(objCommand, "establecimientoEnvioId", model.EstablecimientoEnvioId);
            InputParameterAdd.DateTime(objCommand, "fechaRecepcion", model.FechaRecepcion);
            InputParameterAdd.DateTime(objCommand, "fechaRecepcionRomINS", model.fechaRecepcionRomINS);
            InputParameterAdd.Int(objCommand, "idUsuario", model.IdUsuario);
            InputParameterAdd.Varchar(objCommand, "nroOficio", model.NroOficio);

            var resultado = new List<OrdenMuestraRecepcionado>();
            var dataTable = Execute(objCommand);
            foreach (DataRow row in dataTable.Rows)
            {
                resultado.Add(new OrdenMuestraRecepcionado
                {
                    CodigoMuestra = Converter.GetString(row, "CodigoMuestra"),
                    CodigoLineal = Converter.GetString(row, "CodigoLineal"),
                    IdOrdenMuestraRecepcion = Converter.GetGuid(row, "idOrdenMuestraRecepcion"),
                    Paciente = Converter.GetString(row, "Paciente"),
                    IdOrdenMuestra = Converter.GetGuid(row, "idOrdenMuestra"),
                    IdTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                    IdCriterioRechazo = Converter.GetInt(row, "idCriterioRechazo"),
                    CriterioRechazoGlosa = Converter.GetString(row, "Glosa")
                });
            }

            return resultado;
        }

        public bool VerificarMuestraPorRecepcionar(string codigoMuestra)
        {
            var objCommand = GetSqlCommand("pNLS_VerificarMuestraPorRecepcionar");
            InputParameterAdd.Varchar(objCommand, "codigoMuestra", codigoMuestra);
            var dataTable = Execute(objCommand);
            return dataTable.Rows.Count != 0;
        }

        public void RechazarMuestra(OrdenMuestraRecepcionados muestra)
        {
            var objCommand = GetSqlCommand("pNLI_RecepcionMuestraROMRechazo");
            InputParameterAdd.Guid(objCommand, "idOrdenMuestraRecepcion", muestra.IdOrdenMuestraRecepcion);
            InputParameterAdd.Varchar(objCommand, "rechazos", string.Join(",", muestra.MotivoRechazo));
            InputParameterAdd.Int(objCommand, "idUsuario", muestra.IdUsuario);
            ExecuteNonQuery(objCommand);
        }
    }
}

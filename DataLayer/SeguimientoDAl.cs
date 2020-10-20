using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class SeguimientoDAl : DaoBase
    {
        public SeguimientoDAl(IDalSettings settings) : base(settings)
        {
        }

        public SeguimientoDAl() : this(new NetlabSettings())
        {
        }

        public List<CCSeguimientoCab> GetSeguimientos(DateTime FechaInicio, DateTime FechaFin, int IdEstablecimiento, int idEnfermedad, Guid idExamen)
        {
            var objCommand = GetSqlCommand("pNLS_GCCSeguimiento");
            InputParameterAdd.DateTime(objCommand, "fechainicio", FechaInicio);
            InputParameterAdd.DateTime(objCommand, "fechafin", FechaFin);
            InputParameterAdd.Int(objCommand, "IdEstablecimiento", IdEstablecimiento);
            InputParameterAdd.Int(objCommand, "idEnfermedad", idEnfermedad);
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);

            return Execute(objCommand).AsEnumerable().Select(dr => new CCSeguimientoCab()
            {
                idseguimiento = Converter.GetInt(dr, "idseguimiento"),
                idEstablecimiento = Converter.GetInt(dr, "idEstablecimiento"),
                idEnfermedad = Converter.GetInt(dr, "idEstablecimiento"),
                idExamen = Converter.GetGuid(dr, "idExamen"),
                NombreEstablecimiento = Converter.GetString(dr, "NombreEstablecimiento"),
                NombreEnfermedad = Converter.GetString(dr, "NombreEnfermedad"),
                NombreExamen = Converter.GetString(dr, "NombreExamen"),
                ejecutadx = Converter.GetInt(dr, "ejecutadx"),
                ejecutacc = Converter.GetInt(dr, "ejecutacc"),
                cumpliocc = Converter.GetInt(dr, "cumpliocc"),
                NombreUsuario = Converter.GetString(dr, "UsuarioRegistro"),
                mca_inh = Converter.GetInt(dr, "mca_inh"),
                idusuarioregistro = Converter.GetInt(dr, "idusuarioregistro"),
                idusuarioedicion = Converter.GetInt(dr, "idusuarioedicion"),
                fecharegistro = Converter.GetDateTime(dr, "fecharegistro"),
                fechaedicion = Converter.GetDateTime(dr, "fechaedicion")
            }).ToList();
        }
        public Guid InsertSeguimiento(CCSeguimientoCab oCCSeguimientoCab)
        {
            var objCommand = GetSqlCommand("pNLI_GCCSeguimiento");

            InputParameterAdd.Int(objCommand, "idEstablecimiento", oCCSeguimientoCab.idEstablecimiento);
            InputParameterAdd.Int(objCommand, "idEnfermedad", oCCSeguimientoCab.idEnfermedad);
            InputParameterAdd.Guid(objCommand, "idExamen", oCCSeguimientoCab.idExamen);
            InputParameterAdd.Int(objCommand, "ejecutadx", oCCSeguimientoCab.ejecutadx);
            InputParameterAdd.Int(objCommand, "ejecutacc", oCCSeguimientoCab.ejecutacc);
            InputParameterAdd.Int(objCommand, "cumpliocc", oCCSeguimientoCab.cumpliocc);
            InputParameterAdd.Int(objCommand, "estado", oCCSeguimientoCab.mca_inh);
            InputParameterAdd.Int(objCommand, "idusuarioregistro", oCCSeguimientoCab.idusuarioregistro);
            OutputParameterAdd.UniqueIdentifier(objCommand, "newIdSeguimiento");
            
            ExecuteNonQuery(objCommand);

            return (Guid)objCommand.Parameters["@newIdSeguimiento"].Value;
        }
        public void InsertSeguimientoDetalle(CCSEguimientoDetalle oCCSeguimientoDetalle)
        {
            var objCommand = GetSqlCommand("pNLI_GCCSeguimientoDetalle");

            InputParameterAdd.Guid(objCommand, "idSeguimiento", oCCSeguimientoDetalle.idseguimiento);
            InputParameterAdd.Int(objCommand, "tipoinfraestructura", oCCSeguimientoDetalle.tipoinfraestructura);
            InputParameterAdd.Int(objCommand, "estadoequipo", oCCSeguimientoDetalle.estadoequipo);
            InputParameterAdd.Int(objCommand, "estadomaterial", oCCSeguimientoDetalle.estadomaterial);
            InputParameterAdd.Int(objCommand, "tipopersonal", oCCSeguimientoDetalle.personal);
            //InputParameterAdd.Varchar(objCommand, "otros", oCCSeguimientoDetalle.otros);
            InputParameterAdd.Int(objCommand, "idusuarioregistro", oCCSeguimientoDetalle.idusuarioregistro);

            ExecuteNonQuery(objCommand);
        }
    }
}

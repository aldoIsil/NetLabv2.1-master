using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using Model;
using Model.Temp;

namespace DataLayer
{
    public class TempOrdenDal : DaoBase
    {
        public TempOrdenDal(IDalSettings settings) : base(settings)
        {
        }

        public TempOrdenDal() : this(new NetlabSettings())
        {

        }
        public Guid InsertTempOrden(TempModel.TempOrden oTempOrden)
        {
            var objCommand = GetSqlCommand("pNLI_TempOrden");
            InputParameterAdd.Guid(objCommand, "idOrden", oTempOrden.IdOrden);
            InputParameterAdd.Guid(objCommand, "idPaciente", oTempOrden.IdPaciente);
            InputParameterAdd.Int(objCommand, "idProyecto", oTempOrden.IdProyecto);
            InputParameterAdd.Int(objCommand, "estatus", oTempOrden.estatus);
            InputParameterAdd.Int(objCommand, "estado", oTempOrden.estado);
            InputParameterAdd.Varchar(objCommand, "observacion", oTempOrden.observacion);
            InputParameterAdd.Int(objCommand, "idUsuario", oTempOrden.idUsuario);
            ExecuteNonQuery(objCommand);
            return oTempOrden.IdOrden;
        }
        public void UpdateTempOrden(TempModel.TempOrden oTempOrden)
        {
            var objCommand = GetSqlCommand("pNLU_TempOrden");
            InputParameterAdd.Guid(objCommand, "idPaciente", oTempOrden.IdPaciente);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", oTempOrden.IdEstablecimiento);
            InputParameterAdd.Int(objCommand, "idUsuario", oTempOrden.idUsuario);
            InputParameterAdd.Varchar(objCommand, "nroCelular", oTempOrden.nroCelularPaciente);
            
            ExecuteNonQuery(objCommand);
        }
        public void InsertTempOrdenDatoClinico(OrdenDatoClinico ordenDatoClinico, Guid idPaciente)
        {
            var objCommand = GetSqlCommand("pNLI_TempOrdenDatoClinico");
            InputParameterAdd.Guid(objCommand, "idPaciente", idPaciente);
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
        public void InsertTempOrdenMuestra(TempModel.TempOrdenMuestra oTempOrdenMuestra, Guid idPaciente, Guid idExamen, int idtipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLI_TempOrdenMuestra");
            InputParameterAdd.Guid(objCommand, "idPaciente", idPaciente);
            InputParameterAdd.Guid(objCommand, "idExamen", idExamen);
            InputParameterAdd.Int(objCommand, "idTIpoMuestra", oTempOrdenMuestra.IdTipoMuestra);
            InputParameterAdd.DateTime(objCommand, "fechaColeccion", oTempOrdenMuestra.FechaColeccion);
            InputParameterAdd.Int(objCommand, "idusuario", oTempOrdenMuestra.idusuario);

            ExecuteNonQuery(objCommand);
        }
        public void DeleteTempOrden(Guid idPaciente, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLD_TempOrden");
            InputParameterAdd.Guid(objCommand, "idPaciente", idPaciente);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            ExecuteNonQuery(objCommand);
        }
        public void DeleteTempOrdenbyNroDocumento(Guid idOrden, int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLD_TempOrdenbyNroDocumento");
            InputParameterAdd.Guid(objCommand, "idOrden", idOrden);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            ExecuteNonQuery(objCommand);
        }
        public void DeleteTempOrden(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLD_TempOrdenbyIdUsuario");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            ExecuteNonQuery(objCommand);
        }
        public void InsertPreRecepcion(string tipoValidacion, string Nombres, string apePaterno, string apeMaterno, string nroDocumento, string tipoDocumento, int idusuario)
        {
            var objCommand = GetSqlCommand("pNLI_MuestraPreRecepcion");
            InputParameterAdd.Varchar(objCommand, "tipoValidacion", tipoValidacion);
            InputParameterAdd.Varchar(objCommand, "Nombre", Nombres);
            InputParameterAdd.Varchar(objCommand, "apePaterno", apePaterno);
            InputParameterAdd.Varchar(objCommand, "apeMaterno", apeMaterno);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            InputParameterAdd.Varchar(objCommand, "tipoDocumento", tipoDocumento);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", idusuario);

            ExecuteNonQuery(objCommand);
        }
    }
}

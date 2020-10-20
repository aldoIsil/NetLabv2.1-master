using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer
{
    public class NetlabDal : DaoBase
    {
        public NetlabDal(IDalSettings settings) : base(settings)
        {
        }

        public NetlabDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Registra informacion del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="paciente"></param>
        public void InsertPaciente(Paciente paciente)
        {
            var objCommand = GetSqlCommand("pNLI_Paciente");

            InputParameterAdd.Varchar(objCommand, "apellidoPaterno", paciente.ApellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apellidoMaterno", paciente.ApellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombres", paciente.Nombres);
            InputParameterAdd.Int(objCommand, "genero", paciente.Genero);
            InputParameterAdd.DateTime(objCommand, "fechaNacimiento", paciente.FechaNacimiento);
            InputParameterAdd.Int(objCommand, "tipoDocumento", paciente.TipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", paciente.NroDocumento);            
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", paciente.IdUsuarioRegistro);
            /*InputParameterAdd.Varchar(objCommand, "idUbigeoReniec", paciente.DatoComplementario.IdUbigeo);
            InputParameterAdd.Varchar(objCommand, "direccionReniec", paciente.DatoComplementario.DireccionReniec);
            InputParameterAdd.Varchar(objCommand, "idUbigeoActual", paciente.DatoComplementario.IdUbigeoActual);
            InputParameterAdd.Varchar(objCommand, "direccionActual", paciente.DatoComplementario.DireccionActual);
            InputParameterAdd.Varchar(objCommand, "telefonoFijo", paciente.DatoComplementario.TelefonoFijo);
            InputParameterAdd.Varchar(objCommand, "celular1", paciente.DatoComplementario.Celular1);
            InputParameterAdd.Varchar(objCommand, "celular2", paciente.DatoComplementario.Celular2);
            InputParameterAdd.Int(objCommand, "estado", paciente.DatoComplementario.Estado);           
            */
            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualizar informacion del paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="paciente"></param>
        public void UpdatePaciente(Paciente paciente)
        {
            var objCommand = GetSqlCommand("pNLU_Paciente");

            InputParameterAdd.Guid(objCommand, "idPaciente", paciente.IdPaciente);
            InputParameterAdd.Varchar(objCommand, "apellidoPaterno", paciente.ApellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apellidoMaterno", paciente.ApellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombres", paciente.Nombres);
            InputParameterAdd.Int(objCommand, "genero", paciente.Genero);
            InputParameterAdd.DateTime(objCommand, "fechaNacimiento", paciente.FechaNacimiento);
            InputParameterAdd.Int(objCommand, "tipoDocumento", paciente.TipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", paciente.NroDocumento);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", paciente.IdUsuarioEdicion);
            InputParameterAdd.Varchar(objCommand, "idUbigeoReniec", paciente.UbigeoReniec.Id);
            InputParameterAdd.Varchar(objCommand, "direccionReniec", paciente.DireccionReniec);
            InputParameterAdd.Varchar(objCommand, "idUbigeoActual", paciente.UbigeoActual.Id);
            InputParameterAdd.Varchar(objCommand, "direccionActual", paciente.DireccionActual);
            InputParameterAdd.Varchar(objCommand, "telefonoFijo", paciente.TelefonoFijo);
            InputParameterAdd.Varchar(objCommand, "celular1", paciente.Celular1);
            InputParameterAdd.Varchar(objCommand, "celular2", paciente.Celular2);
            InputParameterAdd.Int(objCommand, "estado", paciente.Estado);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Obtiene información del paciente filtrado por el nro de documento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <returns></returns>
        public Paciente GetPacienteByDocumento(string nroDocumento)
        {
            var objCommand = GetSqlCommand("pNLS_PacienteByDocumento");
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);

            return NetlabConvertTo.Paciente(Execute(objCommand));
        }
    }
}

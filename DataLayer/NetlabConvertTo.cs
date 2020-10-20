using Framework.DAL;
using System.Data;
using Model;

namespace DataLayer
{
    public class NetlabConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte informacion de un data table a una Entidad Paciente.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios, funcion no utilizada.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static Paciente Paciente(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new Paciente
            {
                IdPaciente = GetGuid(row, "idPaciente"),
                ApellidoMaterno = row["apellidoMaterno"].ToString(),
                ApellidoPaterno = row["apellidoPaterno"].ToString(),
                Nombres = row["nombres"].ToString(),
                Genero = GetInt(row, "genero"),
                FechaNacimiento = GetDateTime(row, "fechaNacimiento"),
                TipoDocumento = GetInt(row, "tipoDocumento"),
                NroDocumento = row["nroDocumento"].ToString(),
                Estado = GetInt(row, "estado"),
                UbigeoReniec = new Ubigeo { Id = row["idUbigeoReniec"].ToString() },
                DireccionReniec = row["direccionReniec"].ToString(),
                UbigeoActual = new Ubigeo { Id = row["idUbigeoActual"].ToString() },
                DireccionActual = row["direccionActual"].ToString(),
                TelefonoFijo = row["telefonoFijo"].ToString(),
                Celular1 = row["celular1"].ToString(),
                Celular2 = row["celular2"].ToString()
            };
        }
    }
}

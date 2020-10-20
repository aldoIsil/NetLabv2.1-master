using System.Data;
using Framework.DAL;
using Model;

namespace DataLayer.DalConverter
{
    public class OrdenResultadoConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte informacion de un data table en la entidad OrdenResultado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: 1. Se agregaron comentarios.
        ///               2. Se modifico el "DireccionEESSOrigen" por "SolDireccionEESS" - Marcos Mejia 10-05-2018
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static OrdenResultado Orden(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new OrdenResultado
            {
                idOrden = GetGuid(row, "idOrden"),
                NroSolicitud = GetString(row, "nroSolicitud"),
                NroOficio = GetString(row, "nroOficio"),
                FechaSolicitud = GetDateTime(row, "fechaRegistro"),
                Paciente = GetString(row, "paciente"),
                DireccionActual = GetString(row, "direccionActual"),
                MedicoTratante = GetString(row, "medicotratante"),
                FechaNacimiento = GetString(row, "fechaNacimiento"),
                FechadeSolicitud = GetString(row, "FechaSolicitud"),
                Genero = GetString(row, "genero"),
                Establecimiento = GetString(row, "establecimiento"),
                Validador = GetValidador(row),
                Liberador = GetLiberador(row),
                Observaciones = GetString(row, "observacion"),
                nombrePaciente = GetString(row, "nombrePaciente"),
                fechaNacimiento = GetDateTime(row, "fechaNacimiento"),
                EstablecimientoSolicitante = GetString(row, "EstablecimientoSolicitante"),
                DocIdentidad = GetString(row, "DocIdentidad"),
                codigoOrden = GetString(row, "codigoOrden"),
                Edad = GetString(row, "Edad"),
                Sexo = GetString(row, "Sexo"),
                Telefono = GetString(row, "Telefono"),
                Direccion = GetString(row, "Direccion"),
                Solicitante = GetString(row, "Solicitante"),
                DocIdentidadSol = GetString(row, "DniSolicitante"),
                Ubicacion = GetString(row, "SolDireccionEESS"),
                EstablecimientoDestino = GetString(row, "EESS_LAB_Destino"),
                NumeroInforme = GetInt(row, "numeroInforme"),
                FechaIngresoInsRom = GetString(row, "fechaRecepcionRomINS"),
                TipoDocumento = GetString(row, "TipoDocumento"),
                idLaboratorioDestino = GetInt(row, "idLaboratorioDestino"),
                Inacal = GetInt(row,"Inacal")
            };
        }
        /// <summary>
        /// Descripción: Obtiene informacion de un data row y retorna una entidad UsuarioResultado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static UsuarioResultado GetValidador(DataRow row)
        {
            return new UsuarioResultado
            {
                Nombre = GetString(row, "validador"),
                Cargo = GetString(row, "cargoValidador"),
                Colegio = GetString(row, "colegioValidador"),
                FirmaDigital = GetBytes(row, "firmaValidador")
            };
        }
        /// <summary>
        /// Descripción: Obtiene informacion de un data row y retorna una entidad UsuarioResultado.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static UsuarioResultado GetLiberador(DataRow row)
        {
            return new UsuarioResultado
            {
                Nombre = GetString(row, "liberador"),
                Cargo = GetString(row, "cargoLiberador"),
                Colegio = GetString(row, "colegioLiberador"),
                FirmaDigital = GetBytes(row, "firmaLiberador")
            };
        }


        public static ResultadoKobos Kobos(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new ResultadoKobos
            {
                idPruebaRapidaKobo = GetInt(row, "idPruebaRapidaKobo"),
                tipoDocumento = GetString(row, "tipoDocumento"),
                nroDocumento = GetString(row, "nroDocumento"),
                Nombres = GetString(row, "Nombres"),
                apellidoPaterno = GetString(row, "apellidoPaterno"),
                apellidoMaterno = GetString(row, "apellidoMaterno"),
                direccion = GetString(row, "direccion"),
                edad = GetInt(row, "edad"),
                ejecutorPrueba = GetString(row, "ejecutorPrueba"),
                establecimientoEjecutor = GetString(row, "establecimientoEjecutor"),
                fechaObtencion = GetString(row, "fechaObtencion"),
                FechaResultado = GetString(row, "FechaResultado"),
                Resultado = GetString(row, "Resultado"),
                nroInforme = GetString(row, "nroInforme")
            };
        }
        public static CargoUsuarioEstablecimiento CargoUsuarioEstablecimiento(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            return new CargoUsuarioEstablecimiento
            {
                cargo = GetString(row, "cargo"),
                nombresUsuario = GetString(row, "nombreUsuario")
            };
        }
    }
}
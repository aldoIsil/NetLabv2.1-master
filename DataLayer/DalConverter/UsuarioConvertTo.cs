using Framework.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;

namespace DataLayer.DalConverter
{
    public class UsuarioConvertTo : Converter
    {
        /// <summary>
        /// Descripción: Convierte un data table a una lista tipada con la entidad Usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Usuario> Usuarios(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(GetUsuario).ToList();
        }

        public static Usuario Usuario(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];

            Usuario usuario = new Usuario();
            usuario.idUsuario = GetInt(row, "idUsuario");
            usuario.apellidoPaterno = row["apellidoPaterno"].ToString();
            usuario.apellidoMaterno = row["apellidoMaterno"].ToString();
            usuario.nombres = row["nombres"].ToString();
            usuario.iniciales = row["iniciales"].ToString();
            usuario.codigoColegio = row["codigoColegio"].ToString();
            usuario.RNE = row["RNE"].ToString();
            usuario.correo = row["correo"].ToString();
            if (row["contrasenia"] != DBNull.Value)
                usuario.contrasenia = (byte[])row["contrasenia"];
            if (!row.IsNull("fechaIngreso"))
                usuario.fechaIngreso = Converter.GetDateTime(row, "fechaIngreso");
            if (!row.IsNull("fechaCaducidad"))
                usuario.fechaCaducidad = Converter.GetDateTime(row, "fechaCaducidad");
            if (row["firmadigital"] != DBNull.Value)
                usuario.firmaDigital = (byte[])row["firmadigital"];

            return usuario;
        }

        private static Usuario GetUsuario(DataRow row)
        {
            return new Usuario
            {
                idUsuario = GetInt(row, "idUsuario"),
                apellidoPaterno = row["apellidoPaterno"].ToString(),
                apellidoMaterno = row["apellidoMaterno"].ToString(),
                nombres = row["nombres"].ToString()
            };
        }
    }
}

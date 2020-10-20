using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using System.Data;
using System;
using DataLayer.DalConverter;
using Model;
using System.Data.SqlClient;
using System.Configuration;

using System.Net;
using System.Net.NetworkInformation;

namespace DataLayer
{
    public class UsuarioDal : DaoBase
    {
        public UsuarioDal(IDalSettings settings) : base(settings)
        {
        }

        public UsuarioDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripcion: Obtiene los permisos de un usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Menu> getMenuUsuario(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_MenuItemsByUser");
            InputParameterAdd.Int(objCommand, "idUser", idUsuario);

            return UsuarioConvertTo.ListMenu(Execute(objCommand));
        }
        /// <summary>
        /// Descripcion: Obtiene el estado actual del Login(Usuario), puede estar:
        /// CONTRASEÑA ERRADA.
        /// CLAVE CADUCADA.
        /// USUARIO NUEVO
        /// CONTRASEÑA CADUCADA
        /// USUARIO DESACTIVADO
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="clave"></param>
        /// <returns></returns>
        public Usuario LoginUsuario(string usuario, string clave, string ipaddress = "")
        {
            var objCommand = GetSqlCommand("pNLS_LoginUsuario");
            InputParameterAdd.Varchar(objCommand, "login", usuario);
            InputParameterAdd.Varchar(objCommand, "pass", clave);
            InputParameterAdd.Varchar(objCommand, "ipaddress", ipaddress);

            return UsuarioConvertTo.Usuario(Execute(objCommand));
        }
        /// <summary>
        /// Descripcion: Obtiene los establecimientos de un usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns>DataTable con los establecimientos</returns>
        public Establecimiento getEstablecimientoUsuario(int idUsuario)
        {
            var objCommand = GetSqlCommand("[pNLS_EstablecimientoByUser]");
            InputParameterAdd.Int(objCommand, "idUser", idUsuario);

            return UsuarioConvertTo.Establecimiento(Execute(objCommand));
        }
        /// <summary>
        /// Descripcion: Obtiene los establecimientos de un usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns>Lista de Establecimientos</returns>
        public List<Establecimiento> getEstablecimientosUsuario(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_EstablecimientoByUser");
            InputParameterAdd.Int(objCommand, "idUser", idUsuario);

            return UsuarioConvertTo.ListEstablecimiento(Execute(objCommand));
        }
        /// <summary>
        /// Descripcion: Obtiene los laboratorios de un usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns>Lista de Laboratorios</returns>
        public List<Laboratorio> getLaboratoriosUsuario(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_LaboratorioByUser");
            InputParameterAdd.Int(objCommand, "idUser", idUsuario);

            return UsuarioConvertTo.ListLaboratorio(Execute(objCommand));
        }

        /// <summary>
        /// Descripcion: Obtiene los Instituciones de un usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns>List<Institucion></returns>
        //combos dependientes(selectLocal)
        public List<Institucion> getInstitucionxUsuario(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_InstitucionByUsuario");
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            return UsuarioConvertTo.ListInstitucion(Execute(objCommand));

        }
        /// <summary>
        /// Descripcion: Obtiene los DISAS de un usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="IdInstitucion"></param>
        /// <returns>List<DISA></returns>
        public List<DISA> getDisaxUsuario(int idUsuario, int IdInstitucion)
        {
            var objCommand = GetSqlCommand("pNLS_DISAbyInstitucionByUsuario");
            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "IdInstitucion", IdInstitucion);

            return UsuarioConvertTo.ListDisa(Execute(objCommand));

        }


        /// <summary>
        /// Descripcion: Envia y actualiza password del usuario.
        /// 'Se envio su nueva clave a su correo'
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idtipo"></param>
        /// <param name="correo"></param>
        /// <returns></returns>
        public Usuario ActualizarPasword(int idtipo, string correo)
        {
            var usuario = new Usuario();
            var objCommand = GetSqlCommand("pNLU_Recuperar_Password");
            InputParameterAdd.Int(objCommand, "Tipo", idtipo);
            InputParameterAdd.Varchar(objCommand, "Cuenta", correo);

            DataSet ds = ExecuteDataSet(objCommand);
            DataTable dt1 = ds.Tables[0];
            
            if (dt1.Rows.Count == 0)
                return null;
            var row = dt1.Rows[0];
            usuario.condicionLaboral = row["texto"].ToString();

            if (ds.Tables.Count > 1)
            {
                DataTable dt2 = ds.Tables[1];
                var row2 = dt2.Rows[0];
                usuario.correo = row2["correo"].ToString();
            }
            return usuario;

        }
        /// <summary>
        /// Descripcion: Actualiza password del usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idusuario"></param>
        /// <param name="newpassword"></param>
        /// <returns></returns>
        public int ActualizarPaswordUsuarioNuevo(int idusuario, string newpassword)
        {
            int rpta;

            try
            {
                var objCommand = GetSqlCommand("pNLU_Usuario_Password");
                InputParameterAdd.Int(objCommand, "IdUsuario", idusuario);
                InputParameterAdd.Varchar(objCommand, "NuevaPassword", newpassword);

                ExecuteNonQuery(objCommand);
                rpta = 1;
            }
            catch (Exception)
            {
                rpta = 0;
            }


            return rpta;
        }
        /// <summary>
        /// Descripcion: Inserta nuevo usuario222
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public string InsertUsuario(Usuario usuario)
        {
            var objCommand = GetSqlCommand("pNLI_Usuario");
            string texto = "";
            var MsjError = string.Empty;
            try
            {
                InputParameterAdd.Varchar(objCommand, "login", usuario.login);
                InputParameterAdd.Varchar(objCommand, "dni", usuario.documentoIdentidad);
                InputParameterAdd.Varchar(objCommand, "apellidoPaterno", usuario.apellidoPaterno);
                InputParameterAdd.Varchar(objCommand, "apellidoMaterno", usuario.apellidoMaterno);
                InputParameterAdd.Varchar(objCommand, "nombres", usuario.nombres);
                InputParameterAdd.Varchar(objCommand, "codigoColegio", usuario.codigoColegio);
                InputParameterAdd.Varchar(objCommand, "RNE", usuario.RNE);
                InputParameterAdd.Varchar(objCommand, "cargo", usuario.cargo);
                InputParameterAdd.Varchar(objCommand, "correo", usuario.correo);
                InputParameterAdd.Int(objCommand, "estatus", usuario.estatus);
                InputParameterAdd.Int(objCommand, "idUsuarioRegistro", usuario.IdUsuarioRegistro);
                InputParameterAdd.Varchar(objCommand, "telefono", usuario.telefono);
                InputParameterAdd.Int(objCommand, "tiempoCaducidad", usuario.tiempoCaducidad);
                InputParameterAdd.Int(objCommand, "idProfesion", usuario.profesion);
                InputParameterAdd.Int(objCommand, "tipo", usuario.tipo);
                InputParameterAdd.Int(objCommand, "componente", usuario.componente);
                InputParameterAdd.Int(objCommand, "tipoUsuario", usuario.idTipoUsuario);
                InputParameterAdd.Int(objCommand, "nAprobacion", usuario.nAprobacion);
                InputParameterAdd.VarBinary(objCommand, "firmadigital", usuario.firmaDigital);
                InputParameterAdd.Varchar(objCommand, "contrasenia", usuario.login);
                //InputParameterAdd.Int(objCommand, "idEstablecimiento", usuario.Establecimiento);

                //DataSet dataSet = ExecuteDataSet(objCommand);

                DataTable dataUsuario = Execute(objCommand);
                //DataTable dataError = dataSet.Tables[1];


                if (dataUsuario.Rows.Count == 0)
                    return "0";
                for (int i = 0; i < dataUsuario.Rows.Count; i++)
                {
                    var row = dataUsuario.Rows[i];
                    texto = row["texto"].ToString();
                    //Contrasenia = row["Contrasenia"].ToString();
                }

                //for (int i = 0; i < dataError.Rows.Count; i++)
                //{
                //    var row = dataError.Rows[i];
                //    MsjError = Converter.GetString(row, "MsjError");
                //}
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return texto;
            //return Contrasenia;
            //return String.IsNullOrEmpty(MsjError) ? nuevoUsuarioId.ToString(): MsjError;
        }
        /// <summary>
        /// Descripcion: Actualiza usuario
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        public void UpdateUsuario(Usuario usuario)
        {
            var objCommand = GetSqlCommand("pNLU_Usuario");

            InputParameterAdd.Int(objCommand, "idUsuario", usuario.idUsuario);
            InputParameterAdd.Varchar(objCommand, "login", usuario.login);
            InputParameterAdd.Varchar(objCommand, "dni", usuario.documentoIdentidad);
            InputParameterAdd.Varchar(objCommand, "apellidoPaterno", usuario.apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apellidoMaterno", usuario.apellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombres", usuario.nombres);
            InputParameterAdd.Varchar(objCommand, "codigoColegio", usuario.codigoColegio);
            InputParameterAdd.Varchar(objCommand, "RNE", usuario.RNE);
            InputParameterAdd.Varchar(objCommand, "cargo", usuario.cargo);
            InputParameterAdd.Varchar(objCommand, "correo", usuario.correo);
            InputParameterAdd.Int(objCommand, "estatus", usuario.estatus);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", usuario.IdUsuarioEdicion);
            InputParameterAdd.Varchar(objCommand, "telefono", usuario.telefono);
            InputParameterAdd.Int(objCommand, "tiempoCaducidad", usuario.tiempoCaducidad);
            InputParameterAdd.Int(objCommand, "idProfesion", usuario.profesion);
            InputParameterAdd.Int(objCommand, "tipo", usuario.tipo);
            InputParameterAdd.VarBinary(objCommand, "firmadigital", usuario.firmaDigital);
            InputParameterAdd.Int(objCommand, "Estado", usuario.estado);
            InputParameterAdd.Int(objCommand, "Renovacion", usuario.Renovacion);
            InputParameterAdd.Int(objCommand, "componente", usuario.componente);
            InputParameterAdd.Int(objCommand, "tipoUsuario", usuario.idTipoUsuario);
            InputParameterAdd.Int(objCommand, "nAprobacion", usuario.nAprobacion);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripcion: Buscar un usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <param name="nombres"></param>
        /// <param name="dni"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public List<Usuario> SearchUsuario(string login, string apellidoPaterno, string apellidoMaterno, string nombres, string dni, int region)
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            string consulta = "pNLS_Usuario";
            if (region == 1) //usuario region
            {
                consulta = "pNLS_UsuarioRegion";
            }

            var objCommand = GetSqlCommand(consulta);
            InputParameterAdd.Varchar(objCommand, "login", login);
            InputParameterAdd.Varchar(objCommand, "apellidoPaterno", apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apellidoMaterno", apellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombres", nombres);
            InputParameterAdd.Varchar(objCommand, "documentoIdentidad", dni);

            DataTable dataUsuarios = Execute(objCommand);

            if (dataUsuarios.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataUsuarios.Rows.Count; i++)
            {
                var row = dataUsuarios.Rows[i];
                Usuario usuario = new Usuario();
                usuario.idUsuario = int.Parse(row["idUsuario"].ToString());
                usuario.login = row["login"].ToString();
                usuario.documentoIdentidad = row["documentoIdentidad"].ToString();
                usuario.apellidoMaterno = row["apellidoMaterno"].ToString();
                usuario.apellidoPaterno = row["apellidoPaterno"].ToString();
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
                if (row["idTipoUsuario"] != DBNull.Value)
                    usuario.tipo = int.Parse(row["idTipoUsuario"].ToString()); //1:tipo admin | 2:tipo region
                usuario.estatus = int.Parse(row["estatus"].ToString()); //0: no validado | 1: validado
                usuario.estado = int.Parse(row["estado"].ToString());
                usuario.cargo = row["cargo"].ToString();
                usuario.profesion = int.Parse(row["idprofesion"].ToString());
                usuario.componente = int.Parse(row["idcomponente"].ToString());
                listaUsuarios.Add(usuario);
            }

            return listaUsuarios;
        }
        /// <summary>
        /// Descripcion: Obtiene la información de un usuario por el ID
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Usuario GetUsuarioById(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_UsuarioById");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);

            return UsuarioConvertTo.Usuario(Execute(objCommand));
        }
        /// <summary>
        /// Descripcion: Obtiene los roles de un usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Rol> GetRolesByIdUser(int idUsuario)
        {
            List<Rol> listaRolesbyUsuario = new List<Rol>();

            var objCommand = GetSqlCommand("pNLS_UsuarioRol");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);

            DataTable dataRoles = Execute(objCommand);

            if (dataRoles.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataRoles.Rows.Count; i++)
            {
                var row = dataRoles.Rows[i];
                Rol rol = new Rol();
                rol.idRol = int.Parse(row["idUsuario"].ToString());
                rol.nombre = row["nombre"].ToString();
                rol.descripcion = row["descripcion"].ToString();

                listaRolesbyUsuario.Add(rol);
            }

            return listaRolesbyUsuario;
        }
        /// <summary>
        /// Descripcion: Obtiene informacion de un Paciente a traves del DNI.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="dni"></param>
        /// <returns></returns>
        public Usuario BuscarPersonaPorDni(string dni)
        {
            Usuario usuario = new Usuario();
            var objCommand = GetSqlCommand("pNLS_PersonaByDni");
            InputParameterAdd.Varchar(objCommand, "dni", dni);

            DataTable dataRoles = Execute(objCommand);

            if (dataRoles.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataRoles.Rows.Count; i++)
            {
                var row = dataRoles.Rows[i];
                usuario.apellidoPaterno = row["apellidoPaterno"].ToString();
                usuario.apellidoMaterno = row["apellidoMaterno"].ToString();
                usuario.nombres = row["nombres"].ToString();
                usuario.estatus = 0;
            }

            return usuario;
        }
        /// <summary>
        /// Descripcion: Inserta los establecimientos de un usuario
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="listaUsuarioEstabecimiento"></param>
        public void InsertListaUsuarioEstablecimiento(List<UsuarioEstablecimiento> listaUsuarioEstabecimiento)
        {
            if (listaUsuarioEstabecimiento == null) return;

            foreach (var item in listaUsuarioEstabecimiento)
            {
                InsertUsuarioEstablecimiento(item);
            }
        }
        /// <summary>
        /// Descripcion:  Metodo que ejecuta el procedimiento en la base de datos para el registro de los establecimientos de un usuario.
        /// Alineado con el metodo InsertListaUsuarioEstablecimiento
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="usuarioEstablecimiento"></param>
        public void InsertUsuarioEstablecimiento(UsuarioEstablecimiento usuarioEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLI_UsuarioEstablecimiento");

            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", usuarioEstablecimiento.IdUsuarioRegistro);
            InputParameterAdd.Int(objCommand, "idUsuario", usuarioEstablecimiento.idUsuario);
            InputParameterAdd.Int(objCommand, "idInstitucion", usuarioEstablecimiento.idInstitucion);
            InputParameterAdd.Varchar(objCommand, "idDisa", usuarioEstablecimiento.idDISA);
            InputParameterAdd.Varchar(objCommand, "idRed", usuarioEstablecimiento.idRed);
            InputParameterAdd.Varchar(objCommand, "idMicrored", usuarioEstablecimiento.idMicrored);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", usuarioEstablecimiento.idEstablecimiento);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripcion: Verifica si existe el LOGIN ingresado
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        public int ExisteLogin(string login, int accion) //0: creacion | 1: edicion
        {
            int loginExitente = 0;

            var objCommand = GetSqlCommand("pNLS_UsuarioByLogin");
            InputParameterAdd.Varchar(objCommand, "login", login);
            string loginBD = "";
            DataTable data = Execute(objCommand);

            if (accion == 0)
            {
                if (data.Rows.Count == 0)
                    loginExitente = 0;
                else
                    loginExitente = 1;
            }
            else
            {
                if (data.Rows.Count == 0)
                    loginExitente = 0;
                else
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        var row = data.Rows[i];
                        loginBD = row["login"].ToString();
                    }
                    if (loginBD.Equals(login))
                    {
                        loginExitente = 0;
                    }
                    else
                    {
                        loginExitente = 1;
                    }
                }

            }


            return loginExitente;
        }
        /// <summary>
        /// Descripcion: Cambio de password de una cuenta.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="claveActual"></param>
        /// <param name="nuevaClave"></param>
        public void ChangePassword(int idUsuario, string claveActual, string nuevaClave)
        {
            var objCommand = GetSqlCommand("pNLU_Usuario_Password");

            InputParameterAdd.Int(objCommand, "IdUsuario", idUsuario);
            InputParameterAdd.Varchar(objCommand, "ActualPassword", claveActual);
            InputParameterAdd.Varchar(objCommand, "NuevaPassword", nuevaClave);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripcion: Verifica si existe el DNI de un paciente.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="dni"></param>
        /// <returns></returns>
        public bool ExisteDni(string dni)
        {
            var objCommand = GetSqlCommand("pNLS_UsuarioByDni");
            InputParameterAdd.Varchar(objCommand, "dni", dni);

            var dataTable = Execute(objCommand);

            return dataTable.Rows.Count > 0;
        }
        /// <summary>
        /// Descripcion: Obtiene el total de establecimientos con los que cuenta un usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public int GetTotalCountEstablecimientoByUsuario(int idUsuario)
        {
            var objCommand = GetSqlCommand("pNLS_TotalUsuarioEstablecimientoByUsuario");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);

            var dataTable = Execute(objCommand);

            return dataTable.Rows.Count > 0 ? Converter.GetInt(dataTable.Rows[0], "Total") : 0;
        }
        /// <summary>
        /// Descripcion: Obtiene el total de establecimientos con los que cuenta un usuario y los tranforma a una lista.
        /// El retorno de esta informacion sirve para la paginación.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="startIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public IEnumerable<UsuarioEstablecimiento> GetEstablecimientoByUsuario(int idUsuario, int startIndex, int maximumRows)
        {
            var objCommand = GetSqlCommand("pNLS_UsuarioEstablecimientoByUsuario");
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            InputParameterAdd.Int(objCommand, "startRowIndex", startIndex);
            InputParameterAdd.Int(objCommand, "maximumRows", maximumRows);

            return UsuarioEstablecimientoConvertTo.Establecimientos(Execute(objCommand));
        }
        /// <summary>
        /// Descripcion: Reenvia password al correo del usuario.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="id"></param>
        public string ResetearClave(int id)
        {
            string texto = "";
            var objCommand = GetSqlCommand("pNLS_Mail_Password");
            InputParameterAdd.Int(objCommand, "codusuario", id);

            DataTable dataUsuario = Execute(objCommand);
            if (dataUsuario.Rows.Count == 0)
                return "0";
            for (int i = 0; i < dataUsuario.Rows.Count; i++)
            {
                var row = dataUsuario.Rows[i];
                texto = row["texto"].ToString();
            }
            return texto;
        }

        public SolicitudUsuario InsertarSolicitudUsuario(SolicitudUsuario usuario)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarSolicitudUsuario");
            InputParameterAdd.Int(objCommand, "tipoSolicitud", usuario.tipoSolicitud);
            InputParameterAdd.Varchar(objCommand, "login", usuario.usuario.login);
            InputParameterAdd.Varchar(objCommand, "apePaterno", usuario.usuario.apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apeMaterno", usuario.usuario.apellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombre", usuario.usuario.nombres);
            InputParameterAdd.Int(objCommand, "tipoDocumento", usuario.usuario.tipoDocumento);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", usuario.usuario.documentoIdentidad);
            InputParameterAdd.Int(objCommand, "tipoUsuario", usuario.usuario.idTipoUsuario);
            InputParameterAdd.Varchar(objCommand, "cargo", usuario.usuario.cargo);
            InputParameterAdd.Varchar(objCommand, "condicionLaboral", usuario.usuario.condicionLaboral);
            InputParameterAdd.Int(objCommand, "profesion", usuario.usuario.profesion);
            InputParameterAdd.Varchar(objCommand, "codigoColegio", usuario.usuario.codigoColegio);
            InputParameterAdd.Varchar(objCommand, "iniciales", usuario.usuario.iniciales);
            InputParameterAdd.Varchar(objCommand, "correo", usuario.usuario.correo);
            InputParameterAdd.Varchar(objCommand, "telefono", usuario.usuario.telefono);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", usuario.usuario.Establecimiento);
            InputParameterAdd.Int(objCommand, "tipoEstablecimiento", usuario.usuario.tipo);
            InputParameterAdd.Int(objCommand, "componente", usuario.usuario.componente);
            InputParameterAdd.Varchar(objCommand, "JefeEESS", usuario.JefeEESS);
            InputParameterAdd.Varchar(objCommand, "cargoJf", usuario.cargoJf);
            OutputParameterAdd.Int(objCommand, "idSolicitudUsuario");
            ExecuteNonQuery(objCommand);
            usuario.idSolicitudUsuario = (int)objCommand.Parameters["@idSolicitudUsuario"].Value;
            return usuario;
        }
        public SolicitudUsuario InsertarExamenSolicitudUsuario(SolicitudUsuario usuario)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarExamenSolicitudUsuario");
            InputParameterAdd.Int(objCommand, "idSolicitudUsuario", usuario.idSolicitudUsuario);
            InputParameterAdd.Int(objCommand, "idExamenAgregado", usuario._examen.idExamenAgrupado);
            InputParameterAdd.Int(objCommand, "tipo", 1);
            ExecuteNonQuery(objCommand);
            return usuario;
        }

        public SolicitudUsuario InsertarRolSolicitudUsuario(SolicitudUsuario usuario)
        {
            var objCommand = GetSqlCommand("pNLI_InsertarRolSolicitudUsuario");
            InputParameterAdd.Int(objCommand, "idSolicitudUsuario", usuario.idSolicitudUsuario);
            InputParameterAdd.Int(objCommand, "idRol", usuario._rol.idRol);
            ExecuteNonQuery(objCommand);
            return usuario;
        }

        public List<SolicitudUsuario> ListaSolicitudUsuario(SolicitudUsuario usuario, string fechaDesde, string fechaHasta, int idEstablecimientoLog, int estado)
        {
            var objCommand = GetSqlCommand("pNLS_ListaSolicitudUsuario");
            InputParameterAdd.Varchar(objCommand, "fechaDesde", fechaDesde);
            InputParameterAdd.Varchar(objCommand, "fechaHasta", fechaHasta);
            InputParameterAdd.Varchar(objCommand, "apePaterno", usuario.usuario.apellidoPaterno);
            InputParameterAdd.Varchar(objCommand, "apeMaterno", usuario.usuario.apellidoMaterno);
            InputParameterAdd.Varchar(objCommand, "nombre", usuario.usuario.nombres);
            InputParameterAdd.Varchar(objCommand, "nroDocumento", usuario.usuario.documentoIdentidad);
            InputParameterAdd.Int(objCommand, "idUsuario", usuario.usuario.idUsuario);
            InputParameterAdd.Int(objCommand, "estado", estado);
            var dataTable = Execute(objCommand);

            List<SolicitudUsuario> _usuario = new List<SolicitudUsuario>();
            if (dataTable.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                SolicitudUsuario user = new SolicitudUsuario();
                user.usuario = new Usuario();
                user.idSolicitudUsuario = int.Parse(row["idSolicitudUsuario"].ToString());
                user.fechaEnvio = row["FechaEnvio"].ToString();
                user.tipoSolicitud = int.Parse(row["tipoSolicitud"].ToString());
                user.usuario.nombres = row["Usuario"].ToString();
                user.usuario.documentoIdentidad = row["Documento"].ToString();
                user.Establecimiento = row["EESS"].ToString();
                user.estado = int.Parse(row["estado"].ToString());
                _usuario.Add(user);
            }

            return _usuario;
        }

        public SolicitudUsuario GetSolicitudUsuario(int idSolicitudUsuario)
        {
            SolicitudUsuario usuario = null;
            var objCommand = GetSqlCommand("pNLS_GetSolicitudUsuario");
            InputParameterAdd.Int(objCommand, "idSolicitudUsuario", idSolicitudUsuario);
            DataSet dataSet = ExecuteDataSet(objCommand);
            DataTable solicitudUsuario = dataSet.Tables[0];
            DataTable solicitudPerfil = dataSet.Tables[1];
            DataTable solicitudExamen = dataSet.Tables[2];

            usuario = new SolicitudUsuario();
            usuario.usuario = new Usuario();

            #region Solicitud
            if (solicitudUsuario.Rows.Count == 0)
                return null;
            foreach (DataRow row in solicitudUsuario.Rows)
            {

                usuario.idSolicitudUsuario = Converter.GetInt(row, "idSolicitudUsuario");
                usuario.fechaEnvio = Converter.GetString(row, "FechaEnvio");
                usuario.tipoSolicitud = Converter.GetInt(row, "tipoSolicitud");
                usuario.usuario.nombres = Converter.GetString(row, "Usuario");
                usuario.usuario.tipoDoc = Converter.GetString(row, "TipoDocumento");
                usuario.usuario.documentoIdentidad = Converter.GetString(row, "Documento");
                usuario.Renipress = Converter.GetString(row, "Renipress");
                usuario.Establecimiento = Converter.GetString(row, "EESS");
                usuario.nombreInstitucion = Converter.GetString(row, "nombreInstitucion");
                usuario.nombreDisa = Converter.GetString(row, "nombreDisa");
                usuario.nombreRed = Converter.GetString(row, "nombreRed");
                usuario.nombreMicroRed = Converter.GetString(row, "nombreMicroRed");
                usuario.usuario.cargo = Converter.GetString(row, "cargo");
                usuario.usuario.condicionLaboral = Converter.GetString(row, "condicionLaboral");
                usuario.usuario.profesion = Converter.GetInt(row, "idProfesion");
                usuario.usuario._profesion = Converter.GetString(row, "Profesion");
                usuario.usuario.codigoColegio = Converter.GetString(row, "Colegiatura");
                usuario.usuario.correo = Converter.GetString(row, "correo");
                usuario.usuario.telefono = Converter.GetString(row, "telefono");
                usuario.usuario.idTipoUsuario = Converter.GetInt(row, "idTipoUsuario");
                usuario.usuario.TipoUsuario = Converter.GetString(row, "TipoUsuario");
                usuario.usuario.componente = Converter.GetInt(row, "idComponente");
                usuario.usuario.componenteUsuario = Converter.GetString(row, "Componente");
                usuario.estado = Converter.GetInt(row, "estado");
                usuario.idUsuarioVal1 = Converter.GetInt(row, "idUsuarioVal1");
                usuario.idUsuarioVal2 = Converter.GetInt(row, "idUsuarioVal2");
                usuario.fechaVal1 = Converter.GetString(row, "fechaValidacion1");
                usuario.fechaVal2 = Converter.GetString(row, "fechaValidacion2");
                usuario.validador1 = Converter.GetString(row, "Validador1");
                usuario.validador2 = Converter.GetString(row, "Validador2");
                usuario.comentario1 = Converter.GetString(row, "comentario");
                usuario.comentario2 = Converter.GetString(row, "comentario2");
            }
            #endregion
            #region Solicitud
            if (solicitudPerfil.Rows.Count == 0)
                return null;

            usuario.rol = new List<Rol>();
            Rol _rol = null;
            foreach (DataRow row in solicitudPerfil.Rows)
            {
                _rol = new Rol()
                {
                    nombre = Converter.GetString(row, "nombre")
                };
                usuario.rol.Add(_rol);
            }

            usuario.examen = new List<Examen>();
            Examen _examen = null;
            #endregion
            if (solicitudExamen.Rows.Count > 0)
            {
                foreach (DataRow row in solicitudExamen.Rows)
                {
                    _examen = new Examen()
                    {
                        nombre = Converter.GetString(row, "nombre")
                    };
                    usuario.examen.Add(_examen);
                }
            }
            return usuario;
        }

        public int ValidarSolicitudUsuario(SolicitudUsuario usuario)
        {
            int valor = 0;
            try
            {
                var objCommand = GetSqlCommand("pNLU_ValidarSolicitudUsuario");
                InputParameterAdd.Int(objCommand, "idSolicitudUsuario", usuario.idSolicitudUsuario);
                InputParameterAdd.Int(objCommand, "idUsuario", usuario.usuario.idUsuario);
                InputParameterAdd.Varchar(objCommand, "comentario", usuario.comentario1);
                InputParameterAdd.Int(objCommand, "estado", usuario.estado);
                OutputParameterAdd.Int(objCommand, "valor");
                ExecuteNonQuery(objCommand);
                valor = (int)objCommand.Parameters["@valor"].Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valor;
        }

        public List<SolicitudUsuario> GetEstadoSolicitud(string nroDocumento)
        {
            List<SolicitudUsuario> solicitud = new List<SolicitudUsuario>();
            var objCommand = GetSqlCommand("pNLS_GetEstadoSolicitud");
            InputParameterAdd.Varchar(objCommand, "nroDocumento", nroDocumento);
            var dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                SolicitudUsuario user = new SolicitudUsuario();
                user.usuario = new Usuario();
                user.idSolicitudUsuario = int.Parse(row["idSolicitudUsuario"].ToString());
                user.usuario.nombres = row["UserSolicitante"].ToString();
                user.fechaEnvio = row["FechaTramite"].ToString();
                user.estado = int.Parse(row["estado"].ToString());
                user.validador1 = row["Validador1"].ToString();
                user.validador2 = row["Validador2"].ToString();
                solicitud.Add(user);
            }
            return solicitud;
        }

        public int DevuelveCorrelativo(string NombreTabla)
        {
            int codigo = 0;
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionNetLabPrueba1"].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("NFL_SiguienteCorrelativo", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@sNombreTabla", System.Data.SqlDbType.VarChar)).Value = NombreTabla;

                    var reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        codigo = reader.GetInt32(reader.GetOrdinal("correlativo"));
                    }
                    conexion.Close();
                    conexion.Dispose();
                }
            }
            return codigo;
        }

        public void InsertarSolicitud(SolicitudUsuario net1)
        {
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionNetLabPrueba1"].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("NFL_SOLUsuario", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@iOperacion", System.Data.SqlDbType.VarChar)).Value = 3;
                    comando.Parameters.Add(new SqlParameter("@sLoginUsuario", System.Data.SqlDbType.VarChar)).Value = net1.usuario.login;
                    comando.Parameters.Add(new SqlParameter("@iCodUsuario", System.Data.SqlDbType.Int)).Value = net1.usuario.idUsuario;
                    comando.Parameters.Add(new SqlParameter("@sNombresUsuario", System.Data.SqlDbType.VarChar)).Value = net1.usuario.nombres;
                    comando.Parameters.Add(new SqlParameter("@sApellidosUsuario", System.Data.SqlDbType.VarChar)).Value = net1.usuario.apellidoPaterno;
                    comando.Parameters.Add(new SqlParameter("@sCadenaBuscar", System.Data.SqlDbType.VarChar)).Value = net1.Establecimiento;
                    comando.Parameters.Add(new SqlParameter("@colegio", System.Data.SqlDbType.VarChar)).Value = net1.usuario.codigoColegio;
                    comando.Parameters.Add(new SqlParameter("@sCorreo", System.Data.SqlDbType.VarChar)).Value = net1.usuario.correo;
                    comando.Parameters.Add(new SqlParameter("@sTelefono", System.Data.SqlDbType.VarChar)).Value = net1.usuario.telefono;
                    comando.Parameters.Add(new SqlParameter("@ccodTipo", System.Data.SqlDbType.VarChar)).Value = net1.usuario.TipoUsuario;
                    comando.Parameters.Add(new SqlParameter("@FechaObtencion", System.Data.SqlDbType.VarChar)).Value = net1.fechaEnvio;
                    comando.Parameters.Add(new SqlParameter("@FechaRecepcion", System.Data.SqlDbType.VarChar)).Value = net1.fechaEnvio;
                    comando.Parameters.Add(new SqlParameter("@Cargo", System.Data.SqlDbType.VarChar)).Value = "Otro (especifique)";
                    comando.Parameters.Add(new SqlParameter("@DNI", System.Data.SqlDbType.VarChar)).Value = net1.usuario.documentoIdentidad;
                    comando.Parameters.Add(new SqlParameter("@EstadoSol", System.Data.SqlDbType.VarChar)).Value = "PENDIENTE";
                    comando.Parameters.Add(new SqlParameter("@stIngresado", System.Data.SqlDbType.Int)).Value = 0;
                    comando.Parameters.Add(new SqlParameter("@sUsuarioAnt", System.Data.SqlDbType.VarChar)).Value = net1.usuario.login;
                    comando.Parameters.Add(new SqlParameter("@tipoSol", System.Data.SqlDbType.VarChar)).Value = net1.tipoSolicitud;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    conexion.Dispose();
                }
            }
        }

        public void InsertarArchivo(SolicitudUsuario net1)
        {
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionNetLabPrueba1"].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("NFL_MANTUsuarioArchivo", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@cCodArchivo", System.Data.SqlDbType.Int)).Value = net1.file.cCodArchivo;
                    comando.Parameters.Add(new SqlParameter("@iCodUsuario", System.Data.SqlDbType.Int)).Value = net1.file.Codigo;
                    comando.Parameters.Add(new SqlParameter("@dsNombre", System.Data.SqlDbType.VarChar)).Value = net1.file.dsNombre;
                    comando.Parameters.Add(new SqlParameter("@dsData", System.Data.SqlDbType.VarBinary)).Value = net1.file.dsData;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    conexion.Dispose();
                }
            }
        }

        public void InsertarAuditoria(SolicitudUsuario net1)
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }

            int iCodAuditoria = DevuelveCorrelativo("Auditoria");
            string[] NombreCampo = new string[3];
            NombreCampo[0] = "cCodUsuario";
            NombreCampo[1] = "sLoginUsuario";
            NombreCampo[2] = "sNombresUsuario";
            string[] ValorNuevo = new string[3];
            ValorNuevo[0] = Convert.ToString(net1.usuario.idUsuario);
            ValorNuevo[1] = net1.usuario.login;
            ValorNuevo[2] = net1.usuario.nombres;

            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionNetLabPrueba1"].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("NFL_InsertarAuditoria", conexion))
                {
                    conexion.Open();
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@iCodAuditoria", System.Data.SqlDbType.Int)).Value =iCodAuditoria;
                    comando.Parameters.Add(new SqlParameter("@sNombreTabla", System.Data.SqlDbType.VarChar)).Value = "UsuarioSol";
                    comando.Parameters.Add(new SqlParameter("@sIdRegistro", System.Data.SqlDbType.VarChar)).Value = net1.usuario.idUsuario;
                    comando.Parameters.Add(new SqlParameter("@iCodMenu", System.Data.SqlDbType.Int)).Value = 0;
                    comando.Parameters.Add(new SqlParameter("@iCodUsuario", System.Data.SqlDbType.Int)).Value = net1.usuario.idUsuario;
                    comando.Parameters.Add(new SqlParameter("@sIp", System.Data.SqlDbType.VarChar)).Value = localIP;
                    comando.Parameters.Add(new SqlParameter("@iOperacion", System.Data.SqlDbType.Int)).Value = 1;
                    comando.ExecuteNonQuery();
                    //conexion.Close();
                    //conexion.Dispose();
                }

                for (int i = 0; i < 3; i++)
                {
                    using (SqlCommand comando = new SqlCommand("NFL_InsertarDetalleAuditoria", conexion))
                    {
                        //conexion.Open();
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@iCodAuditoria", System.Data.SqlDbType.Int)).Value = iCodAuditoria;
                        comando.Parameters.Add(new SqlParameter("@sNombreCampo", System.Data.SqlDbType.VarChar)).Value = NombreCampo[i];
                        comando.Parameters.Add(new SqlParameter("@sValorAntiguo", System.Data.SqlDbType.VarChar)).Value = "";
                        comando.Parameters.Add(new SqlParameter("@sValorNuevo", System.Data.SqlDbType.VarChar)).Value = ValorNuevo[i];
                        comando.ExecuteNonQuery();
                        
                    }
                }
                conexion.Close();
                conexion.Dispose();
            }

        }

        public Usuario GetUsuarioByDocumento(string documentoIdentidad)
        {
            Usuario usuario = new Usuario();
            var objCommand = GetSqlCommand("pNLS_UsuarioByDni");
            InputParameterAdd.Varchar(objCommand, "dni", documentoIdentidad);
            var dataTable = Execute(objCommand);

            if (dataTable.Rows.Count>0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    usuario.idUsuario = Converter.GetInt(row, "idUsuario");
                    usuario.nombres = Converter.GetString(row, "nombres");
                    usuario.apellidoPaterno = Converter.GetString(row, "apellidoPaterno");
                    usuario.apellidoMaterno = Converter.GetString(row, "apellidoMaterno");
                    usuario.documentoIdentidad = Converter.GetString(row, "documentoIdentidad");
                    usuario.login = Converter.GetString(row, "login");
                    usuario.codigoColegio = Converter.GetString(row, "codigoColegio") == null ? "" : Converter.GetString(row, "codigoColegio");
                    usuario._profesion = Converter.GetString(row, "Profesion");
                    usuario.RNE = Converter.GetString(row, "RNE") ==null ? "" : Converter.GetString(row, "RNE");
                    usuario.correo = Converter.GetString(row, "correo");
                    usuario.cargo = Converter.GetString(row, "cargo") == null ? "" : Converter.GetString(row, "cargo");
                    usuario.telefono = Converter.GetString(row, "telefonoContacto") == null ? "" : Converter.GetString(row, "telefonoContacto");
                    usuario.idTipoUsuario = Converter.GetInt(row, "idTipoUsuario");
                    usuario.componente = Converter.GetInt(row, "idComponente");
                    usuario.tipo = Converter.GetInt(row, "idTipoAcceso");
                    usuario.nAprobacion = Converter.GetInt(row, "idNivelAprobacion");
                }
            }
            return usuario;
        }

        public List<Usuario> ListaDatosUsuariosPorCaducar()
        {
            List<Usuario> usuario = new List<Usuario>();
            var objCommand = GetSqlCommand("pNLS_ListaDatosUsuariosPorCaducar");
            var dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
                return null;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                var user = new Usuario();
                user.idUsuario = int.Parse(row["idusuario"].ToString());
                user.correo = row["correo"].ToString();
                user.estado = int.Parse(row["Dias"].ToString());
                usuario.Add(user);
            }
            return usuario;
        }

        public string GetBodyCorreo(Usuario usuario)
        {
            string body = ""; string nombre = usuario.nombres + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
            var objCommand = GetSqlCommand("pNLS_ConsultaCuerpoCorreo");
            InputParameterAdd.Varchar(objCommand, "nomUser", nombre);
            InputParameterAdd.Varchar(objCommand, "login_user", usuario.login);
            InputParameterAdd.Varchar(objCommand, "newPassword", usuario.documentoIdentidad);
            var dataTable = Execute(objCommand);
            if (dataTable.Rows.Count == 0)
                return null;

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    body = Converter.GetString(row, "body");
                }
            }
            return body;
        }
    }

}


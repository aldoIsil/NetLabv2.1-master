using BusinessLayer.Interfaces;
using DataLayer;
using System;
using System.Collections.Generic;
using Model;

namespace BusinessLayer
{

    public class UsuarioBl : IUsuarioBl
    {
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Descripcion: Realiza la refernecia con el usuarioDal.SearchUsuario.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="apellidosMaterno"></param>
        /// <param name="apellidosPaterno"></param>
        /// <param name="nombres"></param>
        /// <param name="dni"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public List<Usuario> GetUsuarios(string login, string apellidosMaterno, string apellidosPaterno, string nombres, string dni, int region)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                return usuarioDal.SearchUsuario(login, apellidosPaterno, apellidosMaterno, nombres, dni, region);
            }
        }
        /// <summary>
        /// Descripcion: Realiza la refernecia con el usuarioDal.GetUsuarioById.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Usuario GetUsuarioById(int idUsuario)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                return usuarioDal.GetUsuarioById(idUsuario);
            }
        }
        /// <summary>
        /// Descripcion: Realiza la refernecia con el usuarioDal.GetTotalCountEstablecimientoByUsuario.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public int GetTotalCountEstablecimientoByUsuario(int idUsuario)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                return usuarioDal.GetTotalCountEstablecimientoByUsuario(idUsuario);
            }
        }
        /// <summary>
        /// Descripcion: Realiza la refernecia con el usuarioDal.GetEstablecimientoByUsuario.
        /// con informacion del menu.
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
            using (var usuarioDal = new UsuarioDal())
            {
                return usuarioDal.GetEstablecimientoByUsuario(idUsuario, startIndex, maximumRows);
            }
        }
        /// <summary>
        /// Descripcion: Realiza la refernecia con el usuarioDal.InsertUsuario.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public string InsertUsuario(Usuario usuario)
        {
            try
            {
                using (var usuarioDal = new UsuarioDal())
                {
                    return usuarioDal.InsertUsuario(usuario);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Descripcion: Realiza la refernecia con el usuarioDal.UpdateUsuario.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        public void UpdateUsuario(Usuario usuario)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                usuarioDal.UpdateUsuario(usuario);
            }
        }
        /// <summary>
        /// Descripcion: Realiza la refernecia con el usuarioDal.GetRolesByIdUser.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Rol> GetRolesByIdUser(int idUsuario)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                return usuarioDal.GetRolesByIdUser(idUsuario);
            }
        }
        /// <summary>
        /// Descripcion: Realiza la validacion del usuario/paciente contra el webservice de la reniec.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="dni"></param>
        /// <returns></returns>
        public Usuario ValidarDatosUsuario(Usuario usuario, string dni)
        {
            var pacienteDal = new PacienteDal();
            Boolean reniec = pacienteDal.EstadoReniec();
            if (reniec)
            {
                IReniecConsumer reniecConsumer = new ReniecConsumer();
                var persona = reniecConsumer.getReniec(dni);
                this.ErrorMessage = reniecConsumer.ErrorMessage;

                /*Si Persona es distina de Null quiere decir que se encontro el Paciente*/
                if (persona != null)
                {
                    usuario.apellidoPaterno = persona.ApellidoPaterno;
                    usuario.apellidoMaterno = persona.ApellidoMaterno;
                    usuario.nombres = persona.Nombres;
                    usuario.estatus = 1;
                }

                if (string.IsNullOrEmpty(ErrorMessage) && persona == null)
                    ErrorMessage = "No se encontraron coincidencias.";
            }
            else
            {
                usuario.nombres = "Servicio de Reniec Inactivo";
            }
            return usuario;
        }
        /// <summary>
        /// Descripcion:  Realiza la refernecia con el usuarioDal.InsertListaUsuarioEstablecimiento.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="listaUsuariosEstablecimientos"></param>
        public void InsertListaUsuarioEstablecimiento(List<UsuarioEstablecimiento> listaUsuariosEstablecimientos)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                usuarioDal.InsertListaUsuarioEstablecimiento(listaUsuariosEstablecimientos);
            }
        }
        /// <summary>
        /// Descripcion:  Realiza la refernecia con el usuarioDal.InsertUsuarioEstablecimiento.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="usuarioEstablecimiento"></param>
        public void InsertUsuarioEstablecimiento(UsuarioEstablecimiento usuarioEstablecimiento)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                usuarioDal.InsertUsuarioEstablecimiento(usuarioEstablecimiento);
            }
        }
        /// <summary>
        /// Descripcion:  Realiza la refernecia con el usuarioDal.ResetearClave.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="id"></param>
        public string ResetearClave(int id)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                return usuarioDal.ResetearClave(id);
            }

        }
        void IUsuarioBl.InsertUsuario(Usuario usuario)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Descripcion:  Realiza la refernecia con el usuarioDal.ExisteLogin.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        public int ExisteLogin(string login, int accion)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                return usuarioDal.ExisteLogin(login, accion);
            }
        }
        /// <summary>
        /// Descripcion:  Realiza la refernecia con el usuarioDal.getEstablecimientosUsuario.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosUsuario(int idUsuario)
        {
            using (var dal = new UsuarioDal())
            {
                return dal.getEstablecimientosUsuario(idUsuario);
            }
        }
        /// <summary>
        /// Descripcion:  Realiza la referencia con el usuarioDal.ExisteDni.
        /// con informacion del menu.
        /// Author: Terceros.
        /// Fecha Creación: 01/01/2017
        /// Modificacion: : Se agrearon Comentarios.
        /// </summary>
        /// <param name="dni"></param>
        /// <returns></returns>
        public bool ExisteDni(string dni)
        {
            if (string.IsNullOrEmpty(dni)) return false;

            using (var usuarioDal = new UsuarioDal())
            {
                return usuarioDal.ExisteDni(dni);
            }
        }

        public SolicitudUsuario InsertarSolicitudUsuario(SolicitudUsuario usuario)
        {
            using (var dal = new UsuarioDal())
            {
                dal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    dal.InsertarSolicitudUsuario(usuario);
                    for (int i = 0; i < usuario.rol.Count; i++)
                    {
                        usuario._rol = new Rol();
                        usuario._rol.idRol = usuario.rol[i].idRol;
                        dal.InsertarRolSolicitudUsuario(usuario);
                    }
                    if (usuario.examen != null && usuario.examen.Count > 0)
                    {
                        for (int i = 0; i < usuario.examen.Count; i++)
                        {
                            usuario._examen = new Examen();
                            usuario._examen.idExamenAgrupado = usuario.examen[i].idExamenAgrupado;
                            dal.InsertarExamenSolicitudUsuario(usuario);
                        }
                    }
                    dal.Commit();
                }
                catch (Exception ex)
                {
                    dal.Rollback();
                    ValidateOrderStateOnException(ex);
                }
            }
            return usuario;
        }

        private void ValidateOrderStateOnException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public List<SolicitudUsuario> ListaSolicitudUsuario(SolicitudUsuario usuario, string fechaDesde, string fechaHasta, int idEstablecimientoLog, int estado)
        {
            using (var usuarioDal = new UsuarioDal())
            {
                return usuarioDal.ListaSolicitudUsuario(usuario, fechaDesde, fechaHasta, idEstablecimientoLog, estado);
            }
        }

        public SolicitudUsuario GetSolicitudUsuario(int idSolicitudUsuario)
        {
            SolicitudUsuario usuario = new SolicitudUsuario();
            using (var dal = new UsuarioDal())
            {
                usuario = dal.GetSolicitudUsuario(idSolicitudUsuario);
            }
            return usuario;
        }

        public int ValidarSolicitudUsuario(SolicitudUsuario usuario)
        {
            int valor = 0;
            using (var dal = new UsuarioDal())
            {
                valor = dal.ValidarSolicitudUsuario(usuario);
            }
            return valor;
        }

        public List<SolicitudUsuario> GetEstadoSolicitud(string nroDocumento)
        {
            List<SolicitudUsuario> usuario = new List<SolicitudUsuario>();
            using (var dal = new UsuarioDal())
            {
                return dal.GetEstadoSolicitud(nroDocumento);
            }
        }

        public SolicitudUsuario InsertarSolicitudUsuarioNetlab1(SolicitudUsuario usuario)
        {
            using (var dal = new UsuarioDal())
            {
                dal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    dal.InsertarSolicitudUsuario(usuario);
                    dal.Commit();
                }
                catch (Exception ex)
                {
                    dal.Rollback();
                    ValidateOrderStateOnException(ex);
                }
            }
            return usuario;
        }

        public int DevuelveCorrelativo(string NombreTabla)
        {
            int codigo = 0;
            using(var dal = new UsuarioDal())
            {
                codigo = dal.DevuelveCorrelativo(NombreTabla);
            }
            return codigo;
        }

        public void InsertarSolicitud(SolicitudUsuario net1)
        {
            using (var dal = new UsuarioDal())
            {
                dal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    dal.InsertarSolicitud(net1);
                    dal.InsertarArchivo(net1);
                    dal.InsertarAuditoria(net1);
                    dal.Commit();
                }
                catch (Exception ex)
                {
                    dal.Rollback();
                    ValidateOrderStateOnException(ex);
                }
                
            }
        }

        public Usuario GetUsuarioByDocumento(string documentoIdentidad)
        {
            using (var dal = new UsuarioDal())
            {
                return dal.GetUsuarioByDocumento(documentoIdentidad);
            }
        }

        public List<Usuario> ListaDatosUsuariosPorCaducar()
        {
            using (var dal = new UsuarioDal())
            {
                return dal.ListaDatosUsuariosPorCaducar();
            }
        }

        public string GetBodyCorreo(Usuario usuario)
        {
            using (var dal = new UsuarioDal())
            {
                return dal.GetBodyCorreo(usuario);
            }
        }
    }
}

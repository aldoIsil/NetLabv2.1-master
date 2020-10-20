using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using System.Linq;
using Model;

namespace BusinessLayer
{
    public class EstablecimientoBl : IEstablecimientoBl
    {
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el nombre del establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosByTextoBusqueda(string textoBusqueda, int idUsuario)//, List<Establecimiento> listaEstablecimientos)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                var establecimientos = establecimientoDal.GetEstablecimientosByTextoBusqueda(textoBusqueda, idUsuario);
                return establecimientos.Select(x => x).Take(50).ToList();
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos de la base de datos : EstablecimientoCache
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosCache()
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                var establecimientos = establecimientoDal.GetEstablecimientosCache();
                return establecimientos;
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Nombre del Establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosByNombre(string nombre)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetEstablecimientosByNombre(nombre);
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Codigo del Usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Establecimiento GetEstablecimientoById(int id)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetEstablecimientoById(id);
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Codigo del Usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Establecimiento GetEstablecimientoByIdUsuario(int idUsuario)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetEstablecimientoByIdUsuario(idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos frecuentes filtrado por el Codigo del Usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosFrecuentesByIdUsuario(int idUsuario)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetEstablecimientosFrecuentesByIdUsuario(idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Nombre de la institucion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idInstitucion"></param>
        /// <returns></returns>
        public List<Establecimiento> GetDisaByInstitucionByTextoBusqueda(string textoBusqueda, int idInstitucion)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetDisaByInstitucionByTextoBusqueda(textoBusqueda, idInstitucion);
            }
        }
        /// <summary>
        /// Descripción: Obtiene Laboratorios filtrado por el Nombre de la institucion y id de la institucion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idInstitucion"></param>
        /// <returns></returns>
        public List<Establecimiento> GetDisaByInstitucionLabByTextoBusqueda(string textoBusqueda, int idInstitucion)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetDisaByInstitucionLabByTextoBusqueda(textoBusqueda, idInstitucion);
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos de la DISA filtrado por el Nombre de la institucion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetDisaRegionByTextoBusqueda(string textoBusqueda, int idUsuario)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetDisaRegionByTextoBusqueda(textoBusqueda, idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Obtiene Laboratorios filtrado por el Nombre de la institucion y id del usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetDisaRegionLabByTextoBusqueda(string textoBusqueda, int idUsuario)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetDisaRegionLabByTextoBusqueda(textoBusqueda, idUsuario);
            }
        }

        /// <summary>
        /// Descripción: Obtiene establecimientos de la Red filtrado por el Nombre de la institucion, id de la institucion y is de la DISA
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public List<Establecimiento> GetRedByDisaByTextoBusqueda(string textoBusqueda, int idInstitucion, int idDisa)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetRedByDisaByTextoBusqueda(textoBusqueda, idInstitucion, idDisa);
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos de la Micro Red filtrado por el Nombre de la institucion, id de la institucion, Id de la DISA y el id de la Red
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idDisa"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public List<Establecimiento> GetMicroredByRedByTextoBusqueda(string textoBusqueda, int idDisa, int idInstitucion, int idRed)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetMicroredByRedByTextoBusqueda(textoBusqueda, idDisa, idInstitucion, idRed);
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Nombre de la institucion, id de la institucion, Id de la DISA, el id de la Red y el id de la MicroRed
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idDisa"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicrored"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientoByRedByTextoBusqueda(string textoBusqueda, int idDisa, int idInstitucion, int idRed, int idMicrored)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetEstablecimientoByRedByTextoBusqueda(textoBusqueda, idDisa, idInstitucion, idRed, idMicrored);
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Nombre de la institucion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosAllByTextoBusqueda(string textoBusqueda)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetEstablecimientosAllByTextoBusqueda(textoBusqueda);
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el id del Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<UsuarioEstablecimiento> GetUsuarioEstablecimientoByUser(int idUsuario)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetUsuarioEstablecimientoByUser(idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Elimina o inhabilita un establecimiento filtrado por el codigo de usuario y el de usuario de edicion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioEdicion"></param>
        /// <param name="idUsuario"></param>
        public void EliminarUsuarioEstablecimiento(int idUsuarioEdicion, int idUsuario)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                establecimientoDal.EliminarUsuarioEstablecimiento(idUsuarioEdicion, idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Elimina o inhabilita un establecimiento filtrado por el codigo de usuario y el de usuario de edicion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioEdicion"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idEstablecimiento"></param>
        public void EliminarEstablecimientos(int idUsuarioEdicion, int idUsuario, int[] establecimientos)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                foreach (var establecimiento in establecimientos)
                {
                    establecimientoDal.EliminarEstablecimiento(idUsuarioEdicion, idUsuario, establecimiento);
                }
            }
        }
        /// <summary>
        /// Descripción: Obtiene establecimientos filtrado por el Id Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="establecimientoId"></param>
        /// <returns></returns>
        public Establecimiento GetEstablecimientosById(int idEstablecimiento)
        {
            using (var dal = new EstablecimientoDal())
            {
                return dal.GetEstablecimientosById(idEstablecimiento);
            }
        }
        /// <summary>
        /// Descripción: Registra establecimientos por usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioRegistro"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicroRed"></param>
        public void InsertarUsuarioEstablecimientoByMicroRed(int idUsuarioRegistro, int idUsuario, int idInstitucion, string idDisa, string idRed, string idMicroRed)
        {
            using (var dal = new EstablecimientoDal())
            {
                dal.InsertarUsuarioEstablecimientoByMicroRed(idUsuarioRegistro, idUsuario, idInstitucion, idDisa, idRed, idMicroRed);
            }
        }
        /// <summary>
        /// Descripción: Registra establecimientos por usuario y Red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioRegistro"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        public void InsertarUsuarioEstablecimientoByRed(int idUsuarioRegistro, int idUsuario, int idInstitucion, string idDisa, string idRed)
        {
            using (var dal = new EstablecimientoDal())
            {
                dal.InsertarUsuarioEstablecimientoByRed(idUsuarioRegistro, idUsuario, idInstitucion, idDisa, idRed);
            }
        }
        /// <summary>
        /// Descripción: Registra establecimientos por usuario, Red y MicroRed.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioRegistro"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        public void InsertarUsuarioEstablecimientoByDisa(int idUsuarioRegistro, int idUsuario, int idInstitucion, string idDisa)
        {
            using (var dal = new EstablecimientoDal())
            {
                dal.InsertarUsuarioEstablecimientoByDisa(idUsuarioRegistro, idUsuario, idInstitucion, idDisa);
            }
        }
        /// <summary>
        /// Descripción: Registra establecimientos por usuario y Id Institucion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuarioRegistro"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        public void InsertarUsuarioEstablecimientoByInstitucion(int idUsuarioRegistro, int idUsuario, int idInstitucion)
        {
            using (var dal = new EstablecimientoDal())
            {
                dal.InsertarUsuarioEstablecimientoByInstitucion(idUsuarioRegistro, idUsuario, idInstitucion);
            }
        }

        public List<Establecimiento> GetLaboratorioINS()
        {
            List<Establecimiento> labIns = new List<Establecimiento>();
            EstablecimientoDal dal = new EstablecimientoDal();
            labIns = dal.GetLaboratorioINS();
            return labIns;
        }
    }
}

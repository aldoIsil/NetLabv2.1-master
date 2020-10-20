using System.Collections.Generic;
using DataLayer;
using Model;

namespace BusinessLayer
{
    public class LocalBl
    {
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por el codigo de usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Institucion> GetInstituciones(Local local)
        {
            using (var dal = new InstitucionDal())
            {
                return dal.GetInstitucionesByUsuarioId(local.IdUsuario);
            }
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por el codigo de usuario y de la institucion.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <returns></returns>
        public List<DISA> GetDisas(Local local)
        {
            using (var dal = new InstitucionDal())
            {
                return dal.GetInstitucionesByInstitucionUsuario(local.IdUsuario, local.IdInstitucion);
            }
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las redes, realiza busqueda filtrado por el codigo de usuario, id institucion y el id de la DISA.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public List<Red> GetRedes(Local local)
        {
            using (var dal = new InstitucionDal())
            {
                return dal.GetInstitucionesByInstitucionDisaUsuario(local.IdUsuario, local.IdInstitucion, local.IdDisa);
            }
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por el codigo de usuario, id institucion, el id de la DISA y el id de la Red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public List<MicroRed> GetMicroRedes(Local local)
        {
            using (var dal = new InstitucionDal())
            {
                return dal.GetInstitucionesByInstitucionDisaRedUsuario(local.IdUsuario, local.IdInstitucion, local.IdDisa, local.IdRed);
            }
        }
        /// <summary>
        /// Descripción: Obtiene Informacion de las instituciones, realiza busqueda filtrado por el codigo de usuario, id institucion, el id de la DISA, el id de la Red y el id de la MicroRed.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicroRed"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientos(Local local)
        {
            using (var dal = new InstitucionDal())
            {
                return dal.GetInstitucionesByInstitucionDisaRedMicroRedUsuario(local.IdUsuario, local.IdInstitucion, local.IdDisa, local.IdRed, local.IdMicroRed);
            }
        }

        public List<Institucion> GetInstituciones()
        {
            using (var dal = new InstitucionDal())
            {
                return dal.GetInstituciones();
            }
        }

        public List<DISA> GetDisas()
        {
            using (var dal = new DISADal())
            {
                return dal.ObtenerDISAs();
            }
        }

        public List<Red> GetRedes()
        {
            using (var dal = new DISADal())
            {
                return dal.ObtenerRedes();
            }
        }

        public List<MicroRed> GetMicroRedes()
        {
            using (var dal = new InstitucionDal())
            {
                return dal.GetMicroRedes();
            }
        }

        public List<Establecimiento> GetEstablecimientos()
        {
            using (var dal = new InstitucionDal())
            {
                return dal.GetEstablecimientos();
            }
        }
    }
}

using BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataLayer;

namespace BusinessLayer
{
    public class RedMantBl : IRedBl
    {
        /// <summary>
        /// Descripción: Obtener las redes filtrado por id disa y el id red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public RedMant GetRedByID(string idDisa, string idRed)
        {
            using (var redMantDal = new RedMantDal())
            {
                return redMantDal.GetRedByID(idDisa,idRed);
            }
        }
        /// <summary>
        /// Descripción: Obtiene las redes filtrado por id disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public List<RedMant> GetRedByIDDisa(string idDisa)
        {
            using (var redMantDal = new RedMantDal())
            {
                return redMantDal.GetRedByIDDisa(idDisa);
            }
        }
        /// <summary>
        /// Descripción: Obtiene las redes filtrado por el Nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<RedMant> GetRedes(string nombre = null)
        {
            using (var redMantDal = new RedMantDal())
            {
                return redMantDal.GetRedes(nombre);
            }
        }
        /// <summary>
        /// Descripción: Registra las redes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="red"></param>
        public void InsertRedes(RedMant red)
        {
            using (var redMantDal = new RedMantDal())
            {
                redMantDal.InsertRedes(red);
            }
        }
        /// <summary>
        /// Descripción: Actualiza redes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="red"></param>
        public void UpdateRedes(RedMant red)
        {
            using (var redMantDal = new RedMantDal())
            {
                redMantDal.UpdateRedes(red);
            }
        }

    }
}

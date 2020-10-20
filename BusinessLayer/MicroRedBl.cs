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
    public class MicroRedBl : IMicroRedBI
    {
        /// <summary>
        /// Descripción: Obtiene las Microredes filtradas por el disa, red y microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDisa"></param>
        /// <param name="idRed"></param>
        /// <param name="idmicrored"></param>
        /// <returns></returns>
        public MicroRedMant GetMicroRedByID(string idDisa, string idRed, string idmicrored)
        {
            using (var MicroredMantDal = new MicroRedMantDal())
            {
                return MicroredMantDal.GetMicroRedByID(idDisa,idRed,idmicrored);
            }
        }
        /// <summary>
        /// Descripción: Obtiene las Microredes filtradas por el nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<MicroRedMant> GetMicroRedes(string nombre = null)
        {
            using (var MicroredMantDal = new MicroRedMantDal())
            {
                return MicroredMantDal.GetMicroRedes(nombre);
            }
        }
        /// <summary>
        /// Descripción: Insertar una microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="microred"></param>
        public void InsertMicroRedes(MicroRedMant microred)
        {
            using (var redMantDal = new MicroRedMantDal())
            {
                redMantDal.InsertMicroRedes(microred);
            }
           
        }
        /// <summary>
        /// Descripción: Actualiza MicroRedes.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="microred"></param>
        public void UpdateMicroRedes(MicroRedMant microred)
        {
            using (var redMantDal = new MicroRedMantDal())
            {
                redMantDal.UpdateMicroRedes(microred);
            }

        }
    }
}

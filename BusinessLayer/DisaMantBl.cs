using BusinessLayer.Interfaces;
using DataLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
   public  class DisaMantBl:IDisaBI
    {
        //List<DISA> GetDisas(string nombre = null);
        //void InsertDisa(DISA disa);
        //void UpdateDisa(DISA disa);


        /// <summary>
        /// Descripción: Obtiene informacion de las DISAS filtrado por el Nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<DisaMant> GetDisas(string nombre = null) {

            using (var disaMantDal=new DisaMantDal()) {
                return disaMantDal.GetDisas(nombre);
            }

        }
        /// <summary>
        /// Descripción: Obtiene informacion de las DISAS filtrado por el Id de Disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDisa"></param>
        /// <returns></returns>
        public DisaMant GetDisaByID(string idDisa) {
            using (var disaMantDal=new DisaMantDal())
            {
                return disaMantDal.GetDisaByID(idDisa);
            }

        }

        /// <summary>
        /// Descripción: Registra informacion de una disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="disa"></param>
        public void InsertDisa(DisaMant disa) {

            using (var disaMantDal = new DisaMantDal())
            {
                 disaMantDal.InsertDisas(disa);
            }
        }
        /// <summary>
        /// Descripción: Actualiza informacion de una Disa.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="disa"></param>
        public void UpdateDisa(DisaMant disa) {

            using (var disaMantDal = new DisaMantDal())
            {
                disaMantDal.UpdateDisas(disa);
            }
        }

    }
}

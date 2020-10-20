using System;
using System.Collections.Generic;
using DataLayer;
using Model;
using Model.DTO;
using BusinessLayer.Interfaces;

namespace BusinessLayer
{
    public class DISABl
    {
        /// <summary>
        /// Descripción: Obtiene las Disas activas
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<DISA> GetDISAs()
        {
            using (var disaDAL = new DISADal())
            {
                return disaDAL.ObtenerDISAs();
            }
        }
        /// <summary>
        /// Descripción: Obtiene las redes fitlrado por una disa
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDISA"></param>
        /// <returns></returns>
        public List<Red> GetRedes(string idDISA)
        {
            using (var disaDAL = new DISADal())
            {
                return disaDAL.ObtenerRedes(idDISA);
            }
        }
        /// <summary>
        /// Descripción: Obtiene las microredes filtrado por la disa y la red.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDISA"></param>
        /// <param name="idRed"></param>
        /// <returns></returns>
        public List<MicroRed> GetMicroRedes(string idDISA, string idRed)
        {
            using (var disaDAL = new DISADal())
            {
                return disaDAL.ObtenerMicroRedes(idDISA, idRed);
            }
        }
    }
}

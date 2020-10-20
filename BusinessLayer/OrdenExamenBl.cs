using DataLayer;
using Model;

namespace BusinessLayer
{
    public class OrdenExamenBl
    {
        /// <summary>
        /// Descripción: Registra la finalizacion del ingreso de resultados
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="ordenExamen"></param>
        public void UpdateOrdenExamenStatus(OrdenExamen ordenExamen)
        {
            using (var ordenExamenDal = new OrdenExamenDal())
            {
                ordenExamenDal.UpdateOrdenExamenStatus(ordenExamen);
            }
        }
    }
}
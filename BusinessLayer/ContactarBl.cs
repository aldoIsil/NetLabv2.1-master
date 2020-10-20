using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using DataLayer;

namespace BusinessLayer
{
    public class ContactarBl
    {
        /// <summary>
        /// Descripción: Registra datos para la sugerencia
        /// Author: Jose Chavez
        /// Fecha Creación : 15/06/2019
        /// </summary>
        /// <param name="contactar"></param>
        public void InsertQueja(Contactar contactar)
        {
            using (var Contactar = new ContactarDal())
            {
                Contactar.InsertQueja(contactar);
            }
        }
    }
}

using DataLayer;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class RecepcionBl
    {
        public bool VerificarMuestraPorRecepcionar(string codigoMuestra)
        {
            using (var dal = new RecepcionDal())
            {
                return dal.VerificarMuestraPorRecepcionar(codigoMuestra);
            }
        }

        public List<OrdenMuestraRecepcionado> RecepcionarMuestra(OrdenMuestraVM model)
        {
            using (var dal = new RecepcionDal())
            {
                return dal.RecepcionarMuestra(model);
            }
        }

        public void RechazarMuestra(OrdenMuestraRecepcionados muestra)
        {
            using (var dal = new RecepcionDal())
            {
                dal.RechazarMuestra(muestra);
            }
        }
    }
}

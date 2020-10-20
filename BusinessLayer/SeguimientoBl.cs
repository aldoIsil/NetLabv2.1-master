using DataLayer;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class SeguimientoBl
    {
        public List<CCSeguimientoCab> GetSeguimientos(DateTime FechaInicio, DateTime FechaFin, int IdEstablecimiento, int idEnfermedad, Guid idExamen)
        {
            using (var SeguimientoDal = new SeguimientoDAl())
            {
                return SeguimientoDal.GetSeguimientos(FechaInicio, FechaFin, IdEstablecimiento, idEnfermedad, idExamen);
            }
        }
        public string InsertSeguimiento(CCSeguimientoCab oCCSeguimientoCab, CCSEguimientoDetalle oCCSEguimientoDetalle)
        {
            string result = "";
            using (var SeguimientoDal = new SeguimientoDAl())
            {
                SeguimientoDal.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    oCCSEguimientoDetalle.idseguimiento = SeguimientoDal.InsertSeguimiento(oCCSeguimientoCab);

                    //Registrar Condiciones Basicas
                    SeguimientoDal.InsertSeguimientoDetalle(oCCSEguimientoDetalle);

                    SeguimientoDal.Commit();

                    result = "ok";
                }
                catch (Exception ex)
                {
                    SeguimientoDal.Rollback();
                    result = ex.Message;
                }
            }
            return result;
        }
    }
}

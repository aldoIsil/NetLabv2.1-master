using System.Collections.Generic;
using DataLayer.PEED;
using Model;
using Model.ViewData;

namespace BusinessLayer.PEED
{
    public class IngresoResultadosControlCalidadBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de algunos resultados ingresados para un eess evaluado
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="objIngresoResultadosPEEC"></param>
        /// <returns></returns>
        public List<IngresoResultadosPEEC> GetIngresoResultadoControlCalidad(IngresoResultadosPEEC objIngresoResultadosPEEC)
        {
            using (var oIngresoResultadosControlCalidadDal = new IngresoResultadosControlCalidadDal())
            {
                return oIngresoResultadosControlCalidadDal.GetIngresoResultadoControlCalidad(objIngresoResultadosPEEC);
            }
        }
        /// <summary>
        /// Descripción: Metodo para Obtener los resultados registrados de una evaluacion
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018
        /// </summary>
        /// <param name="objResultadoControlCalidadVd"></param>
        /// <returns></returns>
        public List<ResultadoControlCalidadVd> GetResultadoControlCalidadVd(ResultadoControlCalidadVd objResultadoControlCalidadVd)
        {
            using (var oIngresoResultadosControlCalidadDal = new IngresoResultadosControlCalidadDal())
            {
                return oIngresoResultadosControlCalidadDal.GetResultadoControlCalidadVd(objResultadoControlCalidadVd);
            }
        }
        /// <summary>
        /// Descripción: Metodo para registrar resultados de cada material
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018
        /// </summary>
        /// <param name="oResultadoControlCalidadVd"></param>
        /// <returns></returns>
        public int InsertResultadoControlCalidadVd(ResultadoControlCalidadVd oResultadoControlCalidadVd)
        {
            var res = 0;
            using (var oIngresoResultadosControlCalidadDal = new IngresoResultadosControlCalidadDal())
            {
                if (oIngresoResultadosControlCalidadDal.ExistResultadoControlCalidadVd(oResultadoControlCalidadVd) == 1)
                {
                    oResultadoControlCalidadVd.idusuarioregistro =
                    res = EditResultadoControlCalidadVd(oResultadoControlCalidadVd);
                }
                else
                {
                    res = oIngresoResultadosControlCalidadDal.InsertResultadoControlCalidadVd(oResultadoControlCalidadVd);
                }
            }
            return res;
        }
        /// <summary>
        /// Descripción: Metodo para actualizar resultados de cada material
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018
        /// </summary>
        /// <param name="oResultadoControlCalidadVd"></param>
        /// <returns></returns>
        public int EditResultadoControlCalidadVd(ResultadoControlCalidadVd oResultadoControlCalidadVd)
        {
            using (var oIngresoResultadosControlCalidadDal = new IngresoResultadosControlCalidadDal())
            {
                return oIngresoResultadosControlCalidadDal.EditResultadoControlCalidadVd(oResultadoControlCalidadVd);
            }
        }

        public List<ResultadoControlCalidadVd> GetResultadoEvaluacionMaterial(ResultadoControlCalidadVd objResultadoControlCalidadVd)
        {
            using (var oIngresoResultadosControlCalidadDal = new IngresoResultadosControlCalidadDal())
            {
                return oIngresoResultadosControlCalidadDal.GetResultadoEvaluacionMaterial(objResultadoControlCalidadVd);
            }
        }
    }        
}

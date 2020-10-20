using System.Collections.Generic;
using DataLayer.PEED;
using Model;

namespace BusinessLayer.PEED
{
    public class ConfiguracionEvalControlCalidadBl
    {
        /// <summary>
        /// Descripción: Obtiene las evaluaciones registradas
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionEvalControlCalidad"></param>
        /// <returns></returns>
        public List<ConfiguracionEvalControlCalidad> GetConfiguracionEvalControlCalidad(ConfiguracionEvalControlCalidad xConfiguracionEvalControlCalidad)
        {
            using (var oConfiguracionEvalControlCalidadDal = new ConfiguracionEvalControlCalidadDal())
            {
                return oConfiguracionEvalControlCalidadDal.GetConfiguracionEvalControlCalidad(xConfiguracionEvalControlCalidad);
            }
        }
        /// <summary>
        /// Descripción: Obtiene evaluacion por Id
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionEvalControlCalidad"></param>
        /// <returns></returns>
        public ConfiguracionEvalControlCalidad GetConfiguracionEvalControlCalidadById(ConfiguracionEvalControlCalidad xConfiguracionEvalControlCalidad)
        {
            using (var oConfiguracionEvalControlCalidadDal = new ConfiguracionEvalControlCalidadDal())
            {
                return oConfiguracionEvalControlCalidadDal.GetConfiguracionEvalControlCalidadById(xConfiguracionEvalControlCalidad);
            }
        }
        /// <summary>
        /// Descripción: Realiza el registro de las evaluaciones
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionEvalControlCalidad"></param>
        /// <returns></returns>
        public int InsertConfigEval(ConfiguracionEvalControlCalidad xConfiguracionEvalControlCalidad)
        {
            using (var oConfiguracionEvalControlCalidadDal = new ConfiguracionEvalControlCalidadDal())
            {
                return oConfiguracionEvalControlCalidadDal.InsertConfigEval(xConfiguracionEvalControlCalidad);
            }
        }
        /// <summary>
        /// Descripción: Realiza la actualizacion de las evaluaciones
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionEvalControlCalidad"></param>
        /// <returns></returns>
        public int EditConfigEval(ConfiguracionEvalControlCalidad xConfiguracionEvalControlCalidad)
        {
            using (var oConfiguracionEvalControlCalidadDal = new ConfiguracionEvalControlCalidadDal())
            {
                return oConfiguracionEvalControlCalidadDal.EditConfigEval(xConfiguracionEvalControlCalidad);
            }
        }
    }
}

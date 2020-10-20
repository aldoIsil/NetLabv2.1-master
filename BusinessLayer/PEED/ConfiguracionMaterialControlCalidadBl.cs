using System.Collections.Generic;
using DataLayer.PEED;
using Model;

namespace BusinessLayer.PEED
{
    public class ConfiguracionMaterialControlCalidadBl
    {
        /// <summary>
        /// Descripción: Obtiene los materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionMaterialControlCalidad"></param>
        /// <returns></returns>
        public List<ConfiguracionMaterialControlCalidad> GetConfiguracionMaterialControlCalidad(ConfiguracionMaterialControlCalidad xConfiguracionMaterialControlCalidad)
        {
            using (var oConfiguracionMaterialControlCalidadDal = new ConfiguracionMaterialControlCalidadDal())
            {
                return oConfiguracionMaterialControlCalidadDal.GetConfiguracionMaterialControlCalidad(xConfiguracionMaterialControlCalidad);
            }
        }
        /// <summary>
        /// Descripción: Realiza el registro de los materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionMaterialControlCalidad"></param>
        /// <returns></returns>
        public int InsertConfigEvalMaterial(ConfiguracionMaterialControlCalidad xConfiguracionMaterialControlCalidad)
        {
            using (var oConfiguracionMaterialControlCalidadDal = new ConfiguracionMaterialControlCalidadDal())
            {
                return oConfiguracionMaterialControlCalidadDal.InsertConfigEvalMaterial(xConfiguracionMaterialControlCalidad);
            }
        }
        /// <summary>
        /// Descripción: Realiza las modificaciones de los materiales
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="oConfiguracionMaterialControlCalidad"></param>
        /// <returns></returns>
        public int EditConfigEvalMaterial(ConfiguracionMaterialControlCalidad oConfiguracionMaterialControlCalidad)
        {
            using (var oConfiguracionMaterialControlCalidadDal = new ConfiguracionMaterialControlCalidadDal())
            {
                return oConfiguracionMaterialControlCalidadDal.EditConfigEvalMaterial(oConfiguracionMaterialControlCalidad);
            }
        }
    }
}

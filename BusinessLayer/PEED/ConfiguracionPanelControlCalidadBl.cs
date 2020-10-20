using System.Collections.Generic;
using DataLayer.PEED;
using Model;

namespace BusinessLayer.PEED
{
    public class ConfiguracionPanelControlCalidadBl
    {
        /// <summary>
        /// Descripción: Obtiene los paneles
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public List<ConfiguracionPanelControlCalidad> GetConfiguracionPanelControlCalidad(ConfiguracionPanelControlCalidad xConfiguracionPanelControlCalidad)
        {
            using (var oConfiguracionPanelControlCalidadDal = new ConfiguracionPanelControlCalidadDal())
            {
                return oConfiguracionPanelControlCalidadDal.GetConfiguracionPanelControlCalidad(xConfiguracionPanelControlCalidad);
            }
        }
        /// <summary>
        /// Descripción: Obtiene los Paneles por codigo
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public ConfiguracionPanelControlCalidad GetConfiguracionPanelControlCalidadById(ConfiguracionPanelControlCalidad xConfiguracionPanelControlCalidad)
        {
            using (var oConfiguracionPanelControlCalidadDal = new ConfiguracionPanelControlCalidadDal())
            {
                return oConfiguracionPanelControlCalidadDal.GetConfiguracionPanelControlCalidadById(xConfiguracionPanelControlCalidad);
            }
        }
        /// <summary>
        /// Descripción: Registrar los paneles
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public int InsertConfigEvalPanel(ConfiguracionPanelControlCalidad xConfiguracionPanelControlCalidad)
        {
            using (var oConfiguracionPanelControlCalidadDal = new ConfiguracionPanelControlCalidadDal())
            {
                return oConfiguracionPanelControlCalidadDal.InsertConfigEvalPanel(xConfiguracionPanelControlCalidad);
            }
        }
        /// <summary>
        /// Descripción: Metodo para la modificacion de paneles
        /// Author: Juan Muga
        /// Fecha Creacion: 01/07/2018 
        /// </summary>
        /// <param name="xConfiguracionPanelControlCalidad"></param>
        /// <returns></returns>
        public int EditConfigEvalPanel(ConfiguracionPanelControlCalidad xConfiguracionPanelControlCalidad)
        {
            using (var oConfiguracionPanelControlCalidadDal = new ConfiguracionPanelControlCalidadDal())
            {                
                return oConfiguracionPanelControlCalidadDal.EditConfigEvalPanel(xConfiguracionPanelControlCalidad);
            }
        }
    }
}

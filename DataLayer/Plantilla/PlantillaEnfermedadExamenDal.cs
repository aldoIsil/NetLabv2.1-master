using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer.Plantilla
{
    public class PlantillaEnfermedadExamenDal : DaoBase
    {
        public PlantillaEnfermedadExamenDal(IDalSettings settings) : base(settings)
        {
        }

        public PlantillaEnfermedadExamenDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Metodo que retorna la lista de las muestras
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param> 
        /// <returns></returns>
        public List<PlantillaEnfermedadExamen> GetMuestras(int idPlantilla)
        {
            var objCommand = GetSqlCommand("pNLS_PlantillaEnfermedadExamen");
            InputParameterAdd.Int(objCommand, "idPlantilla", idPlantilla);

            return PlantillaEnfermedadExamenConvertTo.Muestras(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Metodo que registra la plantilla de enfermedades
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="muestra"></param>
        public void InsertMuestra(PlantillaEnfermedadExamen muestra)
        {
            var objCommand = GetSqlCommand("pNLI_PlantillaEnfermedadExamen");

            InputParameterAdd.Int(objCommand, "idPlantilla", muestra.IdPlantilla);
            InputParameterAdd.Int(objCommand, "idEnfermedad", muestra.IdEnfermedad);
            InputParameterAdd.Guid(objCommand, "idExamen", muestra.IdExamen);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", muestra.IdTipoMuestra);
            InputParameterAdd.Int(objCommand, "idMaterial", muestra.IdMaterial);
            InputParameterAdd.Int(objCommand, "cantidad", muestra.Cantidad);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", muestra.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que actualiza la plantilla de enfermedades por examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="muestra"></param>
        public void UpdateMuestra(PlantillaEnfermedadExamen muestra)
        {
            var objCommand = GetSqlCommand("pNLU_PlantillaEnfermedadExamen");

            InputParameterAdd.Int(objCommand, "idPlantilla", muestra.IdPlantilla);
            InputParameterAdd.Int(objCommand, "idEnfermedad", muestra.IdEnfermedad);
            InputParameterAdd.Guid(objCommand, "idExamen", muestra.IdExamen);
            InputParameterAdd.Int(objCommand, "idTipoMuestra", muestra.IdTipoMuestra);
            InputParameterAdd.Int(objCommand, "idMaterial", muestra.IdMaterial);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", muestra.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", muestra.Estado);

            ExecuteNonQuery(objCommand);
        }
    }
}
using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using Model;

namespace DataLayer.Plantilla
{
    public class EstablecimientoPlantillaDal : DaoBase
    {
        public EstablecimientoPlantillaDal(IDalSettings settings) : base(settings)
        {
        }

        public EstablecimientoPlantillaDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene los establecimientos por el codigo de la plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablecimientosByPlantillaId(int idPlantilla)
        {
            var objCommand = GetSqlCommand("pNLS_EstablecimientosByPlantillaId");
            InputParameterAdd.Int(objCommand, "idPlantilla", idPlantilla);

            return EstablecimientoConvertTo.Establecimientos(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Metodo que ejecuta la actualización de los establecimientos de una plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="plantillaEstablecimiento"></param>
        public void InsertEstablecimientoByPlantilla(PlantillaEstablecimiento plantillaEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLI_EstablecimientoPlantilla");

            InputParameterAdd.Int(objCommand, "idPlantilla", plantillaEstablecimiento.IdPlantilla);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", plantillaEstablecimiento.IdEstablecimiento);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", plantillaEstablecimiento.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que actualiza los establecimientos de una plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="plantillaEstablecimiento"></param>
        public void UpdateEstablecimientoByPlantilla(PlantillaEstablecimiento plantillaEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLU_EstablecimientoPlantilla");

            InputParameterAdd.Int(objCommand, "idPlantilla", plantillaEstablecimiento.IdPlantilla);
            InputParameterAdd.Int(objCommand, "idEstablecimiento", plantillaEstablecimiento.IdEstablecimiento);
            InputParameterAdd.Int(objCommand, "estado", plantillaEstablecimiento.Estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", plantillaEstablecimiento.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}
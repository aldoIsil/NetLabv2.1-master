using System.Collections.Generic;
using DataLayer.DalConverter;
using Framework.DAL;
using Framework.DAL.Settings.Implementations;

namespace DataLayer.Plantilla
{
    public class PlantillaDal : DaoBase
    {
        public PlantillaDal(IDalSettings settings) : base(settings)
        {
        }

        public PlantillaDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Metodo que obtiene una plantilla filtrado por el nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Model.Plantilla> GetPlantillas(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_PlantillaByNombre");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return PlantillaConvertTo.Plantillas(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Metodo que obtiene una plantilla filtrado por el Codigo de la Plantilla
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idPlantilla"></param>
        /// <returns></returns>
        public Model.Plantilla GetPlantillaById(int idPlantilla)
        {
            var objCommand = GetSqlCommand("pNLS_PlantillaById");
            InputParameterAdd.Int(objCommand, "idPlantilla", idPlantilla);

            return PlantillaConvertTo.Plantilla(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Metodo que registra la plantilla
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="plantilla"></param>
        public void InsertPlantilla(Model.Plantilla plantilla)
        {
            var objCommand = GetSqlCommand("pNLI_Plantilla");

            InputParameterAdd.Varchar(objCommand, "nombre", plantilla.Nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", plantilla.Descripcion);
            InputParameterAdd.Int(objCommand, "idTipo", plantilla.IdTipo);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", plantilla.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que actualiza registro de una plantilla.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="plantilla"></param>
        public void UpdatePlantilla(Model.Plantilla plantilla)
        {
            var objCommand = GetSqlCommand("pNLU_Plantilla");

            InputParameterAdd.Int(objCommand, "idPlantilla", plantilla.IdPlantilla);
            InputParameterAdd.Varchar(objCommand, "nombre", plantilla.Nombre);
            InputParameterAdd.Varchar(objCommand, "descripcion", plantilla.Descripcion);
            InputParameterAdd.Int(objCommand, "idTipo", plantilla.IdTipo);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", plantilla.IdUsuarioEdicion);
            InputParameterAdd.Int(objCommand, "estado", plantilla.Estado);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Metodo que Obtiene plantillas de acuerdo a un establecimiento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEstablecimiento"></param>
        /// <returns></returns>
        public List<Model.Plantilla> GetPlantillaByEstablecimiento(int idEstablecimiento)
        {
            var objCommand = GetSqlCommand("pNLS_PlantillaByIdEstablecimiento");
            InputParameterAdd.Int(objCommand, "idEstablecimiento", idEstablecimiento);

            return PlantillaConvertTo.Plantillas(Execute(objCommand));
        }
    }
}

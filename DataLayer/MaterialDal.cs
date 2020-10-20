using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System.Collections.Generic;
using System.Data;
using DataLayer.DalConverter;
using Model;

namespace DataLayer
{
    public class MaterialDal : DaoBase
    {
        public MaterialDal(IDalSettings settings) : base(settings)
        {
        }

        public MaterialDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los materiales activos filtrado por el id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Material> GetMateriales(string nombre)
        {
            var objCommand = GetSqlCommand("pNLS_Material");
            InputParameterAdd.Varchar(objCommand, "nombre", nombre);

            return MaterialConvertTo.Materiales(Execute(objCommand));
        }

        /// <summary>
        /// Descripción: Obtiene los materiales por cada tipo de muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public List<Material> GetMaterialesByIdTipoMuestra(int idTipoMuestra)
        {
            var objCommand = GetSqlCommand("pNLS_MaterialByIdTipoMuestra");
            InputParameterAdd.Int(objCommand, "idTipoMuestra", idTipoMuestra);
            DataTable dataTable = Execute(objCommand);
            List<Material> datoList = new List<Material>();

            foreach (DataRow row in dataTable.Rows)
            {
                Material material = new Material
                {
                    idMaterial = Converter.GetInt(row, "idMaterial"),
                    Presentacion = new Presentacion
                    {
                        idPresentacion = Converter.GetInt(row, "idPresentacion"),
                        glosa = Converter.GetString(row, "glosa")
                    },
                    Reactivo = new Reactivo
                    {
                        idReactivo = Converter.GetInt(row, "idReactivo"),
                        glosa = Converter.GetString(row, "glosaReactivo")
                    },
                    TipoMuestra = new TipoMuestra
                    {
                        idTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                        nombre = Converter.GetString(row, "nombre")
                    },
                    volumen = Converter.GetDecimal(row, "volumen")
                };
                datoList.Add(material);
            }
            return datoList;

        }
        /// <summary>
        /// Descripción: Obtiene los criterios de rechazo por tipo de muestra y presentacion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <param name="idPresentacion"></param>
        /// <returns></returns>
        public List<Material> GetMaterialesByIdTipoMuestraIdPresentacion(int idTipoMuestra, int idPresentacion)
        {
            var objCommand = GetSqlCommand("pNLS_MaterialByIdTipoMuestraIdPresentacion");
            InputParameterAdd.Int(objCommand, "idTipoMuestra", idTipoMuestra);
            InputParameterAdd.Int(objCommand, "idPresentacion", idPresentacion);
            DataTable dataTable = Execute(objCommand);
            List<Material> datoList = new List<Material>();

            foreach (DataRow row in dataTable.Rows)
            {
                Material material = new Material
                {
                    idMaterial = Converter.GetInt(row, "idMaterial"),
                    Presentacion = new Presentacion
                    {
                        idPresentacion = Converter.GetInt(row, "idPresentacion"),
                        glosa = Converter.GetString(row, "glosa")
                    },
                    Reactivo = new Reactivo
                    {
                        idReactivo = Converter.GetInt(row, "idReactivo"),
                        glosa = Converter.GetString(row, "glosaReactivo")
                    },
                    TipoMuestra = new TipoMuestra
                    {
                        idTipoMuestra = Converter.GetInt(row, "idTipoMuestra"),
                        nombre = Converter.GetString(row, "nombre")
                    },
                };
                material.volumen = Converter.GetDecimal(row, "volumen");
                datoList.Add(material);
            }
            return datoList;

        }
        /// <summary>
        /// Descripción: Obtiene información de los datos de los Materiales filtrado por el id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idMaterial"></param>
        /// <returns></returns>
        public Material GetMaterialById(int idMaterial)
        {
            var objCommand = GetSqlCommand("pNLS_MaterialById");
            InputParameterAdd.Int(objCommand, "idMaterial", idMaterial);

            return MaterialConvertTo.Material(Execute(objCommand));
        }
        /// <summary>
        /// Descripción: Registra información de los datos del Material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="material"></param>
        public void InsertMaterial(Material material)
        {
            var objCommand = GetSqlCommand("pNLI_Material");

            InputParameterAdd.Int(objCommand, "idTipoMuestra", material.IdTipoMuestra);
            InputParameterAdd.Int(objCommand, "idPresentacion", material.IdPresentacion);
            InputParameterAdd.Int(objCommand, "idReactivo", material.IdReactivo);
            InputParameterAdd.Decimal(objCommand, "volumen", material.volumen);
            InputParameterAdd.Int(objCommand, "idUsuarioRegistro", material.IdUsuarioRegistro);

            ExecuteNonQuery(objCommand);
        }
        /// <summary>
        /// Descripción: Actualiza información de los datos del Material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="material"></param>
        public void UpdateMaterial(Material material)
        {
            var objCommand = GetSqlCommand("pNLU_Material");

            InputParameterAdd.Int(objCommand, "idMaterial", material.idMaterial);
            InputParameterAdd.Decimal(objCommand, "volumen", material.volumen);
            InputParameterAdd.Int(objCommand, "estado", material.estado);
            InputParameterAdd.Int(objCommand, "idUsuarioEdicion", material.IdUsuarioEdicion);

            ExecuteNonQuery(objCommand);
        }
    }
}

using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using Model;

namespace BusinessLayer
{
    public class MaterialBl : IMaterialBl
    {
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
            using (var materiaDal = new MaterialDal())
            {
                return materiaDal.GetMateriales(nombre);
            }
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
            using (var materiaDal = new MaterialDal())
            {
                return materiaDal.GetMaterialById(idMaterial);
            }
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
            using (var materiaDal = new MaterialDal())
            {
                return materiaDal.GetMaterialesByIdTipoMuestra(idTipoMuestra);
            }
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
            using (var materiaDal = new MaterialDal())
            {
                return materiaDal.GetMaterialesByIdTipoMuestraIdPresentacion(idTipoMuestra, idPresentacion);
            }
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
        public List<Material> GetMaterialesByIdTipoMuestra(List<int> idTipoMuestraList)
        {

            using (var materialDal = new MaterialDal())
            {
                List<Material> materialList = new List<Material>();
                foreach (int idTipoMuestra in idTipoMuestraList)
                {
                    List<Material> materialListTmp = materialDal.GetMaterialesByIdTipoMuestra(idTipoMuestra);

                    foreach (Material materialTmp in materialListTmp)
                    {
                        bool materialExiste = false;
                        foreach (Material material in materialList)
                        {
                            if (materialTmp.idMaterial == material.idMaterial)
                            {
                                materialExiste = true;
                                break;
                            }
                        }
                        if (!materialExiste)
                        {
                            materialList.Add(materialTmp);
                        }
                    }
                }

                return materialList;

            }
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
            using (var materialDal = new MaterialDal())
            {
                materialDal.InsertMaterial(material);
            }
        }
        /// <summary>
        /// Descripción: Registra información de los datos del Material
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="material"></param>
        public void UpdateMaterial(Material material)
        {
            using (var materialDal = new MaterialDal())
            {
                materialDal.UpdateMaterial(material);
            }
        }
    }
}

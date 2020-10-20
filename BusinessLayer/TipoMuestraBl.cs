using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using System;
using Model;

namespace BusinessLayer
{
    public class TipoMuestraBl : ITipoMuestraBl 
    {
        /// <summary>
        /// Descripción: Obtiene tipos de muestra filtrado por nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<TipoMuestra> GetTipoMuestras(string nombre = null)
        {
            using (var tipoMuestraDal = new TipoMuestraDal())
            {
                return tipoMuestraDal.GetTipoMuestras(nombre);
            }
        }

        /// <summary>
        /// Descripción: Obtiene tipos de muestra activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<TipoMuestra> GetTipoMuestrasActivas(string nombre = null)
        {
            using (var tipoMuestraDal = new TipoMuestraDal())
            {
                return tipoMuestraDal.GetTipoMuestrasActivas(nombre);
            }
        }

        /// <summary>
        /// Descripción: Obtiene tipos de muestra por metodo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<TipoMuestra> GetTiposMuestraByIdExamen(Guid idExamen)
        {
            using (var tipoMuestraDal = new TipoMuestraDal())
            {
                return tipoMuestraDal.GetTiposMuestraByIdExamen(idExamen);
            }
        }
        /// <summary>
        /// Descripción: Obtiene tipos de muestra por metodo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<TipoMuestra> GetTiposMuestraByIdExamen(List<Guid> idExamenList)
        {
            using (var tipoMuestraDal = new TipoMuestraDal())
            {
                List<TipoMuestra> tipoMuestraList = new List<TipoMuestra>();
                foreach (Guid idExamen in idExamenList)
                {
                    List<TipoMuestra> tipoMuestraListTmp = tipoMuestraDal.GetTiposMuestraByIdExamen(idExamen);

                    foreach (TipoMuestra tipoMuestraTmp in tipoMuestraListTmp)
                    {
                        bool tipoMuestraExiste = false;
                        foreach (TipoMuestra tipoMuestra in tipoMuestraList)
                        {
                            if (tipoMuestraTmp.idTipoMuestra == tipoMuestra.idTipoMuestra)
                            {
                                tipoMuestraExiste = true;
                                break;
                            }
                        }
                        if (!tipoMuestraExiste)
                        {
                            tipoMuestraList.Add(tipoMuestraTmp);
                        }
                    }

                    
                }

                return tipoMuestraList;
            }
            
        }
        /// <summary>
        /// Descripción: Obtener tipos de muestra filtrado por ID
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public TipoMuestra GetTiposMuestraById(int idTipoMuestra)
        {
            using (var tipoMuestraDal = new TipoMuestraDal())
            {
                return tipoMuestraDal.GetTiposMuestraById(idTipoMuestra);
            }
        }
        /// <summary>
        /// Descripción: Obtener tipos de muestra filtrado por ID
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipoMuestra"></param>
        /// <returns></returns>
        public TipoMuestra GetTipoMuestraById(int idTipoMuestra)
        {
            using (var tipoMuestraDal = new TipoMuestraDal())
            {
                return tipoMuestraDal.GetTipoMuestraById(idTipoMuestra);
            }
        }
        /// <summary>
        /// Descripción: Registra tipos de muestra
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipoM"></param>
        public void InsertTipoMuestra(TipoMuestra tipoM)
        {
            using (var tipoMuestraDal = new TipoMuestraDal())
            {
                tipoMuestraDal.InsertTipoMuestra(tipoM);
            }
        }
        /// <summary>
        /// Descripción: Actualiza los tipos de muestra.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="tipoM"></param>
        public void UpdateTipoMuestra(TipoMuestra tipoM)
        {
            using (var tipoMuestraDal = new TipoMuestraDal())
            {
                tipoMuestraDal.UpdateTipoMuestra(tipoM);
            }
        }
    }
}

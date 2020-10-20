using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using System;
using Model;

namespace BusinessLayer
{
    public class ExamenBl : IExamenBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por Nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByNombre(string nombre)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenesByNombre(nombre);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por idExamen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public Examen GetExamenById(Guid idExamen)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenById(idExamen);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por Laboratorio, Nombre y genero.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLaboratorio"></param>
        /// <param name="genero"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByIdLaboratorio(int idEnfermedad, int genero, String data)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenesByIdLaboratorio(idEnfermedad, genero, data);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por Laboratorio, Nombre y genero.
        /// Author: SOTERO BUSTAMANTE.
        /// Fecha Creacion: 19/11/2017
        /// Fecha Modificación: 19/11/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idLaboratorio"></param>
        /// <param name="genero"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByTipoMuestra(string idOrden, string data, int idEnfermedad)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenesByTipoMuestra(idOrden, data, idEnfermedad);
            }
        }
        /// <summary>
        /// Descripción: Obtiene información de los examenes filtrados por Nombre y genero
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="genero"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByGenero(int genero, String data)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenesByGenero(genero, data);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por idExamen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<ExamenMetodo> GetMetodoByExamen(Guid idExamen)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetMetodoByExamen(idExamen);
            }
        }
        /// <summary>
        /// Descripción: Registra un examen nuevo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenMetodo"></param>
        public void InsertMetodoByExamen(ExamenMetodo examenMetodo)
        {
            using (var examenDal = new ExamenDal())
            {
                examenDal.InsertMetodoByExamen(examenMetodo);
            }
        }
        /// <summary>
        /// Descripción: Actualiza un examen.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examenMetodo"></param>
        public void UpdateMetodoByExamen(ExamenMetodo examenMetodo)
        {
            using (var examenDal = new ExamenDal())
            {
                examenDal.UpdateMetodoByExamen(examenMetodo);
            }
        }

        /// <summary>
        /// Descripción: Registro de examenes
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examen"></param>
        public void InsertExamen(Examen examen)
        {
            using (var examenDal = new ExamenDal())
            {
                examenDal.InsertExamen(examen);
            }
        }
        /// <summary>
        /// Descripción: Actualziacion de examenes
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="examen"></param>
        public void UpdateExamen(Examen examen)
        {
            using (var examenDal = new ExamenDal())
            {
                examenDal.UpdateExamen(examen);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por idExamen y genero
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByIdEnfermedad(int idEnfermedad, int genero, String data)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenesByIdEnfermedad(idEnfermedad, genero, data);
            }
        }

        public List<Examen> GetExamenesByIdEnfermedadOrden(int idEnfermedad, String data)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenesByIdEnfermedadOrden(idEnfermedad, data);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los examenes filtrados por idLab
        /// Author: Juan Muga.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <param name="genero"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<Examen> GetExamenesByIdLaboratorioRecepcion(int idLaboratorio, int genero, String data)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenesByIdLaboratorioRecepcion(idLaboratorio, genero, data);
            }
        }
        public List<Examen> GetExamenUsuario(string data, int idUsuario)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenUsuario(data,idUsuario);
            }
        }

        public List<Examen> GetExamenesByIdEnfermedadAgrupado(int idEnfermedad)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenesByIdEnfermedadAgrupado(idEnfermedad);
            }
        }

        public List<TipoMuestra> GetExamenAgrupado(int idExamenAgrupado)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenAgrupado(idExamenAgrupado);
            }
        }

        public List<Examen> GetExamenesPorEnfermedad(int idEnfermedad)
        {
            using (var examenDal = new ExamenDal())
            {
                return examenDal.GetExamenesPorEnfermedad(idEnfermedad);
            }
        }
    }
}

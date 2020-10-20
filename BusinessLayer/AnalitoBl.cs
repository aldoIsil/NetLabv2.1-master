using BusinessLayer.Interfaces;
using DataLayer;
using System.Collections.Generic;
using System;
using Model;

namespace BusinessLayer
{
    public class AnalitoBl : IAnalitoBl
    {
        /// <summary>
        /// Descripción: Obteniene todos los analitos activos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Analito> GetAnalitos()
        {
            using (var analitoDal = new AnalitoDal())
            {
                return analitoDal.GetAnalitos();
            }
        }
        /// <summary>
        /// Descripción: Obteniene todos los analitos activos filtrado por nombre
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Analito> GetAnalitosByNombre(string nombre)
        {
            using (var analitoDal = new AnalitoDal())
            {
                return analitoDal.GetAnalitos(nombre);
            }
        }
        /// <summary>
        /// Descripción: Obteniene todos los analitos activos filtrado por el codigo del analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public Analito GetAnalitoById(Guid idAnalito)
        {
            using (var analitoDal = new AnalitoDal())
            {
                return analitoDal.GetAnalitoById(idAnalito);
            }
        }
        /// <summary>
        /// Descripción: Obteniene todos los analitos activos filtrado por Codigo del examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<Analito> GetAnalitosByIdExamen(Guid idExamen)
        {
            using (var analitoDal = new AnalitoDal())
            {
                return analitoDal.GetAnalitosByIdExamen(idExamen);
            }
        }
        /// <summary>
        /// Descripción: Obtener información de Analitos filtrado por nombre y/o descripcion
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Analito> SearchAnalitos(string search)
        {
            using (var analitoDal = new AnalitoDal())
            {
                return analitoDal.SearchAnalitos(search);
            }
        }
        /// <summary>
        /// Descripción: Registra información de Analitos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="analito"></param>
        public void RegistrarAnalito(Analito analito)
        {
            using (var analitoDal = new AnalitoDal())
            {
                analitoDal.InsertAnalito(analito);
            }
        }
        /// <summary>
        /// Descripción: Actualiza información de Analitos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="analito"></param>
        public void ActualizarAnalito(Analito analito)
        {
            using (var analitoDal = new AnalitoDal())
            {
                analitoDal.UpdateAnalito(analito);
            }
        }
        /// <summary>
        /// Descripción: Obtiene información de los metodos de un Analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public List<ExamenMetodo> GetMetodosByAnalito(Guid idAnalito)
        {
            using (var dal = new AnalitoMetodoDal())
            {
                return dal.GetAnalitoMetodoByAnalito(idAnalito);
            }
        }
        /// <summary>
        /// Descripción: Obtiene información de los metodos de un Analito
        /// No existe el SP y el metodo no se utiliza!!!!!
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="metodo"></param>
        public void RegistrarMetodo(ExamenMetodo metodo)
        {
            using (var dal = new AnalitoMetodoDal())
            {
                dal.InsertAnalitoMetodo(metodo);
            }
        }
        /// <summary>
        ///  Descripción: Modifica datos del metodos de un analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="metodo"></param>
        public void ActualizarMetodo(ExamenMetodo metodo)
        {
            using (var dal = new AnalitoMetodoDal())
            {
                dal.UpdateAnalitoMetodo(metodo);
            }
        }
        /// <summary>
        /// Descripción: Obtiene las opciones ingresados por cada analito.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public List<AnalitoOpcionResultado> GetOpcionesByAnalito(Guid idAnalito)
        {
            using (var dal = new AnalitoOpcionDal())
            {
                return dal.GetAnalitoOpcionByAnalito(idAnalito);
            }
        }
        /// <summary>
        /// Descripción: Obtiene información de AnalitosOpcion de resultados.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoOpcion"></param>
        /// <returns></returns>
        public AnalitoOpcionResultado GetOpcionById(Guid idAnalito, int idAnalitoOpcion)
        {
            using (var dal = new AnalitoOpcionDal())
            {
                return dal.GetAnalitoOpcionById(idAnalito, idAnalitoOpcion);
            }
        }
        /// <summary>
        /// Descripción: Registra información de Analitos y sus opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcion"></param>
        public void RegistrarOpcion(AnalitoOpcionResultado opcion)
        {
            using (var dal = new AnalitoOpcionDal())
            {
                dal.InsertAnalitoOpcion(opcion);
            }
        }
        /// <summary>
        /// Descripción: Actualiza información de Analitos y sus opciones.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcion"></param>
        public void ActualizarOpcion(AnalitoOpcionResultado opcion)
        {
            using (var dal = new AnalitoOpcionDal())
            {
                dal.UpdateAnalitoOpcion(opcion);
            }
        }
        /// <summary>
        /// Descripción: Obtiene información de los valores de los Analitos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <returns></returns>
        public List<AnalitoValorNormal> GetValoresByAnalito(Guid idAnalito)
        {
            using (var dal = new AnalitoValorDal())
            {
                return dal.GetAnalitoValorByAnalito(idAnalito);
            }
        }
        /// <summary>
        /// Descripción: Obtener información de los valores standar de los Analitos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idAnalito"></param>
        /// <param name="idAnalitoValor"></param>
        /// <returns></returns>
        public AnalitoValorNormal GetValorById(Guid idAnalito, int idAnalitoValor)
        {
            using (var dal = new AnalitoValorDal())
            {
                return dal.GetAnalitoValorById(idAnalito, idAnalitoValor);
            }
        }
        /// <summary>
        /// Descripción: Registra información de los valores normales del resultado de un Analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="Valor"></param>
        public void RegistrarValor(AnalitoValorNormal valor)
        {
            using (var dal = new AnalitoValorDal())
            {
                dal.InsertAnalitoValor(valor);
            }
        }
        /// <summary>
        /// Descripción: Actualiza información de los valores normales del resultado de un Analito
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="Valor"></param>
        public void ActualizarValor(AnalitoValorNormal valor)
        {
            using (var dal = new AnalitoValorDal())
            {
                dal.UpdateAnalitoValor(valor);
            }
        }

        public List<ExamenResultadoInterpretacion> ObtenerAnalitoInterpretacion(Guid idAnalito)
        {
            using (var dal = new AnalitoInterpretacionDal())
            {
                return dal.ObtenerAnalitoInterpretacion(idAnalito);
            }
        }

        public void RegistrarNuevaInterpretacion(string idAnalito, string GlosaParent, string Glosa, string Interpretacion, int idUsuario)
        {
            using (var dal = new AnalitoInterpretacionDal())
            {
                dal.RegistrarNuevaInterpretacion(idAnalito, GlosaParent, Glosa, Interpretacion, idUsuario);
            }
        }

        public ExamenResultadoInterpretacion ObtenerAnalitoInterpretacionPorId(int idInterpretacion)
        {
            using (var dal = new AnalitoInterpretacionDal())
            {
                return dal.ObtenerAnalitoInterpretacionPorId(idInterpretacion);
            }
        }

        public void EditarAnalitoInterpretacion(int idInterpretacion, string GlosaParent, string Glosa, string Interpretacion, int estado, int idUsuario)
        {
            using (var dal = new AnalitoInterpretacionDal())
            {
                dal.EditarAnalitoInterpretacion(idInterpretacion, GlosaParent, Glosa, Interpretacion, estado, idUsuario);
            }
        }
    }
}

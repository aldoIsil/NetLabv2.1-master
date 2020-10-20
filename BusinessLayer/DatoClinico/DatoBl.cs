using System;
using System.Collections.Generic;
using System.Data;
using BusinessLayer.DatoClinico.Interfaces;
using DataLayer;
using DataLayer.Area.DatoClinico;
using Model;

namespace BusinessLayer.DatoClinico
{
    public class DatoBl : IDatoBl
    {
        /// <summary>
        /// Descripción: Obtiene informacion de las opciones de datos por examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Dato> GetDatos()
        {
            using (var datoDal = new DatoDal())
            {
                return datoDal.GetDatos();
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las opciones de datos por examen
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idEnfermedad"></param>
        /// <returns></returns>
        public List<Dato> GetDatosByIdEnfermedad(string enfermedad)
        {
            using (var datoDal = new DatoDal())
            {
                return datoDal.GetDatosByIdEnfermedad(enfermedad);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los datos clinicos por categoria
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idCategoria"></param>
        /// <returns></returns>
        public List<Dato> GetDatoByCategoria(int idCategoria)
        {
            using (var datoClinicoDal = new DatoClinicoDal())
            {
                var datos = datoClinicoDal.GetDatoByCategoria(idCategoria, string.Empty);

                datos.ForEach(d => d.Tipo = GetTipo(d.IdTipo));

                return datos;
            }
        }
        /// <summary>
        /// Descripción: Obtiene los tipo de datos activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<TipoDato> GetTipos()
        {
            using (var datoClinicoDal = new DatoClinicoDal())
            {
                return datoClinicoDal.GetTipoDato();
            }
        }
        /// <summary>
        /// Descripción: Registra los datos por categoria.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dato"></param>
        /// <param name="idCategoria"></param>
        public void InsertDato(Dato dato, int idCategoria)
        {
            using (var datoClinicoDal = new DatoClinicoDal())
            {
                datoClinicoDal.InsertDato(dato, idCategoria);
            }
        }
        /// <summary>
        /// Descripción: Obtiene los datos filtrado por su codigo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Dato GetDatoById(int id)
        {
            using (var datoClinicoDal = new DatoClinicoDal())
            {
                return datoClinicoDal.GetDatoById(id);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los datos y categorias enlazadas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idCategoria"></param>
        /// <returns></returns>
        public DatoCategoriaDato GetDatoCategoriaById(int id, int idCategoria)
        {
            using (var datoClinicoDal = new DatoClinicoDal())
            {
                return datoClinicoDal.GetDatoCategoriaById(id, idCategoria);
            }
        }
        /// <summary>
        /// Descripción: Actualiza informacion de los Datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dato"></param>
        public void UpdateDato(Dato dato, DatoCategoriaDato datoCategoria)
        {
            using (var datoClinicoDal = new DatoClinicoDal())
            {
                datoClinicoDal.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    datoClinicoDal.UpdateDato(dato);
                    datoClinicoDal.UpdateDatoCategoria(datoCategoria);

                    datoClinicoDal.Commit();
                }
                catch (Exception)
                {
                    datoClinicoDal.Rollback();
                }
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las Lista de datos activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<ListaDato> GetListaDato()
        {
            using (var listaDatoDal = new ListaDatoDal())
            {
                return listaDatoDal.GetListaDato();
            }
        }
        /// <summary>
        /// Descripción: Registra informacion de la Lista de Datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaDato"></param>
        public void InsertListaDato(ListaDato listaDato)
        {
            using (var listaDatoDal = new ListaDatoDal())
            {
                listaDatoDal.InsertListaDato(listaDato);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las listas de datos activos filtrado por el Id de ListaDato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idListaDato"></param>
        /// <returns></returns>
        public ListaDato GetListaDatoById(int id)
        {
            using (var listaDatoDal = new ListaDatoDal())
            {
                return listaDatoDal.GetListaDatoById(id);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las opciones de datos filtrado por el id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OpcionDato GetOpcionDatoById(int id)
        {
            using (var listaDatoDal = new ListaDatoDal())
            {
                return listaDatoDal.GetOpcionDatoById(id);
            }
        }
        /// <summary>
        /// Descripción: Obtiene informacion de las opciones activas filtradas por el id de listas.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idListaDato"></param>
        /// <returns></returns>
        public List<OpcionDato> GetOpcionDatoByLista(int id)
        {
            using (var listaDatoDal = new ListaDatoDal())
            {
                return listaDatoDal.GetOpcionDatoByLista(id);
            }
        }
        /// <summary>
        /// Descripción: Actualiza información de una Opcion de Dato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcionDato"></param>
        public void UpdateOpcionDato(OpcionDato opcionDato)
        {
            using (var listaDatoDal = new ListaDatoDal())
            {
                listaDatoDal.UpdateOpcionDato(opcionDato);
            }
        }
        /// <summary>
        /// Descripción: Registra información de una nueva Opcion de Dato.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="opcionDato"></param>
        public void InsertOpcionDato(OpcionDato opcionDato)
        {
            using (var listaDatoDal = new ListaDatoDal())
            {
                listaDatoDal.InsertOpcionDato(opcionDato);
            }
        }
        /// <summary>
        /// Descripción: Actualiza informacion de las listas/datos.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="listaDato"></param>
        public void UpdateListaDato(ListaDato listaDato)
        {
            using (var listaDatoDal = new ListaDatoDal())
            {
                listaDatoDal.UpdateListaDato(listaDato);
            }
        }
        /// <summary>
        /// Descripción: Obtiene los tipo de datos activos
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idTipo"></param>
        /// <returns></returns>
        private static TipoDato GetTipo(int idTipo)
        {
            using (var datoClinicoDal = new DatoClinicoDal())
            {
                return datoClinicoDal.GetTipoDatoById(idTipo);
            }
        }
    }
}

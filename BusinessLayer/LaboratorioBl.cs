using System.Collections.Generic;
using BusinessLayer.Interfaces;
using DataLayer;
using Model;
using System;
using System.Linq;

namespace BusinessLayer
{
    public class LaboratorioBl : ILaboratorioBl
    {
        /// <summary>
        /// Descripción: Metodo para obtener informacion de labortorio filtrado por un texto y el codigo de usuario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<LaboratorioVMSelect> GetLaboratoriosByTextoBusqueda(string textoBusqueda, int idUsuario)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                return laboratorioDal.GetLaboratoriosByTextoBusqueda(textoBusqueda, idUsuario,null);
            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de labortorio filtrado por un texto, el codigo de usuario y el GUID del Examen o metodo
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idUsuario"></param>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public List<LaboratorioVMSelect> GetLaboratoriosByTextoBusqueda(string textoBusqueda, int idUsuario, Guid idExamen)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                return laboratorioDal.GetLaboratoriosByTextoBusqueda(textoBusqueda, idUsuario, idExamen);
            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de Estsablecimiento filtrado por su Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Establecimiento GetEstablacimientoById(int id)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetEstablecimientoById(id);
            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de los establecimientos frecuentes filtrados por el Codigo del Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Establecimiento> GetEstablacimientosFrecuentesByIdUsuario(int idUsuario)
        {
            using (var establecimientoDal = new EstablecimientoDal())
            {
                return establecimientoDal.GetEstablecimientosFrecuentesByIdUsuario(idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de labortorio filtrado por el nombre.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<Laboratorio> GetLaboratoriosByNombre(string nombre)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                var nombreBusqueda = nombre == null ? string.Empty : nombre.ToUpper();
                var laboratorios = laboratorioDal.GetLaboratoriosByNombre(nombre);
                return laboratorios.Where(x => IsValidLaboratorio(x, nombreBusqueda)).ToList();
            }
        }
        /// <summary>
        /// Descripción: Metodo para validar que el dato no esté vacio o sea nulo.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 1.02/02/2017.
        ///                     2.06/04/2017
        /// Modificación: 1.Se agregaron comentarios.
        ///               2.Se modifica el retorno agregando el CodigoUnico - Juan Muga.
        /// </summary>
        /// <param name="laboratorio"></param>
        /// <param name="nombreBusqueda"></param>
        /// <returns></returns>
        private static bool IsValidLaboratorio(Laboratorio laboratorio, string nombreBusqueda)
        {
            int intParsed;
            if (int.TryParse(nombreBusqueda.Trim(), out intParsed))
            {
                nombreBusqueda = Convert.ToString(intParsed);
            }
            return !string.IsNullOrEmpty(laboratorio.Nombre) && laboratorio.Nombre.ToUpper().Contains(nombreBusqueda) || laboratorio.CodigoUnico.Contains(nombreBusqueda);
        }
        /// <summary>
        /// Descripción: Obtiene informacion de los laboratorios por el Id
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Laboratorio GetLaboratorioById(int id)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                return laboratorioDal.GetLaboratorioById(id);
            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de labortorio filtrado por  el codigo de usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public Laboratorio GetLaboratorioByUserId(int idUsuario)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                return laboratorioDal.GetLaboratorioByUserId(idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de labortorio filtrado por un texto.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <returns></returns>
        public List<Laboratorio> GetLaboratoriosAllByTextoBusqueda(string textoBusqueda)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                var laboratorios = laboratorioDal.GetLaboratoriosAllByTextoBusqueda(textoBusqueda);
                var nombreBusqueda = textoBusqueda == null ? string.Empty : textoBusqueda.ToUpper();
                return laboratorios.Where(x => IsValidLaboratorio(x, nombreBusqueda)).ToList();
            }
        }

        /// <summary>
        /// Descripción: Metodo para Actualizar informacion de los laboratorios
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="laboratorio"></param>
        public void UpdateLaboratorio(Laboratorio laboratorio)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                laboratorioDal.UpdateLaboratorio(laboratorio);
            }
        }

        /// <summary>
        /// Descripción: Metodo para Registrar informacion de los laboratorios
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="laboratorio"></param>
        public void InsertLaboratorio(Laboratorio laboratorio)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                laboratorioDal.InsertLaboratorio(laboratorio);
            }
        }

        /// <summary>
        /// Descripción: Metodo para obtener informacion de los laboratorios filtrados por Disa,Institucion,Red y microred.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="textoBusqueda"></param>
        /// <param name="idDisa"></param>
        /// <param name="idInstitucion"></param>
        /// <param name="idRed"></param>
        /// <param name="idMicrored"></param>
        /// <returns></returns>
        public List<Laboratorio> GetLaboratorioByMicroredByTextoBusqueda(string textoBusqueda, int idDisa, int idInstitucion, int idRed, int idMicrored)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                return laboratorioDal.GetLaboratorioByMicroredByTextoBusqueda(textoBusqueda, idDisa, idInstitucion, idRed, idMicrored);

            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de los laboratorios por Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Laboratorio> GetListaLaboratorioByUserId(int idUsuario)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                return laboratorioDal.GetListaLaboratorioByUserId(idUsuario);
            }
        }
        /// <summary>
        /// Descripción: Metodo para obtener informacion de los laboratorios por Usuario.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<UsuarioLaboratorio> GetUsuarioLaboratorioByUser(int idUsuario)
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                return laboratorioDal.GetUsuarioLaboratorioByUser(idUsuario);
            }
        }

        public List<LaboratorioVMSelect> GetLaboratoriossStaticCache()
        {
            using (var laboratorioDal = new LaboratorioDal())
            {
                return laboratorioDal.GetLaboratoriossStaticCache();
            }
        }
    }
}

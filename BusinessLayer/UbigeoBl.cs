using System;
using System.Collections.Generic;
using DataLayer;
using Model;
using Model.DTO;

namespace BusinessLayer
{
    public class UbigeoBl
    {
        public static List<UbigeoDTO> ubigeoDTOList;
        /// <summary>
        /// Descripción: Obtiene todos los Departamentos y los convierte en una lista.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<UbigeoDTO> ObtenerDepartamentos()
        {

            if (UbigeoBl.ubigeoDTOList == null)
            {
                try
                {
                    using (var ubigeoDal = new UbigeoDal())
                    {
                        ubigeoDTOList = ubigeoDal.ObtenerUbigeos();
                    }
                }
                catch (Exception e)
                {
                    String A = e.Message;
                }
            }
            return ubigeoDTOList;

        }
        /// <summary>
        /// Descripción: Obtiene todos los Departamentos 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Ubigeo> GetDepartamentos()
        {
            using (var ubigeoDal = new UbigeoDal())
            {
                return ubigeoDal.ObtenerDepartamentos();
            }
        }
        /// <summary>
        /// Descripción: Obtiene todas las provincias por departamento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDepartamento"></param>
        /// <returns></returns>
        public List<Ubigeo> GetProvincias(string idDepartamento)
        {
            using (var ubigeoDal = new UbigeoDal())
            {
                return ubigeoDal.ObtenerProvincias(idDepartamento);
            }
        }
        /// <summary>
        /// Descripción: Obtiene Distritos por Provincia.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDepartamento"></param>
        /// <param name="idProvincia"></param>
        /// <returns></returns>
        public List<Ubigeo> GetDistritos(string idDepartamento, string idProvincia)
        {
            using (var ubigeoDal = new UbigeoDal())
            {
                return ubigeoDal.ObtenerDistritos(idDepartamento, idProvincia);
            }
        }
        /// <summary>
        /// Descripción: Obtiene el ubigeo(departamento-Provincia-Distrito) por Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUbigeo"></param>
        /// <returns></returns>
        public Ubigeo GetUbigeoById(string idUbigeo)
        {
            using (var ubigeoDal = new UbigeoDal())
            {
                return ubigeoDal.GetUbigeoById(idUbigeo);
            }
        }
    }

    public class UbigeoPacienteBl
    {
        public static List<UbigeoDTO> ubigeoDTOList;
        /// <summary>
        /// Descripción: Obtiene todos los Departamentos y los convierte en una lista.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<UbigeoDTO> ObtenerDepartamentos()
        {

            if (UbigeoBl.ubigeoDTOList == null)
            {
                try
                {
                    using (var ubigeoDal = new UbigeoPacienteDal())
                    {
                        ubigeoDTOList = ubigeoDal.ObtenerUbigeos();
                    }
                }
                catch (Exception e)
                {
                    String A = e.Message;
                }
            }
            return ubigeoDTOList;

        }
        /// <summary>
        /// Descripción: Obtiene todos los Departamentos 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Ubigeo> GetDepartamentos()
        {
            using (var ubigeoDal = new UbigeoPacienteDal())
            {
                return ubigeoDal.ObtenerDepartamentos();
            }
        }
        /// <summary>
        /// Descripción: Obtiene todas las provincias por departamento.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDepartamento"></param>
        /// <returns></returns>
        public List<Ubigeo> GetProvincias(string idDepartamento)
        {
            using (var ubigeoDal = new UbigeoPacienteDal())
            {
                return ubigeoDal.ObtenerProvincias(idDepartamento);
            }
        }
        /// <summary>
        /// Descripción: Obtiene Distritos por Provincia.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idDepartamento"></param>
        /// <param name="idProvincia"></param>
        /// <returns></returns>
        public List<Ubigeo> GetDistritos(string idDepartamento, string idProvincia)
        {
            using (var ubigeoDal = new UbigeoPacienteDal())
            {
                return ubigeoDal.ObtenerDistritos(idDepartamento, idProvincia);
            }
        }
        /// <summary>
        /// Descripción: Obtiene el ubigeo(departamento-Provincia-Distrito) por Id.
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="idUbigeo"></param>
        /// <returns></returns>
        public Ubigeo GetUbigeoById(string idUbigeo, string descripcion, int idUsuario)
        {
            using (var ubigeoDal = new UbigeoPacienteDal())
            {
                return ubigeoDal.GetUbigeoById(idUbigeo, descripcion, idUsuario);
            }
        }
    }
}

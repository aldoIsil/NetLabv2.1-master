using Framework.DAL;
using Framework.DAL.Settings.Implementations;
using System;
using System.Data;
using System.Collections.Generic;
using DataLayer.DalConverter;
using Model;
using Model.DTO;

namespace DataLayer
{
    public class UbigeoDal  :DaoBase
    {
        public UbigeoDal(IDalSettings settings) : base(settings)
        {
        }

        public UbigeoDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene todos los Departamentos 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<UbigeoDTO> ObtenerUbigeos()
        {
            var objCommand = GetSqlCommand("pNLS_Ubigeo");
            //InputParameterAdd.Varchar(objCommand,"codDepartamento", codDepartamento);
            DataTable dataTableUbigeo = Execute(objCommand);

            List<UbigeoDTO> ubigeoDTODepartamentos = new List<UbigeoDTO>();
            UbigeoDTO ultimoDepartamento = new UbigeoDTO();
            ultimoDepartamento.codigo = "  ";
            UbigeoDTO ultimaProvincia = new UbigeoDTO();

            foreach (DataRow row in dataTableUbigeo.Rows)
            {

                String departamentoTmp = "";
                String provinciaTmp = "";
                String distritoTmp = "";

                UbigeoDTO ubigeoDTO = new UbigeoDTO
                {
                    codigo = Converter.GetString(row, "idUbigeo"),
                    nombre = Converter.GetString(row, "descripcion")
                };

                departamentoTmp = ubigeoDTO.codigo.Substring(0, 2);
                provinciaTmp = ubigeoDTO.codigo.Substring(2, 2);
                distritoTmp = ubigeoDTO.codigo.Substring(4, 2);

                //Si el departamento Cambia, la ubicacion se agrega como departamento
                if (!ultimoDepartamento.codigo.Equals(departamentoTmp))
                {
                    ultimoDepartamento = ubigeoDTO;
                    ultimoDepartamento.codigo = departamentoTmp;
                    ultimoDepartamento.tipo = 1;
                    //Se instancia la lista para las provincias
                    ultimoDepartamento.ubigeoDTOList = new List<UbigeoDTO>();
                    //Se agrega a la lista de departamentos
                    ubigeoDTODepartamentos.Add(ultimoDepartamento);
                    ultimaProvincia = new UbigeoDTO();
                    ultimaProvincia.codigo = "  ";
                }
                else if (!ultimaProvincia.codigo.Equals(provinciaTmp))
                {
                    
                    ultimaProvincia = ubigeoDTO;
                    ultimaProvincia.codigo = provinciaTmp;
                    ultimaProvincia.tipo = 2;
                    //Se instancia la lista para los distritos
                    ultimaProvincia.ubigeoDTOList = new List<UbigeoDTO>();
                    //Se agrega a la lista de provincias
                    ultimoDepartamento.ubigeoDTOList.Add(ultimaProvincia);
                }
                else
                {
                    //Se agrega a la lista de distritos
                    ubigeoDTO.codigo = distritoTmp;
                    ubigeoDTO.tipo = 3;
                    ultimaProvincia.ubigeoDTOList.Add(ubigeoDTO);
                }
            }
            return ubigeoDTODepartamentos;
        }
        /// <summary>
        /// Descripción: Obtiene todos los Departamentos 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Ubigeo> ObtenerDepartamentos()
        {
            var objCommand = GetSqlCommand("pNLS_Departamento");

            return UbigeoConvertTo.Ubigeos(Execute(objCommand));
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
        public List<Ubigeo> ObtenerProvincias(string idDepartamento)
        {
            var objCommand = GetSqlCommand("pNLS_Provincia");
            InputParameterAdd.Varchar(objCommand, "CodigoDepartamento", idDepartamento);

            return UbigeoConvertTo.Ubigeos(Execute(objCommand));
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
        public List<Ubigeo> ObtenerDistritos(string idDepartamento, string idProvincia)
        {
            var objCommand = GetSqlCommand("pNLS_Distrito");
            InputParameterAdd.Varchar(objCommand, "CodigoDepartamento", idDepartamento);
            InputParameterAdd.Varchar(objCommand, "CodigoProvincia", idProvincia);

            return UbigeoConvertTo.Ubigeos(Execute(objCommand));
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
            Ubigeo ubigeo = new Ubigeo();
            var objCommand = GetSqlCommand("pNLS_UbigeoById");
            InputParameterAdd.Varchar(objCommand, "idUbigeo", idUbigeo);
            OutputParameterAdd.Varchar(objCommand, "departamento",500);
            OutputParameterAdd.Varchar(objCommand, "provincia", 500);
            OutputParameterAdd.Varchar(objCommand, "distrito", 500);
            ExecuteNonQuery(objCommand);
            ubigeo.Id = idUbigeo;
            ubigeo.Departamento = (string)objCommand.Parameters["@departamento"].Value;
            ubigeo.Provincia = (string)objCommand.Parameters["@provincia"].Value;
            ubigeo.Distrito = (string)objCommand.Parameters["@distrito"].Value;
            return ubigeo;
        }
    }
    public class UbigeoPacienteDal : DaoBase
    {
        public UbigeoPacienteDal(IDalSettings settings) : base(settings)
        {
        }

        public UbigeoPacienteDal() : this(new NetlabSettings())
        {
        }
        /// <summary>
        /// Descripción: Obtiene todos los Departamentos 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<UbigeoDTO> ObtenerUbigeos()
        {
            var objCommand = GetSqlCommand("pNLS_UbigeoPaciente");
            //InputParameterAdd.Varchar(objCommand,"codDepartamento", codDepartamento);
            DataTable dataTableUbigeo = Execute(objCommand);

            List<UbigeoDTO> ubigeoDTODepartamentos = new List<UbigeoDTO>();
            UbigeoDTO ultimoDepartamento = new UbigeoDTO();
            ultimoDepartamento.codigo = "  ";
            UbigeoDTO ultimaProvincia = new UbigeoDTO();

            foreach (DataRow row in dataTableUbigeo.Rows)
            {

                String departamentoTmp = "";
                String provinciaTmp = "";
                String distritoTmp = "";

                UbigeoDTO ubigeoDTO = new UbigeoDTO
                {
                    codigo = Converter.GetString(row, "idUbigeo"),
                    nombre = Converter.GetString(row, "descripcion")
                };

                departamentoTmp = ubigeoDTO.codigo.Substring(0, 2);
                provinciaTmp = ubigeoDTO.codigo.Substring(2, 2);
                distritoTmp = ubigeoDTO.codigo.Substring(4, 2);

                //Si el departamento Cambia, la ubicacion se agrega como departamento
                if (!ultimoDepartamento.codigo.Equals(departamentoTmp))
                {
                    ultimoDepartamento = ubigeoDTO;
                    ultimoDepartamento.codigo = departamentoTmp;
                    ultimoDepartamento.tipo = 1;
                    //Se instancia la lista para las provincias
                    ultimoDepartamento.ubigeoDTOList = new List<UbigeoDTO>();
                    //Se agrega a la lista de departamentos
                    ubigeoDTODepartamentos.Add(ultimoDepartamento);
                    ultimaProvincia = new UbigeoDTO();
                    ultimaProvincia.codigo = "  ";
                }
                else if (!ultimaProvincia.codigo.Equals(provinciaTmp))
                {

                    ultimaProvincia = ubigeoDTO;
                    ultimaProvincia.codigo = provinciaTmp;
                    ultimaProvincia.tipo = 2;
                    //Se instancia la lista para los distritos
                    ultimaProvincia.ubigeoDTOList = new List<UbigeoDTO>();
                    //Se agrega a la lista de provincias
                    ultimoDepartamento.ubigeoDTOList.Add(ultimaProvincia);
                }
                else
                {
                    //Se agrega a la lista de distritos
                    ubigeoDTO.codigo = distritoTmp;
                    ubigeoDTO.tipo = 3;
                    ultimaProvincia.ubigeoDTOList.Add(ubigeoDTO);
                }
            }
            return ubigeoDTODepartamentos;
        }
        /// <summary>
        /// Descripción: Obtiene todos los Departamentos 
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        public List<Ubigeo> ObtenerDepartamentos()
        {
            var objCommand = GetSqlCommand("pNLS_DepartamentoPaciente");

            return UbigeoConvertTo.Ubigeos(Execute(objCommand));
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
        public List<Ubigeo> ObtenerProvincias(string idDepartamento)
        {
            var objCommand = GetSqlCommand("pNLS_ProvinciaPaciente");
            InputParameterAdd.Varchar(objCommand, "CodigoDepartamento", idDepartamento);

            return UbigeoConvertTo.Ubigeos(Execute(objCommand));
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
        public List<Ubigeo> ObtenerDistritos(string idDepartamento, string idProvincia)
        {
            var objCommand = GetSqlCommand("pNLS_DistritoPaciente");
            InputParameterAdd.Varchar(objCommand, "CodigoDepartamento", idDepartamento);
            InputParameterAdd.Varchar(objCommand, "CodigoProvincia", idProvincia);

            return UbigeoConvertTo.Ubigeos(Execute(objCommand));
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
            Ubigeo ubigeo = new Ubigeo();
            var objCommand = GetSqlCommand("pNLS_UbigeoPacienteById");
            InputParameterAdd.Varchar(objCommand, "idUbigeo", idUbigeo);
            InputParameterAdd.Varchar(objCommand, "Descripcion", descripcion);
            InputParameterAdd.Int(objCommand, "idUsuario", idUsuario);
            OutputParameterAdd.Varchar(objCommand, "departamento", 500);
            OutputParameterAdd.Varchar(objCommand, "provincia", 500);
            OutputParameterAdd.Varchar(objCommand, "distrito", 500);
            ExecuteNonQuery(objCommand);
            ubigeo.Id = idUbigeo;
            ubigeo.Departamento = (string)objCommand.Parameters["@departamento"].Value;
            ubigeo.Provincia = (string)objCommand.Parameters["@provincia"].Value;
            ubigeo.Distrito = (string)objCommand.Parameters["@distrito"].Value;
            return ubigeo;
        }
    }
}

using System;
using System.Web;
using BusinessLayer;
using BusinessLayer.Interfaces;
using Model;
using NetLab.Controllers.FormConverter.Entities;
using NetLab.Controllers.FormConverter.Interfaces;

namespace NetLab.Controllers.FormConverter
{
    [Serializable()]
    public class LaboratorioConverter : ILaboratorioConverter
    {
        private readonly ILaboratorioBl _laboratorioBl;

        public LaboratorioConverter(ILaboratorioBl laboratorioBl)
        {
            _laboratorioBl = laboratorioBl;
        }

        public LaboratorioConverter() : this(new LaboratorioBl())
        {
        }
        /// <summary>
        /// Descripción: Convierte una entidad de tipo LaboratorioConverterRequest a Laboratorio
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Actualizado por: Marcos Mejia
        /// Fecha: 19/01/2017
        /// Modificación: Se agrega dato del logo Regional.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Laboratorio ConvertFrom(LaboratorioConverterRequest request)
        {
            var laboratorio = _laboratorioBl.GetLaboratorioById(request.Laboratorio.IdLaboratorio);

            //var logo = GetBytes(request.LaboratorioModel.Logo) ?? laboratorio.Logo;
            //var sello = GetBytes(request.LaboratorioModel.Sello) ?? laboratorio.Sello;
            //var logoRegional = GetBytes(request.LaboratorioModel.LogoRegional) ?? laboratorio.LogoRegional;
            var logo = laboratorio.Logo;
            var sello = laboratorio.Sello;
            var logoRegional = laboratorio.LogoRegional;

            return new Laboratorio
            {
                IdLaboratorio = request.Laboratorio.IdLaboratorio,
                CodigoInstitucion = request.Laboratorio.CodigoInstitucion,
                CodigoUnico = request.Laboratorio.CodigoUnico,
                Nombre = request.Laboratorio.Nombre,
                clasificacion = request.Laboratorio.clasificacion,
                Ubigeo = request.Laboratorio.Ubigeo,
                Direccion = request.Laboratorio.Direccion,
                IdDisa = request.Laboratorio.IdDisa,
                IdRed = request.Laboratorio.IdRed,
                IdMicroRed = request.Laboratorio.IdMicroRed,
                IdCategoria = request.Laboratorio.IdCategoria,
                Latitud = request.Laboratorio.Latitud,
                Longitud = request.Laboratorio.Longitud,
                correoElectronico = request.Laboratorio.correoElectronico,
                LogoRegional = logoRegional,
                Logo = logo,
                Sello = sello,
                tipo = request.Laboratorio.tipo,
                Website = request.Laboratorio.Website,
                Estado = request.Laboratorio.Estado,
                IdUsuarioRegistro = request.IdUsuarioLogueado,
                IdUsuarioEdicion = request.IdUsuarioLogueado
            };
        }

        /// <summary>
        /// Descripción: Convierte una entidad de tipo LaboratorioConverterRequest a Laboratorio
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// Actualizado por: Marcos Mejia
        /// Fecha: 19/01/2017
        /// Modificación: Se agrega dato del logo Regional.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Laboratorio ConvertFromInsert(LaboratorioConverterRequest request)
        {
            //  var laboratorio = _laboratorioBl.GetLaboratorioById(request.Laboratorio.IdLaboratorio);

            var logo = request.LaboratorioModel.InsertarLogo;
            var sello = request.LaboratorioModel.InsertarSello;
            var logoRegional = request.LaboratorioModel.InsertarLogoRegional;

            return new Laboratorio
            {
               // IdLaboratorio = request.Laboratorio.IdLaboratorio,
                CodigoInstitucion = request.Laboratorio.CodigoInstitucion,
                CodigoUnico = request.Laboratorio.CodigoUnico,
                Nombre = request.Laboratorio.Nombre,
                clasificacion=request.Laboratorio.clasificacion,
                Ubigeo = request.Laboratorio.Ubigeo,
                Direccion = request.Laboratorio.Direccion,
                IdDisa = request.Laboratorio.IdDisa,
                IdRed = request.Laboratorio.IdRed,
                IdMicroRed = request.Laboratorio.IdMicroRed,
                IdCategoria = request.Laboratorio.IdCategoria,
                Latitud = request.Laboratorio.Latitud,
                Longitud = request.Laboratorio.Longitud,
                tipo=request.Laboratorio.tipo,
                correoElectronico=request.Laboratorio.correoElectronico,
                LogoRegional = logoRegional,
                Logo = logo,
                Sello = sello,
                Website = request.Laboratorio.Website,
                Estado = request.Laboratorio.Estado,
                IdUsuarioRegistro = request.IdUsuarioLogueado,
                IdUsuarioEdicion = request.IdUsuarioLogueado
            };
        }

        /// <summary>
        /// Descripción: Convierte un archivo en binario
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static byte[] GetBytes(HttpPostedFileBase file)
        {
            if (!(file?.ContentLength > 0)) return null;

            var imgBinaryData = new byte[file.ContentLength];
            file.InputStream.Read(imgBinaryData, 0, file.ContentLength);

            return imgBinaryData;
        }
    }
}
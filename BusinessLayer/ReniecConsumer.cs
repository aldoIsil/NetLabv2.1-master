using System;
using Framework.WebCommon;
using Reniec;
using Reniec.Entities;
using Reniec.Interfaces;

namespace BusinessLayer
{
    public class ReniecConsumer : IReniecConsumer
    {
        private readonly IReniecProxy _reniecProxy;
        public string ErrorMessage { get; set; }

        public ReniecConsumer(IReniecProxy reniecProxy)
        {
            _reniecProxy = reniecProxy;
        }

        public ReniecConsumer() : this(GetInstance())
        {
        }
        /// <summary>
        /// Descripción: Conecta a webservice de la reniec
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <returns></returns>
        private static IReniecProxy GetInstance()
        {
            var useReniecService = WebConfig.GetSetting<bool>("useReniecService");

            if (!useReniecService) return new ReniecProxyMock();

            return new ReniecProxy();
        }
        /// <summary>
        /// Descripción: Conecta a webservice de la reniec
        /// Author: Terceros.
        /// Fecha Creacion: 01/01/2017
        /// Fecha Modificación: 02/02/2017.
        /// Modificación: Se agregaron comentarios.
        /// </summary>
        /// <param name="dni"></param>
        /// <returns></returns>
        public Persona getReniec(string dni)
        {
            try
            {
                return _reniecProxy.getReniec(dni);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Hubieron problemas al conectarse al servicio de la Reniec.";
                return null;
            }
            
        }
    }
}

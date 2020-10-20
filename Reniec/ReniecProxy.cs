using Reniec.Entities;
using Reniec.Interfaces;
using Reniec.ReniecService;
using Reniec.ReniecNewService;

namespace Reniec
{
    public class ReniecProxy : IReniecProxy
    {
        private readonly IReniecConverter _reniecConverter;

        public ReniecProxy(IReniecConverter reniecConverter)
        {
            _reniecConverter = reniecConverter;
        }

        public ReniecProxy() : this(new ReniecConverter())
        {
        }

        public Persona getReniecAnt(string dni)
        {
            var client = new ServiceDNISoapClient();

            var personaArray = client.GetReniec(string.Empty, dni);

            var persona = _reniecConverter.Convert(personaArray);

            return persona;
        }

        public Persona getReniec(string dni)
        {
            Credencialmq cm = new Credencialmq
            {
                app = "INS",
                usuario = "41272665",
                clave = "@56%#.90+Z01"

            };
            var client = new serviciomqSoapClient();
            var personaArray = client.obtenerDatosBasicos(cm, dni);

            if (personaArray == null)
            {
                getReniecAnt(dni);
            }
            var persona = _reniecConverter.ConvertNew(personaArray);

            return persona;
        }
    }
}

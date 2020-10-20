using Renaes.Entities;
using Renaes.Interfaces;
using Renaes.Service_References.RenaesService;

namespace Renaes
{
    public class RenaesProxy : IRenaesProxy
    {
        private readonly IRenaesConverter _renaesConverter;

        public RenaesProxy(IRenaesConverter renaesConverter)
        {
            _renaesConverter = renaesConverter;
        }

        public RenaesProxy() : this(new RenaesConverter())
        {
        }

        public Establecimiento getEstablecimiento(string codigoEstablecimiento)
        {
            var client = new ServicioEstablecimientoSoapClient();
            
            var dsEstablecimiento = client.GetEstablecimiento(codigoEstablecimiento);
            
            var establecimiento = _renaesConverter.Convert(dsEstablecimiento);
            
            return establecimiento;
        }
    }
}

using Model;
using NetLab.Controllers.FormConverter.Entities;

namespace NetLab.Controllers.FormConverter.Interfaces
{
    public interface IDatoConverter
    {
        Dato ConvertFrom(DatoConverterRequest request);
    }
}
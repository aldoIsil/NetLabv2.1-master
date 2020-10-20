using Model;
using NetLab.Controllers.FormConverter.Entities;

namespace NetLab.Controllers.FormConverter.Interfaces
{
    public interface IAnimalConverter
    {
        Animal ConvertFrom(AnimalConverterRequest request);
    }
}
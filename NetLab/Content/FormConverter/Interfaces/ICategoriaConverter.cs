using Model;
using NetLab.Controllers.FormConverter.Entities;

namespace NetLab.Controllers.FormConverter.Interfaces
{
    public interface ICategoriaConverter
    {
        CategoriaDato ConvertFrom(CategoriaConverterRequest request);
    }
}
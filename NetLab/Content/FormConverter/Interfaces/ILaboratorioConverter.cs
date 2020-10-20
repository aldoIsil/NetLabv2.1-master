using Model;
using NetLab.Controllers.FormConverter.Entities;

namespace NetLab.Controllers.FormConverter.Interfaces
{
    public interface ILaboratorioConverter
    {
        Laboratorio ConvertFrom(LaboratorioConverterRequest request);
        Laboratorio ConvertFromInsert(LaboratorioConverterRequest request);
    }
}
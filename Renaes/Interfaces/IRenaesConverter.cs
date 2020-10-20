using Renaes.Entities;
using System.Data;

namespace Renaes.Interfaces
{
    public interface IRenaesConverter
    {
        Establecimiento Convert(DataSet dsEstablecimiento);
    }
}

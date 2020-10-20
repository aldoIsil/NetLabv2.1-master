using Model;
using System;
using System.Collections.Generic;


namespace BusinessLayer.Interfaces
{
    public interface IDisaBI
    {

        List<DisaMant> GetDisas(string nombre = null);

        DisaMant GetDisaByID(string idDisa);

        void InsertDisa(DisaMant disa);

        void UpdateDisa(DisaMant disa);

    }
}

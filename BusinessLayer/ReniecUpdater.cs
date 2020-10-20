using Reniec.Entities;
using Model;
using Model.Enums;

namespace BusinessLayer
{
    public class ReniecUpdater : IReniecUpdater
    {
        public Paciente GetPaciente(Persona persona)
        {
            if (persona == null) return null;

            return new Paciente
            {
                ApellidoPaterno = persona.ApellidoPaterno,
                ApellidoMaterno = persona.ApellidoMaterno,
                Nombres = persona.Nombres,
                FechaNacimiento = persona.FechaNacimiento,
                Genero = ConvertGenero(persona.Genero),
                DireccionReniec = persona.Direccion.DireccionReniec,
                DireccionActual = persona.Direccion.DireccionReniec,
                UbigeoActual = new Ubigeo { Id = persona.Direccion.CodigoUbigeo },
                UbigeoReniec = new Ubigeo { Id = persona.Direccion.CodigoUbigeo }
            };
        }

        public static int ConvertGenero(Genero genero)
        {
            switch (genero)
            {
                case Genero.Masculino:
                    return 1;
                default:
                    return 2;
            }
        }
    }
}

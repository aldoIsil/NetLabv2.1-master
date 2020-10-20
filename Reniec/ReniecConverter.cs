using Reniec.Interfaces;
using Reniec.Entities;
using Reniec.Entities.Enums;
using System;
using System.Collections.Generic;
using Model.Enums;

namespace Reniec
{
    public class ReniecConverter : IReniecConverter
    {
        private const string NotFoundIndicator = "DNE";

        public Persona Convert(string[] personaArray)
        {
            if (NotFound(personaArray)) return null;

            return new Persona
            {
                Nombres = personaArray[3],
                ApellidoPaterno = personaArray[1],
                ApellidoMaterno = personaArray[2],
                Direccion = GetDireccion(personaArray),
                //DonacionOrgano = GetDonacionOrgano(personaArray[20]),
                Genero = GetGenero(personaArray[17]),
                FechaNacimiento = GetFechaNacimiento(personaArray[18])
            };
        }

        private static bool NotFound(IReadOnlyList<string> personaArray)
        {
            if (personaArray == null) return true;
            return personaArray.Count == 1 && personaArray[0].Equals(NotFoundIndicator);
        }

        private static Direccion GetDireccion(IReadOnlyList<string> personaArray)
        {
            return new Direccion
            {
                CodigoUbigeo = personaArray[6] + personaArray[7] + personaArray[8],
                NombreDistrito = personaArray[14],
                DireccionReniec = personaArray[16]
            };
        }

        private static DonacionOrgano GetDonacionOrgano(string donacionOrgano)
        {
            switch (donacionOrgano)
            {
                case "N":
                    return DonacionOrgano.No;
                default:
                    return DonacionOrgano.Si;
            }
        }

        private static Genero GetGenero(string genero)
        {
            return genero == "1" ? Genero.Masculino : Genero.Femenino;
        }

        private static DateTime GetFechaNacimiento(string fecha)
        {
            return DateTime.ParseExact(fecha, "yyyyMMdd", null);
        }

        public Persona ConvertNew(string[] personaArray)
        {
            if (NotFound(personaArray)) return null;
            if (personaArray[3] == "SIN DATOS")
            {
                if (personaArray[4] == "SIN DATOS")
                {
                    personaArray[3] = "";
                }
                else
                {
                    personaArray[3] = personaArray[4];
                }
            }
            return new Persona
            {
                Nombres = personaArray[5],
                ApellidoPaterno = personaArray[2],
                ApellidoMaterno = personaArray[3],
                Direccion = GetDireccionNew(personaArray),
                //DonacionOrgano = GetDonacionOrgano(personaArray[20]),
                Genero = GetGenero(personaArray[19]),
                FechaNacimiento = GetFechaNacimientoNew(personaArray[20])
            };
        }

        private static Direccion GetDireccionNew(IReadOnlyList<string> personaArray)
        {
            return new Direccion
            {
                CodigoUbigeo = personaArray[8] + personaArray[9] + personaArray[10],
                NombreDistrito = personaArray[16],
                DireccionReniec = personaArray[18]
            };
        }
        private static DateTime GetFechaNacimientoNew(string fecha)
        {
            return DateTime.ParseExact(fecha, "dd/MM/yyyy", null);
        }

    }
}

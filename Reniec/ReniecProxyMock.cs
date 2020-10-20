using Reniec.Interfaces;
using Reniec.Entities;
using Reniec.Entities.Enums;
using System;
using Model.Enums;

namespace Reniec
{
    public class ReniecProxyMock : IReniecProxy
    {
        public Persona getReniec(string dni)
        {
            switch (dni)
            {
                case "12123123":
                    return new Persona
                    {
                        Nombres = "CÉSAR HUMBERTO",
                        ApellidoPaterno = "CORNEJO",
                        ApellidoMaterno = "LLANOS",
                        DonacionOrgano = DonacionOrgano.Si,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1986, 12, 26),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010101",
                            DireccionReniec = "LAS ROSAS 200 EDIF. 5 601 PASEO TOMAS VALLE II"
                        }
                    };
                case "44445555":
                    return new Persona
                    {
                        Nombres = "YOHANA",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "LLANOS",
                        DonacionOrgano = DonacionOrgano.Si,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1986, 12, 26),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010101",
                            DireccionReniec = "LAS ROSAS 200 EDIF. 5 601 PASEO TOMAS VALLE II"
                        }
                    };
                case "18190001":
                    return new Persona
                    {
                        Nombres = "FERNANDO",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121201":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121202":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121203":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121204":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121205":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121206":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121207":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121208":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121209":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };
                case "12121210":
                    return new Persona
                    {
                        Nombres = "CARLOS",
                        ApellidoPaterno = "ESPINOZA",
                        ApellidoMaterno = "SOLOZARNO",
                        DonacionOrgano = DonacionOrgano.No,
                        Genero = Genero.Masculino,
                        FechaNacimiento = new DateTime(1979, 9, 21),
                        Direccion = new Direccion
                        {
                            CodigoUbigeo = "010102",
                            DireccionReniec = "MIRAFLORES N 09"
                        }
                    };

                default:
                    return null;
            }
        }
    }
}

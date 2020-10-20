using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reniec.Test
{
    [TestClass]
    public class ReniecTests
    {
        private static string[] GetPersonaArray()
        {
            return new[] {
                "0000",
                "SANDY",
                "TORRES",
                "JENNIFER RAQUEL",
                "92",
                "33",
                "14",
                "01",
                "42",
                "000",
                "AMERICA",
                "PERU",
                "LIMA",
                "LIMA",
                "LOS OLIVOS",
                string.Empty,
                "LAS ROSAS 200 EDIF. 5 601 PASEO TOMAS VALLE II",
                "2",
                "19820124",
                "20141205",
                "S",
                "41272665",
                "22",
                string.Empty
            };
        }

        [TestMethod]
        public void ConverterTest()
        {
            var converter = new ReniecConverter();
            var personaArray = GetPersonaArray();

            var persona = converter.Convert(personaArray);

            Assert.IsNotNull(persona);
            Console.WriteLine(persona.Nombres);
            Console.WriteLine(persona.ApellidoPaterno);
            Console.WriteLine(persona.ApellidoMaterno);
            Console.WriteLine(persona.FechaNacimiento);
            Console.WriteLine(persona.Genero);
            Console.WriteLine(persona.DonacionOrgano);
            Console.WriteLine(persona.Direccion.CodigoUbigeo);
            Console.WriteLine(persona.Direccion.DireccionReniec);
        }
    }
}

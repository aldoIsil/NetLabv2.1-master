using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Model;
namespace DataLayer.DalConverter
{
    public class SerializarResultado
    {
        public List<AnalitoValues> DeserializarResultados(string json)
        {
            var Resultados = new List<AnalitoValues>();
            if (!String.IsNullOrEmpty(json))
            {
                Resultados = JsonConvert.DeserializeObject<List<AnalitoValues>>(json);
            }

            return Resultados;
        }
    }
}

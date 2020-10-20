using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Compartido
{
    public class Common
    {
        public string Tipo(int dato, string tipo)
        {
            var Rspta = "";
            if (tipo == "TipoEvaluacion")
            {
                switch (dato)
                {
                    case 1:
                        Rspta = "Directo";
                        break;
                    case 2:
                        Rspta = "Indirecto";
                        break;
                    default:
                        break;
                }
            }
            if (tipo == "TipoMetodo")
            {
                switch (dato)
                {
                    case 1:
                        Rspta = "Baciloscopia";
                        break;
                    case 2:
                        Rspta = "Suceptibilidad-Genotype";
                        break;
                    case 3:
                        Rspta = "Medio de Cultivo";
                        break;
                    default:
                        break;
                }
            }
            if (tipo == "TipoMetodoMaterial")
            {
                switch (dato)
                {
                    case 1:
                        Rspta = "RELECTURA DE DOBLE CIEGO DE LAMINAS DE BACILOSCOPIAS";
                        break;
                    case 2:
                        Rspta = "RIFAMPICINA";
                        break;
                    case 3:
                        Rspta = "ISONIACIDA";
                        break;
                    case 4:
                        Rspta = "KAT G";
                        break;
                    case 5:
                        Rspta = "ISONIACIDA A";
                        break;
                    case 6:
                        Rspta = "KAT G ISONIACIDA A";
                        break;
                    case 7:
                        Rspta = "Identificación molecular";
                        break;
                    case 8:
                        Rspta = "RESULTADOS DEL CRECIMIENTO BACTERIANO DE MTB";
                        break;
                    default:
                        break;
                }
            }
            if (tipo == "SubTipoMetodoMaterial")
            {
                switch (dato)
                {                    
                    default:
                        break;
                }
            }
            if (tipo == "Respuesta")
            {
                switch (dato)
                {
                    case 1:
                        Rspta = "NEGATIVO";
                        break;
                    case 2:
                        Rspta = "1-9 BAR";
                        break;
                    case 3:
                        Rspta = "1+";
                        break;
                    case 4:
                        Rspta = "2+";
                        break;
                    case 5:
                        Rspta = "3+";
                        break;
                    case 6:
                        Rspta = "SENSIBLE";
                        break;
                    case 7:
                        Rspta = "RESISTENTE";
                        break;
                    case 8:
                        Rspta = "EXCLUIDO";
                        break;
                    case 9:
                        Rspta = "NO TB";
                        break;
                    case 10:
                        Rspta = "CONTAMINADO";
                        break;
                    case 11:
                        Rspta = "COMPLEJO MTB";
                        break;
                    case 12:
                        Rspta = "OTROS";
                        break;
                    default:
                        break;
                }
            }
            return Rspta;
        }
        public string TipoRespuesta(int dato)
        {
            var respuesta = "";
            switch (dato)
            {
                case 1:
                    respuesta = "NEGATIVO";
                    break;
                case 2:
                    respuesta = "1-9 BAR";
                    break;
                case 3:
                    respuesta = "1+";
                    break;
                case 4:
                    respuesta = "2+";
                    break;
                case 5:
                    respuesta = "3+";
                    break;
                case 6:
                    respuesta = "SENSIBLE";
                    break;
                case 7:
                    respuesta = "RESISTENTE";
                    break;
                case 8:
                    respuesta = "EXCLUIDO";
                    break;
                case 9:
                    respuesta = "NO TB";
                    break;
                case 10:
                    respuesta = "CONTAMINADO";
                    break;
                case 11:
                    respuesta = "COMPLEJO MTB";
                    break;
                case 12:
                    respuesta = "OTROS";
                    break;
                default:
                    break;
            }
            return respuesta;
        }
    }
}

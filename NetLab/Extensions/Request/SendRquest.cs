using BusinessLayer;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using static Model.ResultadoWebService;

namespace NetLab.Extensions.Request
{
    public class SendRquest
    {
        public void EnviarResultados(string CodigoOrden)
        {
            //Enviar Resultados MINSA EQHALI
            //
            try
            {
                var request = new Request();
                var dato = new Resultado();
                var OrdenBL = new OrdenBl();
                HttpWebResponse response = null;
                var lista = OrdenBL.GetTramaResultadobyCodigoOrden(CodigoOrden);
                foreach (var item in lista)
                {
                    response = (HttpWebResponse)request.Execute("http://dlaboratorio.minsa.gob.pe/api/v1/laboratorio/recepcion-resultados", item, "POST");
                    if (HttpStatusCode.OK == response.StatusCode)
                    {
                        //Update Estado = Enviado
                        new OrdenBl().UpdateTRamaResultadobyCodigoOrden(CodigoOrden);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public class TokenObj
        {
            public string Access_Token { get; set; }
            public string Refresh_Token { get; set; }
            public string Scope { get; set; }
            public string Token_Type { get; set; }
        }

        public class RespuestaObj
        {
            public string Cod_Respuesta { get; set; }
            public string Des_Respuesta { get; set; }
        }

        public string generartoken()
        {
            try
            {
                string URL = ConfigurationManager.AppSettings["URLGenerarToken"].ToString();
                string authorizationHeader = ConfigurationManager.AppSettings["GenerateTokenAuthorizationHeader"].ToString();
                string token = string.Empty;
                //string URL = "https://apimanager.minsa.gob.pe:8243/token";
                HttpClient client = new HttpClient();
                var parameters = new Dictionary<string, string>();
                parameters.Add("username", "usr_ins_siscovid");
                parameters.Add("password", "M1ns4$s1scov1d");
                parameters.Add("grant_type", "password");
                string urlParameters = "?username=usr_ins_siscovid&password=M1ns4$s1scov1d&grant_type=password";
                //string authorizationHeader = "eWQ1Z0VhTFJHOW5FWGpmbzJaSjFPOHNXemJzYTpta21OaWhrN1htVHR2ak52M19GaEpzNVJNZEVh";
                //string authorizationHeader = "ZlA2Y2FOdHZheE82NklrMmsxUU1ieGh1VVFJYTpQRUNZZEo3eWtQZWUwRktCbHlYYWZ6YzRBNlFh";
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                HttpResponseMessage response = client.PostAsync(urlParameters, new FormUrlEncodedContent(parameters)).Result;
                if (response.IsSuccessStatusCode)
                {

                    string resultContent = response.Content.ReadAsStringAsync().Result;
                    var jsonobj = JsonConvert.DeserializeObject<TokenObj>(resultContent);
                    token = jsonobj.Access_Token;
                }

                return token;
            }
            catch (Exception ex)
            {
                //throw ex;
                return string.Empty;
            }
        }

        public string EnviarResultadosCovid(ResultadoCovidPaciente data)
        {
            var response = MainAsync(data);
            if (response.IsSuccessStatusCode)
            {
                string resultContent = response.Content.ReadAsStringAsync().Result;
                //ESTA RETORNANDO VACIO
                //var jsonobj = JsonConvert.DeserializeObject<RespuestaObj>(resultContent);
                //if (string.IsNullOrWhiteSpace(jsonobj.Cod_Respuesta) && string.IsNullOrWhiteSpace(jsonobj.Des_Respuesta))
                //{
                //    return "Error en Registro - retorna objeto vacio";
                //}
                //else
                //{
                return string.Empty;
                //}
            }
            else
            {
                return "Error en Registro - no exitoso";
            }
        }

        public string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Format("{0}{1}", "?", String.Join("&", properties.ToArray()));
        }

        static HttpResponseMessage MainAsync(ResultadoCovidPaciente data)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                    string uribase = ConfigurationManager.AppSettings["RegistrarPruebaUriBase"].ToString();
                    string requesturi = ConfigurationManager.AppSettings["RegistrarPruebaRequestUri"].ToString();
                    client.BaseAddress = new Uri(uribase); 
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var content = new FormUrlEncodedContent(new[]
                    //{
                    //    new KeyValuePair<string, string>("tip_documento", data.tip_documento),
                    //    new KeyValuePair<string, string>("nro_documento", data.nro_documento),
                    //    new KeyValuePair<string, string>("nombres", data.nombres),
                    //    new KeyValuePair<string, string>("ape_paterno", data.ape_paterno),
                    //    new KeyValuePair<string, string>("ape_materno", data.ape_materno),
                    //    new KeyValuePair<string, string>("nro_celular", data.nro_celular),
                    //    new KeyValuePair<string, string>("email", data.email),
                    //    new KeyValuePair<string, string>("ubigeo", data.ubigeo),
                    //    new KeyValuePair<string, string>("des_departamento", data.des_departamento),
                    //    new KeyValuePair<string, string>("des_provincia", data.des_provincia),
                    //    new KeyValuePair<string, string>("des_distrito", data.des_distrito),
                    //    new KeyValuePair<string, string>("direccion", data.direccion),
                    //    new KeyValuePair<string, string>("fec_coleccion", data.fec_coleccion),
                    //    new KeyValuePair<string, string>("hor_coleccion", data.hor_coleccion),
                    //    new KeyValuePair<string, string>("fec_validacion", data.fec_validacion),
                    //    new KeyValuePair<string, string>("rest_prueba", data.rest_prueba),
                    //    new KeyValuePair<string, string>("tip_prueba", data.tip_prueba)
                    //});

                    var payload = JsonConvert.SerializeObject(data);
                    HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");
                    var result = client.PostAsync(requesturi, c);
                    var resultContent = result.Result;
                    return resultContent;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
using PracticaS13_WEB.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace PracticaS13_WEB.Repository
{
    public class AbonoRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, string> _apiEndpoints;

        public AbonoRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;

            var baseUrl = _configuration.GetValue<string>("Variables:urlWebApi")!;

            _apiEndpoints = new Dictionary<string, string>
            {
                { "RegistrarAbono", $"{baseUrl}Abono/RegistrarAbono" }
            };
        }

        public async Task<RespuestaModel> RegistrarAbono(AbonoModel model)
        {
            RespuestaModel respuesta = new RespuestaModel();
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var url = _apiEndpoints["RegistrarAbono"];

                var httpResponse = await httpClient.PostAsJsonAsync(url, model);

                if (httpResponse.IsSuccessStatusCode)
                {
                    respuesta.Resultado = true;
                    respuesta.Mensaje = "Abono registrado exitosamente.";
                    respuesta.Datos = model;
                }
                else
                {
                    respuesta.Resultado = false;
                    respuesta.Mensaje = $"Error al registrar abono, estado HTTP: {httpResponse.StatusCode}";
                    respuesta.Datos = null;
                }
            }
            catch (Exception ex)
            {
                respuesta.Resultado = false;
                respuesta.Mensaje = $"Error al registrar abono: {ex.Message}";
                respuesta.Datos = null;
            }
            return respuesta;
        }
    }
}

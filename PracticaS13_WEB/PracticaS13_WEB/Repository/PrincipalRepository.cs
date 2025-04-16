using PracticaS13_WEB.Models;
using PracticaS13_WEB.Repository.Extensiones;
using System.Net;
using System.Text.Json;


namespace PracticaS13_WEB.Repository
{
    public class PrincipalRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, string> _apiEndpoints;

        public PrincipalRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;

            var baseUrl = _configuration.GetValue<string>("Variables:urlWebApi")!;
            _apiEndpoints = new Dictionary<string, string>
            {
                { "ObtenerCompras", $"{baseUrl}Principal/ObtenerCompras" },
                { "ObtenerComprasPendientes", $"{baseUrl}Principal/ObtenerComprasPendientes" },
                { "ObtenerSaldoCompra", $"{baseUrl}Principal/ObtenerSaldo" }
            };
        }

        public async Task<RespuestaModel> ObtenerCompras()
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var url = _apiEndpoints["ObtenerCompras"];
                var response = await httpClient.GetAsync(url);

                return await HttpRespuestaProcesada.Procesar<List<PrincipalModel>>(response);
            }
            catch (Exception ex)
            {
                return new RespuestaModel
                {
                    Resultado = false,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<RespuestaModel> ObtenerComprasPendientes()
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var url = _apiEndpoints["ObtenerComprasPendientes"];

                var response = await httpClient.GetAsync(url);
                return await HttpRespuestaProcesada.Procesar<List<PrincipalModel>>(response); 
            }
            catch (Exception ex)
            {
                return new RespuestaModel
                {
                    Resultado = false,
                    Mensaje = $"Error: {ex.Message}"
                };
            }
        }


        public async Task<RespuestaModel> ObtenerSaldoCompra(long idCompra)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient();
                var url = _apiEndpoints["ObtenerSaldoCompra"] + $"?IdCompra={idCompra}";

                var response = await httpClient.GetAsync(url);
                return await HttpRespuestaProcesada.Procesar<PrincipalModel>(response);
            }
            catch (Exception ex)
            {
                return new RespuestaModel
                {
                    Resultado = false,
                    Mensaje = $"Error al obtener el saldo de la compra: {ex.Message}",
                    Datos = null
                };
            }
        }
    }
}

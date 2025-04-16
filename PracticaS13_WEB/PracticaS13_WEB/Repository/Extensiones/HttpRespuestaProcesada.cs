using PracticaS13_WEB.Models;
using System.Net.Http;
using System.Text.Json;

namespace PracticaS13_WEB.Repository.Extensiones
{
    public static class HttpRespuestaProcesada
    {
        public static async Task<RespuestaModel> Procesar<T>(HttpResponseMessage response)
        {
            var respuesta = new RespuestaModel();

            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var apiRespuesta = JsonSerializer.Deserialize<RespuestaModel>(
                        jsonString,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                    if (apiRespuesta != null && apiRespuesta.Resultado)
                    {
                        var datos = JsonSerializer.Deserialize<T>(
                            apiRespuesta.Datos?.ToString() ?? "",
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );

                        respuesta.Resultado = true;
                        respuesta.Datos = datos;
                        respuesta.Mensaje = apiRespuesta.Mensaje ?? "Operación exitosa.";
                    }
                    else
                    {
                        respuesta.Resultado = false;
                        respuesta.Mensaje = apiRespuesta?.Mensaje ?? "Error desconocido en la API.";
                    }
                }
                else
                {
                    respuesta.Resultado = false;
                    respuesta.Mensaje = $"Error HTTP: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                respuesta.Resultado = false;
                respuesta.Mensaje = $"Excepción: {ex.Message}";
            }

            return respuesta;
        }
    }
}

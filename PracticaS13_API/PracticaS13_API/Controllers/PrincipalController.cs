using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PracticaS13_API.Models;

namespace PracticaS13_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrincipalController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PrincipalController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("ObtenerCompras")]
        public IActionResult ObtenerCompras()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var respuesta = new RespuestaModel();

                var resultado = context.Query<PrincipalModel>("SP_ObtenerCompras");

                if (resultado.Any())
                {
                    respuesta.Resultado = true;
                    respuesta.Datos = resultado;
                }
                else
                {
                    respuesta.Resultado = false;
                    respuesta.Mensaje = "No hay compras registradas";
                }
                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ObtenerComprasPendientes")]
        public IActionResult ObtenerComprasPendientes()
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var respuesta = new RespuestaModel();

                var resultado = context.Query<PrincipalModel>("SP_ObtenerComprasPendientes");

                if (resultado.Any())
                {
                    respuesta.Resultado = true;
                    respuesta.Datos = resultado;
                }
                else
                {
                    respuesta.Resultado = false;
                    respuesta.Mensaje = "No hay compras registradas";
                }
                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ObtenerSaldo")]
        public IActionResult ObtenerSaldoCompra(long IdCompra)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var respuesta = new RespuestaModel();

                var resultado = context.QueryFirstOrDefault<PrincipalModel>("SP_ObtenerSaldoCompra",
                    new { IdCompra});

                if (resultado != null)
                {
                    respuesta.Resultado = true;
                    respuesta.Datos = resultado;
                }
                else
                {
                    respuesta.Resultado = false;
                    respuesta.Mensaje = "No hay una compra con ese id";
                }
                return Ok(respuesta);
            }
        }
    }
}

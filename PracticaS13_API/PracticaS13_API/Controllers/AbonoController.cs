
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PracticaS13_API.Models;

namespace PracticaS13_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbonoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AbonoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("RegistrarAbono")]
        public IActionResult RegistrarAbono(AbonoModel model)
        {
            using (var context = new SqlConnection(_configuration.GetSection("ConnectionStrings:BDConnection").Value))
            {
                var respuesta = new RespuestaModel();

                var parametros = new DynamicParameters(new
                {
                    model.IdCompra,
                    model.Monto
                });
                parametros.Add("@CodigoError", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parametros.Add("@Mensaje", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);

                context.Execute("SP_RegistrarAbono", parametros, commandType: CommandType.StoredProcedure);

                int codigoError = parametros.Get<int>("@CodigoError");
                string mensaje = parametros.Get<string>("@Mensaje");

                if (codigoError == 0 )
                {
                    respuesta.Resultado = true;
                    respuesta.Mensaje = mensaje;
                }
                else
                {
                    respuesta.Resultado = false;
                    respuesta.Mensaje = mensaje;
                }
                return Ok(respuesta);
            }
        }
    }
}

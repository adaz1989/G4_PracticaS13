using Microsoft.AspNetCore.Mvc;
using PracticaS13_WEB.Models;
using PracticaS13_WEB.Repository;

namespace PracticaS13_WEB.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly PrincipalRepository _principalRepository;

        public PrincipalController(PrincipalRepository principalRepository)
        {
            _principalRepository = principalRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var respuesta = await _principalRepository.ObtenerCompras();
            TempData["Mensaje"] = respuesta.Mensaje;
            return View(respuesta.Resultado ? respuesta.Datos : new List<PrincipalModel>());
        }

        [HttpGet]
        public async Task<IActionResult> ComprasPendientes()
        {
            var respuesta = await _principalRepository.ObtenerComprasPendientes();
            TempData["Mensaje"] = respuesta.Mensaje;
            return View(respuesta.Resultado ? respuesta.Datos : new List<PrincipalModel>());
        }

        [HttpGet]
        public async Task<IActionResult> SaldoCompra(long idCompra)
        {
            var respuesta = await _principalRepository.ObtenerSaldoCompra(idCompra);
            TempData["Mensaje"] = respuesta.Mensaje;
            return View(respuesta.Resultado ? respuesta.Datos : null);
        }
    }
}

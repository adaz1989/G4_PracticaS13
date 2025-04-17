using Microsoft.AspNetCore.Mvc;
using PracticaS13_WEB.Models;
using PracticaS13_WEB.Repository;

namespace PracticaS13_WEB.Controllers
{
    public class AbonoController : Controller
    {
        private readonly AbonoRepository _abonoRepository;
        private readonly PrincipalRepository _principalRepository;

        public AbonoController(AbonoRepository abonoRepository, PrincipalRepository principalRepository)
        {
            _abonoRepository = abonoRepository;
            _principalRepository = principalRepository;
        }

        // Acción para registrar un abono vía POST.
        [HttpPost]
        public async Task<IActionResult> Registrar(AbonoModel abono)
        {
            var respuesta = await _abonoRepository.RegistrarAbono(abono);
            TempData["Mensaje"] = respuesta.Mensaje;

            if (!respuesta.Resultado)
            {
                var pendientes = await _principalRepository.ObtenerComprasPendientes();
                ViewBag.Pendientes = pendientes.Resultado && pendientes.Datos != null
                    ? pendientes.Datos as List<PrincipalModel>
                    : new List<PrincipalModel>();
                return View(abono);
            }

            return RedirectToAction("Index", "Principal");
        }


        [HttpGet]
        public async Task<IActionResult> Registrar()
        {
            var respuesta = await _principalRepository.ObtenerComprasPendientes();

            if (!respuesta.Resultado || respuesta.Datos == null)
            {
                TempData["Mensaje"] = "No se pudieron cargar las compras pendientes.";
                ViewBag.Pendientes = new List<PrincipalModel>();
            }
            else
            {
                ViewBag.Pendientes = respuesta.Datos as List<PrincipalModel>;
            }

            return View();
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PracticaS13_WEB.Models;
using PracticaS13_WEB.Repository;

namespace PracticaS13_WEB.Controllers
{
    public class AbonoController : Controller
    {
        private readonly AbonoRepository _abonoRepository;

        public AbonoController(AbonoRepository abonoRepository)
        {
            _abonoRepository = abonoRepository;
        }

        // Acción para registrar un abono vía POST.
        [HttpPost]
        public async Task<IActionResult> Registrar(AbonoModel abono)
        {
            var respuesta = await _abonoRepository.RegistrarAbono(abono);
            TempData["Mensaje"] = respuesta.Mensaje;

            return View(abono);
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}

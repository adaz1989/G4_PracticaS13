using Microsoft.AspNetCore.Mvc;

namespace PracticaS13_WEB.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult CapturarError()
        {
            return View("Error");
        }
    }
}

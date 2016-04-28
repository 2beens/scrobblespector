using Microsoft.AspNet.Mvc;

namespace Scrobblespector.Controllers
{
    public class ScrobblespectorController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace ProfileRegistratrionWithAiUsage.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}

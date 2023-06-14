using Microsoft.AspNetCore.Mvc;

namespace Project10pm.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace WebH
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Archive()
        {
            return View();
        }

    }
}
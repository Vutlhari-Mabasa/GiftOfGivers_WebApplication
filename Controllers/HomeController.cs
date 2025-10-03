using Microsoft.AspNetCore.Mvc;

namespace GiftOfGivers_WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Gift Of Givers";
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class SecondController : Controller
    {
        public IActionResult Index() => View("Common");
    }
}
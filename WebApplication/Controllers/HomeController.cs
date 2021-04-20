using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;

        public HomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public IActionResult List() {
            return View(_dataContext.Products);
        }
        
        public async Task<IActionResult> Index(long id = 1)
        {
            ViewBag.AveragePrice = await _dataContext.Products.AverageAsync(p => p.Price);
            return View(await _dataContext.Products.FindAsync(id));
        }

        public IActionResult Html()
        {
            return View((object)"this is a <h3><i>string</i></h3>");
        }
    }
}
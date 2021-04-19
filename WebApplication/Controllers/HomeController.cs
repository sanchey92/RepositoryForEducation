using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        
        public async Task<IActionResult> Index(long id = 1)
        {
            var product = await _dataContext.Products.FindAsync(id);
            return product?.CategoryId == 1 
                ? View("Watersports", product) 
                : View(product);
        }

        public IActionResult Common()
        {
            return View();
        }
    }
}
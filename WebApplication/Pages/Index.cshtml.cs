using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;

namespace WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DataContext _context;
        public Product Product { get; set; }

        public IndexModel(DataContext ctx)
        {
            _context = ctx;
        }

        public async Task<IActionResult> OnGetAsync(long id = 1)
        {
            Product = await _context.Products.FindAsync(id);
            return Page();
        }
    }
}
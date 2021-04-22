using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Models;

namespace WebApplication.Pages
{
    public class EditorModel : PageModel
    {
        private readonly DataContext _context;
        public Product Product { get; set; }

        public EditorModel(DataContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(long id)
        {
            Product = await _context.Products.FindAsync(id);
        }

        public async Task<IActionResult> OnPostAsync(long id, decimal price)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null) product.Price = price;
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
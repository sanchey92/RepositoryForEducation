using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public SuppliersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("{id}")]
        public async Task<Supplier> GetSupplier(long id)
        {
            var supplier = await _dataContext.Suppliers
                .Include(s => s.Products)
                .FirstAsync(s => s.SupplierId == id);
            foreach (var product in supplier.Products)
            {
                product.Supplier = null;
            }

            return supplier;
        }

        [HttpPatch("{id}")]
        public async Task<Supplier> PatchSupplier(long id, 
            JsonPatchDocument<Supplier> patchDocument)
        {
            var supplier = await _dataContext.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                patchDocument.ApplyTo(supplier);
                await _dataContext.SaveChangesAsync();
            }

            return supplier;
        }
    }
}
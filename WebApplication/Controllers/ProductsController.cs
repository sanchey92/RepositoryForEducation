using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private DataContext _dataContext;

        public ProductsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IAsyncEnumerable<Product> GetProducts()
        {
            return (IAsyncEnumerable<Product>) _dataContext.Products;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await _dataContext.Products.FindAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductBindingTarget target)
        {
            var product = target.ToProduct();
            await _dataContext.Products.AddAsync(product);
            await _dataContext.SaveChangesAsync();
            return Ok(product);

        }

        [HttpPut]
        public async Task UpdateProduct(Product product)
        {
            _dataContext.Update(product);
            await _dataContext.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(long id)
        {
            _dataContext.Products.Remove(new Product {ProductId = id});
            await _dataContext.SaveChangesAsync();
        }

        [HttpGet("redirect")]
        public IActionResult Redirect()
        {
            return RedirectToAction(nameof(GetProduct),new {Id = 1});
        }
    }
}
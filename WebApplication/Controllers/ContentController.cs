using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ContentController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("string")]
        public string GetString() => "This is a string response";

        [HttpGet("object")]
        public async Task<ProductBindingTarget> GetObject()
        {
            var product = await _dataContext.Products.FirstAsync();
            return new ProductBindingTarget()
            {
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId
            };
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Services;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public IActionResult getAllProducts(string search, double? from, double? to, string sortBy, int page = 1)
        {
            try
            {
                var result = _productRepository.getAll(search, from, to, sortBy, page);
                return Ok(result);
            }
            catch
            {
                return BadRequest("cant get product");
            }

        }
    }
}

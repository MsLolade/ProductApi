using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI;
using ProductAPI.Data;

namespace ProductAPI.Controllers
{
    [Authorize]

    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _context.Products.ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex.Message);
                return StatusCode(500, "An error occurred while retrieving data");
            }
        }
        [HttpGet]
        public IActionResult GetProductById(string Id)
        {
            try
            {
                var products = _context.Products.FirstOrDefault(v => v.Id == Id);
                if (products == null)
                {
                    return BadRequest("Product does not exist");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex.Message);
                return StatusCode(500, "An error occurred while retrieving data");
            }
        }
        [HttpPost]
        public IActionResult PostProducts(ProductVM product)
        {
            try
            {
                var products = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Id = Guid.NewGuid().ToString(),
                };
                _context.Products.Add(products);
                _context.SaveChanges();
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex.Message);
                return StatusCode(500, "An error occurred while posting data");
            }
        }
    }
    public class ProductVM
    {
        public string Name { get; set; }
        public string Price { get; set; }
    }
}

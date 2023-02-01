namespace MarketNetwork.API.Controllers
{
    using MarketNetwork.API.Context;
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    using MarketNetwork.API.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        private readonly MarketNetworkContext db;

        public ProductController(ILogger<ProductController> logger, IProductService productService, MarketNetworkContext db)
        {
            _logger = logger;
            _productService = productService;
            this.db = db;
        }

        // Get all of the items from Database
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            return await this.db.Product.ToListAsync();
        }

        // Get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            Product? product = await db.Product.FirstOrDefaultAsync(x => x.Id == id);

            // Check if this item exists in Database
            if (product == null)
                return NotFound();

            return new ObjectResult(product);
        }

        // Add new item to Database
        [HttpPost("[action]")]
        public async Task<ActionResult<Product>> Create([FromQuery] CreateProductModel createProductModel)
        {
            // Create Entity using input data
            Product product = new()
            {
                ProductName = createProductModel.ProductName,
                ProductCompany = createProductModel.ProductCompany,
                Quantity = createProductModel.Quantity,
                Price = createProductModel.Price,
                Weight = createProductModel.Weight,
                Description = createProductModel.Description,
                ExpirationDate = createProductModel.ExpirationDate
            };

            // Check if variables entered are valid
            if (_productService.GetCheckedProduct(product) == null)
                return this.BadRequest();

            db.Product.Add(product);
            await this.db.SaveChangesAsync();

            return Ok(product);
        }

        // Update item in Database
        [HttpPut("[action]")]
        public async Task<ActionResult<Product>> Update([FromQuery] Product product)
        {
            // Check if variables entered are valid
            if (_productService.GetCheckedProduct(product) == null)
                return this.BadRequest();

            // Check if item exists in Database
            if (!db.Product.Any(x => x.Id == product.Id))
                return this.NotFound();

            db.Product.Update(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        // Delete item from Database using Id
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            Product? product = await db.Product.FirstOrDefaultAsync(x => x.Id == id);

            // Check if item exists in Database
            if (id < 0 || product == null)
                return NotFound();

            db.Product.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }
    }
}
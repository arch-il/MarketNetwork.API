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
    public class ReceiptController : ControllerBase
    {
        private readonly ILogger<ReceiptController> _logger;
        private readonly IReceiptService _receiptService;
        private readonly MarketNetworkContext db;

        public ReceiptController(ILogger<ReceiptController> logger, IReceiptService receiptService, MarketNetworkContext db)
        {
            _logger = logger;
            _receiptService = receiptService;
            this.db = db;
        }

        // Get all of the items from Database
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Receipt>>> GetAll()
        {
            return await this.db.Receipt.Include(x => x.Market).Include(x => x.Client).Include(x => x.Products).ToListAsync();
        }

        // Get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Receipt>> GetById(int id)
        {
            Receipt? receipt = await db.Receipt.Include(x => x.Market).Include(x => x.Client).Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);

            // Check if this item exists in Database
            if (receipt == null)
                return NotFound();

            return new ObjectResult(receipt);
        }

        // Add new item to Database
        [HttpPost("[action]")]
        public async Task<ActionResult<Receipt>> Create([FromQuery] CreateReceiptModel createReceiptModel)
        {
            // Get market and client from Database (No need to check them for null here, Service does it for us)
            Market ?market = await db.Market.FirstOrDefaultAsync(x => x.Id == createReceiptModel.MarketId);
            Client ?client = await db.Client.FirstOrDefaultAsync(x => x.Id == createReceiptModel.ClientId);

            // Create Entity using input data
            Receipt receipt = new()
            {
                Market = market,
                Client = client,
                Products = createReceiptModel.Products,
                TotalPrice = createReceiptModel.TotalPrice,
                PurchaseDate = createReceiptModel.PurchaseDate
            };

            // Check if variables entered are valid
            if (_receiptService.GetCheckedReceipt(receipt) == null)
                return this.BadRequest();

            db.Receipt.Add(receipt);
            await this.db.SaveChangesAsync();

            return Ok(receipt);
        }

        // Update item in Database
        [HttpPut("[action]")]
        public async Task<ActionResult<Receipt>> Update([FromQuery] Receipt receipt)
        {
            // Check if variables entered are valid
            if (_receiptService.GetCheckedReceipt(receipt) == null)
                return this.BadRequest();

            // Check if item exists in Database
            if (!db.Receipt.Any(x => x.Id == receipt.Id))
                return this.NotFound();

            db.Receipt.Update(receipt);
            await db.SaveChangesAsync();

            return Ok(receipt);
        }

        // Delete item from Database using Id
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<Receipt>> Delete(int id)
        {
            Receipt? receipt = await db.Receipt.FirstOrDefaultAsync(x => x.Id == id);

            // Check if item exists in Database
            if (id < 0 || receipt == null)
                return NotFound();

            db.Receipt.Remove(receipt);
            await db.SaveChangesAsync();

            return Ok(receipt);
        }
    }
}
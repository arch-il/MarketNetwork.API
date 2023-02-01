namespace MarketNetwork.API.Controllers
{
    using MarketNetwork.API.Context;
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    using MarketNetwork.API.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics.Metrics;

    [ApiController]
    [Route("[controller]")]
    public class MarketController : ControllerBase
    {
        private readonly ILogger<MarketController> _logger;
        private readonly IMarketService _marketService;
        private readonly MarketNetworkContext db;

        public MarketController(ILogger<MarketController> logger, IMarketService marketService, MarketNetworkContext db)
        {
            _logger = logger;
            _marketService = marketService;
            this.db = db;
        }

        // Get all of the items from Database
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Market>>> GetAll()
        {
            return await this.db.Market.Include(x => x.Owner).Include(x => x.OwnerCompany).ToListAsync();
        }

        // Get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Market>> GetById(int id)
        {
            Market ?market = await this.db.Market.Include(x => x.Owner).Include(x => x.OwnerCompany).FirstOrDefaultAsync(x => x.Id == id);

            // Check if this item exists in Database
            if (market == null)
                return NotFound();

            return new ObjectResult(market);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<List<Product>>> GetProductsById(int id)
        {
            Market? market = await db.Market.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);

            // Check if item exists
            if (market == null)
                return NotFound();

            // Update product prices
            for (int index = 0; index < market.Products.Count; index++)
            {
                // Calculate how many days left until ExpirationDate
                double daysLeft = (market.Products[index].ExpirationDate.Date - DateTime.Now.Date).TotalDays;
                
                // Check if Product is out of stock
                if (daysLeft <= 0 || market.Products[index].Quantity == 0)
                    market.Products[index].ProductName = "[Out Of Stock] " + market.Products[index].ProductName;

                // Calculate Sale
                else if (daysLeft <= 7 && daysLeft >= 6)
                {
                    market.Products[index].ProductName = "[Sale 10% off] " + market.Products[index].ProductName;
                    market.Products[index].Price *= 0.9M; // 10% off
                }
                else if (daysLeft <= 5 && daysLeft >= 4)
                {
                    market.Products[index].ProductName = "[Sale 20% off] " + market.Products[index].ProductName;
                    market.Products[index].Price *= 0.8M; // 20% off
                }
                else if (daysLeft <= 3 && daysLeft >= 2)
                {
                    market.Products[index].ProductName = "[Sale 30% off] " + market.Products[index].ProductName;
                    market.Products[index].Price *= 0.7M; // 30% off
                }
                else if (daysLeft <= 1 && daysLeft > 0)
                {
                    market.Products[index].ProductName = "[Sale 50% off] " + market.Products[index].ProductName;
                    market.Products[index].Price *= 0.5M; // 50% off
                }
            }
            // No need to save changes to database. Price is recalculated every time

            return new ObjectResult(market.Products);
        }

        // Add new item to Database
        [HttpPost("[action]")]
        public async Task<ActionResult<Market>> Create([FromQuery] CreateMarketModel createMarketModel)
        {
            // Get ownerCompany and owner from Database (No need to check them for null here, Service does it for us)
            MarketCompany ?ownerCompany = await db.MarketCompany.FirstOrDefaultAsync(x => x.Id == createMarketModel.OwnerCompanyId);
            Owner ?owner = await db.Owner.FirstOrDefaultAsync(x => x.Id == createMarketModel.OwnerId);

            // Create Entity using input data
            Market market = new()
            {
                NetWorth = createMarketModel.NetWorth,
                Location = createMarketModel.Location,
                OwnerCompany = ownerCompany,
                Owner = owner
            };

            // Check if variables entered are valid
            if (_marketService.GetCheckedMarket(market) == null)
                return this.BadRequest();

            db.Market.Add(market);
            await this.db.SaveChangesAsync();

            return Ok(market);
        }

        // Update item in Database
        [HttpPut("[action]")]
        public async Task<ActionResult<Market>> Update([FromQuery] Market market)
        {
            // Check if variables entered are valid
            if (_marketService.GetCheckedMarket(market) == null)
                return this.BadRequest();

            // Check if item exists in Database
            if (!db.Market.Any(x => x.Id == market.Id))
                return this.NotFound();

            db.Market.Update(market);
            await db.SaveChangesAsync();

            return Ok(market);
        }

        // Add product from Market protected by OwnerId
        [HttpPut("[action]")]
        public async Task<ActionResult<Market>> AddProduct([FromQuery] ModifyProductToMarketModel modifyProductToMarketModel)
        {
            Market? market = await db.Market.Include(x => x.Products).Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == modifyProductToMarketModel.MarketId);
            Product? product = await db.Product.FirstOrDefaultAsync(x => x.Id == modifyProductToMarketModel.ProductId);

            // Check if items exist and owner owns this company
            if (market == null || product == null || market.Owner.Id != modifyProductToMarketModel.OwnerId)
                return BadRequest();

            market.Products.Add(product);

            db.Market.Update(market);
            await db.SaveChangesAsync();

            return Ok(market);
        }

        // Remove product from Market protected by OwnerId
        [HttpPut("[action]")]
        public async Task<ActionResult<Market>> RemoveProduct([FromQuery] ModifyProductToMarketModel modifyProductToMarketModel)
        {
            Market? market = await db.Market.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == modifyProductToMarketModel.MarketId);
            Product? product = await db.Product.FirstOrDefaultAsync(x => x.Id == modifyProductToMarketModel.ProductId);

            // Check if items exist and owner owns this company
            if (market == null || product == null || market.Owner.Id != modifyProductToMarketModel.OwnerId)
                return BadRequest();

            market.Products.Remove(product);

            db.Market.Update(market);
            await db.SaveChangesAsync();

            return Ok(market);
        }

        // Buy List of Products and Quantites by Client
        [HttpPut("[action]")]
        public async Task<ActionResult<Receipt>> BuyProducts([FromQuery] BuyProductsModel buyProductsModel)
        {
            Client? client = await db.Client.FirstOrDefaultAsync(x => x.Id == buyProductsModel.ClintId);
            Market? market = await db.Market.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == buyProductsModel.MarketId);
            List<Product> products = await db.Product.Where(x => buyProductsModel.ProductIds.Contains(x.Id)).ToListAsync();

            // Check if items exist
            if (client == null || market == null || products.Count == 0)
                return BadRequest();

            // Check if client is blacklisted   
            if (db.BlackList.Where(x => x.ClientId == client.Id).ToList().Count > 0)
                return BadRequest();

            // Check if Market has enough products
            for (int index = 0; index < products.Count; index++)
            {
                if (!market.Products.Contains(products[index]) || 
                    products[index].Quantity < buyProductsModel.ProductQuantities[index] ||
                    buyProductsModel.ProductQuantities[index] < 1)
                {
                    return BadRequest();
                }
            }

            // Update products quantites in market
            for (int index = 0; index < products.Count; index++)
                products[index].Quantity -= buyProductsModel.ProductQuantities[index];
            db.Product.UpdateRange(products);

            // Calculate total price of products
            decimal totalPrice = 0;
            List<Product> boughtProducts = new();
            for (int index = 0; index < products.Count; index++)
            {
                // Calculate how many days left until ExpirationDate
                double daysLeft = (products[index].ExpirationDate.Date - DateTime.Now.Date).TotalDays;

                // Check if Product is out of stock
                if (daysLeft <= 0 || products[index].Quantity == 0)
                {
                    products[index].ProductName = "[Out Of Stock] " + products[index].ProductName;
                    return BadRequest(); // Can't buy expired products
                }

                // Calculate Sale
                else if (daysLeft <= 7 && daysLeft >= 6)
                {
                    products[index].ProductName = "[Sale 10% off] " + products[index].ProductName;
                    products[index].Price *= 0.9M; // 10% off
                }
                else if (daysLeft <= 5 && daysLeft >= 4)
                {
                    products[index].ProductName = "[Sale 20% off] " + products[index].ProductName;
                    products[index].Price *= 0.8M; // 20% off
                }
                else if (daysLeft <= 3 && daysLeft >= 2)
                {
                    products[index].ProductName = "[Sale 30% off] " + products[index].ProductName;
                    products[index].Price *= 0.7M; // 30% off
                }
                else if (daysLeft <= 1 && daysLeft > 0)
                {
                    products[index].ProductName = "[Sale 50% off] " + products[index].ProductName;
                    products[index].Price *= 0.5M; // 50% off
                }
                // No need to save changes to database. Price is recalculated every time

                boughtProducts.Add(new()
                {
                    ProductName = products[index].ProductName,
                    ProductCompany = products[index].ProductCompany,
                    Quantity = buyProductsModel.ProductQuantities[index], // Set Quantity here
                    Price = products[index].Price,
                    Weight = products[index].Weight,
                    Description = products[index].Description,
                    ExpirationDate = products[index].ExpirationDate
                });

                totalPrice += boughtProducts[index].Price * boughtProducts[index].Quantity;
            }
            totalPrice *= 1.18M; // Add 18 percent tax fee
            // Check if client has enough money and update client money in database
            if (client.MoneyInWallet < totalPrice)
                return BadRequest();
            client.MoneyInWallet -= totalPrice;
            db.Client.Update(client);
            
            Receipt receipt = new()
            {
                Market = market,
                Client = client,
                Products = boughtProducts,
                TotalPrice = totalPrice,
                PurchaseDate = DateTime.Now
            };
            await db.Receipt.AddAsync(receipt);

            await db.SaveChangesAsync();
            return receipt;
        }

        // Return sold products using receiptId and clientId
        [HttpPut("[action]")]
        public async Task<ActionResult<Receipt>> ReturnProducts([FromQuery] int receiptId, int clientId)
        {
            Receipt? receipt = await db.Receipt.Include(x => x.Client).Include(x => x.Market).Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == receiptId);
            Client? client = await db.Client.FirstOrDefaultAsync(x => x.Id == clientId);

            // Check if receipt and client exist
            if (receipt == null || client == null)
                return BadRequest();

            // Check if client is blacklisted   
            if (db.BlackList.Where(x => x.ClientId == client.Id).ToList().Count > 0)
                return BadRequest();

            // Check if client is trying to steal
            if (receipt.Client.Id != client.Id)
            {
                client.Warnings++;
                // Check if this was clients last warning
                if (client.Warnings >= 3)
                    await db.BlackList.AddAsync(new BlackList() { ClientId = client.Id });
                db.Client.Update(client);
                await db.SaveChangesAsync();
                return BadRequest();
            }

            // Find and add to product Quantity in market
            Market market = await db.Market.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == receipt.Market.Id);
            foreach (var product in receipt.Products)
            {
                for (int index = 0; index < market.Products.Count; index++)
                {
                    if (product.ProductName == market.Products[index].ProductName &&
                        product.ProductCompany == market.Products[index].ProductCompany &&
                        product.Price == market.Products[index].Price &&
                        product.Weight == market.Products[index].Weight &&
                        product.Description == market.Products[index].Description &&
                        product.ExpirationDate == market.Products[index].ExpirationDate)
                    {
                        market.Products[index].Quantity += product.Quantity;
                        break;
                    }
                }
            }
            
            // Return money to client
            client.MoneyInWallet += receipt.TotalPrice;

            // Update data in database
            db.Client.Update(client);
            db.Market.Update(market);
            db.Product.UpdateRange(market.Products);
            db.Receipt.Remove(receipt);
            await db.SaveChangesAsync();

            return receipt;
        }

        // Delete item from Database using Id
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<Market>> Delete(int id)
        {
            Market? market = await db.Market.FirstOrDefaultAsync(x => x.Id == id);

            // Check if item exists in Database
            if (id < 0 || market == null)
                return NotFound();

            db.Market.Remove(market);
            await db.SaveChangesAsync();

            return Ok(market);
        }
    }
}
namespace MarketNetwork.API.Controllers
{
    using MarketNetwork.API.Context;
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    using MarketNetwork.API.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System.Runtime.InteropServices;

    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientService _clientService;
        private readonly MarketNetworkContext db;

        public ClientController(ILogger<ClientController> logger, IClientService clientService, MarketNetworkContext db)
        {
            _logger = logger;
            _clientService = clientService;
            this.db = db;
        }

        // Get all of the items from Database
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Client>>> GetAll()
        {
            return await this.db.Client.ToListAsync();
        }

        // Get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Client>> GetById(int id)
        {
            var client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);

            // Check if this item exists in Database
            if (client == null)
                return NotFound();

            return new ObjectResult(client);
        }

        // Add new item to Database
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> Create([FromQuery] CreateClientModel createClientModel)
        {
            // Create Entity using input data
            Client client = new()
            {
                FullName = createClientModel.FullName,
                Age = createClientModel.Age,
                MoneyInWallet = createClientModel.MoneyInWallet,
                Warnings = 0
            };


            // Check if variables entered are valid
            if (_clientService.GetCheckedClient(client) == null)
                return this.BadRequest();

            db.Client.Add(client);
            await this.db.SaveChangesAsync();

            return Ok();
        }

        // Update item in Database
        [HttpPut("[action]")]
        public async Task<ActionResult<Client>> Update([FromQuery] Client client)
        {
            // Check if variables entered are valid
            if (_clientService.GetCheckedClient(client) == null)
                return this.BadRequest();

            // Check if item exists in Database
            if (!db.Client.Any(x => x.Id == client.Id))
                return this.NotFound();

            db.Client.Update(client);
            await db.SaveChangesAsync();

            return Ok(client);
        }

        // Delete item from Database using Id
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<Client>> Delete(int id)
        {
            Client? client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);

            // Check if item exists in Database
            if (id < 0 || client == null)
                return NotFound();

            db.Client.Remove(client);
            await db.SaveChangesAsync();

            return Ok(client);
        }

        // Enter how much money you earned
        [HttpPut("[action]/{amount}")]
        public async Task<ActionResult<Client>> EarnMoney(int id, int amount)
        {
            Client? client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);
            
            // Check if client exists
            if (client == null)
                return this.BadRequest();

            client.MoneyInWallet += amount;

            db.Client.Update(client);
            await db.SaveChangesAsync();

            return Ok(client);
        }

        // Enter how much money you lost
        [HttpPut("[action]/{amount}")]
        public async Task<ActionResult<Client>> LoseMoney(int id, int amount)
        {
            Client? client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);
            
            // Check if client exists
            if (client == null)
                return this.BadRequest();

            // Check if amount lost is valid
            if (client.MoneyInWallet - amount < 0)
                return this.BadRequest();
            
            client.MoneyInWallet += amount;

            db.Client.Update(client);
            await db.SaveChangesAsync();

            return Ok(client);
        }

        // Try your luck in lottery. 1 in 1,000,000 chance of winning million dollars. price is 25
        [HttpPut("[action]/{id}")]
        public async Task<ActionResult<Client>> BuyLotteryTicket(int id)
        {
            Client? client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);

            // Check if client exists
            if (client == null)
                return this.BadRequest();

            // Check if client has enough money for lottery ticket
            if (client.MoneyInWallet - 25 < 0)
                return this.BadRequest();
            
            // make client pay for lottery ticket
            client.MoneyInWallet -= 25;

            // roll 1 in 1,000,000 chance of winning million dollars. lucky number is 12345
            Random rand = new Random();
            if (rand.Next(0, 100000) == 12345)
                client.MoneyInWallet += 1000000;

            db.Client.Update(client);
            await db.SaveChangesAsync();

            return Ok(client);
        }
    }
}
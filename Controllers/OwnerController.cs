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
    public class OwnerController : ControllerBase
    {
        private readonly ILogger<OwnerController> _logger;
        private readonly IOwnerService _ownerService;
        private readonly MarketNetworkContext db;

        public OwnerController(ILogger<OwnerController> logger, IOwnerService ownerService, MarketNetworkContext db)
        {
            _logger = logger;
            _ownerService = ownerService;
            this.db = db;
        }

        // Get all of the items from Database
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Owner>>> GetAll()
        {
            return await this.db.Owner.ToListAsync();
        }

        // Get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Owner>> GetById(int id)
        {
            Owner? owner = await db.Owner.FirstOrDefaultAsync(x => x.Id == id);

            // Check if this item exists in Database
            if (owner == null)
                return NotFound();

            return new ObjectResult(owner);
        }

        // Add new item to Database
        [HttpPost("[action]")]
        public async Task<ActionResult<Owner>> Create([FromQuery] CreateOwnerModel createOwnerModel)
        {
            // Create Entity using input data
            Owner owner = new()
            {
                FullName = createOwnerModel.FullName,
                Age = createOwnerModel.Age,
                NetWorth = createOwnerModel.NetWorth
            };

            // Check if variables entered are valid
            if (_ownerService.GetCheckedOwner(owner) == null)
                return this.BadRequest();

            db.Owner.Add(owner);
            await this.db.SaveChangesAsync();

            return Ok(owner);
        }

        // Update item in Database
        [HttpPut("[action]")]
        public async Task<ActionResult<Owner>> Update([FromQuery] Owner owner)
        {
            // Check if variables entered are valid
            if (_ownerService.GetCheckedOwner(owner) == null)
                return this.BadRequest();

            // Check if item exists in Database
            if (!db.Owner.Any(x => x.Id == owner.Id))
                return this.NotFound();

            db.Owner.Update(owner);
            await db.SaveChangesAsync();

            return Ok(owner);
        }

        // Delete item from Database using Id
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<Owner>> Delete(int id)
        {
            Owner? owner = await db.Owner.FirstOrDefaultAsync(x => x.Id == id);

            // Check if item exists in Database
            if (id < 0 || owner == null)
                return NotFound();

            db.Owner.Remove(owner);
            await db.SaveChangesAsync();

            return Ok(owner);
        }
    }
}
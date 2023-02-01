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
    public class MarketCompanyController : ControllerBase
    {
        private readonly ILogger<MarketCompanyController> _logger;
        private readonly IMarketCompanyService _marketCompanyService;
        private readonly MarketNetworkContext db;

        public MarketCompanyController(ILogger<MarketCompanyController> logger, IMarketCompanyService marketCompanyService, MarketNetworkContext db)
        {
            _logger = logger;
            _marketCompanyService = marketCompanyService;
            this.db = db;
        }

        // Get all of the items from Database
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<MarketCompany>>> GetAll()
        {
            return await this.db.MarketCompany.ToListAsync();
        }

        // Get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<MarketCompany>> GetById(int id)
        {
            MarketCompany? marketCompany = await db.MarketCompany.FirstOrDefaultAsync(x => x.Id == id);

            // Check if this item exists in Database
            if (marketCompany == null)
                return NotFound();

            return new ObjectResult(marketCompany);
        }

        // Add new item to Database
        [HttpPost("[action]")]
        public async Task<ActionResult<MarketCompany>> Create([FromQuery] CreateMarketCompanyModel createMarketCompanyModel)
        {
            // Create Entity using input data
            MarketCompany marketCompany = new()
            {
                CompanyName = createMarketCompanyModel.CompanyName,
                NetWorth = createMarketCompanyModel.NetWorth
            };

            // Check if variables entered are valid
            if (_marketCompanyService.GetCheckedMarketCompany(marketCompany) == null)
                return this.BadRequest();

            db.MarketCompany.Add(marketCompany);
            await this.db.SaveChangesAsync();

            return Ok(marketCompany);
        }

        // Update item in Database
        [HttpPut("[action]")]
        public async Task<ActionResult<MarketCompany>> Update([FromQuery] MarketCompany marketCompany)
        {
            // Check if variables entered are valid
            if (_marketCompanyService.GetCheckedMarketCompany(marketCompany) == null)
                return this.BadRequest();

            // Check if item exists in Database
            if (!db.MarketCompany.Any(x => x.Id == marketCompany.Id))
                return this.NotFound();

            db.MarketCompany.Update(marketCompany);
            await db.SaveChangesAsync();

            return Ok(marketCompany);
        }

        // Delete item from Database using Id
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<MarketCompany>> Delete(int id)
        {
            MarketCompany? marketCompany = await db.MarketCompany.FirstOrDefaultAsync(x => x.Id == id);

            // Check if item exists in Database
            if (id < 0 || marketCompany == null)
                return NotFound();

            db.MarketCompany.Remove(marketCompany);
            await db.SaveChangesAsync();

            return Ok(marketCompany);
        }
    }
}
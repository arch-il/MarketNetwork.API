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
    public class BlackListController : ControllerBase
    {
        private readonly ILogger<BlackListController> _logger;
        private readonly IBlackListService _blackListService;
        private readonly MarketNetworkContext db;

        public BlackListController(ILogger<BlackListController> logger, IBlackListService blackListService, MarketNetworkContext db)
        {
            _logger = logger;
            _blackListService = blackListService;
            this.db = db;
        }

        // Get all of the items from Database
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<BlackList>>> GetAll()
        {
            return await this.db.BlackList.ToListAsync();
        }

        // Get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<BlackList>> GetById(int id)
        {
            BlackList? blackList = await db.BlackList.FirstOrDefaultAsync(x => x.Id == id);

            // Check if this item exists in Database
            if (blackList == null)
                return NotFound();

            return new ObjectResult(blackList);
        }

        // Add new item to Database
        [HttpPost("[action]")]
        public async Task<ActionResult<BlackList>> Create([FromQuery] CreateBlackListModel createBlackListModel)
        {
            // Create Entity using input data
            BlackList blackList = new()
            {
                ClientId = createBlackListModel.ClientId
            };

            // Check if variables entered are valid
            if (_blackListService.GetCheckedBlackList(blackList) == null)
                return this.BadRequest();

            db.BlackList.Add(blackList);
            await this.db.SaveChangesAsync();

            return Ok(blackList);
        }

        // Update item in Database
        [HttpPut("[action]")]
        public async Task<ActionResult<BlackList>> Update([FromQuery] BlackList blackList)
        {
            // Check if variables entered are valid
            if (_blackListService.GetCheckedBlackList(blackList) == null)
                return this.BadRequest();

            // Check if item exists in Database
            if (!db.BlackList.Any(x => x.Id == blackList.Id))
                return this.NotFound();

            db.BlackList.Update(blackList);
            await db.SaveChangesAsync();

            return Ok(blackList);
        }

        // Delete item from Database using Id
        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<BlackList>> Delete(int id)
        {
            BlackList? blackList = await db.BlackList.FirstOrDefaultAsync(x => x.Id == id);

            // Check if item exists in Database
            if (id < 0 || blackList == null)
                return NotFound();

            db.BlackList.Remove(blackList);
            await db.SaveChangesAsync();

            return Ok(blackList);
        }
    }
}
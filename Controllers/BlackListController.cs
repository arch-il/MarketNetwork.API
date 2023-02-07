namespace MarketNetwork.API.Controllers
{
    using MarketNetwork.API.Context;
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    using MarketNetwork.API.Models;

    using System.Net;
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
        public async Task<CustomResponseModel<IEnumerable<ViewBlackListModel>>> GetAll()
        {
            try
            {
                // get list of all blacklists
                var blackLists = await this.db.BlackList.ToListAsync();

                // create empty list of blacklist view models
                var blackListViewModels = new List<ViewBlackListModel>();

                // fill up blacklist view models with data from database
                foreach (var blackList in blackLists)
                {
                    blackListViewModels.Add(new()
                    {
                        Id = blackList.Id,
                        ClientId = blackList.ClientId
                    });
                }

                // return response model
                return new CustomResponseModel<IEnumerable<ViewBlackListModel>>()
                {
                    StatusCode = 200,
                    Result = blackListViewModels
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<IEnumerable<ViewBlackListModel>>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }

        // Get single item from Database using Id
        [HttpGet("[action]/{id}")]
        public async Task<CustomResponseModel<ViewClientModel>> GetById(int id)
        {
            try
            {
                // find blackList in Database
                var blackList = await db.BlackList.FirstOrDefaultAsync(x => x.Id == id);

                // Check if this item exists in Database
                if (blackList == null)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "BlackList not found in Database"
                    };

                // find client in Database
                var client = await db.Client.FirstOrDefaultAsync(x => x.Id == blackList.ClientId);

                // Check if this item exists in Database
                if (client == null)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // map client entity to model
                var blackListViewModel = new ViewClientModel()
                {
                    Id = client.Id,
                    Age = client.Age,
                    FullName = client.FullName,
                    MoneyInWallet = client.MoneyInWallet,
                    Warnings = client.Warnings
                };

                // return response model
                return new CustomResponseModel<ViewClientModel>()
                {
                    StatusCode = 200,
                    Result = blackListViewModel
                };
            }
            catch(Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<ViewClientModel>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }

        // Add new item to Database
        [HttpPost("[action]")]
        public async Task<CustomResponseModel<bool>> Create([FromQuery] CreateBlackListModel createBlackListModel)
        {
            try
            {
                // map create model to entity model
                var blackList = new BlackList()
                {
                    ClientId = createBlackListModel.ClientId
                };

                // Check if variables entered are valid
                if (_blackListService.GetCheckedBlackList(blackList) == null)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database",
                        Result = false
                    };

                // write to database
                db.BlackList.Add(blackList);
                await this.db.SaveChangesAsync();


                // return success code
                return new CustomResponseModel<bool>()
                {
                    StatusCode = 200,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<bool>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }

        // Update item in Database
        [HttpPut("[action]")]
        public async Task<CustomResponseModel<ViewClientModel>> Update([FromQuery] UpdateBlackListModel updateBlackListModel)
        {
            try
            {
                // get client from database
                var client = await db.Client.FirstOrDefaultAsync(x => x.Id == updateBlackListModel.ClientId);

                // check if client exists in Database
                if (client == null)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "BlackList not found in Database"
                    };

                // map to entity
                var blackList = new BlackList()
                {
                    Id = updateBlackListModel.Id,
                    ClientId = updateBlackListModel.ClientId
                };

                // update in database
                db.BlackList.Update(blackList);
                await db.SaveChangesAsync();

                // map to view model
                var viewClientModel = new ViewClientModel()
                {
                    Id = client.Id,
                    FullName = client.FullName,
                    Age = client.Age,
                    MoneyInWallet = client.MoneyInWallet,
                    Warnings = client.Warnings
                };

                return new CustomResponseModel<ViewClientModel>()
                {
                    StatusCode = 200,
                    Result = viewClientModel
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<ViewClientModel>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }

        // Delete item from Database using Id
        [HttpDelete("[action]/{id}")]
        public async Task<CustomResponseModel<ViewClientModel>> Delete(int id)
        {
            try
            {
                // get item from database
                var blackList = await db.BlackList.FirstOrDefaultAsync(x => x.Id == id);

                // Check if item exists in Database
                if (id < 0 || blackList == null)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "BlackList not found in Database"
                    };

                // delete from database
                db.BlackList.Remove(blackList);
                await db.SaveChangesAsync();

                // get client from database
                var client = await db.Client.FirstOrDefaultAsync(x => x.Id == blackList.ClientId);

                // map entity to view model
                var viewClientModel = new ViewClientModel()
                {
                    Id = client.Id,
                    FullName = client.FullName,
                    Age = client.Age,
                    MoneyInWallet = client.MoneyInWallet,
                    Warnings = client.Warnings
                };

                // return status code 200
                return new CustomResponseModel<ViewClientModel>()
                {
                    StatusCode = 200,
                    Result = viewClientModel
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<ViewClientModel>()
                {
                    StatusCode = 500,
                    ErrorMessage = "Something went wrong, please contact support"
                };
            }
        }
    }
}
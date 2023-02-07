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
        public async Task<CustomResponseModel<IEnumerable<ViewClientModel>>> GetAll()
        {
            try
            {
                // get list of all blacklists
                var clients = await this.db.Client.ToListAsync();

                // create empty list of blacklist view models
                var clientViewModels = new List<ViewClientModel>();

                // fill up blacklist view models with data from database
                foreach (var client in clients)
                {
                    clientViewModels.Add(new()
                    {
                        Id = client.Id,
                        FullName = client.FullName,
                        Age = client.Age,
                        MoneyInWallet = client.MoneyInWallet,
                        Warnings = client.Warnings
                    });
                }

                // return response model
                return new CustomResponseModel<IEnumerable<ViewClientModel>>()
                {
                    StatusCode = 200,
                    Result = clientViewModels
                };
            }
            catch (Exception ex)
            {
                // log problem
                _logger.LogCritical(ex.Message);

                // return status code 500
                return new CustomResponseModel<IEnumerable<ViewClientModel>>()
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
                // find client in Database
                var client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);

                // Check if this item exists in Database
                if (client == null)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // map client entity to model
                var clientViewModel = new ViewClientModel()
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
                    Result = clientViewModel
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

        // Add new item to Database
        [HttpPost("[action]")]
        public async Task<CustomResponseModel<bool>> Create([FromQuery] CreateClientModel createClientModel)
        {
            try
            {
                // map create model to entity model
                Client client = new()
                {
                    FullName = createClientModel.FullName,
                    Age = createClientModel.Age,
                    MoneyInWallet = createClientModel.MoneyInWallet,
                    Warnings = 0
                };

                // Check if variables entered are valid
                if (_clientService.GetCheckedClient(client) == null)
                    return new CustomResponseModel<bool>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database",
                        Result = false
                    };

                // write to database
                db.Client.Add(client);
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
        public async Task<CustomResponseModel<ViewClientModel>> Update([FromQuery] UpdateClientModel updateClientModel)
        {
                try
                {
                    // get client from database
                    var client = await db.Client.FirstOrDefaultAsync(x => x.Id == updateClientModel.Id);

                    // check if client exists in Database
                    if (client == null)
                        return new CustomResponseModel<ViewClientModel>()
                        {
                            StatusCode = 400,
                            ErrorMessage = "Client not found in Database"
                        };

                    // map to entity
                    client = new Client()
                    {
                        Id = updateClientModel.Id,
                        FullName = updateClientModel.FullName,
                        Age = updateClientModel.Age,
                        MoneyInWallet = updateClientModel.MoneyInWallet,
                        Warnings = updateClientModel.Warnings
                    };

                    // update in database
                    db.Client.Update(client);
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

                    // return success code
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
                var client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);

                // Check if item exists in Database
                if (id < 0 || client == null)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // delete from database
                db.Client.Remove(client);
                await db.SaveChangesAsync();

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

        // Enter how much money you earned
        [HttpPut("[action]/{amount}")]
        public async Task<CustomResponseModel<ViewClientModel>> EarnMoney(int id, int amount)
        {
            try
            {
                // find client in Database
                var client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);

                // Check if client exists
                if (client == null)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // increment by amount
                client.MoneyInWallet += amount;

                // update in database
                db.Client.Update(client);
                await db.SaveChangesAsync();

                // map base entity to view model
                var viewClientModel = new ViewClientModel()
                {
                    Id = client.Id,
                    FullName = client.FullName,
                    Age = client.Age,
                    MoneyInWallet = client.MoneyInWallet,
                    Warnings = client.Warnings
                };

                // return success code
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

        // Enter how much money you lost
        [HttpPut("[action]/{amount}")]
        public async Task<CustomResponseModel<ViewClientModel>> LoseMoney(int id, int amount)
        {
            try
            {
                // find client in Database
                var client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);

                // Check if client exists
                if (client == null)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // check if client has enough money
                if (client.MoneyInWallet - amount < 0)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client does not have enough money"
                    };

                // decrement by amount
                client.MoneyInWallet -= amount;

                // update in database
                db.Client.Update(client);
                await db.SaveChangesAsync();

                // map base entity to view model
                var viewClientModel = new ViewClientModel()
                {
                    Id = client.Id,
                    FullName = client.FullName,
                    Age = client.Age,
                    MoneyInWallet = client.MoneyInWallet,
                    Warnings = client.Warnings
                };

                // return success code
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

        // Try your luck in lottery. 1 in 1,000,000 chance of winning million dollars. price is 25
        [HttpPut("[action]/{id}")]
        public async Task<CustomResponseModel<ViewClientModel>> BuyLotteryTicket(int id)
        {
            try
            {
                // find client in database
                var client = await db.Client.FirstOrDefaultAsync(x => x.Id == id);

                // Check if client exists
                if (client == null)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client not found in Database"
                    };

                // Check if client has enough money for lottery ticket
                if (client.MoneyInWallet - 25 < 0)
                    return new CustomResponseModel<ViewClientModel>()
                    {
                        StatusCode = 400,
                        ErrorMessage = "Client does not have enough money"
                    };

                // make client pay for lottery ticket
                client.MoneyInWallet -= 25;

                // roll 1 in 1,000,000 chance of winning million dollars. lucky number is 12345
                Random rand = new Random();
                if (rand.Next(0, 100000) == 12345)
                    client.MoneyInWallet += 1000000;

                db.Client.Update(client);
                await db.SaveChangesAsync();

                var viewClientModel = new ViewClientModel()
                {
                    Id = client.Id,
                    FullName = client.FullName,
                    Age = client.Age,
                    MoneyInWallet = client.MoneyInWallet,
                    Warnings = client.Warnings
                };

                // return succcess code
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
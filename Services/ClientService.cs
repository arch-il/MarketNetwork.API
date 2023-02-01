namespace MarketNetwork.API.Services
{
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    public class ClientService : IClientService
    {
        public Client GetCheckedClient(Client client)
        {
            if (client == null ||
                client.FullName.Length < 3 ||
                client.Age < 0 ||
                client.Warnings < 0 ||
                client.MoneyInWallet < 0)
            {
                return null;
            }
            return client;
        }
    }
}

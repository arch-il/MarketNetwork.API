namespace MarketNetwork.API.Interfaces
{
    using MarketNetwork.API.Entities;
    public interface IClientService
    {
        public Client GetCheckedClient(Client client);
    }
}

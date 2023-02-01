namespace MarketNetwork.API.Interfaces
{
    using MarketNetwork.API.Entities;
    public interface IMarketService
    {
        public Market GetCheckedMarket(Market market);
    }
}

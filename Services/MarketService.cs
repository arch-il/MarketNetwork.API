namespace MarketNetwork.API.Services
{
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    public class MarketService : IMarketService
    {
        public Market GetCheckedMarket(Market market)
        {
            
            if (market == null ||
                market.NetWorth < 0 ||
                market.Location.Length < 0 ||
                market.OwnerCompany == null ||
                market.Owner == null)
            {
                return null;
            }
            
            return market;
        }
    }
}

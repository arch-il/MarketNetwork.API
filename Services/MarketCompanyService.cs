namespace MarketNetwork.API.Services
{
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    public class MarketCompanyService : IMarketCompanyService
    {
        public MarketCompany GetCheckedMarketCompany(MarketCompany marketCompany)
        {
            if (marketCompany == null ||
                marketCompany.CompanyName.Length < 3 ||
                marketCompany.NetWorth < 0)
            {
                return null;
            }
            return marketCompany;
        }
    }
}

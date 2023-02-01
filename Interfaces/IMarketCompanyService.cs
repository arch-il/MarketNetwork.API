namespace MarketNetwork.API.Interfaces
{
    using MarketNetwork.API.Entities;
    public interface IMarketCompanyService
    {
        public MarketCompany GetCheckedMarketCompany(MarketCompany marketCompany);
    }
}

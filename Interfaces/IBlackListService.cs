namespace MarketNetwork.API.Interfaces
{
    using MarketNetwork.API.Entities;
    public interface IBlackListService
    {
        public BlackList GetCheckedBlackList(BlackList blackList);
    }
}

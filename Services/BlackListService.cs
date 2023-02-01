namespace MarketNetwork.API.Services
{
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    public class BlackListService : IBlackListService
    {
        public BlackList GetCheckedBlackList(BlackList blackList)
        {
            if (blackList == null ||
                blackList.ClientId < 0)
            {
                return null;
            }
            return blackList;
        }
    }
}

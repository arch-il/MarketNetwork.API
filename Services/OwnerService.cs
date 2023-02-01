namespace MarketNetwork.API.Services
{
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    public class OwnerService : IOwnerService
    {
        public Owner GetCheckedOwner(Owner owner)
        {
            if (owner == null ||
                owner.FullName.Length < 3 ||
                owner.Age < 0 ||
                owner.NetWorth < 0)
            {
                return null;
            }
            return owner;
        }
    }
}

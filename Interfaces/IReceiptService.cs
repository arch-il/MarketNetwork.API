namespace MarketNetwork.API.Interfaces
{
    using MarketNetwork.API.Entities;
    public interface IReceiptService
    {
        public Receipt GetCheckedReceipt(Receipt receipt);
    }
}

namespace MarketNetwork.API.Services
{
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;
    public class ReceiptService : IReceiptService
    {
        public Receipt GetCheckedReceipt(Receipt receipt)
        {
            if (receipt == null ||
                receipt.Market == null ||
                receipt.Client == null ||
                receipt.TotalPrice < 0)
            {
                return null;
            }
            return receipt;
        }
    }
}

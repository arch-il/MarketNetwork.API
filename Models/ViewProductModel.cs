namespace MarketNetwork.API.Models
{
    public class ViewProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCompany { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}

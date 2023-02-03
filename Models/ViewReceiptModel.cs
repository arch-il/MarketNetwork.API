using MarketNetwork.API.Entities;

namespace MarketNetwork.API.Models
{
    public class ViewReceiptModel
    {
        public int Id { get; set; }
        public Market Market { get; set; }
        public Client Client { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}

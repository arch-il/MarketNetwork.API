namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Receipt
    {
        [Key]
        public int Id { get; set; }
        public Market Market { get; set; }
        public Client Client { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}

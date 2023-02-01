namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Receipt
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public Market Market { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public Client Client { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public List<Product> Products { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public DateTime PurchaseDate { get; set; }
    }
}

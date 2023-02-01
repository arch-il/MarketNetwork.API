namespace MarketNetwork.API.Models
{
    using MarketNetwork.API.Entities;

    using System.ComponentModel.DataAnnotations;
    public class CreateReceiptModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        public int MarketId { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int ClientId { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public List<Product> Products { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public DateTime PurchaseDate { get; set; }
    }
}

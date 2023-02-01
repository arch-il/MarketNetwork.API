namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public string ProductCompany { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public DateTime ExpirationDate { get; set; }
    }
}

namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Market
    {
        [Key]
        public int Id { get; set; }
        public List<Product> Products { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public decimal NetWorth { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public string Location { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public MarketCompany OwnerCompany { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public Owner Owner { get; set; }
    }
}

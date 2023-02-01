namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class MarketCompany
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public decimal NetWorth { get; set; }
    }
}

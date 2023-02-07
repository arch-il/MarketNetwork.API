namespace MarketNetwork.API.Models
{
    using System.ComponentModel.DataAnnotations;
    public class CreateMarketCompanyModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public decimal NetWorth { get; set; }
    }
}

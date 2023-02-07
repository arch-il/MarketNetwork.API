namespace MarketNetwork.API.Models
{
    using System.ComponentModel.DataAnnotations;
    public class CreateMarketModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        public decimal NetWorth { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public string Location { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int OwnerCompanyId { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int OwnerId { get; set; }
    }
}

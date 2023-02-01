namespace MarketNetwork.API.Models
{
    using System.ComponentModel.DataAnnotations;
    public class CreateOwnerModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int Age { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public decimal NetWorth { get; set; }
    }
}

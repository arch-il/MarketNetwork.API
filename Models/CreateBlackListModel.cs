namespace MarketNetwork.API.Models
{
    using System.ComponentModel.DataAnnotations;
    public class CreateBlackListModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        public int ClientId { get; set; }
    }
}

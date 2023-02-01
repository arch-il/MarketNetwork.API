namespace MarketNetwork.API.Models
{
    using System.ComponentModel.DataAnnotations;
    public class ModifyProductToMarketModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int MarketId { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int OwnerId { get; set; }
    }
}

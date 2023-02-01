namespace MarketNetwork.API.Models
{
    using System.ComponentModel.DataAnnotations;
    public class BuyProductsModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        public int ClintId { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int MarketId { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public List<int> ProductIds { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public List<int> ProductQuantities { get; set; }
    }
}

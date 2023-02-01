namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class BlackList
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int ClientId { get; set; }
    }
}

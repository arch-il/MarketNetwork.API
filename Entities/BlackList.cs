namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class BlackList
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
    }
}

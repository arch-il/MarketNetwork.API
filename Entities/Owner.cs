namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Owner
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public decimal NetWorth { get; set; }
    }
}

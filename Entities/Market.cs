namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Market
    {
        [Key]
        public int Id { get; set; }
        public List<Product> Products { get; set; }
        public decimal NetWorth { get; set; }
        public string Location { get; set; }
        public MarketCompany OwnerCompany { get; set; }
        public Owner Owner { get; set; }
    }
}

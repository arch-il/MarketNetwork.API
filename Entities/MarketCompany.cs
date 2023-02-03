namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class MarketCompany
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public decimal NetWorth { get; set; }
    }
}

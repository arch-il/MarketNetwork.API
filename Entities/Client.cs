namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public int Warnings { get; set; }
        public decimal MoneyInWallet { get; set; }
    }
}

namespace MarketNetwork.API.Models
{
    public class UpdateClientModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public int Warnings { get; set; }
        public decimal MoneyInWallet { get; set; }
    }
}

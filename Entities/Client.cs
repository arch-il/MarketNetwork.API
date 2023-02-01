namespace MarketNetwork.API.Entities
{
    using System.ComponentModel.DataAnnotations;
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int Age { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public int Warnings { get; set; }
        [Required(ErrorMessage = "{0} is required!")]
        public decimal MoneyInWallet { get; set; }
    }
}

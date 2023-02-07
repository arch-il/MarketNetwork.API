namespace MarketNetwork.API.Models
{
    public class CustomResponseModel<T>
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Result { get; set; }
    }
}

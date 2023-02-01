namespace MarketNetwork.API.Interfaces
{
    using MarketNetwork.API.Entities;
    public interface IProductService
    {
        public Product GetCheckedProduct(Product product);
    }
}

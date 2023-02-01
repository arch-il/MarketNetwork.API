namespace MarketNetwork.API.Services
{
    using MarketNetwork.API.Entities;
    using MarketNetwork.API.Interfaces;

    public class ProductService : IProductService
    {
        public Product GetCheckedProduct(Product product)
        {
            if (product == null ||
                product.ProductName.Length < 3 ||
                product.ProductCompany.Length < 3 ||
                product.Quantity < 0 ||
                product.Price < 0 ||
                product.Weight < 0 ||
                product.Description.Length < 3)
            {
                return null;
            }
            return product;
        }
    }
}

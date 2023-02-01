namespace MarketNetwork.API.Context
{
    using MarketNetwork.API.Entities;
    using Microsoft.EntityFrameworkCore;
    public sealed class MarketNetworkContext : DbContext
    {
        public DbSet<BlackList> BlackList { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Market> Market { get; set; }
        public DbSet<MarketCompany> MarketCompany  { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Receipt> Receipt { get; set; }
    public MarketNetworkContext(DbContextOptions<MarketNetworkContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}

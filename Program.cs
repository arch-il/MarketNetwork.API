using MarketNetwork.API.Context;
using MarketNetwork.API.Interfaces;
using MarketNetwork.API.Services;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add context to connection string to make migration
builder.Services.AddDbContext<MarketNetworkContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MarketNetworkDB")));

// Add all the controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add all the Service interfaces
builder.Services.AddTransient<IBlackListService, BlackListService>();
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IMarketCompanyService, MarketCompanyService>();
builder.Services.AddTransient<IMarketService, MarketService>();
builder.Services.AddTransient<IOwnerService, OwnerService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IReceiptService, ReceiptService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

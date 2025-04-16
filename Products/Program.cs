using Microsoft.EntityFrameworkCore;
using Products.Data;
using Products.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var productsContext = builder.Configuration.GetConnectionString("ProductsContext")
    ?? throw new InvalidOperationException("Connection string 'ProductsContext' not found.");

builder.Services.AddDbContext<ProductDataContext>(options => options.UseSqlite(productsContext));

// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapProductEndpoints();

app.UseStaticFiles();

await app.CreateDbIfNotExistsAsync();

await app.RunAsync();

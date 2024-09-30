using Microsoft.EntityFrameworkCore;
using ProductAPI.Data.Entities;

namespace ProductAPI.Data;

public class ProductDbContext : DbContext 
{
    public DbSet<Product> Products { get; set; }
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) {}
}
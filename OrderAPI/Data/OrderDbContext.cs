using Microsoft.EntityFrameworkCore;
using OrderAPI.Data.Entities;

namespace OrderAPI.Data;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Orders
        modelBuilder.Entity<Order>().HasKey(x => x.Id);

        // Order Details
        modelBuilder.Entity<OrderDetail>().HasKey(x => x.Id);
         modelBuilder.Entity<OrderDetail>()
        .Property(o => o.Price)
        .HasColumnType("decimal(18,2)");

        // Relationship
        modelBuilder.Entity<Order>().HasMany(x => x.OrderDetails);
    }
}
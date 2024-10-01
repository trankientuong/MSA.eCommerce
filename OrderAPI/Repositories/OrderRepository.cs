using Business;
using OrderAPI.Data;
using OrderAPI.Data.Entities;

namespace OrderAPI.Repositories;

public class OrderRepository : GenericRepository<OrderDbContext, Order>
{
    public OrderRepository(OrderDbContext context) : base(context)
    {
        
    }
}
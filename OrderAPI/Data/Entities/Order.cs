using Contracts.Domain;

namespace OrderAPI.Data.Entities;

public class Order : IEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
using Contracts.Domain;
using Contracts.Events.Cart;
using MassTransit;
using OrderAPI.Data.Entities;

namespace OrderAPI.Consumers;

public class CartCreatedConsumer : IConsumer<CartCreated>
{
    private readonly IRepository<Order> _repository;

    public CartCreatedConsumer(IRepository<Order> repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<CartCreated> context)
    {
        var message = context.Message;
        Console.WriteLine($"OrderAPI is received message {message}.");

        var order = new Order() 
        {
            UserId = message.UserId,
            OrderStatus = "Created",
            OrderDetails = message.cartDetails.Select(x => new OrderDetail
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                Price = x.Price
            }).ToList()
        };

        await _repository.AddAsync(order);
    }
}
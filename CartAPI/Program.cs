using Business.MassTransit;
using CartAPI.Entities;
using Contracts.Events.Cart;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransitWithRabbitMQ();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/CheckoutOrder", async (CartDto cartDto, IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(new CartCreated
    {
        UserId = cartDto.UserId,
        cartDetails = cartDto.CartDetailsDtos.Select(x => new CartDetails
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Quantity = x.Quantity,
            Price = x.Price
        })
    });

    return Results.Ok();
});

app.Run();

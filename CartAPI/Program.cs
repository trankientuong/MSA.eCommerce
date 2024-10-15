using System.Security.Claims;
using Business.MassTransit;
using CartAPI.Entities;
using Contracts.Events.Cart;
using Contracts.Settings;
using MassTransit;
using Microsoft.OpenApi.Models;
using Security.Authentication;
using Security.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransitWithRabbitMQ()
                .AddMSAAuthentication()
                .AddMSAAuthorization(opt => {
                    opt.AddPolicy("checkout_access", policy => {
                        policy.RequireClaim("scope","cartapi.checkout");
                    });
                })
                .AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var srvUrlsSetting = builder.Configuration.GetSection(nameof(ServiceUrlsSetting)).Get<ServiceUrlsSetting>();
builder.Services.AddSwaggerGen(options =>
{
    var scheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{srvUrlsSetting.IdentityServiceUrl}/connect/authorize"),
                TokenUrl = new Uri($"{srvUrlsSetting.IdentityServiceUrl}/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                     { "cartapi.checkout", "Access checkout operations" }
                }
            }
        },
        Type = SecuritySchemeType.OAuth2
    };

    options.AddSecurityDefinition("OAuth", scheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference { Id = "OAuth", Type = ReferenceType.SecurityScheme }
             },
             new List<string> { }
         }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
     {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart API V1");
        options.OAuthClientId("cart-swagger");
        options.OAuthScopes("profile", "openid");
        options.OAuthUsePkce();
     });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapPost("/api/Cart/CheckoutOrder", async (CartDto cartDto, IPublishEndpoint publishEndpoint, IHttpContextAccessor httpContext) =>
{
    var userId = httpContext?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    await publishEndpoint.Publish(new CartCreated
    {
        UserId = userId,
        cartDetails = cartDto.CartDetailsDtos.Select(x => new CartDetails
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Quantity = x.Quantity,
            Price = x.Price
        })
    });

    return Results.Ok();
})
.RequireAuthorization("checkout_access");

app.Run();

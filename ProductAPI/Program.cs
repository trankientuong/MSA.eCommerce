using Business.IServices;
using Business.Services;
using Contracts.Domain;
using Contracts.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Security.Authentication;
using Security.Authorization;
using ProductAPI.Data;
using ProductAPI.Data.Entities;
using ProductAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

SqlDbSetting serviceSetting = builder.Configuration.GetSection(nameof(SqlDbSetting)).Get<SqlDbSetting>();
// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddDbContext<ProductDbContext>(options => 
    options.UseSqlServer(serviceSetting?.ConnectionString));

builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddMSAAuthentication()
                .AddMSAAuthorization(opt =>
                {
                    opt.AddPolicy("read_access", policy => {
                        policy.RequireClaim("scope", "productapi.read");
                    });
                    opt.AddPolicy("write_access", policy => {
                        policy.RequireClaim("scope", "productapi.write");
                    });
                });

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
                     { "productapi.read", "Access read operations" },
                     { "productapi.write", "Access write operations" }
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
    // app.UseSwaggerUI(c => 
    // {
    //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
    // });
    app.UseSwaggerUI(options =>
     {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
        options.OAuthClientId("product-swagger");
        options.OAuthScopes("profile", "openid");
        options.OAuthUsePkce();
     });
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

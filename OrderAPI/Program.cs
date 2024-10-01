using Business.MassTransit;
using Contracts.Domain;
using Contracts.Settings;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.Data.Entities;
using OrderAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

SqlDbSetting serviceSetting = builder.Configuration.GetSection(nameof(SqlDbSetting)).Get<SqlDbSetting>();
// Add services to the container.
builder.Services.AddDbContext<OrderDbContext>(options => 
    options.UseSqlServer(serviceSetting?.ConnectionString));

builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
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

app.Run();

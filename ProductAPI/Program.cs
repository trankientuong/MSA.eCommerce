using Business.IServices;
using Business.Services;
using Contracts.Domain;
using Contracts.Settings;
using Microsoft.EntityFrameworkCore;
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
    });
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

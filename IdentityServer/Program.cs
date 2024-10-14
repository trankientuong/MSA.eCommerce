using Contracts.Settings;
using IdentityServer.Config;
using IdentityServer.Data;
using IdentityServer.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

SqlDbSetting serviceSetting = builder.Configuration.GetSection(nameof(SqlDbSetting)).Get<SqlDbSetting>();

// Add services to the container.
// Configure DBContext with ASP.NET Identity
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(serviceSetting?.ConnectionString));

// Configure ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure IdentityServer
builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.Events.RaiseFailureEvents = true;

    options.EmitStaticAudienceClaim = true;
}).AddDeveloperSigningCredential() // 
    .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes) // Add API scopes
    .AddInMemoryClients(IdentityServerConfig.Clients)     // Add clients
    .AddInMemoryApiResources(IdentityServerConfig.ApiResources)  // Add API resources
    .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources) // Add identity resources
    .AddAspNetIdentity<ApplicationUser>()   // Integrate with ASP.NET Identity
    .AddProfileService<CustomProfileService>();  // Add custom profile

builder.Services.AddRazorPages();
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

app.UseStaticFiles();

app.UseIdentityServer();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

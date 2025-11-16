using EvenApp.Application.Services;
using EvenApp.Domain.Interfaces;
using EvenApp.Infrastructure.Data;
using EvenApp.Infrastructure.Hubs;
using EvenApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT support
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "EvenApp",
        ValidAudience = jwtSettings["Audience"] ?? "EvenApp",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
    
    // Configure SignalR JWT
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/inventoryhub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAdmin", policy => policy.RequireRole("Admin", "Manager"));
});

// Configure Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost;Database=EvenAppInventory;User=root;Password=;";
var dbFactory = new EvenApp.Infrastructure.Data.DbConnectionFactory(connectionString);
builder.Services.AddSingleton<EvenApp.Infrastructure.Data.DbConnectionFactory>(dbFactory);
builder.Services.AddSingleton<EvenApp.Application.Interfaces.IDbConnectionFactory>(dbFactory);

// Register Repositories
builder.Services.AddScoped<IUserRepository, EvenApp.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<IProductRepository, EvenApp.Infrastructure.Repositories.ProductRepository>();
builder.Services.AddScoped<IInventoryRepository, EvenApp.Infrastructure.Repositories.InventoryRepository>();
builder.Services.AddScoped<ISupplierRepository, EvenApp.Infrastructure.Repositories.SupplierRepository>();
builder.Services.AddScoped<IOrderRepository, EvenApp.Infrastructure.Repositories.OrderRepository>();
builder.Services.AddScoped<IAlertRepository, EvenApp.Infrastructure.Repositories.AlertRepository>();
builder.Services.AddScoped<IInventoryHistoryRepository, EvenApp.Infrastructure.Repositories.InventoryHistoryRepository>();
builder.Services.AddScoped<ITransactionRepository, EvenApp.Infrastructure.Repositories.TransactionRepository>();
builder.Services.AddScoped<IProductSupplierRepository, EvenApp.Infrastructure.Repositories.ProductSupplierRepository>();

// Register Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Register IDbConnectionFactory
builder.Services.AddSingleton<EvenApp.Application.Interfaces.IDbConnectionFactory>(sp => 
    sp.GetRequiredService<EvenApp.Infrastructure.Data.DbConnectionFactory>());

// Configure SignalR
builder.Services.AddSignalR();

// Register Background Services
builder.Services.AddHostedService<EvenApp.Infrastructure.Services.AlertBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowVueApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<InventoryHub>("/inventoryhub");

app.Run();

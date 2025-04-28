using CryptoDepositApp.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter("fixed", _ =>
            new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(10)
            }));
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Register services
builder.Services.AddSingleton<ICryptoService, CryptoService>();
builder.Services.AddScoped<IEthereumService, EthereumService>();
builder.Services.AddSingleton<ITokenListingService, TokenListingService>();

// Register HttpClient for API calls
builder.Services.AddHttpClient();

// Add environment variables including API keys
builder.Configuration.AddEnvironmentVariables();

// Register TronService with direct HttpClient dependency for real API usage
builder.Services.AddHttpClient<ITronService, TronService>((client, sp) =>
{
    var logger = sp.GetRequiredService<ILogger<TronService>>();
    var config = sp.GetRequiredService<IConfiguration>();
    
    return new TronService(null, null, logger, config, client);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crypto Deposit API V1");
        c.RoutePrefix = "swagger"; // Change to serve swagger at /swagger
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors();
app.UseAuthorization();
app.UseRouting();

app.MapControllers();

// Add a fallback route to serve the index.html for SPA
app.MapFallbackToFile("index.html");

app.Run();

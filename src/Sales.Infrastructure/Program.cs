using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Read connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("SalesDB");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'SalesDB' is not configured.");
}

// Register DbContext with PostgreSQL connection
builder.Services.AddDbContext<SalesDBContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();
app.Run();

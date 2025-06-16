using GeoJourneyer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default") ??
    $"Data Source={Path.Combine(AppContext.BaseDirectory, "app.db")}";
var context = new DatabaseContext(connectionString);
builder.Services.AddSingleton(context);

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();
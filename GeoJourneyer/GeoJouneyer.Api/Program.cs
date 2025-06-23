using GeoJourneyer.Infrastructure.Persistance;
using GeoJourneyer.Application.Extensions;
using GeoJourneyer.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default") ??
    $"Data Source={Path.Combine(AppContext.BaseDirectory, "app.db")}";
var context = new DatabaseContext(connectionString);
builder.Services.AddSingleton(context);

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
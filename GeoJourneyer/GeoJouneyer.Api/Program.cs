using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
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

var app = builder.Build();

app.MapGet("/countries", (ICountryService service) => Results.Ok(service.GetAll()));
app.MapPost("/countries", (ICountryService service, Country country) => Results.Ok(service.AddCountry(country)));

app.MapGet("/countries/{id}/places", (IPlaceService service, int id) => Results.Ok(service.GetByCountry(id)));
app.MapPost("/places", (IPlaceService service, Place place) => Results.Ok(service.AddPlace(place)));

app.MapGet("/plans/{userId}", (ITravelPlanService service, int userId) => Results.Ok(service.GetPlans(userId)));
app.MapPost("/plans", (ITravelPlanService service, TravelPlanDto dto) =>
{
    var id = service.CreatePlan(new TravelPlan { UserId = dto.UserId, CountryId = dto.CountryId, Name = dto.Name }, dto.PlaceIds);
    return Results.Ok(id);
});
app.MapGet("/plans/{planId}/stops", (ITravelPlanService service, int planId) => Results.Ok(service.GetPlanStops(planId)));

app.MapPost("/plans/{planId}/optimize", (ITravelPlanService service, int planId) =>
{
    var stops = service.GetPlanStops(planId);
    var places = service.GetPlaces(stops.Select(s => s.PlaceId));
    var optimized = service.OptimizeRoute(places);
    service.SaveStops(planId, optimized);
    return Results.Ok(optimized);
});

app.Run();

record TravelPlanDto(int UserId, int CountryId, string Name, IEnumerable<int> PlaceIds);
using MySpot.Api.Repositories;
using MySpot.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSingleton<IClock, Clock>()
    // register as singleton, because of it is hardcoded data
    .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
    .AddSingleton<IReservationsService, ReservationsService>()
    .AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();


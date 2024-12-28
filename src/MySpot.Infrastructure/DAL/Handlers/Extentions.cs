using MySpot.Application.DTO;
using MySpot.Core.Entities;

namespace MySpot.Infrastructure.DAL.Handlers;

internal static class Extentions
{
    public static WeeklyParkingSpotDto AsDto(this WeeklyParkingSpot entity) => new()
    {
        Id = entity.Id.Value.ToString(),
        Name = entity.Name,
        From = entity.Week.From.Value.DateTime,
        To = entity.Week.To.Value.DateTime,
        Capacity = entity.Capacity,
        Reservations = entity.Reservations.Select(x => new ReservationDto()
        {
            Id = x.Id,
            Date = x.Date.Value.DateTime,
            EmployeeName = x is VehicleReservation vr ? vr.EmployeeName : null,
            ParkingSpotId = x.ParkingSpotId,
            Type = x is VehicleReservation ? "vehicle" : "cleaning"
        })
    };
    
    public static UserDto AsDto(this User entity)
        => new()
        {
            Id = entity.Id,
            Username = entity.Username,
            FullName = entity.FullName
        };
}
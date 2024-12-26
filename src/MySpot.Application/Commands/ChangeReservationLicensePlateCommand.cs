using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;

public record ChangeReservationLicensePlateCommand(Guid ReservationId, string LicensePlate) : ICommand;
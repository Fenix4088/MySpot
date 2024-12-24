namespace MySpot.Application.Commands;

public record ReserveParkingSpotForVehicleCommand(Guid ParkingSpotId, Guid ReservationId, int Capacity, string EmployeeName, string LicensePlate, DateTime Date);
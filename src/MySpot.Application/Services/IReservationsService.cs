using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.Services;

public interface IReservationsService
{
    Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync();
    Task<ReservationDto?> GetAsync(Guid id);
    Task<Guid?> ReserveForVehicleAsync(ReserveParkingSpotForVehicleCommand reserveParkingSpotForVehicleCommand);    
    Task ReserveForCleaningAsync(ReserveParkingSpotForCleaningCommand reserveParkingSpotForCleaningCommand);
    Task<bool> ChangeReservationLicensePlateAsync(ChangeReservationLicensePlateCommand changeReservationLicensePlateCommand);
    Task<bool> DeleteAsync(DeleteReservationCommand deleteReservationCommand);
}
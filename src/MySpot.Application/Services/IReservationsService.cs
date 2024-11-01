using MySpot.Application.Commands;
using MySpot.Application.DTO;

namespace MySpot.Application.Services;

public interface IReservationsService
{
    IEnumerable<ReservationDto> GetAllWeekly();
    ReservationDto? Get(Guid id);
    Guid? Create(CreateReservationCommand createReservationCommand);
    bool Update(ChangeReservationLicensePlateCommand changeReservationLicensePlateCommand);
    bool Delete(DeleteReservationCommand deleteReservationCommand);
}
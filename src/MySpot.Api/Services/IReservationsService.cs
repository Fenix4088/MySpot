using MySpot.Api.Commands;
using MySpot.Api.DTO;

namespace MySpot.Api.Services;

public interface IReservationsService
{
    IEnumerable<ReservationDto> GetAllWeekly();
    ReservationDto? Get(Guid id);
    Guid? Create(CreateReservationCommand createReservationCommand);
    bool Update(ChangeReservationLicensePlateCommand changeReservationLicensePlateCommand);
    bool Delete(DeleteReservationCommand deleteReservationCommand);
}
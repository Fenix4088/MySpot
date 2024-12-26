using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class ReservationNotFoundException(Guid reservationId): MySpotException($"Reservation with id: {reservationId} not found.")
{
    
}
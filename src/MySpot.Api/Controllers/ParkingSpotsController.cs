using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("parking-spots")]
public class ParkingSpotsController : ControllerBase
{
    private readonly ICommandHandler<ReserveParkingSpotForVehicleCommand> _reserveParkingSpotsForVehicleHandler;
    private readonly ICommandHandler<ReserveParkingSpotForCleaningCommand> _reserveParkingSpotsForCleaningHandler;
    private readonly ICommandHandler<ChangeReservationLicensePlateCommand> _changeReservationLicensePlateHandler;
    private readonly ICommandHandler<DeleteReservationCommand> _deleteReservationCommandHandler;
    private readonly IQueryHandler<GetWeeklyParkingSpotsQuery, IEnumerable<WeeklyParkingSpotDto>> _getWeeklyParkingSpotsQueryHandler;

    public ParkingSpotsController(
        ICommandHandler<ReserveParkingSpotForVehicleCommand> reserveParkingSpotsForVehicleHandler,
        ICommandHandler<ReserveParkingSpotForCleaningCommand> reserveParkingSpotsForCleaningHandler,
        ICommandHandler<ChangeReservationLicensePlateCommand> changeReservationLicencePlateHandler,
        ICommandHandler<DeleteReservationCommand> deleteReservationCommandHandler,
        IQueryHandler<GetWeeklyParkingSpotsQuery, IEnumerable<WeeklyParkingSpotDto>> getWeeklyParkingSpotsQueryHandler)
    {
        _reserveParkingSpotsForVehicleHandler = reserveParkingSpotsForVehicleHandler;
        _reserveParkingSpotsForCleaningHandler = reserveParkingSpotsForCleaningHandler;
        _changeReservationLicensePlateHandler = changeReservationLicencePlateHandler;
        _deleteReservationCommandHandler = deleteReservationCommandHandler;
        _getWeeklyParkingSpotsQueryHandler = getWeeklyParkingSpotsQueryHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> Get([FromQuery] GetWeeklyParkingSpotsQuery query) => Ok(await _getWeeklyParkingSpotsQueryHandler.HandleAsync(query));
    
    [HttpPost("{parkingSpotId:guid}/reservations/vehicle")]
    public async Task<ActionResult> Post(Guid parkingSpotId, ReserveParkingSpotForVehicleCommand command)
    {
        await _reserveParkingSpotsForVehicleHandler.HandleAsync(command with
        {
            ReservationId = Guid.NewGuid(), 
            ParkingSpotId = parkingSpotId
        });

        return NoContent();
    }
    
    [HttpPost("reservations/cleaning")]
    public async Task<ActionResult> Post(ReserveParkingSpotForCleaningCommand command)
    {
        await _reserveParkingSpotsForCleaningHandler.HandleAsync(command);
        return NoContent();
    }

    [HttpPut("reservations/{reservationId:guid}")]
    public async Task<ActionResult> Put(Guid reservationId, [FromBody] ChangeReservationLicensePlateCommand command)
    {
        await _changeReservationLicensePlateHandler.HandleAsync(command with { ReservationId = reservationId });
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _deleteReservationCommandHandler.HandleAsync(new DeleteReservationCommand(id));
        return NoContent();
    }
}
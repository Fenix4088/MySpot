using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationsController(IReservationsService reservationService): ControllerBase
{
    private readonly IReservationsService _reservationService = reservationService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> Get() => Ok(await _reservationService.GetAllWeeklyAsync());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservationDto?>> GetById(Guid id)
    {
        var reservation = await _reservationService.GetAsync(id);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost("vehicle")]
    public async Task<ActionResult> Post(ReserveParkingSpotForVehicleCommand reserveParkingSpotForVehicleCommand)
    {
        var id = await _reservationService.ReserveForVehicleAsync(reserveParkingSpotForVehicleCommand with { ReservationId = Guid.NewGuid() });

        if (id is null) return BadRequest();

        return CreatedAtAction(nameof(Get), new { id }, null);
    }
    
    [HttpPost("cleaning")]
    public async Task<ActionResult> Post(ReserveParkingSpotForCleaningCommand reserveParkingSpotForCleaningCommand)
    {
        await _reservationService.ReserveForCleaningAsync(reserveParkingSpotForCleaningCommand);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Put(Guid id, [FromBody] ChangeReservationLicensePlateCommand changeReservationLicensePlateCommand)
    {

        if (await _reservationService.ChangeReservationLicensePlateAsync(changeReservationLicensePlateCommand with { ReservationId = id}))
        {
            return NoContent();
        }
        
        return BadRequest();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        if (await _reservationService.DeleteAsync(new DeleteReservationCommand(id)))
        {
            return NoContent();
        }
        return NotFound();
    }
}
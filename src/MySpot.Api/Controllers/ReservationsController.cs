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
    public ActionResult<IEnumerable<ReservationDto>> Get() => Ok(_reservationService.GetAllWeekly());

    [HttpGet("{id:guid}")]
    public ActionResult<ReservationDto?> GetById(Guid id)
    {
        var reservation = _reservationService.Get(id);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(CreateReservationCommand createReservationCommand)
    {
        var id = _reservationService.Create(createReservationCommand with { ReservationId = Guid.NewGuid() });

        if (id is null) return BadRequest();

        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPut("{id:guid}")]
    public ActionResult Put(Guid id, [FromBody] ChangeReservationLicensePlateCommand changeReservationLicensePlateCommand)
    {

        if (_reservationService.Update(changeReservationLicensePlateCommand with { ReservationId = id}))
        {
            return NoContent();
        }
        
        return BadRequest();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult Delete(Guid id)
    {
        if (_reservationService.Delete(new DeleteReservationCommand(id)))
        {
            return NoContent();
        }
        return NotFound();
    }
}
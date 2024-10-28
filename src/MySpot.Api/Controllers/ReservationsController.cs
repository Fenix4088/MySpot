using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;
using MySpot.Api.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationsController: ControllerBase
{

    private readonly ReservationsService _reservationService = new ();

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_reservationService.GetAll());

    [HttpGet("{id:int}")]
    public ActionResult<Reservation?> GetById(int id)
    {
        var reservation = _reservationService.Get(id);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(Reservation reservation)
    {
        var id = _reservationService.Create(reservation);

        if (id is null) return BadRequest();

        return CreatedAtAction(nameof(Get), new { id }, null);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, [FromBody] Reservation newReservation)
    {

        if (_reservationService.Update(id, newReservation))
        {
            return NoContent();
        }
        
        return BadRequest();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        if (_reservationService.Delete(id))
        {
            return NoContent();
        }
        return NotFound();
    }
}
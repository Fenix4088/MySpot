using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservationsController: ControllerBase
{
    private static int Id = 1;
    private static readonly List<string> ParkingSpotNames = [
        "P1",
        "P2",
        "P3",
        "P4",
        "P5",
    ];
    private static readonly List<Reservation> ReservationsList = [];

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(ReservationsList);

    [HttpGet("{id:int}")]
    public ActionResult<Reservation?> GetById(int id)
    {
        var reservation =  ReservationsList.SingleOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(Reservation reservation)
    {
        if (ParkingSpotNames.All(spot => spot != reservation.ParkingSpotName))
        {
            return BadRequest();
        }

        if (ReservationsList.Any(r => r.Date == reservation.Date && r.ParkingSpotName == reservation.ParkingSpotName))
        {
            return BadRequest();
        }

        reservation.Id = Id;
        Id++;
        ReservationsList.Add(reservation);

        return CreatedAtAction(nameof(Get), new {id = reservation.Id}, null);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, [FromBody] Reservation newReservation)
    {
        var reservation = ReservationsList.SingleOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return NotFound();
        }
        
        if (ParkingSpotNames.All(spot => spot != newReservation.ParkingSpotName))
        {
            return BadRequest();
        }
        
        var reservationIndex = ReservationsList.IndexOf(reservation);
        ReservationsList[reservationIndex] = newReservation;
        ReservationsList[reservationIndex].Id = id;
        return NoContent();
    }
}
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
    public IEnumerable<Reservation> Get()
    {
        return ReservationsList;
    }

    [HttpGet("{id:int}")]
    public Reservation? GetById(int id)
    {
        var reservation =  ReservationsList.SingleOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return default;
        }

        return reservation;
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

        return Created($"http://localhost:5000/reservation/{reservation.Id}", null);
    }
}
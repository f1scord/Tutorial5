using Microsoft.AspNetCore.Mvc;
using Tutorial5.Models;

namespace Tutorial5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll([FromQuery] DateOnly? date, [FromQuery] string? status, [FromQuery] int? roomId)
    {
        var reservations = DataStore.Reservations.AsEnumerable();

        if (date.HasValue)
            reservations = reservations.Where(r => r.Date == date.Value);
        if (!string.IsNullOrEmpty(status))
            reservations = reservations.Where(r => r.Status == status);
        if (roomId.HasValue)
            reservations = reservations.Where(r => r.RoomId == roomId.Value);

        return Ok(reservations.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Reservation reservation)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room == null) return NotFound("Room not found.");
        if (!room.IsActive) return BadRequest("Room is inactive.");

        var overlap = DataStore.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.StartTime < reservation.EndTime &&
            reservation.StartTime < r.EndTime);

        if (overlap) return Conflict("Reservation overlaps with an existing one.");

        reservation.Id = DataStore.Reservations.Count > 0 ? DataStore.Reservations.Max(r => r.Id) + 1 : 1;
        DataStore.Reservations.Add(reservation);
        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Reservation updated)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();

        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == updated.RoomId);
        if (room == null) return NotFound("Room not found.");
        if (!room.IsActive) return BadRequest("Room is inactive.");

        var overlap = DataStore.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == updated.RoomId &&
            r.Date == updated.Date &&
            r.StartTime < updated.EndTime &&
            updated.StartTime < r.EndTime);

        if (overlap) return Conflict("Reservation overlaps with an existing one.");

        reservation.RoomId = updated.RoomId;
        reservation.OrganizerName = updated.OrganizerName;
        reservation.Topic = updated.Topic;
        reservation.Date = updated.Date;
        reservation.StartTime = updated.StartTime;
        reservation.EndTime = updated.EndTime;
        reservation.Status = updated.Status;

        return Ok(reservation);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null) return NotFound();

        DataStore.Reservations.Remove(reservation);
        return NoContent();
    }
}

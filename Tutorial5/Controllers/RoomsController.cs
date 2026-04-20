using Microsoft.AspNetCore.Mvc;
using Tutorial5.Models;

namespace Tutorial5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var rooms = DataStore.Rooms.AsEnumerable();

        if (minCapacity.HasValue)
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        if (hasProjector.HasValue)
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
        if (activeOnly == true)
            rooms = rooms.Where(r => r.IsActive);

        return Ok(rooms.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();
        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = DataStore.Rooms.Where(r => r.BuildingCode == buildingCode).ToList();
        return Ok(rooms);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Room room)
    {
        room.Id = DataStore.Rooms.Count > 0 ? DataStore.Rooms.Max(r => r.Id) + 1 : 1;
        DataStore.Rooms.Add(room);
        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Room updated)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        room.Name = updated.Name;
        room.BuildingCode = updated.BuildingCode;
        room.Floor = updated.Floor;
        room.Capacity = updated.Capacity;
        room.HasProjector = updated.HasProjector;
        room.IsActive = updated.IsActive;

        return Ok(room);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null) return NotFound();

        var hasReservations = DataStore.Reservations.Any(res => res.RoomId == id);
        if (hasReservations) return Conflict("Cannot delete room with existing reservations.");

        DataStore.Rooms.Remove(room);
        return NoContent();
    }
}

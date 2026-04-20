namespace Tutorial5;

using Models;

public static class DataStore
{
    public static List<Room> Rooms = new List<Room>
    {
        new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true, IsActive = true },
        new Room { Id = 3, Name = "Room 305", BuildingCode = "B", Floor = 3, Capacity = 15, HasProjector = false, IsActive = true },
        new Room { Id = 4, Name = "Hall A", BuildingCode = "A", Floor = 1, Capacity = 100, HasProjector = true, IsActive = false },
        new Room { Id = 5, Name = "Lab 002", BuildingCode = "C", Floor = 0, Capacity = 30, HasProjector = false, IsActive = true }
    };

    public static List<Reservation> Reservations = new List<Reservation>
    {
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Anna Kowalska", Topic = "Math Workshop", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0), Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 2, OrganizerName = "Jan Nowak", Topic = "C# Basics", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0), Status = "planned" },
        new Reservation { Id = 3, RoomId = 1, OrganizerName = "Maria Wiśniewska", Topic = "REST API", Date = new DateOnly(2026, 5, 11), StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(16, 0), Status = "confirmed" },
        new Reservation { Id = 4, RoomId = 3, OrganizerName = "Piotr Kowalski", Topic = "Testing", Date = new DateOnly(2026, 5, 12), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 30), Status = "canceled" }
    };
}
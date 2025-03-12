using BackendTzReworked.DAL.ConferenceRooms.Entityes;
using BackendTzReworked.DAL.EntityFeamework;
using BackendTzReworked.DAL.RoomSupplements.Entityes;

public class Seed
{
    public Seed(Context context)
    {
        _context = context;
    }

    private readonly Context _context;

    public void SeedDataContext()
    {
        if (!_context.Room.Any() && !_context.Supplement.Any())
        {
            _context.Room.AddRange(
                new Rooms
                {
                    Name = "Зал А",
                    Capacity = 50,
                    CostPerHour = 2000
                },
                new Rooms
                {
                    Name = "Зал B",
                    Capacity = 100,
                    CostPerHour = 3500
                },
                new Rooms
                {
                    Name = "Зал C",
                    Capacity = 30,
                    CostPerHour = 1500
                }
            );

            _context.Supplement.AddRange(
                new Supplements
                {
                    Name = "Проєктор",
                    Cost = 500
                },
                new Supplements
                {
                    Name = "Wi-Fi",
                    Cost = 300
                },
                new Supplements
                {
                    Name = "Звук",
                    Cost = 700
                }
            );

            _context.SaveChanges();
        }
    }
}

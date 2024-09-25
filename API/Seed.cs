using API.Entityes;

public class Seed
{
    //Info: "Поле контексту бази даних"
    private readonly Context _context;

    //Info: "Конструктор, що приймає контекст"
    public Seed(Context context)
    {
        _context = context;
    }

    //Info: "Метод для заповнення бази даних початковими даними"
    public void SeedDataContext()
    {
        //Info: "Перевіряє, чи не існує записів у таблицях конференц-залів та додаткових послуг"
        if (!_context.ConferenceRoom.Any() && !_context.AdditionalService.Any())
        {
            //Info: "Додає початкові дані для конференц-залів"
            _context.ConferenceRoom.AddRange(
                new ConferenceRooms
                {
                    Name = "Зал А",
                    Capacity = 50,
                    CostPerHour = 2000
                },
                new ConferenceRooms
                {
                    Name = "Зал B",
                    Capacity = 100,
                    CostPerHour = 3500
                },
                new ConferenceRooms
                {
                    Name = "Зал C",
                    Capacity = 30,
                    CostPerHour = 1500
                }
            );

            //Info: "Додає початкові дані для додаткових послуг"
            _context.AdditionalService.AddRange(
                new AdditionalServices
                {
                    Name = "Проєктор",
                    Cost = 500
                },
                new AdditionalServices
                {
                    Name = "Wi-Fi",
                    Cost = 300
                },
                new AdditionalServices
                {
                    Name = "Звук",
                    Cost = 700
                }
            );

            //Info: "Зберігає зміни до бази даних"
            _context.SaveChanges();
        }
    }
}

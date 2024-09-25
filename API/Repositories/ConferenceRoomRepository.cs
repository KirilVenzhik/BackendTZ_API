//Info: "Реалізація репозиторію для конференц-залів"
using API.Entityes;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ConferenceRoomRepository : Repository<ConferenceRooms>, IConferenceRoomRepository
{
    //Info: "Конструктор ініціалізує базовий клас з контекстом бази даних"
    public ConferenceRoomRepository(Context context) : base(context) { }

    //Info: "Шукає конференц-зал за його назвою"
    //Info: "Використовує метод FirstOrDefaultAsync для пошуку першого запису, де Name дорівнює переданій назві"
    public async Task<ConferenceRooms> GetByNameAsync(string name)
        => await _context.ConferenceRoom.FirstOrDefaultAsync(cr => cr.Name == name);

    //Info: "Отримує список конференц-залів з місткістю, більшою або рівною заданій"
    //Info: "Фільтрує таблицю ConferenceRoom за полем Capacity"
    public async Task<IEnumerable<ConferenceRooms>> GetByCapacityAsync(int capacity)
        => await _context.ConferenceRoom.Where(cr => cr.Capacity >= capacity).ToListAsync();

    //Info: "Отримує конференц-зали за вартістю за годину"
    //Info: "Фільтрує таблицю ConferenceRoom за полем CostPerHour"
    public async Task<IEnumerable<ConferenceRooms>> GetByCostPerHour(double costPerHour)
        => await _context.ConferenceRoom.Where(cr => cr.CostPerHour == costPerHour).ToListAsync();

    //Info: "Пошук доступних залів в заданий проміжок часу та з мінімальною місткістю"
    //Info: "Шукає конференц-зали, які не мають перетинів за часом з іншими бронюваннями і відповідають вимогам по місткості"
    public async Task<IEnumerable<ConferenceRooms>> SearchAvailableRooms(DateTime startTime, DateTime endTime, int capacity)
        => await _context.ConferenceRoom
            .Include(cr => cr.Reservations)
            .Where(cr => cr.Capacity >= capacity &&
                         !cr.Reservations.Any(r => r.TimeReservation < endTime && r.EndTimeReservation > startTime))
            .ToListAsync();

    //Info: "Отримує доступні додаткові послуги для конкретного конференц-залу"
    //Info: "Використовує Include і ThenInclude для завантаження пов'язаних даних про додаткові послуги для залу"
    public async Task<IEnumerable<AdditionalServices>> GetAvilableAdditionalServicesById(int conferenceRoomId)
    {
        var conferenceRoom = await _context.ConferenceRoom
            .Include(cr => cr.ConferenceRoomsAdditionalServices)
            .ThenInclude(cas => cas.AdditionalService)
            .FirstOrDefaultAsync(cr => cr.Id == conferenceRoomId);

        return conferenceRoom?.ConferenceRoomsAdditionalServices.Select(cas => cas.AdditionalService).ToList();
    }

    //Info: "Об'єднує конференц-зал з додатковими послугами, якщо вони не були додані раніше"
    //Info: "Перевіряє кожну додаткову послугу на наявність дублікатів і додає нові зв'язки"
    public async Task<ConferenceRooms> MergeConferenceRoomAndAdditionalServies(ConferenceRooms conferenceRoom, ICollection<int>? additionalServicesIds)
    {
        if (additionalServicesIds == null || !additionalServicesIds.Any())
            return conferenceRoom;

        var additionalServices = await _context.AdditionalService
                                               .Where(s => additionalServicesIds.Contains(s.Id))
                                               .ToListAsync();

        foreach (var additionalService in additionalServices)
        {
            if (!conferenceRoom.ConferenceRoomsAdditionalServices.Any(cas => cas.AdditionalServiceId == additionalService.Id))
            {
                var conferenceRoomAdditionalService = new ConferenceRoomAdditionalService
                {
                    ConferenceRoom = conferenceRoom,
                    AdditionalService = additionalService
                };

                conferenceRoom.ConferenceRoomsAdditionalServices.Add(conferenceRoomAdditionalService);
            }
        }

        return conferenceRoom;
    }
}

//Info: "Реалізація репозиторію для бронювань залів"
using API.Entityes;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

public class RoomReservationRepository : Repository<RoomReservations>, IRoomReservationRepository
{
    //Info: "Конструктор ініціалізує базовий клас з контекстом бази даних"
    public RoomReservationRepository(Context context) : base(context) { }

    //Info: "Отримує бронювання за його ідентифікатором і підключає пов'язаний конференц-зал для повної інформації"
    public async Task<RoomReservations> GetByIdAsync(int id)
        => await _context.RoomReservation
        .Include(entity => entity.ReservedRoom)  // Завантажує пов'язані дані конференц-залу
        .FirstOrDefaultAsync(entity => entity.Id == id);  // Повертає перше бронювання, яке відповідає Id

    //Info: "Перевіряє, чи не перетинається дане бронювання з іншими бронюваннями для того ж конференц-залу"
    public async Task<bool> CheckingIfReservationIsNotOverlapingOther(RoomReservations reservation, int reservedRoomId)
    {
        if (reservation == null)  // Перевірка на null, щоб уникнути помилок
            return false;

        //Info: "Шукає інші бронювання для того ж конференц-залу, що перетинаються за часом"
        return await _context.RoomReservation
            .AnyAsync(r => r.ReservedRoom.Id == reservedRoomId &&
                           r.TimeReservation < reservation.EndTimeReservation &&  // Початок нового бронювання не після завершення іншого
                           r.EndTimeReservation > reservation.TimeReservation);   // Кінець нового бронювання не до початку іншого
    }

    //Info: "Отримує список доступних залів за параметрами (час та мінімальна місткість)"
    public async Task<ICollection<ConferenceRooms>> GetAvailableRooms(DateTime startTime, DateTime endTime, int capacity)
    {
        var allRooms = await _context.ConferenceRoom.ToListAsync();  // Отримує всі зали

        //Info: "Отримує ідентифікатори залів, які вже зарезервовані в заданий проміжок часу"
        var reservedRoomIds = await _context.RoomReservation
            .Where(r => r.TimeReservation < endTime && r.EndTimeReservation > startTime)  // Фільтрує перетинаючіся бронювання
            .Select(r => r.Id)  // Отримує ідентифікатори залів
            .ToListAsync();

        //Info: "Фільтрує зали, які не зарезервовані та відповідають вимогам по місткості"
        var availableRooms = allRooms
            .Where(room => !reservedRoomIds.Contains(room.Id) && room.Capacity >= capacity)
            .ToList();

        return availableRooms;
    }

    //Info: "Отримує бронювання за номером телефону клієнта"
    public async Task<IEnumerable<RoomReservations>> GetRoomReservationsByClientPhone(string phoneNumber)
        => await _context.RoomReservation
            .Where(r => r.ClientPhone == phoneNumber)  // Фільтрує бронювання за номером телефону
            .ToListAsync();  // Повертає список бронювань

    //Info: "Отримує бронювання за часом початку"
    public async Task<RoomReservations> GetReservationByTimeReservation(DateTime timeReservation)
        => await _context.RoomReservation
            .FirstOrDefaultAsync(r => r.TimeReservation == timeReservation);  // Повертає перше бронювання за заданим часом

    //Info: "Розраховує вартість бронювання з урахуванням конференц-залу і обраних послуг"
    public async Task<double> CalculateReservationCost(int conferenceRoomId, DateTime reservationStartTime, DateTime reservationEndTime, IEnumerable<int> selectedServicesId)
    {
        var conferenceRoom = await _context.ConferenceRoom.FindAsync(conferenceRoomId);  // Шукає конференц-зал за Id
        if (conferenceRoom == null)  // Перевіряє, чи існує конференц-зал
            throw new ArgumentException("Conference room not found");

        double totalCost = 0;
        var duration = reservationEndTime.Hour - reservationStartTime.Hour;  // Визначає тривалість бронювання

        //Info: "Обчислює вартість залежно від часу бронювання (пік, ранок, вечір)"
        for (var counter = duration; counter > 0; counter--)
            if (reservationStartTime.Hour + counter >= 12 && reservationStartTime.Hour + counter < 14)
            {
                totalCost += conferenceRoom.CostPerHour * 1.15;  // Пікові години
            }
            else if (reservationStartTime.Hour + counter >= 6 && reservationStartTime.Hour + counter < 9)
            {
                totalCost += conferenceRoom.CostPerHour * 0.90;  // Ранкові години
            }
            else if (reservationStartTime.Hour + counter >= 18 && reservationStartTime.Hour + counter < 23)
            {
                totalCost += conferenceRoom.CostPerHour * 0.80;  // Вечірні години
            }
            else
            {
                totalCost += conferenceRoom.CostPerHour;  // Стандартні години
            }

        //Info: "Додає вартість додаткових послуг до загальної суми"
        var selectedServices = new List<AdditionalServices>();
        foreach (var el in selectedServicesId)
        {
            selectedServices.Add(await _context.AdditionalService.FirstOrDefaultAsync(a => a.Id == el));  // Додає кожну вибрану послугу
        }
        if (selectedServices != null && selectedServices.Any() && selectedServices[0] != null)
            totalCost += selectedServices.Sum(service => service.Cost);  // Додає вартість кожної послуги

        return totalCost;
    }

    //Info: "Об'єднує бронювання з конференц-залом і перевіряє всі умови"
    public async Task<(RoomReservations ResultReservation, bool ResultBool)> MergeReservationAndConferenceRoom(RoomReservations reservation, int conferenceRoomId)
    {
        var conferenceRoom = await _context.ConferenceRoom
            .Include(cr => cr.ConferenceRoomsAdditionalServices)  // Завантажує пов'язані додаткові послуги
            .FirstOrDefaultAsync(cr => cr.Id == conferenceRoomId);  // Шукає конференц-зал за Id

        if (conferenceRoom == null)  // Перевірка існування конференц-залу
            return (reservation, false);

        var selectedAdditionalServicesIds = reservation.SelectedServices;

        //Info: "Перевіряє, чи були вибрані додаткові послуги"
        if (selectedAdditionalServicesIds == null || !selectedAdditionalServicesIds.Any())
            return (reservation, false);

        //Info: "Перевіряє, чи всі вибрані послуги доступні в даному конференц-залі"
        var availableServiceIds = conferenceRoom.ConferenceRoomsAdditionalServices
            .Select(cas => cas.AdditionalServiceId).ToList();

        if (availableServiceIds.Any())
            foreach (var additionalServiceId in availableServiceIds)
                if (!conferenceRoom.ConferenceRoomsAdditionalServices.Any(cras => cras.AdditionalServiceId == additionalServiceId))
                    return (reservation, false);

        //Info: "Перевіряє, чи не перетинається бронювання з іншими бронюваннями для того ж залу"
        if (await CheckingIfReservationIsNotOverlapingOther(reservation, conferenceRoomId))
            return (reservation, false);

        //Info: "Оновлює бронювання: додає вибрані послуги і конференц-зал"
        reservation.ReservedRoom = conferenceRoom;
        reservation.SelectedServices = selectedAdditionalServicesIds;

        return (reservation, true);
    }
}

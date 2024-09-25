//Info: "Інтерфейс репозиторію для бронювань залів, що успадковує методи від IRepository"
namespace API.Interfaces
{
    public interface IRoomReservationRepository : IRepository<RoomReservations>
    {
        //Info: "Отримати бронювання за ідентифікатором"
        Task<RoomReservations> GetByIdAsync(int id);

        //Info: "Перевірка, чи не перетинається бронювання з іншими"
        Task<bool> CheckingIfReservationIsNotOverlapingOther(RoomReservations roomReservation, int reservedRoomId);

        //Info: "Отримати бронювання за номером телефону клієнта"
        Task<IEnumerable<RoomReservations>> GetRoomReservationsByClientPhone(string phoneNumber);

        //Info: "Отримати бронювання за часом початку"
        Task<RoomReservations> GetReservationByTimeReservation(DateTime timeReservation);

        //Info: "Розрахувати вартість бронювання, включаючи обрані послуги"
        Task<double> CalculateReservationCost(int conferenceRoomId, DateTime reservationStartTime, DateTime reservationEndTime, IEnumerable<int> selectedServices);

        //Info: "Об'єднати бронювання та конференц-зал"
        Task<(RoomReservations ResultReservation, bool ResultBool)> MergeReservationAndConferenceRoom(RoomReservations reservation, int conferenceRoomId);

        //Info: "Отримати доступні зали за параметрами"
        Task<ICollection<ConferenceRooms>> GetAvailableRooms(DateTime startTime, DateTime endTime, int capacity);
    }
}

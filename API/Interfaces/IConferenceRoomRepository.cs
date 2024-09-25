//Info: "Інтерфейс репозиторію для конференц-залів, що успадковує методи від IRepository"
using API.Entityes;

namespace API.Interfaces
{
    public interface IConferenceRoomRepository : IRepository<ConferenceRooms>
    {
        //Info: "Отримати конференц-зал за назвою"
        Task<ConferenceRooms> GetByNameAsync(string name);

        //Info: "Отримати конференц-зали за місткістю"
        Task<IEnumerable<ConferenceRooms>> GetByCapacityAsync(int capacity);

        //Info: "Отримати конференц-зали за вартістю за годину"
        Task<IEnumerable<ConferenceRooms>> GetByCostPerHour(double costPerHour);

        //Info: "Пошук доступних залів за часом та місткістю"
        Task<IEnumerable<ConferenceRooms>> SearchAvailableRooms(DateTime startTime, DateTime endTime, int capacity);

        //Info: "Отримати доступні додаткові послуги для конкретного конференц-залу"
        Task<IEnumerable<AdditionalServices>> GetAvilableAdditionalServicesById(int conferenceRoomId);

        //Info: "Об'єднати конференц-зал з додатковими послугами за їх ідентифікаторами"
        Task<ConferenceRooms> MergeConferenceRoomAndAdditionalServies(ConferenceRooms conferenceRoom, ICollection<int>? additionalServicesIds);
    }
}

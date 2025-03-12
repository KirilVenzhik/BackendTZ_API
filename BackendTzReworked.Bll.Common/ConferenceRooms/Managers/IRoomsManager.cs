using BackendTzReworked.Bll.Common.Base.Managers;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;

namespace BackendTzReworked.Bll.Common.ConferenceRooms.Managers
{
    public interface IRoomsManager : IBaseManager<MRooms>
    {
        Task<MRooms> GetByNameAsync(string name);
        Task<IEnumerable<MRooms>> GetByCapacityAsync(int capacity);
        Task<IEnumerable<MRooms>> GetByCostPerHourAsync(double costPerHour);
        Task<IEnumerable<MSupplements>> GetAvilableSupplementsByRoomIdAsync(int conferenceRoomId);
        Task<MRooms> MergeRoomAndSupplementsAsync(MRooms conferenceRoom, ICollection<int>? supplementsIds);
    }
}

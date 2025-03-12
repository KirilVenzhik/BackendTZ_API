using BackendTzReworked.Bll.Common.Base.Repositories;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;

namespace BackendTzReworked.Bll.Common.ConferenceRooms.Repositories
{
    public interface IRoomsRepository : IBaseRepository<MRooms>
    {
        Task<MRooms> GetByNameAsync(string name);
        Task<List<MRooms>> GetByCapacityAsync(int capacity);
        Task<IEnumerable<MRooms>> GetByCostPerHourAsync(double costPerHour);
        Task<IEnumerable<MSupplements>> GetAvilableSupplementsByIdAsync(int conferenceRoomId);
        Task<MRooms> MergeConferenceRoomAndAdditionalServiesAsync(MRooms roomModel, ICollection<int>? supplementsIds);
    }
}

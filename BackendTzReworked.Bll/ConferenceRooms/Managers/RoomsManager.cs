using BackendTzReworked.Bll.Common.ConferenceRooms.Managers;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.ConferenceRooms.Repositories;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;

namespace BackendTzReworked.Bll.ConferenceRooms.Managers
{
    public class RoomsManager : IRoomsManager
    {
        public RoomsManager(IRoomsRepository _repository)
        {
            this._repository = _repository;
        }

        private readonly IRoomsRepository _repository;



        public async Task<IEnumerable<MRooms>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<MRooms> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<MRooms> GetByNameAsync(string name)
            => await _repository.GetByNameAsync(name);

        public async Task<IEnumerable<MRooms>> GetByCapacityAsync(int capacity)
            => await _repository.GetByCapacityAsync(capacity);

        public async Task<IEnumerable<MRooms>> GetByCostPerHourAsync(double costPerHour)
            => await _repository.GetByCostPerHourAsync(costPerHour);

        public async Task CreateAsync(MRooms model)
            => await _repository.AddAsync(model);

        public async Task UpdateAsync(MRooms model)
            => await _repository.UpdateAsync(model);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);

        public async Task<bool> SaveAsync()
            => await _repository.Save();



        public async Task<IEnumerable<MSupplements>> GetAvilableSupplementsByRoomIdAsync(int conferenceRoomId)
            => await GetAvilableSupplementsByRoomIdAsync(conferenceRoomId);

        public async Task<MRooms> MergeRoomAndSupplementsAsync(MRooms conferenceRoom, ICollection<int>? supplementsIds)
            => await _repository.MergeConferenceRoomAndAdditionalServiesAsync(conferenceRoom, supplementsIds);
    }
}

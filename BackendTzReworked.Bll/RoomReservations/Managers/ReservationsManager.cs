using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.RoomReservation.Managers;
using BackendTzReworked.Bll.Common.RoomReservation.Models;
using BackendTzReworked.Bll.Common.RoomReservation.Repositories;

namespace BackendTzReworked.Bll.RoomReservations.Managers
{
    public class ReservationsManager : IReservationsManager
    {
        public ReservationsManager(IReservationsRepository _repository)
        {
            this._repository = _repository;
        }

        private readonly IReservationsRepository _repository;



        public async Task<IEnumerable<MReservations>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<MReservations> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<MReservations>> GetRoomReservationsByClientPhoneAsync(string phoneNumber)
            => await _repository.GetReservationsByClientPhone(phoneNumber);

        public async Task<MReservations> GetReservationByTimeReservationAsync(DateTime timeReservation)
            => await _repository.GetReservationByTimeReservation(timeReservation);

        public async Task CreateAsync(MReservations model)
            => await _repository.AddAsync(model);

        public async Task UpdateAsync(MReservations model)
            => await _repository.UpdateAsync(model);

        public async Task DeleteAsync(int id)
            => await _repository.DeleteAsync(id);

        public async Task<bool> SaveAsync()
            => await _repository.Save();



        public async Task<bool> CheckingIfReservationIsNotOverlapingOtherAsync(MReservations reservation, int reservedRoomId)
            => await _repository.CheckingIfReservationIsNotOverlapingOther(reservation, reservedRoomId);



        public async Task<IEnumerable<MRooms>> SearchAvailableRoomsAsync(DateTime startTime, DateTime endTime, int capacity)
            => await _repository.SearchAvailableRoomsAsync(startTime, endTime, capacity);



        public async Task<double> CalculateReservationCostAsync(int conferenceRoomId, DateTime reservationStartTime, DateTime reservationEndTime, IEnumerable<int> selectedServicesId)
            => await _repository.CalculateReservationCost(conferenceRoomId, reservationStartTime, reservationEndTime, selectedServicesId);



        public async Task<(MReservations ResultReservation, bool ResultBool)> MergeReservationAndConferenceRoomAsync(MReservations reservation, int conferenceRoomId)
            => await _repository.MergeReservationAndConferenceRoom(reservation, conferenceRoomId);
    }
}

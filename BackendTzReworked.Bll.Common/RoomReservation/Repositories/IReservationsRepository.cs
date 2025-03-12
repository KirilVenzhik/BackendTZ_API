using BackendTzReworked.Bll.Common.Base.Repositories;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.RoomReservation.Models;

namespace BackendTzReworked.Bll.Common.RoomReservation.Repositories
{
    public interface IReservationsRepository : IBaseRepository<MReservations>
    {
        Task<MReservations> GetByIdAsync(int id);
        Task<IEnumerable<MReservations>> GetReservationsByClientPhone(string phoneNumber);
        Task<MReservations> GetReservationByTimeReservation(DateTime timeReservation);
        Task<IEnumerable<MRooms>> SearchAvailableRoomsAsync(DateTime startTime, DateTime endTime, int capacity);
        Task<double> CalculateReservationCost(int conferenceRoomId, DateTime reservationStartTime, DateTime reservationEndTime, IEnumerable<int> selectedServicesId);
        Task<bool> CheckingIfReservationIsNotOverlapingOther(MReservations reservation, int reservedRoomId);
        Task<(MReservations ResultReservation, bool ResultBool)> MergeReservationAndConferenceRoom(MReservations reservation, int conferenceRoomId);
    }
}

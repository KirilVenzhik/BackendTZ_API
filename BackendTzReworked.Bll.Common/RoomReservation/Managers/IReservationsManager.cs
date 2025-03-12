using BackendTzReworked.Bll.Common.Base.Managers;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.RoomReservation.Models;

namespace BackendTzReworked.Bll.Common.RoomReservation.Managers
{
    public interface IReservationsManager : IBaseManager<MReservations>
    {
        Task<IEnumerable<MReservations>> GetRoomReservationsByClientPhoneAsync(string phoneNumber);
        Task<MReservations> GetReservationByTimeReservationAsync(DateTime timeReservation);
        Task<IEnumerable<MRooms>> SearchAvailableRoomsAsync(DateTime startTime, DateTime endTime, int capacity);
        Task<double> CalculateReservationCostAsync(int conferenceRoomId, DateTime reservationStartTime, DateTime reservationEndTime, IEnumerable<int> selectedServicesId);
        Task<(MReservations ResultReservation, bool ResultBool)> MergeReservationAndConferenceRoomAsync(MReservations reservation, int conferenceRoomId);
    }
}

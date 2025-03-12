using BackendTzReworked.Bll.Common.Base.Models;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;

namespace BackendTzReworked.Bll.Common.RoomReservation.Models
{
    public class MReservations : BaseModel
    {
        public string ClientName { get; set; }

        public string ClientPhone { get; set; }

        public DateTime TimeReservation { get; set; }

        public DateTime EndTimeReservation { get; set; }

        public List<int>? SelectedSupplements { get; set; } = new List<int>();

        public MRooms ReservedRoom { get; set; }
    }
}

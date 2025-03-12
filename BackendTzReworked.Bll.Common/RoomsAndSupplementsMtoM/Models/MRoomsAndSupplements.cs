using BackendTzReworked.Bll.Common.Base.Models;
using BackendTzReworked.Bll.Common.ConferenceRooms.Models;
using BackendTzReworked.Bll.Common.RoomSupplements.Models;

namespace BackendTzReworked.Bll.Common.RoomsAndSupplementsMtoM.Models
{
    public class MRoomsAndSupplements : BaseModel
    {
        public int RoomId { get; set; }
        public MRooms Room { get; set; } = new MRooms();

        public int SupplementId { get; set; }
        public MSupplements Supplement { get; set; } = new MSupplements();
    }
}

using BackendTzReworked.DAL.Base.Entityes;
using BackendTzReworked.DAL.ConferenceRooms.Entityes;
using BackendTzReworked.DAL.RoomSupplements.Entityes;

namespace BackendTzReworked.DAL.RoomsAndSupplementsMtoM.Entityes
{
    public class RoomsAndSupplements : BaseEntity
    {
        public int RoomId { get; set; }
        public Rooms Room { get; set; } = new Rooms();

        public int SupplementId { get; set; }
        public Supplements Supplement { get; set; } = new Supplements();
    }
}

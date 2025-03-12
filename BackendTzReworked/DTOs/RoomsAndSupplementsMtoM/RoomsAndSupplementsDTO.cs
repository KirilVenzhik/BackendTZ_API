using BackendTzReworked.DTOs.ConferenceRooms;
using BackendTzReworked.DTOs.RoomSupplements;

namespace BackendTzReworked.DTOs.RoomsAndSupplementsMtoM
{
    public class RoomsAndSupplementsDTO : BaseDTO
    {
        public int RoomId { get; set; }
        public RoomsDTO ConferenceRoom { get; set; }

        public int SupplementId { get; set; }
        public SupplementsDTO Supplement { get; set; }
    }
}

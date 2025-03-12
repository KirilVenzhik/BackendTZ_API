using BackendTzReworked.Bll.Common.Base.Models;
using BackendTzReworked.Bll.Common.RoomsAndSupplementsMtoM.Models;

namespace BackendTzReworked.Bll.Common.ConferenceRooms.Models
{
    public class MRooms : BaseModel
    {
        public string Name { get; set; }

        public int Capacity { get; set; }

        public double CostPerHour { get; set; }

        public ICollection<MRoomsAndSupplements>? RoomsAndSupplementsList { get; set; } = new List<MRoomsAndSupplements>();

        public ICollection<MRooms>? Reservations { get; set; } = new List<MRooms> { };
    }
}

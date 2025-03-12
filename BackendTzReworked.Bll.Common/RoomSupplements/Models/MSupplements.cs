using BackendTzReworked.Bll.Common.Base.Models;
using BackendTzReworked.Bll.Common.RoomsAndSupplementsMtoM.Models;

namespace BackendTzReworked.Bll.Common.RoomSupplements.Models
{
    public class MSupplements : BaseModel
    {
        public string Name { get; set; }

        public double Cost { get; set; }

        public ICollection<MRoomsAndSupplements>? RoomsAndSupplements { get; set; } = new List<MRoomsAndSupplements>();
    }
}

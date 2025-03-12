using System.ComponentModel.DataAnnotations;
using BackendTzReworked.DAL.Base.Entityes;
using BackendTzReworked.DAL.RoomsAndSupplementsMtoM.Entityes;

namespace BackendTzReworked.DAL.RoomSupplements.Entityes
{
    public class Supplements : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Cost { get; set; }

        public ICollection<RoomsAndSupplements>? RoomsAndSupplements { get; set; } = new List<RoomsAndSupplements>();
    }
}

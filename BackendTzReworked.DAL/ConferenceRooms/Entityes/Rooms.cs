using BackendTzReworked.DAL.Base.Entityes;
using BackendTzReworked.DAL.RoomReservations.Entityes;
using BackendTzReworked.DAL.RoomsAndSupplementsMtoM.Entityes;
using System.ComponentModel.DataAnnotations;

namespace BackendTzReworked.DAL.ConferenceRooms.Entityes
{
    public class Rooms : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required, Range(5, int.MaxValue)]
        public int Capacity { get; set; }

        [Required, Range(100, double.MaxValue)]
        public double CostPerHour { get; set; }

        public ICollection<RoomsAndSupplements>? RoomsAndSupplementsList { get; set; } = new List<RoomsAndSupplements>();

        public ICollection<Reservations>? Reservations { get; set; } = new List<Reservations> { };
    }
}
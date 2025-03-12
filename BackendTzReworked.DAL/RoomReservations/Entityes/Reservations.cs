using System.ComponentModel.DataAnnotations;
using BackendTzReworked.DAL.Base.Entityes;
using BackendTzReworked.DAL.ConferenceRooms.Entityes;

namespace BackendTzReworked.DAL.RoomReservations.Entityes
{
    public class Reservations : BaseEntity
    {
        public string ClientName { get; set; }

        public string ClientPhone { get; set; }

        [Required]
        public DateTime TimeReservation { get; set; }

        [Required]
        public DateTime EndTimeReservation { get; set; }

        public List<int>? SelectedSupplements { get; set; } = new List<int>();

        [Required]
        public Rooms ReservedRoom { get; set; }
    }
}
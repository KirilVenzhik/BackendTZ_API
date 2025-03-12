namespace BackendTzReworked.DTOs.RoomReservations
{
    public class ReservationsDTO
    {
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public DateTime TimeReservation { get; set; }
        public DateTime EndTimeReservation { get; set; }
        public List<int>? SelectedSupplements { get; set; }
    }
}

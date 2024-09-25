//Info: "DTO клас для бронювання залу з інформацією про клієнта, часом та послугами"
namespace API.Dto
{
    public class RoomReservationsDto : BaseEntityDto
    {
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public DateTime TimeReservation { get; set; }
        public DateTime EndTimeReservation { get; set; }
        public List<int>? SelectedServices { get; set; }
    }
}

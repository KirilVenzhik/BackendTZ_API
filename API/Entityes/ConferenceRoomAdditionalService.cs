//Info: "Клас для асоціації конференц-залу та додаткових послуг"
namespace API.Entityes
{
    public class ConferenceRoomAdditionalService
    {
        public int ConferenceRoomId { get; set; }
        public ConferenceRooms ConferenceRoom { get; set; } = new ConferenceRooms();

        public int AdditionalServiceId { get; set; }
        public AdditionalServices AdditionalService { get; set; } = new AdditionalServices();
    }
}

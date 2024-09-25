//Info: "DTO клас для асоціації конференц-залу та додаткових послуг"
namespace API.Dto
{
    public class ConferenceRoomAdditionalServiceDto
    {
        public int ConferenceRoomId { get; set; }
        public ConferenceRoomDto ConferenceRoom { get; set; }

        public int AdditionalServiceId { get; set; }
        public AdditionalServicesDto AdditionalService { get; set; }
    }
}

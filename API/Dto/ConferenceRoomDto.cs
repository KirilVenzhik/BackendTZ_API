//Info: "DTO клас для конференц-залів з назвою, місткістю та вартістю за годину"
namespace API.Dto
{
    public class ConferenceRoomDto : BaseEntityDto
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public double CostPerHour { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace API.Entityes
{
    //Info: "Клас для опису додаткових послуг, успадковує BaseEntity"
    public class AdditionalServices : BaseEntity
    {
        //Info: "Назва послуги, є обов'язковою"
        [Required]
        public string Name { get; set; }

        //Info: "Вартість послуги, є обов'язковою"
        [Required]
        public double Cost { get; set; }

        //Info: "Колекція додаткових послуг конференц-залів, може бути порожньою"
        public ICollection<ConferenceRoomAdditionalService>? ConferenceRoomsAddtitionalServices { get; set; } = new List<ConferenceRoomAdditionalService>();
    }
}

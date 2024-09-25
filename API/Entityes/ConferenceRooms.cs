using API.Entityes;
using System.ComponentModel.DataAnnotations;

public class ConferenceRooms : BaseEntity
{
    //Info: "Назва конференц-залу, є обов'язковою"
    [Required]
    public string Name { get; set; }

    //Info: "Місткість залу, повинна бути не меншою за 5 осіб"
    [Required, Range(5, int.MaxValue)]
    public int Capacity { get; set; }

    //Info: "Вартість оренди залу за годину, повинна бути не меншою за 100"
    [Required, Range(100, double.MaxValue)]
    public double CostPerHour { get; set; }

    //Info: "Колекція додаткових послуг конференц-залу, може бути порожньою"
    public ICollection<ConferenceRoomAdditionalService>? ConferenceRoomsAdditionalServices { get; set; } = new List<ConferenceRoomAdditionalService>();

    //Info: "Колекція бронювань залу, може бути порожньою"
    public ICollection<RoomReservations>? Reservations { get; set; } = new List<RoomReservations> { };
}
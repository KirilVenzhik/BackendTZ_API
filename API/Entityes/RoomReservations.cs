using API.Entityes;
using System.ComponentModel.DataAnnotations;

public class RoomReservations : BaseEntity
{
    //Info: "Ім'я клієнта, який бронює зал"
    public string ClientName { get; set; }

    //Info: "Номер телефону клієнта, який бронює зал"
    public string ClientPhone { get; set; }

    //Info: "Час початку бронювання, є обов'язковим"
    [Required]
    public DateTime TimeReservation { get; set; }

    //Info: "Час завершення бронювання, є обов'язковим"
    [Required]
    public DateTime EndTimeReservation { get; set; }

    //Info: "Список обраних додаткових послуг для залу, може бути порожнім"
    public List<int>? SelectedServices { get; set; } = new List<int>();

    //Info: "Зарезервований конференц-зал, є обов'язковим"
    [Required]
    public ConferenceRooms ReservedRoom { get; set; }
}

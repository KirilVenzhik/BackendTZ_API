using API.Entityes;
using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{
    //Info: "Конструктор для ініціалізації контексту з переданими параметрами"
    public Context(DbContextOptions<Context> options) : base(options) { }

    //Info: "DbSet для конференц-залів"
    public DbSet<ConferenceRooms> ConferenceRoom { get; set; }

    //Info: "DbSet для додаткових послуг"
    public DbSet<AdditionalServices> AdditionalService { get; set; }

    //Info: "DbSet для бронювань залів"
    public DbSet<RoomReservations> RoomReservation { get; set; }

    //Info: "Конфігурація моделі при створенні моделі"
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Info: "Налаштування складеного ключа для ConferenceRoomAdditionalService"
        modelBuilder.Entity<ConferenceRoomAdditionalService>()
            .HasKey(crs => new { crs.ConferenceRoomId, crs.AdditionalServiceId });

        //Info: "Налаштування зв'язку між ConferenceRoom та ConferenceRoomAdditionalService (один до багатьох)"
        modelBuilder.Entity<ConferenceRoomAdditionalService>()
            .HasOne(crs => crs.ConferenceRoom)
            .WithMany(cr => cr.ConferenceRoomsAdditionalServices)
            .HasForeignKey(crs => crs.ConferenceRoomId);

        //Info: "Налаштування зв'язку між AdditionalServices та ConferenceRoomAdditionalService (один до багатьох)"
        modelBuilder.Entity<ConferenceRoomAdditionalService>()
            .HasOne(crs => crs.AdditionalService)
            .WithMany(a => a.ConferenceRoomsAddtitionalServices)
            .HasForeignKey(crs => crs.AdditionalServiceId);
    }
}

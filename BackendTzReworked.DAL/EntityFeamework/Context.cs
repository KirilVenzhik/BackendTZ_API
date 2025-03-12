using Microsoft.EntityFrameworkCore;
using BackendTzReworked.DAL.ConferenceRooms.Entityes;
using BackendTzReworked.DAL.RoomReservations.Entityes;
using BackendTzReworked.DAL.RoomsAndSupplementsMtoM.Entityes;
using BackendTzReworked.DAL.RoomSupplements.Entityes;

namespace BackendTzReworked.DAL.EntityFeamework
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }



        public DbSet<Rooms> Room { get; set; }
        public DbSet<Supplements> Supplement { get; set; }
        public DbSet<Reservations> Reservation { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomsAndSupplements>()
                .HasKey(crs => new { crs.RoomId, crs.SupplementId });

            modelBuilder.Entity<RoomsAndSupplements>()
                .HasOne(crs => crs.Room)
                .WithMany(cr => cr.RoomsAndSupplementsList)
                .HasForeignKey(crs => crs.RoomId);

            modelBuilder.Entity<RoomsAndSupplements>()
                .HasOne(crs => crs.Supplement)
                .WithMany(a => a.RoomsAndSupplements)
                .HasForeignKey(crs => crs.SupplementId);
        }
    }
}

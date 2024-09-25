﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Entityes.AdditionalServices", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("AdditionalService");
                });

            modelBuilder.Entity("API.Entityes.ConferenceRoomAdditionalService", b =>
                {
                    b.Property<int>("ConferenceRoomId")
                        .HasColumnType("int");

                    b.Property<int>("AdditionalServiceId")
                        .HasColumnType("int");

                    b.HasKey("ConferenceRoomId", "AdditionalServiceId");

                    b.HasIndex("AdditionalServiceId");

                    b.ToTable("ConferenceRoomAdditionalService");
                });

            modelBuilder.Entity("API.Entityes.ConferenceRooms", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<double>("CostPerHour")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ConferenceRoom");
                });

            modelBuilder.Entity("API.Entityes.RoomReservations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndTimeReservation")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReservedRoomId")
                        .HasColumnType("int");

                    b.Property<string>("SelectedServices")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeReservation")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ReservedRoomId");

                    b.ToTable("RoomReservation");
                });

            modelBuilder.Entity("API.Entityes.ConferenceRoomAdditionalService", b =>
                {
                    b.HasOne("API.Entityes.AdditionalServices", "AdditionalService")
                        .WithMany("ConferenceRoomsAddtitionalServices")
                        .HasForeignKey("AdditionalServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entityes.ConferenceRooms", "ConferenceRoom")
                        .WithMany("ConferenceRoomsAdditionalServices")
                        .HasForeignKey("ConferenceRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdditionalService");

                    b.Navigation("ConferenceRoom");
                });

            modelBuilder.Entity("API.Entityes.RoomReservations", b =>
                {
                    b.HasOne("API.Entityes.ConferenceRooms", "ReservedRoom")
                        .WithMany("Reservations")
                        .HasForeignKey("ReservedRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReservedRoom");
                });

            modelBuilder.Entity("API.Entityes.AdditionalServices", b =>
                {
                    b.Navigation("ConferenceRoomsAddtitionalServices");
                });

            modelBuilder.Entity("API.Entityes.ConferenceRooms", b =>
                {
                    b.Navigation("ConferenceRoomsAdditionalServices");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}

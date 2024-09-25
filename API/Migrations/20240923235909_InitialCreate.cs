using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    CostPerHour = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceRoom", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConferenceRoomAdditionalService",
                columns: table => new
                {
                    ConferenceRoomId = table.Column<int>(type: "int", nullable: false),
                    AdditionalServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConferenceRoomAdditionalService", x => new { x.ConferenceRoomId, x.AdditionalServiceId });
                    table.ForeignKey(
                        name: "FK_ConferenceRoomAdditionalService_AdditionalService_AdditionalServiceId",
                        column: x => x.AdditionalServiceId,
                        principalTable: "AdditionalService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConferenceRoomAdditionalService_ConferenceRoom_ConferenceRoomId",
                        column: x => x.ConferenceRoomId,
                        principalTable: "ConferenceRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomReservation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeReservation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTimeReservation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SelectedServices = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReservedRoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomReservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomReservation_ConferenceRoom_ReservedRoomId",
                        column: x => x.ReservedRoomId,
                        principalTable: "ConferenceRoom",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceRoomAdditionalService_AdditionalServiceId",
                table: "ConferenceRoomAdditionalService",
                column: "AdditionalServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomReservation_ReservedRoomId",
                table: "RoomReservation",
                column: "ReservedRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConferenceRoomAdditionalService");

            migrationBuilder.DropTable(
                name: "RoomReservation");

            migrationBuilder.DropTable(
                name: "AdditionalService");

            migrationBuilder.DropTable(
                name: "ConferenceRoom");
        }
    }
}

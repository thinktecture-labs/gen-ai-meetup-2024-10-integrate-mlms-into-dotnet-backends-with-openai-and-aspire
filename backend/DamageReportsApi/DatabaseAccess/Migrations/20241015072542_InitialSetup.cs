using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DamageReportsApi.DatabaseAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DamageReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ZipCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    InsuranceId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Telephone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LicensePlate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DateOfAccidentUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccidentType = table.Column<int>(type: "integer", nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ReasonOfTravel = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CarType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CarColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FrontBumper = table.Column<int>(type: "integer", nullable: false),
                    RearBumper = table.Column<int>(type: "integer", nullable: false),
                    Hood = table.Column<int>(type: "integer", nullable: false),
                    TrunkLid = table.Column<int>(type: "integer", nullable: false),
                    Roof = table.Column<int>(type: "integer", nullable: false),
                    FrontLeftDoor = table.Column<int>(type: "integer", nullable: false),
                    FrontRightDoor = table.Column<int>(type: "integer", nullable: false),
                    RearLeftDoor = table.Column<int>(type: "integer", nullable: false),
                    RearRightDoor = table.Column<int>(type: "integer", nullable: false),
                    FrontLeftFender = table.Column<int>(type: "integer", nullable: false),
                    FrontRightFender = table.Column<int>(type: "integer", nullable: false),
                    RearLeftFender = table.Column<int>(type: "integer", nullable: false),
                    RearRightFender = table.Column<int>(type: "integer", nullable: false),
                    Grille = table.Column<int>(type: "integer", nullable: false),
                    LeftHeadlights = table.Column<int>(type: "integer", nullable: false),
                    RightHeadlights = table.Column<int>(type: "integer", nullable: false),
                    LeftTaillights = table.Column<int>(type: "integer", nullable: false),
                    RightTaillights = table.Column<int>(type: "integer", nullable: false),
                    LeftSideMirror = table.Column<int>(type: "integer", nullable: false),
                    RightSideMirror = table.Column<int>(type: "integer", nullable: false),
                    Windshield = table.Column<int>(type: "integer", nullable: false),
                    RearWindshield = table.Column<int>(type: "integer", nullable: false),
                    FrontLeftWindow = table.Column<int>(type: "integer", nullable: false),
                    FrontRightWindow = table.Column<int>(type: "integer", nullable: false),
                    RearLeftWindow = table.Column<int>(type: "integer", nullable: false),
                    RearRightWindow = table.Column<int>(type: "integer", nullable: false),
                    FrontLeftWheel = table.Column<int>(type: "integer", nullable: false),
                    FrontRightWheel = table.Column<int>(type: "integer", nullable: false),
                    RearLeftWheel = table.Column<int>(type: "integer", nullable: false),
                    RearRightWheel = table.Column<int>(type: "integer", nullable: false),
                    LeftExteriorTrim = table.Column<int>(type: "integer", nullable: false),
                    RightExteriorTrim = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherPartyContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DamageReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherPartyContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherPartyContacts_DamageReports_DamageReportId",
                        column: x => x.DamageReportId,
                        principalTable: "DamageReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DamageReportId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passengers_DamageReports_DamageReportId",
                        column: x => x.DamageReportId,
                        principalTable: "DamageReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtherPartyContacts_DamageReportId",
                table: "OtherPartyContacts",
                column: "DamageReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_DamageReportId",
                table: "Passengers",
                column: "DamageReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtherPartyContacts");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "DamageReports");
        }
    }
}

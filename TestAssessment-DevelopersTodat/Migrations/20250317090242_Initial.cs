using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAssessment_DevelopersTodat.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CabData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TpepPickupDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TpepDropoffDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassengerCount = table.Column<int>(type: "int", nullable: false),
                    TripDistance = table.Column<int>(type: "int", nullable: false),
                    StoreAndFwdFlag = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    PULocationId = table.Column<int>(type: "int", nullable: false),
                    DOLocationId = table.Column<int>(type: "int", nullable: false),
                    FareAmount = table.Column<double>(type: "float", nullable: false),
                    TipAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CabData_PULocationId",
                table: "CabData",
                column: "PULocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CabData_StoreAndFwdFlag",
                table: "CabData",
                column: "StoreAndFwdFlag");

            migrationBuilder.CreateIndex(
                name: "IX_CabData_TripDistance",
                table: "CabData",
                column: "TripDistance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CabData");
        }
    }
}

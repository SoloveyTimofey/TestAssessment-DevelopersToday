using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAssessment_DevelopersTodat.Migrations
{
    /// <inheritdoc />
    public partial class RenameDatePropertiesToUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TpepPickupDateTime",
                table: "CabData",
                newName: "TpepPickupDateTimeUtc");

            migrationBuilder.RenameColumn(
                name: "TpepDropoffDateTime",
                table: "CabData",
                newName: "TpepDropoffDateTimeUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TpepPickupDateTimeUtc",
                table: "CabData",
                newName: "TpepPickupDateTime");

            migrationBuilder.RenameColumn(
                name: "TpepDropoffDateTimeUtc",
                table: "CabData",
                newName: "TpepDropoffDateTime");
        }
    }
}

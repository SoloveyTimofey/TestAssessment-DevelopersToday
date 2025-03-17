using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestAssessment_DevelopersTodat.Migrations
{
    /// <inheritdoc />
    public partial class ChnageTripDistanceTypeToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TripDistance",
                table: "CabData",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TripDistance",
                table: "CabData",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}

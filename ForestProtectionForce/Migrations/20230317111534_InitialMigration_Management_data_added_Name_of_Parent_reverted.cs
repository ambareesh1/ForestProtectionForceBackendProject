using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForestProtectionForce.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration_Management_data_added_Name_of_Parent_reverted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "Division");

            migrationBuilder.DropColumn(
                name: "CircleName",
                table: "District");

            migrationBuilder.DropColumn(
                name: "DivisionName",
                table: "Compartment");

            migrationBuilder.DropColumn(
                name: "ProvinceName",
                table: "Circle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "Division",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CircleName",
                table: "District",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DivisionName",
                table: "Compartment",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProvinceName",
                table: "Circle",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

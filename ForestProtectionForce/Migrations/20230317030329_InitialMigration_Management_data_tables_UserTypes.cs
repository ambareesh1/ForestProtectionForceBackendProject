using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForestProtectionForce.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration_Management_data_tables_UserTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "UserTypes",
                newName: "userType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userType",
                table: "UserTypes",
                newName: "UserType");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForestProtectionForce.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration_Management_data_added_Name_of_Parent_reverted_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Compartment",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Compartment",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Division_DistrictId",
                table: "Division",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_District_CircleId",
                table: "District",
                column: "CircleId");

            migrationBuilder.CreateIndex(
                name: "IX_Compartment_DivisionId",
                table: "Compartment",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Circle_ProvinceId",
                table: "Circle",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Circle_Province_ProvinceId",
                table: "Circle",
                column: "ProvinceId",
                principalTable: "Province",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Compartment_Division_DivisionId",
                table: "Compartment",
                column: "DivisionId",
                principalTable: "Division",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_District_Circle_CircleId",
                table: "District",
                column: "CircleId",
                principalTable: "Circle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Division_District_DistrictId",
                table: "Division",
                column: "DistrictId",
                principalTable: "District",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Circle_Province_ProvinceId",
                table: "Circle");

            migrationBuilder.DropForeignKey(
                name: "FK_Compartment_Division_DivisionId",
                table: "Compartment");

            migrationBuilder.DropForeignKey(
                name: "FK_District_Circle_CircleId",
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_Division_District_DistrictId",
                table: "Division");

            migrationBuilder.DropIndex(
                name: "IX_Division_DistrictId",
                table: "Division");

            migrationBuilder.DropIndex(
                name: "IX_District_CircleId",
                table: "District");

            migrationBuilder.DropIndex(
                name: "IX_Compartment_DivisionId",
                table: "Compartment");

            migrationBuilder.DropIndex(
                name: "IX_Circle_ProvinceId",
                table: "Circle");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Compartment",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");
        }
    }
}

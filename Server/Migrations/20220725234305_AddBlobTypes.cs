using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class AddBlobTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BloodTypes",
                column: "Desc",
                values: new object[]
                {
                    "A-",
                    "A+",
                    "AB-",
                    "AB+",
                    "B-",
                    "B+",
                    "O-",
                    "O+"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BloodTypes",
                keyColumn: "Desc",
                keyValue: "A-");

            migrationBuilder.DeleteData(
                table: "BloodTypes",
                keyColumn: "Desc",
                keyValue: "A+");

            migrationBuilder.DeleteData(
                table: "BloodTypes",
                keyColumn: "Desc",
                keyValue: "AB-");

            migrationBuilder.DeleteData(
                table: "BloodTypes",
                keyColumn: "Desc",
                keyValue: "AB+");

            migrationBuilder.DeleteData(
                table: "BloodTypes",
                keyColumn: "Desc",
                keyValue: "B-");

            migrationBuilder.DeleteData(
                table: "BloodTypes",
                keyColumn: "Desc",
                keyValue: "B+");

            migrationBuilder.DeleteData(
                table: "BloodTypes",
                keyColumn: "Desc",
                keyValue: "O-");

            migrationBuilder.DeleteData(
                table: "BloodTypes",
                keyColumn: "Desc",
                keyValue: "O+");
        }
    }
}

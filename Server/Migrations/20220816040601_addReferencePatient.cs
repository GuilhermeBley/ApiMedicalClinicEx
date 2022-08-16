using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class addReferencePatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Patients_BloodType",
                table: "Patients",
                column: "BloodType");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_BloodTypes_BloodType",
                table: "Patients",
                column: "BloodType",
                principalTable: "BloodTypes",
                principalColumn: "Desc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_BloodTypes_BloodType",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_BloodType",
                table: "Patients");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class ClinicTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodTypes",
                columns: table => new
                {
                    Desc = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodTypes", x => x.Desc);
                })
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Cpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    DateCreate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BloodType = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true)
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Cpf);
                })
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Medic = table.Column<int>(type: "int", nullable: false),
                    Patient = table.Column<string>(type: "varchar(11)", nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    DateAppo = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Desc = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    LastAppo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Appointments_LastAppo",
                        column: x => x.LastAppo,
                        principalTable: "Appointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Appointments_AspNetUsers_Medic",
                        column: x => x.Medic,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_Patient",
                        column: x => x.Patient,
                        principalTable: "Patients",
                        principalColumn: "Cpf",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.CreateTable(
                name: "PatientAllergys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cpf = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false)
                        .Annotation("MySql:CharSet", "latin1"),
                    Desc = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAllergys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientAllergys_Patients_Cpf",
                        column: x => x.Cpf,
                        principalTable: "Patients",
                        principalColumn: "Cpf",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_LastAppo",
                table: "Appointments",
                column: "LastAppo");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Medic",
                table: "Appointments",
                column: "Medic");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Patient",
                table: "Appointments",
                column: "Patient");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergys_Cpf",
                table: "PatientAllergys",
                column: "Cpf");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "BloodTypes");

            migrationBuilder.DropTable(
                name: "PatientAllergys");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}

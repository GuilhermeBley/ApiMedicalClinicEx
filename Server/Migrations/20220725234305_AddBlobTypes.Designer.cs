﻿// <auto-generated />
using System;
using ApiMedicalClinicEx.Server.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(AppClinicContext))]
    [Migration("20220725234305_AddBlobTypes")]
    partial class AddBlobTypes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "latin1");

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAppo")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("LastAppo")
                        .HasColumnType("int");

                    b.Property<int>("Medic")
                        .HasColumnType("int");

                    b.Property<string>("Patient")
                        .IsRequired()
                        .HasColumnType("varchar(11)");

                    b.HasKey("Id");

                    b.HasIndex("LastAppo");

                    b.HasIndex("Medic");

                    b.HasIndex("Patient");

                    b.ToTable("Appointments");

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.BloodType", b =>
                {
                    b.Property<string>("Desc")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.HasKey("Desc");

                    b.ToTable("BloodTypes");

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");

                    b.HasData(
                        new
                        {
                            Desc = "O-"
                        },
                        new
                        {
                            Desc = "O+"
                        },
                        new
                        {
                            Desc = "A-"
                        },
                        new
                        {
                            Desc = "A+"
                        },
                        new
                        {
                            Desc = "B-"
                        },
                        new
                        {
                            Desc = "B+"
                        },
                        new
                        {
                            Desc = "AB-"
                        },
                        new
                        {
                            Desc = "AB+"
                        });
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.Patient", b =>
                {
                    b.Property<string>("Cpf")
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.Property<string>("BloodType")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)");

                    b.Property<DateTime?>("DateCreate")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Cpf");

                    b.ToTable("Patients");

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.PatientAllergy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.Property<string>("Desc")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("Cpf");

                    b.ToTable("PatientAllergys");

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("IdDoctor")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.UserToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "latin1");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.Appointment", b =>
                {
                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.Appointment", "LastAppointiment")
                        .WithMany()
                        .HasForeignKey("LastAppo");

                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.User", "MedicUser")
                        .WithMany()
                        .HasForeignKey("Medic")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.Patient", "PatientAppo")
                        .WithMany()
                        .HasForeignKey("Patient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LastAppointiment");

                    b.Navigation("MedicUser");

                    b.Navigation("PatientAppo");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.PatientAllergy", b =>
                {
                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("Cpf")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.RoleClaim", b =>
                {
                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.UserClaim", b =>
                {
                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.UserLogin", b =>
                {
                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.UserRole", b =>
                {
                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApiMedicalClinicEx.Server.Context.Model.UserToken", b =>
                {
                    b.HasOne("ApiMedicalClinicEx.Server.Context.Model.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

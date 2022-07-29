using ApiMedicalClinicEx.Server.Context.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace ApiMedicalClinicEx.Server.Context;

public class AppClinicContext : IdentityDbContext<User, Role, int,
                                            UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    private readonly IConfiguration _config;

    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<BloodType> BloodTypes { get; set; } = null!;
    public DbSet<PatientAllergy> PatientAllergys { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;

    public AppClinicContext(IConfiguration config)
    {
        _config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        base.OnConfiguring(optionsBuilder);
        
        optionsBuilder.UseMySql(
            _config.GetConnectionString("Clinic"),
            ServerVersion.AutoDetect(_config.GetConnectionString("Clinic"))
        );
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        
        builder.HasCharSet("latin1");

        // identity
        builder.Entity<User>().HasCharSet("latin1");
        builder.Entity<Role>().HasCharSet("latin1");
        builder.Entity<UserToken>().HasCharSet("latin1");
        builder.Entity<UserLogin>().HasCharSet("latin1");
        builder.Entity<RoleClaim>().HasCharSet("latin1");
        builder.Entity<UserRole>().HasCharSet("latin1");
        builder.Entity<UserClaim>().HasCharSet("latin1");

        // Clinic tables
        builder.Entity<Patient>().HasCharSet("latin1");
        builder.Entity<BloodType>().HasCharSet("latin1");
        builder.Entity<PatientAllergy>().HasCharSet("latin1");
        builder.Entity<Appointment>().HasCharSet("latin1");

        // Add data
        builder.Entity<BloodType>().HasData(new BloodType{Desc="O-"});
        builder.Entity<BloodType>().HasData(new BloodType{Desc="O+"});
        builder.Entity<BloodType>().HasData(new BloodType{Desc="A-"});
        builder.Entity<BloodType>().HasData(new BloodType{Desc="A+"});
        builder.Entity<BloodType>().HasData(new BloodType{Desc="B-"});
        builder.Entity<BloodType>().HasData(new BloodType{Desc="B+"});
        builder.Entity<BloodType>().HasData(new BloodType{Desc="AB-"});
        builder.Entity<BloodType>().HasData(new BloodType{Desc="AB+"});

        base.OnModelCreating(builder);
    }
}
using ApiMedicalClinicEx.Server.Context.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace ApiMedicalClinicEx.Server.Context;

internal class AppClinicContext : IdentityDbContext<User, Role, int,
                                            UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    private readonly IConfiguration _config;

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

        base.OnModelCreating(builder);
    }
}
using ApiMedicalClinicEx.Server.Context;
using ApiMedicalClinicEx.Server.Extension;
using ApiMedicalClinicEx.Server.Context.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ApiMedicalClinicEx.Server.Services;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithConfig();

#region Add Services

builder.Services
    .AddSingleton<IConfiguration>(builder.Configuration)
    .AddSingleton<IClaimTypeService, ClaimTypeService>()
    .AddSingleton<IBloodTypesService, BloodTypesService>()
    .AddScoped<IAuthService, AuthService>()
    .AddScoped<IPatientService, PatientService>()
    .AddScoped<IAppointmentService, AppointmentService>()
    .AddTransient<ICurrentlyUserService, CurrentlyUserService>();

#endregion

#region Context

builder.Services.AddDbContext<AppClinicContext>();

#endregion

#region Identity

builder.Services.AddIdentity<User, Role>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Sign
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = System.TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

})
.AddEntityFrameworkStores<AppClinicContext>()
.AddUserManager<UserManager<User>>()
.AddSignInManager<SignInManager<User>>();

#endregion

#region Authentication

var key = System.Text.Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    })
    .AddJwtBearer(optionsBearer =>
    {
        optionsBearer.RequireHttpsMetadata = false;
        optionsBearer.SaveToken = true;
        optionsBearer.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

#endregion

#region Authorization

builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy(ClaimTypeService.Policy.OnlyAdm, police => police.RequireRole(ClaimTypeService.Claim.Admin));
    }
);

#endregion

#region Mapper

builder.Services.AddAutoMapper(typeof(Program));

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using System.Text;
using EducationalWebsite.Application;
using EducationalWebsite.Application.Services;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Domain.Interfaces.Users;
using EducationalWebsite.Infrastructure.Authentication;
using EducationalWebsite.Infrastructure.Identity;
using EducationalWebsite.Infrastructure.Jwt;
using EducationalWebsite.Infrastructure.MongoDB;
using EducationalWebsite.Infrastructure.Notification;
using EducationalWebsite.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Google.Apis.Gmail.v1;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConnection>(builder.Configuration.GetSection("MongoDbConnection"));

// Configure MongoDB Identity
builder.Services.AddIdentityExtensions(builder.Configuration);


builder.Services.AddJwtAuthentication(builder.Configuration); // Add JWT authentication
builder.Services.AddGoogleAuthentication(builder.Configuration); // Add Google authentication
builder.Services.AddSingleton<MongoDatabaseManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IRoleManagementService, RoleManagementService>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSingleton<GmailServiceInitializer>();
builder.Services.AddHttpContextAccessor();



// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "EducationalWebsite API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }});
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EducationalWebsite API v1"));
app.MapControllers();

// Create default roles and users
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await CreateRolesAndUsers(roleManager, userManager, builder.Configuration);
}

app.Run();

static async Task CreateRolesAndUsers(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
{
    string[] roleNames = { "ADMIN", "USER" };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            roleResult = await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
        }
    }

    // Create default Admin user
    var adminUser = new ApplicationUser
    {
        UserName = "adminUser",
        Email = configuration["DefaultAdmin:Email"],
        FirstName = "Admin",
        LastName = "User",
        EmailConfirmed = true
    };

    var adminPassword = configuration["DefaultAdmin:Password"];
    var adminExist = await userManager.FindByEmailAsync(adminUser.Email);

    if (adminExist == null)
    {
        var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
        if (createAdminUser.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "ADMIN");
        }
    }

    // Create default regular user
    var regularUser = new ApplicationUser
    {
        UserName = "regularUser",
        Email = configuration["DefaultUser:Email"],
        FirstName = "Regular",
        LastName = "User",
        EmailConfirmed = true
    };

    var userPassword = configuration["DefaultUser:Password"];
    var userExist = await userManager.FindByEmailAsync(regularUser.Email);

    if (userExist == null)
    {
        var createRegularUser = await userManager.CreateAsync(regularUser, userPassword);
        if (createRegularUser.Succeeded)
        {
            await userManager.AddToRoleAsync(regularUser, "USER");
        }
    }
}

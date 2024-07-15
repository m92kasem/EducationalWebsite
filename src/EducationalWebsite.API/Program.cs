using System.Text;
using EducationalWebsite.Application;
using EducationalWebsite.Domain.Entities;
using EducationalWebsite.Infrastructure.Jwt;
using EducationalWebsite.Infrastructure.MongoDB;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConnection>(builder.Configuration.GetSection("MongoDbConnection"));

// Configure MongoDB Identity
builder.Services.AddIdentity<User, ApplicationRole>()
    .AddMongoDbStores<User, ApplicationRole, Guid>(
        builder.Configuration["MongoDbConnection:ConnectionString"], 
        builder.Configuration["MongoDbConnection:DatabaseName"])
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
// Add JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "EducationalWebsite API", Version = "v1" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
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
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    await CreateRolesAndUsers(roleManager, userManager);
}

app.Run();








static async Task CreateRolesAndUsers(RoleManager<ApplicationRole> roleManager, UserManager<User> userManager)
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
    var adminUser = new User
    {
        UserName = "admin@domain.com",
        Email = "admin@domain.com",
        FirstName = "Admin",
        LastName = "User",
        EmailConfirmed = true
    };

    var adminPassword = "Admin@123"; // Use a strong password
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
    var regularUser = new User
    {
        UserName = "user@domain.com",
        Email = "user@domain.com",
        FirstName = "Regular",
        LastName = "User",
        EmailConfirmed = true
    };

    var userPassword = "User@123"; // Use a strong password
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
﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShootyGameAPI.Authorization;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Repositorys;
using ShootyGameAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IWeaponTypeRepository, WeaponTypeRepository>();
builder.Services.AddScoped<IWeaponTypeService, WeaponTypeService>();

builder.Services.AddScoped<IWeaponRepository, WeaponRepository>();
builder.Services.AddScoped<IWeaponService, WeaponService>();

builder.Services.AddScoped<IScoreRepository, ScoreRepository>();
builder.Services.AddScoped<IScoreService, ScoreService>();

builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IFriendReqService, FriendReqService>();

builder.Services.AddScoped<IUserWeaponRepository, UserWeaponRepository>();

builder.Services.AddScoped<IFriendRepository, FriendRepository>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var appSettings = new AppSettings();

if (builder.Environment.IsDevelopment())
{
    // In development, allow secret from appsettings.json
    builder.Configuration.GetSection("AppSettings").Bind(appSettings);

    // If a secret is found in the environment, override the one from appsettings.json
    var secretFromEnv = builder.Configuration["ShootyGameAPI_JWT_SECRET"];
    if (!string.IsNullOrWhiteSpace(secretFromEnv))
    {
        appSettings.Secret = secretFromEnv;
        Console.WriteLine("JWT secret from Env found (overriding appsettings.json)");
    }
    else
    {
        Console.WriteLine("JWT secret loaded from appsettings.json");
    }
}

// Register the configured AppSettings into DI
builder.Services.Configure<AppSettings>(options =>
{
    options.Secret = appSettings.Secret;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShootyGameAPI", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();

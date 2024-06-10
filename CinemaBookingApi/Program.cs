using Microsoft.EntityFrameworkCore;
using CinemaBookingApi.Data;
using CinemaBookingApi.Entites;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "CinemaBookingApi", Version = "v1" });
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme.",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };
        c.AddSecurityDefinition("Bearer", securityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                securityScheme,
                Array.Empty<string>()
            }
        });
    });


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var token = context.SecurityToken as JwtSecurityToken;
            if (token != null)
            {
                var exp = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                context.HttpContext.Response.Headers.Add("Token-Expires", exp);
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };



        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:7069", // builder.Configuration.GetValue 
            ValidAudience = "http://localhost:7069",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cine#.m+aDbJw28tAuthe?#nticaitonKey"))
        };
    });

builder.Services.AddDbContext<CinemaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("cinemadb")));

builder.Services.AddStackExchangeRedisCache(opt => 
    opt.Configuration = builder.Configuration.GetConnectionString("Cache"));

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

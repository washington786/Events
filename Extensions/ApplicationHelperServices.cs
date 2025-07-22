using System;
using System.Text;
using Events.Data;
using Events.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Events.Extensions;

public class ApplicationHelperServices(IServiceCollection _services, IConfiguration _configuration)
{
    private readonly IServiceCollection services = _services;
    private readonly IConfiguration configuration = _configuration;

    public IServiceCollection AddApplicationServices()
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String Invalid!");

        // injecting database context
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

        // injecting identity for roles
        services.AddIdentity<Users, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

        // configuring session
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        // configuring cors
        services.AddCors(options =>
        {
            options.AddPolicy("Prod", (builder) => builder.WithOrigins("https://production-app.com").AllowAnyHeader().AllowAnyMethod());

            options.AddPolicy("dev", (builder) => builder.WithOrigins("https://localhost:7345").AllowAnyHeader().AllowAnyMethod());
        });

        services.AddAuthentication("Bearer").AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
            };
        });

        services.AddControllers();
        return services;
    }

}

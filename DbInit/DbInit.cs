using System;
using Events.Data;
using Events.Models;
using Events.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Events.DbInit;

public class DbInit(ApplicationDbContext dbContext,
    RoleManager<IdentityRole> roleManager,
    UserManager<Users> userManager) : IDbInit
{

    private readonly ApplicationDbContext _dbCtx = dbContext;
    private readonly RoleManager<IdentityRole> _roleManger = roleManager;
    private readonly UserManager<Users> _userManger = userManager;


    public void Initialize()
    {
        try
        {
            if (_dbCtx.Database.GetPendingMigrations().Count() > 0)
            {
                _dbCtx.Database.Migrate();
            }

        }
        catch (System.Exception ex)
        {
            throw new Exception($"Error: {ex}");
        }
        if (_dbCtx.Roles.Any(r => r.Name == Helper.Admin)) return;

        _roleManger.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
        _roleManger.CreateAsync(new IdentityRole(Helper.Client)).GetAwaiter().GetResult();
        _roleManger.CreateAsync(new IdentityRole(Helper.Doctor)).GetAwaiter().GetResult();

        _userManger.CreateAsync(new Users
        {
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            EmailConfirmed = true,
            Name = "Super Admin"
        }, "admin@12").GetAwaiter().GetResult();

        var user = _dbCtx.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
        _userManger.AddToRoleAsync((Users)user!, Helper.Admin).GetAwaiter().GetResult();
    }
}

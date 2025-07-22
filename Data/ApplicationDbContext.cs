using System;
using Events.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Events.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) : IdentityDbContext(dbContext)
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    public DbSet<Appointments> Appointments { get; set; }

}

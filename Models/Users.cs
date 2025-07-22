using System;
using Microsoft.AspNetCore.Identity;

namespace Events.Models;

public class Users : IdentityUser
{
    public string Name { get; set; } = null!;
}

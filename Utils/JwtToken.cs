using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Events.Models;
using Microsoft.IdentityModel.Tokens;

namespace Events.Utils;

public class JwtToken(IConfiguration _configuration)
{
    private readonly IConfiguration configuration = _configuration;

    public string CreateJwtToken(Users user)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        var Claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Email, user.Email!) };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: Claims,
            expires: DateTime.Now.AddHours(4),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token).ToString() ?? throw new Exception("Couldn't create a token");
    }
}

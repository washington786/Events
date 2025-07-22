using System.ComponentModel.DataAnnotations;

namespace Events.Dtos.auth;

public record class RegisterDto()
{
    [Required]
    [StringLength(100)]
    public string Name { get; init; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; init; } = null!;

    [Required]
    [Compare("Password", ErrorMessage = "Passwords mismatch!")]
    public string ConfirmPassword { get; init; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;



    [Required]
    public string Role { get; init; } = null!;
}

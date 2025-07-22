using System.ComponentModel.DataAnnotations;

namespace Events.Dtos.auth;

public record class LoginDto([Required][EmailAddress] string Email, [Required][DataType(DataType.Password)] string Password, [Display(Name = "Remember me")] bool IsRemember)
{

}

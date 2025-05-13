namespace ColorsApi.Dtos;

/// <summary>
/// Pour l'inscription et le login.
/// </summary>
public class RegisterUserDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
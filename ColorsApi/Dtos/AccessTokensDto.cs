namespace ColorsApi.Dtos;

/// <summary>
/// Pour retourner le JWT.
/// </summary>
public class AccessTokensDto
{
    public required string AccessToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
}
using System.Security.Claims;
using System.Text;
using ColorsApi.DataBase;
using ColorsApi.Dtos;
using ColorsApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ColorsApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ColorsDbContext _colorsDbContext;
    private readonly JwtAuthOptions _jwtOptions;

    public AuthController(
        UserManager<IdentityUser> userManager,
        ColorsDbContext colorsDbContext,
        IOptions<JwtAuthOptions> jwtOptions)
    {
        _userManager = userManager;
        _colorsDbContext = colorsDbContext;
        _jwtOptions = jwtOptions.Value;
    }

    [HttpPost("access-token")]
    public async Task<ActionResult<AccessTokensDto>> RegisterUser(RegisterUserDto registerUserDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerUserDto.Email,
            Email = registerUserDto.Email
        };
        
        IdentityResult createUserResult = await _userManager.CreateAsync(identityUser, registerUserDto.Password);

        if (!createUserResult.Succeeded)
        {
            return BadRequest(createUserResult.Errors);
        }

        var user = new ColorUserEntity
        {
            IdentityId = identityUser.Id
        };

        _colorsDbContext.Users.Add(user);
        await _colorsDbContext.SaveChangesAsync();
        
        var accessToken = CreateToken(identityUser.Id, identityUser.Email);
        
        return Ok(new AccessTokensDto
        {
            AccessToken = accessToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes)
        });
    }

    [HttpPut("access-token")]
    public async Task<ActionResult<AccessTokensDto>> LoginUser(RegisterUserDto loginUserDto)
    {
        var identityUser = await _userManager.FindByEmailAsync(loginUserDto.Email);
        if (identityUser == null || !await _userManager.CheckPasswordAsync(identityUser, loginUserDto.Password))
        {
            return Unauthorized("Email ou mot de passe invalide");
        }

        var accessToken = CreateToken(identityUser.Id, identityUser.Email);
        return Ok(new AccessTokensDto
        {
            AccessToken = accessToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes)
        });
    }
    
    private string CreateToken(string userId, string? email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(ClaimTypes.NameIdentifier, userId) // Pour ColorPaletteController
        };
        
        if (!string.IsNullOrEmpty(email))
        {
            claims.Add(new(JwtRegisteredClaimNames.Email, email));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        var accessToken = handler.WriteToken(token);
        
        return accessToken;
    }
}
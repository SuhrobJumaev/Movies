using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.BusinessLogic.Helpers;
using Movies.DataAccess;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.BusinessLogic;

public class IdentityService : IIdentityService
{
    private readonly IOptions<JwtSettings> _options;
    private readonly IValidator<LoginDto> _validator;
    private readonly IValidator<RegistrationDto> _registrationValidator;
    private readonly IUserRepository _userRepository;

    public IdentityService(
        IOptions<JwtSettings> options, 
        IValidator<LoginDto> validator,
        IValidator<RegistrationDto> registrationValidator,
        IUserRepository userRepository
        )
    {
        _options = options;
        _validator = validator;
        _userRepository = userRepository;
        _registrationValidator = registrationValidator;
    }

    public string GenerateToken(UserDtoResponse? user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var key = Encoding.UTF8.GetBytes(_options.Value.Key);

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Sub, user.Value.Email),
            new (JwtRegisteredClaimNames.Email, user.Value.Email),
            new (ClaimTypes.Role , user.Value.RoleName),
            new ("UserId", user.Value.Id.ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(_options.Value.TokenLifeTime),
            Issuer = _options.Value.Issuer,
            Audience = _options.Value.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        string jwtToken = tokenHandler.WriteToken(token); 

        return jwtToken;
    }

    public async Task<UserDtoResponse?> GetUserByEmailAndPasswordAsync(LoginDto loginDto, CancellationToken token = default)
    {
        _validator.Validate(loginDto, opt => opt.ThrowOnFailures());

        loginDto.Password = PasswordHashHelper.PasswordHash(loginDto.Password, loginDto.Email);

        User user = await _userRepository.GetUserByEmailAndPasswordAsync(loginDto.Email, loginDto.Password, token);

        if (user is null)
            return null;

        return user.UserToResponseDto();
    }

    public async Task<UserDtoResponse> RegistrationUser(RegistrationDto registrationDto, CancellationToken token = default)
    {
        await _registrationValidator.ValidateAndThrowAsync(registrationDto);

        registrationDto.Password = PasswordHashHelper.PasswordHash(registrationDto.Password, registrationDto.Email);

        int userId = await _userRepository.CreateAsync(registrationDto.RegistrationDtoToUser(), token);
        
        User createdUser = await _userRepository.GetAsync(userId);

        return createdUser.UserToResponseDto();
    }
}

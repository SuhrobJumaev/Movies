﻿using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.BusinessLogic.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Movies.BusinessLogic;

public class IdentityService : IIdentityService
{
    private readonly IOptions<JwtSettings> _options;
    private readonly IValidator<LoginDto> _validator;
    private readonly IUserService _userService;

    public IdentityService(
        IOptions<JwtSettings> options, 
        IValidator<LoginDto> validator, 
        IUserService userService)
    {
        _options = options;
        _validator = validator;
        _userService = userService;

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
            new ("UserId", user.Value.Id.ToString()),
            new ("Role", user.Value.RoleName),
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
}
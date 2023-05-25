using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebRecipesApi.Domain;

namespace WebRecipesApi.BusinessLogic
{
    public class JWTService
    {
        private readonly UserService _userService;

        public JWTService(UserService userService)
        {
            _userService = userService;
        }

        public string CreateJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("NxgW6KsCk6O0XpQdvnuy16jfRk5ceag9ZhjgERymnhI=");

            // Create claims for the user
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, $"{user.Email}"),
                new Claim("id", user.Id.ToString(), ClaimValueTypes.Integer)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return jwtTokenHandler.WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            // Generate a random refresh token
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            // Check if the token already exists in the user
            var tokenInUser = _userService.RefreshTokenExists(refreshToken);
            if (tokenInUser)
            {
                // If the token already exists, generate a new one
                return CreateRefreshToken();
            }

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("NxgW6KsCk6O0XpQdvnuy16jfRk5ceag9ZhjgERymnhI=");

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            // Check if the token is valid and properly signed
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is an invalid token");

            return principal;
        }
    }
}

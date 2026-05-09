<<<<<<< HEAD
﻿using Microsoft.IdentityModel.Tokens;
=======
using Microsoft.IdentityModel.Tokens;
>>>>>>> origin/master
using ProiectII.Interfaces;
using ProiectII.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

<<<<<<< HEAD

namespace ProiectII.Services.SecurityIdentity
{


=======
namespace ProiectII.Services.SecurityIdentity
{
>>>>>>> origin/master
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config) => _config = config;
<<<<<<< HEAD
        //new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName)
        public string CreateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim> {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };
            foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
=======

        public string CreateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
                new Claim(ClaimTypes.Name, user.Email ?? user.UserName ?? "User"), // This is what @User.Identity.Name looks for
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add roles
            foreach (var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secret = _config["JWT:Secret"] ?? "Super_Secret_Key_At_Least_64_Characters_Long_1234567890_!@#$%^&*()";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Using Sha256 for better compatibility
>>>>>>> origin/master

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds,
<<<<<<< HEAD
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
=======
                Issuer = _config["JWT:ValidIssuer"],
                Audience = _config["JWT:ValidAudience"]
>>>>>>> origin/master
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
<<<<<<< HEAD

=======
>>>>>>> origin/master
}

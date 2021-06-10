using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using TaskPlanner.Repositories.Interfaces;
using TaskPlanner.Utilities.Interfaces;

namespace TaskPlanner.Utilities.Implementations
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        // nuget Microsoft.AspNetCore.Authentication is needed
        // nuget System.IdentityModel.Tokens.Jwt is needed

        private readonly IUserRepo _userRepo;
        private readonly IHashManager _hashManager;

        public JwtAuthenticationManager(IUserRepo userRepo,
                                        IHashManager hashManager)
        {
            _userRepo = userRepo;
            _hashManager = hashManager;
        }

        public async Task<string> Authenticate(string email, string password)
        {

            var user = await _userRepo.GetByEmailAsync(email);
            if (user is null) return null;
            if (!_hashManager.CompareAgainstHash(password, user.PasswordHash)) return null;
            
            return GetToken(email);
        }

        public string GetToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(KeyHolder.GetKey());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}

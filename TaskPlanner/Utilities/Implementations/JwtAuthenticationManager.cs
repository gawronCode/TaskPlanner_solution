using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using TaskPlanner.Utilities.Interfaces;

namespace TaskPlanner.Utilities.Implementations
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        // nuget Microsoft.AspNetCore.Authentication is needed
        // nuget System.IdentityModel.Tokens.Jwt is needed

        private readonly IDictionary<string, string> _users = new Dictionary<string, string>
            {{"user1", "password1"}, {"user2", "password2"}, {"user3", "password3"} };

        private readonly string _key;
        public JwtAuthenticationManager(string key)
        {
            _key = key;
        }

        public string Authenticate(string username, string password)
        {

            if (!_users.ContainsKey(username)) return null;
            if (_users[username] != password) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
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

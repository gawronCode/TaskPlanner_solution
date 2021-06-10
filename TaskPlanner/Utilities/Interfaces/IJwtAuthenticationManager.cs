using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace TaskPlanner.Utilities.Interfaces
{
    public interface IJwtAuthenticationManager
    {
        public Task<string> Authenticate(string email, string password);
        public string GetToken(string email);
    }
}

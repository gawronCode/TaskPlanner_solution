using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.Repositories.Interfaces;
using TaskPlanner.Utilities.Interfaces;

namespace TaskPlanner.Utilities.Implementations
{
    public class HashManager : IHashManager
    {

        

        public string CreateHash(string password)
        {
            var sha256 = SHA256.Create();
            var hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            foreach (var c in hashValue)
            {
                builder.Append(c.ToString("x2"));
            }

            return builder.ToString();

        }


        public bool CompareAgainstHash(string password, string dbHash)
        {
            var passwordHash = CreateHash(password);
            return passwordHash == dbHash;
        }


    }
}

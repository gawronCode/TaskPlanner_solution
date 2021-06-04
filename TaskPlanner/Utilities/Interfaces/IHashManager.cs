using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPlanner.Utilities.Interfaces
{
    public interface IHashManager
    {
        public string CreateHash(string password);
        public bool CompareAgainstHash(string password, string dbHash);
    }
}

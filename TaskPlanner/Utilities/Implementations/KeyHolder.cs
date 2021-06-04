using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.Utilities.Interfaces;

namespace TaskPlanner.Utilities.Implementations
{
    public static class KeyHolder
    {
        public static string GetKey()
        {
            return "very-complex-password";
        }
    }
}

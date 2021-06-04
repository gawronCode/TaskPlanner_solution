using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.Models.DbModels;

namespace TaskPlanner.Repositories.Interfaces
{
    public interface IUserRepo : IGeneralRepo<User>
    {
        public Task<User> GetByEmailAsync(string email);
    }
}

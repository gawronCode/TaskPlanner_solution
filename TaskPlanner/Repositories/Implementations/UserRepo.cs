using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskPlanner.Data;
using TaskPlanner.Models.DbModels;
using TaskPlanner.Repositories.Interfaces;
using Task = TaskPlanner.Models.DbModels.Task;

namespace TaskPlanner.Repositories.Implementations
{
    public class UserRepo : IUserRepo
    {
        private readonly TaskPlannerDbContext _context;

        public UserRepo(TaskPlannerDbContext context)
        {
            _context = context;
        }


        public async Task<bool> CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(User entity)
        {
            _context.Users.Remove(entity);
            return await SaveAsync();
        }

        public Task<ICollection<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(q => q.Email == email);
            return user;
        }

        public async Task<bool> SaveAsync()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public Task<bool> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}

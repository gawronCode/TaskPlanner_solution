using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskPlanner.Data;
using TaskPlanner.Models.DbModels;
using TaskPlanner.Repositories.Interfaces;

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

        public async Task<ICollection<User>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
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

        public async Task<bool> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            return await SaveAsync();
        }
    }
}

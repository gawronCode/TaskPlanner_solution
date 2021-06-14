﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskPlanner.Data;
using TaskPlanner.Models.DbModels;
using TaskPlanner.Repositories.Interfaces;

namespace TaskPlanner.Repositories.Implementations
{
    public class EventRepo : IEventRepo
    {

        private readonly TaskPlannerDbContext _context;

        public EventRepo(TaskPlannerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Event entity)
        {
            await _context.Events.AddAsync(entity);
            return await SaveAsync();

        }

        public Task<bool> DeleteAsync(Event entity)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Event>> GetAllAsync()
        {
            var events = await _context.Events.ToListAsync();
            return events;
        }

        public async Task<ICollection<Event>> GetAllByUserIdAsync(int userId)
        {
            var userEvents = await _context.Events.Where(q => q.UserId == userId).ToListAsync();
            return userEvents;
        }

        public Task<Event> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAsync()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public Task<bool> UpdateAsync(Event entity)
        {
            throw new NotImplementedException();
        }
    }
}

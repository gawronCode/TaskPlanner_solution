using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskPlanner.Models.DbModels;
using Task = TaskPlanner.Models.DbModels.Task;

namespace TaskPlanner.Data
{
    public class TaskPlannerDbContext : DbContext
    {
        public TaskPlannerDbContext(DbContextOptions<TaskPlannerDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskWork> TaskWorks { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(entity => new { entity.Email }).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

    }
}

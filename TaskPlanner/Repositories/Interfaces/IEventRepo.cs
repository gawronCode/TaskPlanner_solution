using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.Models.DbModels;


namespace TaskPlanner.Repositories.Interfaces
{
    public interface IEventRepo : IGeneralRepo<Event>
    {
        public Task<ICollection<Event>> GetAllByUserIdAsync(int userId);
    }
}

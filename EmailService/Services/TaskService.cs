using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EmailService.Models;

namespace EmailService.Services
{
    public class TaskService
    {
        public ApplicationDbContext db;

        public TaskService()
        {
            db = new ApplicationDbContext();
        }

        public async Task<IEnumerable<Tasks>> GetAll()
        {
            var tasks = await db.Tasks.ToListAsync();
            return tasks;
        }
    }
}
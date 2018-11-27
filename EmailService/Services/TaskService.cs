using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public int GetCount()
        {
            return db.Tasks.Count();
        }

        public async Task<IEnumerable<Tasks>> GetAll(int from, int count)
        {
            var tasks = await db.Tasks.AsQueryable().OrderByDescending(x => x.TaskId).Skip(from).Take(count).ToListAsync();
            return tasks;
        }
    }
}
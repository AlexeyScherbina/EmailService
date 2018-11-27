using System.Data.Entity;
using System.Configuration;

namespace EmailService.Models
{
    //ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }


        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
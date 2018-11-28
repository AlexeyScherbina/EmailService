using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmailService.Models
{
    public class User
    {
        public User()
        {
            if (Tasks == null)
            {
                Tasks = new List<Tasks>();
            }
        }

        [Key]
        public int UserId { get; set; }

        public string FullName { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
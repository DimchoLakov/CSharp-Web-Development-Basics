using System;
using System.Collections.Generic;

namespace ByTheCakeApp.Models
{
    public class User : BaseModel<int>
    {
        public User()
        {
            this.Orders = new HashSet<Order>();
            this.DateOfRegistration = DateTime.UtcNow;
        }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateOfRegistration { get; set; }

        public virtual ICollection<Order> Orders { get; }
    }
}

using System;
using System.Collections.Generic;

namespace ByTheCakeApp.Models
{
    public class Order : BaseModel<int>
    {
        public Order()
        {
            this.OrderProducts = new HashSet<OrderProduct>();
            this.DateOfCreation = DateTime.UtcNow;
        }

        public DateTime DateOfCreation { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

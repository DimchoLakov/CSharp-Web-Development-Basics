using System.Collections.Generic;

namespace ByTheCakeApp.Models
{
    public class Product : BaseModel<int>
    {
        public Product()
        {
            this.ProductOrders = new HashSet<OrderProduct>();
        }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<OrderProduct> ProductOrders { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual ShopUser User { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Amount { get; set; }

        public DateTime OrderDate { get; set; }
        public bool IsFramed { get; set; }
    }
}

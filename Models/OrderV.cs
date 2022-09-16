using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Models
{
    public class OrderV
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }

        public int Price { get; set; }

        public int Amount { get; set; }
        public int TotalPrice { get; set; }
        public bool IsFramed { get; set; }

        public DateTime OrderDate { get; set; }
    }
}

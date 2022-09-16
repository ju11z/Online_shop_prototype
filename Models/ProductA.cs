using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Models
{
    public class ProductA
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public int ProductCategoryId { get; set; }
        //public virtual ProductCategory ProductCategory { get; set; }

        public string ProductImageName { get; set; }

        public int TotalAmount { get; set; }

    }
}

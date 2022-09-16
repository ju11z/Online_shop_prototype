using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Models
{
    public class ShopUser:IdentityUser
    {
        public virtual List<Order> Orders { get; set; }
    }
}

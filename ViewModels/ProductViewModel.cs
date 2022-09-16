using Microsoft.AspNetCore.Http;
using Shop_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.ViewModels
{
    public class ProductViewModel
    {
        private readonly IShopRepository shopRepository;
        public ProductViewModel()
        {

        }
        public ProductViewModel(
            IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }


        public int CategoryId { get; set; }

        public IFormFile Image { get; set; }
    }
}

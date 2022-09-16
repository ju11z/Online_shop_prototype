using Microsoft.AspNetCore.Mvc;
using Shop_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Controllers
{
    public class CatalogueController:Controller
    {
        
        private readonly IShopRepository shopRepository;
        public CatalogueController(IShopRepository shopRepository)
        {
            this.shopRepository = shopRepository;
        }
        [HttpGet]
        public IActionResult Products(int id)
        {
            var model = shopRepository.GetProductsByCategory(id);
            return View(model);
        }
        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            var model = shopRepository.GetProductById(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult ProductDetails(int id,string someValue="cat")
        {
            if (User.Identity.IsAuthenticated)
            {
                if (shopRepository.ProductIsAvailable(id))
                {
                    shopRepository.AddToShoppingCart(id);
                    return RedirectToAction("ShoppingCart", "Home");
                }
                ViewBag.ErrorMessage = "Sorry, this product in unavailable now.";
                

            }
            else
            {
                ViewBag.ErrorMessage = "You need to be logged in to add products to basket.";
                
            }
            var model = shopRepository.GetProductById(id);
            return View(model);

        }
       
    }
}

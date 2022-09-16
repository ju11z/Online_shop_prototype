using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop_1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IShopRepository shopRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,IShopRepository shopRepository)
        {
            _logger = logger;
            this.shopRepository = shopRepository;
           
        }

        public IActionResult Index()
        {
            
            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult ShoppingCart() {
            var model = shopRepository.GetUserShoppingCart();
            ViewBag.Products = shopRepository.GetAllProducts();
            ViewBag.OrderPrice = shopRepository.GetUserOrderPrice();
            return View(model);
        }

        public IActionResult RemoveFromShoppingCart(int id)
        {
            shopRepository.RemoveFromShoppingCart(id);
            return RedirectToAction("ShoppingCart");
        }

        public IActionResult IncreaseAmount(int id)
        {
            shopRepository.IncreaseProductAmount(id);
            return RedirectToAction("ShoppingCart");
        }

        public IActionResult DecreaseAmount(int id)
        {
            shopRepository.DecreaseProductAmount(id);
            return RedirectToAction("ShoppingCart");
        }

        [HttpPost]
        public IActionResult ShoppingCart(string someValue="") {
            shopRepository.OrderShoppingCart();
            return RedirectToAction("Index");
        }

    }
}

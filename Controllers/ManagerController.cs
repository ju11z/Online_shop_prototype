using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop_1.Models;
using Shop_1.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Controllers
{
    [Authorize(Roles="Manager")]
    public class ManagerController : Controller
    {
        private readonly IShopRepository shopRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public ManagerController(IShopRepository shopRepository,IHostingEnvironment hostingEnvironment,RoleManager<IdentityRole> roleManager)
        {
            this.shopRepository = shopRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.CategoriesList = new SelectList(shopRepository.GetAllCategories(),"Id","Name");
            return View();
        }
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            ViewBag.CategoriesList = new SelectList(shopRepository.GetAllCategories(), "Id", "Name");

            var product = shopRepository.GetProductById(id);
            var model = new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.ProductCategoryId,
                ExistingImagePath=product.ProductImageName

            };
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateProduct(ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = shopRepository.GetProductById(model.Id);
                product.Name = model.Name;
                product.Price = model.Price;
                product.ProductCategoryId = model.CategoryId;

                if (model.Image != null)
                {
                    if (model.ExistingImagePath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "assets/img/Products", model.ExistingImagePath);
                        System.IO.File.Delete(filePath);
                    }
                    product.ProductImageName = ProcessUploadedFile(model);
                }


                shopRepository.UpdateProduct(product);
                return RedirectToAction("ProductsFull");
            }
            return View();

        }

        [HttpPost]
        public  async Task<IActionResult>  AddProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Image != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "assets\\img\\Products");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Product product = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    ProductImageName = uniqueFileName,
                    ProductCategoryId = model.CategoryId
                };
                shopRepository.AddProduct(product);
                
            }
            
            return RedirectToAction("ProductsFull");
        }
        [HttpGet]
        public IActionResult UpdateStock(int id)
        {
            var model = shopRepository.GetStockById(id);
            ViewBag.ProductsList = new SelectList(shopRepository.GetAllProducts(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateStock(Stock model)
        {
            if (ModelState.IsValid)
            {
                Stock stock = shopRepository.GetStockById(model.StockId);
                stock.ProductId = model.ProductId;
                stock.Amount = model.Amount;
                stock.DateSupplied = model.DateSupplied;

                shopRepository.UpdateStock(stock);
                return RedirectToAction("Stocks");
            }
            
            return View();
        }

        private string ProcessUploadedFile(ProductViewModel model)
        {
            string uniqueFileName = null;
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "assets\\img\\Products");
                uniqueFileName = Guid.NewGuid().ToString() + " " + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(filestream);
                }

            }

            return uniqueFileName;
        }

        [HttpGet]
        public IActionResult ProductsFull()
        {
            var model = shopRepository.GetProductsWithAmount();
            return View(model);
        }

        [HttpGet]
        public IActionResult Stocks()
        {
            ViewBag.Products = shopRepository.GetAllProducts().ToList();
            var model = shopRepository.GetAllStocks();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddStock()
        {
            ViewBag.ProductsList = new SelectList(shopRepository.GetAllProducts(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult AddStock(Stock model)
        {
            shopRepository.AddStock(model);
            return RedirectToAction("Stocks");
        }
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            
            if (shopRepository.ProductIsDeletable(id))
            {
                shopRepository.DeleteProduct(id);
            }
            else
            {
                ViewBag.DeleteErrorMessage = "Sorry, you cant delete the product besauce it is in someone's shopping cart.";
            }
            return RedirectToAction("ProductsFull"); 
            
        }
        [HttpPost]
        public IActionResult DeleteStock(int id)
        {
            shopRepository.DeleteStock(id);
            return RedirectToAction("Stocks");
        }

        [HttpGet]
        public IActionResult Categories()
        {
            var model = shopRepository.GetAllCategories();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddCategory() {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(ProductCategory model)
        {
            shopRepository.AddCategory(model);
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            var model = shopRepository.GetCategoryById(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateCategory(ProductCategory model)
        {
           
            if (ModelState.IsValid)
            {
                ProductCategory category = shopRepository.GetCategoryById(model.Id);
                category.Name = model.Name;

                shopRepository.UpdateCategory(category);
                return RedirectToAction("Categories");
            }
            return View();
        }

        public IActionResult DeleteCategory(int id)
        {
            if (shopRepository.CategoryIsDeletable(id))
            {
                shopRepository.DeleteCategory(id);
            }
            return RedirectToAction("Categories");
        }


    }
}

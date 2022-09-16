
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Models
{
    public class MsSqlShopRepository : IShopRepository
    {
        private readonly AppDbContext context;
        private readonly UserManager<ShopUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        public MsSqlShopRepository(AppDbContext context, UserManager<ShopUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;

        }

        public void AddProduct(Product model)
        {
            this.context.Products.Add(model);
            this.context.SaveChanges();
        }

        public void AddStock(Stock model)
        {
            context.Stocks.Add(model);
            context.SaveChanges();
        }

        public void AddToShoppingCart(int id)
        {
            //var userId = "1e91d9ec-70f1-48f2-81a1-19361c62a009";//_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (context.Orders.Where(o => o.UserId == userId && o.ProductId == id&&o.IsFramed==false).Count() > 0)
            {
                var orderPrev = context.Orders.Where(o => o.UserId == userId && o.ProductId == id).FirstOrDefault();
                orderPrev.Amount++;
            }
            else
            {
                Order order = new Order
                {
                    UserId = userId,
                    ProductId = id,
                    IsFramed = false,
                    Amount = 1
                };
                context.Orders.Add(order);
            }
            
            context.SaveChanges();
        }

        public IEnumerable<ProductCategory> GetAllCategories()
        {
            return context.ProductCategories;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return context.Products;
        }

        public Product GetProductById(int id)
        {
            return context.Products.Where(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<ProductA> GetProductsWithAmount()
        {
            return context.ProductsA;
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return context.Stocks;
        }

        public IEnumerable<OrderV> GetUserShoppingCart(){
            
            context.SaveChanges();
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = context.OrdersV.Where(b => b.UserId == userId).ToList();
            return model;
        }

        public void OrderShoppingCart()
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            context.Orders.Where(o => o.UserId == userId).ToList().ForEach(o=>o.IsFramed=true);
            context.SaveChanges();
        }

        public void RemoveFromShoppingCart(int id)
        {
            var order = context.Orders.Where(o => o.Id == id).FirstOrDefault();
            context.Orders.Remove(order);
            context.SaveChanges();
        }

        public IEnumerable<Product> GetProductsByCategory(int id)
        {
            var model = context.Products.Where(p => p.ProductCategoryId == id).ToList();
            return model;
        }

        public void DecreaseProductAmount(int id)
        {
            var order = context.Orders.Where(o => o.Id == id).FirstOrDefault();
            if (order.Amount > 0)
            {
                order.Amount--;
                context.SaveChanges();
            }
            
        }

        public void IncreaseProductAmount(int id)
        {
            var order=context.Orders.Where(o=>o.Id==id).FirstOrDefault();
            order.Amount++;
            context.SaveChanges();
        }

        public bool ProductIsAvailable(int id)
        {
            var product = context.ProductsA.Where(p => p.Id == id).FirstOrDefault();
            if (product.TotalAmount > 0)
            {
                return true;
            }
            return false;
        }

        public Product UpdateProduct(Product model)
        {
            var product = context.Products.Attach(model);
            product.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return model;
        }

        public bool ProductIsDeletable(int id)
        {
            if (context.Orders.Where(o => o.ProductId == id).Count() + context.Stocks.Where(s => s.ProductId == id).Count() == 0) {
                return true;
            };
            return false;
        }

        public void DeleteStock(int id)
        {
            var stock = context.Stocks.Where(s => s.StockId == id).FirstOrDefault();
            context.Stocks.Remove(stock);
            context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product=context.Products.Where(p=>p.Id==id).FirstOrDefault();
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public Stock GetStockById(int id)
        {
            var stock = context.Stocks.Where(s => s.StockId == id).FirstOrDefault();
            return stock;
        }

        public Stock UpdateStock(Stock model)
        {
            var stock = context.Stocks.Attach(model);
            stock.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return model;
        }

        public void AddCategory(ProductCategory model) {
            context.ProductCategories.Add(model);
            context.SaveChanges();
        
        }

        public bool CategoryIsDeletable(int id)
        {
            if (context.Products.Where(p => p.ProductCategoryId == id).Count() + context.ProductsA.Where(p => p.ProductCategoryId == id).Count() == 0)
            {
                return true;
            }
            return false;
        }
        public void DeleteCategory(int id)
        {
            var category = context.ProductCategories.Where(c => c.Id == id).FirstOrDefault();
            context.ProductCategories.Remove(category);
            context.SaveChanges();
        }

        public ProductCategory UpdateCategory(ProductCategory model)
        {   
            var category = context.ProductCategories.Attach(model);
            category.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return model;
            
        }
        public IEnumerable<ProductCategory> GetCategories()
        {
            return context.ProductCategories;
        }

        public ProductCategory GetCategoryById(int id)
        {
            var model=context.ProductCategories.Where(p=>p.Id==id).FirstOrDefault();
            return model;
        }

        public int GetUserOrderPrice()
        {
            int price = 0;
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            /*
             var companies = people
                    .GroupBy(p=>p.Company)
                    .Select(g => new { Name = g.Key, Count = g.Count() });
             */
            price = context.OrdersV.Where(o=>o.UserId==userId&&o.IsFramed==false)
    .GroupBy(o => o.UserId)
    .Select(cl => new OrderPrice
    {
        Price = cl.Sum(c => c.TotalPrice),
    }).Select(o=>o.Price).FirstOrDefault();


            return price;
        }
        class OrderPrice
        {
            public int Price { get; set; }
        }
    }
}

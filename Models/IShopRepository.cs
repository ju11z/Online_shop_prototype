using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Models
{
    public interface IShopRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<ProductCategory> GetAllCategories();

        void AddProduct(Product model);

        Product GetProductById(int id);

        void AddToShoppingCart(int id);

        void RemoveFromShoppingCart(int id);

        public IEnumerable<ProductA> GetProductsWithAmount();

        public void AddStock(Stock model);
        public IEnumerable<Stock> GetAllStocks();

        public IEnumerable<Product> GetProductsByCategory(int id);

        public void DecreaseProductAmount(int id);

        public void IncreaseProductAmount(int id);

        void OrderShoppingCart();
        public IEnumerable<OrderV> GetUserShoppingCart();

        public bool ProductIsAvailable(int id);

        public bool ProductIsDeletable(int id);

        public void DeleteProduct(int id);

        public void DeleteStock(int id);

        public Stock GetStockById(int id);

        public Product UpdateProduct(Product model);

        public Stock UpdateStock(Stock model);

        public bool CategoryIsDeletable(int id);
        public ProductCategory UpdateCategory(ProductCategory category);

        public void DeleteCategory(int id);

        public IEnumerable<ProductCategory> GetCategories();

        public ProductCategory GetCategoryById(int id);

        public void AddCategory(ProductCategory category);

        public int GetUserOrderPrice();
    }
}

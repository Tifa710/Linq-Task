using Linq_Task.Data;
namespace Linq_Task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ApplicationDbContext _db = new();

            // 1. Customers' first & last names with email
            var q1 = _db.Customers
                .Select(c => new { c.FirstName, c.LastName, c.Email });

            // 2. Orders by specific staff
            var q2 = _db.Orders
                .Where(o => o.StaffId == 3);

            // 3. Products in category "Mountain Bikes"
            var q3 = from p in _db.Products
                     join c in _db.Categories on p.CategoryId equals c.CategoryId
                     where c.CategoryName == "Mountain Bikes"
                     select p;

            // 4. Total number of orders per store
            var q4 = _db.Orders
                .GroupBy(o => o.StoreId)
                .Select(g => new { StoreId = g.Key, TotalOrders = g.Count() });

            // 5. Orders not shipped yet
            var q5 = _db.Orders
                .Where(o => o.ShippedDate == null);

            // 6. Each customer's full name and number of orders
            var q6 = from c in _db.Customers
                     join o in _db.Orders on c.CustomerId equals o.CustomerId into g
                     select new
                     {
                         FullName = c.FirstName + " " + c.LastName,
                         OrdersCount = g.Count()
                     };

            // 7. Products never ordered
            var q7 = _db.Products
                .Where(p => !_db.OrderItems.Any(oi => oi.ProductId == p.ProductId));

            // 8. Products with quantity < 5
            var q8 = from s in _db.Stocks
                     join p in _db.Products on s.ProductId equals p.ProductId
                     where s.Quantity < 5
                     select new { p.ProductName, s.StoreId, s.Quantity };

            // 9. First product
            var q9 = _db.Products
                .OrderBy(p => p.ProductId)
                .FirstOrDefault();

            // 10. Products with model year 2022
            var q10 = _db.Products
                .Where(p => p.ModelYear == 2022);

            // 11. Each product with number of times ordered
            var q11 = from p in _db.Products
                      join oi in _db.OrderItems on p.ProductId equals oi.ProductId into g
                      select new
                      {
                          p.ProductName,
                          TimesOrdered = g.Count()
                      };

            // 12. Count products in category (id = 5)
            var q12 = _db.Products
                .Count(p => p.CategoryId == 5);

            // 13. Average list price
            var q13 = _db.Products
                .Average(p => p.ListPrice);

            // 14. Specific product by ID (10)
            var q14 = _db.Products
                .FirstOrDefault(p => p.ProductId == 10);

            // 15. Products ordered with quantity > 3
            var q15 = (from p in _db.Products
                       join oi in _db.OrderItems on p.ProductId equals oi.ProductId
                       where oi.Quantity > 3
                       select p).Distinct();

            // 16. Staff name and number of orders processed
            var q16 = from s in _db.Staffs
                      join o in _db.Orders on s.StaffId equals o.StaffId into g
                      select new
                      {
                          StaffName = s.FirstName + " " + s.LastName,
                          OrdersProcessed = g.Count()
                      };

            // 17. Active staff with phone
            var q17 = _db.Staffs
                .Where(s => s.Active == 1)
                .Select(s => new { s.FirstName, s.LastName, s.Phone });

            // 18. Products with brand and category
            var q18 = from p in _db.Products
                      join b in _db.Brands on p.BrandId equals b.BrandId
                      join c in _db.Categories on p.CategoryId equals c.CategoryId
                      select new
                      {
                          p.ProductName,
                          b.BrandName,
                          c.CategoryName
                      };

            // 19. Completed orders
            var q19 = _db.Orders
                .Where(o => o.OrderStatus == 4);

            // 20. Each product with total quantity sold
            var q20 = from p in _db.Products
                      join oi in _db.OrderItems on p.ProductId equals oi.ProductId
                      group oi by p.ProductName into g
                      select new
                      {
                          ProductName = g.Key,
                          TotalSold = g.Sum(x => x.Quantity)
                      };
            var result = q20.ToList();
            foreach (var item in result)
            {
                Console.WriteLine($"Product: {item.ProductName,-30} | Total Sold: {item.TotalSold}");
            }
        }
    }
}

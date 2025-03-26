using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StatsController : Controller
	{
        private ApplicationDbContext _db;

        public StatsController(ApplicationDbContext db)
        {
            _db = db;
        }
		public IActionResult Index()
		{
			return View();
		}

        [HttpPost]
		public async Task<IActionResult> ProductsInCategory()
		{
			var data = await _db.Product
				.GroupBy(p => p.Category.CategoryName)
				.Select(g => new
				{
                    label = g.Key,
                    value = g.Count()
				})
				.ToListAsync();

			return Json(data);
		}

        [HttpPost]
        public IActionResult ProductsInBrand()
        {
            var productsInBrand = _db.Product
                .GroupBy(p => p.SpecialTag.SpecialTagName)
                .Select(g => new
                {
                    label = g.Key,
                    value = g.Count()
                })
                .ToList();

            return Json(productsInBrand);
        }

        public IActionResult GetTotalAmountByCategory()
        {
            var result = _db.OrderDetails
                .Include(od => od.Product)
                .ThenInclude(p => p.Category)
                .GroupBy(od => od.Product.Category.CategoryName)
                .Select(g => new
                {
                    label = g.Key,
                    value = g.Sum(od => od.Quantity * od.UnitPrice)
                })
                .ToList();

            return Json(result);
        }

        public IActionResult GetTotalAmountByBrand()
        {
            var result = _db.OrderDetails
                .Include(od => od.Product)
                .ThenInclude(p => p.SpecialTag)
                .GroupBy(od => od.Product.SpecialTag.SpecialTagName)
                .Select(g => new
                {
                    label = g.Key,
                    value = g.Sum(od => od.Quantity * od.UnitPrice)
                })
                .ToList();

            return Json(result);
        }
    }
}

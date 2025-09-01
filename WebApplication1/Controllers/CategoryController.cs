using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Models.ViewModel;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoryproduct = await _context.Categories
                .Select(c => new CategoryDTO
                {
                    CategoryName = c.CategoryName
                }).ToListAsync();

            var products = await _context.Products
                .Select(c => new ProductDTO
                {
                    ProductName = c.ProductName,
                    Quantity = c.Quantity
                }).ToListAsync();

            var model = new CategoryProductVM
            {
                CategoryVMs = categoryproduct,
                ProductVMs = products
            };
            return View(model);
        }

        // Details
        [HttpGet]
        public async Task<IActionResult> GetSingle(string categoryname)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == categoryname);

            if (category == null)
                return NotFound();

            var model = new CategoryDTO
            {
                CategoryName = category.CategoryName
            };

            return View(model);
        }

        // Create (GET)
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        // Create (POST)
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
        {
            if (await CheckCategoryDuplicate(category.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Duplicated Category");
                return View(category);
            }

            var model = new Category
            {
                CategoryName = category.CategoryName
            };

            await _context.Categories.AddAsync(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Edit (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(string categoryname)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == categoryname);

            if (category == null)
                return NotFound();

            var model = new CategoryDTO
            {
                CategoryName = category.CategoryName
            };

            // We pass the old name through route values, not ViewBag
            return View(model);
        }

        // Edit (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(string oldCategoryName, CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return View(category);

            var existing = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == oldCategoryName);

            if (existing == null)
                return NotFound();

            existing.CategoryName = category.CategoryName;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Delete (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(string categoryname)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == categoryname);

            if (category == null)
                return NotFound();

            var model = new CategoryDTO
            {
                CategoryName = category.CategoryName
            };

            return View(model);
        }

        // Delete (POST)
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string categoryname)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == categoryname);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Duplicate check
        public async Task<bool> CheckCategoryDuplicate(string categoryname)
        {
            return await _context.Categories
                .AnyAsync(c => c.CategoryName == categoryname);
        }
    }
}

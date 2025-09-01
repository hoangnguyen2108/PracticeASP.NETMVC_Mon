using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoryproduct = await _context.Categories.Select(c => new CategoryDTO
            {
                CategoryName = c.CategoryName,
                
            } ).ToListAsync();

            var products = await _context.Products.Select(c => new ProductDTO
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
        // SIngle Category
        [HttpGet]
        public async Task<IActionResult> GetSingle(string categoryname)
        {
            var product = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryname);

            if (product == null)
            {
                return BadRequest("Not found");
            }

            var model = new CategoryDTO
            {
                CategoryName = product.CategoryName

            };
            
            return View(model);
        }

        // Create Category 
        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
        {
           
            var model = new Category
            {
                CategoryName = category.CategoryName
            };
            await _context.Categories.AddAsync(model);
            if(!await CheckCategoryDuplicate(model.CategoryName)){
                return BadRequest("Duplicated Category");
            }
            await _context.SaveChangesAsync();
        }


        public async Task<bool>  CheckCategoryDuplicate(string categoryname)
        {
            return  await _context.Categories.AnyAsync(c => c.CategoryName == categoryname);
        }
    }
}

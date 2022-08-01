using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;

namespace ShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 3;

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Products.Count() / pageSize);

            var products = await 
                _context.Products.
                OrderByDescending(p => p.Id).
                Include(p => p.Category).
                Skip((p - 1) * pageSize).
                Take(pageSize).
                ToListAsync();

            return View(products);

        }

        [HttpGet]
        public IActionResult Create(int p = 1)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name",product.CategoryId);

            if(ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                var IsAlreadyThere = _context.Products.Any(p => p.Slug == product.Slug);

                if(IsAlreadyThere)
                {
                    ModelState.AddModelError("", "Product already exists");
                    return View(product);
                }


                if(product.ImageUpload != null)
                {
                    
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir,imageName);

                    FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    await product.ImageUpload.CopyToAsync(stream);
                    stream.Close();

                    product.Image = imageName;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The product has been created";

                return RedirectToAction("Index");
            }

            return View(product);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var product = await _context.Products.FindAsync(Id);

            if(product == null) return View("Error");

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name",product.CategoryId);

            return View(product);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Product product)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                var IsAlreadyThere = _context.Products.Any(p => p.Id == product.Id);

                if (!IsAlreadyThere)
                {
                    return View("Error");
                }


                if (product.ImageUpload != null)
                {

                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    await product.ImageUpload.CopyToAsync(stream);
                    stream.Close();

                    product.Image = imageName;
                }

                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The product has been edited";

                return RedirectToAction("Index");

            }

            return View(product);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(long Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return View("Error");

            if (!product.Image.Equals("noimage.png"))
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string oldImagePath = Path.Combine(uploadDir, product.Image);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            } 


            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["Success"] = "The product has been deleted";

            return RedirectToAction("Index");

        }
    }
}

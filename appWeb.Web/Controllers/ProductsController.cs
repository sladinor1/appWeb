using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using appWeb.Common.Entities;
using appWeb.Web.Data;
using appWeb.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using SendGrid.Helpers.Mail;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Http;

namespace appWeb.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            List<ProductCatalogyViewModel> productcatalogyviewmodel = new List<ProductCatalogyViewModel>();
            foreach (var item in products)
            {
                var images = await _context.ProductImages.Where(p => p.ProductId == item.Id).ToListAsync();
                ICollection<ProductImage> list = new Collection<ProductImage>();
                foreach (var i in images)
                {
                    list.Add(i);
                }
                productcatalogyviewmodel.Add(new ProductCatalogyViewModel
                {
                    Id = item.Id,
                    Description = item.Description,
                    Tittle = item.Tittle,
                    Author = item.Author,
                    Price = item.Price,
                    Lot = item.Lot,
                    ProductImages = list
                }); }
            return View(productcatalogyviewmodel);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            var images = await _context.ProductImages.Where(p => p.ProductId == id).ToListAsync();
            ICollection<ProductImage> list = new Collection<ProductImage>();
            foreach (var i in images)
            {
                list.Add(i);
            }
      
            if (product == null)
            {
                return NotFound();
            }

            return View(new ProductCatalogyViewModel
            {
                Id = product.Id,
                Description = product.Description,
                Tittle = product.Tittle,
                Author = product.Author,
                Price = product.Price,
                Lot = product.Lot,
                ProductImages = list
            });
        }

        [Authorize(Roles = "User,Admin")]
        // GET: Products/Create
        public IActionResult Create()
        {
            ProductCatalogyViewModel productcatalogyviewmodel = new ProductCatalogyViewModel();
            List<CategoryViewModel> listcategory = new List<CategoryViewModel>();
            var category = _context.Categories;
            foreach (var item in category)
            {
                listcategory.Add(new CategoryViewModel() { Id = item.Id, Name = item.Name, Checked = false });
            }

            productcatalogyviewmodel.CategoryList = listcategory;

            return View(productcatalogyviewmodel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCatalogyViewModel productcatalogyviewmodel, IFormFile Pimage)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                product.Description = productcatalogyviewmodel.Description;
                product.Tittle = productcatalogyviewmodel.Tittle;
                product.Author = productcatalogyviewmodel.Author;
                product.Price = productcatalogyviewmodel.Price;
                product.Lot = productcatalogyviewmodel.Lot;
                string filePath = "";
                if (Pimage == null)
                {
                    return View(productcatalogyviewmodel);
                }
                if (Pimage != null)
                {
                    filePath = $"wwwroot/images/Products/{Pimage.FileName}";
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        Pimage.CopyTo(stream);
                    }
                }
                ProductImage img = new ProductImage { ImageFullPath = $"https://localhost:44371/images/Products/{Pimage.FileName}",
                    ProductId = product.Id };
                List<ProductImage> list = new List<ProductImage>();
                list.Add(img);
                product.ProductImages = list;
                _context.Add(product);
                foreach (var item in list)
                {
                    _context.Add(item);
                }
                await _context.SaveChangesAsync();
                if (productcatalogyviewmodel.CategoryList != null)
                {
                    foreach (var item in productcatalogyviewmodel.CategoryList)
                    {
                        CategoryProduct relation = new CategoryProduct();
                        if (item.Checked == true)
                        {
                            var pr = await _context.Products.FirstOrDefaultAsync(p => p.Tittle == product.Tittle);
                            var ct = await _context.Categories.FirstOrDefaultAsync(c => c.Name == item.Name);
                            relation.IdProduct = pr.Id;
                            relation.IdCategory = ct.Id;
                            _context.Add(relation);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productcatalogyviewmodel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Author,Tittle,Description,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var images = await _context.ProductImages.Where(p => p.ProductId == id).ToListAsync();
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            foreach (var item in images)
            {
                _context.ProductImages.Remove(item);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public ActionResult GetImg(string id)
        {
            var path = $@"C:/Users/Familia/ProjectImages/{id}.png";
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "image/png");
        }

        public async Task<IActionResult> Set_Publications()
        {
            var products = await _context.Products.Where(p => p.Author == User.Identity.Name).ToListAsync();
            List<ProductCatalogyViewModel> productcatalogyviewmodel = new List<ProductCatalogyViewModel>();
            foreach (var item in products)
            {
                var images = await _context.ProductImages.Where(p => p.ProductId == item.Id).ToListAsync();
                ICollection<ProductImage> list = new Collection<ProductImage>();
                foreach (var i in images)
                {
                    list.Add(i);
                }
                productcatalogyviewmodel.Add(new ProductCatalogyViewModel
                {
                    Id = item.Id,
                    Description = item.Description,
                    Tittle = item.Tittle,
                    Author = item.Author,
                    Price = item.Price,
                    Lot = item.Lot,
                    ProductImages = list
                });
            }
            return View(productcatalogyviewmodel);
        }

        public async Task<IActionResult> Chat()
        {
            return RedirectToAction("Chat","Account");
        }
    }
}

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Service.Interface;

namespace EShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        // GET: Products
        public IActionResult Index()
        {
            var allProducts = productService.GetAllProducts();
            return View(allProducts);
        }

        // GET: Products/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.Products
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var product = this.productService.GetDetailsForProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ProductName,ProductDescription,ProductImage,ProductPrice,ProductRating")] Product product)
        {
            if (ModelState.IsValid)
            {
                this.productService.CreateNewProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this.productService.GetDetailsForProduct(id);
            
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,ProductName,ProductDescription,ProductImage,ProductPrice,ProductRating")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.productService.UpdateExistingProduct(product);
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
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this.productService.GetDetailsForProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this.productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return this.productService.GetDetailsForProduct(id) != null;
        }

        public IActionResult AddProductToCart(Guid id)
        {
            var model = this.productService.GetShoppingCartInfo(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProductToCart(AddToShoppingCartDto model)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this.productService.AddToShoppingCart(model, userId);

            if (result)
            {
                return RedirectToAction("Index", "Products");
            }

            return View(model);
        }
    }
}

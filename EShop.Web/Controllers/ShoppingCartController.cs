using EShop.Web.Data;
using EShop.Web.Models.Domain;
using EShop.Web.Models.DTO;
using EShop.Web.Models.Identity;
using EShop.Web.Models.Relations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<EShopApplicationUser> _userManager;

        public ShoppingCartController(ApplicationDbContext context, UserManager<EShopApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users
                .Where(z => z.Id.Equals(userId))
                .Include(z => z.UserCart)
                .Include(z => z.UserCart.ProductInShoppingCart)
                .Include("UserCart.ProductInShoppingCart.Product")
                .FirstOrDefaultAsync();

            var userShoppingCart = loggedInUser.UserCart;

            var productPrice = userShoppingCart
                .ProductInShoppingCart
                .Select(z => new
                {
                    ProductPrice = z.Product.ProductPrice,
                    Quantity = z.Quantity
                })
                .ToList();


            double totalPrice = 0;

            foreach (var item in productPrice)
            {
                totalPrice += item.ProductPrice * item.Quantity;
            }



            // Select() LINQ izrazot vsushnost pravi SELECT query

            //var allProducts = userShoppingCart
            //    .ProductInShoppingCart
            //    .Select(z => z.Product)
            //    .ToList();

            ShoppingCartDto shoppingCartDtoItem = new ShoppingCartDto
            {
                Products = userShoppingCart.ProductInShoppingCart.ToList(),
                TotalPrice = totalPrice
            };

            return View(shoppingCartDtoItem);
        }

        public async Task<IActionResult> DeleteProductFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users
                .Where(z => z.Id.Equals(userId))
                .Include(z => z.UserCart)
                .Include(z => z.UserCart.ProductInShoppingCart)
                .Include("UserCart.ProductInShoppingCart.Product")
                .FirstOrDefaultAsync();

            var userShoppingCart = loggedInUser.UserCart;

            userShoppingCart
                .ProductInShoppingCart
                .Remove(userShoppingCart.ProductInShoppingCart.Where(z => z.ProductId.Equals(id))
                .FirstOrDefault());

            _context.Update(userShoppingCart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ShoppingCart");
        }

        public async Task<IActionResult> OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users
                .Where(z => z.Id.Equals(userId))
                .Include(z => z.UserCart)
                .Include(z => z.UserCart.ProductInShoppingCart)
                .Include("UserCart.ProductInShoppingCart.Product")
                .FirstOrDefaultAsync();

            var userShoppingCart = loggedInUser.UserCart;

            Order orderItem = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                User = loggedInUser
            };

            _context.Update(orderItem);
            await _context.SaveChangesAsync();

            List<ProductInOrder> productInOrders = new List<ProductInOrder>();

            productInOrders = userShoppingCart
                .ProductInShoppingCart
                .Select(z => new ProductInOrder
                {
                    OrderId = orderItem.Id,
                    ProductId = z.Product.Id,
                    SelectedProduct = z.Product,
                    UserOrder = orderItem
                })
                .ToList();

            foreach (var item in productInOrders)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
            }

            loggedInUser.UserCart.ProductInShoppingCart.Clear();

            _context.Update(loggedInUser);
            await _context.SaveChangesAsync();

            // Duri ne mora sekade da pravam SaveChangesAsync() mozam samo ednas
            // tuka na krajot od metodot da go povikam

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}

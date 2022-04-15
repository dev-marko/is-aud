using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Domain.Identity;
using EShop.Repository;
using EShop.Service.Interface;
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
        private readonly IShoppingCartService shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this.shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ShoppingCartDto shoppingCartDto = this.shoppingCartService.GetShoppingCartInfo(userId);

            return View(shoppingCartDto);
        }

        public IActionResult DeleteProductFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.shoppingCartService.DeleteProductFromShoppingCart(id, userId);

            return RedirectToAction("Index", "ShoppingCart");
        }

        public IActionResult OrderNow()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this.shoppingCartService.OrderNow(userId);

            if (result)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            else
            {
                // Nemame error page zatoa pak na istata si vrakjame
                return RedirectToAction("Index", "ShoppingCart");
            }

        }
    }
}

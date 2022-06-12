using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> shoppingCartRepository;
        private readonly IRepository<Order> orderRepository;
        private readonly IRepository<ProductInOrder> productInOrderRepository;
        private readonly IUserRepository userRepository;

        public ShoppingCartService(
            IRepository<ShoppingCart> shoppingCartRepository, 
            IRepository<Order> orderRepository, 
            IRepository<ProductInOrder> productInOrderRepository, 
            IUserRepository userRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.orderRepository = orderRepository;
            this.productInOrderRepository = productInOrderRepository;
            this.userRepository = userRepository;
        }

        public ShoppingCartDto GetShoppingCartInfo(string userId)
        {
            var loggedInUser = this.userRepository.Get(userId);

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

            ShoppingCartDto shoppingCartDtoItem = new ShoppingCartDto
            {
                Products = userShoppingCart.ProductInShoppingCart.ToList(),
                TotalPrice = totalPrice
            };

            return shoppingCartDtoItem;
        }

        public void DeleteProductFromShoppingCart(Guid id, string userId)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = this.userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart
                    .ProductInShoppingCart
                    .Where(z => z.ProductId.Equals(id))
                    .FirstOrDefault();

                userShoppingCart.ProductInShoppingCart.Remove(itemToDelete);

                this.shoppingCartRepository.Update(userShoppingCart);

            }
        }

        public bool OrderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = this.userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                Order orderItem = new Order
                {
                    UserId = userId,
                    User = loggedInUser
                };

                this.orderRepository.Insert(orderItem);

                List<ProductInOrder> productInOrders = new List<ProductInOrder>();

                productInOrders = userShoppingCart
                    .ProductInShoppingCart
                    .Select(z => new ProductInOrder
                    {
                        Id = Guid.NewGuid(),
                        OrderId = orderItem.Id,
                        ProductId = z.Product.Id,
                        SelectedProduct = z.Product,
                        UserOrder = orderItem
                    })
                    .ToList();

                foreach (var item in productInOrders)
                {
                    this.productInOrderRepository.Insert(item);
                }

                loggedInUser.UserCart.ProductInShoppingCart.Clear();

                this.userRepository.Update(loggedInUser);
                
                return true;
            }

            return false;
        }
    }
}

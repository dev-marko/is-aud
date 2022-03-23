using EShop.Domain.DomainModels;
using EShop.Domain.DomainModels.Relations;
using EShop.Domain.DTO;
using EShop.Repository.Interface;
using EShop.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EShop.Service.Implementation
{
    public class ProductService : IProductService
    {

        private readonly IRepository<Product> productRepository;
        private readonly IRepository<ProductInShoppingCart> productInShoppingCartRepository;
        private readonly IUserRepository userRepository;
        public ProductService
            (
            IRepository<Product> productRepository, 
            IUserRepository userRepository, 
            IRepository<ProductInShoppingCart> productInShoppingCartRepository
            )
        {
            this.productRepository = productRepository;
            this.userRepository = userRepository;
            this.productInShoppingCartRepository = productInShoppingCartRepository;
        }

        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {
            var user = this.userRepository.Get(userID);
            var userCart = user.UserCart;

            if (userCart != null && userCart != null)
            {
                var product = this.GetDetailsForProduct(item.SelectedProductId);

                if (product != null)
                {
                    var itemToAdd = new ProductInShoppingCart
                    {
                        ShoppingCart = userCart,
                        Product = product,
                        ProductId = product.Id,
                        ShoppingCartId = userCart.Id,
                        Quantity = item.Quantity
                    };

                    this.productInShoppingCartRepository.Insert(itemToAdd);
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateNewProduct(Product p)
        {
            this.productRepository.Insert(p);
        }

        public void DeleteProduct(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAllProducts()
        {
            return this.productRepository.GetAll().ToList();
        }

        public Product GetDetailsForProduct(Guid? id)
        {
            return this.productRepository.Get(id);
        }

        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var product = this.GetDetailsForProduct(id);

            var result = new AddToShoppingCartDto
            {
                SelectedProduct = product,
                SelectedProductId = product.Id,
                Quantity = 1
            };

            return result;
        }

        public void UpdateExistingProduct(Product p)
        {
            this.productRepository.Update(p);
        }
    }
}

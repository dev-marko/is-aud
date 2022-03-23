using EShop.Domain.DomainModels;
using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Service.Interface
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetDetailsForProduct(Guid? id);
        void CreateNewProduct(Product p);
        void UpdateExistingProduct(Product p);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        void DeleteProduct(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
    }
}

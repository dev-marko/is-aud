using EShop.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Service.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto GetShoppingCartInfo(string userId);
        void DeleteProductFromShoppingCart(Guid id, string userId);
        bool OrderNow(string userId);
    }
}

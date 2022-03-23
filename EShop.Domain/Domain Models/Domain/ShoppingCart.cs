using EShop.Domain.Domain_Models;
using EShop.Domain.DomainModels.Relations;
using EShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual EShopApplicationUser Owner { get; set; }
        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCart { get; set; }

    }
}

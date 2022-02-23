using EShop.Web.Models.Identity;
using EShop.Web.Models.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Web.Models.Domain
{
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        public virtual EShopApplicationUser Owner { get; set; }
        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCart { get; set; }

    }
}

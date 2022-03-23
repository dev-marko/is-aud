using EShop.Domain.Domain_Models;
using EShop.Domain.DomainModels.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.DomainModels
{
    public class Product : BaseEntity
    {
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        public double ProductRating { get; set; }
        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCart { get; set; }
        public virtual ICollection<ProductInOrder> Orders { get; set; }
    }
}

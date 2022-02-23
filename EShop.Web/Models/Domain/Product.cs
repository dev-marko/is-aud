using EShop.Web.Models.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Web.Models.Domain
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        public string ProductImage { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        public double ProductRating { get; set; }
        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCart { get; set; }
    }
}

﻿using EShop.Web.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Web.Models.DTO
{
    public class AddToShoppingCartDto
    {
        public Guid SelectedProductId { get; set; }
        public Product SelectedProduct { get; set; }
        public int Quantity { get; set; }
    }
}

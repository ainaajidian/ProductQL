using System;
using System.Collections.Generic;

#nullable disable

namespace ProductQL.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public double Price { get; set; }
        public DateTime Created { get; set; }
    }
}

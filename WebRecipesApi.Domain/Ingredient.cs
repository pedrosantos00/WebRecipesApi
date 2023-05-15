using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRecipesApi.Domain
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Quantity { get; set; }
        public string? QuantityType { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRecipesApi.Domain
{
    public class RateAudit
    {
        public int Id { get; set; }
        public int RatedBy { get; set; }
        public int RecipeId { get; set; }
    }

}

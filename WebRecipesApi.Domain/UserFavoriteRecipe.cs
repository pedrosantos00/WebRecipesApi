﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRecipesApi.Domain
{
    public class UserFavoriteRecipe
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int RecipeId { get; set; }
    }
}

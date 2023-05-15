using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRecipesApi.Domain
{
    public class Recipe
    {
        //INFO
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[]? Img { get; set; }
        public float? EstimatedTime { get; set; }
        public string? Difficulty { get; set; }
        public int? MealsPerRecipe { get; set; }
        public float? Rate { get; set; } = 0;
        public bool? Aprooved { get; set; } = false;
        public int? UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<Ingredient>? Ingredients { get; set; }
        public ICollection<Step>? Steps { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<User>? FavoritedBy { get; set; }
    }
}
